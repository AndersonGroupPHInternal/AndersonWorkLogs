(function () {
    'use strict';

    angular
        .module('App')
        .factory('EmployeeDepartmentService', EmployeeDepartmentService);

    EmployeeDepartmentService.$inject = ['$http'];

    function EmployeeDepartmentService($http) {
        return {
            Read: Read
        }

        function Read(attendanceFilter) {
            return $http({
                method: 'POST',
                url: '/EmployeeDepartment/Read',
                data: $.param(attendanceFilter),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }
    }
})();