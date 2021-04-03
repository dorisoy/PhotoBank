import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import { PhotoApiNotifierService } from 'src/app/services/photo-api-notifier.service';
import { PhotoDeleteConfirmModalComponent } from 'src/app/modals/photo-delete-confirm-modal/photo-delete-confirm-modal.component';
import { PhotoDescriptionModalComponent } from 'src/app/modals/photo-description-modal/photo-description-modal.component';
import Utils from 'src/utils';

interface Photo {
  id: number,
  content: string,
  createDate: Date
}

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css'],
  providers: [PhotoApiService, PhotoApiNotifierService]
})
export class PhotosComponent implements OnInit {

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
          content: Utils.getImageFromBase64(response.fileBase64Content),
          createDate: response.createDate
        };
        self.photos.push(photo);
        self.photos.sort((x,y) => x.createDate < y.createDate ? -1 : (x.createDate > y.createDate ? 1 : 0));
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
      self.photoApi.getPhotos();
    });
  }

  loadPhotosContent(photoIds): void {
    var self = this;
    for (var photoIdIndex in photoIds) {
      self.photoApi.getPhoto(photoIds[photoIdIndex]); // получаем содержимое каждой фотки
    }
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
