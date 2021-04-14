import { Component, Input, OnInit } from '@angular/core';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import { PhotoApiNotifierService } from 'src/app/services/photo-api-notifier.service';
import Utils from 'src/utils';

@Component({
    selector: 'app-user-edit-modal',
    templateUrl: './user-edit-modal.component.html',
    styleUrls: ['./user-edit-modal.component.css'],
    providers: [PhotoApiService, PhotoApiNotifierService]
})
export class UserEditModalComponent implements OnInit {

    @Input() userName: string = "";
    @Input() userEmail: string = "";
    @Input() userAbout: string = "";
    userPicture: string = "";
    newUserPictureId: string = "";

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

        self.photoApiNotifier.onGetUserInfoResponse(function (response) {
            if (response && response.success) {
                self.userName = response.name;
                self.userEmail = response.email;
                self.userAbout = response.about;
                self.userPicture = Utils.getImageFromBase64(response.pictureBase64Content);
            }
        });

        self.photoApiNotifier.onLoadUserPictureResponse(function (response) {
            if (response && response.success) {
                self.userPicture = Utils.getImageFromBase64(response.pictureBase64Content);
                self.newUserPictureId = response.newPictureId;
            }
        });

        self.photoApiNotifier.start().then(function () {
            self.photoApi.getUserInfo();
        });
    }

    handleFilesUpload(files: FileList): void {
        var self = this;
        var uploadFunc = function (fileBase64) {
            self.photoApi.loadUserPicture(fileBase64);
        };
        Utils.fileToBase64(files[0], uploadFunc);
    }
}
