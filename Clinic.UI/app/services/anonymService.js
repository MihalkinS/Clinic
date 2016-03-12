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
                
                _doctors = response;
                currDoctorId = response[0].Id;
                _getCurrWeekTimes(response[0].Id).then(function (results) {
                    _currWeekTimes = results;
                    console.log(results);
                    console.log(_currWeekTimes);
                    console.log(_currWeekTimes);
                });
                deferred.resolve(response);

            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getCurrWeek = function () {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/CurrWeek')
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

    anonymServiceFactory.GetDoctors = _getDoctors;
    anonymServiceFactory.GetCurrWeek = _getCurrWeek;
    anonymServiceFactory.GetCurrWeekTimes = _getCurrWeekTimes;
    anonymServiceFactory.Doctors = _doctors;
    anonymServiceFactory.CurrWeekTimes = _currWeekTimes;


    return anonymServiceFactory;

}]);