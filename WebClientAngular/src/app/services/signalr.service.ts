import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import Config from 'src/config';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  connection: HubConnection;

  constructor() {
  }

  start(clientId: string): void {
    var self = this;
    self.connection = new HubConnectionBuilder().withUrl(Config.host + "hub").build();
    self.connection.start().then(function () {
      self.connection.invoke("Register", clientId);
    });
  }
}
