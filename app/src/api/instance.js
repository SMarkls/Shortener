import axios from 'axios'

const instance = axios.create({
	baseURL: 'https://localhost:7086',
	headers: {
		'Content-Type': 'application/json',
		'Accept': 'text/plain'
	},
	withCredentials: false,
})

export default instance