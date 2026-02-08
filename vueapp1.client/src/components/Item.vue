<template>
    <div id="item-container">
        <div id="information">
            <p class="position-label">{{ this.position }}</p>
            <div id="address">
                <p>{{ this.house }} {{ this.street }}</p>
                <p>{{ this.postcode }} {{ this.city }}</p>
            </div>
            <p class="distance" v-if="this.distance > 1">
             ({{ this.distance.toFixed(2) }} km)
            </p>
            <p class="distance" v-else>
             ({{ (this.distance * 1000).toFixed(0) }} m)
            </p>
        </div>
        <div id="button">
            <Button @click="changeInfoMachineStatus" buttonText="Learn More" />
            <Button @click="openItinerary" buttonText="Intineraire" />
        </div>
        <div id="details">
                <InfoMachine v-if="showInfoMachine" 
                :machine_id="this.id"
                :machine_params="this.params_info"
                :machine_position="this.position"
                :machine_address="this.house + ' ' + this.street"
                :machine_city="this.postcode + ' '+ this.city"
                @close-info-machine="changeInfoMachineStatus"
                />
        </div>
    </div>
</template>

<script>
import Button from '@/components/Button.vue';
import InfoMachine from '@/components/InfoMachine.vue'
export default{
    name: "Item",
    components:{
        Button,
        InfoMachine,
    },
    props:{
        id:{
            type: Number,
        },
        position:{
            type: Number,
        },
        name:{
            type: String,
            default: 'No Name'
        },
        house:{
        },
            type: String,
        street:{
            type: String,
        },
        postcode:{
            type: String,
        },
        city:{
            type: String,
        },
        address:{
            type: String,
        },
        distance:{
            type: Number,
            default: 0
        },
        lat: {
            type: Number,
        },
        lon: {
            type: Number,
        },
        params_info:{
            type: Object,
            default: () => ({})
        }
    },
    data(){
        return{
            showInfoMachine:false,
        }
    },
    methods:{
        changeInfoMachineStatus(){
            this.showInfoMachine = !this.showInfoMachine;
        },
        openItinerary(){
            const url = `https://www.google.com/maps/dir/?api=1&destination=${this.lat},${this.lon}&travelmode=driving&zoom=15`;
            window.open(url, '_blank');
        }
    }
}
</script>
<style scoped>
#item-container{
    display: flex;
    flex-direction: column;
    gap: 5px;
    border: 1px solid lightgray;
    padding : 10px;
}   
#information{
    display: flex;
    flex-direction: row;
    align-items: center;    /* This centers the children vertically */
}
.position-label {
  display: flex;
  align-items: center;
  justify-content: center;
  /* Ensure text is visible */
  color: white;
  font-weight: bold;
  font-size: 14px;
  /* If using a circle background */
  background-color: #42b883;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  min-width: 30px;  /* Extra safety */
  min-height: 30px; /* Extra safety */
  line-height: 30px;
  text-align: center;
  margin-right: 5px;
}
#address p {
  margin: 0;
}
.distance{
    margin-left: auto ;
}
#details{
    font-size: 14px;
    color: gray;
}
#button{
    display: flex;
    flex-direction: row;
    gap: 10px;
    align-items: center;
    justify-content: left;
}
</style>