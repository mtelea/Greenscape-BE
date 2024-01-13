import { Component, OnInit} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  userRoles: string = '';
  isExpanded = false;

  ngOnInit(): void {
    this.loadUserRoles();
  }

  constructor(private http: HttpClient, private router: Router) { }


  loadUserRoles(): void {
    const url = 'https://localhost:7211/user/get-current-user-role';

    const httpOptions = {
      withCredentials: true
    };

    this.http.get<string>(url, httpOptions).subscribe(
      (roles: string) => {
        this.userRoles = roles;
      },
      (error) => {
        console.error('Error loading user roles:', error);
      }
    );
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
