import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { NotificationService } from '../_services/notification.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private notificationService: NotificationService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe
    (
      catchError((error: HttpErrorResponse) => 
        {
          if (error)
          {
            switch (error.status)
            {
              case 400:
                if (error.error.errors)
                {
                  const modelStateErrors = [];

                  for (const key in error.error.errors)
                  {
                    if (error.error.errors[key])
                    {
                      modelStateErrors.push(error.error.errors[key]);
                    }
                  }
                  throw modelStateErrors;
                }
                else
                {
                  this.notificationService.error(error.status.toString(), error.error, 3000)
                }

                break;
              case 401:
                this.notificationService.error(error.status.toString(), "Unathorised", 3000);
                break;
              case 404:
                this.router.navigateByUrl('/not-found');
                this.notificationService.error(error.status.toString(), "Page not found", 3000);
                break;
              // case 405:
              //   console.log(error);
              //   break;
              case 500:
                const navigationExtras: NavigationExtras = 
                {
                  state: 
                  {
                    error: error.error
                  }
                }

                this.router.navigateByUrl('/server-error', navigationExtras);
                break;
              default:
                this.notificationService.error("Error", "Some problems", 3000);
                break;
            }
          }
          throw error;
      })
    );
  }
}
