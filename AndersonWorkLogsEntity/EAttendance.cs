using BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AndersonWorkLogsEntity
{
    [Table("Attendance")]
    public class EAttendance : EBase
    {
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime? ApprovedDate { get; set; }

        [NotMapped]
        public double Hours => (TimeOut - TimeIn).TotalHours;
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceId { get; set; }
        public int ApprovedBy { get; set; }
        public int EmployeeId { get; set; }
        public int ManagerEmployeeId { get; set; }

        public virtual ICollection<EWorkLog> WorkLogs { get; set; }
    }
}