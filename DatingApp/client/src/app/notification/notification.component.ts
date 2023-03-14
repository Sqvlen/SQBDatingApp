import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { Notification } from '../_notification/Notification';
import { NotificationType } from '../_notification/_enums/notification-type';
import { NotificationService } from '../_services/notification.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationListComponent {

  notifications: Notification[] = [];
  subscription!: Subscription;

  constructor(private notificationService: NotificationService) {

  }

  private _addNotification(notification: Notification) {
    this.notifications.push(notification);

    if (notification.timeout !== 0) {
      setTimeout(() => this.close(notification), notification.timeout);

    }
  }

  ngOnInit() {
    this.subscription = this.notificationService.getObservable().subscribe(notification => this._addNotification(notification));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  close(notification: Notification) {
    this.notifications = this.notifications.filter(notif => notif.id !== notification.id);
  }


  className(notification: Notification): string {

    let style: string;

    switch (notification.type) {

      case NotificationType.success:
        style = 'success';
        break;

      case NotificationType.warning:
        style = 'warning';
        break;

      case NotificationType.error:
        style = 'error';
        break;

      default:
        style = 'info';
        break;
    }

    return style;
  }
}