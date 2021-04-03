import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import { PhotoApiNotifierService } from 'src/app/services/photo-api-notifier.service';
import { UserEditModalComponent } from 'src/app/modals/user-edit-modal/user-edit-modal.component';
import { PhotoDeleteConfirmModalComponent } from 'src/app/modals/photo-delete-confirm-modal/photo-delete-confirm-modal.component';
import { PhotoDescriptionModalComponent } from 'src/app/modals/photo-description-modal/photo-description-modal.component';
import Utils from 'src/utils';

interface Photo {
  id: number,
  content: string
}

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css'],
  providers: [PhotoApiService, PhotoApiNotifierService]
})
export class PhotosComponent implements OnInit {

  userName: string = "";
  userEmail: string = "";
  userPicture: string = "";
  photos: Photo[] = [];
  
  constructor(
    private router: Router,
    private photoApi: PhotoApiService,
    private photoApiNotifier: PhotoApiNotifierService,
    private modalService: MatDialog) {
      const clientId = Utils.getClientId();
      this.photoApi.setClientId(clientId);
      this.photoApiNotifier.setClientId(clientId);
  }

  ngOnInit(): void {
    var self = this;
    
    self.photoApiNotifier.onGetUserInfoResponse(function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.userName = response.name;
        self.userEmail = response.email;
        self.userPicture = 'data:image/png;base64,' + response.pictureBase64Content;
      }
    });
    
    self.photoApiNotifier.onGetPhotosResponse(function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.loadPhotosContent(response.photoIds);
      }
    });
    
    self.photoApiNotifier.onGetPhotoResponse(function (response) {
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
    
    self.photoApiNotifier.onUploadPhotosResponse(function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.loadPhotosContent([response.photoId]);
      }
    });

    self.photoApiNotifier.onDeletePhotoResponse(function (response) {
      if (!response || !response.success) {
        self.router.navigate(['/']);
      } else {
        self.photos = self.photos.filter(photo => photo.id !== response.photoId);
      }
    });

    self.photoApiNotifier.start().then(function () {
      self.photoApi.getUserInfo();
      self.photoApi.getPhotos();
    });
  }

  loadPhotosContent(photoIds): void {
    var self = this;
    for (var photoIdIndex in photoIds) {
      self.photoApi.getPhoto(photoIds[photoIdIndex]); // получаем содержимое каждой фотки
    }
  }

  openUserEditModal(): void {
    var self = this;
    var modal = self.modalService.open(UserEditModalComponent);
    modal.afterClosed().subscribe(result => {
      if (result) {
        self.userName = modal.componentInstance.userName;
        self.userEmail = modal.componentInstance.userEmail;
        self.userPicture = modal.componentInstance.userPicture;
        // сохраняем данные пользователя
        self.photoApi.setUserInfo(modal.componentInstance.userName, modal.componentInstance.userEmail, modal.componentInstance.userAbout);
        // сохраняем новую картинку пользователя
        self.photoApi.setUserPicture(modal.componentInstance.newUserPictureId);
      }
    });
  }

  deletePhoto(photoId): void {
    var self = this;
    var modal = self.modalService.open(PhotoDeleteConfirmModalComponent);
    modal.afterClosed().subscribe(result => {
      if (result) {
        self.photoApi.deletePhoto(photoId);
      }
    });
  }

  editPhotoDescription(photoId): void {
    var self = this;
    var modal = self.modalService.open(PhotoDescriptionModalComponent);
    modal.componentInstance.setPhotoId(photoId);
    modal.afterClosed().subscribe(result => {
      if (result) {
        modal.componentInstance.save();
      }
    });
  }
}
