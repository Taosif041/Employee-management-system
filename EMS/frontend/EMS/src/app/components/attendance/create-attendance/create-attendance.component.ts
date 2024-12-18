import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SharedService } from '../../../services/shared/shared.service';
import { AttendanceService } from '../../../services/attendance/attendance.service';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { Attendance } from '../../../models/attendance.model';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-create-attendance',
  imports: [
    MatButtonModule,
    MatFormFieldModule,
    MatCardModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
  ],
  templateUrl: './create-attendance.component.html',
  styleUrls: ['./create-attendance.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateAttendanceComponent {
  private _sharedService = inject(SharedService);
  private _attendanceService = inject(AttendanceService);
  private router = inject(Router);

  newAttendance = new FormGroup({
    employeeId: new FormControl('', Validators.required),
    date: new FormControl('', Validators.required),
    checkInTime: new FormControl(''),
    checkOutTime: new FormControl(''),
    status: new FormControl('', Validators.required),
  });

  createNewAttendance() {
    if (this.newAttendance.valid) {
      const attendance: Attendance = this.newAttendance.value as Attendance;
      attendance.checkInTime = attendance.date + 'T' + attendance.checkInTime;
      attendance.checkOutTime = attendance.date + 'T' + attendance.checkOutTime;

      console.log(attendance);

      this._attendanceService.createAttendance(attendance).subscribe({
        next: (response) => {
          this._sharedService.openSnackBar('Attendance', 'created', true);
          this.newAttendance.reset();
          console.log('Attendance created', response);
          this.router.navigate(['/attendance']);
        },
        error: (err) => {
          this._sharedService.openSnackBar(
            'Attendance',
            'creation failed',
            false
          );
          this.newAttendance.reset();
          console.log('Attendance creation failed', err);
        },
      });
    } else {
      console.log('Invalid form data', this.newAttendance);
      this._sharedService.openSnackBar('Form', 'is invalid', false);
      this.newAttendance.reset();
    }
  }
}
