import { Component, OnInit, Directive } from '@angular/core';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';

import { EmployeeService } from '../../../services/employee/employee.service';
import { Employee } from '../../../models/employee.model';

@Component({
  selector: 'app-employee-profile',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatListModule,
    RouterLink,
  ],
  templateUrl: './employee-profile.component.html',
  styleUrl: './employee-profile.component.css',
})
export class EmployeeProfileComponent implements OnInit {
  employeeId: number = 0;
  selectedEmployee: Employee | null = null;

  constructor(
    private route: ActivatedRoute,
    private _employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.employeeId = +this.route.snapshot.paramMap.get('id')!;
    this.getEmployeeById(this.employeeId);
    console.log('employeeId ->', this.employeeId);
  }

  getEmployeeById(id: number): void {
    this._employeeService.getEmployeeById(id).subscribe(
      (employee: Employee) => {
        this.selectedEmployee = employee;
      },
      (error) => {
        console.error('Error fetching employee by ID:', error);
      }
    );
  }
}
