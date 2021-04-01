import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { MatDialogModule } from '@angular/material/dialog';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthComponent } from './auth/auth.component';
import { PhotosComponent } from './photos/photos.component';
import { UploadPhotoComponent } from './upload-photo/upload-photo.component';
import { UserEditModalComponent } from './modals/user-edit-modal/user-edit-modal.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PhotoDeleteConfirmModalComponent } from './modals/photo-delete-confirm-modal/photo-delete-confirm-modal.component';
import { PhotoDescriptionModalComponent } from './modals/photo-description-modal/photo-description-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    PhotosComponent,
    UploadPhotoComponent,
    UserEditModalComponent,
    PhotoDeleteConfirmModalComponent,
    PhotoDescriptionModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MatDialogModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
