import axios from 'axios'

const urlParts = location.href.split(':')
urlParts[urlParts.length - 1] = 443
const instance = axios.create({
	baseURL: urlParts.join(':'),
	headers: {
		'Content-Type': 'application/json',
		'Accept': 'text/plain'
	},
	withCredentials: false,
})

export default instance