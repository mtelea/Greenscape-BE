import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IPlant } from 'src/app/shared/IPlant';
import { LegumeService } from '../legume.service';

@Component({
  selector: 'app-legume-details',
  templateUrl: './legume-details.component.html',
  styleUrls: ['./legume-details.component.css']
})
export class LegumeDetailsComponent implements OnInit {
  pageTitle = 'Product Detail';
  errorMessage = '';
  product: IPlant | undefined;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private productService: LegumeService) { }

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
    this.router.navigate(['/legume']);
  }

}
