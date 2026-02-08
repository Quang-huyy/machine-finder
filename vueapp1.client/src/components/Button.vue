<template>
    <button 
    class="btn">
    {{ buttonText }}
    </button>
</template>

<script>
export default {
    name: 'Button',
    components: {},
    props:{
        buttonText:{
            type:String,
            default:'Click Me'
        }
    },
}
</script>
<style scoped>
.btn {
  padding: 10px 20px;
  background-color: #42b883; /* Vue Green */
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.btn:hover {
  background-color: #35495e;
}
.btn:disabled{
    background-color: grey;
}
</style>