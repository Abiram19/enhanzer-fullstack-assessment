import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'purchase-bill',
    canActivate: [authGuard],
    loadComponent: () => import('./features/purchase-bill/purchase-bill.component').then(m => m.PurchaseBillComponent)
  },
  {
    path: '**',
    redirectTo: ''
  }
];

