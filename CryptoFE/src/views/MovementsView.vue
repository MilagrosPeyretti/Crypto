<template>
  <div class="main-content">
    <h2>Movimientos</h2>
    <p v-if="loading">Cargando clientes...</p>
    <p v-if="errorMessage">{{ errorMessage }}</p>

    <table v-if="!loading && clients.length">
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
          <tr v-for="client in clients" :key="client.id">
          <td>{{ client.id }}</td>
          <td>{{ client.name }}</td>
          <td>
             <button @click="goToTransactions(client.id)" class="btn">Transacciones</button>
            <button @click="goToSummary(client.id)" class="btn">Resumen</button>
          </td>
        </tr>
      </tbody>
    </table>

  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const clients = ref([])
const loading = ref(true)
const router = useRouter()
const errorMessage = ref('')


const fetchClients = async () => {
  try {
    const response = await axios.get('https://localhost:7027/api/Clients')
    clients.value = response.data
  } catch (err) {
    if (err.response.data) {
          errorMessage.value = err.response.data
    } else {
      console.error('Error saving transaction:', err)
    }
  } finally {
    loading.value = false
  }
}

const goToTransactions = (clientId) => {
  router.push(`/movements/transactions/${clientId}`)
}

const goToSummary = (clientId) => {
  router.push(`/movements/summary/${clientId}`)
}

onMounted(fetchClients)
</script>
