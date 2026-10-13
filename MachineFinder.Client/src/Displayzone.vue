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
            }
        }
    };

</script>
<style scoped>
.display-zone {
  --shared-height: clamp(480px, calc(78dvh / 0.8), 760px);
  --radius: 16px;
  --border: #e5e5e5;
  --shadow: 0 10px 30px -10px rgba(0, 0, 0, 0.12),
            0 2px 6px rgba(0, 0, 0, 0.05);

  display: flex;
  flex-direction: row;
  align-items: stretch;
  gap: clamp(0.75rem, 2vw, 1rem);
  width: 100%;
  min-width: 0;
  box-sizing: border-box;
  color: #000000;
}

.display-zone > * {
  min-width: 0;
  height: var(--shared-height);
  box-sizing: border-box;
  overflow: auto;
  background: #ffffff;
  border: 1px solid var(--border);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  -webkit-overflow-scrolling: touch;
}

/* 2 : 3 ratio = 40% list, 60% map (of the space left after the gap) */
.display-zone > :first-child { flex: 2 1 0; }
.display-zone > :last-child  { flex: 3 1 0; }

/* ---------- Tablets and phones: stack the panels ---------- */
@media (max-width: 768px) {
  .display-zone {
    flex-direction: column;
    --shared-height: auto;
  }

  .display-zone > :first-child,
  .display-zone > :last-child {
    flex: none;
    width: 100%;
    height: auto;
    min-height: 320px;
  }

  /* On phones, keep the list from getting endlessly long */
  .display-zone > :first-child {
    max-height: 60dvh;
  }
}

@media (max-width: 480px) {
  .display-zone {
    --radius: 14px;
  }

  .display-zone > * {
    min-height: 280px;
  }
}
</style>