import { Component } from '@angular/core';

@Component({
  selector: 'app-page-layout',
  template: `<div class="page-layout">
    <app-nav-bar></app-nav-bar>
    <div class="page-layout__content">
      <ng-content></ng-content>
    </div>
  </div> `,
})
export class PageLayoutComponent {}
