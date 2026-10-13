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
#item-container {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  box-sizing: border-box;
  padding: 0.9rem 1rem;
  color: #000000;
  background: #ffffff;
  border: 1px solid #e5e5e5;
  border-radius: var(--radius, 12px);
  transition: border-color 0.15s ease, box-shadow 0.15s ease;
}

#item-container:hover {
  border-color: #000000;
  box-shadow: 0 6px 18px -8px rgba(0, 0, 0, 0.2);
}

#information {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 0.75rem;
  min-width: 0;
}

.position-label {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  width: 30px;
  height: 30px;
  font-size: 14px;
  font-weight: 700;
  color: #ffffff;
  background: #000000;
  border-radius: 50%;
}

#address {
  min-width: 0;
  overflow-wrap: anywhere;
}

#address p {
  margin: 0;
  line-height: 1.4;
}

.distance {
  margin-left: auto;
  flex-shrink: 0;
  font-size: 0.9rem;
  font-weight: 600;
  white-space: nowrap;
}

#details {
  font-size: 0.875rem;
  line-height: 1.5;
  color: #6b6b6b;
}

#button {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  align-items: center;
  justify-content: flex-start;
  gap: 0.5rem;
}

/* Phones: buttons share the row equally */
@media (max-width: 480px) {
  #item-container {
    padding: 0.8rem 0.9rem;
  }

  #button > * {
    flex: 1 1 140px;
  }
}
</style>