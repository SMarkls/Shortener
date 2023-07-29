export default function (instance) {
	return {
		createLink(payload) {
			return instance.post('/ShortenLink/Create', payload)
		},
		updateLink(payload) {
			return instance.put('/ShortenLink/Update', payload)
		},
		delete(id) {
			return instance.delete('/ShortenLink/Delete/' + id)
		},
		getList() {
			if (instance.defaults.headers["Authorization"] == undefined) {
				instance.defaults.headers["Authorization"] = 'Bearer ' + localStorage.getItem('accessToken')
				instance.defaults.headers["RefreshToken"] = localStorage.getItem('refreshToken')
			}
			return instance.get('/ShortenLink/GetList')
		}
	}
}