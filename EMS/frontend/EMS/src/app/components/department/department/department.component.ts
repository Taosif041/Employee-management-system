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
import { Department } from '../../../models/department.model';
import { DepartmentService } from '../../../services/department/department.service';

@Component({
  selector: 'app-department',
  styleUrls: ['./department.component.css'],
  templateUrl: './department.component.html',
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
export class DepartmentComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = ['departmentId', 'name', 'action'];

  dataSource: MatTableDataSource<Department> =
    new MatTableDataSource<Department>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) sort!: MatSort;

  constructor(private _departmentService: DepartmentService) {}

  ngOnInit(): void {
    this.getAllDepartments(); // Fetch data when the component initializes
  }

  getAllDepartments(): void {
    this._departmentService
      .getAllDepartments()
      .subscribe((departments: Department[]) => {
        this.dataSource.data = departments;

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
