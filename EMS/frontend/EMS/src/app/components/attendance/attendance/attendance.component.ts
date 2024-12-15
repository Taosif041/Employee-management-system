import { AfterViewInit, Component, ViewChild, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { Attendance } from '../../../models/attendance.model';
import { AttendanceService } from '../../../services/attendance/attendance.service';

@Component({
  selector: 'app-attendance',
  styleUrls: ['./attendance.component.css'],
  templateUrl: './attendance.component.html',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    CommonModule,
    MatIconModule,
    MatDividerModule,
    MatButtonModule,
  ],
})
export class AttendanceComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'attendanceId',
    'employeeId',
    'date',
    'checkInTime',
    'checkOutTime',
    'status',
    'action',
  ];

  dataSource: MatTableDataSource<Attendance> =
    new MatTableDataSource<Attendance>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) sort!: MatSort;

  constructor(private _attendanceService: AttendanceService) {}

  ngOnInit(): void {
    this.getAllAttendances(); // Fetch data when the component initializes
  }

  getAllAttendances(): void {
    this._attendanceService
      .getAllAttendances()
      .subscribe((attendances: Attendance[]) => {
        this.dataSource.data = attendances;

        if (this.paginator) {
          this.dataSource.paginator = this.paginator;
        }
        if (this.sort) {
          this.dataSource.sort = this.sort;
        }
      });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
