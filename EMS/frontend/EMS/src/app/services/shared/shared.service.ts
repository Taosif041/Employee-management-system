import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  private _snackBar = inject(MatSnackBar);

  openSnackBar(entity: string, action: string, isSuccess: boolean): void {
    let message = '';

    if (isSuccess) {
      switch (action.toLowerCase()) {
        case 'custom':
          message = entity;
          break;
        case 'create':
          message = `${entity} was successfully created!`;
          break;
        case 'update':
          message = `${entity} was successfully updated!`;
          break;
        case 'delete':
          message = `${entity} was successfully deleted!`;
          break;
        case 'login':
          message = `Successfully logged in!`;
          break;
        case 'logout':
          message = `Successfully logged out!`;
          break;

        case 'sessionExpired':
          message = `Session Expired! Please log in again.`;
          break;
        default:
          message = `Operation on ${entity} was successful!`;
          break;
      }
    } else {
      if (action == 'sessionExpired') {
        message = `Session Expired! Please log in again.`;
      } else if (action == 'required') {
        message = `Access denied.`;
      } else if (action == 'denied') {
        message = `Access denied. Please log in.`;
      } else {
        message = `Failed to ${action.toLowerCase()} ${entity}!`;
      }
    }

    this._snackBar.open(message, 'Done', {
      duration: 10000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }
}
