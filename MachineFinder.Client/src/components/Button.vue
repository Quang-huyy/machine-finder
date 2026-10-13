<template>
    <button 
    class="btn">
    {{ buttonText }}
    </button>
</template>

<script>
export default {
    name: 'Button',
    components: {},
    props:{
        buttonText:{
            type:String,
            default:'Click Me'
        }
    },
}
</script>
<style scoped>
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.4rem;
  height: var(--control-height, 48px);
  min-height: var(--control-height, 48px);
  box-sizing: border-box;
  padding: 0 1.25rem;
  font-family: inherit;
  font-size: 16px;
  font-weight: 600;
  line-height: 1.2;
  white-space: nowrap;
  color: #ffffff;
  background: #000000;
  border: 2px solid #000000;
  border-radius: var(--radius, 12px);
  cursor: pointer;
  transition: background-color 0.15s ease, border-color 0.15s ease,
              transform 0.1s ease;
}

.btn:hover:not(:disabled) {
  background: #333333;
  border-color: #333333;
}

.btn:active:not(:disabled) {
  transform: scale(0.97);
}

.btn:focus-visible {
  outline: 3px solid rgba(0, 0, 0, 0.35);
  outline-offset: 2px;
}

.btn:disabled {
  color: #9a9a9a;
  background: #f0f0f0;
  border-color: #e0e0e0;
  cursor: not-allowed;
}
</style>