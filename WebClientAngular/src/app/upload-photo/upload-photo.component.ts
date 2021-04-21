import { Component, OnInit } from '@angular/core';
import { Locale, LocalizationService } from 'src/app/services/localization.service';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import Utils from 'src/utils';

@Component({
    selector: 'app-upload-photo',
    templateUrl: './upload-photo.component.html',
    styleUrls: ['./upload-photo.component.css']
})
export class UploadPhotoComponent implements OnInit {

    locale: Locale;
    files: FileList;
    filesCount: number = 0;

    constructor(
        private localizationService: LocalizationService,
        private photoApi: PhotoApiService
    ) {
        var self = this;
        self.locale = self.localizationService.getLocale();
        self.localizationService.addChangeLanguageCallback(function () {
            self.locale = self.localizationService.getLocale();
        });
    }

    ngOnInit(): void {
    }

    handleFilesUpload(files: FileList): void {
        var self = this;
        self.files = files;
        self.filesCount = files.length;
    }

    submitFiles(): void {
        var self = this;
        if (self.files) {
            var uploadFunc = function (filesBase64) {
                self.photoApi.uploadPhotos(filesBase64);
                self.files = null;
                self.filesCount = 0;
            };
            Utils.filesToBase64(self.files, uploadFunc);
        }
    }
}
