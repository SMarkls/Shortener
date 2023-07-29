<template>
  <nav>
    <div class="logo">
      LOGO
    </div>
    <router-link to="/login" v-if="!this.isLogined">Войти</router-link>
    <a @click="logoutClicked" v-else>Выйти</a>
  </nav>
  <router-view />
</template>


<script>

export default {
  name: 'App',
  data() {
    return {
      isLogined: false
    }
  },
  mounted() {
    this.checkIsLogined()
  },
  watch: {
    '$route.path'() {
      if (this.$route.fullPath == '/') {
        this.checkIsLogined();
      }
    }
  },
  methods: {
    logoutClicked(event) {
      this.$store.commit('authorizationTokens/SET_TOKENS', { accessToken: '', refreshToken: '' })
      this.$api.auth.logout()
      this.isLogined = false
      location.reload()
    },
    checkIsLogined() {
      if (localStorage.getItem('accessToken') != '' && localStorage.getItem('accessToken') != undefined) {
        this.isLogined = true;
      }
    }
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  display: block;
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
  float: right;
  text-decoration: underline;
  cursor: pointer;
}

nav a.router-link-exact-active {
  color: #42b983;
}

.logo {
  float: left;
}
</style>
