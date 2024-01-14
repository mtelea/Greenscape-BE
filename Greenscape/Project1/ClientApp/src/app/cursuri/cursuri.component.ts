import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cursuri',
  templateUrl: './cursuri.component.html',
  styleUrls: ['./cursuri.component.css']
})
export class CursuriComponent implements OnInit {
  points100 = false;
  points500 = false;
  points1000 = false;

  constructor() { }

  ngOnInit(): void {
  }

}
