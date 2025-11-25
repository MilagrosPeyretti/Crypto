<template>
  <div class="main-content">
    <div class="header-row">
    <h2>Transacciones</h2>
    <button @click="goToCreate()">Nueva Transacción</button>
  </div>

    <p v-if="loading">Cargando transacciones...</p>
    <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>


    <table v-if="!loading && transactions.length">
      <thead>
        <tr>
          <th>ID</th>
          <th>Crypto</th>
          <th>Tipo</th>
          <th>Cliente</th>
          <th>Monto</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="transaction in transactions" :key="transaction.id">
          <td>{{ transaction.id }}</td>
          <td>{{ transaction.cryptoCode }}</td>
          <td>{{ formatAction(transaction.action) }}</td>
          <td>{{ transaction.clientName}}</td>
          <td>{{ transaction.money }}</td>
          <td>
             <button @click="goToDetails(transaction.id)">Detalles</button>
            |
             <button @click="goToEdit(transaction.id)">Editar</button>
            |
            <button @click="deleteTransaction(transaction.id)">Eliminar</button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-if="!loading && transactions.length === 0">No se encontraron transacciones.</p>
    <p v-if="success" class="success-text">Transacción eliminada con éxito</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const formatAction = (action) => {
  if (action === 'purchase') return 'Compra'
  if (action === 'sale') return 'Venta'
  return action
}
const transactions = ref([])
const loading = ref(true)
const success = ref(false)
const errorMessage = ref('')


const fetchTransactions = async () => {
  try {
    const response = await axios.get('https://localhost:7027/api/Transactions')
    transactions.value = response.data
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

import { useRouter } from 'vue-router'
const router = useRouter()

const goToCreate = () => {
  router.push(`/transaction/create`)
}
const goToEdit = (id) => {
  router.push(`/transaction/edit/${id}`)
}
const goToDetails = (id) => {
  router.push(`/transaction/${id}`)
}


const deleteTransaction = async (id) => {
  try {
    await axios.delete(`https://localhost:7027/api/Transactions/${id}`)
    success.value = true
    setTimeout(async () => {
      await fetchTransactions()
      success.value = false
    }, 1000)
  } catch (err) {
    console.error('Error deleting transaction:', err)
  }
}

onMounted(fetchTransactions)
</script>

