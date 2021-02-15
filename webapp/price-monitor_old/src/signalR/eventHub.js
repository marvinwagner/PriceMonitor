import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'
 
const connection = new HubConnectionBuilder()
  .withUrl('https://localhost:5001/updates')
  .configureLogging(LogLevel.Information)
  .build()
 
connection.start()