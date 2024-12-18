export interface Attendance {
  attendanceId?: number;
  employeeId: number | null;
  date: string;
  checkInTime: string;
  checkOutTime: string;
  status: string;
}
