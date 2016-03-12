'use strict';
clinicApp.controller('indexCtrl', ['$scope', '$location', 'authService', 'adminService', 'anonymService', function ($scope, $location, authService, adminService, anonymService) {

    $scope.SignOut = function () {
        authService.logOut();
        $location.path('/');
    }

    $scope.authentication = authService.authentication;


}]);