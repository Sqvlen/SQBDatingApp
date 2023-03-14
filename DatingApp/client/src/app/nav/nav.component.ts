import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { Member } from '../_models/member';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  public user: User = {} as User;

  constructor(public accountService: AccountService, private router: Router, private memberService: MembersService) 
  {
    
  }

  ngOnInit(): void 
  {
    this.loadUser();
  }

  loadUser(): void
  {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user)
          this.user = user
      }
    });
  }

  login(): void 
  {
    this.accountService.login(this.model).subscribe
    ({
      next: () => this.router.navigateByUrl('/members'),
    });
  }

  logout(): void
  {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
