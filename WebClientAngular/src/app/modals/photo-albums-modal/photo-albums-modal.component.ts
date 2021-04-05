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
  styleUrls: ['./photo-albums-modal.component.css']
})
export class PhotoAlbumsModalComponent implements OnInit {

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
        response.albums.forEach(a => self.loadedAlbums.push({ id: a.id, name: a.name, isSelected: false, isDeleted: false }));
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

    self.photoApiNotifier.start().then(function () {
      self.photoApi.getUserAlbums();
    });
  }

  setPhotoId(photoId: number): void {
    this.photoId = photoId;
  }

  selectAlbum(album: Album): void {
    album.isSelected = !album.isSelected;
  }

  addAlbum(): void {
    var self = this;
    var newAlbum = { id: 0, name: self.newAlbumName, isSelected: false, isDeleted: false };
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

  save(): void {
    var self = this;

    if (self.addedAlbums.length > 0) {
      var newAlbums = self.addedAlbums.map(a => { name: a.name });
      self.photoApi.createUserAlbums(newAlbums);
    }

    var deletedAlbums = self.loadedAlbums.filter(a => a.isDeleted);
    if (deletedAlbums.length > 0) {
      var albumsId = deletedAlbums.map(a => a.id);
      self.photoApi.deleteUserAlbums(albumsId);
    }

    var settedAlbumsId = self.loadedAlbums.filter(a => a.isSelected).map(a => a.id);
    var settedAlbumsName = self.addedAlbums.filter(a => a.isSelected).map(a => a.name);
    self.photoApi.setPhotoAlbums(self.photoId, settedAlbumsId, settedAlbumsName);
  }
}
