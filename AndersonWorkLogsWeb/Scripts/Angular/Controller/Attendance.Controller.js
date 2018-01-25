(function () {
    'use strict';

    angular
        .module('App')
        .controller('AttendanceController', AttendanceController);

    AttendanceController.$inject = ['$filter', '$window', 'AttendanceService','UserService', 'EmployeeService'];

    function AttendanceController($filter, $window, AttendanceService, UserService, EmployeeService) {
        var vm = this;
        
        vm.Attendances = [];
        vm.Users = [];
        vm.Employees = [];
        
        vm.Initialise = Initialise;
        vm.InitialiseSummary = InitialiseSummary;
        vm.GoToUpdatePage = GoToUpdatePage;
        vm.CheckboxToggled = CheckboxToggled;
        vm.ToggleAll = ToggleAll;
        vm.Approve = Approve;
        vm.ApproveSelected = ApproveSelected;
        vm.ConfirmApproval = ConfirmApproval;
        vm.Delete = Delete;
        
        function Initialise() {
            Read();
        }

        function InitialiseSummary() {
            ReadSummary();
        }

        function GoToUpdatePage(attendanceId) {
            $window.location.href = '../Attendance/Update/' + attendanceId;
        }

        function CheckboxToggled() {
            vm.isAllSelected = vm.Attendances.every(function (attendance) {
                return attendance.Selected;
            });
        }

        function ToggleAll() {
            var toggleStatus = vm.isAllSelected;
            angular.forEach(vm.Attendances, function (attendance) {
                attendance.Selected = !toggleStatus;
            });
        }

        function Approve(id) {
            AttendanceService.Approve(id)
                .then(function (response) {
                    if (response.data === true) {
                        Read();
                    }
                })
                .catch(function (data, status) {
                });
        }

        function ApproveSelected() {
            var selectedAttendance = {
                SelectedIds: []
            };

            angular.forEach(vm.Attendances, function (attendance) {
                if (attendance.Selected) {
                    selectedAttendance.SelectedIds.push(attendance.AttendanceId);
                }
            });

            AttendanceService.ApproveSelected(selectedAttendance)
                .then(function (response) {
                    if (response.data === true) {
                        Read();
                    }
                })
                .catch(function (data, status) {
                });
        }

        function ConfirmApproval() {
            var selectedAttendance = $filter('filter')(vm.Attendances, { Selected: true });

            if (selectedAttendance.length != 0) {
                if (confirm("Approve " + selectedAttendance.length + " attendance?")) {
                    ApproveSelected();
                    alert("Successfully Approved!");
                }
                else {
                    alert("Approval Cancelled");
                }
            }
        }

        function ReadSummary() {
            AttendanceService.ReadSummary()
                .then(function (response) {
                    vm.Attendances = response.data;
                    ReadUsers();
                })
                .catch(function (data, status) {
                    new PNotify({
                        title: status,
                        text: data,
                        type: 'error',
                        hide: true,
                        addclass: "stack-bottomright"
                    });
                });
        }

        function Read() {
            AttendanceService.Read()
                .then(function (response) {
                    vm.Attendances = response.data;
                    ReadUsers();
                })
                .catch(function (data, status) {
                    new PNotify({
                        title: status,
                        text: data,
                        type: 'error',
                        hide: true,
                        addclass: "stack-bottomright"
                    });
                });
        }

        function ReadUsers() {
            UserService.Read()
                .then(function (response) {
                    vm.Users = response.data;
                    UpdateUser();
                    ReadEmployees();
                })
                .catch(function (data, status) {
                    new PNotify({
                        title: status,
                        text: data,
                        type: 'error',
                        hide: true,
                        addclass: "stack-bottomright"
                    });
                });
        }

        function UpdateUser() {
            angular.forEach(vm.Attendances, function (attendance) {
                attendance.User = $filter('filter')(vm.Users, { UserId: attendance.CreatedBy })[0];
            });
        }

        function ReadEmployees() {
            EmployeeService.Read()
                .then(function (response) {
                    vm.Employees = response.data;
                    UpdateEmployeeNames();
                })
                .catch(function (data, status) {
                    new PNotify({
                        title: status,
                        text: data,
                        type: 'error',
                        hide: true,
                        addclass: "stack-bottomright"
                    });
                });
        }

        function UpdateEmployeeNames() {
            angular.forEach(vm.Attendances, function (attendance) {
                attendance.Employee = $filter('filter')(vm.Employees, { EmployeeId: attendance.User.EmployeeId })[0];
                attendance.Employee.FullName = attendance.Employee.LastName + ", " + attendance.Employee.FirstName + " " + attendance.Employee.MiddleName;
                attendance.Manager = $filter('filter')(vm.Employees, { EmployeeId: attendance.ManagerEmployeeId })[0];
                if (attendance.Manager != undefined)
                    attendance.Manager.FullName = attendance.Manager.LastName + ", " + attendance.Manager.FirstName + " " + attendance.Manager.MiddleName;
            });
        }
        
        function Delete(attendanceId) {
            AttendanceService.Delete(attendanceId)
                .then(function (response) {
                    Read();
                })
                .catch(function (data, status) {
                    new PNotify({
                        title: status,
                        text: data,
                        type: 'error',
                        hide: true,
                        addclass: "stack-bottomright"
                    });
                });
        }
    }
})();