<template>
	<td>
		{{ token }}
	</td>
	<td>
		<a :href="fullLink">{{ fullLink }}</a>
	</td>
	<td>
		{{ countOfRedirections }}
	</td>
	<td>
		<svg @click="deleteClicked" viewBox="0 0 50 50" xmlns="http://www.w3.org/2000/svg">
			<line x1="0" x2="50" y1="0" y2="50" />
			<line x1="0" x2="50" y1="50" y2="0" />
		</svg>
	</td>
</template>

<script>

export default {
	name: 'ShortenLinkItem',
	props: ['id', 'token', 'fullLink', 'countOfRedirections'],
	methods: {
		async deleteClicked() {
			await this.$api.shortenLink.delete(this.id);
			location.reload()
		}
	}
}
</script>

<style>
td {
	align-items: center;
}

svg {
	width: 20px;
}

svg line {
	stroke: #000;
	stroke-width: 10;
}

svg:hover line {
	stroke: red;
}
</style>