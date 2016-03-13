'use strict';
clinicApp.controller('indexCtrl', ['$scope', '$location', 'authService', 'adminService', 'anonymService', 'clientService', function ($scope, $location, authService, adminService, anonymService, clientService) {

    $scope.SignOut = function () {
        authService.logOut();
        $location.path('/');
    }

    $scope.authentication = authService.authentication;


}]);