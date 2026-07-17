import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { PagedResponse } from '../../../shared/models/paged-response.model';
import { Policy } from '../models/policy.model';

@Injectable({
  providedIn: 'root'
})
export class PolicyService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/policies`;

  getPolicies(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResponse<Policy>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<PagedResponse<Policy>>(this.apiUrl, { params });
  }
}