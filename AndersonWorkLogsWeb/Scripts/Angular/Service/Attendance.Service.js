﻿(function () {
    'use strict';

    angular
        .module('App')
        .factory('AttendanceService', AttendanceService);

    AttendanceService.$inject = ['$http'];

    function AttendanceService($http) {
        return {
            FilteredRead: FilteredRead,
            Read: Read,
            ReadSummary: ReadSummary,
            ReadTemporaryDeleted: ReadTemporaryDeleted,

            Approve: Approve,
            ApproveSelected: ApproveSelected,
            RestoreDeleted: RestoreDeleted,

            Delete: Delete,
            TemporaryDelete: TemporaryDelete
        }

        function FilteredRead(attendanceFilter) {
            return $http({
                method: 'POST',
                url: '/Attendance/FiltRead',
                data: $.param(attendanceFilter),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
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

        function ReadTemporaryDeleted() {
            return $http({
                method: 'POST',
                url: '/Attendance/ReadTemporaryDeleted',
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

        function RestoreDeleted(attendanceId) {
            return $http({
                method: 'POST',
                url: '/Attendance/RestoreDeleted/' + attendanceId,
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

        function TemporaryDelete(attendanceId) {
            return $http({
                method: 'DELETE',
                url: '/Attendance/TemporaryDelete/' + attendanceId,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }
    }
})();