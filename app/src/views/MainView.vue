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
			const list = await this.$api.shortenLink.getList()
			this.productList = list.data;
		}
	}
}
</script>

<style></style>