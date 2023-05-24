import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { LobbyComponent } from './game/lobby/lobby.component';
import { ErrorComponent } from './components/error/error.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ListComponent } from './game/list/list.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { CreateComponent } from './game/create/create.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { AuthButtonComponent } from './components/auth-button/auth-button.component';
import { environment as env } from '../environments/environment';
import { SharedModule } from '@app/shared';

@NgModule({
  declarations: [
    AppComponent,
    LobbyComponent,
    ErrorComponent,
    ListComponent,
    NotFoundComponent,
    CreateComponent,
    ServerErrorComponent,
    AuthButtonComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    HttpClientModule,
    AuthModule.forRoot({
      ...env.auth0,
      cacheLocation: 'localstorage',
      httpInterceptor: {
        allowedList: ['*'],
      },
    }),
    SharedModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
