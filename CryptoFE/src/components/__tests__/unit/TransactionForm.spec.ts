import { describe, it, expect } from 'vitest'
import { mount, flushPromises } from '@vue/test-utils'
import TransactionForm from '@/components/TransactionForm.vue'

describe('TransactionForm.vue', () => {
  it('muestra errores de validación si se intenta enviar vacío', async () => {

    const wrapper = mount(TransactionForm, {
      props: { clientId: 1 }
    })

    await wrapper.find('form').trigger('submit')

    await flushPromises()

    const textoPantalla = wrapper.text()
    expect(textoPantalla).toContain('Seleccionar exchange')

  })
})
