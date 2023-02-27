import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
})
export class ServerErrorComponent {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  error: any;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error'];
  }
}
