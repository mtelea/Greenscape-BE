import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Reset } from './reset';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { debounceTime } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
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
  selector: 'app-reset-pasword',
  templateUrl: './reset-pasword.component.html',
  styleUrls: ['./reset-pasword.component.css']
})
export class ResetPaswordComponent implements OnInit {
  resetForm!: FormGroup;
  reset = new Reset();
  passwordMessage = '';
  showPassword = false;
  email = '';
  token = '';
  emailAndTokensMissing = true;
  resetSuccessful = false
  resetError = false
  
  private validationMessages: any = {
    required: 'Please choose a password.',
    minlength: 'Your password must have at least 6 characters.'
  };

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.resetForm = this.fb.group({
      passwordGroup: this.fb.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      }, { validators: passwordMatcher })
    });

    const passwordControl = this.resetForm.get('passwordGroup.password');
    passwordControl?.valueChanges.pipe(
      debounceTime(1000)
    ).subscribe(
      value => this.setMessage(passwordControl)
    );

    this.activatedRoute.queryParams
      .subscribe(params => {
        console.log(params);
        this.token = params.token;
        console.log(this.token); 
        this.email = params.email;
        console.log(this.email); 
      })

    if (this.token != null && this.email != null) {
      this.emailAndTokensMissing = false
    }
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

  doResetPassword(): void {
    const url = 'https://localhost:7211/account/reset-password?token=' + this.token.replace(/ /g, '+') + "&email=" + this.email;
    console.log("URL: " + url);
    console.log("TOKEN " + this.token.replace(/ /g, '+'))


    const payload = {
      newPassword: this.resetForm.get('passwordGroup.password')?.value
    };

    const httpOptions = {
      withCredentials: true
    };

    this.http.post(url, payload, httpOptions).subscribe(
      (response: any) => {
        this.resetSuccessful = true
        this.resetError = false
        setTimeout(() => {
          this.router.navigate(['/'])
            .then(() => {
              window.location.reload()
            });
        }, 4000);
        console.log(response.Message);
      },
      (error) => {
        this.resetSuccessful = false
        this.resetError = true
        console.error('Error during check-in', error);
      }
    );
  }

}
