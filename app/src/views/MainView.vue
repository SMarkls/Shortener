<template>
	<div>
		<LinkInput />
		<ShortenLinkList :productList="this.productList" />
	</div>
</template>

<script>
import ShortenLinkList from '@/components/ShortenLink/ShortenLinkList.vue'
import LinkInput from '@/components/LinkInput.vue';

export default {
	name: 'MainView',
	data() {
		return {
			productList: []
		}
	},
	components: {
		ShortenLinkList,
		LinkInput
	},
	async created() {
		if (this.$store.getters["authorizationTokens/getTokens"][0] != '') {
			try {
				const list = await this.$api.shortenLink.getList()
				this.productList = list.data;
			}
			catch (err) {
				if (err.response.status == 401) {
					try {
						console.log('ab')
						const oldTokens = this.$store.getters["authorizationTokens/getTokens"]
						const tokens = await this.$api.refreshToken({ accessToken: oldTokens[0], refreshToken: oldTokens[1] })
						this.$store.commit('authorizationTokens/SET_TOKENS', { accessToken: tokens.accessToken, refreshToken: tokens.refreshToken })
					}
					catch (err) {
						this.$store.commit('authorizationTokens/SET_TOKENS', { accessToken: '', refreshToken: '' })
					}
				}
			}
		}
	}
}
</script>

<style></style>