import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SharedService } from '../../../services/shared/shared.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { EmployeeService } from '../../../services/employee/employee.service';
import { Employee } from '../../../models/employee.model';

@Component({
  selector: 'app-create-employee',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
  ],
  templateUrl: './create-employee.component.html',
  styleUrl: './create-employee.component.css',
})
export class CreateEmployeeComponent {
  private _sharedService = inject(SharedService);
  private _employeeService = inject(EmployeeService);

  newEmployee = new FormGroup({
    officeEmployeeId: new FormControl('', Validators.required),
    name: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    phone: new FormControl('', Validators.required),
    address: new FormControl('', Validators.required),
    dob: new FormControl('', Validators.required),
    departmentId: new FormControl('', Validators.required),
    designationId: new FormControl('', Validators.required),
  });
  handleSubmit() {
    if (this.newEmployee.valid) {
      const employee: Employee = this.newEmployee.value as Employee;

      this._employeeService.createEmployee(employee).subscribe({
        next: (response: any) => {
          this._sharedService.openSnackBar('Employee', 'created', true);
          this.newEmployee.reset();
          console.log('Employee created', response.data);
        },
        error: (err) => {
          this._sharedService.openSnackBar(
            'Employee',
            'creation failed',
            false
          );
          console.error('Error creating employee:', err);
          this.newEmployee.reset();
        },
      });
    } else {
      console.log('Invalid form data\n', this.newEmployee);
      this._sharedService.openSnackBar('Form', 'is invalid', false);
      this.newEmployee.reset();
    }
  }
}
