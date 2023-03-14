import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  loginMode = false;
  users: any;

  constructor() { }

  ngOnInit(): void 
  {

  }

  registerToggle(): void
  {
    this.registerMode = !this.registerMode;
  }

  loginToggle(): void
  {
    this.loginMode = !this.loginMode;
  }

  cancelRegisterMode(event: boolean): void
  {
    this.registerMode = event;
  }

  cancelLoginMode(event: boolean): void
  {
    this.loginMode = event;
  }
}
