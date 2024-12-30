import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../../services/shared/shared.service';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { DesignationService } from '../../../services/designation/designation.service';
import { Designation } from '../../../models/designation.model';

@Component({
  selector: 'app-update-designation',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './update-designation.component.html',
  styleUrl: './update-designation.component.css',
})
export class UpdateDesignationComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private _designationService = inject(DesignationService);
  private _sharedService = inject(SharedService);
  private router = inject(Router);

  id: number = 0;
  selectedDesignation: Designation | null = null;
  updatedDesignation!: FormGroup;
  constructor() {}

  ngOnInit() {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    console.log('Id ->', this.id);
    this.selectedDesignation =
      this._designationService.getDesignationById(this.id) || null;
    console.log(this.selectedDesignation);
    this.initializeForm();
  }

  initializeForm(): void {
    if (this.selectedDesignation) {
      this.updatedDesignation = new FormGroup({
        designationId: new FormControl(this.selectedDesignation.designationId),
        name: new FormControl(
          this.selectedDesignation.name,
          Validators.required
        ),
      });
    } else {
      console.log('joy bangla');
    }
  }

  handleSubmit() {
    if (this.updatedDesignation.valid) {
      const editedDesignation: Designation = this.updatedDesignation
        .value as Designation;

      this._designationService
        .updateDesignation(editedDesignation, this.id)
        .subscribe({
          next: (response) => {
            this._sharedService.openSnackBar('Designation', 'updated', true);
            console.log('Designation updated', response);
            this.updatedDesignation.reset();
            this.router.navigate(['/designation']);
          },
          error: (err) => {
            this._sharedService.openSnackBar(
              'Designation',
              'update failed',
              false
            );
            console.error('Error updating Designation:', err);
          },
        });
    } else {
      this._sharedService.openSnackBar('Form', 'is invalid', false);
    }
  }
}
