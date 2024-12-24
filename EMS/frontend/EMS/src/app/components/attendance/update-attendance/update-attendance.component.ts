import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { SharedService } from '../../../services/shared/shared.service';
import { AttendanceService } from '../../../services/attendance/attendance.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Attendance } from '../../../models/attendance.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-update-attendance',
  imports: [CommonModule, MatButtonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './update-attendance.component.html',
  styleUrls: ['./update-attendance.component.css'],
})
export class UpdateAttendanceComponent implements OnInit {
  private _sharedService = inject(SharedService);
  private _attendanceService = inject(AttendanceService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  id: number = 0;
  selectedAttendance: Attendance | null = null;
  updatedAttendance: FormGroup = new FormGroup({}); // Initialize with an empty form group

  constructor() {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.selectedAttendance =
      this._attendanceService.getAttendanceById(this.id) || null;
    this.initializeForm();
  }

  // Initialize the form with the fetched attendance data
  initializeForm(): void {
    if (this.selectedAttendance) {
      this.updatedAttendance = new FormGroup({
        attendanceId: new FormControl(this.selectedAttendance.attendanceId),
        employeeId: new FormControl(
          this.selectedAttendance.employeeId,
          Validators.required
        ),
        date: new FormControl(
          this.selectedAttendance.date,
          Validators.required
        ),
        checkInTime: new FormControl(
          this.selectedAttendance.checkInTime,
          Validators.required
        ),
        checkOutTime: new FormControl(
          this.selectedAttendance.checkOutTime,
          Validators.required
        ),
        status: new FormControl(
          this.selectedAttendance.status,
          Validators.required
        ),
      });
    }
  }

  handleSubmit() {
    if (this.updatedAttendance.valid) {
      const editedAttendance: Attendance = this.updatedAttendance
        .value as Attendance;
      console.log(editedAttendance);

      this._attendanceService
        .updateAttendance(this.id, editedAttendance)
        .subscribe({
          next: (response: any) => {
            this._sharedService.openSnackBar('Attendance', 'updated', true);
            console.log('Attendance updated', response.data);
            this.updatedAttendance.reset();
            this.router.navigate(['/attendance']);
          },
          error: (err) => {
            this._sharedService.openSnackBar(
              'Attendance',
              'update failed',
              false
            );
            console.error('Error updating Attendance:', err);
          },
        });
    } else {
      this._sharedService.openSnackBar('Form', 'is invalid', false);
    }
  }
}
