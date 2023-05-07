import { AuthService } from '@auth0/auth0-angular';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ReCaptchaV3Service } from 'ng-recaptcha';
import { ILogin } from '../_models/ILogin';
import { GameHubService } from '../_services/gamehub.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  username?: string;
  model: ILogin = { username: '', password: '' };
  token?: string;

  constructor(
    private router: Router,
    public auth: AuthService,
    private recaptchaV3Service: ReCaptchaV3Service,
    private game: GameHubService
  ) {}

  getGames(): void {
    this.game.getNotStartedGames().subscribe();
  }
}
