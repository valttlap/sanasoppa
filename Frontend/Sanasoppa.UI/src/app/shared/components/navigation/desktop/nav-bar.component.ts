import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-bar',
  template: `
    <div class="navbar navbar-expand-lg bg-main-color">
      <nav class="container-fluid">
        <app-nav-bar-buttons></app-nav-bar-buttons>
      </nav>
    </div>
  `,
})
export class NavBarComponent {}
