import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, NgForm, ValidatorFn, Validators } from '@angular/forms';
import { Signup } from './signup';
import { debounceTime } from 'rxjs';

function passwordMatcher(c: AbstractControl): { [key: string]: boolean } | null {
  const passwordControl = c.get('password');
  const confirmControl = c.get('confirmPassword');

  if (passwordControl?.pristine || confirmControl?.pristine) {
    return null;
  }

  if (passwordControl?.value === confirmControl?.value) {
    return null;
  }
  return { match: true };
}

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  signupForm!: FormGroup;
  signup = new Signup();
  passwordMessage = '';
  showPassword = false;

  private validationMessages: any = {
    required: 'Please choose a password.',
    minlength: 'Your password must have at least 6 characters.'
  };

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.signupForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      passwordGroup: this.fb.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      }, { validators: passwordMatcher })
    });

    const passwordControl = this.signupForm.get('passwordGroup.password');
    passwordControl?.valueChanges.pipe(
      debounceTime(1000)
    ).subscribe(
      value => this.setMessage(passwordControl)
    );
  }

  save(): void {
    console.log(this.signupForm);
    console.log('Saved: ' + JSON.stringify(this.signupForm.value));
  }

  setMessage(c: AbstractControl): void {
    this.passwordMessage = '';
    if ((c.touched || c.dirty) && c.errors) {
      this.passwordMessage = Object.keys(c.errors).map(
        key => this.validationMessages[key]).join(' ');
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
    let passwordInput = document.getElementById('passwordId') as HTMLInputElement;
    passwordInput.type = this.showPassword ? 'text' : 'password';
  }

}
