
var clinicApp = angular.module('clinicApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'ngAnimate']);


clinicApp
    .config(function ($routeProvider) {

        $routeProvider.when('/doctor/calendar/',
        {
            templateUrl: '/views/Doctor/calendar.html',
            controller: 'doctorCtrl'
        });
        $routeProvider.when('/doctor/visits/',
        {
            templateUrl: '/views/Doctor/visits.html',
            controller: 'doctorCtrl'
        });

        $routeProvider.when('/client/calendar/',
        {
            templateUrl: '/views/Client/calendar.html',
            controller: 'clientCtrl'
        });
        $routeProvider.when('/client/comment/',
        {
            templateUrl: '/views/Client/comment.html',
            controller: 'clientCtrl'
        });



        $routeProvider.when('/admin/doctors/',
        {
            templateUrl: '/views/Admin/doctors.html',
            controller: 'adminCtrl'
        });
        $routeProvider.when('/admin/doctor/add/',
        {
            templateUrl: '/views/Admin/addDoctor.html',
            controller: 'adminCtrl'
        });
        $routeProvider.when('/admin/holidays/',
        {
            templateUrl: '/views/Admin/holidays.html',
            controller: 'adminCtrl'
        });
        $routeProvider.when('/admin/procedures/',
        {
            templateUrl: '/views/Admin/procedures.html',
            controller: 'adminCtrl'
        });
        $routeProvider.when('/admin/drugs/',
        {
            templateUrl: '/views/Admin/drugs.html',
            controller: 'adminCtrl'
        });
        $routeProvider.when('/admin/drugStorage/add/:id',
        {
            templateUrl: '/views/Admin/drugStorageAdd.html',
            controller: 'drugStorageCtrl'
        });
        $routeProvider.when('/admin/drug/add/',
        {
            templateUrl: '/views/Admin/addDrug.html',
            controller: 'adminCtrl'
        });


        $routeProvider.when('/signin',
        {
            templateUrl: '/views/signin.html',
            controller: 'signinCtrl'
        });
        $routeProvider.when('/signup',
        {
            templateUrl: '/views/signup.html',
            controller: 'signupCtrl'
        });

        $routeProvider.when('/anonym/calendar/',
        {
            templateUrl: '/views/anonym/calendar.html',
            controller: 'anonymCtrl'
        });

        $routeProvider.when('/404',
        {
            templateUrl: '/views/404.html'
        });

        $routeProvider.when('/',
        {
            templateUrl: '/views/main.html'
        });

        $routeProvider.otherwise({ redirectTo: '/404' });
    });

clinicApp.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
