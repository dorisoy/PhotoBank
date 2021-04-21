import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import Config from 'src/config';

export interface Language {
    name: string,
    code: string,
}

export interface Locale {
    ok: '',
    cancel: '',
    yes: '',
    no: '',

    authWelcome: '',
    authLogin: '',
    authPassword: '',
    authSend: '',
    authLoginError: '',

    photosAllAlbums: '',
    photosSelectAlbum: '',

    uploadUploadPhotos: '',
    uploadSelectedPhotos: '',
    uploadSend: '',

    usereditEdit: '',
    usereditName: '',
    usereditEmail: '',
    usereditChangePicture: '',

    albumsAdded: '',
    albumsAdd: '',

    photodeleteConfirm: '',

    photodescDescription: ''
}

export interface Localization {
    availableLanguages: Language[];
    locale: Locale;
}

@Injectable({
    providedIn: 'root'
})
export class LocalizationService {

    private localization: Localization;
    private changeLanguageCallbacks: any[] = [];

    constructor(
        private httpClient: HttpClient
    ) {
    }

    init() {
        var self = this;
        return self.httpClient.post(Config.getLocalization, {}).toPromise().then(function (localization: Localization) {
            self.localization = localization;
        });
    }

    changeLanguage(language: string) {
        var self = this;
        var postData = { language: language };
        return self.httpClient.post(Config.getLocalization, postData).toPromise().then(function (localization: Localization) {
            self.localization = localization;
            self.changeLanguageCallbacks.forEach(function (callback) { callback(); });
        });
    }

    addChangeLanguageCallback(callback: any) {
        this.changeLanguageCallbacks.push(callback);
    }

    getLocalization(): Localization {
        return this.localization;
    }

    getLocale(): Locale {
        return this.localization.locale;
    }
}
