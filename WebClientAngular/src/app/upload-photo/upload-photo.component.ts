import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import Config from 'src/config';
import Utils from 'src/utils';

@Component({
  selector: 'app-upload-photo',
  templateUrl: './upload-photo.component.html',
  styleUrls: ['./upload-photo.component.css']
})
export class UploadPhotoComponent implements OnInit {

  files: FileList

  constructor(
    private localStorage: LocalStorageService,
    private httpClient: HttpClient
  ) { }

  ngOnInit(): void {
  }

  handleFilesUpload(files: FileList): void {
    var self = this;
    self.files = files;
  }

  submitFiles(): void {
    var self = this;
    var authData = self.localStorage.getAuthData();
    var uploadFunc = function (filesBase64) {
      var postData = { login: authData.login, token: authData.token, clientId: authData.clientId, files: filesBase64 };
      self.httpClient.post(Config.uploadPhotosApiPath, postData).toPromise();
    };
    Utils.filesToBase64(self.files, uploadFunc);
  }
}
