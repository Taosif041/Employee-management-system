import { Component, OnInit, Directive, inject } from '@angular/core';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';

import { EmployeeService } from '../../../services/employee/employee.service';
import { Employee } from '../../../models/employee.model';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SharedService } from '../../../services/shared/shared.service';

@Component({
  selector: 'app-update-employee',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatListModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
  ],
  templateUrl: './update-employee.component.html',
  styleUrl: './update-employee.component.css',
})
export class UpdateEmployeeComponent implements OnInit {
  private _sharedService = inject(SharedService);
  private _employeeService = inject(EmployeeService);

  id: number = 0;
  selectedEmployee: Employee | null = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    // Retrieve the 'id' parameter from the URL
    this.id = +this.route.snapshot.paramMap.get('id')!; // Use the correct parameter name ('id')
    this.getEmployeeById(this.id);
    console.log('employeeId ->', this.id);
    console.log('employee details ->', this.selectedEmployee);
  }

  getEmployeeById(id: number): void {
    this._employeeService.getEmployeeById(id).subscribe(
      (employee) => {
        this.selectedEmployee = employee;
      },
      (error) => {
        console.error('Error fetching employee by ID:', error);
      }
    );
  }
  updatedEmployee = new FormGroup({
    employeeId: new FormControl(this.id),
    officeEmployeeId: new FormControl(
      this.selectedEmployee?.officeEmployeeId,
      Validators.required
    ),
    name: new FormControl(this.selectedEmployee?.name, Validators.required),
    email: new FormControl(this.selectedEmployee?.email, [
      Validators.required,
      Validators.email,
    ]),
    phone: new FormControl(this.selectedEmployee?.phone, Validators.required),
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

  print() {
    console.log(this.updatedEmployee);
  }
  handleSubmit() {
    if (this.updatedEmployee.valid) {
      const editedEmployee: Employee = this.updatedEmployee.value as Employee;

      this._employeeService.updateEmployee(this.id, editedEmployee).subscribe({
        next: (response) => {
          this._sharedService.openSnackBar('Employee', 'updated', true);
          this.updatedEmployee.reset();
          console.log('Employee updated', response);
        },
        error: (err) => {
          this._sharedService.openSnackBar('Employee', 'update failed', false);
          console.error('Error updating employee:', err);
          this.updatedEmployee.reset();
        },
      });
    } else {
      this._sharedService.openSnackBar('Form', 'is invalid', false);
      this.updatedEmployee.reset();
    }
  }
}
