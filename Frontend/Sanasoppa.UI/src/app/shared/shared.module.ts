import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignupButtonComponent } from './components/buttons/signup-button.component';
import { LoginButtonComponent } from './components/buttons/login-button.component';
import { LogoutButtonComponent } from './components/buttons/logout-button.component';
import { RouterModule } from '@angular/router';
import { AUTH_BUTTON_COMPONENTS, COMPONENTS } from './components';
import { PageLayoutComponent } from './components/page-layout.component';
import { PageFooterComponent } from './components/page-footer.component';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [
    SignupButtonComponent,
    LoginButtonComponent,
    LogoutButtonComponent,
    ...COMPONENTS,
    PageLayoutComponent,
    PageFooterComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
    NgxSpinnerModule.forRoot({
      type: 'line-scale-party',
    }),
  ],
  exports: [
    ...COMPONENTS,
    ToastrModule,
    NgxSpinnerModule,
    ...AUTH_BUTTON_COMPONENTS,
  ],
})
export class SharedModule {}
