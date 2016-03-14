'use strict';
clinicApp.controller('indexCtrl', ['$scope', '$location', 'authService', 'adminService', 'anonymService', 'clientService', 'commentService', function ($scope, $location, authService, adminService, anonymService, clientService, commentService) {

    $scope.SignOut = function () {
        authService.logOut();
        $location.path('/');
    }

    $scope.authentication = authService.authentication;


}]);