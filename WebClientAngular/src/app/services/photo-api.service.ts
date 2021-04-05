import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import Config from 'src/config';

@Injectable()
export class PhotoApiService {

  private clientId: string = "";

  constructor(
    private localStorage: LocalStorageService,
    private httpClient: HttpClient
  ) { }

  setClientId(clientId): void {
    this.clientId = clientId;
  }

  login(login, password): void {
    var self = this;
    var postData = { login: login, password: password, clientId: self.clientId };
    self.httpClient.post(Config.loginApiPath, postData).toPromise();
  }

  getUserInfo(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId };
    self.httpClient.post(Config.getUserInfoApiPath, postData).toPromise();
  }
  
  setUserInfo(userName, userEmail, userAbout): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = {
      login: authData.login,
      token: authData.token,
      clientId: self.clientId,
      name: userName,
      email: userEmail,
      about: userAbout
    };
    self.httpClient.post(Config.setUserInfoApiPath, postData).toPromise();
  }

  loadUserPicture(pictureFile): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, pictureFile: pictureFile };
    self.httpClient.post(Config.loadUserPictureApiPath, postData).toPromise();
  }

  setUserPicture(newUserPictureId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = {
      login: authData.login,
      token: authData.token,
      clientId: self.clientId,
      newPictureId: newUserPictureId,
    };
    self.httpClient.post(Config.setUserPictureApiPath, postData).toPromise();
  }

  getPhotos(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId };
    self.httpClient.post(Config.getPhotosApiPath, postData).toPromise();
  }

  getPhoto(photoId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, photoId: photoId };
    self.httpClient.post(Config.getPhotoApiPath, postData).toPromise();
  }

  getPhotoAdditionalInfo(photoId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, photoId: photoId };
    self.httpClient.post(Config.getPhotoAdditionalInfoApiPath, postData).toPromise();
  }

  setPhotoAdditionalInfo(photoId, additionalInfo): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = {
      login: authData.login,
      token: authData.token,
      clientId: self.clientId,
      photoId: photoId,
      additionalInfo: additionalInfo
    };
    self.httpClient.post(Config.setPhotoAdditionalInfoApiPath, postData).toPromise();
  }

  uploadPhotos(files): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, files: files };
    self.httpClient.post(Config.uploadPhotosApiPath, postData).toPromise();
  }

  deletePhoto(photoId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, photoId: photoId };
    self.httpClient.post(Config.deletePhotoApiPath, postData).toPromise();
  }

  getUserAlbums(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId };
    self.httpClient.post(Config.getUserAlbums, postData).toPromise();
  }

  createUserAlbums(newAlbums): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, newAlbums: newAlbums };
    self.httpClient.post(Config.createUserAlbums, postData).toPromise();
  }

  deleteUserAlbums(albumsId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, albumsId: albumsId };
    self.httpClient.post(Config.deleteUserAlbums, postData).toPromise();
  }

  getPhotoAlbums(photoId): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, photoId: photoId };
    self.httpClient.post(Config.getPhotoAlbums, postData).toPromise();
  }

  setPhotoAlbums(photoId, albumsId, albumsName): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var postData = { login: authData.login, token: authData.token, clientId: self.clientId, photoId: photoId, albumsId: albumsId, albumsName: albumsName };
    self.httpClient.post(Config.setPhotoAlbums, postData).toPromise();
  }
}
