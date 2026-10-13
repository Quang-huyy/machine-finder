<template>
    <div class="filter-container">
        <select
        id="max-distance-dropdown" 
        v-model="selectedMaxDistance"
        class="custom-select"
        @change="sendSelectedMaxDistance">
        <option v-for="distance in maxDistances"
        :key="distance"
        :value="distance">
        {{ distance }} km
        </option>
    </select>
    </div>
</template>

<script>
export default{
    name: "DropdownMaxDistance",
    props:{
        maxDistances:{
            type: Array,
            default: () => ([1,3,5,10])
        }
    },
    data(){
        return{
            selectedMaxDistance: 3,
        }
    },
    methods:{
        sendSelectedMaxDistance(){
            this.$emit('update-max-distance', this.selectedMaxDistance);
        }
    },  
}
</script><style scoped>
.filter-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  min-width: 0;
}

.custom-select {
  -webkit-appearance: none;
  appearance: none;
  width: 100%;
  height: var(--control-height, 48px);
  min-height: var(--control-height, 48px);
  box-sizing: border-box;
  padding: 0 2.5rem 0 1rem;
  font-family: inherit;
  font-size: 16px; /* 16px+ prevents iOS zoom on focus */
  line-height: 1.2;
  color: #000000;
  background-color: #ffffff;
  border: 2px solid #000000;
  border-radius: var(--radius, 12px);
  cursor: pointer;
  outline: none;
  transition: box-shadow 0.15s ease;

  /* Custom black arrow */
  background-image: url("data:image/svg+xml;charset=UTF-8,%3Csvg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24'%3E%3Cpath fill='%23000' d='M7 10l5 5 5-5z'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 0.85rem center;
  background-size: 1.25rem;
}

.custom-select:focus {
  box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.15);
}

.custom-select:disabled {
  color: #9a9a9a;
  background-color: #f0f0f0;
  cursor: not-allowed;
}
</style>