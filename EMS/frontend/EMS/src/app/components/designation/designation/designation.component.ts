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
import { Designation } from '../../../models/designation.model';
import { DesignationService } from '../../../services/designation/designation.service';

@Component({
  selector: 'app-designation',
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
  templateUrl: './designation.component.html',
  styleUrl: './designation.component.css',
})
export class DesignationComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = ['designationId', 'name', 'action'];

  dataSource: MatTableDataSource<Designation> =
    new MatTableDataSource<Designation>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) sort!: MatSort;

  constructor(private _designationService: DesignationService) {}

  ngOnInit(): void {
    this.getAllDesignations(); // Fetch data when the component initializes
  }

  getAllDesignations(): void {
    this._designationService
      .getAllDesignations()
      .subscribe((designations: Designation[]) => {
        this.dataSource.data = designations;

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
