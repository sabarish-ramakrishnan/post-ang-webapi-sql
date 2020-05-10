import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = this.authService.isAuthenticated()
      ? this.authService.getUserData().token
      : '';
    const headers = req.headers
      .set('Content-Type', 'application/json')
      .set('Authorization', 'Bearer ' + token)
      .set('Accept', '*/*');
    const authReq = req.clone({ headers });
    return next.handle(authReq);
  }
}
