import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from 'src/app/services/signalr.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
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
    private router: Router,
    private signalr: SignalRService,
    private localStorage: LocalStorageService,
    private httpClient: HttpClient) {
  }

  ngOnInit(): void {
    var self = this;
    self.onLoginResponse();
  }

  onLoginResponse(): void {
    var self = this;
    self.signalr.start(self.clientId);
    self.signalr.connection.on("LoginResponse", function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.localStorage.setAuthData({ login: self.login, token: response.token, clientId: self.clientId });
        self.router.navigate(['/photos']);
      }
    });
  }

  sendAuth(): void {
    var self = this;
    var postData = { login: self.login, password: self.password, clientId: self.clientId };
    self.httpClient.post(Config.loginApiPath, postData).toPromise();
  }
}
