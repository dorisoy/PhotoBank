import { Injectable } from '@angular/core';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SignalRService } from 'src/app/services/signalr.service';

@Injectable()
export class PhotoApiNotifierService {

  private signalr: SignalRService

  constructor(
    private localStorage: LocalStorageService
  ) {
    this.signalr = new SignalRService();
  }

  start(clientId?: string): Promise<void> {
    var self = this;
    if (!clientId) {
      var authData = self.localStorage.getAuthData();
      clientId = authData.clientId
    };
    return self.signalr.start(clientId);
  }

  onLoginResponse(handler: any): void {
    this.signalr.addHandler("LoginResponse", handler);
  }

  onGetUserInfoResponse(handler: any): void {
    this.signalr.addHandler("GetUserInfoResponse", handler);
  }

  onLoadUserPictureResponse(handler: any): void {
    this.signalr.addHandler("LoadUserPictureResponse", handler);
  }

  onGetPhotosResponse(handler: any): void {
    this.signalr.addHandler("GetPhotosResponse", handler);
  }

  onGetPhotoResponse(handler: any): void {
    this.signalr.addHandler("GetPhotoResponse", handler);
  }

  onGetPhotoAdditionalInfoResponse(handler: any): void {
    this.signalr.addHandler("GetPhotoAdditionalInfoResponse", handler);
  }

  onUploadPhotosResponse(handler: any): void {
    this.signalr.addHandler("UploadPhotosResponse", handler);
  }

  onDeletePhotoResponse(handler: any): void {
    this.signalr.addHandler("DeletePhotoResponse", handler);
  }
}
