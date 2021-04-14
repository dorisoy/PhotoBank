import { Injectable } from '@angular/core';
import { SignalRService } from 'src/app/services/signalr.service';

@Injectable()
export class PhotoApiNotifierService {

    private clientId: string = "";
    private signalr: SignalRService

    constructor() {
        this.signalr = new SignalRService();
    }

    setClientId(clientId): void {
        this.clientId = clientId;
    }

    start(): Promise<void> {
        return this.signalr.start(this.clientId);
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

    onGetUserAlbums(handler: any): void {
        this.signalr.addHandler("GetUserAlbumsResponse", handler);
    }

    onCreateUserAlbums(handler: any): void {
        this.signalr.addHandler("CreateUserAlbumsResponse", handler);
    }

    onDeleteUserAlbums(handler: any): void {
        this.signalr.addHandler("DeleteUserAlbumsResponse", handler);
    }

    onGetPhotoAlbums(handler: any): void {
        this.signalr.addHandler("GetPhotoAlbumsResponse", handler);
    }

    onSetPhotoAlbums(handler: any): void {
        this.signalr.addHandler("SetPhotoAlbumsResponse", handler);
    }
}
