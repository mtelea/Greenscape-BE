import { Component, OnInit } from '@angular/core';
import { IPlant } from 'src/app/shared/IPlant';
import { AdminService } from '../admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Admin } from '../admin';

@Component({
  selector: 'app-admin-details',
  templateUrl: './admin-details.component.html',
  styleUrls: ['./admin-details.component.css']
})
export class AdminDetailsComponent implements OnInit {
  id: number = Number(this.route.snapshot.paramMap.get('id'));
  adminForm!: FormGroup;
  signup = new Admin();
  passwordMessage = '';
  showPassword = false;
  registrationSuccess = false
  pageTitle = 'Product Detail';
  errorMessage = '';
  product: IPlant | undefined;
  
  constructor(private fb: FormBuilder,
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private productService: AdminService) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.getProduct(id);
    }
    this.adminForm = this.fb.group({
      name:'',
      image:'',
      type:'',
      species:'',
      description:''
    });
    
  }

  getProduct(id: number): void {
    this.productService.getProduct(id).subscribe({
      next: product => {
        this.product = product;
        this.populateData();
      },
      error: err => this.errorMessage = err
    });
  }

  onBack(): void {
    this.router.navigate(['/admin']);
  }

  save(): void {
    console.log(this.adminForm);
    console.log('Saved: ' + JSON.stringify(this.adminForm.value));
  }

  populateData(): void{
    this.adminForm.patchValue({
      name: this.product?.plantName,
      type: this.product?.type,
      species: this.product?.plantSpecies,
      description: this.product?.plantDescription
    })
  }

  savePlantDetails(): void {
    const url = 'https://localhost:7211/plants/update/' + this.id;

    const httpOptions = {
      withCredentials: true
    };

    const payload = {
      plantName: this.adminForm.get('name')?.value,
      plantImage: this.product?.plantImage,
      type: this.adminForm.get('type')?.value,
      plantSpecies: this.adminForm.get('species')?.value,
      plantDescription: this.adminForm.get('description')?.value,
    };

    this.httpClient.post<any>(url, payload, httpOptions).subscribe((response: any) => {
      console.log(response.Message);
    },
      (error) => {
        console.error('Error updating plant:', error);
      });
  }

  editPicture(event: any): void {
    const input = event.target;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const formData = new FormData();
      formData.append('picture', file);

      const url = 'https://localhost:7211/plants/update/' + this.id + '/image';

      const httpOptions = {
        withCredentials: true
      };

      this.httpClient.post(url, formData, httpOptions).subscribe(
        (response: any) => {
          console.log(response.Message);
          // Add image refresh on success
        },
        (error) => {
          console.error('Error uploading picture:', error);
        }
      );
    }
  }

}
