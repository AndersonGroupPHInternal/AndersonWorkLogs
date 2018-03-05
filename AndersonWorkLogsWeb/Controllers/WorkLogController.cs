using AndersonWorkLogsFunction;
using System.Web.Mvc;

namespace AndersonWorkLogsWeb.Controllers
{
    public class WorkLogController : BaseController
    {
        private IFWorkLog _iFWorkLog;
        public WorkLogController(IFWorkLog iFWorkLog)
        {
            _iFWorkLog = iFWorkLog;
        }

        #region Create
        #endregion

        #region Read
        [HttpPost]
        public JsonResult Read()
        {
            return Json(_iFWorkLog.Read());
        }

        [HttpPost]
        public JsonResult ReadId(int id)
        {
            return Json(_iFWorkLog.Read(id));
        }
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion
    }
}