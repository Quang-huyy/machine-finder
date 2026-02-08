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
            console.log(this.suggestChangeList, this.suggestChangeList.length)
            if (this.suggestChangeList.length === 0){
                this.submitButtonDisabled = true
            }
            else{
                this.submitButtonDisabled = false
            }
            console.log(this.submitButtonDisabled)
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
    gap: 20px;
    align-items: flex-end; /* Aligns the inputs at the bottom */
    background: #f9f9f9;
    padding: 20px;
    border-radius: 8px;
    border: 1px solid #ddd;
}

.zone {
  display: flex;
  flex-direction: column;
  flex: 1;
  min-width: 150px;
}

.zone label {
  font-size: 0.9rem;
  font-weight: bold;
  color: #58616b; /* Navy color from earlier */
  margin-bottom: 8px;
}
.zone.add-zone {
    height: 40px;
}
.zone.add-zone button {
    height: 100%; 
}
.custom-select, .display-value, .suggest-value {
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 1rem;
}

.display-value {
  background-color: #eee; /* Makes it look read-only */
  color: #666;
  cursor: not-allowed;
}

.suggest-value:focus {
  border-color: #42b883; /* Vue Green */
  outline: none;
  box-shadow: 0 0 5px rgba(66, 184, 131, 0.3);
}
.alert {
    padding: 15px;
    margin: 20px 0;
    border-radius: 4px;
    position: relative;
}

.alert-success {
    background-color: #d4edda;
    color: #155724;
    border: 1px solid #c3e6cb;
}

.alert-danger {
    background-color: #f8d7da;
    color: #721c24;
    border: 1px solid #f5c6cb;
}

.alert ul {
    margin: 10px 0 0 20px;
    padding: 0;
}

.close-btn {
    float: right;
    background: none;
    border: none;
    font-size: 0.8rem;
    cursor: pointer;
    text-decoration: underline;
    color: inherit;
}
</style>