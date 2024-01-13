import { Component, OnInit } from '@angular/core';
import { FloriService } from '../flori.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IPlant } from 'src/app/shared/IPlant';

@Component({
  selector: 'app-flori-details',
  templateUrl: './flori-details.component.html',
  styleUrls: ['./flori-details.component.css']
})
export class FloriDetailsComponent implements OnInit {
  pageTitle = 'Product Detail';
  errorMessage = '';
  product: IPlant | undefined;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private productService: FloriService) { }

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
    this.router.navigate(['/flori']);
  }

}
