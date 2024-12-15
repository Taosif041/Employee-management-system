import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  private _snackBar = inject(MatSnackBar);

  openSnackBar(entity: string, action: string, isSuccess: boolean) {
    let message = '';

    if (isSuccess) {
      switch (action) {
        case 'custom':
          message = entity;
          break;
        case 'create':
          message = `${entity} successfully created!`;
          break;
        case 'update':
          message = `${entity} successfully updated!`;
          break;
        case 'delete':
          message = `${entity} successfully deleted!`;
          break;
        default:
          message = `Operation on ${entity} successful!`;
          break;
      }
    } else {
      message = `Failed to ${action} ${entity}!`;
    }

    this._snackBar.open(message, 'done', {
      duration: 10000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }
}
