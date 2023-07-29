import instance from './instance'
import authModule from './auth'
import shortenModule from './shortenLink'

export default {
	auth: authModule(instance),
	shortenLink: shortenModule(instance)
}