<template>
    <div id="info-machine-container">
            <h3>Detailed Information</h3>
            <dl class="details-list">
                <template v-for="(value, key) in machine_params" :key="key">
                    <div v-if="value !== null" class="detail-item">
                        <dt>{{ formatFirstLetterUppercase(key) }} </dt>
                        <dd>{{ formatFirstLetterUppercase(value) }}</dd>
                    </div>
                </template>
                <dt v-if="!machineParamsHasNoInfos">There is no additional informations</dt>
            </dl>
            <Button @click="changeModalViewStatus" buttonText="Suggest Edits" />
            <SuggestionModal 
            v-if="showModalView"
            :machine_id="machine_id"
            :machine_position="machine_position"
            :machine_address="machine_address"
            :machine_city="machine_city"
            :machine_params ="machine_params"
            @close-modal="changeModalViewStatus" />
    </div>
</template>

<script>
import Button from '@/components/Button.vue';
import SuggestionModal from '@/components/SuggestionModal.vue';

export default {
    name: "InfoMachine",
    components:{
        Button,
        SuggestionModal,
    },
    props:{ 
        machine_id:{
            type:Number
        },
        machine_params:{
            type: Object,
            default: () => ({})
        },
        machine_position:{
            type:Number
        },
        machine_address:{
            type:String
        },
        machine_city:{
            type: String
        }
    },
    data(){
        return{
            showModalView: false,
        }
    },
    computed:{
        machineParamsHasNoInfos(){
            return Object.values(this.machine_params).some(value => value !== null);
        }
    },
    methods:{
        formatFirstLetterUppercase(value){
            return value.charAt(0).toUpperCase() + value.slice(1).replace(/_/g, ' ');
        },
        changeModalViewStatus(){
            this.showModalView = !this.showModalView
        }
    }
}
</script>

<style scoped>
.detail-item {
  display: flex;
  justify-content: space-between; /* Pushes label to left, value to right */
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}
dt { font-weight: bold; color: #555; }
dd { margin: 0; color: #333; }
</style>