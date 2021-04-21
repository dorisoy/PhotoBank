import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Locale, LocalizationService } from 'src/app/services/localization.service';
import { MatDialog } from '@angular/material/dialog';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import { PhotoApiNotifierService } from 'src/app/services/photo-api-notifier.service';
import { PhotoAlbumsModalComponent } from 'src/app/modals/photo-albums-modal/photo-albums-modal.component';
import { PhotoDescriptionModalComponent } from 'src/app/modals/photo-description-modal/photo-description-modal.component';
import { PhotoDeleteConfirmModalComponent } from 'src/app/modals/photo-delete-confirm-modal/photo-delete-confirm-modal.component';
import Utils from 'src/utils';

interface Photo {
    id: number,
    content: string,
    createDate: Date
}

interface Album {
    id: number,
    name: string
}

@Component({
    selector: 'app-photos',
    templateUrl: './photos.component.html',
    styleUrls: ['./photos.component.css'],
    providers: [PhotoApiService, PhotoApiNotifierService]
})
export class PhotosComponent implements OnInit {

    locale: Locale;
    photos: Photo[] = [];
    albums: Album[] = [];

    constructor(
        private router: Router,
        private localizationService: LocalizationService,
        private photoApi: PhotoApiService,
        private photoApiNotifier: PhotoApiNotifierService,
        private modalService: MatDialog) {
        var self = this;
        const clientId = Utils.getClientId();
        self.photoApi.setClientId(clientId);
        self.photoApiNotifier.setClientId(clientId);
        self.locale = self.localizationService.getLocale();
        self.localizationService.addChangeLanguageCallback(function () {
            self.locale = self.localizationService.getLocale();
        });
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
                self.photos.sort((x, y) => x.createDate < y.createDate ? -1 : (x.createDate > y.createDate ? 1 : 0));
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

        self.photoApiNotifier.onGetUserAlbums(function (response) {
            if (!response || !response.success) {
                self.router.navigate(['/']);
            } else {
                var albums = response.albums;
                albums.sort(function (a, b) { return a.name.localeCompare(b.name); });
                self.albums = albums;
            }
        });

        self.photoApiNotifier.start().then(function () {
            self.photoApi.getPhotos();
            self.photoApi.getUserAlbums();
        });
    }

    loadPhotosContent(photoIds): void {
        var self = this;
        for (var photoIdIndex in photoIds) {
            self.photoApi.getPhoto(photoIds[photoIdIndex]); // получаем содержимое каждой фотки
        }
    }

    selectAlbum(albumId): void {
        var self = this;
        self.photos = [];
        if (albumId) {
            self.photoApi.getPhotos([albumId]);
        } else {
            self.photoApi.getPhotos();
        }
    }

    editPhotoAlbums(photoId): void {
        var self = this;
        var modal = self.modalService.open(PhotoAlbumsModalComponent);
        modal.componentInstance.setPhotoId(photoId);
        modal.componentInstance.setCreateUserAlbumsCallback(() => self.photoApi.getUserAlbums());
        modal.componentInstance.setDeleteUserAlbumsCallback(() => self.photoApi.getUserAlbums());
        modal.afterClosed().subscribe(result => {
            if (result) {
                modal.componentInstance.save();
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

    deletePhoto(photoId): void {
        var self = this;
        var modal = self.modalService.open(PhotoDeleteConfirmModalComponent);
        modal.afterClosed().subscribe(result => {
            if (result) {
                self.photoApi.deletePhoto(photoId);
            }
        });
    }
}
