import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignupButtonComponent } from './components/buttons/signup-button.component';
import { LoginButtonComponent } from './components/buttons/login-button.component';
import { LogoutButtonComponent } from './components/buttons/logout-button.component';
import { RouterModule } from '@angular/router';
import { COMPONENTS } from './components';
import { PageLayoutComponent } from './components/page-layout.component';
import { PageFooterComponent } from './components/page-footer.component';

@NgModule({
  declarations: [
    SignupButtonComponent,
    LoginButtonComponent,
    LogoutButtonComponent,
    ...COMPONENTS,
    PageLayoutComponent,
    PageFooterComponent,
  ],
  imports: [CommonModule, RouterModule],
  exports: [...COMPONENTS],
})
export class SharedModule {}
