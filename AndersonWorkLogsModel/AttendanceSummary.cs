using BaseModel;

namespace AndersonWorkLogsModel
{
    public class AttendanceSummary : Base
    {
        public double Hours { get; set; }
        
        public int NumberOfAttendances { get; set; }
    }
}
