import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DepartmentService } from '../../../services/department/department.service';
import { SharedService } from '../../../services/shared/shared.service';
import { Department } from '../../../models/department.model';
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

@Component({
  selector: 'app-update-department',
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
  templateUrl: './update-department.component.html',
  styleUrl: './update-department.component.css',
})
export class UpdateDepartmentComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private _departmentService = inject(DepartmentService);
  private _sharedService = inject(SharedService);
  private router = inject(Router);

  id: number = 0;
  selectedDepartment: Department | null = null;
  updatedDepartment!: FormGroup;
  constructor() {}

  ngOnInit() {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.selectedDepartment =
      this._departmentService.getDepartmentById(this.id) || null;
    this.initializeForm();
  }

  initializeForm(): void {
    if (this.selectedDepartment) {
      this.updatedDepartment = new FormGroup({
        departmentId: new FormControl(this.selectedDepartment.departmentId),
        name: new FormControl(
          this.selectedDepartment.name,
          Validators.required
        ),
      });
    }
  }

  handleSubmit() {
    if (this.updatedDepartment.valid) {
      const editedDepartment: Department = this.updatedDepartment
        .value as Department;

      this._departmentService
        .updateDepartment(editedDepartment, this.id)
        .subscribe({
          next: (response: any) => {
            this._sharedService.openSnackBar('Department', 'updated', true);
            console.log('Department updated', response);
            this.updatedDepartment.reset();
            this.router.navigate(['/department']);
          },
          error: (err) => {
            this._sharedService.openSnackBar(
              'Department',
              'update failed',
              false
            );
            console.error('Error updating department:', err);
          },
        });
    } else {
      this._sharedService.openSnackBar('Form', 'is invalid', false);
    }
  }
}
