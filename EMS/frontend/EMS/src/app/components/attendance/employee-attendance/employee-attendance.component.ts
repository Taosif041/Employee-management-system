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

@Component({
  selector: 'app-employee-attendance',
  imports: [MatCardModule, MatListModule, RouterLink],
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
    // Check if data is already in sessionStorage
    const storedAttendances = sessionStorage.getItem(
      `attendance_${this.employeeId}`
    );

    if (storedAttendances) {
      // If found in sessionStorage, use the data from there
      this.attendances = JSON.parse(storedAttendances);
      console.log('Loaded attendance from sessionStorage');
    } else {
      // If not in sessionStorage, fetch data from the service
      const result = this._attendanceService.getDesignationById(
        this.employeeId
      );
      if (result && result.length > 0) {
        this.attendances = result;
        // Store the fetched data in sessionStorage for future use
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
