<template>
  <div class="main-content">
    <h2>Precio actual criptomonedas</h2>

    <p v-if="loading">Cargando precios...</p>
    <p v-if="error">{{ error }}</p>

    <table v-if="!loading && rows.length">
      <thead>
        <tr>
          <th>Crypto</th>
          <th>Mejor Compra (ARS)</th>
          <th>Mejor Venta (ARS)</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="row in rows" :key="row.crypto">
          <td>{{ row.crypto }}</td>
          <td>{{ row.bestBuy.totalAsk.toLocaleString() }} <small>({{ row.bestBuy.exchange }})</small></td>
          <td>{{ row.bestSell.totalBid.toLocaleString() }} <small>({{ row.bestSell.exchange }})</small></td>
        </tr>
      </tbody>
    </table>

    <div style="margin-top: 20px;">
        <router-link to="/">
            <button>Volver al Inicio</button>
        </router-link>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const loading = ref(true)
const error = ref(null)
const rows = ref([])

const exchanges = ['buenbit', 'binance', 'satoshitango']
const cryptos = ['BTC', 'ETH', 'USDT', 'USDC', 'DAI']

const fetchPrices = async () => {
  try {
    const volume = 0.1
    const fiat = 'ARS'

    for (const crypto of cryptos) {
      const prices = []
      for (const exchange of exchanges) {
        const url = `https://criptoya.com/api/${exchange}/${crypto}/${fiat}/${volume}`
        try {
          const { data } = await axios.get(url)
          prices.push({
            exchange,
            totalAsk: data.totalAsk,
            totalBid: data.totalBid
          })
        } catch (error) {
          console.error(`Error fetching ${crypto} from ${exchange}`, error)
        }
      }
      if (prices.length) {
        const bestBuy = prices.reduce((min, p) => p.totalAsk < min.totalAsk ? p : min, prices[0])
        const bestSell = prices.reduce((max, p) => p.totalBid > max.totalBid ? p : max, prices[0])
        rows.value.push({ crypto, bestBuy, bestSell })
      }
    }
  } catch (error) {
    error.value = 'Error general obteniendo precios'
  } finally {
    loading.value = false
  }
}

onMounted(fetchPrices)
</script>

<style scoped>
.main-content {
  padding: 1rem;
}
th, td {
  padding: 0.5rem 1rem;
  text-align: left;
}
small {
  font-size: 0.8rem;
  color: #555;
}
</style>
