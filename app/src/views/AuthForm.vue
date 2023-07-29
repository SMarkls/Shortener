<template>
	<div>
		<h1>{{ getHeader() }}</h1>
		<form @submit.prevent="submitHandler" method="post">
			<div>
				<label for="login">Логин</label>
				<input id="login" :class="v$.login.$errors.length > 0 ? 'incorrect' : ''" type="text" v-model="login"
					@mouseenter="mouseEnterHandler" />
				<div>
					<small v-if="v$.login.$dirty">{{ getLoginErrorMessage() }}</small>
				</div>
			</div>
			<div>
				<label for="password">Пароль</label>
				<input id="password" :class="v$.password.$errors.length > 0 ? 'incorrect' : ''" type="password"
					v-model="password" @mouseenter="mouseEnterHandler" />
				<div>
					<small v-if="v$.password.$dirty">{{ getPasswordErrorMessage() }}</small>
				</div>
			</div>
			<div v-if="isRegister">
				<label for="acceptPassword">Повторите пароль</label>
				<input id="acceptPassword" type="password" v-model="acceptPassword" />
				<div>
					<small v-if="v$.acceptPassword.$dirty">{{ getAcceptPasswordMessage() }}</small>
				</div>
			</div>
			<div v-if="!isRegister">
				<button type="submit">Войти</button>
				<button type="button" @click="switchButton">Нет аккаунта?</button>
			</div>
			<div v-else>
				<button type="submit">Регистрация</button>
				<button type="button" @click="switchButton">Уже есть аккаунт?</button>
			</div>
		</form>
		<p>{{ errorMessage }}</p>
	</div>
</template>

<script>
import useValidate from '@vuelidate/core'
import { required, minLength, maxLength, sameAs } from '@vuelidate/validators'
export default {
	name: 'AuthForm',
	data() {
		return {
			v$: useValidate(),
			login: '',
			password: '',
			acceptPassword: '',
			isRegister: false,
			errorMessage: ''
		}
	},
	mounted() {
		if (this.$store.getters['authorizationTokens/getTokens'][0] != '')
			this.$router.push('/')
	},
	methods: {
		getHeader() {
			return this.isRegister ? 'Регистрация' : 'Авторизация'
		},
		switchButton() {
			this.isRegister = !this.isRegister
			this.v$.$reset()
		},
		mouseEnterHandler(event) {
			let elem = event.target
			if (elem.classList.contains('incorrect')) {
				elem.classList.remove('incorrect')
			}
		},
		async submitHandler() {
			if (this.isRegister && this.v$.$invalid) {
				this.v$.$touch()
				return
			}
			else if (this.v$.login.$invalid || this.v$.password.$invalid) {
				this.v$.$touch()
				return
			}

			let tokens = {}
			if (this.isRegister) {
				try {
					tokens = await this.$api.auth.register({ nickname: this.login, password: this.password, acceptPassword: this.acceptPassword })
				}
				catch (err) {
					if (err.response.status == 400) {
						this.errorMessage = 'Ошибка сервера, попробуйте позже!'
					}
					return
				}
			}
			else {
				try {
					tokens = await this.$api.auth.login({ nickname: this.login, password: this.password })
				}
				catch (err) {
					if (err.response.status == 400) {
						this.errorMessage = 'Неверный логин или пароль!'
					}
					return
				}
			}
			this.$store.commit('authorizationTokens/SET_TOKENS', { accessToken: tokens.data.accessToken, refreshToken: tokens.data.refreshToken })
			this.$emit('successfullyLogined')
			setTimeout(this.$router.go, 100, 0);
			return
		},
		getLoginErrorMessage() {
			if (this.v$.login.required.$invalid) {
				return 'Поле не должно быть пустым'
			}
			if (this.v$.login.minLength.$invalid) {
				return 'Длина логина должна быть не менее 4 символов'
			}
			if (this.v$.login.maxLength.$invalid) {
				return 'Длина логина должна быть не более 20 символов'
			}
		},
		getPasswordErrorMessage() {
			if (this.v$.password.required.$invalid) {
				return 'Поле не должно быть пустым'
			}
			if (this.v$.password.minLength.$invalid) {
				return 'Длина пароля должна быть не менее 4 символов'
			}
		},
		getAcceptPasswordMessage() {
			if (this.v$.acceptPassword.sameAs.$invalid) {
				return 'Пароли не совпадают'
			}
		}
	},
	validations() {
		return {
			login: {
				required,
				minLength: minLength(4),
				maxLength: maxLength(20)
			},
			password: {
				required,
				minLength: minLength(4),
			},
			acceptPassword: {
				sameAs: sameAs(this.password)
			}
		}
	}
}
</script>

<style>
input {
	border-radius: 1rem;
	margin: 0.5em;
	width: 30rem;
	height: 3rem;
}

label {
	display: block;
	font-size: 1em;
	margin-block-start: 0.67em;
	margin-block-end: 0.67em;
	margin-inline-start: 0px;
	margin-inline-end: 0px;
	font-weight: bold;
}

button {
	border-radius: 1rem;
	width: 15rem;
	height: 2rem;
	margin-inline: 0.5rem;
	margin-top: 1rem;
	font-weight: bold;
}

small {
	width: 100%;
	color: darkred;
	font-weight: bolder;
	font-size: medium;
}

.incorrect {
	border-color: red;
	border-width: 5px;
}

p {
	color: red;
}
</style>