export default function (instance) {
	return {
		login(payload) {
			return instance.post('/User/Login', payload)
		},
		register(payload) {
			return instance.post('/User/Register', payload)
		},
		logout() {
			instance.defaults.headers['Authorization'] = ''
			instance.defaults.headers['RefreshToken'] = ''
		}
	}
}