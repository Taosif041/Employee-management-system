import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SharedService } from '../../../services/shared/shared.service';
import { Department } from '../../../models/department.model';
import { DepartmentService } from '../../../services/department/department.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-create-department',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
  ],
  templateUrl: './create-department.component.html',
  styleUrl: './create-department.component.css',
})
export class CreateDepartmentComponent {
  private _sharedService = inject(SharedService);
  private _departmentService = inject(DepartmentService);
  newDepartment = new FormGroup({
    name: new FormControl('', Validators.required),
  });

  createNewDepartment() {
    if (this.newDepartment.valid) {
      const department: Department = this.newDepartment.value as Department;
      this._departmentService.createDepartment(department).subscribe({
        next: (Response) => {
          this._sharedService.openSnackBar('Department', 'created', true);
          this.newDepartment.reset();
          console.log('Department created', Response);
        },
        error: (err) => {
          this._sharedService.openSnackBar(
            'Department',
            'creation failed',
            false
          );
          this.newDepartment.reset();
          console.log('Department creation failed', Response);
        },
      });
    } else {
      console.log('Invalid form data\n', this.newDepartment);
      this._sharedService.openSnackBar('Form', 'is invalid', false);
      this.newDepartment.reset();
    }
  }
}
