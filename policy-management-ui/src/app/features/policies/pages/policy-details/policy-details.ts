import { Component } from '@angular/core';
import { PageHeader } from '../../../../shared/components/page-header/page-header';

@Component({
  selector: 'app-policy-details',
  standalone: true,
  imports: [PageHeader],
  templateUrl: './policy-details.html',
  styleUrl: './policy-details.css'
})
export class PolicyDetails {}