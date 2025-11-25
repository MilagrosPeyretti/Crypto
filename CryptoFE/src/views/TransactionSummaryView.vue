<template>
  <div class="main-content">
    <h2>Resumen de Transacciones</h2>
    <p v-if="loading">Cargando resumen...</p>
    <p v-if="error">{{ error }}</p>

    <div v-if="!loading && summary">
      <table>
        <thead>
          <tr>
            <th>Crypto</th>
            <th>Cantidad</th>
            <th>Valor (ARS)</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="crypto in summary.cryptos" :key="crypto.cryptoCode">
            <td>{{ crypto.cryptoCode }}</td>
            <td>{{ crypto.amount }}</td>
            <td>{{ crypto.valueARS }}</td>
          </tr>
        </tbody>
      </table>
      <h3>Total ARS: {{ summary.totalARS }}</h3>
    </div>

    <button @click="goBack" class="btn">Atrás</button>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'

const summary = ref(null)
const loading = ref(true)
const error = ref(null)
const route = useRoute()
const router = useRouter()
const clientId = route.params.clientId

const fetchSummary = async () => {
  try {
    const res = await axios.get(`https://localhost:7027/api/Transactions/summary/${clientId}`)
    summary.value = res.data
  } catch (err) {
    error.value = 'Failed to load summary'
    console.error(err)
  } finally {
    loading.value = false
  }
}

const goBack = () => {
  router.push('/movements')
}

onMounted(fetchSummary)
</script>

<style scoped>
</style>

