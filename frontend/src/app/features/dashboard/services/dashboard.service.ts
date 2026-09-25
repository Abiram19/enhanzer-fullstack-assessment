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

export interface TableWidgetRowDto {
  orderNo: string;
  product: string;
  dueDate: string;
  daysLate: string;
  purchaseOrderId?: number;
  netAmount?: number;
  noOfItems?: number;
}

export interface TableWidgetDto {
  title: string;
  period: string;
  subtitle: string;
  headers: string[];
  rows: TableWidgetRowDto[];
}

export interface ListWidgetItemDto {
  title: string;
  subtitle: string;
  updatedText: string;
  badgeText?: string | null;
  amount: string;
  isNegative?: boolean;
  purchaseOrderId?: number;
  quantity?: number;
}

export interface ListWidgetDto {
  title: string;
  period: string;
  items: ListWidgetItemDto[];
}

export interface ChartSliceDto {
  code: string;
  label: string;
  formattedValue: string;
  value: number;
  percentage: number;
  color: string;
}

export interface ChartWidgetDto {
  title: string;
  period: string;
  totalValue: string;
  totalLabel: string;
  slices: ChartSliceDto[];
}

export interface DashboardDataDto {
  latestPurchaseOrders: LatestPurchaseOrderDto[];
  oldestPurchaseOrderItems: OldestPurchaseOrderItemDto[];
  purchaseOrderItemChart: PurchaseOrderItemChartDto[];
  tableWidget?: TableWidgetDto;
  listWidget?: ListWidgetDto;
  chartWidget?: ChartWidgetDto;
  poTableWidget?: TableWidgetDto;
  poListWidget?: ListWidgetDto;
  poChartWidget?: ChartWidgetDto;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private http: HttpClient) { }

  getDashboardData(companyCode: string): Observable<DashboardDataDto> {
    return this.http.get<DashboardDataDto>(
      `https://enhanzer-fullstack-assessment.onrender.com/api/Dashboard?companyCode=${encodeURIComponent(companyCode)}`
    );
  }
}
