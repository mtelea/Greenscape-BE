import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  username: string = '';
  email: string = '';
  points: number = 0;
  profilePicture: string = '';
  dailyAlreadyClaimed = false
  dailySuccess = false
  logOutSuccess = false

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.fetchUserData();
  }

  fetchUserData(): void {
    const url = 'https://localhost:7211/api/UserData';

    const httpOptions = {
      withCredentials: true
    };

    this.http.get<any>(url, httpOptions).subscribe((response: any) => {
      // Assuming there is only one user in the array for simplicity
      this.username = response.username;
      this.email = response.email;
      this.profilePicture = this.transformProfilePicture(response.profilePicture);
      this.points = response.points;
    });
  }

  transformProfilePicture(profilePictureValue: string): string {
    const localhostPart = 'https://localhost:7211/';
    const transformedPath = localhostPart + profilePictureValue
    return transformedPath;
  }

  defineProfileValues():void{
    this.username = 'lila';
    this.email = 'lilaley@gmail.com';
    this.points = 100;
  }

  editPicture(event: any): void{
    const input = event.target;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const formData = new FormData();
      formData.append('picture', file);

      const url = 'https://localhost:7211/api/UserData/set-user-profile-picture';

      const httpOptions = {
        withCredentials: true
      };

      this.http.post(url, formData, httpOptions).subscribe(
        (response: any) => {
          console.log(response.Message);
          this.fetchUserData();
        },
        (error) => {
          console.error('Error uploading picture:', error);
        }
      );
    }
  }

  dailyCheckin(): void{
    const url = 'https://localhost:7211/api/UserData/update-user-points';

    const payload = {
      points: 100,
      operation: "add",
      source: "Daily"
    };

    const httpOptions = {
      withCredentials: true
    };

    this.http.post(url, payload, httpOptions).subscribe(
      (response: any) => {
        console.log(response.Message);
        this.fetchUserData();
        this.dailySuccess = true
      },
      (error) => {
        console.error('Error during check-in', error);
        this.dailyAlreadyClaimed = true;
      }
    );
  }

  logOut(): void {
    const url = 'https://localhost:7211/api/Login/logout';

    const httpOptions = {
      withCredentials: true
    };

    this.http.get(url, httpOptions).subscribe(
      (response: any) => {
        console.log(response.Message);
        this.logOutSuccess = true
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 3000);
      },
      (error) => {
        console.error('Error during log-out', error);
        this.router.navigate(['/']);
      }
    );
  }


}
