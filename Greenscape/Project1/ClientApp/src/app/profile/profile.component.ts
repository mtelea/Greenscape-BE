import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  username: string = '';
  email: string = '';
  points: number = 0;

  constructor() { }

  ngOnInit(): void {
    this.defineProfileValues();
  }

  defineProfileValues():void{
    this.username = 'lila';
    this.email = 'lilaley@gmail.com';
    this.points = 100;
  }

  editPicture(){
    console.log("editpicture");
  }

  dailyCheckin(){
    this.points+=100;
  }

}
