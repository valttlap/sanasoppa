import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignupButtonComponent } from './components/buttons/signup-button.component';
import { LoginButtonComponent } from './components/buttons/login-button.component';
import { LogoutButtonComponent } from './components/buttons/logout-button.component';
import { RouterModule } from '@angular/router';
import { COMPONENTS } from './components';

@NgModule({
  declarations: [
    SignupButtonComponent,
    LoginButtonComponent,
    LogoutButtonComponent,
    ...COMPONENTS,
  ],
  imports: [CommonModule, RouterModule],
  exports: [...COMPONENTS],
})
export class SharedModule {}
