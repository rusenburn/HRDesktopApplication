using HR.Domain.Models.EmployeeModels;
using HR.Domain.Models.JobHistoryModels;

namespace HR.Domain.Models.JobModels;
public record JobDetailModel(int JobId,
                             string JobTitle,
                             int MinSalary,
                             int MaxSalary,
                             EmployeeModel[] Employees,
                             JobHistoryModel[] JobHistories);

