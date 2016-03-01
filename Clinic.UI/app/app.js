
var clinicApp = angular.module('clinicApp', ["ngRoute"])
    .config(function ($routeProvider) {

        $routeProvider.when('/doctor/calendar/',
        {
            templateUrl: '/views/Doctor/calendar.html',
            controller: 'doctorCtrl'
        });


        $routeProvider.when('/client/register/',
        {
            templateUrl: '/views/Client/register.html',
            controller: 'clientCtrl'
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

        $routeProvider.when('/admin/add/',
        {
            templateUrl: '/views/Admin/add.html',
            controller: 'adminCtrl'
        });
        $routeProvider.when('/admin/holidays/',
        {
            templateUrl: '/views/Admin/add.html',
            controller: 'adminCtrl'
        });
        $routeProvider.when('/admin/procedures/',
        {
            templateUrl: '/views/Admin/procedures.html',
            controller: 'adminCtrl'
        });

        $routeProvider.when('/404',
        {
            templateUrl: '/views/404.html'
        });

        $routeProvider.otherwise({ redirectTo: '/404' });
    });