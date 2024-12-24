import {
  AfterViewInit,
  Component,
  ViewChild,
  OnInit,
  inject,
  ChangeDetectionStrategy,
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
import { Department } from '../../../models/department.model';
import { DepartmentService } from '../../../services/department/department.service';
import { RouterLink } from '@angular/router';
import { SharedService } from '../../../services/shared/shared.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../common/delete-dialog/delete-dialog.component';

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
    RouterLink,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DepartmentComponent implements AfterViewInit, OnInit {
  readonly dialog = inject(MatDialog);

  private _departmentService = inject(DepartmentService);
  private _sharedService = inject(SharedService);
  displayedColumns: string[] = ['departmentId', 'name', 'action'];

  dataSource: MatTableDataSource<Department> =
    new MatTableDataSource<Department>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) sort!: MatSort;

  constructor() {}

  ngOnInit(): void {
    this.getAllDepartments();
  }

  getAllDepartments(): void {
    this._departmentService
      .getAllDepartments()
      .subscribe((departments: any) => {
        this._departmentService.setAllDepartments(departments);
        this.dataSource.data = departments.data;

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
  DeleteDepartment(departmentId: number): void {
    this._departmentService.deleteDepartment(departmentId).subscribe({
      next: (Response: any) => {
        this._sharedService.openSnackBar('Dpeartment', 'delete', true);
        this.getAllDepartments();
        console.log('Department deleted', Response.data);
      },
      error: (err) => {
        this._sharedService.openSnackBar('Delete', 'failed', false);
        console.log('Department delete failed', err);
      },
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
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

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.DeleteDepartment(departmentId);
      } else {
        console.log('Deletion cancelled by the user.');
      }
    });
  }
}
