import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { NotificationService } from '../_services/notification.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private notificationService: NotificationService) {

  }

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe
      (
        map
          (
            user => {
              if (user) {
                return true;
              }
              else {
                this.notificationService.error('Error', 'Your shall not pass!', 2000);
                return false;
              }
            }
          )
      );
  }

}
