using BaseExtension;
using BaseModel;
using System;
using System.Collections.Generic;

namespace AndersonWorkLogsModel
{
    public class AttendanceFilter : Base
    {
        public DateTime? TimeInFrom { get; set; }
        public DateTime? TimeInTo { get; set; }

        public List<int> AttendanceIds { get; set; }
        public List<int> DepartmentIds { get; set; }
        public List<int> EmployeeIds { get; set; }
        public List<int> EmployeeIdsOfSelectedDepartments { get; set; }
        public List<int> ManagerEmployeeIds { get; set; }
    }
}
