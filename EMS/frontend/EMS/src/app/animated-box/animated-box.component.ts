import { Component } from '@angular/core';
import {
  trigger,
  state,
  style,
  animate,
  transition,
} from '@angular/animations';

@Component({
  selector: 'app-animated-box',
  template: `
    <div [@fadeInOut]="isVisible ? 'visible' : 'hidden'" class="box"></div>
    <button (click)="toggle()">Toggle Visibility</button>
  `,
  styles: [
    `
      .box {
        width: 100px;
        height: 100px;
        background-color: lightblue;
        margin: 20px auto;
      }
    `,
  ],
  animations: [
    trigger('fadeInOut', [
      state('visible', style({ opacity: 1 })),
      state('hidden', style({ opacity: 0 })),
      transition('visible <=> hidden', [animate('0.5s ease-in-out')]),
    ]),
  ],
})
export class AnimatedBoxComponent {
  isVisible = true;

  toggle() {
    this.isVisible = !this.isVisible;
  }
}
