<template>
    <div class="display-zone">
        <ItemList :items="itemList"/>
        <Map 
        :lat="lat" 
        :lon="lon" 
        :maxDistance="maxDistance"
        @init-result="updateItemList"
        @update-result="updateItemList"/>
    </div>
</template>
<script>
    import Map from '@/components/Map.vue';
    import ItemList from './ItemList.vue';
    export default {
        name: 'Displayzone',
        components: {
            Map,
            ItemList
        },
        props:{
            lat:{
                type:Number,
            },
            lon:{
                type:Number,
            },
            maxDistance:{
                type:Number,
            }
        },
        data(){
            return{
                itemList: [],
            }
        },
        methods:{
            updateItemList(data){
                this.itemList = data;
                console.log('Received data:', data);
            }
        }
    };

</script>
<style scoped>
.display-zone{
    display: flex;
    justify-content: center; /* Keeps items centered horizontally */
    align-items: flex-start; /* Aligns items to the top */
    width: 50%;
    --shared-height: 600px;
    gap: 10px;
}
</style>