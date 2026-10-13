<template>
    <div class="suggestion-container">
        <div class="zone">
            <label for="param-select">Parameter</label>
            <select 
            id="param-select"
            class="custom-select"
            v-model="selectedKey"
            >
                <option value="" disabled selected >Select a key</option>
                <option
                v-for="(value, key) in filteredParams" 
                :key="key"
                :value="key"
                >
                {{ formatLabel(key) }}
                </option>
            </select>
        </div>
        <div class="zone">
            <label for="value-zone-display">Current Value</label>
            <input
            type="text"
            :value="currentValue()"
            readonly
            class="display-value"
            />
        </div>
        <div class="zone">
            <label for="suggest-value-zone">Your Suggestion</label>
            <DropdownYesNo
                v-if="isDropDownYesNoType"
                @update-choice="updateChoice" 
            />
            <input 
                v-else-if="isDateType" 
                type="date" 
                v-model="suggestedValue" 
                class="suggest-value" 
            />
            <input 
                v-else 
                type="text" 
                v-model="suggestedValue" 
                class="suggest-value" 
                placeholder="Type new value" 
            />
        </div>
        <div class="zone add-zone">
            <Button
            @click="addSuggestion"
            :button-text="AddButton"
            :disabled="addButtonDisabled" >
            </Button>
        </div>
    </div>
    <SuggestionPreview 
    :suggestion_list="suggestChangeList"
    @remove-suggestion="removeSuggestion"/>
    <div class="zone action-zone">
        <Button 
        @click="submitSuggestion" 
        :disabled="submitButtonDisabled" 
        :button-text="submitButton">
        </Button>
    </div>
    <div v-if="successMessage" class="alert alert-success">
    ✅ {{ successMessage }}
    </div>

    <div v-if="errorList.length > 0" class="alert alert-danger">
        <strong>⚠️ {{ errorMessage || 'Submission Failed' }}</strong>
        <ul>
            <li v-for="(err, index) in errorList" :key="index">{{ err }}</li>
        </ul>
        <button @click="errorList = []" class="close-btn">dismiss</button>
    </div>
</template>
<script>
import Button from '@/components/Button.vue'
import SuggestionPreview from '@/components/SuggestionPreview.vue';
import DropdownYesNo from '@/components/DropdownYesNo.vue';
export default {
    name:"SuggestionCurrentModule",
    components:{
        Button,
        SuggestionPreview,
        DropdownYesNo
    },
    props:{
        filteredParams:{
            type:Array,
            default: ()=>([])
        },
        machine_id:{
            type:Number
        }
    },
    data(){
        return {
            selectedKey: null,
            suggestedValue: '',
            AddButton: 'Add More Suggestion',
            submitButton: 'Submit Suggestions',
            addButtonDisabled: true,
            submitButtonDisabled: true,
            suggestChangeList: [],
            successMessage: '',
            errorMessage: '',
            errorList: [],
        }
    },
    watch:{
        selectedKey(){
            this.isAddButtonDisabled();
            this.suggestedValue = '';
        },
        suggestedValue(){
            this.isAddButtonDisabled();
        },
        suggestChangeList: {
            handler() {
                this.isSubmitButtonDisabled();
            },
            deep: true // This tells Vue to look inside the array
        }
    },
    computed: {
        currentKeyName() {
        const entry = this.filteredParams[this.selectedKey];
        return entry ? entry[0] : '';
        },

        isDropDownYesNoType() {
            const dropdownKeys = ['indoor', 'disused', 'charge', 'cash', 'coins', 'credit_cards', 'debit_cards'];
            return dropdownKeys.includes(this.currentKeyName);
        },

        isDateType() {
            return this.currentKeyName === 'check_date';
        }
    },
    methods:{
        formatLabel(key){
            const paramKey = this.filteredParams[key][0]
            return paramKey.charAt(0).toUpperCase() + paramKey.slice(1).replace(/_/g,' ');
        },
        currentValue(){
            const entry = this.filteredParams[this.selectedKey]
            if(entry){
                const [key, value] = entry;
                return value
            }
            return ''
        },
        updateChoice(choice){
            this.suggestedValue = choice;
        },
        isAddButtonDisabled(){
            const isKeyEmpty = this.selectedKey === '' ||this.selectedKey === null;
            const isSuggestEmpty = this.suggestedValue.trim() === '';
            const isSuggestSameAsCurrent = this.suggestedValue.trim().toUpperCase() === this.currentValue().trim().toUpperCase();
            if (isKeyEmpty || isSuggestEmpty || isSuggestSameAsCurrent){
                this.addButtonDisabled = true
            }
            else{
                this.addButtonDisabled = false
            }
        },
        isSubmitButtonDisabled(){
            if (this.suggestChangeList.length === 0){
                this.submitButtonDisabled = true
            }
            else{
                this.submitButtonDisabled = false
            }
        },
        addSuggestion(){
            if (this.suggestChangeList.some(suggestion => suggestion.TagKey === this.filteredParams[this.selectedKey][0]))
            {
                alert('You have already added a suggestion for this parameter.');
                return;
            }
            this.suggestChangeList.push({
                MachineID: this.machine_id,
                TagKey: this.filteredParams[this.selectedKey][0],
                TagValue: this.currentValue(),
                SuggestTagValue: this.suggestedValue
            });
            this.selectedKey = null;
            this.suggestedValue = '';
        },
        removeSuggestion(index){
            this.suggestChangeList.splice(index, 1)
        },
        displaySuccess() {
            this.successMessage = "Suggestions submitted successfully! Thanks for your contribution.";
            this.errorList = [];
            this.errorMessage = '';
            
            // Hide success message after 5 seconds
            setTimeout(() => { this.successMessage = ''; }, 5000);
        },
        displayErrors(errors, message) {
            this.successMessage = '';
            this.errorMessage = message;
            // If errors is null/empty, provide a generic fallback
            this.errorList = errors.length > 0 ? errors : ["Please try again later"];
        },
        async submitSuggestion(){
            const url = `${import.meta.env.VITE_API_URL}/api/Feedback/submitMachineSuggestion`;
            try{
                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.suggestChangeList)
                });
                if (response.ok) {
                    this.suggestChangeList = [];
                    this.displaySuccess();
                }
                else{
                    const errorData = await response.json();
                    this.displayErrors(errorData.errors, errorData.message);
                }
            }
            catch (error) {
                this.displayErrors([], "Could not connect to the server.");
            }
        },
    }
}
</script>
<style scoped>
.suggestion-container {
  display: flex;
  flex-wrap: wrap;
  align-items: flex-end; /* inputs share one baseline */
  gap: clamp(0.75rem, 2vw, 1.25rem);
  box-sizing: border-box;
  padding: clamp(1rem, 3vw, 1.5rem);
  color: #000000;
  background: #ffffff;
  border: 1px solid #e5e5e5;
  border-radius: var(--radius, 16px);
  box-shadow: 0 10px 30px -10px rgba(0, 0, 0, 0.12),
              0 2px 6px rgba(0, 0, 0, 0.05);
}

