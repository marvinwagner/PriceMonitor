<template>
  <div class="container">
      <div class="py-5 text-center">
        <h2>Checkout form</h2>
        <p class="lead">Below is an example form built.</p>
      </div>

      <div class="row">
        <div class="col-md-4 order-md-2 mb-4">

          <h4 class="d-flex justify-content-between align-items-center mb-3">
            <span class="text-muted">Last price changes</span>
            <span class="badge badge-secondary badge-pill">{{this.$store.state.history.length}}</span>
          </h4>
          <ul class="list-group mb-3">
            
            <li class="list-group-item d-flex justify-content-between lh-condensed"
              v-for="evt in this.$store.state.history" :key="evt.id"
            >
              <div>
                <h6 class="my-0">{{evt.name}}</h6>
                <!-- <small class="text-muted">Brief description</small> -->
              </div>
              <span class="text-muted">{{evt.inCashValue}} - {{evt.newInCashValue}}</span>
            </li>

          </ul>

          <form class="card p-2">
            <div class="input-group">
              <input type="text" class="form-control" placeholder="Promo code">
              <div class="input-group-append">
                <button type="submit" class="btn btn-secondary">Redeem</button>
              </div>
            </div>
          </form>
        </div>
        <div class="col-md-8 order-md-1">
          <h4 class="mb-3">Monitor a new product</h4>
          <form class="needs-validation"  v-on:submit.prevent="add">
            <div class="row">
              <div class="col-md-6 mb-3">
                <label for="productName">Product name</label>
                <input type="text" class="form-control" id="productName" v-model="name" placeholder="" value="" required>
                <div class="invalid-feedback">
                  Invalid product name.
                </div>
              </div>
              <div class="col-md-6 mb-3">
                <label for="productUrl">Product url</label>
                <input type="text" class="form-control" id="productUrl" v-model="url" placeholder="" value="" required>
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
                  <h5 class="card-title">{{item.name}}</h5>
                  <a :href="item.url" class="btn btn-primary" target="_blank">Go to store</a>
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
  name: 'HelloWorld',
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
    add() {
      this.$store.dispatch('newItem', { name: this.name, url: this.url })
    },
    onPriceUpdated(evt) {
      var exists = this.$store.state.history.some(function(field) {
        return field.createdAt == evt.createdAt
      });

      console.log(exists);
      if (exists)
        return;

      var item = this.$store.state.items.find(i => i.id == evt.itemId)
      var history = {
        ...item,
        newInCashValue: evt.inCashValue,
        newNormalValue: evt.normalValue,
        newFullValue: evt.fullValue,
        createdAt: evt.createdAt
      }
      console.log(history)
      this.$store.commit('addHistory', history);
    }
  },
  created () {
    console.log(this.$store.state.items)
    this.$store.dispatch('loadItems');
  },
  mounted () {    
    this.$store.state.history = []
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
a {
  color: #42b983;
}
</style>
