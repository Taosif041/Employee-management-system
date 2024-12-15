import {
  AfterViewInit,
  Component,
  ViewChild,
  OnInit,
  Output,
  EventEmitter,
} from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';

import { EmployeeService } from '../../../services/employee/employee.service';
import { Employee } from '../../../models/employee.model';
import { RouterLink } from '@angular/router';
import { SharedService } from '../../../services/shared/shared.service';
import { EmployeeProfileComponent } from '../employee-profile/employee-profile.component';

@Component({
  selector: 'app-employee',
  styleUrls: ['./employee.component.css'],
  templateUrl: './employee.component.html',
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
    RouterLink,
  ],
})
export class EmployeeComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'employeeId',
    'name',
    'email',
    'phone',
    'departmentId',
    'designationId',
    'action',
  ];

  dataSource: MatTableDataSource<Employee> = new MatTableDataSource<Employee>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _employeeService: EmployeeService,
    private _sharedService: SharedService
  ) {}

  ngOnInit(): void {
    this.getAllEmployees();
  }

  getAllEmployees(): void {
    this._employeeService
      .getAllEmployees()
      .subscribe((employees: Employee[]) => {
        this.dataSource.data = employees;

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

  deleteEmployee(employeeId: number): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      this._employeeService.deleteEmployee(employeeId).subscribe(() => {
        // Refresh the list after deletion
        this.getAllEmployees();
        this._sharedService.openSnackBar('Employee', 'delete', true);
      });
    }
  }
}
