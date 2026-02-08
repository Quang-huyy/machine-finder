<template>
  <nav class="navbar">
    <div class="container-fluid">
      <div class="brand-section">
        <div class="logo-dot"></div>
        <a class="navbar-brand" href="#">Machine<span>Finder</span></a>
      </div>
    </div>
  </nav>
</template>

<script>
export default {
    name: 'Navbar',
    components: {
    },
};
</script>

<style scoped>
.navbar {
  display: flex;
  align-items: center;
  /* Glassmorphism effect */
  background-color: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(10px);
  
  padding: 0.75rem 1.5rem;
  border-bottom: 1px solid rgba(0, 0, 0, 0.05);
  position: sticky;
  top: 0;
  z-index: 1000;
}

.container-fluid {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
}

/* Brand Section */
.brand-section {
  display: flex;
  align-items: center;
  gap: 10px;
}

.logo-dot {
  width: 12px;
  height: 12px;
  background-color: #42b883; /* Vue Green */
  border-radius: 50%;
  box-shadow: 0 0 10px rgba(66, 184, 131, 0.5);
}

.navbar-brand {
  text-decoration: none;
  color: #2c3e50;
  font-weight: 800;
  font-size: 1.25rem;
  letter-spacing: -0.5px;
}

.navbar-brand span {
  color: #42b883; /* Highlight part of the name */
}

/* Status Badge */
.status-badge {
  background: #e1f5fe;
  color: #0288d1;
  padding: 4px 12px;
  border-radius: 20px;
  font-size: 0.75rem;
  font-weight: bold;
  text-transform: uppercase;
  border: 1px solid #b3e5fc;
}

/* Hover effect for the brand */
.navbar-brand:hover {
  opacity: 0.8;
}
</style>