<template>
  <div class="form-container">
    <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>
    <Form :initial-values="initialValues" :validation-schema="schema" @submit="handleSubmit">
     <div>
        <label class="label">Exchange</label>
        <Field name="exchange" as="select" class="input-field">
          <option disabled value="">Seleccionar exchange</option>
          <option v-for="exchange in exchangeOptions" :key="exchange" :value="exchange">
            {{ exchange }}
          </option>
        </Field>
        <ErrorMessage name="exchange" class="error-text" />
      </div>

     <div>
        <label class="label">Crypto</label>
        <Field name="cryptoCode" as="select" class="input-field">
          <option disabled value="">Seleccionar crypto</option>
          <option v-for="code in cryptoOptions" :key="code" :value="code">
            {{ code }}
          </option>
        </Field>
        <ErrorMessage name="cryptoCode" class="error-text" />
      </div>


      <div>
        <label class="label">Tipo</label>
        <Field name="action" as="select" class="input-field">
          <option disabled value="">Seleccionar tipo</option>
          <option value="purchase">Compra</option>
          <option value="sale">Venta</option>
        </Field>
        <ErrorMessage name="action" class="error-text" />
      </div>

      <div>
        <label class="label">Cantidad</label>
        <Field name="cryptoAmount" type="number" step="0.0001" class="input-field" />
        <ErrorMessage name="cryptoAmount" class="error-text" />
      </div>

      <div>
        <label class="label">Fecha</label>
        <Field name="dateTime" type="datetime-local" class="input-field" />
        <ErrorMessage name="dateTime" class="error-text" />
      </div>

      <div v-if="initialValues.id">
        <label class="label">Montos</label>
        <Field name="money" type="number" step="0.01" class="input-field" />
        <ErrorMessage name="money" class="error-text" />
      </div>

      <button type="submit" class="btn">{{ submitText }}</button>

      <p v-if="success" class="success-text">Transacción guardada exitosamente</p>
    </Form>
  </div>
</template>

<script setup>
import { Form, Field, ErrorMessage } from 'vee-validate'
import * as yup from 'yup'
import { ref } from 'vue'
import axios from 'axios'

const props = defineProps({
  clientId: Number,
  initialValues: {
    type: Object,
    default: () => ({
      cryptoCode: '',
      action: '',
      cryptoAmount: '',
      money: yup.number().positive().label('Money'),
      dateTime: new Date(Date.now() - new Date().getTimezoneOffset() * 60000).toISOString().slice(0, 16)

    })
  },
  submitText: {
    type: String,
    default: 'Guardar Transacción'
  }
})

const success = ref(false)
const errorMessage = ref('')
const emit = defineEmits(['onSuccess'])
const cryptoOptions = ['BTC', 'ETH', 'USDT', 'USDC', 'DAI']
const exchangeOptions = ['binance', 'buenbit', 'satoshitango']

const schema = yup.object({
  exchange: yup.string().required('Exchange is required'),
  cryptoCode: yup.string().required('Crypto is required'),
  action: yup.string().oneOf(['purchase', 'sale']).required('Action is required'),
  cryptoAmount: yup.number().positive().required('Amount is required'),
  dateTime: yup.string().required('Date is required')
})

const handleSubmit = async (values) => {
  try {
    if (values.id) {
      await axios.patch(`https://localhost:7027/api/Transactions/${values.id}`, {
        cryptoAmount: values.cryptoAmount,
        dateTime: values.dateTime,
        money: values.money
      })
    } else {
      await axios.post('https://localhost:7027/api/Transactions', {
        ...values,
        clientId: props.clientId
      })
    }

    success.value = true
    emit('onSuccess')
  } catch (err) {
    if (err.response?.data) {
      errorMessage.value = err.response.data
    } else {
      console.error('Error al guardar la transacción:', err)
    }
  }
}

</script>
<style scoped></style>
