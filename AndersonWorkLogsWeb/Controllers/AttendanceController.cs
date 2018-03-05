using AndersonWorkLogsFunction;
using AndersonWorkLogsModel;
using System;
using System.Web.Mvc;

namespace AndersonWorkLogsWeb.Controllers
{
    public class AttendanceController : BaseController
    {
        private IFAttendance _iFAttendance;
        private IFWorkLog _iFWorkLog;
        public AttendanceController(IFAttendance iFAttendance, IFWorkLog iFWorkLog)
        {
            _iFAttendance = iFAttendance;
            _iFWorkLog = iFWorkLog;
        }

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            Attendance attendance = new Attendance()
            {
                TimeIn = DateTime.Now,
                TimeOut = DateTime.Now
            }; 
            return View(attendance);
        }

        [HttpPost]
        public ActionResult Create(Attendance attendance)
        {
            var createdAttendance = _iFAttendance.Create(UserId, EmployeeId, ManagerEmployeeId, attendance);
            _iFWorkLog.Create(createdAttendance.AttendanceId, UserId, attendance.WorkLogs);
            return RedirectToAction("Index");
        }
        #endregion

        #region Read
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RecentlyDeleted()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Read()
        {
            return Json(_iFAttendance.Read(UserId, EmployeeId));
        }

        [HttpPost]
        public JsonResult FiltRead(AttendanceFilter attendanceFilter)
        {
            try
            {
                return Json(_iFAttendance.Read(attendanceFilter));
            }
            catch (Exception exception)
            {
                return Json(exception);
            }
        }

        [HttpPost]
        public JsonResult ReadSummary()
        {
            return Json(_iFAttendance.ReadSummary());
        }

        [HttpPost]
        public JsonResult ReadTemporaryDeleted()
        {
            return Json(_iFAttendance.ReadTemporaryDeleted());
        }
        #endregion

        #region Update
        [HttpGet]
        public ActionResult Update(int id)
        {
            var attendance = _iFAttendance.ReadId(id);
            attendance.UserId = UserId;
            return View(attendance);
        }

        [HttpPost]
        public ActionResult Update(Attendance attendance)
        {
            if (attendance.CreatedBy == UserId && !attendance.Approved)
            {
                var createdAttendance = _iFAttendance.Update(UserId, attendance);
                _iFWorkLog.Create(createdAttendance.AttendanceId, UserId, attendance.WorkLogs);
                _iFWorkLog.Delete(attendance.DeletedWorkLogs);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Approve(int id)
        {
            try
            {
                _iFAttendance.Approve(UserId, id);
                return Json(true);
            }
            catch
            {
                return Json(true);
            }
        }

        [HttpPost]
        public JsonResult ApproveSelected(AttendanceFilter attendanceFilter)
        {
            if (attendanceFilter.AttendanceIds != null)
                _iFAttendance.MultipleApprove(UserId, attendanceFilter.AttendanceIds);
            return Json(true);
        }

        [HttpPost]
        public JsonResult RestoreDeleted(int id)
        {
            _iFAttendance.RestoreDeleted(id);
            return Json(true);
        }
        #endregion

        #region Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            _iFWorkLog.Delete(id);
            _iFAttendance.Delete(id);
            return Json(string.Empty);
        }

        [HttpDelete]
        public JsonResult TemporaryDelete(int id)
        {
            _iFAttendance.TemporaryDelete(id);
            return Json(string.Empty);
        }
        #endregion
    }
}