using AndersonCRMFunction;
using AccountsWebAuthentication.Helper;
using AndersonWorkLogsWeb.Controllers;
using System.Web.Mvc;

namespace AndersonCRMWeb.Controllers
{
    public class DepartmentController : BaseController
    {
        private IFDepartment _iFDepartment;
        public DepartmentController(IFDepartment iFDepartment)
        {
            _iFDepartment = iFDepartment;
        }

        #region Read
        [HttpPost]
        public JsonResult Read()
        {
            return Json(_iFDepartment.Read());
        }
        #endregion
    }
}