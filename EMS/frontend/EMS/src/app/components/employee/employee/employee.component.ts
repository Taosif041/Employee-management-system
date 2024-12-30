import {
  AfterViewInit,
  Component,
  ViewChild,
  OnInit,
  Output,
  EventEmitter,
  inject,
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
import { DeleteDialogComponent } from '../../common/delete-dialog/delete-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { DepartmentService } from '../../../services/department/department.service';
import { DesignationService } from '../../../services/designation/designation.service';
import { Department } from '../../../models/department.model';
import { Designation } from '../../../models/designation.model';

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
  private readonly dialog = inject(MatDialog);
  private _employeeService = inject(EmployeeService);
  private _sharedService = inject(SharedService);

  private _departmentService = inject(DepartmentService);
  private _designationService = inject(DesignationService);

  displayedColumns: string[] = [
    'employeeId',
    'name',
    'email',
    'phone',
    'departmentId',
    'designationId',
    'action',
  ];

  employeeMap: { [key: number]: Employee } = {};
  departmentMap: { [key: number]: string } = {};
  designationMap: { [key: number]: string } = {};

  dataSource: MatTableDataSource<Employee> = new MatTableDataSource<Employee>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) sort!: MatSort;

  constructor() {}

  ngOnInit(): void {
    this.getAllEmployees();
    this.getAllDepartments();
    this.getAllDesignations();
  }

  getAllEmployees(): void {
    this._employeeService.getAllEmployees().subscribe({
      next: (employees: Employee[]) => {
        this.dataSource.data = employees;

        console.log('employees -> ', employees);

        if (this.paginator) {
          this.dataSource.paginator = this.paginator;
        }
        if (this.sort) {
          this.dataSource.sort = this.sort;
        }
      },
      error: (err) => {
        console.error('Error fetching employee data', err);
      },
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
    this._employeeService.deleteEmployee(employeeId).subscribe({
      next: (Response) => {
        this._sharedService.openSnackBar('Employee', 'delete', true);
        this.getAllEmployees();
        console.log('Employee deleted', Response);
      },
      error: (err) => {
        this._sharedService.openSnackBar('Delete', 'failed', false);
        console.log('Employee delete failed', err);
      },
    });
  }

  openDialog(
    enterAnimationDuration: string,
    exitAnimationDuration: string,
    departmentId: number
  ): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.deleteEmployee(departmentId);
      } else {
        console.log('Deletion cancelled by the user.');
      }
    });
  }

  getAllDepartments() {
    this._departmentService.getAllDepartments().subscribe({
      next: (data: Department[]) => {
        for (let i = 0; i < data.length; i++) {
          this.departmentMap[data[i].departmentId] = data[i].name;
        }
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  getAllDesignations() {
    this._designationService.getAllDesignations().subscribe({
      next: (data: Designation[]) => {
        for (let i = 0; i < data.length; i++) {
          this.designationMap[data[i].designationId] = data[i].name;
        }
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  downloadEmployeeCSVData(): void {
    this._employeeService.getEmployeeCSVData().subscribe(
      (response: Blob) => {
        const url = window.URL.createObjectURL(response);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'EmployeeData.csv'; // Specify the CSV file name
        link.click();
        window.URL.revokeObjectURL(url);
      },
      (error) => {
        console.error('CSV download failed', error);
      }
    );
  }

  downloadEmployeeExcelData(): void {
    this._employeeService.getEmployeeExcelData().subscribe(
      (response: Blob) => {
        const url = window.URL.createObjectURL(response);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'EmployeeData.xlsx'; // Specify the Excel file name
        link.click();
        window.URL.revokeObjectURL(url);
      },
      (error) => {
        console.error('Excel download failed', error);
      }
    );
  }
}
