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
        this.populateData(); // Call populateData here after product is fetched
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

}
