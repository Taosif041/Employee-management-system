import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Department } from '../../models/department.model';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private apiUrl = 'http://localhost:5000/api/Department';

  constructor(private http: HttpClient) {}
  private allDepartments: Department[] = [];

  getAllDepartments(): Observable<any> {
    return this.http.get<Department[]>(this.apiUrl);
  }

  setAllDepartments(departments: any): any {
    this.allDepartments = departments.data;
  }

  getDepartmentById(id: number): any {
    return this.allDepartments.find(
      (department) => department.departmentId === id
    );
  }

  updateDepartment(department: Department, id: number): Observable<any> {
    return this.http.put<Department>(`${this.apiUrl}/${id}`, department);
  }

  createDepartment(department: Department): Observable<any> {
    return this.http.post<Department>(this.apiUrl, department);
  }

  deleteDepartment(id: number): Observable<any> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
