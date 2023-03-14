import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl: string = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presenceService: PresenceService) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe
      (
        map((response: User) => {
          const user = response;
          if (user) {
            this.setCurrentUser(user);
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);

    this.presenceService.stopHubConnection();
  }

  setCurrentUser(user: User): void {
    user.roles = [];
    const roles = this.getDecodedToke(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);

    this.presenceService.createHubConnection(user)
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe
      (map((user: User) => {
          if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
          }
          return user;
        })
      );
  }

  getDecodedToke(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}