import json
import pyodbc
from geopy.extra.rate_limiter import RateLimiter
from geopy.geocoders import Nominatim
from geopy.exc import GeocoderTimedOut, GeocoderServiceError
JSON_FILE = "vendingmachineworld.json"
CONNECTION_STRING = ("Driver={ODBC Driver 18 for SQL Server};"
                    "Server=tcp:condom.database.windows.net,1433;"
                    "Database=condom-vend;"
                    "Uid=condomadmin;"
                    "Pwd=Quanghuy281099!;"
                    "Encrypt=yes;"
                    "TrustServerCertificate=no;"
                    "Connection Timeout=30;")
KEY_TAG_LIST = ["check_date", "survey:date ", "indoor", "operator", "level", "brand", "payment:coins", "payment:credit_cards", "payment:debit_cards", "payment:cash", "fee", "charge", "disused", "opening_hours"]

def main():
    with open(JSON_FILE, 'r') as file:
        data = json.load(file)

    elements = data.get("elements", [])
    conn = pyodbc.connect(CONNECTION_STRING)
    cursor = conn.cursor()
    geolocator = Nominatim(user_agent="my_machine_locator_app_v1", timeout=10)
    reverse_with_retry = RateLimiter(geolocator.reverse, min_delay_seconds=1.1, max_retries=3, error_wait_seconds=2)
    for el in elements:
        try:
            id = el.get("id")
            lat = el.get("lat")
            lon = el.get("lon")
            if id is None or lat is None or lon is None:
                continue
            else:
                print(id, lat, lon)
                lat_float = float(lat)
                lon_float = float(lon)
                house, street, city, postcode, full_address = "", "", "", "", ""
                location = geolocator.reverse((lat_float, lon_float), language='en')
                if location and 'address' in location.raw:
                    addr = location.raw['address']
                    house = addr.get('house_number', '')
                    street = addr.get('road', '')
                    city = addr.get('city', addr.get('town', addr.get('village', '')))
                    postcode = addr.get('postcode', '')
                    full_address = f"{street}, {city}, {postcode}".strip(', ') 
                tags = el.get("tags", {})
        except (GeocoderTimedOut, GeocoderServiceError):
            print(f"Skipping ID {id} after multiple retries.")
            continue # Skip to the next machine instead of crashing
        except (ValueError, TypeError) as e:
            print(f"Skipping ID {el.get('id')}: Invalid coordinate format ({lat}, {lon})")
            continue
        cursor.execute("""
            MERGE INTO Machine AS target
            USING (SELECT ? AS id, ? AS lat, ? AS lon, ? AS house, ? AS street, ? AS city, ? AS postcode, ? AS full_address) AS source
            ON target.id = source.id
            WHEN MATCHED THEN
                UPDATE SET lat = source.lat, 
                       lon = source.lon, 
                       house = source.house, 
                       street = source.street, 
                       city = source.city,
                       postcode = source.postcode, 
                       full_address = source.full_address
            WHEN NOT MATCHED THEN
                INSERT (id, lat, lon, house, street, city, postcode, full_address) 
            VALUES (source.id, source.lat, source.lon, source.house, 
                    source.street, source.city, source.postcode, source.full_address);
        """, (id, lat, lon, house, street, city, postcode, full_address))
        for key, value in tags.items():
            if key in KEY_TAG_LIST:
                if ":" in key:
                    collon_index = key.index(":")
                    key = key[collon_index+1:]
                cursor.execute("""
                    MERGE INTO MachineParams AS target
                    USING (SELECT ? AS machineID, ? as TagKey, ? AS TagValue) AS SOURCE
                    ON target.machineId = source.machineID AND target.TagKey = source.TagKey
                    WHEN MATCHED THEN
                        UPDATE SET TagValue = source.TagValue
                    WHEN NOT MATCHED THEN
                        INSERT (machineID, TagKey, TagValue) VALUES (source.machineID, source.TagKey, source.TagValue);""",
                    id, key, value)
    conn.commit()
    cursor.close()
    conn.close()
    print("Data insertion completed.")
if __name__ == "__main__":
    main()