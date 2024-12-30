import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
} from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';

import { AttendanceService } from '../../../services/attendance/attendance.service';
import { Attendance } from '../../../models/attendance.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-attendance',
  imports: [MatCardModule, MatListModule, RouterLink, CommonModule],
  templateUrl: './employee-attendance.component.html',
  styleUrls: ['./employee-attendance.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmployeeAttendanceComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private _attendanceService = inject(AttendanceService);

  attendances: Attendance[] = [];
  employeeId: number = 0;

  ngOnInit() {
    this.employeeId = +this.route.snapshot.paramMap.get('id')!;
    this.getAllAttendanceByEmployeeId();
  }

  getAllAttendanceByEmployeeId() {
    const storedAttendances = sessionStorage.getItem(
      `attendance_${this.employeeId}`
    );

    if (storedAttendances) {
      this.attendances = JSON.parse(storedAttendances);
      console.log('Loaded attendance from sessionStorage');
    } else {
      const result = this._attendanceService.getDesignationById(
        this.employeeId
      );
      if (result && result.length > 0) {
        this.attendances = result;
        sessionStorage.setItem(
          `attendance_${this.employeeId}`,
          JSON.stringify(this.attendances)
        );
        console.log('Attendance data saved to sessionStorage');
      } else {
        console.warn('No attendance records found for the given employee ID.');
        this.attendances = [];
      }
    }
  }
}
