<template>
	<div>
		<form method="post">
			<input type="text" v-model="link" />
			<button type="submit" @click="submitClicked">Сократить</button>
		</form>
	</div>
</template>

<script>
export default {
	name: 'LinkInput',
	data() {
		return {
			link: ''
		}
	},
	methods: {
		submitClicked(event) {
			if (!this.link.includes("https://")) {
				this.link = 'https://' + this.link;
			}
			if (!this.isUrl(this.link)) {
				return;
			}
		},
		isUrl(url) {
			try {
				new URL(url);
				return true;
			} catch {
				return false;
			}
		}
	}
}
</script>

<style>
form {
	display: block;
}

button {
	border-radius: 1rem;
	width: 15rem;
	height: 2rem;
	margin-inline: 0.5rem;
	margin-top: 1rem;
	font-weight: bold;
}

input {
	padding-left: 10px;
	border-radius: 1rem;
	margin: 0.5em;
	width: 30rem;
	height: 3rem;
}
</style>