import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout/main-layout';
import { PolicyList } from './features/policies/pages/policy-list/policy-list';
import { CreatePolicy } from './features/policies/pages/create-policy/create-policy';
import { EditPolicy } from './features/policies/pages/edit-policy/edit-policy';
import { PolicyDetails } from './features/policies/pages/policy-details/policy-details';
import { LoginComponent } from './features/authentication/pages/login.component';
import { authGuard } from './features/authentication/guards/auth.guard';
import { guestGuard } from './features/authentication/guards/guest.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [guestGuard]
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: '',
    component: MainLayout,
    canActivate: [authGuard],
    children: [
      {
        path: 'policies',
        component: PolicyList
      },
      {
        path: 'policies/create',
        component: CreatePolicy
      },
      {
        path: 'policies/:guid/edit',
        component: EditPolicy
      },
      {
        path: 'policies/:guid',
        component: PolicyDetails
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];