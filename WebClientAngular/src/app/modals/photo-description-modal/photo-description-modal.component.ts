import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from 'src/app/services/signalr.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import Config from 'src/config';

@Component({
  selector: 'app-photo-description-modal',
  templateUrl: './photo-description-modal.component.html',
  styleUrls: ['./photo-description-modal.component.css'],
  providers: [{ provide: SignalRService }]
})
export class PhotoDescriptionModalComponent implements OnInit {

  photoId: number;
  @Input() photoDescription: string = "";

  constructor(
    private signalr: SignalRService,
    private localStorage: LocalStorageService,
    private httpClient: HttpClient
  ) { }

  ngOnInit(): void {
  }

  setPhotoId(photoId): void {
    var self = this;
    self.photoId = photoId;
    self.signalr.addHandler("GetPhotoAdditionalInfoResponse", function (response) {
      if (response && response.success) {
        self.photoDescription = response.additionalInfo.description;
      }
    });
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, photoId: self.photoId };
    self.signalr.start(authData.clientId).then(function () {
      self.httpClient.post(Config.getPhotoAdditionalInfo, postData).toPromise();
    });
  }

  save(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = {
      login: authData.login,
      token: authData.token,
      clientId: authData.clientId,
      photoId: self.photoId,
      additionalInfo: { description: self.photoDescription }
    };
    self.httpClient.post(Config.setPhotoAdditionalInfo, postData).toPromise();
  }
}
