import { NotificationType } from "./_enums/notification-type";

export class Notification {
    constructor(public id: number, public type: NotificationType, public title: string, public message: string, public timeout: number)
    {

    }
  }