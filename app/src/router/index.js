import { createRouter, createWebHistory } from 'vue-router'
import functions from '@/api/shortenLink'
import instance from '@/api/instance'
import MainView from '@/views/MainView.vue'

const routes = [
  {
    path: '/',
    name: 'Main',
    component: MainView
  },
  {
    path: '/login',
    name: 'Login',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/AuthForm.vue'),
  },
  {
    name: 'ShortenLink',
    path: '/:token',
    async beforeEnter(to, from, next) {
      const link = await getFull(to.path.substring(1))
      window.location.href = link
    }
  }
]

async function getFull(token) {
  console.log('a')
  const api = functions(instance)
  const link = await api.getFullLink(token)
  if (isUrl(link.data)) {
    return link.data
  }
  return '/'
}

function isUrl(link) {
  try {
    new URL(link);
    return true;
  } catch {
    return false;
  }
}

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  mode: 'history',
  routes
})

export default router
