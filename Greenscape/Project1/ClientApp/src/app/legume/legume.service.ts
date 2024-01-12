import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, map, tap, throwError } from "rxjs";
import { IPlant } from "../shared/IPlant";

@Injectable({
  providedIn: 'root'
})
export class LegumeService {
  private productUrl = 'https://localhost:7211/plants/getByType/legume';

  constructor(private http: HttpClient) { }

  getProducts(): Observable<IPlant[]> {
    const httpOptions = {
      withCredentials: true
    };

    return this.http.get<IPlant[]>(this.productUrl, httpOptions)
      .pipe(
        tap(data => console.log('All: ', JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getProduct(id: number): Observable<IPlant | undefined> {
    return this.getProducts()
      .pipe(
        map((products: IPlant[]) => products.find(p => p.plantID === id))
      );
  }

  private handleError(err: HttpErrorResponse): Observable<never> {
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(() => errorMessage);
  }

}
