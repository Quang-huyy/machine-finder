<template>
  <div id="map"></div>
</template>

<script>
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import {redIcon, blueIcon, iconWithNumber} from '@/assets/marker.js';
import { h, render } from 'vue';
import Item from "@/components/Item.vue";
import '@/assets/marker.css';
export default {
  name: "LeafletMap",
  components: { Item },
  props: {
    lat: {
      type: Number,
    },
    lon: {
      type: Number,
    },
    maxDistance:{
      type: Number,
    }
  },
  data() {
    return {
      map: null,
      marker: null,
      data: null,
      nearbyMarkers: [],
    };
  },
  mounted() {
    this.initMap();
  },
  watch: {
    latLng: {
      handler() {
        this.updateMarker();
      }
    },
    maxDist: {
      handler() {
        this.updateMarker();
      }
    },
  },
  computed:{
    latLng() {
      return `${this.lat}|${this.lon}`;
    },
    maxDist(){
      return this.maxDistance;
    }
  },
  methods: {
    async initMap() {
      this.map = L.map("map").setView([this.lat, this.lon], 13);
    
      L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        attribution: "© OpenStreetMap contributors",
      }).addTo(this.map);
      this.marker = L.marker([this.lat, this.lon], { icon: redIcon }).addTo(this.map);
      const data = await this.getResult();
      await this.displayNearbyMachine(data)
    },
    async updateMarker() {
      if (!this.map || !this.marker) return;
      this.marker.setLatLng([this.lat, this.lon]);
      this.map.setView([this.lat, this.lon], this.map.getZoom(), { animate: false });
      const data = await this.getResult();
      await this.displayNearbyMachine(data)
    },

    async displayNearbyMachine(data){
      this.nearbyMarkers.forEach(m => {
          this.map.removeLayer(m);
      });
      this.nearbyMarkers = [];
      
      if (Array.isArray(data) && data.length !== 0){
        for (let item of data) { 
          const numberIcon = iconWithNumber(item.position);
          const container = document.createElement('div');
          const vnode = h(Item, {
            position:item.position,
            id: item.id,
            house: item.house,
            street: item.street,
            postcode: item.postcode,
            city: item.city,
            distance: item.distance,
            lat: item.lat,
            lon: item.lon,
            params_info: item.params_info
          });

          render(vnode, container);

          const newMarker = L.marker([item.lat, item.lon], { icon: numberIcon })
            .addTo(this.map)
            .bindPopup(container, {
              padding: [0, 0]
            });

          this.nearbyMarkers.push(newMarker);
        }
      }
      this.$emit('update-result', data);
    },
    async getResult() {
      const url = `${import.meta.env.VITE_API_URL}/api/Address/getResults?maxDistance=${this.maxDistance}`;
      const response = await fetch(url, {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json'
          },
          body: JSON.stringify({ lat: this.lat.toString(), lon: this.lon.toString()})
      });
      const data = await response.json();
      return data;
    }
  },
};
</script>

<style>
#map {
  height: var(--shared-height);
  width: 100%;
  border-radius: 8px;
}

</style>