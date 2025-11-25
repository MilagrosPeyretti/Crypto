<template>
  <div class="main-content">
    <h2>Esitar Cliente</h2>
    <p v-if="loading">Cargando cliente...</p>
    <p v-if="error">{{ error }}</p>

    <ClientForm
      v-if="!loading && !error"
      :initial-values="client"
      :client-id="client.id"
      submitText="Actualizar Cliente"
      @onSuccess="goToList"
      />
      <BackToClientsButton></BackToClientsButton>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'
import ClientForm from '@/components/ClientForm.vue'
import BackToClientsButton from '@/components/BackToClientsButton.vue'


const router = useRouter()
const route = useRoute()
const clientId = route.params.id
const client = ref(null)
const loading = ref(true)
const error = ref(null)

const fetchClient = async () => {
  try {
    const response = await axios.get(`https://localhost:7027/api/Clients/${clientId}`)
    client.value = response.data
  } catch (err) {
    error.value = 'Error loading client'
    console.error(err)
  } finally {
    loading.value = false
  }
}

const goToList = () => {
  setTimeout(() => {
    router.push('/client')
  }, 1000)
}

onMounted(fetchClient)
</script>

<style scoped>
button {
  margin-top: 1rem;
}
</style>
