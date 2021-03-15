<template>
  <div id="app">
    <Monitor msg="Welcome to Your Vue.js App"/>
  </div>
</template>

<script>
import Monitor from './components/Monitor.vue'

export default {
  name: 'App',
  components: {
    Monitor
  }
}
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
