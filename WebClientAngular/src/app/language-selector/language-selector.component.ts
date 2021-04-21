import { Component, Input, OnInit } from '@angular/core';
import { Language, Locale, LocalizationService } from 'src/app/services/localization.service';

@Component({
    selector: 'app-language-selector',
    templateUrl: './language-selector.component.html',
    styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent implements OnInit {

    availableLanguages: Language[] = [];

    constructor(
        private localizationService: LocalizationService
    ) {
        var self = this;
        self.availableLanguages = self.localizationService.getLocalization().availableLanguages;
    }

    ngOnInit(): void {
    }

    changeLanguage(selectedLanguage: Language): void {
        var self = this;
        self.localizationService.changeLanguage(selectedLanguage.code);
    }
}
