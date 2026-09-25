import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LatestPurchaseOrderDto {
  id: number;
  netAmount: number;
  noOfItems: number;
}

export interface OldestPurchaseOrderItemDto {
  purchaseOrderId: number;
  itemName: string;
  noOfQuantity: number;
}

export interface PurchaseOrderItemChartDto {
  itemName: string;
  totalQuantity: number;
}

export interface DashboardDataDto {
  latestPurchaseOrders: LatestPurchaseOrderDto[];
  oldestPurchaseOrderItems: OldestPurchaseOrderItemDto[];
  purchaseOrderItemChart: PurchaseOrderItemChartDto[];
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private http: HttpClient) { }

  getDashboardData(companyCode: string): Observable<DashboardDataDto> {
    return this.http.get<DashboardDataDto>(`https://enhanzer-fullstack-assessment.onrender.com/api/Dashboard?companyCode=${encodeURIComponent(companyCode)}`);

  }
}

