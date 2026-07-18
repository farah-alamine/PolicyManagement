import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { PolicyType } from '../models/policy-type.model';

@Injectable({
  providedIn: 'root'
})
export class PolicyTypeService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/policy-types`;

  getAll(): Observable<PolicyType[]> {
    return this.http.get<PolicyType[]>(this.apiUrl);
  }
}