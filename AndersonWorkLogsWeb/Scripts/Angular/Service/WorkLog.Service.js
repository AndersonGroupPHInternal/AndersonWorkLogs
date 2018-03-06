(function () {
    'use strict';

    angular
        .module('App')
        .factory('WorkLogService', WorkLogService);

    WorkLogService.$inject = ['$http'];

    function WorkLogService($http) {
        return {
            Read: Read,
            ReadId: ReadId
        };

        function Read() {
            return $http({
                method: 'POST',
                url: '/WorkLog/Read',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }

        function ReadId(attendanceId) {
            return $http({
                method: 'POST',
                url: '/WorkLog/ReadId/' + attendanceId,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }
    }
})();