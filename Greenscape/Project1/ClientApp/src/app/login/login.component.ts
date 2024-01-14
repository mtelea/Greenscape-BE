import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Login } from './login';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  login = new Login();
  showPassword = false;
  loginSuccess = false;
  loginError = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: false
    });
  }

  doLogin(): void {
    console.log(this.loginForm);
    console.log('Saved: ' + JSON.stringify(this.loginForm.value));

    const httpOptions = {
      withCredentials: true
    };

    this.http.post('https://localhost:7211/api/Login/login', this.loginForm.value, httpOptions)
      .subscribe(response => {
        this.loginSuccess = true;
        this.loginError = false;
        console.log('Response:', response);
        setTimeout(() => {
          this.router.navigate(['/']).then(() => {
            window.location.reload()
          });
        }, 3000);
      }, error => {
        this.loginError = true;
        this.loginSuccess = false;
        console.error('Error:', error);
      });
  }

  save(): void {
    console.log(this.loginForm);
    console.log('Saved: ' + JSON.stringify(this.loginForm.value));
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
    let passwordInput = document.getElementById('passwordId') as HTMLInputElement;
    passwordInput.type = this.showPassword ? 'text' : 'password';
  }

}
