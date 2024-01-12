import { Component, OnInit} from '@angular/core';
import { Subscription } from 'rxjs';
import { IPlant } from '../shared/IPlant';
import { LegumeService } from './legume.service';

@Component({
  selector: 'app-legume',
  templateUrl: './legume.component.html',
  styleUrls: ['./legume.component.css']
})
export class LegumeComponent implements OnInit {
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

  constructor(private productService: LegumeService) { }

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
