import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { SignalRService } from 'src/app/services/signalr.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import Config from 'src/config';
import Utils from 'src/utils';
import { UserEditModalComponent } from '../modals/user-edit-modal/user-edit-modal.component';
import { PhotoDeleteConfirmModalComponent } from '../modals/photo-delete-confirm-modal/photo-delete-confirm-modal.component';

interface Photo {
  id: number,
  content: string
}

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css']
})
export class PhotosComponent implements OnInit {

  userName: string = "";
  userEmail: string = "";
  userPicture: string = "";
  photos: Photo[] = [];

  constructor(
    private router: Router,
    private signalr: SignalRService,
    private localStorage: LocalStorageService,
    private httpClient: HttpClient,
    private modalService: MatDialog) {
  }

  ngOnInit(): void {
    var self = this;
    
    self.signalr.addHandler("GetUserInfoResponse", function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.userName = response.name;
        self.userEmail = response.email;
        self.userPicture = 'data:image/png;base64,' + response.pictureBase64Content;
      }
    });
    
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
        var photo = {
          id: response.photoId,
          content: 'data:image/png;base64,' + response.fileBase64Content
        };
        self.photos.push(photo);
      }
    });
    
    self.signalr.addHandler("UploadPhotosResponse", function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.loadPhotosContent([response.photoId]);
      }
    });

    self.signalr.addHandler("DeletePhotoResponse", function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.photos = self.photos.filter(photo => photo.id !== response.photoId);
      }
    });

    var authData = self.localStorage.getAuthData();
    self.signalr.start(authData.clientId).then(function () {
      self.getUserInfo();
      self.loadPhotosId();
    });
  }

  getUserInfo(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId };
    self.httpClient.post(Config.getUserInfo, postData).toPromise();
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
    for (var photoIdIndex in photoIds) { // получаем содержимое каждой фотки
      var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, photoId: photoIds[photoIdIndex] };
      self.httpClient.post(Config.getPhotoApiPath, postData).toPromise();
    }
  }

  openUserEditModal(): void {
    var self = this;
    var ref = self.modalService.open(UserEditModalComponent);
    ref.afterClosed().subscribe(result => {
      if (result) {
        self.userName = ref.componentInstance.userName;
        self.userEmail = ref.componentInstance.userEmail;
        self.userPicture = ref.componentInstance.userPicture;
        var authData = self.localStorage.getAuthData();
        // сохраняем данные пользователя
        var setUserInfoPostData = {
          login: authData.login,
          token: authData.token,
          clientId: authData.clientId,
          name: ref.componentInstance.userName,
          email: ref.componentInstance.userEmail,
          about: ref.componentInstance.userAbout
        };
        self.httpClient.post(Config.setUserInfo, setUserInfoPostData).toPromise();
        // сохраняем новую картинку пользователя
        var setUserPicturePostData = {
          login: authData.login,
          token: authData.token,
          clientId: authData.clientId,
          newPictureId: ref.componentInstance.newUserPictureId,
        };
        self.httpClient.post(Config.setUserPicture, setUserPicturePostData).toPromise();
      }
    });
  }

  deletePhoto(photoId): void {
    var self = this;
    var ref = self.modalService.open(PhotoDeleteConfirmModalComponent);
    ref.afterClosed().subscribe(result => {
      if (result) {
        var authData = self.localStorage.getAuthData();
        var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, photoId: photoId };
        self.httpClient.post(Config.deletePhotoApiPath, postData).toPromise();
      }
    });
  }
}
