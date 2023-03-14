import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member = {} as Member;

  constructor(private memberService: MembersService, private notificationService: NotificationService, public presenceService: PresenceService) { }

  ngOnInit(): void {
  }

  addLike(member: Member)
  {
    this.memberService.addLike(member.userName).subscribe({
      next: () => this.notificationService.success('Like', 'You have liked ' + member.knownAs, 2000)
    });
  }
}
