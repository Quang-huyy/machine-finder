<template>
    <div class="suggestion-preview-container">
        <div v-if="suggestion_list.length>0" class="suggestion-list">
            <h3>Pending Suggestions</h3>
            <div class="suggestion-list-header">
                <span class="suggestion-list-header-item">Parameter</span>
                <span class="suggestion-list-header-item">Current Value</span>
                <span class="suggestion-list-header-item">Suggested Value</span>
                <span class="suggestion-list-header-item"></span>
            </div>
            <div v-for="(item, index) in suggestion_list" :key="index" class="suggestion-list-row">
                <span class="suggestion-row-item"> {{ keyFormat(item.TagKey) }}</span>
                <span class="suggestion-row-item"> {{ item.TagValue }}</span>
                <span class="suggestion-row-item" > {{ item.SuggestTagValue }}</span>
                <div class="action-col">
                    <Button 
                        class="btn-remove-icon"
                        button-text="×"
                        @click="removeSuggestion(index)"
                    />
                </div>
            </div>
        </div>
        <div v-else class="no-suggestion">
            <h3>No current pending suggestions</h3>
        </div>
    </div>
</template>

<script>
import Button from '@/components/Button.vue'
export default {
    name: "SuggestionPreview",
    components:{
        Button
    },
    props:{
        suggestion_list:{
            type:Object,
            default: () => ([])
        },        
    },
    data(){
        return{
            removeButton: 'Remove'
        }
    },
    methods:{
        keyFormat(key){
            return key.charAt(0).toUpperCase() + key.slice(1).replace(/_/g,' ');
        },
        removeSuggestion(index){
            this.$emit('remove-suggestion', index)
        }
    }
}
</script>

<style scoped>
.suggestion-list {
  margin-top: 1.25rem;
  box-sizing: border-box;
  padding: clamp(0.75rem, 3vw, 1.1rem);
  color: #000000;
  background: #ffffff;
  border: 1px solid #e5e5e5;
  border-radius: var(--radius, 16px);
}

.suggestion-list-header,
.suggestion-list-row {
  display: grid;
  /* minmax(0, ...) is what makes the ellipsis work inside a grid */
  grid-template-columns: minmax(0, 1.2fr) minmax(0, 1fr) minmax(0, 1fr) 44px;
  gap: 0.75rem;
  align-items: center;
}

.suggestion-list-header {
  padding: 0.6rem 0;
  font-size: 0.7rem;
  font-weight: 700;
  letter-spacing: 0.05em;
  text-transform: uppercase;
  color: #4b4b4b;
  border-bottom: 2px solid #000000;
}

.suggestion-list-row {
  padding: 0.5rem 0;
  border-bottom: 1px solid #eeeeee;
}

.suggestion-list-row:last-child {
  border-bottom: none;
}

.action-col {
  display: flex;
  justify-content: center;
  width: 44px;
}

.suggestion-row-item {
  min-width: 0;
  font-size: 0.9rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

/* New / changed value */
.highlight-text {
  font-weight: 700;
  color: #000000;
  text-decoration: underline;
  text-underline-offset: 3px;
}

/* ---------- Remove (X) button ---------- */
.btn-remove-icon {
  display: grid !important;
  place-items: center;
  width: 36px;
  height: 36px;
  min-height: 0 !important; /* overrides the shared button height */
  padding: 0 !important;
  font-size: 1.4rem !important;
  line-height: 1;
  color: #c62828 !important;
  background: transparent !important;
  border: none !important;
  border-radius: 50% !important;
  box-shadow: none !important;
  cursor: pointer;
  transition: background-color 0.15s ease, transform 0.1s ease;
}

.btn-remove-icon:hover {
  background: #fdecec !important;
  transform: scale(1.1);
}

.btn-remove-icon:focus-visible {
  outline: 3px solid rgba(0, 0, 0, 0.35);
  outline-offset: 2px;
}

.no-suggestion h3 {
  padding: 1.25rem;
  font-size: 1rem;
  font-weight: 500;
  font-style: italic;
  text-align: center;
  color: #6b6b6b;
}

/* ---------- Phones: let text wrap instead of cutting it off ---------- */
@media (max-width: 480px) {
  .suggestion-list-header,
  .suggestion-list-row {
    gap: 0.5rem;
  }

  .suggestion-row-item {
    font-size: 0.85rem;
    white-space: normal;
    overflow-wrap: anywhere;
  }
}
</style>