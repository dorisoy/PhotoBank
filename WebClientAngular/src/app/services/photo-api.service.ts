import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import Config from 'src/config';

@Injectable({
  providedIn: 'root'
})
export class PhotoApiService {

  constructor(
    private localStorage: LocalStorageService,
    private httpClient: HttpClient
  ) { }

  login(login, password, clientId): void {
    var self = this;
    var postData = { login: login, password: password, clientId: clientId };
    self.httpClient.post(Config.loginApiPath, postData).toPromise();
  }

  getUserInfo(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId };
    self.httpClient.post(Config.getUserInfo, postData).toPromise();
  }

  getPhotos(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId };
    self.httpClient.post(Config.getPhotosApiPath, postData).toPromise();
  }

  getPhoto(photoId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, photoId: photoId };
    self.httpClient.post(Config.getPhotoApiPath, postData).toPromise();
  }

  setUserInfo(userName, userEmail, userAbout): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = {
      login: authData.login,
      token: authData.token,
      clientId: authData.clientId,
      name: userName,
      email: userEmail,
      about: userAbout
    };
    self.httpClient.post(Config.setUserInfo, postData).toPromise();
  }

  loadUserPicture(pictureFile): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, pictureFile: pictureFile };
    self.httpClient.post(Config.loadUserPicture, postData).toPromise();
  }

  setUserPicture(newUserPictureId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = {
      login: authData.login,
      token: authData.token,
      clientId: authData.clientId,
      newPictureId: newUserPictureId,
    };
    self.httpClient.post(Config.setUserPicture, postData).toPromise();
  }

  deletePhoto(photoId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, photoId: photoId };
    self.httpClient.post(Config.deletePhotoApiPath, postData).toPromise();
  }

  uploadPhotos(files): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, files: files };
    self.httpClient.post(Config.uploadPhotosApiPath, postData).toPromise();
  }

  getPhotoAdditionalInfo(photoId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, photoId: photoId };
    self.httpClient.post(Config.getPhotoAdditionalInfo, postData).toPromise();
  }

  setPhotoAdditionalInfo(photoId, additionalInfo): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = {
      login: authData.login,
      token: authData.token,
      clientId: authData.clientId,
      photoId: photoId,
      additionalInfo: additionalInfo
    };
    self.httpClient.post(Config.setPhotoAdditionalInfo, postData).toPromise();
  }
}
