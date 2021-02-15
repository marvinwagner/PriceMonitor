import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'
import axios from 'axios'

const vuexLocal = new VuexPersistence({
  storage: window.localStorage
})

Vue.use(Vuex)

const state = {
  items: {    
    id: '',
    name: '', 
    url: '',
    currentInCashValue: 0,
    currentNormalValue: 0,
    currentFullValue: 0,
    isAvailable: false
  },
  history: [],
  errors: []
}

const mutations = {
  addHistory (state, evt) {
    state.history.push(evt)
  },
  set (state, [variable, value]) {
    state[variable] = value
  },
  handleErrors(state, e) {
    if (e.response?.data)
      if (e.response?.data === Array)
        state.errors = e.response.data
      else
        state.errors = [e.response.data]
    else
      state.errors = ['An error has occurred']
  },
  clearErrors(state) {
    state.errors = []
  }
}

const actions = {
  loadItems({commit}) {
    commit('clearErrors')

    axios
      .get(`${process.env.VUE_APP_API_URL}/monitor`)
      .then(response => {
        commit('set', [ 'items', response.data ])
      })
      .catch(e => {
        commit('handleErrors', e)
      })
  },
  newItem({ dispatch, commit }, item) {
    commit('clearErrors')

    if (item.id) 
      return axios.put(`${process.env.VUE_APP_API_URL}/monitor`, item)
      .then(() => {
        dispatch('loadItems')
      })
      .catch(e => {
        commit('handleErrors', e)
      })    
    else
      return axios.post(`${process.env.VUE_APP_API_URL}/monitor`, item)
      .then(() => {
        dispatch('loadItems')
      })
      .catch(e => {
        commit('handleErrors', e)
      })
  },


  loadMonthlyPoints({ state, commit }) {
    commit('clearErrors')

    axios.get(`${process.env.VUE_APP_API_URL}/points`, { 
      params : { start: state.points.filter.start, end: state.points.filter.end },
      headers: { Authorization: `Bearer ${ state.user.token }` }     
    })
    .then(response => {
      commit('set', [ 'monthlyPoints', response.data ])
    })
    .catch(e => {
      commit('handleErrors', e)
    })
  },
  createPoint({ dispatch, commit }, time) {
    commit('clearErrors')

    axios.defaults.withCredentials = true
    axios.post(`${process.env.VUE_APP_API_URL}/points`, { time }, {headers: { Authorization: `Bearer ${ state.user.token }` }} )
    .then(() => {
      dispatch('loadMonthlyPoints')
    })
    .catch(e => {
      commit('handleErrors', e)
    })
  },
  deletePoint({ dispatch, commit }, id) {
    commit('clearErrors')

    console.log(id)
    axios.delete(`${process.env.VUE_APP_API_URL}/points/${id}`, {
      headers: { Authorization: `Bearer ${ state.user.token }` } 
    })
    //axios.defaults.withCredentials = true
    //axios.delete(`${process.env.VUE_APP_API_URL}/points`, { data: { id } } )
    .then(() => {
      dispatch('loadMonthlyPoints')
    })
    .catch(e => {
      commit('handleErrors', e)
    })
  }
}

export default new Vuex.Store({
  state,
  mutations,
  actions,
  plugins: [vuexLocal.plugin]
})