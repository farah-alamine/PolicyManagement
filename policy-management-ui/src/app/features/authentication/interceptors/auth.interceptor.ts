import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { AuthenticationService } from '../services/authentication.service';
import { environment } from '../../../../environments/environment';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authenticationService = inject(AuthenticationService);
  const token = authenticationService.getToken();

  const isApiRequest = request.url.startsWith(environment.apiUrl);
  const isLoginRequest = request.url.includes('/auth/login');

  if (!token || !isApiRequest || isLoginRequest) {
    return next(request);
  }

  const authenticatedRequest = request.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(authenticatedRequest);
};