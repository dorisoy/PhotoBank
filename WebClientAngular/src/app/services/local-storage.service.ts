import { Inject, Injectable } from '@angular/core';
import { LOCAL_STORAGE, StorageService } from 'ngx-webstorage-service';

interface AuthDataObject {
  login: string,
  token: string
}

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor(@Inject(LOCAL_STORAGE) private storageService: StorageService) { }

  public getAuthData(): AuthDataObject {
    return this.storageService.get("authData");
  }

  public setAuthData(authData: AuthDataObject): void {
    this.storageService.set("authData", authData);
  }
}
