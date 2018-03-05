using AndersonWorkLogsModel;
using System.Collections.Generic;

namespace AndersonWorkLogsFunction
{
    public interface IFAttendance
    {
        #region CREATE
        Attendance Create(int createdBy, int employeeId, int managerEmployeeId, Attendance attendance);
        #endregion

        #region READ
        Attendance ReadId(int attendanceId);
        List<Attendance> Read(AttendanceFilter attendanceFilter);
        List<Attendance> Read(int userId, int employeeId);
        List<AttendanceSummary> ReadSummary();
        List<Attendance> ReadTemporaryDeleted();
        #endregion

        #region UPDATE
        void Approve(int approvedBy, int attendanceId);
        void MultipleApprove(int approvedBy, List<int> attendanceIds);
        void RestoreDeleted(int attendanceId);
        void TemporaryDelete(int attendanceId);
        Attendance Update(int updatedBy, Attendance attendance);
        #endregion

        #region DELETE
        void Delete(int AttendanceId);
        #endregion
    }

}
