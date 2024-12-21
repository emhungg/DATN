import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {

  constructor() { }
  private isHeaderVisible = true;
  private isFooterVisible = true;

  //#region footer hidden
  hideFooter() {
    this.isFooterVisible = false;
  }

  showFooter() {
    this.isFooterVisible = true;
  }

  isFooterHidden() {
    return !this.isFooterVisible;
  }
  //#endregion
  
  //#region header hidden
  hideHeader() {
    this.isHeaderVisible = false;
  }

  showHeader() {
    this.isHeaderVisible = true;
  }

  isHeaderHidden() {
    return !this.isHeaderVisible;
  }
  //#endregion
}
