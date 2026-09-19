import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PurchaseBillItem } from '../models/purchase-bill.model';

export interface CreatePurchaseBillRequest {
  companyCode?: string;
  items: PurchaseBillItem[];
}

@Injectable({
  providedIn: 'root'
})
export class PurchaseBillService {
  private apiUrl = 'http://localhost:5260/api/PurchaseBills';

  constructor(private http: HttpClient) {}

  submitBill(request: CreatePurchaseBillRequest): Observable<any> {
    return this.http.post(this.apiUrl, request);
  }

  getLatestBill(companyCode: string): Observable<any> {
    return this.http.get(`${this.apiUrl}?companyCode=${encodeURIComponent(companyCode)}`);
  }
}
