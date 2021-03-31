import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SignalRService } from 'src/app/services/signalr.service';
import Config from 'src/config';
import Utils from 'src/utils';

@Component({
  selector: 'app-user-edit-modal',
  templateUrl: './user-edit-modal.component.html',
  styleUrls: ['./user-edit-modal.component.css']
})
export class UserEditModalComponent implements OnInit {

  @Input() userName: string = "";
  @Input() userEmail: string = "";
  @Input() userAbout: string = "";
  userPicture: string = "";
  newUserPictureId: string = "";

  constructor(
    private signalr: SignalRService,
    private localStorage: LocalStorageService,
    private httpClient: HttpClient
  ) { }

  ngOnInit(): void {
    var self = this;

    self.signalr.addHandler("GetUserInfoResponse", function (response) {
      if (response && response.success) {
        self.userName = response.name;
        self.userEmail = response.email;
        self.userAbout = response.about;
        self.userPicture = 'data:image/png;base64,' + response.pictureBase64Content;
      }
    });

    self.signalr.addHandler("LoadUserPictureResponse", function (response) {
      if (response && response.success) {
        self.userPicture = 'data:image/png;base64,' + response.pictureBase64Content;
        self.newUserPictureId = response.newPictureId;
      }
    });

    var authData = self.localStorage.getAuthData();
    self.signalr.start(authData.clientId).then(function () {
      self.getUserInfo();
    });
  }

  getUserInfo(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId };
    self.httpClient.post(Config.getUserInfo, postData).toPromise();
  }

  handleFilesUpload(files: FileList): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var uploadFunc = function (fileBase64) {
      var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, pictureFile: fileBase64 };
      self.httpClient.post(Config.loadUserPicture, postData).toPromise();
    };
    Utils.fileToBase64(files[0], uploadFunc);
  }
}
