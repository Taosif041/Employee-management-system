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
import { MatDatepickerModule } from '@angular/material/datepicker';

import { SharedService } from '../../../services/shared/shared.service';
import { EmployeeService } from '../../../services/employee/employee.service';
import { Employee } from '../../../models/employee.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-update-employee',
  imports: [
    CommonModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
  ],
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
  updatedEmployee: FormGroup = new FormGroup({
    employeeId: new FormControl(null),
    officeEmployeeId: new FormControl(null, Validators.required),
    name: new FormControl(null, Validators.required),
    email: new FormControl(null, [Validators.required, Validators.email]),
    phone: new FormControl(null, Validators.required),
    address: new FormControl(null, Validators.required),
    dob: new FormControl(null, Validators.required),
    departmentId: new FormControl(null, Validators.required),
    designationId: new FormControl(null, Validators.required),
  });

  constructor() {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.getEmployeeById(this.id);
  }

  getEmployeeById(id: number): void {
    this._employeeService.getEmployeeById(id).subscribe(
      (employee) => {
        this.selectedEmployee = employee;

        this.updatedEmployee.patchValue({
          employeeId: this.id,
          officeEmployeeId: employee.officeEmployeeId,
          name: employee.name,
          email: employee.email,
          phone: employee.phone,
          address: employee.address,
          dob: employee.dob,
          departmentId: employee.departmentId,
          designationId: employee.designationId,
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
        next: (response) => {
          this._sharedService.openSnackBar('Employee', 'updated', true);
          this.updatedEmployee.reset();
          console.log('Employee updated', response);
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
