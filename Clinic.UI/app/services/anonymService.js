'use strict';
clinicApp.factory('anonymService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serverURL = 'http://localhost:49845/';

    var anonymServiceFactory = {};

    var _doctors = {};
    var _currWeek = {};
    var _currWeekTimes = {};

    var currDoctorId = '';

    var _getDoctors = function () {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Doctor')
            .success(function (response) {
                
                deferred.resolve(response);

            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getCurrWeek = function (id) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/CurrWeek?doctorId=' + id)
            .success(function (response) {
                _currWeek = response;
                deferred.resolve(response);

            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getNextWeek = function (id, lastDayId) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/NextWeek?doctorId=' + id + "&lastDayId=" + lastDayId)
            .success(function (response) {
                _currWeek = response;
                deferred.resolve(response);

            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getPrevWeek = function (id, firstDayId) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/PrevWeek?doctorId=' + id + "&firstDayId=" + firstDayId)
            .success(function (response) {
                _currWeek = response;
                deferred.resolve(response);

            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getCurrWeekTimes = function (currDoctorId) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/CurrWeekTimes?doctorId=' + currDoctorId)
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getNextWeekTimes = function (currDoctorId, lastDayId) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/NextWeekTimes?doctorId=' + currDoctorId + '&lastDayId=' + lastDayId)
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getPrevWeekTimes = function (currDoctorId, firstDayId) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/PrevWeekTimes?doctorId=' + currDoctorId + '&firstDayId=' + firstDayId)
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    anonymServiceFactory.GetDoctors = _getDoctors;
    anonymServiceFactory.GetCurrWeek = _getCurrWeek;
    anonymServiceFactory.GetNextWeek = _getNextWeek;
    anonymServiceFactory.GetPrevWeek = _getPrevWeek;
    anonymServiceFactory.GetCurrWeekTimes = _getCurrWeekTimes;
    anonymServiceFactory.GetNextWeekTimes = _getNextWeekTimes;
    anonymServiceFactory.GetPrevWeekTimes = _getPrevWeekTimes;
    anonymServiceFactory.Doctors = _doctors;
    anonymServiceFactory.CurrWeekTimes = _currWeekTimes;


    return anonymServiceFactory;

}]);