.zone {
  display: flex;
  flex-direction: column;
  flex: 1 1 160px;
  min-width: 0; /* lets fields shrink instead of overflowing */
}

.zone label {
  margin-bottom: 0.4rem;
  font-size: 0.85rem;
  font-weight: 600;
  color: #4b4b4b;
}

/* Add button zone: same height as the fields next to it */
.zone.add-zone {
  flex: 0 0 auto;
  height: var(--control-height, 48px);
}

.zone.add-zone button {
  height: 100%;
}

/* ---------- Fields: one shared height ---------- */
.custom-select,
.display-value,
.suggest-value {
  display: flex;
  align-items: center;
  width: 100%;
  height: var(--control-height, 48px);
  min-height: var(--control-height, 48px);
  box-sizing: border-box;
  margin: 0;
  padding: 0 1rem;
  font-family: inherit;
  font-size: 16px; /* 16px+ prevents iOS zoom on focus */
  line-height: 1.2;
  color: #000000;
  background-color: #ffffff;
  border: 2px solid #000000;
  border-radius: var(--radius, 12px);
  outline: none;
  transition: box-shadow 0.15s ease;
}

.custom-select {
  -webkit-appearance: none;
  appearance: none;
  padding-right: 2.5rem;
  cursor: pointer;
  background-image: url("data:image/svg+xml;charset=UTF-8,%3Csvg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24'%3E%3Cpath fill='%23000' d='M7 10l5 5 5-5z'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 0.85rem center;
  background-size: 1.25rem;
}

/* Read-only look */
.display-value {
  color: #6b6b6b;
  background-color: #f3f3f3;
  border-color: #d4d4d4;
  cursor: not-allowed;
}

.custom-select:focus,
.suggest-value:focus {
  box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.15);
}

/* ---------- Alerts ---------- */
.alert {
  position: relative;
  box-sizing: border-box;
  margin: 1rem 0;
  padding: 0.9rem 1rem 0.9rem 1.1rem;
  font-size: 0.95rem;
  line-height: 1.5;
  overflow-wrap: anywhere;
  border: 1px solid;
  border-left-width: 4px;
  border-radius: 12px;
}

.alert-success {
  color: #14532d;
  background: #f3faf5;
  border-color: #bfe3cb;
}

.alert-danger {
  color: #7f1d1d;
  background: #fdf3f3;
  border-color: #f1c4c4;
}

.alert ul {
  margin: 0.6rem 0 0 1.25rem;
  padding: 0;
}

.close-btn {
  float: right;
  margin-left: 0.75rem;
  padding: 0.25rem 0.5rem;
  font-size: 0.85rem;
  color: inherit;
  text-decoration: underline;
  background: none;
  border: none;
  cursor: pointer;
}

/* ---------- Phones: every zone gets its own row ---------- */
@media (max-width: 600px) {
  .zone,
  .zone.add-zone {
    flex: 1 1 100%;
  }
}
</style>