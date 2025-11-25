<template>
  <div class="main-content">
    <h2>Nuevo Cliente</h2>
    <ClientForm @onSuccess="goToList" submitText="Crear Cliente" />
    <div class="header-row">
    <BackToClientsButton></BackToClientsButton>
    <BackToHome></BackToHome>
  </div>
  </div>
</template>

<script setup>
import ClientForm from '@/components/ClientForm.vue'
import { useRouter } from 'vue-router'
import BackToClientsButton from '@/components/BackToClientsButton.vue'
import BackToHome from '@/components/BackToHome.vue'

const router = useRouter()

const goToList = () => {
  setTimeout(() => {
    router.push('/client')
  }, 1000)
}
</script>

<style scoped></style>
