import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Login } from './login';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  login = new Login();
  showPassword = false;

  constructor(private fb: FormBuilder, private http: HttpClient) { }

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

    this.http.post('https://localhost:7211/api/Login', this.loginForm.value)
      .subscribe(response => {
        console.log('Response:', response);
      }, error => {
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
