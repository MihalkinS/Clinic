clinicApp.controller('adminCtrl', ['$scope', '$location', 'authService', 'adminService', function ($scope, $location, authService, adminService) {

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
            alert();
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

    $scope.saveDrug = function (newDrug, drugForm) {
        if (drugForm.$valid) {
            adminService.addDrug(newDrug).then(function (results) {

                $location.path("/admin/drugs/")

            }, function (error) {
                alert(error.data.message);
            });
        };
    };
}]);
