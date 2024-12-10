import { Routes } from '@angular/router';
import { RouterModule } from '@angular/router';

import { EmployeeComponent } from './components/employee/employee/employee.component';
import { AttendanceComponent } from './components/attendance/attendance/attendance.component';
import { DepartmentComponent } from './components/departmetn/department/department.component';
import { DesignationComponent } from './components/designation/designation/designation.component';
import { LogComponent } from './components/log/log/log.component';

export const routes: Routes = [
  { path: 'employee', component: EmployeeComponent },
  { path: 'attendance', component: AttendanceComponent },
  { path: 'department', component: DepartmentComponent },
  { path: 'designation', component: DesignationComponent },
  { path: 'log', component: LogComponent },
  { path: '', redirectTo: '/employee', pathMatch: 'full' },
];
