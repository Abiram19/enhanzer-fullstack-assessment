import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ItemEntryComponent } from './components/item-entry/item-entry.component';
import { ItemsTableComponent } from './components/items-table/items-table.component';
import { SummaryComponent } from './components/summary/summary.component';
import { PurchaseBillItem } from './models/purchase-bill.model';
import { PurchaseBillService } from './services/purchase-bill.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-purchase-bill',
  standalone: true,
  imports: [CommonModule, ItemEntryComponent, ItemsTableComponent, SummaryComponent],
  templateUrl: './purchase-bill.component.html',
  styleUrls: ['./purchase-bill.component.css']
})
export class PurchaseBillComponent implements OnInit {
  items: PurchaseBillItem[] = [];
  activeTab = 'Items';

  isSubmitting = false;
  submitMessage = '';
  submitError = false;

  constructor(
    private purchaseBillService: PurchaseBillService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadLatestBill();
  }

  loadLatestBill() {
    const companyCode = this.authService.getCompanyCode();
    if (!companyCode) return;

    this.purchaseBillService.getLatestBill(companyCode).subscribe({
        next: (bill: any) => {
          if (bill && bill.message) {
            // No items yet
            this.items = [];
            this.cdr.detectChanges();
            return;
          }
          this.items = (bill.items || []).map((i: any) => ({
            ...i,
            item: i.itemName || i.item
          }));
          this.cdr.detectChanges();
        },
        error: (err: HttpErrorResponse) => {
          console.error('Error loading latest bill:', err);
          this.cdr.detectChanges();
        }
    });
  }

  setTab(tab: string) {
    this.activeTab = tab;
  }

  resetAll() {
    this.loadLatestBill();
    this.submitMessage = '';
    this.submitError = false;
  }

  newBill() {
    this.items = [];
    this.submitMessage = '';
    this.submitError = false;
  }

  close() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  onItemAdded(item: PurchaseBillItem) {
    this.items = [...this.items, item];
  }

  submitBill() {
    if (this.items.length === 0) {
      this.submitMessage = 'Cannot submit an empty bill.';
      this.submitError = true;
      return;
    }

    this.isSubmitting = true;
    this.submitMessage = '';
    this.submitError = false;

    const companyCode = this.authService.getCompanyCode() || undefined;

      this.purchaseBillService.submitBill({ companyCode, items: this.items }).subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.submitMessage = 'Purchase Bill submitted successfully!';
          this.submitError = false;
          this.cdr.detectChanges();
          this.loadLatestBill(); // Reload the saved bill from the backend
        },
      error: (err) => {
        this.isSubmitting = false;
        this.submitError = true;
        
        // Parse the exact validation errors if they exist
        let errorMsg = 'An error occurred while submitting.';
        if (err.error) {
          if (err.error.errors && typeof err.error.errors === 'object') {
            // Usually problem details have { "Items[0].Qty": ["The field..."] }
            // or we returned { errors: ["..."] } directly
            if (Array.isArray(err.error.errors)) {
               errorMsg = err.error.errors.join(' ');
            } else {
               const msgs = [];
               for (const key in err.error.errors) {
                 msgs.push(`${key}: ${err.error.errors[key].join(', ')}`);
               }
               errorMsg = msgs.join(' | ');
            }
          } else if (err.error.message) {
            errorMsg = err.error.message;
          }
        }
        
        this.submitMessage = errorMsg;
        console.error('Submit error:', err);
        this.cdr.detectChanges(); // Ensure the spinner stops spinning!
      }
    });
  }
}
