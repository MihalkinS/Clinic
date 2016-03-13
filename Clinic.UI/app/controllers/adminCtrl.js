clinicApp.controller('adminCtrl', ['$scope', '$window', '$location', 'authService', 'adminService', function ($scope, $window, $location, authService, adminService) {

    if (!authService.authentication.isAuth || authService.authentication.userRole != 'Administrator') {
        $location.path("/");
    };

    $scope.doctors = [];

    adminService.getDoctors().then(function (results) {

        $scope.doctors = results.data;

    }, function (error) {

        alert(error.data.message);

    });

    $scope.deleteDoctor = function (id) {

        adminService.deleteDoctor(id).then(function (results) {
            
            adminService.getDoctors().then(function (results) {

                $scope.doctors = results.data;

            });

        });

    };

    $scope.redirectToMainPage = function () {
        $location.path("/");
    };

    $scope.redirectToDrugList = function () {
        $location.path("/admin/drugs/");
    };

    $scope.addDoctor = function () {
        $location.path("/admin/doctor/add/");
    }

    $scope.newDoctor = {
        userName: "",
        email: "",
        password: "",
        confirmPassword: "",
        phoneNumber: "",
        role: "Doctor"
    };

    $scope.saveDoctor = function (newDoctor, doctorForm) {
        if (doctorForm.$valid) {

            adminService.saveDoctor(newDoctor).then(function (results) {
                $location.path("/admin/doctors/")
            }, function (error) {
                alert(error.data.message);
            });
        };
    };

    adminService.getDrugs().then(function (results) {

        $scope.drugs = results.data;

    }, function (error) {

        alert(error.data.message);

    });

    $scope.addDrug = function () {
        $location.path("/admin/drug/add/");
    };
    
    $scope.newDrug = {
        DrugName: "",
        description: "",
        cost: ""
    };

    $scope.newProcedure = {
        name: "",
        time: "",
        cost: ""
    };

    $scope.saveDrug = function (newDrug, drugForm) {
        if (drugForm.$valid) {
            adminService.addDrug(newDrug).then(function (results) {

                $location.path("/admin/drugs/")

            }, function (error) {
                alert(error.data.message);
            });
        };
    };

    $scope.procedures = adminService.procedures;

    adminService.getProcedures().then(function (results) {

        $scope.procedures = results.data;

    }, function (error) {

        alert(error.data.message);

    });

    $scope.saveProcedure = function (newProcedure, procedureForm) {
        if (procedureForm.$valid) {
            adminService.saveProcedure(newProcedure).then(function (results) {

                adminService.getProcedures().then(function (results) {

                    $scope.procedures = results.data;

                }, function (error) {

                    alert(error.data.message);

                });

            }, function (error) {
                alert(error.data.message);
            });
        };
    };

    $scope.deleteProcedure = function (id) {
        adminService.deleteProcedure(id).then(function (results) {

            adminService.getProcedures().then(function (results) {

                $scope.procedures = results.data;

            }, function (error) {

                alert(error.data.message);

            });

            }, function (error) {
                alert(error.data.message);
            });
    };

}]);
