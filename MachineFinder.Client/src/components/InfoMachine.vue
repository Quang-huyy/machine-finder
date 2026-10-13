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
  justify-content: space-between;
  align-items: baseline;
  gap: 1rem;
  padding: 0.6rem 0;
  font-size: 0.95rem;
  line-height: 1.4;
  border-bottom: 1px solid #eeeeee;
}

.detail-item:last-child {
  border-bottom: none;
}

dt {
  flex-shrink: 0;
  font-weight: 600;
  color: #4b4b4b;
}

dd {
  margin: 0;
  min-width: 0;
  color: #000000;
  text-align: right;
  overflow-wrap: anywhere;
}

/* Very narrow phones: stack label above value */
@media (max-width: 380px) {
  .detail-item {
    flex-direction: column;
    gap: 0.15rem;
  }

  dd {
    text-align: left;
  }
}
</style>