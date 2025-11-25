<template>
  <div class="main-content">
    <h2>Transacciones Cliente</h2>
    <p v-if="loading">Cargando transacciones...</p>
    <p v-if="error">{{ error }}</p>

    <table v-if="!loading && transactions.length">
      <thead>
        <tr>
          <th>ID</th>
          <th>Crypto</th>
          <th>Tipo</th>
          <th>Cantidad</th>
          <th>ARS</th>
          <th>Fecha</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="transaction in transactions" :key="transaction.id">
          <td>{{ transaction.id }}</td>
          <td>{{ transaction.cryptoCode }}</td>

          <td>{{ formatAction(transaction.action) }}</td>

          <td>{{ transaction.cryptoAmount }}</td>
          <td>{{ transaction .money }}</td>
          <td>{{ new Date(transaction.dateTime).toLocaleString() }}</td>
        </tr>
      </tbody>
    </table>
    <BackToAccounting></BackToAccounting>
    <p v-if="!loading && transactions.length === 0">No se encontraron transacciones para este cliente</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import axios from 'axios'
import BackToAccounting from '@/components/BackToAccounting.vue'

const route = useRoute()
const clientId = route.params.clientId
const transactions = ref([])
const loading = ref(true)
const error = ref(null)

const formatAction = (action) => {
  if (action === 'purchase') return 'Compra'
  if (action === 'sale') return 'Venta'
  return action
}

const fetchTransactions = async () => {
  try {
    const res = await axios.get(`https://localhost:7027/api/Transactions?clientId=${clientId}`)
    transactions.value = res.data
  } catch (error) {
    error.value = 'Failed to load transactions'
  } finally {
    loading.value = false
  }
}

onMounted(fetchTransactions)
</script>

<style scoped>
</style>
