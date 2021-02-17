import Vue from 'vue'
import App from './App.vue'
import store from './store'
import {BootstrapVue, IconsPlugin  } from 'bootstrap-vue';
import UpdateHub from './signalR/eventHub'

// Import the styles directly. (Or you could add them via script tags.)
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

Vue.use(BootstrapVue);
Vue.use(IconsPlugin );
Vue.use(UpdateHub);

Vue.config.productionTip = false

new Vue({
  store,
  render: h => h(App)
}).$mount('#app')
