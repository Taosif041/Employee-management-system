import { Component, signal } from '@angular/core';
import { ChildComponent } from '../child/child.component';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-parent',
  imports: [],
  templateUrl: './parent.component.html',
  styleUrl: './parent.component.css',
})
export class ParentComponent {
  firstName = signal('Morgan');
  constructor() {
    // Access the value of the signal
    console.log(this.firstName());

    // Update the value of the signal
    this.firstName.set('Jaime');

    // Use the update method to modify the value based on the current value
    this.firstName.update((name) => name.toUpperCase());

    // Log the updated signal value
    console.log(this.firstName());
  }
}
