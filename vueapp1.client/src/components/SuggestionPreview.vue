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
    margin-top: 20px;
    padding: 15px;
    background-color: #f9f9f9;
    border-radius: 8px;
    border: 1px solid #ddd;
}

.suggestion-list-header, 
.suggestion-list-row {
    display: grid;
    grid-template-columns: 1.2fr 1fr 1fr 40px; 
    gap: 12px;
    align-items: center;
}

.suggestion-list-header {
    font-weight: bold;
    color: #35495e; 
    border-bottom: 2px solid #eee;
    padding: 10px 0;
    text-transform: uppercase;
    font-size: 0.7rem;
    letter-spacing: 0.05em;
}

.suggestion-list-row {
    padding: 8px 0;
    border-bottom: 1px solid #eee;
}

.action-col {
    display: flex;
    justify-content: center;
    width: 40px;
}

.suggestion-row-item {
    font-size: 0.9rem;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.highlight-text {
    font-weight: 600;
    color: #42b883; /* Vue Green to show it's a new change */
}

/* Specific styling for the 'X' button */
.btn-remove-icon {
    background: transparent !important;
    border: none !important;
    color: #ff4444 !important;
    font-size: 1.5rem !important;
    cursor: pointer;
    padding: 0 !important;
    line-height: 1;
    box-shadow: none !important;
}

.btn-remove-icon:hover {
    color: #cc0000 !important;
    transform: scale(1.2);
}

.no-suggestion h3 {
    text-align: center;
    color: #999;
    font-style: italic;
    padding: 20px;
}
</style>
