import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from 'src/app/services/signalr.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import Config from 'src/config';
import Utils from 'src/utils';

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css']
})
export class PhotosComponent implements OnInit {

  photos: string[] = []

  constructor(
    private router: Router,
    private signalr: SignalRService,
    private localStorage: LocalStorageService,
    private httpClient: HttpClient) {
  }

  ngOnInit(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();

    self.signalr.addHandler("GetPhotosResponse", function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.loadPhotosContent(response.photoIds);
      }
    });

    self.signalr.addHandler("GetPhotoResponse", function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.photos.push('data:image/png;base64,' + response.fileBase64Content);
      }
    });

    self.signalr.addHandler("UploadPhotosResponse", function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.loadPhotosContent([response.photoId]);
      }
    });

    self.signalr.start(authData.clientId).then(function () {
      self.loadPhotosId();
    });
  }

  loadPhotosId(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId };
    self.httpClient.post(Config.getPhotosApiPath, postData).toPromise();
  }

  loadPhotosContent(photoIds): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    for (var photoIdIndex in photoIds) {
      // получаем содержимое каждой фотки
      var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, photoId: photoIds[photoIdIndex] };
      self.httpClient.post(Config.getPhotoApiPath, postData).toPromise();
    }
  }
}
