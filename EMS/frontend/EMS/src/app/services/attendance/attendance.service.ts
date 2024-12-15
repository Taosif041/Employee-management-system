import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Attendance } from '../../models/attendance.model';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private apiUrl = 'http://localhost:5000/api/Attendance'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  // Get all Departments
  getAllAttendances(): Observable<any[]> {
    return this.http.get<Attendance[]>(this.apiUrl); // HTTP GET request to fetch all employees
  }
}
