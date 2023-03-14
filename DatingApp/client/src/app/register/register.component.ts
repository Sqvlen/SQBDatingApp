import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  minDate: Date = new Date();
  validationErrors: string[] | undefined;

  constructor(private accountService: AccountService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit(): void 
  {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 10);
    this.minDate.setFullYear(this.minDate.getFullYear() - 100);
  }

  initializeForm()
  {
    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required]],
      gender: ['male', []],
      knownAs: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]],
      
      password: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchValues(matchTo: string): ValidatorFn
  {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null: { notMatching: true }
    }
  }

  register(): void
  {
    const dateOfBirth = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const values = {...this.registerForm.value, dateOfBirth: dateOfBirth}
    this.accountService.register(values).subscribe
    ({
      next: () => {
        this.router.navigateByUrl('/members');
      },
      error: error => {
        this.validationErrors = error;
      }
    });
  }

  cancel(): void
  {
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dateOfBirth: string | undefined)
  {
    if (!dateOfBirth) return;
    let theDateOfBirth = new Date(dateOfBirth);

    return new Date(theDateOfBirth.setMinutes(theDateOfBirth.getMinutes() - theDateOfBirth.getTimezoneOffset())).toISOString().slice(0, 10);
  }
}
