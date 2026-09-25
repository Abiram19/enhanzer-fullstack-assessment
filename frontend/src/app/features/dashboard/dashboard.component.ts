import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser, DecimalPipe } from '@angular/common';
import { Router } from '@angular/router';
import {
  DashboardService,
  DashboardDataDto,
  LatestPORowDto,
  OldestPOItemRowDto,
  ItemChartSliceDto
} from './services/dashboard.service';
import { AuthService } from '../../core/services/auth.service';

export interface SidebarItem {
  id: string;
  label: string;
  active?: boolean;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, DecimalPipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  activeSidebar = 'Shortcuts';
  dropdownOpen = false;

  sidebarItems: SidebarItem[] = [
    { id: 'shortcuts',     label: 'Shortcuts',     active: true },
    { id: 'core',          label: 'Core' },
    { id: 'finance',       label: 'Finance' },
    { id: 'assets',        label: 'Assets' },
    { id: 'procurement',   label: 'Procurement' },
    { id: 'inventory',     label: 'Inventory' },
    { id: 'manufacturing', label: 'Manufacturing' },
    { id: 'sales',         label: 'Sales' },
    { id: 'crm',           label: 'CRM' },
    { id: 'services',      label: 'Services' },
    { id: 'docs',          label: 'Docs' },
    { id: 'analytics',     label: 'Analytics' },
    { id: 'admin',         label: 'Admin' }
  ];

  // ── State ─────────────────────────────────────────────────
  loading    = true;
  error      = '';
  data: DashboardDataDto | null = null;

  // ── Computed helpers ──────────────────────────────────────
  get latestPOs(): LatestPORowDto[] {
    return this.data?.latestPurchaseOrders ?? [];
  }

  get oldestItems(): OldestPOItemRowDto[] {
    return this.data?.oldestPurchaseOrderItems ?? [];
  }

  get chartSlices(): ItemChartSliceDto[] {
    return this.data?.itemChart ?? [];
  }

  get totalQty(): number {
    return this.data?.totalItemQuantity ?? 0;
  }

  /** CSS conic-gradient string for the donut */
  get chartBackground(): string {
    if (!this.chartSlices.length) return '#e2e8f0';
    let parts: string[] = [];
    let cur = 0;
    for (const s of this.chartSlices) {
      const end = cur + s.percentage;
      parts.push(`${s.color} ${cur.toFixed(2)}% ${end.toFixed(2)}%`);
      cur = end;
    }
    return `conic-gradient(${parts.join(', ')})`;
  }

  constructor(
    private dashboardService: DashboardService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.loadData();
    }
  }

  loadData(): void {
    const companyCode = this.authService.getCompanyCode() ?? 'info@enhanzer.com';
    this.loading = true;
    this.error   = '';

    this.dashboardService.getDashboardData(companyCode).subscribe({
      next: (res) => {
        this.data    = res;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Dashboard API error:', err);
        this.error   = 'Could not load dashboard data. Please ensure the backend is running.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // ── Navigation ────────────────────────────────────────────
  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }

  onSidebarClick(item: SidebarItem): void {
    this.sidebarItems.forEach(i => i.active = false);
    item.active = true;
    this.activeSidebar = item.label;
    this.dropdownOpen = false;

    if (item.id === 'procurement') {
      this.router.navigate(['/purchase-bill']);
    }
  }

  goToPurchaseBill(): void {
    this.dropdownOpen = false;
    this.router.navigate(['/purchase-bill']);
  }
}
