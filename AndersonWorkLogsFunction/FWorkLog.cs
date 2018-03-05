using AndersonWorkLogsData;
using AndersonWorkLogsEntity;
using AndersonWorkLogsModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndersonWorkLogsFunction
{
    public class FWorkLog : IFWorkLog
    {
        private IDWorkLog _iDWorkLog;

        public FWorkLog()
        {
            _iDWorkLog = new DWorkLog();
        }

        #region CREATE
        public void Create(int attendanceId, int createdBy, List<WorkLog> workLogs)
        {
            if (!workLogs?.Any() ?? true)
                return;

            List<EWorkLog> eWorkLogs = EWorkLogs(workLogs);
            List<EWorkLog> newEWorkLogs = eWorkLogs.Where(a => a.WorkLogId == 0).ToList();
            newEWorkLogs.ToList().ForEach(a =>
            {
                a.CreatedDate = DateTime.Now;

                a.AttendanceId = attendanceId;
                a.CreatedBy = createdBy;
            });
            _iDWorkLog.Create(newEWorkLogs);
        }
        #endregion

        #region READ
        public List<WorkLog> Read()
        {
            List<EWorkLog> eWorkLogs = _iDWorkLog.List<EWorkLog>(a => true);
            return WorkLogs(eWorkLogs);
        }

        public List<WorkLog> Read(int attendanceId)
        {
            List<EWorkLog> eWorkLogs = _iDWorkLog.List<EWorkLog>(a => a.AttendanceId == attendanceId);
            return WorkLogs(eWorkLogs);
        }
        #endregion

        #region UPDATE
        #endregion

        #region DELETE
        public void Delete(List<WorkLog> workLogs)
        {
            if (!workLogs?.Any() ?? true)
                return;
            
            List<EWorkLog> eWorkLogs = EWorkLogs(workLogs);
            List<int> oldEWorkLogIds = eWorkLogs.Where(a => a.WorkLogId != 0).Select(a=> a.WorkLogId).ToList();
            _iDWorkLog.Delete<EWorkLog>(a => oldEWorkLogIds.Contains(a.WorkLogId));
        }

        public void Delete(int attendanceId)
        {
            _iDWorkLog.Delete<EWorkLog>(a => a.AttendanceId == attendanceId);
        }
        #endregion

        #region OTHER FUNCTION
        private List<EWorkLog> EWorkLogs(List<WorkLog> workLogs)
        {
            return workLogs.Select(a => new EWorkLog
            {
                CreatedDate = a.CreatedDate,
                UpdatedDate = a.UpdatedDate,
                AttendanceId = a.AttendanceId,

                CreatedBy = a.CreatedBy,
                UpdatedBy = a.UpdatedBy,
                WorkLogId = a.WorkLogId,

                WorkDone = a.WorkDone
            }).ToList();
        }

        private List<WorkLog> WorkLogs(List<EWorkLog> eWorkLogs)
        {
            return eWorkLogs.Select(a => new WorkLog
            {
                CreatedDate = a.CreatedDate,
                UpdatedDate = a.UpdatedDate,
                AttendanceId = a.AttendanceId,

                CreatedBy = a.CreatedBy,
                UpdatedBy = a.UpdatedBy,
                WorkLogId = a.WorkLogId,

                WorkDone = a.WorkDone
            }).ToList();
        }

        private EWorkLog EWorkLog(WorkLog workLog)
        {
            return new EWorkLog
            {
                CreatedDate = workLog.CreatedDate,
                UpdatedDate = workLog.UpdatedDate,
                AttendanceId = workLog.AttendanceId,

                CreatedBy = workLog.CreatedBy,
                UpdatedBy = workLog.UpdatedBy,
                WorkLogId = workLog.WorkLogId,

                WorkDone = workLog.WorkDone
            };
        }

        private WorkLog WorkLog(EWorkLog eWorkLog)
        {
            return new WorkLog
            {
                CreatedDate = eWorkLog.CreatedDate,
                UpdatedDate = eWorkLog.UpdatedDate,
                AttendanceId = eWorkLog.AttendanceId,

                CreatedBy = eWorkLog.CreatedBy,
                UpdatedBy = eWorkLog.UpdatedBy,
                WorkLogId = eWorkLog.WorkLogId,

                WorkDone = eWorkLog.WorkDone
            };
        }
        #endregion
    }
}
