<template>
	<div>
		<table>
			<tr>
				<th>
					Токен
				</th>
				<th>
					Полная ссылка
				</th>
				<th>
					Кол-во переходов
				</th>
			</tr>
			<tr v-for="(product, index) in productList" :key="index">
				<ShortenLinkItem :id="product.id" :token="product.token" :fullLink="product.fullLink"
					:countOfRedirections="product.countOfRedirections" />
			</tr>
		</table>
	</div>
</template>

<script>
import ShortenLinkItem from './ShortenLinkItem.vue';

export default {
	props: {
		productList: Array,
		default: []
	},
	components: { ShortenLinkItem }
}
</script>

<style>
table {
	margin: auto;
	margin-top: 2%;
	width: 75%;
	table-layout: fixed;
	text-align: center;
	vertical-align: center;
	border-collapse: collapse;
}

td {
	width: 50%;
	border: 2px solid black;
}

th {
	width: 50%;
	border: 2px solid black;
}
</style>