using AndersonWorkLogsData;
using AndersonWorkLogsEntity;
using AndersonWorkLogsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AndersonWorkLogsFunction
{
    public class FAttendance : IFAttendance
    {
        private IDAttendance _iDAttendance;

        public FAttendance()
        {
            _iDAttendance = new DAttendance();
        }

        #region CREATE
        public Attendance Create(int createdBy, int employeeId, int managerEmployeeId, Attendance attendance)
        {
            EAttendance eAttendance = EAttendance(attendance);
            eAttendance.CreatedDate = DateTime.Now;
            eAttendance.CreatedBy = createdBy;
            eAttendance.EmployeeId = employeeId;
            eAttendance.ManagerEmployeeId = managerEmployeeId;
            eAttendance = _iDAttendance.Create(eAttendance);
            return (Attendance(eAttendance));
        }
        #endregion

        #region READ
        public Attendance ReadId(int attendanceId)
        {
            EAttendance eAttendance = _iDAttendance.Read<EAttendance>(a => a.AttendanceId == attendanceId);

            return Attendance(eAttendance);
        }

        public List<Attendance> Read(AttendanceFilter attendanceFilter)
        {
            if (attendanceFilter.EmployeeIds == null)
                attendanceFilter.EmployeeIds = new List<int>();
            if (attendanceFilter.EmployeeIdsOfSelectedDepartments == null)
                attendanceFilter.EmployeeIdsOfSelectedDepartments = new List<int>();
            if (attendanceFilter.ManagerEmployeeIds == null)
                attendanceFilter.ManagerEmployeeIds = new List<int>();

            Expression<Func<EAttendance, bool>> predicate =
                a => (((a.TimeIn >= attendanceFilter.TimeInFrom && a.TimeIn <= attendanceFilter.TimeInTo)
                || (a.TimeOut >= attendanceFilter.TimeInFrom && a.TimeOut <= attendanceFilter.TimeInTo)) 
                || (!attendanceFilter.TimeInFrom.HasValue || !attendanceFilter.TimeInTo.HasValue))
                && (!attendanceFilter.EmployeeIds.Any() || attendanceFilter.EmployeeIds.Contains(a.EmployeeId))
                && (!attendanceFilter.EmployeeIdsOfSelectedDepartments.Any() || attendanceFilter.EmployeeIdsOfSelectedDepartments.Contains(a.EmployeeId))
                && (!attendanceFilter.ManagerEmployeeIds.Any() || attendanceFilter.ManagerEmployeeIds.Contains(a.ManagerEmployeeId));

            //(a.TimeIn >= attendanceFilter.TimeInFrom) && (a.TimeOut <= attendanceFilter.TimeInTo) ||

            //Expression<Func<EAttendance, bool>> predicate =
            //    a => (attendanceFilter.TimeInFrom.HasValue || a.TimeIn >= attendanceFilter.TimeInFrom)
            //    && (attendanceFilter.TimeInTo.HasValue || a.TimeOut >= attendanceFilter.TimeInTo)
            //    && (attendanceFilter.ManagerEmployeeIds.Any() || attendanceFilter.ManagerEmployeeIds.Contains(a.ManagerEmployeeId));
            //looks like we need to add EmployeeId
            List <EAttendance> eAttendances = _iDAttendance.List(predicate);
            return Attendances(eAttendances);
        }

        public List<Attendance> Read(int userId, int employeeId)
        {
            List<EAttendance> eAttendances = _iDAttendance.List<EAttendance>(a => a.CreatedBy == userId || a.ManagerEmployeeId == employeeId);

            return Attendances(eAttendances);
        }

        public List<AttendanceSummary> ReadSummary()
        {
            List<EAttendance> eAttendances = _iDAttendance.List<EAttendance>(a => true);
            List<AttendanceSummary> attendanceSummaries = new List<AttendanceSummary>();
            attendanceSummaries = eAttendances.GroupBy(d => d.CreatedBy)
                .Select(
                    a => new AttendanceSummary
                    {
                        CreatedBy = a.Key,
                        NumberOfAttendances = a.Count(),
                        Hours = a.Sum(b => b.Hours)
                    }).ToList();

            return attendanceSummaries;
        }
        #endregion

        #region UPDATE
        public Attendance Update(int updatedBy, Attendance attendance)
        {
            EAttendance eAttendance = EAttendance(attendance);
            eAttendance.UpdatedDate = DateTime.Now;
            eAttendance.UpdatedBy = updatedBy;
            eAttendance = _iDAttendance.Update(eAttendance);

            return (Attendance(eAttendance));
        }

        public void Approve(int approvedBy, int attendanceId)
        {
            EAttendance eAttendance = _iDAttendance.Read<EAttendance>(a => a.AttendanceId == attendanceId);
            eAttendance.UpdatedDate = DateTime.Now;
            eAttendance.UpdatedBy = approvedBy;

            eAttendance.ApprovedDate = DateTime.Now;
            eAttendance.ApprovedBy = approvedBy;
            _iDAttendance.Update(eAttendance);
        }

        public void MultipleApprove(int approvedBy, List<int> attendanceIds)
        {
            foreach(int attendanceId in attendanceIds)
            {
                EAttendance eAttendance = _iDAttendance.Read<EAttendance>(a => a.AttendanceId == attendanceId);
                eAttendance.UpdatedDate = DateTime.Now;
                eAttendance.UpdatedBy = approvedBy;

                eAttendance.ApprovedDate = DateTime.Now;
                eAttendance.ApprovedBy = approvedBy;
                _iDAttendance.Update(eAttendance);
            }
        }
        #endregion

        #region DELETE
        public void Delete(int attendanceId)
        {
            _iDAttendance.Delete<EAttendance>(a => a.AttendanceId == attendanceId);
        }
        #endregion

        #region OTHER FUNCTION
        private List<Attendance> Attendances(List<EAttendance> eAttendances)
        {
            return eAttendances.Select(a => new Attendance
            {
                CreatedDate = a.CreatedDate,
                UpdatedDate = a.UpdatedDate,
                TimeIn = a.TimeIn,
                TimeOut = a.TimeOut,
                ApprovedDate = a.ApprovedDate,

                Hours = a.Hours,

                AttendanceId = a.AttendanceId,
                ApprovedBy = a.ApprovedBy,
                EmployeeId = a.EmployeeId,
                ManagerEmployeeId = a.ManagerEmployeeId,
                CreatedBy = a.CreatedBy,
                UpdatedBy = a.UpdatedBy
            }).ToList();
        }

        private EAttendance EAttendance(Attendance attendance)
        {
            return new EAttendance
            {
                CreatedDate = attendance.CreatedDate,
                UpdatedDate = attendance.UpdatedDate,
                TimeIn = attendance.TimeIn,
                TimeOut = attendance.TimeOut,
                ApprovedDate = attendance.ApprovedDate,

                AttendanceId = attendance.AttendanceId,
                ApprovedBy = attendance.ApprovedBy,
                EmployeeId = attendance.EmployeeId,
                ManagerEmployeeId = attendance.ManagerEmployeeId,
                CreatedBy = attendance.CreatedBy,
                UpdatedBy = attendance.UpdatedBy
            };
        }

        private Attendance Attendance(EAttendance eAttendance)
        {
            return new Attendance
            {
                CreatedDate = eAttendance.CreatedDate,
                UpdatedDate = eAttendance.UpdatedDate,
                TimeIn = eAttendance.TimeIn,
                TimeOut = eAttendance.TimeOut,

                AttendanceId = eAttendance.AttendanceId,
                ApprovedBy = eAttendance.ApprovedBy,
                EmployeeId = eAttendance.EmployeeId,
                ManagerEmployeeId = eAttendance.ManagerEmployeeId,
                CreatedBy = eAttendance.CreatedBy,
                UpdatedBy = eAttendance.UpdatedBy
            };
        }
        #endregion
    }
}
