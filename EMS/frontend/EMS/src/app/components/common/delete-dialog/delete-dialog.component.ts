import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogModule,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';

@Component({
  selector: 'app-delete-dialog',
  imports: [
    MatButtonModule,
    MatDialogModule,
    MatDialogActions,
    MatDialogTitle,
    MatDialogContent,
  ],
  templateUrl: './delete-dialog.component.html',
  styleUrl: './delete-dialog.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DeleteDialogComponent {
  readonly dialogRef = inject(MatDialogRef<DeleteDialogComponent>);

  // Close the dialog and return `true`
  confirmDelete(): void {
    this.dialogRef.close(true);
  }

  // Close the dialog and return `false`
  cancel(): void {
    this.dialogRef.close(false);
  }
}
