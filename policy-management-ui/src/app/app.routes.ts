import { Routes } from '@angular/router';

import { PolicyListComponent } from './features/policies/pages/policy-list/policy-list';

export const routes: Routes = [
  {
    path: 'policies',
    component: PolicyListComponent
  },
  {
    path: '',
    redirectTo: 'policies',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'policies'
  }
];