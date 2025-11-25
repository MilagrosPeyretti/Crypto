<template>
  <div class="main-content">
    <h2>Editar Transacción</h2>
    <p v-if="loading">Cargando transacción...</p>
    <p v-if="error">{{ error }}</p>

    <TransactionForm
      v-if="!loading && !error"
      :initial-values="transaction"
      :client-id="transaction.clientId"
      :transaction-id="transaction.id"
      submit-text="Actualizar Transacción"
      @onSuccess="goToList"
    />
    <BackToTransactionsButton></BackToTransactionsButton>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'
import TransactionForm from '@/components/TransactionForm.vue'
import BackToTransactionsButton from '@/components/BackToTransactionsButton.vue'

const router = useRouter()
const route = useRoute()
const id = route.params.id
const transaction = ref(null)
const loading = ref(true)
const error = ref(null)

const fetchTransaction = async () => {
  try {
    const res = await axios.get(`https://localhost:7027/api/Transactions/${id}`)
    transaction.value = res.data
  } catch (err) {
    error.value = 'Error loading transaction'
    console.error(err)
  } finally {
    loading.value = false
  }
}

const goToList = () => {
 setTimeout(() => {
    router.push('/transaction')
  }, 1000)
}

onMounted(fetchTransaction)
</script>
