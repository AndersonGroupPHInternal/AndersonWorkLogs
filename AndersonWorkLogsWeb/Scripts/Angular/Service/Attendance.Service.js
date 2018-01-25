(function () {
    'use strict';

    angular
        .module('App')
        .factory('AttendanceService', AttendanceService);

    AttendanceService.$inject = ['$http'];

    function AttendanceService($http) {
        return {
            Read: Read,
            ReadSummary: ReadSummary,
            Approve: Approve,
            ApproveSelected: ApproveSelected,
            Delete: Delete
        }

        function Read() {
            return $http({
                method: 'POST',
                url: '/Attendance/Read',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }

        function ReadSummary() {
            return $http({
                method: 'POST',
                url: '/Attendance/ReadSummary',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }

        function Approve(attendanceId) {
            return $http({
                method: 'POST',
                url: '/Attendance/Approve/' + attendanceId,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }

        function ApproveSelected(attendance) {
            return $http({
                method: 'POST',
                url: '/Attendance/ApproveSelected',
                data: $.param(attendance),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }

        function Delete(attendanceId) {
            return $http({
                method: 'DELETE',
                url: '/Attendance/Delete/' + attendanceId,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }
    }
})();