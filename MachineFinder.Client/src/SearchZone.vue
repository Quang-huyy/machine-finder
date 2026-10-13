<template>
    <div class="search-bar">
        <input 
        type="text" 
        class="search-input" 
        v-model="address"
        placeholder="Enter your address"
        />
        <DropdownMaxDistance
        @update-max-distance="updateMaxDistance" />
        <Button 
        :buttonText="searchButton"
        @click="onSendAddress"
        />
        <Button
        id="localise-me"
        :button-text="LocaliseMe"
        @click="findMyposition"
        />
    </div>
</template>

<script>
    import Button from '@/components/Button.vue';
    import DropdownMaxDistance from './components/DropdownMaxDistance.vue';
    export default {
        name: "SearchZone",
        components:{
            Button,
            DropdownMaxDistance
        },
        data(){
            return {
                address: '',
                searchButton: 'Search',
                LocaliseMe: 'Localise Me',
                maxDistance: 3,
            };
        },
        methods:{
            async onSendAddress(){
                try{
                    const [lat, lon] = await this.addressToCoordinates(this.address);
                    this.$emit('send-address', [lat, lon, this.maxDistance]);
                }
                catch(error){
                    console.error('Error converting address to coordinates:', error);
                }
            },
            async addressToCoordinates(address){
                const url = `${import.meta.env.VITE_API_URL}/api/Address/addressToCoordinates?address=${encodeURIComponent(address)}`;

                const response = await fetch(url, {    
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                });
                const data = await response.json();
                return [data.lat, data.lon];
            },
            updateMaxDistance(newDistance){
                this.maxDistance = newDistance;
            }, 
            async findMyposition(){
                if (!navigator.geolocation){
                    alert("Geo location is not supported")
                }
                else{
                    navigator.geolocation.getCurrentPosition(
                        (position) =>{
                            this.lat = position.coords.latitude;
                            this.lon = position.coords.longitude;
                            this.$emit('send-address', [this.lat, this.lon, this.maxDistance]);
                        },
                        (error) =>{
                            alert("Could not get your location. Please check your permission")
                        }
                    )

                }
            }
        }
    };
</script>
<style scoped>
.search-bar {
  --control-height: 48px;
  --radius: 12px;
  --border: #000000;

  display: flex;
  align-items: stretch;
  justify-content: center;
  gap: 0.5rem;
  width: 100%;
  min-width: 0;
  box-sizing: border-box;
}

/* Every control shares the same height, box model and font */
.search-bar > *,
.search-input,
.search-bar :where(button, input, select) {
  height: var(--control-height);
  min-height: var(--control-height);
  box-sizing: border-box;
  margin: 0;
  border-radius: var(--radius);
  font-family: inherit;
  font-size: 16px; /* 16px+ stops iOS Safari from zooming on focus */
  line-height: 1.2;
}

/* ---------- Input ---------- */
.search-input {
  flex: 0 1 500px;
  min-width: 0; /* lets the input shrink instead of overflowing */
  width: 100%;
  max-width: 500px;
  padding: 0 1rem;
  color: #000000;
  background: #ffffff;
  border: 2px solid var(--border);
  outline: none;
  transition: box-shadow 0.15s ease;
}

.search-input::placeholder {
  color: #7a7a7a;
}

.search-input:focus {
  box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.15);
}

/* ---------- Buttons ---------- */
.search-bar :where(button) {
  flex: 0 0 auto;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.4rem;
  padding: 0 1.1rem;
  color: #ffffff;
  background: #000000;
  border: 2px solid #000000;
  font-weight: 600;
  white-space: nowrap;
  cursor: pointer;
  transition: background-color 0.15s ease, color 0.15s ease,
              transform 0.1s ease;
}

.search-bar :where(button):hover {
  background: #333333;
  border-color: #333333;
}

.search-bar :where(button):active {
  transform: scale(0.97);
}

.search-bar :where(button):focus-visible {
  outline: 3px solid rgba(0, 0, 0, 0.35);
  outline-offset: 2px;
}

/* Secondary button: outlined instead of filled */
#localise-me {
  color: #000000;
  background: #ffffff;
  border-color: #000000;
}

#localise-me:hover {
  color: #ffffff;
  background: #000000;
  border-color: #000000;
}

/* ---------- Tablets and phones ---------- */
@media (max-width: 600px) {
  .search-bar {
    --control-height: 52px; /* slightly taller touch targets */
    flex-wrap: wrap;
  }

  /* Input takes its own row */
  .search-input {
    flex: 1 1 100%;
    max-width: none;
  }

  /* Buttons share the next row equally */
  .search-bar :where(button) {
    flex: 1 1 0;
  }
}
@media (min-width: 769px) {
  .search-input,
  .custom-select,
  .btn,
  .search-bar :where(button) {
    font-size: 14.5px;
  }
}

</style>