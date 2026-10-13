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
  inset: 0;
  z-index: 2000; /* above the sticky navbar (1000) and the map */
  display: flex;
  justify-content: center;
  align-items: center;
  box-sizing: border-box;
  padding:
    max(1rem, env(safe-area-inset-top))
    max(1rem, env(safe-area-inset-right))
    max(1rem, env(safe-area-inset-bottom))
    max(1rem, env(safe-area-inset-left));
  background: rgba(0, 0, 0, 0.55);
  -webkit-backdrop-filter: blur(4px);
  backdrop-filter: blur(4px);
  overscroll-behavior: contain;
}

.modal-container {
  display: flex;
  flex-direction: column;
  width: 100%;
  max-width: 1000px;
  max-height: 100%;
  box-sizing: border-box;
  padding: clamp(1rem, 3vw, 1.5rem);
  color: #000000;
  background: #ffffff;
  border-radius: var(--radius, 16px);
  box-shadow: 0 25px 60px -15px rgba(0, 0, 0, 0.4);
}

.modal-header {
  flex-shrink: 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  padding-bottom: 0.9rem;
  font-size: 1.1rem;
  font-weight: 700;
  color: #000000;
  border-bottom: 1px solid #e5e5e5;
}

/* The body scrolls, so header and footer stay visible on small screens */
.modal-body {
  flex: 1 1 auto;
  min-height: 0;
  padding: 1.25rem 0 0.25rem;
  overflow-y: auto;
  -webkit-overflow-scrolling: touch;
}

/* ---------- Transition ---------- */
.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.25s ease;
}

.modal-fade-enter-active .modal-container,
.modal-fade-leave-active .modal-container {
  transition: transform 0.25s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
}

.modal-fade-enter-from .modal-container,
.modal-fade-leave-to .modal-container {
  transform: translateY(16px) scale(0.98);
}

@media (prefers-reduced-motion: reduce) {
  .modal-fade-enter-active,
  .modal-fade-leave-active,
  .modal-fade-enter-active .modal-container,
  .modal-fade-leave-active .modal-container {
    transition: none;
  }
}

/* ---------- Phones: bottom sheet ---------- */
@media (max-width: 600px) {
  .modal-overlay {
    align-items: flex-end;
    padding: 0;
  }

  .modal-container {
    max-height: 92dvh;
    padding-bottom: max(1rem, env(safe-area-inset-bottom));
    border-radius: 20px 20px 0 0;
  }

  .modal-fade-enter-from .modal-container,
  .modal-fade-leave-to .modal-container {
    transform: translateY(100%);
  }
}
</style>