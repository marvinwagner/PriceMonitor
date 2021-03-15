<template>
  <div class="container">
      <div class="row">
        <div class="col-md-6 order-md-2 mb-4">

          <h4 class="d-flex justify-content-between align-items-center mb-3">
            <span class="text-muted">Last price changes</span>
            <span class="badge badge-secondary badge-pill">{{this.$store.state.history.length}}</span>
          </h4>
          <ul class="list-group mb-3">

            <li class="list-group-item d-flex flex-column justify-content-between lh-condensed"
              v-for="evt in this.$store.state.history" :key="evt.id + evt.createdAt"
            >
              <div>
                <h6 class="my-0">{{evt.name}}</h6>
                <!-- <small class="text-muted">Brief description</small> -->
              </div>

              <div v-if="evt.isAvailable" class="d-flex justify-content-between lh-condensed">
                
                <div class="d-flex flex-column">
                  <small class="text-muted">In Cash</small>
                  <small class="text-muted">R$ {{ evt.currentInCashValue }}</small>
                  <span :class="valueColor(evt.currentInCashValue, evt.newInCashValue)">
                    <BIconArrowUp v-if="higherInCashValue(evt)"/><BIconArrowDown v-if="!higherInCashValue(evt)"/> R$ {{ evt.newInCashValue }}
                  </span>

                </div>
                <div class="d-flex flex-column">
                  <small class="text-muted">Normal</small>
                  <small class="text-muted">R$ {{ evt.currentNormalValue }}</small>
                  <span :class="valueColor(evt.currentNormalValue, evt.newNormalValue)"> 
                    <BIconArrowUp v-if="higherNormalValue(evt)"/><BIconArrowDown v-if="!higherNormalValue(evt)"/> R$ {{ evt.newNormalValue }}
                  </span>

                </div>
                <div class="d-flex flex-column ">
                  <small class="text-muted">Full</small>
                  <small class="text-muted">R$ {{ evt.currentFullValue }}</small>
                  <span :class="valueColor(evt.currentFullValue, evt.newFullValue)" v-show="evt.newFullValue > 0">
                    <BIconArrowUp v-if="higherFullValue(evt)"/><BIconArrowDown v-if="!higherFullValue(evt)"/> R$ {{ evt.newFullValue }}
                  </span>

                </div>
              </div>
              <div v-else>
                Unavailable
              </div>

              <div >
                <span class="small">{{ new Date(evt.createdAt).toLocaleString() }}</span>
              </div>
            </li>

          </ul>

        </div>
        <div class="col-md-6 order-md-1">
          <h4 class="mb-3">Monitor a new product</h4>
          <form class="needs-validation"  v-on:submit.prevent="add">
            <div class="row">
              <div class="col-md-6 mb-3">
                <label for="productName">Product name</label>
                <input type="text" class="form-control" id="productName" v-model="name" placeholder="" maxlength="300" value="" required>
                <div class="invalid-feedback">
                  Invalid product name.
                </div>
              </div>
              <div class="col-md-6 mb-3">
                <label for="productUrl">Product url</label>
                <input type="text" class="form-control" id="productUrl" v-model="url" placeholder=""  maxlength="300" value="" required>
                <div class="invalid-feedback">
                  Invalid product url.
                </div>
              </div>
            </div>
            
            <button class="btn btn-primary btn btn-block" type="submit">Add product</button>
            <hr class="mb-4">
          </form>

          <div class="row">
            
            <div class="col-sm-12" v-for="item in this.$store.state.items" :key="item.id">
              <div class="card">
                <div class="card-body">
                  <BIconPencil />
                  <div class="d-flex justify-content-between align-middle">
                    <p class="text-left middle">{{ item.name }}</p>
                    
                    <a class="btn-sm btn-outline-info" target="_blank" :href="item.url"><BIconReply /></a>
                  </div>

                  <div class="d-flex justify-content-between mt-3">
                        <div class="d-flex flex-column">
                        <small class="text-muted">In Cash</small>
                        <small class="text-muted">R$ {{ item.currentInCashValue }}</small>
                      </div>
                      <div class="d-flex flex-column">
                        <small class="text-muted">Normal</small>
                        <small class="text-muted">R$ {{ item.currentNormalValue }}</small>
                      </div>  
                      <div class="d-flex flex-column">
                        <small class="text-muted">Full</small>
                        <small class="text-muted">R$ {{ item.currentFullValue }}</small>
                      </div>

                  </div>
                  

                </div>
              </div>
            </div>

          </div>
        </div>
      </div>

      <!-- <footer class="my-5 pt-5 text-muted text-center text-small">
        <p class="mb-1">© 2021 Marvin Wagner</p>
        <ul class="list-inline">
          <li class="list-inline-item"><a href="#">Privacy</a></li>
          <li class="list-inline-item"><a href="#">Terms</a></li>
          <li class="list-inline-item"><a href="#">Support</a></li>
        </ul>
      </footer> -->
    </div>
</template>

<script>

export default {
  name: 'Monitor',
  props: {
    msg: String
  },
  data() {
    return {
      name: '',
      url:'',
    }
  },
  methods: {
    higherInCashValue(evt) {
      return evt.newInCashValue > evt.currentInCashValue
    },
    higherNormalValue(evt) {
      return evt.newNormalValue > evt.currentNormalValue
    },
    higherFullValue(evt) {
      return evt.newFullValue > evt.currentFullValue
    },
    valueColor(old, cur) {
      if (cur > old && old > 0) 
        return 'text-danger'
      else if (cur < old && old > 0)
        return 'text-success'
      else
        return ''
    },
    formatMoney(currency, value) {
      return `${currency} ${value}`
    },
    add() {
      this.$store.dispatch('newItem', { name: this.name, url: this.url })
    },
    onPriceUpdated(evt) {
      var exists = this.$store.state.history.some(function(field) {
        return field.createdAt == evt.createdAt
      });

      console.log(evt);
      if (exists)
        return;

      var item = this.$store.state.items.find(i => i.id == evt.itemId)

      var history = {
        ...item,
        newInCashValue: evt.inCashValue,
        newNormalValue: evt.normalValue,
        newFullValue: evt.fullValue,
        createdAt: evt.createdAt,
        available: evt.isAvailable
      }
      history.currentInCashValue = item.currentInCashValue;
      history.currentNormalValue = item.currentNormalValue;
      history.currentFullValue = item.currentFullValue;

      item.currentInCashValue = evt.inCashValue;
      item.currentNormalValue = evt.normalValue;
      item.currentFullValue = evt.fullValue;
      item.isAvailable = evt.isAvailable;
      
      console.log(history)
      this.$store.commit('addHistory', history);
    }
  },
  created () {
    this.$store.commit('clearOldHistory');
    this.$store.dispatch('loadItems');
  },
  mounted () {    
    this.startSignalR()
    this.$updateHub.$on('price-updated', this.onPriceUpdated);
  },
  beforeUnmount() {
    this.$updateHub.$off('price-updated');
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
p.middle {
  line-height: 14px;
  margin: 0px;
  display: table-cell;
  vertical-align: middle;
  padding: 10px;
}
</style>
