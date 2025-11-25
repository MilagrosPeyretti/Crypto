<template>
  <div class="main-content">
    <h2>Nueva Transacción</h2>

    <label>Seleccionar cliente:</label>
    <select v-model.number="selectedClientId" class="input-field">
      <option v-for="c in clients" :key="c.id" :value="c.id">
        {{ c.name }} ({{ c.email }})
      </option>
    </select>

    <TransactionForm
      v-if="selectedClientId"
      :client-id="selectedClientId"
      @onSuccess="goToList"
      submitText="Create Transaction"
    />
    <div class="header-row">
    <BackToTransactionsButton></BackToTransactionsButton>
    <BackToHome></BackToHome>
  </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import TransactionForm from '@/components/TransactionForm.vue'
import { useRouter } from 'vue-router'
import BackToTransactionsButton from '@/components/BackToTransactionsButton.vue'
import BackToHome from '@/components/BackToHome.vue'


const clients = ref([])
const selectedClientId = ref('')
const router = useRouter()

const goToList = () => {
  setTimeout(() => {
    router.push('/transaction')
  }, 1000)
}

onMounted(async () => {
  try {
    const response = await axios.get('https://localhost:7027/api/Clients')
    clients.value = response.data
  } catch (err) {
    console.error('Error fetching clients', err)
  }
})
</script>
