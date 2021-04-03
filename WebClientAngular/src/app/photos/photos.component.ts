import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import { PhotoApiNotifierService } from 'src/app/services/photo-api-notifier.service';
import { UserEditModalComponent } from 'src/app/modals/user-edit-modal/user-edit-modal.component';
import { PhotoDeleteConfirmModalComponent } from 'src/app/modals/photo-delete-confirm-modal/photo-delete-confirm-modal.component';
import { PhotoDescriptionModalComponent } from 'src/app/modals/photo-description-modal/photo-description-modal.component';

interface Photo {
  id: number,
  content: string
}

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css'],
  providers: [PhotoApiNotifierService]
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
    var ref = self.modalService.open(UserEditModalComponent);
    ref.afterClosed().subscribe(result => {
      if (result) {
        self.userName = ref.componentInstance.userName;
        self.userEmail = ref.componentInstance.userEmail;
        self.userPicture = ref.componentInstance.userPicture;
        // сохраняем данные пользователя
        self.photoApi.setUserInfo(ref.componentInstance.userName, ref.componentInstance.userEmail, ref.componentInstance.userAbout);
        // сохраняем новую картинку пользователя
        self.photoApi.setUserPicture(ref.componentInstance.newUserPictureId);
      }
    });
  }

  deletePhoto(photoId): void {
    var self = this;
    var ref = self.modalService.open(PhotoDeleteConfirmModalComponent);
    ref.afterClosed().subscribe(result => {
      if (result) {
        self.photoApi.deletePhoto(photoId);
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
