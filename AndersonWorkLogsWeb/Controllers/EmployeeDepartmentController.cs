using AndersonCRMFunction;
using AndersonCRMModel;
using AndersonWorkLogsModel;
using AndersonWorkLogsWeb.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;

namespace AndersonCRMWeb.Controllers
{
    public class EmployeeDepartmentController : BaseController
    {
        private IFEmployeeDepartment _iFEmployeeDepartment;
        public EmployeeDepartmentController(IFEmployeeDepartment iFEmployeeDepartment)
        {
            _iFEmployeeDepartment = iFEmployeeDepartment;
        }

        #region Read
        [HttpPost]
        public JsonResult Read(AttendanceFilter attendanceFilter)
        {
            if (attendanceFilter.DepartmentIds == null)
                attendanceFilter.DepartmentIds = new List<int>();
            List<EmployeeDepartment> employeeDepartments = _iFEmployeeDepartment.Read(attendanceFilter.DepartmentIds);
            return Json(employeeDepartments);
        }
        #endregion
    }
}