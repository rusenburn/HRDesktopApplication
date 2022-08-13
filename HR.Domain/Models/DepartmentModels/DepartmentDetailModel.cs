using HR.Domain.Models.EmployeeModels;
using HR.Domain.Models.JobHistoryModels;
using HR.Domain.Models.LocationModels;

namespace HR.Domain.Models.DepartmentModels;
public record DepartmentDetailModel(int DepartmentId,
                                    string DepartmentName,
                                    int LocationId,
                                    LocationModel LocationModel,
                                    EmployeeModel[] Employees,
                                    JobHistoryModel[] JobHistories);