import { Subject } from 'rxjs';
import { ApiResponseModel } from './../shared/api-response.model';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { UserData } from './auth.model';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

const BASE_URL = environment.apiUrl + 'user/';
@Injectable()
export class AuthService {
  private loggedInUserData: UserData;
  private loginCompleted = new Subject<UserData>();
  constructor(private http: HttpClient, private router: Router) {}

  getUserData() {
    return this.loggedInUserData;
  }

  isAuthenticated() {
    return this.loggedInUserData != null;
  }

  signOut() {
    this.loggedInUserData = null;
    this.loginCompleted.next(null);
    this.deleteUserDataInLocalStorage();
    this.router.navigate(['/posts']);
  }

  userAuthenticatedListener() {
    return this.loginCompleted.asObservable();
  }

  autoAuthUser() {
    const authData = this.getAuthData();
    if (!authData) {
      return;
    }

    if (!authData.isExpired) {
      this.loggedInUserData = authData;
      this.loginCompleted.next(this.loggedInUserData);
      this.router.navigate(['/posts']);
    } else {
      this.signOut();
    }
  }

  login(email: string, password: string) {
    this.http
      .post<{ status: string; data: { token: string } }>(BASE_URL + 'login', {
        email: email,
        password: password
      })
      .subscribe(resData => {
        this.loggedInUserData = this.decodeToken(resData.data.token);
        this.loginCompleted.next(this.loggedInUserData);
        this.saveUserDataInLocalStorage();
        this.router.navigate(['/posts']);
      });
  }

  signup(email: string, password: string) {
    this.http
      .post<ApiResponseModel>(BASE_URL + 'signup', {
        email: email,
        password: password
      })
      .subscribe(resData => {
        console.log(resData);
        this.router.navigate(['/login']);
      });
  }
  private saveUserDataInLocalStorage() {
    localStorage.setItem('token', this.loggedInUserData.token);
  }

  private deleteUserDataInLocalStorage() {
    localStorage.removeItem('token');
  }

  private decodeToken(token: string): UserData {
    const helper = new JwtHelperService();
    const decodedToken = helper.decodeToken(token);
    const expireDate = helper.getTokenExpirationDate(token);
    const isExpired = helper.isTokenExpired(token);
    const role = decodedToken[environment.jwtFields.role];
    const userId = decodedToken[environment.jwtFields.nameIdentifier];
    const email = decodedToken[environment.jwtFields.name];
    return {
      userId: +userId,
      token: token,
      expireDate: expireDate,
      role: role,
      isExpired: isExpired,
      email: email
    };
  }
  private getAuthData() {
    const token = localStorage.getItem('token');
    if (!token) {
      return null;
    }
    return this.decodeToken(token);
  }
}
