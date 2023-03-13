import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 400:
              this.handleBadRequest(error);
              break;
            case 401:
              this.handleUnauthorized(error);
              break;
            case 404:
              this.handleNotFound();
              break;
            case 500:
              this.handleServerError(error);
              break;
            default:
              this.handleUnexpectedError(error);
              break;
          }
        }
        throw error;
      })
    );
  }

  // Helper functions
  private handleBadRequest(error: HttpErrorResponse) {
    if (error.error.errors) {
      const modelStateErrors = [];
      for (const key in error.error.errors) {
        if (error.error.errors[key]) {
          modelStateErrors.push(error.error.errors[key]);
        }
      }
      throw modelStateErrors.flat();
    } else {
      this.toastr.error(error.error, error.status.toString());
    }
  }

  private handleUnauthorized(error: HttpErrorResponse) {
    this.toastr.error('Unauthorised', error.status.toString());
  }

  private handleNotFound() {
    this.router.navigateByUrl('/not-found');
  }

  private handleServerError(error: HttpErrorResponse) {
    const navigationExtras: NavigationExtras = {
      state: { error: error.error },
    };
    this.router.navigateByUrl('/server-error', navigationExtras);
  }

  private handleUnexpectedError(error: HttpErrorResponse) {
    this.toastr.error('Something unexpected went wrong');
    console.error(error);
  }
}
