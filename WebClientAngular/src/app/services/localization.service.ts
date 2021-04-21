import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import Config from 'src/config';

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
    locale: Locale;
}

@Injectable({
    providedIn: 'root'
})
export class LocalizationService {

    private localization: any = {};

    constructor(
        private httpClient: HttpClient
    ) {
    }

    init() {
        var self = this;
        return self.httpClient.post(Config.getLocalization, null).toPromise().then(function (localization) {
            self.localization = localization;
        });
    }

    getLocalization(): Localization {
        return this.localization;
    }

    getLocale(): Locale {
        return this.localization.locale;
    }
}
