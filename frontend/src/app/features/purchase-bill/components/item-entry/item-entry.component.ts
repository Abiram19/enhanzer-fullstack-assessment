import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../../../core/services/auth.service';
import { LocationDto } from '../../../../core/models/login.model';
import { PurchaseBillItem } from '../../models/purchase-bill.model';

@Component({
  selector: 'app-item-entry',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './item-entry.component.html',
  styleUrls: ['./item-entry.component.css']
})
export class ItemEntryComponent implements OnInit {
  @Output() itemAdded = new EventEmitter<PurchaseBillItem>();

  itemForm: FormGroup;

  allowedItems = [
    'Mango',
    'Apple',
    'Banana',
    'Orange',
    'Grapes',
    'Kiwi',
    'Strawberry'
  ];

  filteredItems: string[] = [];
  showAutocomplete = false;

  locations: LocationDto[] = [];

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private authService: AuthService
  ) {
    this.itemForm = this.fb.group({
      item: ['', Validators.required],
      batch: [''], // Batch is optional
      standardCost: [0, [Validators.required, Validators.min(0)]],
      standardPrice: [0, [Validators.required, Validators.min(0)]],
      margin: [{ value: 0, disabled: true }],
      qty: [1, [Validators.required, Validators.min(1)]],
      freeQty: [0, Validators.min(0)],
      discount: [0, [Validators.min(0), Validators.max(100)]],
      totalCost: [{ value: 0, disabled: true }],
      totalSelling: [{ value: 0, disabled: true }]
    });
  }

  ngOnInit(): void {
    this.fetchLocations();

    this.itemForm.get('item')?.valueChanges.subscribe(value => {
      this.filterItems(value);
    });

    // Real-time calculations
    this.itemForm.valueChanges.subscribe(() => {
      this.calculateDerivedValues();
    });
  }

  calculateDerivedValues(): void {
    const standardCost =
      Number(this.itemForm.get('standardCost')?.value) || 0;

    const standardPrice =
      Number(this.itemForm.get('standardPrice')?.value) || 0;

    const qty =
      Number(this.itemForm.get('qty')?.value) || 0;

    const discount =
      Number(this.itemForm.get('discount')?.value) || 0;

    const margin = standardPrice - standardCost;

    const totalSelling = standardPrice * qty;

    // Discount percentage applied to cost
    const totalCost =
      standardCost * qty * (1 - discount / 100);

    this.itemForm.patchValue(
      {
        margin: margin,
        totalCost: totalCost,
        totalSelling: totalSelling
      },
      { emitEvent: false }
    );
  }

  fetchLocations(): void {
    const companyCode = this.authService.getCompanyCode();

    if (!companyCode) {
      console.warn(
        'No company code found. Cannot fetch locations.'
      );
      return;
    }

    this.http
      .get<LocationDto[]>(
        `https://enhanzer-fullstack-assessment.onrender.com/api/locations?companyCode=${companyCode}`
      )
      .subscribe({
        next: (data) => {
          this.locations = data;
        },
        error: (err) => {
          console.error(
            'Failed to fetch locations',
            err
          );
        }
      });
  }

  filterItems(value: string): void {
    if (!value) {
      this.filteredItems = this.allowedItems;
    } else {
      const filterValue = value.toLowerCase();

      this.filteredItems = this.allowedItems.filter(option =>
        option.toLowerCase().includes(filterValue)
      );
    }
  }

  selectItem(item: string): void {
    this.itemForm.patchValue({
      item
    });

    this.showAutocomplete = false;
  }

  hideAutocomplete(): void {
    setTimeout(() => {
      this.showAutocomplete = false;
    }, 200);
  }

  onAdd(): void {
    if (this.itemForm.valid) {
      const rawValue = this.itemForm.getRawValue();

      const newItem: PurchaseBillItem = {
        item: rawValue.item,
        batch: rawValue.batch || '',
        standardCost: Number(rawValue.standardCost) || 0,
        standardPrice: Number(rawValue.standardPrice) || 0,
        margin: Number(rawValue.margin) || 0,
        qty: Number(rawValue.qty) || 0,
        freeQty: Number(rawValue.freeQty) || 0,
        discount: Number(rawValue.discount) || 0,
        totalCost: Number(rawValue.totalCost) || 0,
        totalSelling: Number(rawValue.totalSelling) || 0
      };

      this.itemAdded.emit(newItem);

      // Reset form to defaults while retaining the current batch
      const currentBatch = rawValue.batch;

      this.itemForm.reset({
        item: '',
        batch: currentBatch,
        standardCost: 0,
        standardPrice: 0,
        margin: 0,
        qty: 1,
        freeQty: 0,
        discount: 0,
        totalCost: 0,
        totalSelling: 0
      });
    } else {
      this.itemForm.markAllAsTouched();
    }
  }
}