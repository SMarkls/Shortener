import axios from 'axios';
const instance = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  timeout: 3000,
  validateStatus: () => true,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default instance;
