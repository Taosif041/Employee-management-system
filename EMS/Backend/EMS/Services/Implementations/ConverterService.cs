using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Repositories.Implementations;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System.Text;
using ClosedXML.Excel;
using MongoDB.Driver.Linq;


namespace EMS.Services.Implementations
{
    public class ConverterService: IConverterService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ApiResultFactory _apiResultFactory;
        ILogger<ConverterService> _logger;

        public ConverterService(
            IEmployeeRepository employeeRepository, 
            IAttendanceRepository attendanceRepository,
            ApiResultFactory apiResultFactory,
            ILogger<ConverterService> logger
            ) {
            _employeeRepository = employeeRepository;
            _attendanceRepository = attendanceRepository;
            _apiResultFactory = apiResultFactory;
            _logger = logger;
        }


        public async Task<ApiResult> GetEmployeeCSVAsync()
        {
            try
            {
                var result = await _employeeRepository.GetAllEmployeesAsync();
                var employees = result.Data as List<Employee>;

                var csvBuilder = new StringBuilder();

                csvBuilder.AppendLine("EmployeeId, OfficeEmployeeId, Name,Email, Phone, Address, DOB, DepartmentId, DesignationId");

                foreach (var emp in employees)
                {
                    string FormatCsvField(string field) =>
                        string.IsNullOrEmpty(field)
                           ? ""
                           : $"\"{field.Replace("\"", "\"\"")}\"";


                    csvBuilder.AppendLine(
                        $"{emp.EmployeeId}," +
                        $"{emp.OfficeEmployeeId}," +
                        $"{emp.Name}," +
                        $"{emp.Email}," +
                        $"{emp.Phone}," +
                        $"{FormatCsvField(emp.Address)}," +
                        $"{emp.DOB?.ToString("yyyy-MM-dd")}," +
                        $"{emp.DepartmentId}," +
                        $"{emp.DesignationId}"
                    );
                }

                var csvData = Encoding.UTF8.GetBytes(csvBuilder.ToString());
                return _apiResultFactory.CreateSuccessResult( csvData );
             }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating employee CSV.");
                return _apiResultFactory.CreateErrorResult(500, "Employee CSV creation Error", ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> GetEmployeeExcelAsync()
        {
            try
            {
                var result = await _employeeRepository.GetAllEmployeesAsync();
                var employees = result.Data as List<Employee>;

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Employees");

                worksheet.Cell(1, 1).Value = "Employee ID";
                worksheet.Cell(1, 2).Value = "Office Employee ID";
                worksheet.Cell(1, 3).Value = "Name";
                worksheet.Cell(1, 4).Value = "Email";
                worksheet.Cell(1, 5).Value = "Phone";
                worksheet.Cell(1, 6).Value = "Address";
                worksheet.Cell(1, 7).Value = "Date of Birth";
                worksheet.Cell(1, 8).Value = "Department ID";
                worksheet.Cell(1, 9).Value = "Designation ID";


                for (int i = 0; i < employees.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = employees[i].EmployeeId;
                    worksheet.Cell(i + 2, 2).Value = employees[i].OfficeEmployeeId;
                    worksheet.Cell(i + 2, 3).Value = employees[i].Name;
                    worksheet.Cell(i + 2, 4).Value = employees[i].Email;
                    worksheet.Cell(i + 2, 5).Value = employees[i].Phone;
                    worksheet.Cell(i + 2, 6).Value = employees[i].Address;
                    worksheet.Cell(i + 2, 7).Value = employees[i].DOB?.ToString("yyyy-MM-dd") ?? "N/A"; 
                    worksheet.Cell(i + 2, 8).Value = employees[i].DepartmentId?.ToString() ?? "N/A";
                    worksheet.Cell(i + 2, 9).Value = employees[i].DesignationId?.ToString() ?? "N/A"; 
                }

                worksheet.Columns().AdjustToContents(); 

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var fileBytes = stream.ToArray();
                return _apiResultFactory.CreateSuccessResult(fileBytes);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while creating employee excel.");
                return _apiResultFactory.CreateErrorResult(500, "Employee Excel creation Error", ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> GetAttendanceCSVAsync()
        {
            try
            {

                var result = await _attendanceRepository.GetAllAttendanceAsync();
                var attendances = result.Data as List<Attendance>;

                var csvBuilder = new StringBuilder();

                csvBuilder.AppendLine("AttendanceId, EmployeeId, Date, CheckInTime, CheckOutTime, Status");


                foreach (var attendance in attendances)
                {
                    //emp.DOB?.ToString("yyyy-MM-dd")}
                csvBuilder.AppendLine($"{attendance.AttendanceId}, {attendance.EmployeeId},{attendance.Date.ToString("yyyy-MM-dd")},{attendance.CheckInTime?.ToString("HH:mm:ss")},{attendance.CheckOutTime?.ToString("HH:mm:ss")}, {attendance.Status}");
                }

                var csvData = Encoding.UTF8.GetBytes(csvBuilder.ToString());

                return _apiResultFactory.CreateSuccessResult(csvData);
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(500, "Attencance CSV creation Error", ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> GetAttendanceExcelAsync()
        {
            try
            {
                var result = await _attendanceRepository.GetAllAttendanceAsync();
                var attendances = result.Data as List<Attendance>;

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Attendance");

                worksheet.Cell(1, 1).Value = "Attendance ID";
                worksheet.Cell(1, 2).Value = "Employee ID";
                worksheet.Cell(1, 3).Value = "Date";
                worksheet.Cell(1, 4).Value = "Check-In Time";
                worksheet.Cell(1, 5).Value = "Check-Out Time";
                worksheet.Cell(1, 6).Value = "Status";

                for (int i = 0; i < attendances.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = attendances[i].AttendanceId;
                    worksheet.Cell(i + 2, 2).Value = attendances[i].EmployeeId;
                    worksheet.Cell(i + 2, 3).Value = attendances[i].Date.ToString("yyyy-MM-dd"); 
                    worksheet.Cell(i + 2, 4).Value = attendances[i].CheckInTime?.ToString("HH:mm:ss") ?? "N/A"; 
                    worksheet.Cell(i + 2, 5).Value = attendances[i].CheckOutTime?.ToString("HH:mm:ss") ?? "N/A"; 
                    worksheet.Cell(i + 2, 6).Value = attendances[i].Status;
                }

                worksheet.Columns().AdjustToContents(); 

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var fileBytes = stream.ToArray();


                return _apiResultFactory.CreateSuccessResult(fileBytes);
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(500, "Attencance Excel creation Error", ErrorLayer.Service);
            }
        }


        
    }
}
