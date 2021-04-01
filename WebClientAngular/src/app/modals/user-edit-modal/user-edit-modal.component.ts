import { Component, Input, OnInit } from '@angular/core';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SignalRService } from 'src/app/services/signalr.service';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import Utils from 'src/utils';

@Component({
  selector: 'app-user-edit-modal',
  templateUrl: './user-edit-modal.component.html',
  styleUrls: ['./user-edit-modal.component.css'],
  providers: [{ provide: SignalRService }]
})
export class UserEditModalComponent implements OnInit {

  @Input() userName: string = "";
  @Input() userEmail: string = "";
  @Input() userAbout: string = "";
  userPicture: string = "";
  newUserPictureId: string = "";

  constructor(
    private localStorage: LocalStorageService,
    private signalr: SignalRService,
    private photoApiService: PhotoApiService
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
      self.photoApiService.getUserInfo();
    });
  }

  handleFilesUpload(files: FileList): void {
    var self = this;
    var uploadFunc = function (fileBase64) {
      self.photoApiService.loadUserPicture(fileBase64);
    };
    Utils.fileToBase64(files[0], uploadFunc);
  }
}
