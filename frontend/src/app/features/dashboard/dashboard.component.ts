import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { DashboardService, DashboardDataDto, LatestPurchaseOrderDto, OldestPurchaseOrderItemDto, PurchaseOrderItemChartDto } from './services/dashboard.service';
import { AuthService } from '../../core/services/auth.service';

export interface NavItem {
  id: string;
  label: string;
  icon: string;
  active?: boolean;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  selectedTimeframe = 'All Time';
  activeNav = 'Dashboard';

  navItems: NavItem[] = [
    { id: 'dashboard', label: 'Dashboard', icon: 'bar-chart', active: true },
    { id: 'purchase-bill', label: 'Purchase Bill', icon: 'file-text' }
  ];

  dashboardData: DashboardDataDto | null = null;
  loading = true;
  error = '';

  constructor(
    private dashboardService: DashboardService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.loadDashboardData();
    }
  }

  loadDashboardData(): void {
    const companyCode = this.authService.getCompanyCode();
    if (!companyCode) {
      this.error = 'No company code found. Please log in.';
      this.loading = false;
      return;
    }

    this.dashboardService.getDashboardData(companyCode).subscribe({
      next: (data) => {
        this.dashboardData = data;
        this.loading = false;
        this.cdr.detectChanges();
        
        // Simple manual pie chart calculation for donut chart
        if (this.dashboardData.purchaseOrderItemChart && this.dashboardData.purchaseOrderItemChart.length > 0) {
            this.calculateChartConicGradient();
        }
      },
      error: (err) => {
        console.error('Error loading dashboard data:', err);
        this.error = 'Failed to load dashboard data.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  setActiveNav(item: NavItem): void {
    this.navItems.forEach(i => i.active = false);
    item.active = true;
    this.activeNav = item.label;
    
    if (item.id === 'purchase-bill') {
      this.router.navigate(['/purchase-bill']);
    } else if (item.id === 'dashboard') {
      this.router.navigate(['/dashboard']);
    }
  }

  // Helper method to generate a CSS conic gradient for a simple donut chart without a library
  chartBackground: string = '';
  chartColors = ['#00a0dc', '#007bbd', '#133e5c', '#26c268', '#f39c12', '#e74c3c', '#9b59b6'];

  calculateChartConicGradient(): void {
    if (!this.dashboardData || !this.dashboardData.purchaseOrderItemChart) return;
    
    const items = this.dashboardData.purchaseOrderItemChart;
    const total = items.reduce((sum, item) => sum + item.totalQuantity, 0);
    
    if (total === 0) return;

    let gradientParts: string[] = [];
    let currentPercentage = 0;

    items.forEach((item, index) => {
      const percentage = (item.totalQuantity / total) * 100;
      const color = this.chartColors[index % this.chartColors.length];
      
      const start = currentPercentage;
      const end = currentPercentage + percentage;
      
      gradientParts.push(`${color} ${start}% ${end}%`);
      currentPercentage = end;
    });

    this.chartBackground = `conic-gradient(${gradientParts.join(', ')})`;
  }

  getChartColor(index: number): string {
     return this.chartColors[index % this.chartColors.length];
  }
}
