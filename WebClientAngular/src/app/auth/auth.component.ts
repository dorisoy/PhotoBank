import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from 'src/app/services/signalr.service';
import Config from 'src/config';
import Utils from 'src/utils';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {

  clientId: string = Utils.getClientId();
  login: string = "vinge";
  password: string = "12345";

  constructor(
    private signalR: SignalRService,
    private httpClient: HttpClient) {
  }

  ngOnInit(): void {
    var self = this;
    self.signalR.start(self.clientId);
    self.signalR.connection.on("LoginResponse", function (response) {
      if (response.success) {
        alert('login signal r');
        //self.$cookies.set('login', self.login);
        //self.$cookies.set('token', response.token);
        //self.$router.push('photos');
      }
    });
  }

  send(): void {
    var self = this;
    var postData = { clientId: self.clientId, login: self.login, password: self.password };
    self.httpClient.post(Config.loginApiPath, postData).toPromise();
  }
}
