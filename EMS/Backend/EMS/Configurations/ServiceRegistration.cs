﻿using EMS.Repositories.Interfaces;
using EMS.Repositories.Implementations;
using EMS.Services.Interfaces;
using EMS.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using EMS.EMS.Repositories.DatabaseProviders.Implementations;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.Models;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDepartmentService, DepartmentService>();

        services.AddScoped<IDesignationRepository, DesignationRepository>();
        services.AddScoped<IDesignationService, DesignationService>();

        services.AddScoped<IOperationLogRepository, OperationLogRepository>();
        services.AddScoped<IOperationLogService, OperationLogService>();


        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IAttendanceService, AttendanceService>();

        services.AddScoped<IDatabaseFactory, DatabaseFactory>();

        services.AddScoped<ApiResultFactory>();




    }
}