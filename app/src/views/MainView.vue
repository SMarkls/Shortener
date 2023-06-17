<template>
	<div>
		<LinkInput />
		<ShortenLinkList :productList="[
			{ id: 1, token: 'asffsa', fullLink: 'vk.com/makelka', countOfRedirections: 20 },
			{ id: 1, token: 'asffsa', fullLink: 'vk.com/makelka', countOfRedirections: 20 }]" />
	</div>
</template>

<script>
import ShortenLinkList from '@/components/ShortenLink/ShortenLinkList.vue'
import LinkInput from '@/components/LinkInput.vue';

export default {
	name: 'MainView',
	components: {
		ShortenLinkList,
		LinkInput
	}
}
</script>

<style></style>