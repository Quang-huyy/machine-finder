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
</script>
<style scoped>
.filter-container {
  display: flex;
  flex-direction: column;
}


.custom-select {
  appearance: none; /* Removes the default browser arrow in some browsers */
  background-color: white;
  border: 2px solid black;
  border-radius: 5px;
  padding: 10px 15px;
  font-size: 1rem;
  color: #2c3e50;
  cursor: pointer;
  outline: none;
  transition: border-color 0.2s;
  
  /* Adding a custom arrow */
  background-image: url("data:image/svg+xml;charset=US-ASCII,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2224%22%20height%3D%2224%22%20viewBox%3D%220%200%2024%2024%22%3E%3Cpath%20fill%3D%22%23666%22%20d%3D%22M7%2010l5%205%205-5z%22%2F%3E%3C%2Fsvg%3E");
  background-repeat: no-repeat;
  background-position: right 10px top 50%;
  padding-right: 30px;
}

.custom-select:focus {
  border-color: #42b883; /* Vue Green */
}
</style>