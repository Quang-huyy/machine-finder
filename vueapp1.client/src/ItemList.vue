<template>
  <div class="item">
    <ul class="item-list" v-if="itemsListNotEmpty">
      <li v-for="item in items" :key="item.id">
        <Item 
          :id="item.machineID"
          :position="item.position"
          :distance="item.distance" 
          :address="item.full_address"
          :house="item.house"
          :street="item.street"
          :postcode="item.postcode"
          :city="item.city"
          :params_info="item.params_info"
          :lat="item.lat"
          :lon="item.lon"
        />
      </li>
    </ul>
    <p class="no-item-notif" v-else>No machine nearby </p>
  </div>
</template>

<script>

import Item from "@/components/Item.vue";

export default {
  components: { Item },
  props: {
    items: {
      type: Array,
      default: () => []
    },
  },
  computed: {
    itemsListNotEmpty() {
      return this.items && this.items.length > 0;
    }
  }
};
</script>
<style scoped>
.item{
  width: 50%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  
}
.item-list {
  list-style: none;
  padding: 0;
  margin: 0;
  /* Vertical Scroll Settings */
  max-height: var(--shared-height);      /* Set a limit so it doesn't grow forever */
  overflow-y: auto;       /* Adds a scrollbar only when content exceeds height */
  overflow-x: hidden;     /* Prevents accidental horizontal shifting */
  
  /* Smooth scrolling feel */
  scroll-behavior: smooth;
}

.item-list li {
  margin-bottom: 12px;
  padding-right: 8px;     /* Space so text doesn't touch the scrollbar */
}
.no-item-notif {
  padding: 40px 100px;
  margin: 0px auto;
  max-width: 400px;
  
  background-color: #f9f9f9;
  color: #888;
  font-size: 1rem;
  font-weight: 500;
  
  border: 2px dashed #e0e0e0;
  border-radius: 12px;
  text-align: center;
}
.no-item-notif::after {
  content: '🥀'; /* Or any emoji/icon */
  font-size: 1.5rem;
  margin-bottom: 12px;
  opacity: 0.6;
}
</style>
