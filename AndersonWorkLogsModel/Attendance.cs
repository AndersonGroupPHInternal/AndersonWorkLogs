using BaseExtension;
using BaseModel;
using System;
using System.Collections.Generic;

namespace AndersonWorkLogsModel
{
    public class Attendance : Base
    {
        public bool Approved
        {
            get
            {
                return ApprovedBy != 0;
            }
        }

        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public double Hours { get; set; }

        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        public int ApprovedBy { get; set; }
        public int ManagerEmployeeId { get; set; }

        public string TimeInString => TimeIn.ToHtml5Date();
        public string TimeOutString => TimeOut.ToHtml5Date();

        public List<int> SelectedIds { get; set; }
        public virtual List<WorkLog> WorkLogs { get; set; }
        public virtual List<WorkLog> DeletedWorkLogs { get; set; }
    }
}
