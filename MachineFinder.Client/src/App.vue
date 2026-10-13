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
/* ---------- Design tokens ---------- */
.app-container {
  --bg: #ffffff;
  --text: #000000;
  --text-muted: #4b4b4b;
  --accent: #000000;
  --alert-bg: #ffffff;
  --alert-border: #e5e5e5;
  --shadow: 0 10px 30px -10px rgba(0, 0, 0, 0.12),
            0 2px 6px rgba(0, 0, 0, 0.05);
  --radius: 16px;
  --content-width: 800px;
  --gap: clamp(1rem, 3vw, 1.5rem);
  --pad-x: clamp(0.75rem, 4vw, 1.5rem);
  --pad-y: clamp(1rem, 4vw, 2rem);
}

/* ---------- Global page styles ---------- */
:global(*),
:global(*::before),
:global(*::after) {
  box-sizing: border-box;
}

:global(html) {
  -webkit-text-size-adjust: 100%;
  text-size-adjust: 100%;
}

:global(body) {
  margin: 0;
  background: #ffffff;
  font-family: "Inter", system-ui, -apple-system, "Segoe UI", Roboto, sans-serif;
  -webkit-font-smoothing: antialiased;
  -webkit-tap-highlight-color: transparent;
  overflow-x: hidden; /* never allow sideways scrolling */
}

/* ---------- Layout ---------- */
.app-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  min-height: calc(100dvh / 0.8);
  min-height: 100dvh; /* accounts for mobile browser toolbars */
  gap: var(--gap);
  padding:
    var(--pad-y)
    max(var(--pad-x), env(safe-area-inset-right))
    max(var(--pad-y), env(safe-area-inset-bottom))
    max(var(--pad-x), env(safe-area-inset-left));
  font-size: clamp(0.875rem, 0.85rem + 0.2vw, 0.95rem);
  color: var(--text);
  background: var(--bg);
}

/* Every direct child (alert, SearchZone, Displayzone) shares one column */
.app-container > * {
  width: 100%;
  max-width: var(--content-width);
  min-width: 0; /* lets flex children shrink instead of overflowing */
}

/* ---------- Info alert ---------- */
.info-alert {
  display: flex;
  gap: 1rem;
  align-items: flex-start;
  padding: clamp(1rem, 3vw, 1.25rem) clamp(1.1rem, 3.5vw, 1.5rem);
  line-height: 1.6;
  color: var(--text);
  background: var(--alert-bg);
  border: 1px solid var(--alert-border);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  position: relative;
  overflow: hidden;
  animation: fade-up 0.5s ease-out both;
}

.info-alert::before {
  content: "";
  position: absolute;
  inset: 0 auto 0 0;
  width: 4px;
  background: var(--accent);
}

.alert-icon {
  flex-shrink: 0;
  display: grid;
  place-items: center;
  width: 2.5rem;
  height: 2.5rem;
  font-size: 1.25rem;
  border-radius: 12px;
  background: #f3f3f3;
}

.alert-content {
  min-width: 0;
  overflow-wrap: anywhere; /* long words/URLs can't break the layout */
}

.alert-content p {
  margin: 0 0 0.5rem;
}

.alert-content p:last-child {
  margin-bottom: 0;
}

.alert-content strong {
  color: var(--text);
  font-weight: 700;
}

.alert-footer {
  font-size: 0.9em;
  color: var(--text-muted);
}

/* ---------- Animation ---------- */
@keyframes fade-up {
  from { opacity: 0; transform: translateY(12px); }
  to   { opacity: 1; transform: translateY(0); }
}

@media (prefers-reduced-motion: reduce) {
  .info-alert { animation: none; }
}

/* ---------- Large screens ---------- */
@media (min-width: 1024px) {
  .app-container {
    --content-width: 960px;
  }
}

/* ---------- Phones ---------- */
@media (max-width: 480px) {
  .app-container {
    --radius: 14px;
  }

  .info-alert {
    flex-direction: column;
    gap: 0.75rem;
  }

  .alert-icon {
    width: 2.25rem;
    height: 2.25rem;
  }
}

/* ---------- Touch devices ---------- */
@media (hover: none) and (pointer: coarse) {
  .app-container :where(button, a, input, select) {
    min-height: 44px; /* comfortable tap targets */
  }
}

.app-container > .display-zone {
  max-width: min(1280px, 100%);
}
:global(body) {
  zoom: 0.8; /* same look as 80% browser zoom */
}
</style>