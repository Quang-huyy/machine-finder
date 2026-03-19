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
  flex-direction: column;
  align-items: center;
  /* Remove fixed height: 80vh; */
  min-height: 100vh; 
  gap: 20px;
  padding: 20px; /* Add padding so it doesn't touch screen edges */
  box-sizing: border-box;
}

.info-alert {
  display: flex;
  gap: 15px;
  background-color: #fff9db;
  border: 1px solid #ffe066;
  border-left: 5px solid #fcc419;
  padding: 1.25rem;
  border-radius: 8px;
  
  /* 2. Responsive width */
  width: 100%; 
  max-width: 800px; 
  
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  line-height: 1.5;
  color: #856404;
  box-sizing: border-box;
}

/* 3. Handle very small screens (Stack the icon) */
@media (max-width: 480px) {
  .info-alert {
    flex-direction: column;
    align-items: flex-start;
    gap: 8px;
  }
  
  .app-container {
    padding: 10px; /* Tighter padding on mobile */
  }
}

.alert-icon {
  font-size: 1.5rem;
}

.alert-content p {
  margin-bottom: 8px; /* Add some breathing room between paragraphs */
}

.alert-content p:last-child {
  margin-bottom: 0;
}

.alert-footer {
  font-size: 0.9rem; /* Slightly smaller for mobile footer */
  font-style: italic;
  opacity: 0.9;
}
</style>
