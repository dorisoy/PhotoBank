import { Component, OnInit } from '@angular/core';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import Utils from 'src/utils';

@Component({
  selector: 'app-upload-photo',
  templateUrl: './upload-photo.component.html',
  styleUrls: ['./upload-photo.component.css']
})
export class UploadPhotoComponent implements OnInit {

  files: FileList

  constructor(
    private photoApi: PhotoApiService
  ) {
  }

  ngOnInit(): void {
  }

  handleFilesUpload(files: FileList): void {
    var self = this;
    self.files = files;
  }

  submitFiles(): void {
    var self = this;
    var uploadFunc = function (filesBase64) {
      self.photoApi.uploadPhotos(filesBase64);
    };
    Utils.filesToBase64(self.files, uploadFunc);
  }
}
