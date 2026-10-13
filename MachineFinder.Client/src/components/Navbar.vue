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
  position: sticky;
  top: 0;
  z-index: 1000;
  display: flex;
  align-items: center;
  box-sizing: border-box;
  padding:
    max(0.75rem, env(safe-area-inset-top))
    max(1.25rem, env(safe-area-inset-right))
    0.75rem
    max(1.25rem, env(safe-area-inset-left));
  background-color: rgba(255, 255, 255, 0.85);
  -webkit-backdrop-filter: blur(10px);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid #e5e5e5;
}

.container-fluid {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 0.75rem;
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
}

/* ---------- Brand ---------- */
.brand-section {
  display: flex;
  align-items: center;
  gap: 0.6rem;
  min-width: 0;
}

.logo-dot {
  flex-shrink: 0;
  width: 12px;
  height: 12px;
  background: #000000;
  border-radius: 50%;
}

.navbar-brand {
  min-width: 0;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-size: clamp(1.05rem, 1rem + 0.5vw, 1.25rem);
  font-weight: 800;
  letter-spacing: -0.5px;
  color: #000000;
  text-decoration: none;
  transition: opacity 0.15s ease;
}

.navbar-brand span {
  color: #6b6b6b;
}

.navbar-brand:hover {
  opacity: 0.7;
}

/* ---------- Status badge ---------- */
.status-badge {
  flex-shrink: 0;
  padding: 4px 12px;
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.03em;
  text-transform: uppercase;
  white-space: nowrap;
  color: #ffffff;
  background: #000000;
  border-radius: 999px;
}

/* ---------- Phones ---------- */
@media (max-width: 480px) {
  .navbar {
    padding-left: max(0.9rem, env(safe-area-inset-left));
    padding-right: max(0.9rem, env(safe-area-inset-right));
  }

  .status-badge {
    padding: 3px 9px;
    font-size: 0.65rem;
  }
}
</style>