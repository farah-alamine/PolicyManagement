import { Component } from '@angular/core';
import { PageHeader } from '../../../../shared/components/page-header/page-header';

@Component({
  selector: 'app-create-policy',
  standalone: true,
  imports: [PageHeader],
  templateUrl: './create-policy.html',
  styleUrl: './create-policy.css'
})
export class CreatePolicy {}