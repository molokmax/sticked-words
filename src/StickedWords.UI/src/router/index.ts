import { createRouter, createWebHistory } from 'vue-router'
import FlashCardListView from '@/views/FlashCardListView.vue'
import AddFlashCardView from '@/views/AddFlashCardView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: FlashCardListView,
    },
    {
      path: '/add',
      name: 'add-card',
      component: AddFlashCardView,
    }
    // {
    //   path: '/about',
    //   name: 'about',
    //   // route level code-splitting
    //   // this generates a separate chunk (About.[hash].js) for this route
    //   // which is lazy-loaded when the route is visited.
    //   component: () => import('../views/AboutView.vue'),
    // },
  ],
})

export default router
