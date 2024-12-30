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
import { Designation } from '../../../models/designation.model'; // Assuming you have a Designation model
import { DesignationService } from '../../../services/designation/designation.service'; // Assuming you have a DesignationService
import { RouterLink, Router } from '@angular/router';

@Component({
  selector: 'app-create-designation',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
  ],
  templateUrl: './create-designation.component.html',
  styleUrls: ['./create-designation.component.css'],
})
export class CreateDesignationComponent {
  private _sharedService = inject(SharedService);
  private _designationService = inject(DesignationService);
  private router = inject(Router);

  newDesignation = new FormGroup({
    name: new FormControl('', Validators.required),
  });

  createNewDesignation() {
    if (this.newDesignation.valid) {
      const designation: Designation = this.newDesignation.value as Designation;

      this._designationService.createDesignation(designation).subscribe({
        next: (Response) => {
          this._sharedService.openSnackBar('Designation', 'created', true);

          this.newDesignation.reset();

          console.log('Designation created', Response);
          this.router.navigate(['/designation']);
        },

        error: (err) => {
          this._sharedService.openSnackBar(
            'Designation',
            'creation failed',
            false
          );

          this.newDesignation.reset();
          console.log('Designation creation failed', err);
        },
      });
    } else {
      console.log('Invalid form data\n', this.newDesignation);

      this._sharedService.openSnackBar('Form', 'is invalid', false);
      this.newDesignation.reset();
    }
  }
}
