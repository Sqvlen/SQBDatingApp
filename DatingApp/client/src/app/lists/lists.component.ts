import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate: string = 'liked';
  pageNumber: number = 1;
  pageSize: number = 5;
  pagination: Pagination | undefined;
  genderList = [{ value: 'male', display: 'Females' }, { value: 'female', display: 'Males' }]

  userParams: UserParams | undefined;

  constructor(private memberService: MembersService) {
    this.userParams = memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.loadLikes();
  }

  pageChanged(event: any) {
    if (this.userParams?.pageSize !== event.page) {
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }
}
