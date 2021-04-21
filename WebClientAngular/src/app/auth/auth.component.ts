import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Locale, LocalizationService } from 'src/app/services/localization.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { PhotoApiService } from 'src/app/services/photo-api.service';
import { PhotoApiNotifierService } from 'src/app/services/photo-api-notifier.service';
import Utils from 'src/utils';

@Component({
    selector: 'app-auth',
    templateUrl: './auth.component.html',
    styleUrls: ['./auth.component.css'],
    providers: [PhotoApiService, PhotoApiNotifierService]
})
export class AuthComponent implements OnInit {

    locale: Locale;
    @Input() login: string = "vinge";
    @Input() password: string = "12345";
    @Input() hasError: boolean = false;

    constructor(
        private router: Router,
        private localizationService: LocalizationService,
        private localStorage: LocalStorageService,
        private photoApi: PhotoApiService,
        private photoApiNotifier: PhotoApiNotifierService) {
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
        self.photoApiNotifier.onLoginResponse(function (response) {
            if (!response || !response.success) {
                self.hasError = true;
            } else {
                self.hasError = false;
                self.localStorage.setAuthData({ login: self.login, token: response.token });
                self.router.navigate(['/photos']);
            }
        });
        self.photoApiNotifier.start();
    }

    sendAuth(): void {
        var self = this;
        self.photoApi.login(self.login, self.password);
    }
}
