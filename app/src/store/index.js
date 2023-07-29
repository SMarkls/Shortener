import { createStore } from 'vuex'
import createPersistedState from "vuex-persistedstate";
import authorizationTokens from './authorizationTokens'

// Vue.use(Vuex)

// export default new Vuex.Store({
// 	modules: { authorizationTokens }
// })


export const store = createStore({
	modules: { authorizationTokens },
	plugins: [createPersistedState({ paths: ['authorizationTokens'] })]
})