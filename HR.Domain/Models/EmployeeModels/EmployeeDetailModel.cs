using HR.Domain.Models.DepartmentModels;
using HR.Domain.Models.JobHistoryModels;

namespace HR.Domain.Models.EmployeeModels;

public record EmployeeDetailModel(int EmployeeId,
                            string FirstName,
                            string LastName,
                            string Email,
                            string PhoneNumber,
                            DateTime? HireDate,
                            int? Salary,
                            int? JobId,
                            int? ManagerId,
                            int? DepartmentId,
                            EmployeeModel Manager,
                            DepartmentModel Department,
                            JobHistoryModel[] JobHistories);
