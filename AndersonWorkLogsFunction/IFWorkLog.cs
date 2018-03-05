using AndersonWorkLogsModel;
using System.Collections.Generic;

namespace AndersonWorkLogsFunction
{
    public interface IFWorkLog
    {
        #region CREATE
        void Create(int attendanceId, int createdBy, List<WorkLog> workLogs);
        #endregion

        #region READ
        List<WorkLog> Read();
        List<WorkLog> Read(int attendanceId);
        #endregion

        #region UPDATE
        #endregion

        #region DELETE
        void Delete(List<WorkLog> workLogs);
        void Delete(int attendanceId);
        #endregion
    }

}
