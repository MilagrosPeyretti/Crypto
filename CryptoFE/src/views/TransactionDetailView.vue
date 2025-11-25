<template>
  <div class="main-content">
    <h2>Transaction Details</h2>

    <p v-if="loading">Loading transaction details...</p>
    <p v-if="error" class="error-text">{{ error }}</p>

    <div v-if="transaction && !loading">
      <p><strong>ID:</strong> {{ transaction.id }}</p>
      <p><strong>Crypto:</strong> {{ transaction.cryptoCode }}</p>
      <p><strong>ClientId:</strong> {{ transaction.clientId }}</p>
      <p><strong>Name:</strong> {{ transaction.clientName }}</p>
      <p><strong>Type:</strong> {{ transaction.action }}</p>
      <p><strong>Amount:</strong> {{ transaction.cryptoAmount }}</p>
      <p><strong>ARS:</strong> {{ transaction.money }}</p>
      <p><strong>Date:</strong> {{ new Date(transaction.dateTime).toLocaleString() }}</p>
      <BackToTransactionsButton></BackToTransactionsButton>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import axios from 'axios'
import BackToTransactionsButton from '@/components/BackToTransactionsButton.vue'

const transaction = ref(null)
const loading = ref(true)
const error = ref('')
const route = useRoute()
const transactionId = route.params.id

const fetchTransaction = async () => {
 try {

    const res = await axios.get(`https://localhost:7027/api/Transactions/${transactionId}`)
    if (Array.isArray(res.data)) {

        transaction.value = res.data.find(t => t.id === Number(transactionId));
    } else {
        transaction.value = res.data;
    }

  } catch (err) {
    error.value = 'Error fetching transaction details.'
    console.error(err)
  } finally {
    loading.value = false
  }
}

onMounted(fetchTransaction)
</script>
<style scoped>

</style>
