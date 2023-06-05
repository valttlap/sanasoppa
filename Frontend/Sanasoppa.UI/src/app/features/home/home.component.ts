import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  isAuthenticated$ = this.auth.isAuthenticated$;
  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.isAuthenticated$.subscribe(isAuthenticated => {
      if (isAuthenticated) {
        this.router.navigate(['/lobby']);
      }
    });
  }
}
