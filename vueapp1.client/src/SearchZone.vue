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
                const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Address/addressToCoordinates`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ address: address })
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
.search-bar{
    display: flex;
    gap: 3px;
}
.search-input{
    outline: none;
    width: 500px;
    font-size: 16px;
    border-radius: 5px;
    border: 2px solid black;
    padding: 10px 15px;
}
.search-input:focus {
    border-color: #42b883;
}
#localise-me{
    background-color: #fcc419;
}
#localise-me:hover{
    background-color: #7c600b;
}
</style>