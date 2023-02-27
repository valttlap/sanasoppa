import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root',
})
export class UserGuard {
  constructor(private accountService: AccountService) {}

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (!user) return false;
        if (
          user.roles.includes('Member') ||
          user.roles.includes('Moderator') ||
          user.roles.includes('Admin')
        ) {
          return true;
        } else {
          return false;
        }
      })
    );
  }
}
