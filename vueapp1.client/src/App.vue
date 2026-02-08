<template>
  <Navbar />
  <div class = "app-container"> 
      <div class="info-alert">
        <span class="alert-icon">💡</span>
        <div class="alert-content">
          <p>
            Hello! This application helps you find nearby distributors. 
          </p>
          <p>
            The first version supports <strong>condom vending machines</strong> because it's the most important! 😉 
          </p>
          <p class="alert-footer">
            Feel free to test it out, give feedback, and suggest features! Your support is much appreciated.
          </p>
        </div>
      </div>
      <SearchZone @send-address="saveCurrentAddress" />
      <Displayzone :lat="lat" :lon="lon" :maxDistance="maxDistance"/>
  </div>
</template>

<script>
  import Navbar from '@/components/Navbar.vue';
  import SearchZone from '@/SearchZone.vue';
  import Displayzone from "@/Displayzone.vue";
  export default {
    name: 'App',
    components: {
      Navbar,
      SearchZone,
      Displayzone
    },
    data() {
      return {
        lat: 45.73,
        lon: 4.88,
        maxDistance: 3,
      };
    },
  methods: {
    saveCurrentAddress(payload) {
      if (!payload || payload.length !== 3) return;
      const [lat, lon, maxDistance] = payload;
      this.lat = lat,
      this.lon = lon,
      this.maxDistance = maxDistance;
    },
  }
};
</script>

<style scoped>
  .app-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 80vh;
    flex-direction: column;
    gap: 10px;
    min-height: 100vh; 
    /* Allow the body to handle the scroll */
    overflow-y: auto;
    padding-bottom: 40px;
  }
  .info-alert {
  display: flex;
  gap: 15px;
  background-color: #fff9db; /* Soft yellow */
  border: 1px solid #ffe066; /* Slightly darker border */
  border-left: 5px solid #fcc419; /* Strong accent line on the left */
  padding: 1.25rem;
  border-radius: 8px;
  max-width: 800px;
  margin-bottom: 20px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  line-height: 1.5;
  color: #856404; /* Dark brownish-yellow for readability */
}

.alert-icon {
  font-size: 1.5rem;
}

.alert-content p {
  margin: 0;
}

.alert-footer {
  margin-top: 8px !important;
  font-size: 1rem;
  font-style: italic;
  opacity: 0.9;
}
</style>
