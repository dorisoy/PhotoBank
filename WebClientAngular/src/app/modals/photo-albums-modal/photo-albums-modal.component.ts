import { Component, Input, OnInit } from '@angular/core';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import { PhotoApiNotifierService } from 'src/app/services/photo-api-notifier.service';
import Utils from 'src/utils';

interface Album {
  id: number,
  name: string,
  isSelected: boolean,
  isDeleted: boolean
}

@Component({
  selector: 'app-photo-albums-modal',
  templateUrl: './photo-albums-modal.component.html',
  styleUrls: ['./photo-albums-modal.component.css'],
  providers: [PhotoApiService, PhotoApiNotifierService]
})
export class PhotoAlbumsModalComponent implements OnInit {

  private createUserAlbumsCallback: any;
  private deleteUserAlbumsCallback: any;
  private photoId: number;
  @Input() loadedAlbums: Album[] = [];
  @Input() addedAlbums: Album[] = [];
  @Input() newAlbumName: string = "";

  constructor(
    private photoApi: PhotoApiService,
    private photoApiNotifier: PhotoApiNotifierService
  ) {
    const clientId = Utils.getClientId();
    this.photoApi.setClientId(clientId);
    this.photoApiNotifier.setClientId(clientId);
  }

  ngOnInit(): void {
    var self = this;

    self.photoApiNotifier.onGetUserAlbums(function (response) {
      if (response && response.success) {
        var loadedAlbums: Album[] = [];
        response.albums.forEach(a => loadedAlbums.push({ id: a.id, name: a.name, isSelected: false, isDeleted: false }));
        loadedAlbums.sort(function (a, b) { return a.name.localeCompare(b.name); });
        self.loadedAlbums = loadedAlbums;
        self.photoApi.getPhotoAlbums(self.photoId);
      }
    });

    self.photoApiNotifier.onGetPhotoAlbums(function (response) {
      if (response && response.success) {
        self.loadedAlbums.forEach(function (a) {
          a.isSelected = response.albumsId.includes(a.id);
        });
      }
    });

    self.photoApiNotifier.onCreateUserAlbums(function (response) {
      if (response && response.success) {
        self.setPhotoAlbums();
        if (self.createUserAlbumsCallback) {
          self.createUserAlbumsCallback();
        }
      }
    });

    self.photoApiNotifier.onDeleteUserAlbums(function (response) {
      if (response && response.success) {
        if (self.deleteUserAlbumsCallback) {
          self.deleteUserAlbumsCallback();
        }
      }
    });

    self.photoApiNotifier.start().then(function () {
      self.photoApi.getUserAlbums();
    });
  }

  setCreateUserAlbumsCallback(callback) {
    this.createUserAlbumsCallback = callback;
  }

  setDeleteUserAlbumsCallback(callback) {
    this.deleteUserAlbumsCallback = callback;
  }

  setPhotoId(photoId: number): void {
    this.photoId = photoId;
  }

  selectAlbum(album: Album): void {
    album.isSelected = !album.isSelected;
  }

  addAlbum(): void {
    var self = this;
    var newAlbum = { id: 0, name: self.newAlbumName, isSelected: true, isDeleted: false };
    self.addedAlbums.push(newAlbum);
    self.newAlbumName = "";
  }

  deleteLoadedAlbum(album: Album): void {
    album.isDeleted = !album.isDeleted;
  }

  deleteAddedAlbum(album: Album): void {
    var self = this;
    self.addedAlbums = self.addedAlbums.filter(a => a !== album);
  }

  setPhotoAlbums(): void {
    var self = this;
    var settedAlbumsId = self.loadedAlbums.filter(a => a.isSelected).map(a => a.id);
    var settedAlbumsName = self.addedAlbums.filter(a => a.isSelected).map(a => a.name);
    self.photoApi.setPhotoAlbums(self.photoId, settedAlbumsId, settedAlbumsName);
  }

  save(): void {
    var self = this;

    if (self.addedAlbums.length > 0) {
      // если есть новые альбомы
      // шлем запрос на их создание и ждем ответа, после которого устанавливаем фотку в альбомы
      var newAlbums = self.addedAlbums.map(function (a) { return { name: a.name }; });
      self.photoApi.createUserAlbums(newAlbums);
    } else {
      // если новых альбомов нет, то сразу устанавливаем фотку в альбомы
      self.setPhotoAlbums();
    }

    var deletedAlbums = self.loadedAlbums.filter(a => a.isDeleted);
    if (deletedAlbums.length > 0) {
      var albumsId = deletedAlbums.map(function (a) { return a.id; });
      self.photoApi.deleteUserAlbums(albumsId);
    }
  }
}
