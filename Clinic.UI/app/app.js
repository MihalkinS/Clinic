var clinicApp = angular.module('clinicApp', ["ngRoute"])
    .config(function ($routeProvider) {

        $routeProvider.when('/doctor',
        {
            templateUrl: 'doctor.html',
            controller: 'doctorCtrl'
        });
        $routeProvider.when('/main',
        {
            templateUrl: 'main.html',
            controller: 'mainCtrl'
        });
        $routeProvider.otherwise({ redirectTo: '/main' });
    });