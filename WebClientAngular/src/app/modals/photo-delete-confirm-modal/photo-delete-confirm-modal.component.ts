import { Component, OnInit } from '@angular/core';
import { Locale, LocalizationService } from 'src/app/services/localization.service';

@Component({
    selector: 'app-photo-delete-confirm-modal',
    templateUrl: './photo-delete-confirm-modal.component.html',
    styleUrls: ['./photo-delete-confirm-modal.component.css']
})
export class PhotoDeleteConfirmModalComponent implements OnInit {

    locale: Locale;

    constructor(
        private localizationService: LocalizationService,
    ) {
        this.locale = this.localizationService.getLocale();
    }

    ngOnInit(): void {
    }
}
