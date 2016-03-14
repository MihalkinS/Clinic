'use strict';
clinicApp.factory('clientService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serverURL = 'http://localhost:49845/';

    var clientServiceFactory = {};

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
                });
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


    var _getVisit = function (id) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: serverURL + 'api/visit/' + id,      
            headers: { 'Authorization': ' Bearer ' + authData.token }
        })
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var _getDoctorById = function (id) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Doctor?doctorId=' + id)
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var _getTimeById = function (id) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/Time?timeId=' + id)
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var _getProcedures = function () {

        return $http.get(serverURL + 'api/Procedure')
        .then(function (results) {
            return results;
        });

    };

    var _saveVisit = function (newVisit) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http.post(serverURL + 'api/visit', newVisit, {
            headers: {
                'Authorization': ' Bearer ' + authData.token
            }
        })
        .success(function (response) {
            deferred.resolve(response);
        })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var _getVisits = function () {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http.get(serverURL + 'api/visit/ClientVisits', {
            headers: {
                'Authorization': ' Bearer ' + authData.token,
                'Content-type': ' application/json'
            }
        })
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    }



    clientServiceFactory.GetDoctors = _getDoctors;
    clientServiceFactory.GetDoctorById = _getDoctorById;
    clientServiceFactory.GetCurrWeek = _getCurrWeek;
    clientServiceFactory.GetNextWeek = _getNextWeek;
    clientServiceFactory.GetPrevWeek = _getPrevWeek;
    clientServiceFactory.GetCurrWeekTimes = _getCurrWeekTimes;
    clientServiceFactory.GetNextWeekTimes = _getNextWeekTimes;
    clientServiceFactory.GetPrevWeekTimes = _getPrevWeekTimes;
    clientServiceFactory.Doctors = _doctors;
    clientServiceFactory.CurrWeekTimes = _currWeekTimes;
    clientServiceFactory.getVisit = _getVisit;
    clientServiceFactory.getTimeById = _getTimeById;
    clientServiceFactory.getDoctorById = _getDoctorById;
    clientServiceFactory.getProcedures = _getProcedures;
    clientServiceFactory.saveVisit = _saveVisit;
    clientServiceFactory.getVisits = _getVisits;


    return clientServiceFactory;

}]);