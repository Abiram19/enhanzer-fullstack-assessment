import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

// ── DTOs matching backend DashboardDataDto ─────────────────────────────────

export interface LatestPORowDto {
  id: number;
  netAmount: number;
  noOfItems: number;
}

export interface OldestPOItemRowDto {
  purchaseOrderId: number;
  itemName: string;
  noOfQuantity: number;
}

export interface ItemChartSliceDto {
  itemName: string;
  totalQuantity: number;
  color: string;
  percentage: number;
}

export interface DashboardDataDto {
  latestPurchaseOrders: LatestPORowDto[];
  oldestPurchaseOrderItems: OldestPOItemRowDto[];
  itemChart: ItemChartSliceDto[];
  totalItemQuantity: number;
}

// ── Service ────────────────────────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class DashboardService {
  /** Local backend – runs on http://localhost:5260 */
  private readonly apiBase = 'http://localhost:5260/api/Dashboard';

  constructor(private http: HttpClient) {}

  getDashboardData(companyCode: string): Observable<DashboardDataDto> {
    const params = new HttpParams().set('companyCode', companyCode);
    return this.http.get<DashboardDataDto>(this.apiBase, { params });
  }
}
