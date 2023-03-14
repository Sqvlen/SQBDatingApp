import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Output() cancelLogin = new EventEmitter();
  model: any = {};
  loginForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  constructor(public accountService: AccountService, private router: Router, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]]
    });
  }

  login(): void {
    this.accountService.login(this.loginForm.value).subscribe
      ({
        next: () => {
          {
            this.router.navigateByUrl('/members');
          }
        },
        error: error => {
          this.validationErrors = error;
          this.model = {};
        }
      });
  }

  cancel() {
    this.cancelLogin.emit(false);
  }
}
