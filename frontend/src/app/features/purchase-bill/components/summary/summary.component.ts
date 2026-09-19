import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PurchaseBillItem } from '../../models/purchase-bill.model';

@Component({
  selector: 'app-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnChanges {
  @Input() items: PurchaseBillItem[] = [];

  totalItems = 0;
  totalQty = 0;

  grossTotal = 0;
  itemDiscount = 0;
  overallDiscount = 0;
  totalDiscount = 0;
  totalBeforeTax = 0;

  totalTax = 0;
  suspendedTax = 0;

  netTotal = 0;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['items']) {
      this.calculateSummary();
    }
  }

  calculateSummary(): void {
    this.totalItems = this.items.length;
    this.totalQty = this.items.reduce((sum, item) => sum + item.qty, 0);

    this.grossTotal = this.items.reduce((sum, item) => sum + item.totalSelling, 0);
    this.itemDiscount = this.items.reduce((sum, item) => sum + ((item.standardCost * item.qty) - item.totalCost), 0);
    this.totalDiscount = this.itemDiscount + this.overallDiscount;
    this.totalBeforeTax = this.grossTotal - this.totalDiscount;
    this.netTotal = this.totalBeforeTax + this.totalTax;
  }
}
