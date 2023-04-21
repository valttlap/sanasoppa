import { IToken } from './../_models/IToken';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { ILogin } from '../_models/ILogin';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {}

  login(model: ILogin) {
    return this.http.post<User>(`${this.baseUrl}/account/login`, model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = AccountService.getDecodedToken(user.token)?.role;
    if (Array.isArray(roles)) {
      user.roles = roles;
    } else if (typeof roles === 'string') {
      user.roles.push(roles);
    } else {
      user.roles = [];
    }
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  static getDecodedToken(token: string): IToken | null {
    const helper = new JwtHelperService();
    return helper.decodeToken<IToken>(token);
  }
}
