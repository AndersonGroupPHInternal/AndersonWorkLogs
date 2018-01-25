using AndersonWorkLogsModel;
using System.Collections.Generic;

namespace AndersonWorkLogsFunction
{
    public interface IFAttendance
    {
        #region CREATE
        Attendance Create(int createdBy, int managerEmployeeId, Attendance attendance);
        #endregion

        #region READ
        Attendance ReadId(int attendanceId);
        List<Attendance> Read();
        List<Attendance> Read(int userId, int employeeId);
        List<AttendanceSummary> Readsummary();
        #endregion

        #region UPDATE
        Attendance Update(int updatedBy, Attendance attendance);
        void Approve(int approvedBy, int attendanceId);
        void MultipleApprove(int approvedBy, List<int> attendanceIds);
        #endregion

        #region DELETE
        void Delete(int AttendanceId);
        #endregion
    }

}
