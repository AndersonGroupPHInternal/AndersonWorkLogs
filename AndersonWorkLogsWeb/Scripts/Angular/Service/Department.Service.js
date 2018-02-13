(function () {
    'use strict';

    angular
        .module('App')
        .factory('DepartmentService', DepartmentService);

    DepartmentService.$inject = ['$http'];

    function DepartmentService($http) {
        return {
            Read: Read
        }

        function Read() {
            return $http({
                method: 'POST',
                url: '/Department/Read',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }
    }
})();