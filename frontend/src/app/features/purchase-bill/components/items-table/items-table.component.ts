import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PurchaseBillItem } from '../../models/purchase-bill.model';

@Component({
  selector: 'app-items-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './items-table.component.html',
  styleUrls: ['./items-table.component.css']
})
export class ItemsTableComponent {
  @Input() items: PurchaseBillItem[] = [];
}
