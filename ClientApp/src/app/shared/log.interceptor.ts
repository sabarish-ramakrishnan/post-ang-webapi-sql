import {
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpEvent,
  HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, finalize } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LogInterceptor implements HttpInterceptor {
  constructor() {}
  private logDetails(msg: string) {
    console.log(msg);
  }
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const startTime = Date.now();
    let status: string;
    return next.handle(req).pipe(
      tap(
        event => {
          status = '';
          if (event instanceof HttpResponse) {
            status = 'succeeded';
          }
        },
        error => {
          status =
            '\nfailed : ' +
            '\nStatus: ' +
            error.status +
            '\nMessage: ' +
            error.message;
          console.log(error);
        }
      ),
      finalize(() => {
        const elapsedTime = Date.now() - startTime;
        const message =
          req.method +
          ' ' +
          req.urlWithParams +
          ' ' +
          status +
          ' in ' +
          elapsedTime +
          'ms';

        this.logDetails(message);
      })
    );
  }
}
