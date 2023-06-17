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
	<!-- Editing mode -->
</template>

<script>

export default {
	name: 'ShortenLinkItem',
	props: ['id', 'token', 'fullLink', 'countOfRedirections']
}
</script>

<style></style>