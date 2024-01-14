import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Forgot } from './forgot';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-pasword',
  templateUrl: './forgot-pasword.component.html',
  styleUrls: ['./forgot-pasword.component.css']
})
export class ForgotPaswordComponent implements OnInit {
  forgotForm!: FormGroup;
  forgot = new Forgot();


  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  doForgotPassword(): void {
    const url = 'https://localhost:7211/account/forgot-password';
    const email = this.forgotForm.get('email')?.value;

    const payload = {
      email: email
    };

    const httpOptions = {
      withCredentials: true
    };

    this.http.post(url, payload, httpOptions).subscribe(
      (response: any) => {
        console.log(response.Message);

      },
      (error) => {
        console.error('Error during check-in', error);
      }
    );
  }

}
