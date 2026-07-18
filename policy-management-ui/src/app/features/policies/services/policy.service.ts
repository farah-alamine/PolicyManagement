import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { CreatePolicyRequest } from '../models/create-policy-request.model';
import { Policy } from '../models/policy.model';
import { PagedResponse } from '../../../shared/models/paged-response.model';

@Injectable({
  providedIn: 'root'
})
export class PolicyService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/policies`;

  getAll(
    pageNumber: number,
    pageSize: number
  ): Observable<PagedResponse<Policy>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<PagedResponse<Policy>>(
      this.apiUrl,
      { params }
    );
  }

  getById(recordGuid: string): Observable<Policy> {
  return this.http.get<Policy>(
    `${this.apiUrl}/${recordGuid}`
  );
}

  create(request: CreatePolicyRequest): Observable<Policy> {
    return this.http.post<Policy>(
      this.apiUrl,
      request
    );
  }

update(
  guid: string,
  request: CreatePolicyRequest
): Observable<void> {
  return this.http.put<void>(
    `${this.apiUrl}/${guid}`,
    request
  );
}

delete(guid: string): Observable<void> {
  return this.http.delete<void>(
    `${this.apiUrl}/${guid}`
  );
}
}