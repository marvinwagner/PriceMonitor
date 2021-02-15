
// import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'
// export default {
//   install () {
//     const connection = new HubConnectionBuilder()
//       .withUrl(`${process.env.VUE_APP_API_URL}/updates`)
//       .configureLogging(LogLevel.Information)
//       .build()

//       connection.on('PriceUpdated', (value) => {
//         console.log('price changed received', value)
//         // updateHub.$emit('price-updated', { value })
//       })

//     connection.start()
//   }
// }

import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install (Vue) {
    // use a new Vue instance as the interface for Vue components to receive/send SignalR events
    // this way every component can listen to events or send new events using this.$questionHub
    const updateHub = new Vue()
    Vue.prototype.$updateHub = updateHub

    // Provide methods to connect/disconnect from the SignalR hub
    let connection = null
    let startedPromise = null
    let manuallyClosed = false

    Vue.prototype.startSignalR = () => {

      connection = new HubConnectionBuilder()
        .withUrl(`${process.env.VUE_APP_API_URL}/updates`)
        .configureLogging(LogLevel.Information)
        .build()

      // Forward hub events through the event, so we can listen for them in the Vue components
      connection.on('PriceUpdated', (value) => {
        updateHub.$emit('price-updated', value)
      })

      // You need to call connection.start() to establish the connection but the client wont handle reconnecting for you!
      // Docs recommend listening onclose and handling it there.
      // This is the simplest of the strategies
      function start () {
        startedPromise = connection.start()
          .catch(err => {
            console.error('Failed to connect with hub', err)
            return new Promise((resolve, reject) => setTimeout(() => start().then(resolve).catch(reject), 5000))
          })
        return startedPromise
      }
      connection.onclose(() => {
        if (!manuallyClosed) start()
      })

      // Start everything
      manuallyClosed = false
      start()
    }
    Vue.prototype.stopSignalR = () => {
      if (!startedPromise) return

      manuallyClosed = true
      return startedPromise
        .then(() => connection.stop())
        .then(() => { startedPromise = null })
    }

    // Provide methods for components to send messages back to server
    // Make sure no invocation happens until the connection is established
    // questionHub.questionOpened = (questionId) => {
    //   if (!startedPromise) return

    //   return startedPromise
    //     .then(() => connection.invoke('JoinQuestionGroup', questionId))
    //     .catch(console.error)
    // }
    // questionHub.questionClosed = (questionId) => {
    //   if (!startedPromise) return

    //   return startedPromise
    //     .then(() => connection.invoke('LeaveQuestionGroup', questionId))
    //     .catch(console.error)
    // }
    // questionHub.sendMessage = (message) => {
    //   if (!startedPromise) return

    //   return startedPromise
    //     .then(() => connection.invoke('SendLiveChatMessage', message))
    //     .catch(console.error)
    // }
  }
}