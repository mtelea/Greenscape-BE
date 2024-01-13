import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IPlant } from '../shared/IPlant';
import { AdminService } from './admin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  pageTitle = 'Product List';
  imageWidth = 50;
  imageMargin = 2;
  showImage = false;
  errorMessage = '';
  sub!: Subscription;
  
  private _listFilter = '';
  get listFilter(): string {
    return this._listFilter;
  }
  set listFilter(value: string) {
    this._listFilter = value;
    this.filteredProducts = this.performFilter(value);
  }

  filteredProducts: IPlant[] = [];
  products: IPlant[] = [];

  constructor(private productService: AdminService) { }

  performFilter(filterBy: string): IPlant[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.products.filter((product: IPlant) =>
      product.plantName.toLocaleLowerCase().includes(filterBy));
  }

  ngOnInit(): void {
    this.sub = this.productService.getProducts().subscribe({
      next: products => {
        this.products = products;
        this.filteredProducts = this.products;
      },
      error: err => this.errorMessage = err
    });
  }
  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

}
