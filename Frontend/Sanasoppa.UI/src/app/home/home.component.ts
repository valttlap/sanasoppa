import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ReCaptchaV3Service } from 'ng-recaptcha';
import { ILogin } from '../_models/ILogin';
import { AccountService } from '../_services/account.service';

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
    public accountService: AccountService,
    private recaptchaV3Service: ReCaptchaV3Service
  ) {}

  login() {
    if (!this.model) return;
    this.recaptchaV3Service.execute('login').subscribe((token: string) => {
      console.log(token);
    });

    this.accountService.login(this.model).subscribe({
      next: () => {
        this.model = { username: '', password: '' };
      },
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
