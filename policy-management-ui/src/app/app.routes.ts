import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout/main-layout';
import { PolicyList } from './features/policies/pages/policy-list/policy-list';
import { CreatePolicy } from './features/policies/pages/create-policy/create-policy';
import { EditPolicy } from './features/policies/pages/edit-policy/edit-policy';
import { PolicyDetails } from './features/policies/pages/policy-details/policy-details';

export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
    {
      path: '',
      redirectTo: 'policies',
      pathMatch: 'full'
    },
    {
      path: 'policies',
      component: PolicyList
    },
    {
      path: 'policies/create',
      component: CreatePolicy
    },
    {
      path: 'policies/:id',
      component: PolicyDetails
    },
    {
      path: 'policies/:id/edit',
      component: EditPolicy
    }
  ]
  },
  {
    path: '**',
    redirectTo: 'policies'
  }
];