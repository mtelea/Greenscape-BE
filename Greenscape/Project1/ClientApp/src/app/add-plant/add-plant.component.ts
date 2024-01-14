import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-plant',
  templateUrl: './add-plant.component.html',
  styleUrls: ['./add-plant.component.css']
})
export class AddPlantComponent implements OnInit {
  editForm!: FormGroup;
  addedPlantSuccess = false;
  addedPlantError = false;
  plantId: number = -1;
  plantImage = ''

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.editForm = this.fb.group({
      name:'',
      image:'',
      type:'',
      species:'',
      description:''
    });
  }

  savePlantDetails(): void {
    const url = 'https://localhost:7211/plants/add';

    const httpOptions = {
      withCredentials: true
    };

    const payload = {
      plantName: this.editForm.get('name')?.value,
      plantImage: '',
      type: this.editForm.get('type')?.value,
      plantSpecies: this.editForm.get('species')?.value,
      plantDescription: this.editForm.get('description')?.value,
    };

    this.http.post<any>(url, payload, httpOptions).subscribe((response: any) => {
      this.addedPlantSuccess = true;
      this.addedPlantError = false;
      this.plantId = response.plantID;
      console.log(response.Message);
    },
      (error) => {
        this.addedPlantSuccess = false;
        this.addedPlantError = true;
        console.error('Error adding plant:', error);

      });
  }

  editPicture(event: any): void {
    const input = event.target;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const formData = new FormData();
      formData.append('picture', file);

      const url = 'https://localhost:7211/plants/update/' + this.plantId + '/image';

      const httpOptions = {
        withCredentials: true
      };

      this.http.post(url, formData, httpOptions).subscribe(
        (response: any) => {
          console.log(response.Message);
          this.addedPlantSuccess = true;
          this.addedPlantError = false;
          this.getPlant()
        },
        (error) => {
          console.error('Error uploading picture:', error);
          this.addedPlantError = true;
          this.addedPlantSuccess = false;
        }
      );
    }
  }

  getPlant(): void {
    const url = 'https://localhost:7211/plants/get/' + this.plantId;

    const httpOptions = {
      withCredentials: true
    };

    this.http.get<any>(url, httpOptions).subscribe((response: any) => {
      this.plantImage = response.plantImage;
      console.log(response.Message);
    },
      (error) => {
        console.error('Error adding plant:', error);

      });
  }

}
