clinicApp.controller('drugStorageCtrl', ['$scope', '$location', '$routeParams', 'authService', 'adminService', function ($scope, $location, $routeParams, authService, adminService) {

    if (!authService.authentication.isAuth || authService.authentication.userRole != 'Administrator') {
        $location.path("/");
    };

    $scope.drug = adminService.drug;

    $scope.redirectToMainPage = function () {
        $location.path("/");
    };

    $scope.redirectToDrugList = function () {
        $location.path("/admin/drugs");
    };
    
    $scope.saveDrugInStore = function (drug) {
        adminService.addDrugInStorage(drug).then(function (results) {

            $location.path("/admin/drugs/")

        }, function (error) {
            alert(error.data.message);
        });
    };

    adminService.getDrugById($routeParams.id).then(function (results) {

        $scope.drug = results.data;

    }, function (error) {

        alert(error.data.message);

    });

}]);
