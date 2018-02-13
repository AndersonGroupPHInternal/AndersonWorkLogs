(function () {
    'use strict';

    angular
        .module('App')
        .controller('AttendanceController', AttendanceController);

    AttendanceController.$inject = ['$filter', '$window', 'AttendanceService', 'DepartmentService', 'EmployeeService', 'EmployeeDepartmentService', 'UserService'];

    function AttendanceController($filter, $window, AttendanceService, DepartmentService, EmployeeService, EmployeeDepartmentService, UserService) {
        var vm = this;

        vm.AttendanceFilter = {};

        vm.Attendances = [];
        vm.Departments = [];
        vm.Employees = [];
        vm.EmployeeDepartments = [];
        vm.Users = [];
        
        vm.Initialise = Initialise;
        vm.InitialiseSummary = InitialiseSummary;
        vm.FilterList = FilterList;
        vm.Approve = Approve;
        vm.ApproveSelected = ApproveSelected;
        vm.CheckboxToggled = CheckboxToggled;
        vm.ConfirmApproval = ConfirmApproval;
        vm.GoToUpdatePage = GoToUpdatePage;
        vm.ToggleAll = ToggleAll;
        vm.Delete = Delete;

        function Initialise() {
            Read();
            ReadDepartment();
        }

        function InitialiseSummary() {
            ReadSummary();
        }

        //### READ ###

        function FilterList() {
            var attendanceFilter = angular.copy(vm.AttendanceFilter)

            if (attendanceFilter.TimeInFrom != undefined && attendanceFilter.TimeInTo != undefined) {
                attendanceFilter.TimeInFrom = moment(attendanceFilter.TimeInFrom).format('YYYY-MM-DD');
                attendanceFilter.TimeInTo = moment(attendanceFilter.TimeInTo).add(1, 'days').format('YYYY-MM-DD');
            }
            attendanceFilter.EmployeeIds = [];
            angular.forEach(vm.AttendanceFilter.Employees, function (employee) {
                attendanceFilter.EmployeeIds.push(employee.EmployeeId);
            });
            attendanceFilter.ManagerEmployeeIds = [];
            angular.forEach(vm.AttendanceFilter.ManagerEmployees, function (managerEmployee) {
                attendanceFilter.ManagerEmployeeIds.push(managerEmployee.EmployeeId);
            });
            attendanceFilter.DepartmentIds = [];
            angular.forEach(vm.AttendanceFilter.Departments, function (department) {
                attendanceFilter.DepartmentIds.push(department.DepartmentId);
            });

            ReadEmployeeDepartment(attendanceFilter);
        }
        
        function FilteredRead(attendanceFilter) {
            AttendanceService.FilteredRead(attendanceFilter)
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

        function ReadDepartment() {
            DepartmentService.Read()
                .then(function (response) {
                    vm.Departments = response.data;
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

        function ReadEmployeeDepartment(attendanceFilter) {
            EmployeeDepartmentService.Read(attendanceFilter)
                .then(function (response) {
                    vm.EmployeeDepartments = response.data;

                    attendanceFilter.EmployeeIdsOfSelectedDepartments = [];
                    angular.forEach(vm.EmployeeDepartments, function (employeeDepartment) {
                        attendanceFilter.EmployeeIdsOfSelectedDepartments.push(employeeDepartment.EmployeeId);
                    });

                    FilteredRead(attendanceFilter);
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
                if (attendance.Manager !== undefined)
                    attendance.Manager.FullName = attendance.Manager.LastName + ", " + attendance.Manager.FirstName + " " + attendance.Manager.MiddleName;
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

        //### UPDATE ###

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
                AttendanceIds: []
            };

            angular.forEach(vm.Attendances, function (attendance) {
                if (attendance.Selected) {
                    selectedAttendance.AttendanceIds.push(attendance.AttendanceId);
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

        function CheckboxToggled() {
            vm.isAllSelected = vm.Attendances.every(function (attendance) {
                return attendance.Selected;
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

        function ToggleAll() {
            var toggleStatus = vm.isAllSelected;
            angular.forEach(vm.Attendances, function (attendance) {
                attendance.Selected = !toggleStatus;
            });
        }

        function GoToUpdatePage(attendanceId) {
            $window.location.href = '../Attendance/Update/' + attendanceId;
        }

        //### DELETE ###

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