import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { SharedService } from '../../../services/shared/shared.service';
import { EmployeeService } from '../../../services/employee/employee.service';
import { Employee } from '../../../models/employee.model';

@Component({
  selector: 'app-update-employee',
  imports: [CommonModule, MatButtonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './update-employee.component.html',
  styleUrl: './update-employee.component.css',
})
export class UpdateEmployeeComponent implements OnInit {
  private _sharedService = inject(SharedService);
  private _employeeService = inject(EmployeeService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  id: number = 0;
  selectedEmployee: Employee | null = null;
  updatedEmployee!: FormGroup;

  constructor() {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.getEmployeeById(this.id);
  }

  getEmployeeById(id: number): void {
    this._employeeService.getEmployeeById(id).subscribe(
      (employee: any) => {
        this.selectedEmployee = employee.data;

        // Initialize form dynamically
        this.updatedEmployee = new FormGroup({
          employeeId: new FormControl(this.id),
          officeEmployeeId: new FormControl(
            this.selectedEmployee?.officeEmployeeId,
            Validators.required
          ),
          name: new FormControl(
            this.selectedEmployee?.name,
            Validators.required
          ),
          email: new FormControl(this.selectedEmployee?.email, [
            Validators.required,
            Validators.email,
          ]),
          phone: new FormControl(
            this.selectedEmployee?.phone,
            Validators.required
          ),
          address: new FormControl(
            this.selectedEmployee?.address,
            Validators.required
          ),
          dob: new FormControl(this.selectedEmployee?.dob, Validators.required),

          departmentId: new FormControl(
            this.selectedEmployee?.departmentId,
            Validators.required
          ),
          designationId: new FormControl(
            this.selectedEmployee?.designationId,
            Validators.required
          ),
        });
      },
      (error) => {
        console.error('Error fetching employee by ID:', error);
      }
    );
  }

  handleSubmit() {
    if (this.updatedEmployee.valid) {
      const editedEmployee: Employee = this.updatedEmployee.value as Employee;

      this._employeeService.updateEmployee(this.id, editedEmployee).subscribe({
        next: (response: any) => {
          this._sharedService.openSnackBar('Employee', 'updated', true);
          this.updatedEmployee.reset();
          console.log('Employee updated', response.data);
          this.router.navigate(['/employee']);
        },
        error: (err) => {
          this._sharedService.openSnackBar('Employee', 'update failed', false);
          console.error('Error updating employee:', err);
        },
      });
    } else {
      this._sharedService.openSnackBar('Form', 'is invalid', false);
    }
  }
}
