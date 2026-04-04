import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: () => import('../pages/Home.vue'),
  },
  {
    path: '/timeline',
    name: 'Timeline',
    component: () => import('../pages/Timeline.vue'),
  },
  {
    path: '/regions/:region',
    name: 'Region',
    component: () => import('../pages/Region.vue'),
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router
