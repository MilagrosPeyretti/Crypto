<template>
  <div class="main-content">
    <div class="header-column">
      <h2>Bienvenido</h2>
      <div class="button-container">
        <button @click="goToCreateClient()" class="action-btn">Nuevo Cliente</button>
        <button @click="goToCreateTransaction()" class="action-btn">Nueva Transacción</button>
        <button @click="goToPrices()" class="action-btn secondary">Ver Precios Actuales</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

const goToCreateClient = () => {
  router.push(`/client/create`)
}
const goToCreateTransaction = () => {
  router.push(`/transaction/create`)
}
const goToPrices = () => {
  router.push(`/prices`)
}
</script>

<style scoped>
h2 {
  font-size: 4rem;
  font-weight: bold;
}

.main-content {
  padding: 2rem;
  display: flex;
  justify-content: center;

}
.header-column {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 50px;
}
.button-container {
    display: flex;
    flex-direction: column;
    gap: 15px;
}
.action-btn {
    padding: 15px 30px;
    font-size: 1.1rem;
    cursor: pointer;
    min-width: 200px;
}
.secondary {
    background-color: #6c757d;
}
</style>
