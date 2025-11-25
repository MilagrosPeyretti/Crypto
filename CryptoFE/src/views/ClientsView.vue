<template>
  <div class="main-content">
   <div class="header-row">
    <h2>Clientes</h2>
    <button @click="goToCreate()">Nuevo Cliente</button>
  </div>

    <p v-if="loading">Cargando clientes...</p>
    <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>


    <table v-if="!loading && clients.length">
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Email</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="client in clients" :key="client.id">
          <td>{{ client.id }}</td>
          <td>{{ client.name }}</td>
          <td>{{ client.email }}</td>
          <td>
             <button @click="goToDetails(client.id)">Details</button>
            |
             <button @click="goToEdit(client.id)">Edit</button>
            |
            <button @click="deleteClient(client.id)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-if="!loading && clients.length === 0">No clients found.</p>
    <p v-if="success" class="success-text">Client successfully deleted</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const clients = ref([])
const loading = ref(true)
const success = ref(false)
const errorMessage = ref('')


const fetchClients = async () => {
  try {
    const response = await axios.get('https://localhost:7027/api/Clients')
    clients.value = response.data
  } catch (err) {
    if (err.response.data) {
          errorMessage.value = err.response.data
    } else {
      console.error('Error saving client:', err)
    }
  } finally {
    loading.value = false
  }
}

import { useRouter } from 'vue-router'
const router = useRouter()

const goToCreate = () => {
  router.push(`/client/create`)
}
const goToEdit = (id) => {
  router.push(`/client/edit/${id}`)
}
const goToDetails = (id) => {
  router.push(`/client/${id}`)
}


const deleteClient = async (id) => {
  try {
    await axios.delete(`https://localhost:7120/api/client/${id}`)
    success.value = true
    setTimeout(async () => {
      await fetchClients()
      success.value = false
    }, 1000)
  } catch (err) {
    if (err.response.data) {
          errorMessage.value = err.response.data
    } else {
      console.error('Error saving transaction:', err)
    }
  }
}

onMounted(fetchClients)
</script>

