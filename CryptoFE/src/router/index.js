import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import ClientsView from '@/views/ClientsView.vue'
import ClientDetailView from '../views/ClientDetailView.vue'
import TransactionsView from '@/views/TransactionsView.vue'
import TransactionDetailView from '@/views/TransactionDetailView.vue'
import MovementsView from '@/views/MovementsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Home',
      component: HomeView
    },
    {
      path: '/prices',
      name: 'CryptoPrices',
      component: () => import('@/views/CryptoPricesView.vue')
    },
    {
      path: '/client',
      name: 'Client',
      component: ClientsView
    },
    {
      path: '/client/:id',
      name: 'ClientDetail',
      component: ClientDetailView
    },
    {
      path: '/client/create',
      name: 'ClientCreate',
      component: () => import('@/views/ClientCreateView.vue')
    },
    {
      path: '/client/edit/:id',
      name: 'ClientEdit',
      component: () => import('@/views/ClientEditView.vue')
    },

    {
      path: '/transaction',
      name: 'Transaction',
      component: TransactionsView
    },
    {
      path: '/transaction/:id',
      name: 'TransactionDetail',
      component: TransactionDetailView
    },
    {
      path: '/transaction/create',
      name: 'TransactionCreate',
      component: () => import('@/views/TransactionCreateView.vue')
    },
    {
      path: '/transaction/edit/:id',
      name: 'TransactionEdit',
      component: () => import('@/views/TransactionEditView.vue')
    },
    {
      path: '/movements',
      name: 'Movements',
      component: MovementsView
    },
    {
      path: '/movements/transactions/:clientId',
      name: 'TransactionListView',
      component: () => import('@/views/TransactionListView.vue')
    },
    {
      path: '/movements/summary/:clientId',
      name: 'TransactionSummaryView',
      component: () => import('@/views/TransactionSummaryView.vue')
    }

  ],
})

export default router
