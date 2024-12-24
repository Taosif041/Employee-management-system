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

  private allAttendances: Attendance[] = [];

  getAllAttendances(): Observable<any> {
    return this.http.get<Attendance[]>(this.apiUrl);
  }

  setAllAttendances(attendances: any): any {
    this.allAttendances = attendances.data;
  }

  fetchAndSetAllAttendances(): any {
    this.getAllAttendances().subscribe({
      next: (attendances: any) => {
        this.setAllAttendances(attendances.data);
      },
      error: (err) => {
        console.error('Error fetching attendances:', err);
      },
    });
  }

  getDesignationById(id: number): any {
    this.fetchAndSetAllAttendances();
    return this.allAttendances.filter(
      (attendance) => attendance.employeeId === id
    );
  }

  getAttendanceById(id: number): any {
    return this.allAttendances.find(
      (attendance) => attendance.attendanceId === id
    );
  }

  createAttendance(attendance: Attendance): Observable<any> {
    return this.http.post<Attendance>(this.apiUrl, attendance);
  }

  updateAttendance(id: number, attendance: Attendance): Observable<any> {
    return this.http.put<Attendance>(`${this.apiUrl}/${id}`, attendance);
  }
}
