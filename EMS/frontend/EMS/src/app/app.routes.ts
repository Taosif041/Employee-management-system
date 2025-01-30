import { Routes } from '@angular/router';

import { EmployeeComponent } from './components/employee/employee/employee.component';
import { AttendanceComponent } from './components/attendance/attendance/attendance.component';
import { DesignationComponent } from './components/designation/designation/designation.component';
import { LogComponent } from './components/log/log/log.component';
import { DepartmentComponent } from './components/department/department/department.component';
import { EmployeeProfileComponent } from './components/employee/employee-profile/employee-profile.component';
import { CreateEmployeeComponent } from './components/employee/create-employee/create-employee.component';
import { UpdateEmployeeComponent } from './components/employee/update-employee/update-employee.component';
import { CreateAttendanceComponent } from './components/attendance/create-attendance/create-attendance.component';
import { UpdateAttendanceComponent } from './components/attendance/update-attendance/update-attendance.component';
import { CreateDepartmentComponent } from './components/department/create-department/create-department.component';
import { UpdateDepartmentComponent } from './components/department/update-department/update-department.component';
import { CreateDesignationComponent } from './components/designation/create-designation/create-designation.component';
import { UpdateDesignationComponent } from './components/designation/update-designation/update-designation.component';
import { EmployeeAttendanceComponent } from './components/attendance/employee-attendance/employee-attendance.component';
import { DemoButtonComponent } from './components/test/demo-button/demo-button.component';
import { InputComponent } from './components/test/input/input.component';
import { ParentComponent } from './components/test/parent/parent.component';
import { ChildComponent } from './components/test/child/child.component';
import { LoginComponent } from './components/auth/login/login.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { AnimatedBoxComponent } from './animated-box/animated-box.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'employee', component: EmployeeComponent },
  { path: 'employeeProfile/:id', component: EmployeeProfileComponent },
  {
    path: 'createEmployee',
    component: CreateEmployeeComponent,
    canActivate: [authGuard],
    data: { roles: [1, 2] },
  },
  { path: 'updateEmployee/:id', component: UpdateEmployeeComponent },

  { path: 'attendance', component: AttendanceComponent },
  { path: 'createAttendance', component: CreateAttendanceComponent },
  { path: 'updateAttendance/:id', component: UpdateAttendanceComponent },
  { path: 'employeeAttendance/:id', component: EmployeeAttendanceComponent },

  { path: 'department', component: DepartmentComponent },
  { path: 'createDepartment', component: CreateDepartmentComponent },
  { path: 'updateDepartment/:id', component: UpdateDepartmentComponent },

  { path: 'designation', component: DesignationComponent },
  { path: 'createDesignation', component: CreateDesignationComponent },
  { path: 'updateDesignation/:id', component: UpdateDesignationComponent },

  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },

  { path: 'log', component: LogComponent },

  { path: 'anime', component: AnimatedBoxComponent },
  { path: 'demo', component: DemoButtonComponent },
  { path: 'input', component: InputComponent },
  { path: 'parent', component: ParentComponent },
  { path: 'child', component: ChildComponent },

  { path: '', redirectTo: '/login', pathMatch: 'full' },
];
