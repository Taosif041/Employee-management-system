CREATE OR REPLACE FUNCTION create_attendance(
    emp_id INT,
    att_date DATE,
    check_in_time TIMESTAMP without time zone,
    check_out_time TIMESTAMP without time zone,
    att_status VARCHAR(20)
)
RETURNS TABLE (
    attendanceId INT,
    employeeId INT,
    date DATE,
    checkInTime TIMESTAMP without time zone,
    checkOutTime TIMESTAMP without time zone,
    status VARCHAR(20)
) AS $$
BEGIN
    INSERT INTO Attendance (employeeId, date, checkInTime, checkOutTime, status)
    VALUES (emp_id, att_date, check_in_time, check_out_time, att_status)
	
    RETURNING attendance.attendanceId, attendance.employeeId, attendance.date, attendance.checkInTime, attendance.checkOutTime, attendance.status
    INTO STRICT attendanceId, employeeId, date, checkInTime, checkOutTime, status;
    RETURN NEXT;
END;
$$ LANGUAGE plpgsql;
