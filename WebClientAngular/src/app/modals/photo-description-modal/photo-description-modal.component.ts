import { Component, Input, OnInit } from '@angular/core';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SignalRService } from 'src/app/services/signalr.service';
import { PhotoApiService } from 'src/app/services/photo-api.service';

@Component({
  selector: 'app-photo-description-modal',
  templateUrl: './photo-description-modal.component.html',
  styleUrls: ['./photo-description-modal.component.css'],
  providers: [{ provide: SignalRService }]
})
export class PhotoDescriptionModalComponent implements OnInit {

  photoId: number;
  @Input() photoDescription: string = "";

  constructor(
    private localStorage: LocalStorageService,
    private signalr: SignalRService,
    private photoApiService: PhotoApiService
  ) { }

  ngOnInit(): void {
  }

  setPhotoId(photoId): void {
    var self = this;
    self.photoId = photoId;
    self.signalr.addHandler("GetPhotoAdditionalInfoResponse", function (response) {
      if (response && response.success) {
        self.photoDescription = response.additionalInfo.description;
      }
    });
    var authData = self.localStorage.getAuthData();
    self.signalr.start(authData.clientId).then(function () {
      self.photoApiService.getPhotoAdditionalInfo(self.photoId);
    });
  }

  save(): void {
    var self = this;
    self.photoApiService.setPhotoAdditionalInfo(self.photoId, { description: self.photoDescription });
  }
}
