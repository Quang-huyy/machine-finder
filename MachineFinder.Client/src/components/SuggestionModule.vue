<template>
    <div class="suggestion-module">
        <SuggestionCurrentModule 
        :machine_id="machine_id"
        :filteredParams="filteredParams"/>
    </div>
</template>

<script>
import Button from '@/components/Button.vue'
import SuggestionCurrentModule from '@/components/SuggestionCurrentModule.vue'
export default {
    name:'SuggestionModule',
    components:{
        Button,
        SuggestionCurrentModule
    },
    props:{
        machine_id:{
            type:Number
        },
        machine_params:{
            type:Object,
            default: ()=>({})
        }
    },
    data(){
        return {
            submitButtonDisabled: true
        }
    },
    watch:{
        selectedKey(){
            this.isSubmitButtonDisabled()
        },
        suggestedValue(){
            this.isSubmitButtonDisabled()
        }
    },
    computed:{
        filteredParams(){
            return Object.entries(this.machine_params).filter(([key,value]) =>value!=null);
        },
    },
    methods:{
        
    }
}
</script>
<style scoped>
</style>