<template>
  <nav>
    <div class="logo">
      LOGO
    </div>
    <router-link to="/account">Войти</router-link>
  </nav>
  <router-view />
</template>

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
}

nav a.router-link-exact-active {
  color: #42b983;
}

.logo {
  float: left;
}
</style>
