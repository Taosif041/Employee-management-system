import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-child',
  imports: [],
  templateUrl: './child.component.html',
  styleUrl: './child.component.css',
})
export class ChildComponent {
  @Input() task: any; // Input from parent
  @Output() taskApproved = new EventEmitter<any>();

  approveTask() {
    this.taskApproved.emit(this.task);
  }
}
