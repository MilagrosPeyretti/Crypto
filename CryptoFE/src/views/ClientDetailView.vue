<template>
  <div class="main-content">
    <h2>Detalle Cliente</h2>

    <p v-if="loading">Cargando detalles del cliente...</p>
    <p v-if="error">{{ error }}</p>

    <div v-if="!loading && client">
      <p><strong>ID:</strong> {{ client.id }}</p>
      <p><strong>Name:</strong> {{ client.name }}</p>
      <p><strong>Email:</strong> {{ client.email }}</p>

      <BackToClientsButton></BackToClientsButton>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import axios from 'axios'
import BackToClientsButton from '@/components/BackToClientsButton.vue'

const client = ref(null)
const loading = ref(true)
const error = ref(null)
const route = useRoute()
const clientId = route.params.id

const fetchClient = async () => {
  try {
    const response = await axios.get(`https://localhost:7027/api/Clients/${clientId}`)
    client.value = response.data
  } catch (err) {
    error.value = 'Error fetching client details'
    console.error(err)
  } finally {
    loading.value = false
  }
}
onMounted(fetchClient)

</script>

<style scoped>

</style>
