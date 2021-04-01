import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SignalRService } from 'src/app/services/signalr.service';
import { PhotoApiService } from '../services/photo-api.service';
import { UserEditModalComponent } from '../modals/user-edit-modal/user-edit-modal.component';
import { PhotoDeleteConfirmModalComponent } from '../modals/photo-delete-confirm-modal/photo-delete-confirm-modal.component';
import { PhotoDescriptionModalComponent } from '../modals/photo-description-modal/photo-description-modal.component';

interface Photo {
  id: number,
  content: string
}

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css'],
  providers: [{ provide: SignalRService }]
})
export class PhotosComponent implements OnInit {

  userName: string = "";
  userEmail: string = "";
  userPicture: string = "";
  photos: Photo[] = [];

  constructor(
    private router: Router,
    private localStorage: LocalStorageService,
    private signalr: SignalRService,
    private photoApiService: PhotoApiService,
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
      self.photoApiService.getUserInfo();
      self.photoApiService.getPhotos();
    });
  }

  loadPhotosContent(photoIds): void {
    var self = this;
    for (var photoIdIndex in photoIds) {
      self.photoApiService.getPhoto(photoIds[photoIdIndex]); // получаем содержимое каждой фотки
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
        // сохраняем данные пользователя
        self.photoApiService.setUserInfo(ref.componentInstance.userName, ref.componentInstance.userEmail, ref.componentInstance.userAbout);
        // сохраняем новую картинку пользователя
        self.photoApiService.setUserPicture(ref.componentInstance.newUserPictureId);
      }
    });
  }

  deletePhoto(photoId): void {
    var self = this;
    var ref = self.modalService.open(PhotoDeleteConfirmModalComponent);
    ref.afterClosed().subscribe(result => {
      if (result) {
        self.photoApiService.deletePhoto(photoId);
      }
    });
  }

  editPhotoDescription(photoId): void {
    var self = this;
    var ref = self.modalService.open(PhotoDescriptionModalComponent);
    ref.componentInstance.setPhotoId(photoId);
    ref.afterClosed().subscribe(result => {
      if (result) {
        ref.componentInstance.save();
      }
    });
  }
}
