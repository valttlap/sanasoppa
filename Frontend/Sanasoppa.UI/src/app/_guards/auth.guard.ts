import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { AuthService } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  constructor(private auth: AuthService) {}

  canActivate(): Observable<boolean> {
    return this.auth.isAuthenticated$.pipe(
      map(user => {
        if (user) return true;
        else {
          return false;
        }
      })
    );
  }
}
