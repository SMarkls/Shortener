export default function (instance) {
	return {
		createLink(payload) {
			if (instance.defaults.headers["Authorization"] == undefined) {
				instance.defaults.headers["Authorization"] = 'Bearer ' + localStorage.getItem('accessToken')
				instance.defaults.headers["RefreshToken"] = localStorage.getItem('refreshToken')
			}
			return instance.post('/ShortenLink/Create', payload)
		},
		updateLink(payload) {
			if (instance.defaults.headers["Authorization"] == undefined) {
				instance.defaults.headers["Authorization"] = 'Bearer ' + localStorage.getItem('accessToken')
				instance.defaults.headers["RefreshToken"] = localStorage.getItem('refreshToken')
			}
			return instance.put('/ShortenLink/Update', payload)
		},
		delete(id) {
			if (instance.defaults.headers["Authorization"] == undefined) {
				instance.defaults.headers["Authorization"] = 'Bearer ' + localStorage.getItem('accessToken')
				instance.defaults.headers["RefreshToken"] = localStorage.getItem('refreshToken')
			}
			return instance.delete('/ShortenLink/Delete?id=' + id)
		},
		getList() {
			if (instance.defaults.headers["Authorization"] == undefined) {
				instance.defaults.headers["Authorization"] = 'Bearer ' + localStorage.getItem('accessToken')
				instance.defaults.headers["RefreshToken"] = localStorage.getItem('refreshToken')
			}
			return instance.get('/ShortenLink/GetList')
		},
		getFullLink(token) {
			return instance.get('/ShortenLink/GetFullLink?token=' + token)
		}
	}
}