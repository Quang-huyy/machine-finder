<template>
    <Transition name="modal-fade">
        <div class ="modal-overlay" @click.self="closeModal">
            <div class="modal-container">
                <header class="modal-header">
                    <slot name="header">{{ modalName }}</slot>
                    <Button :buttonText="closeButton" @click="closeModal"/>
                </header>
                <section class="modal-body">
                    <slot>Modify Current Information</slot>
                    <SuggestionModule 
                    :machine_id="machine_id"
                    :machine_params="machine_params" />
                </section>
                <!-- <footer class="modal-footer">
                    <slot name="footer">
                        <Button :buttonText="addButton" />
                    </slot>
                </footer> -->
            </div>
        </div>
    </Transition>
</template>

<script>
import Button from '@/components/Button.vue'
import SuggestionModule from './SuggestionModule.vue';
export default {
    name:"SuggestionModal",
    components:{
        Button,
        SuggestionModule
    },
    props:{
        machine_id:{
            type:Number
        },
        machine_params:{
            type:Object,
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
            modalName: 'Machine id ' + this.machine_position + ': ' + this.machine_address + ', ' + this.machine_city,
            closeButton: 'Close',
            addButton: 'Add new parameter',
            suggestionChangeList: []
        };
    },
    methods:{
        closeModal(){
            this.$emit('close-modal');
        },
    }

}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0; left: 0;
  width: 100vw; 
  height: 100vh;
  background: rgba(0, 0, 0, 0.6); /* Dim the background */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 2000; /* Ensure it's above the map */
}

.modal-container {
  background: white;
  padding: 24px;
  border-radius: 12px;
  width: 90%;
  max-width: 1000px;
  box-shadow: 0 10px 25px rgba(0,0,0,0.2);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #eee;
  padding-bottom: 12px;
  font-weight: bold;
  color: #35495e; /* Navy tech color */
}

.modal-body {
  padding: 20px 0;
}

/* Vue Transition Logic */
.modal-fade-enter-active, .modal-fade-leave-active {
  transition: opacity 0.3s ease;
}
.modal-fade-enter-from, .modal-fade-leave-to {
  opacity: 0;
}
</style>