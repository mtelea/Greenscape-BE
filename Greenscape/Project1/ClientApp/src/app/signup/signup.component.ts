import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, NgForm, ValidatorFn, Validators } from '@angular/forms';
import { Signup } from './signup';
import { debounceTime } from 'rxjs';
import { HttpClient } from '@angular/common/http';

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

  constructor(private fb: FormBuilder, private httpClient: HttpClient) { }

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

  Register(): void {
    if (this.signupForm.valid) {
      const url = 'https://localhost:7211/register/register-user';

      const { email, username, passwordGroup } = this.signupForm.value;
      const { password, confirmPassword } = passwordGroup;

      const requestBody = {
        email: email,
        userName: username,
        password: password,
        confirmPassword: confirmPassword
      };

      this.httpClient.post(url, requestBody)
        .subscribe(
          (response) => {
            console.log('API Response:', response);
          },
          (error) => {
            console.error('API Error:', error);
          }
        );
    } else {
      console.error('Form is invalid. Please check the form fields.');
    }
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