import { createApp } from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import api from './plugins/api'
import { store } from './store/index'

const app = createApp(App)

app.use(api)
app.use(router)
app.use(store)
app.mount('#app')