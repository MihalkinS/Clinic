'use strict';
clinicApp.controller('signinCtrl', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    if (authService.authentication.isAuth) {
        $location.path("/");
    };

    console.log(authService.authentication);

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.errorMessage = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            $location.path('/');

        },
         function (err) {
             $scope.errorMessage = err.error_description;
         });
    };

    $scope.redirectToMainPage = function () {
        $location.path("/");
    };

}]);