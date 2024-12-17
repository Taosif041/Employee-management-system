import {
  AfterViewInit,
  Component,
  ViewChild,
  OnInit,
  inject,
  ChangeDetectionStrategy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { Designation } from '../../../models/designation.model'; // Assuming you have a Designation model
import { DesignationService } from '../../../services/designation/designation.service'; // Assuming you have a DesignationService
import { RouterLink } from '@angular/router';
import { SharedService } from '../../../services/shared/shared.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../common/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-designation',
  styleUrls: ['./designation.component.css'],
  templateUrl: './designation.component.html',
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
export class DesignationComponent implements AfterViewInit, OnInit {
  readonly dialog = inject(MatDialog);
  private router = inject(Router);

  private _designationService = inject(DesignationService);
  private _sharedService = inject(SharedService);
  displayedColumns: string[] = ['designationId', 'name', 'action'];

  dataSource: MatTableDataSource<Designation> =
    new MatTableDataSource<Designation>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor() {}

  ngOnInit(): void {
    this.getAllDesignations();
  }

  getAllDesignations(): void {
    this._designationService
      .getAllDesignations()
      .subscribe((designations: Designation[]) => {
        this._designationService.setAllDesignations(designations);
        this.dataSource.data = designations;

        if (this.paginator) {
          this.dataSource.paginator = this.paginator;
        }
      });
  }

  ngAfterViewInit(): void {
    // Wait for the view initialization to complete
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;

    // Apply default sort direction after the view is initialized
    if (this.sort) {
      this.sort.active = 'designationId'; // Choose the column to be sorted initially
      this.sort.direction = 'desc'; // Set sort direction to descending
      this.dataSource.sort = this.sort;
    }
  }

  DeleteDesignation(designationId: number): void {
    this._designationService.deleteDesignation(designationId).subscribe({
      next: (Response) => {
        this._sharedService.openSnackBar('Designation', 'deleted', true);
        this.getAllDesignations();
        console.log('Designation deleted', Response);
        console.log(Response);
      },
      error: (err) => {
        this._sharedService.openSnackBar('Delete', 'failed', false);
        this.getAllDesignations();
        console.log('Designation delete failed', err);
        console.log(err);
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
    designationId: number
  ): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.DeleteDesignation(designationId);
      } else {
        console.log('Deletion cancelled by the user.');
      }
    });
  }
}
