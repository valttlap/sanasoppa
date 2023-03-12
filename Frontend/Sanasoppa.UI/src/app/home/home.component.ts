import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ILogin } from '../_models/ILogin';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  username?: string;
  model: ILogin = { username: '', password: '' };

  constructor(private router: Router, public accountService: AccountService) {}

  login() {
    if (!this.model) return;
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
