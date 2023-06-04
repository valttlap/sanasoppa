import { Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-login-button',
  template: `
    <button class="btn btn-primary btn-lg" (click)="handleLogin()">
      Kirjaudu Sisään
    </button>
  `,
})
export class LoginButtonComponent {
  constructor(private auth: AuthService) {}

  handleLogin(): void {
    this.auth.loginWithRedirect({
      appState: {
        target: '/',
      },
      authorizationParams: {
        prompt: 'login',
      },
    });
  }
}
