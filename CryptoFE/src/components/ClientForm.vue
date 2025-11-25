<template>
  <div class="form-container">
    <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>
    <Form :initial-values="initialValues" :validation-schema="schema" @submit="handleSubmit">
      <div>
        <label class="label">Nombre y Apellido</label>
        <Field name="name" type="text" class="input-field" />
        <ErrorMessage name="name" class="error-text" />
      </div>

      <div>
        <label class="label">Email</label>
        <Field name="email" type="email" class="input-field" />
        <ErrorMessage name="email" class="error-text" />
      </div>

      <button type="submit" class="btn">{{ submitText }}</button>

      <p v-if="success" class="success-text">Cliente guardado exitosamente</p>
    </Form>
  </div>
</template>

<script setup>
import { Form, Field, ErrorMessage } from 'vee-validate'
import * as yup from 'yup'
import { ref } from 'vue'
import axios from 'axios'

const props = defineProps({
  initialValues: {
    type: Object,
    default: () => ({ name: '', email: '' })
  },
  onSuccess: {
    type: Function,
    default: null
  },
  submitText: {
    type: String,
    default: 'Save Client'
  },
  clientId: {
    type: Number,
    default: null
  }
})

const success = ref(false)
const errorMessage = ref('')
const emit = defineEmits(['onSuccess'])


const schema = yup.object({
  name: yup.string().required('Name is required'),
  email: yup.string().email('Invalid email').required('Email is required')
})

const handleSubmit = async (values) => {
  try {
    if (props.clientId) {
      await axios.patch(`https://localhost:7027/api/Clients/${props.clientId}`, values)
    } else {
      await axios.post('https://localhost:7027/api/Clients', values)
    }

    success.value = true
    emit('onSuccess')
  } catch (err) {
   if (err.response?.data) {
      errorMessage.value = err.response.data
    } else {
      console.error('Error saving client:', err)
    }
  }
}
</script>

<style scoped></style>
