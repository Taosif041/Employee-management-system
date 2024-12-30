import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-demo-button',
  imports: [ButtonModule, InputTextModule],
  templateUrl: './demo-button.component.html',
  styleUrl: './demo-button.component.css',
})
export class DemoButtonComponent {}
