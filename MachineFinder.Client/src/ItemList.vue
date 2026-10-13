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
/* ---------- Panel ---------- */
.item {
  display: flex;
  flex-direction: column;
  align-items: stretch;
  justify-content: flex-start; /* list starts at the top instead of floating mid-panel */
  width: 100%; /* was 50%; Displayzone's flex rule now sizes the panel */
  min-width: 0;
  min-height: 0;
  padding: clamp(0.75rem, 2.5vw, 1.25rem);
  box-sizing: border-box;
  color: #000000;
}

/* ---------- Scrollable list ---------- */
.item-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;

  flex: 1 1 auto;
  min-height: 0; /* lets the list scroll inside the flex panel */
  overflow-y: auto;
  overflow-x: hidden;
  overscroll-behavior: contain; /* scrolling the list doesn't scroll the page behind it */
  scroll-behavior: smooth;
  -webkit-overflow-scrolling: touch;

  /* Slim, subtle scrollbar */
  scrollbar-width: thin;
  scrollbar-color: #c9c9c9 transparent;
}

.item-list::-webkit-scrollbar {
  width: 6px;
}
.item-list::-webkit-scrollbar-thumb {
  background: #c9c9c9;
  border-radius: 999px;
}

.item-list li {
  padding-right: 8px; /* keeps text away from the scrollbar */
  min-width: 0;
  overflow-wrap: anywhere; /* long names or addresses can't push the layout sideways */
}

/* ---------- Empty state ---------- */
.no-item-notif {
  width: 100%;
  max-width: 400px;
  margin: auto; /* centers it inside the panel, both ways */
  padding: clamp(1.5rem, 6vw, 2.5rem) clamp(1rem, 5vw, 2rem);
  box-sizing: border-box;

  background-color: #fafafa;
  color: #666666; /* darker than #888 for better readability */
  font-size: 1rem;
  font-weight: 500;
  line-height: 1.5;
  text-align: center;

  border: 2px dashed #d4d4d4;
  border-radius: 14px;
}

.no-item-notif::after {
  content: "🥀";
  display: block; /* puts the emoji on its own line under the text */
  margin-top: 0.75rem;
  font-size: 1.5rem;
  opacity: 0.6;
}

/* ---------- Tablets and phones (panels are stacked) ---------- */
@media (max-width: 768px) {
  .item-list {
    max-height: 70dvh; /* the panel height is auto here, so cap the list */
  }
}

@media (prefers-reduced-motion: reduce) {
  .item-list {
    scroll-behavior: auto;
  }
}
</style>
