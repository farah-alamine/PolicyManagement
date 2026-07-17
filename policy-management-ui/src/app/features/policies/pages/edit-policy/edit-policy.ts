import { Component } from '@angular/core';
import { PageHeader } from '../../../../shared/components/page-header/page-header';

@Component({
  selector: 'app-edit-policy',
  standalone: true,
  imports: [PageHeader],
  templateUrl: './edit-policy.html',
  styleUrl: './edit-policy.css'
})
export class EditPolicy {}