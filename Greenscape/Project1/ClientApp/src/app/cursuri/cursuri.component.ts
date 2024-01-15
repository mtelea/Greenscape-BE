import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cursuri',
  templateUrl: './cursuri.component.html',
  styleUrls: ['./cursuri.component.css']
})
export class CursuriComponent implements OnInit {
  points100 = false;
  points500 = false;
  points1000 = false;
  userPoints = 0;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.fetchUserData()
  }

  fetchUserData(): void {
    const url = 'https://localhost:7211/api/UserData';

    const httpOptions = {
      withCredentials: true
    };

    this.http.get<any>(url, httpOptions).subscribe((response: any) => {
      this.userPoints = response.points;
      if (this.userPoints >= 100) {
        this.points100 = true
      }
      if (this.userPoints >= 500) {
        this.points500 = true
      }
      if (this.userPoints >= 1000) {
        this.points1000 = true
      }
    });
  }

  buyCourse(selectedCourseId:number): void {
    const url = 'https://localhost:7211/api/UserData/update-user-points';

    const payload = {
      points: 100,
      operation: "add",
      source: "Course" + selectedCourseId
    };

    const httpOptions = {
      withCredentials: true
    };

    this.http.post(url, payload, httpOptions).subscribe(
      (response: any) => {
        const pdfUrl = `https://localhost:7211/courses/Course${selectedCourseId}.pdf`;
        const downloadLink = document.createElement('a');
        downloadLink.href = pdfUrl;
        downloadLink.download = `Course${selectedCourseId}.pdf`;
        downloadLink.click();
        console.log(response.Message);
        this.fetchUserData();
      },
      (error) => {
        console.error('Error buying course / Not enough points', error);
        
      }
    );
  }




}
