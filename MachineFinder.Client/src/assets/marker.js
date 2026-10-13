import L from "leaflet";

export const redIcon = L.icon({
  iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-red.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
  iconSize: [35, 56],
  iconAnchor: [17, 56],
  popupAnchor: [1, -34],
  shadowSize: [55, 55]
});

export const blueIcon = L.icon({
  iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-blue.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41]
});

export const iconWithNumber = (number) => {
  return L.divIcon({
          className: 'icon-with-number',
          // Use the same image URL as your blueIcon
          html: `
            <img src="https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-blue.png" class="marker-img">
            <span class="marker-number">${number}</span>
          `,
          iconSize: [25, 41],    // Match standard blueIcon size
          iconAnchor: [12, 41],  // Match standard blueIcon anchor
          popupAnchor: [1, -34]  // Match standard blueIcon popup anchor
        });
}