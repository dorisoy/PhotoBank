import { Component, Input } from '@angular/core';
import { LocalizationService } from './services/localization.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {

    title = 'WebClientAngular';
    @Input() isInited: boolean = false;

    constructor(
        private localizationService: LocalizationService
    ) {
        var self = this;
        localizationService.init().then(() => self.isInited = true);
    }
}
