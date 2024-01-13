import { Component, OnInit } from '@angular/core';
import { IPlant } from 'src/app/shared/IPlant';
import { FructeService } from '../fructe.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-fructe-details',
  templateUrl: './fructe-details.component.html',
  styleUrls: ['./fructe-details.component.css']
})
export class FructeDetailsComponent implements OnInit {
  pageTitle = 'Product Detail';
  errorMessage = '';
  product: IPlant | undefined;
  
  constructor(private route: ActivatedRoute,
    private router: Router,
    private productService: FructeService) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.getProduct(id);
    }
  }

  getProduct(id: number): void {
    this.productService.getProduct(id).subscribe({
      next: product => this.product = product,
      error: err => this.errorMessage = err
    });
  }

  onBack(): void {
    this.router.navigate(['/fructe']);
  }


}
