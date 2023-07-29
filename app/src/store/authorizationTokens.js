export default {
	namespaced: true,
	state: {
	},
	getters: {
		getTokens(state) {
			return [localStorage.getItem('accessToken'), localStorage.getItem('refreshToken')]
		}
	},
	mutations: {
		SET_TOKENS(state, payload) {
			localStorage.setItem('accessToken', payload.accessToken)
			localStorage.setItem('refreshToken', payload.refreshToken)
		}
	}
}