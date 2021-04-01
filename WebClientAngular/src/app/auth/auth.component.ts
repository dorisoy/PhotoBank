import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SignalRService } from 'src/app/services/signalr.service';
import { PhotoApiService } from '../services/photo-api.service';
import Utils from 'src/utils';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
  providers: [{ provide: SignalRService }]
})
export class AuthComponent implements OnInit {

  clientId: string = Utils.getClientId();
  @Input()  login: string = "vinge";
  @Input()  password: string = "12345";

  constructor(
    private router: Router,
    private localStorage: LocalStorageService,
    private signalr: SignalRService,
    private photoApiService: PhotoApiService) {
  }

  ngOnInit(): void {
    var self = this;
    self.onLoginResponse();
  }

  onLoginResponse(): void {
    var self = this;
    self.signalr.addHandler("LoginResponse", function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.localStorage.setAuthData({ login: self.login, token: response.token, clientId: self.clientId });
        self.router.navigate(['/photos']);
      }
    });
    self.signalr.start(self.clientId);
  }

  sendAuth(): void {
    var self = this;
    self.photoApiService.login(self.login, self.password, self.clientId);
  }
}
