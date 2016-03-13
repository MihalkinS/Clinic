'use strict';
clinicApp.factory('doctorService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serverURL = 'http://localhost:49845/';

    var doctorServiceFactory = {};

    var _doctor = {};
    var _currWeek = {};
    var _currWeekTimes = {};

    var _getDoctor = function (id) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Doctor?Id=' + id)
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

    var _getCurrWeekTimes = function (DoctorId) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/CurrWeekTimes?doctorId=' + DoctorId)
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getNextWeekTimes = function (DoctorId, lastDayId) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/NextWeekTimes?doctorId=' + DoctorId + '&lastDayId=' + lastDayId)
            .success(function (response) {
                deferred.resolve(response);
            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getPrevWeekTimes = function (DoctorId, firstDayId) {

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Day/PrevWeekTimes?doctorId=' + DoctorId + '&firstDayId=' + firstDayId)
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

        $http.get(serverURL + 'api/visit/' + id, {
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

        $http.post(serverURL + 'api/visit/FromDoctor', newVisit, {
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

    var _getClientById = function (id) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Client?Id=' + id, {
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

    var _getClients = function () {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Client', {
            headers: {
                'Authorization': ' Bearer ' + authData.token
            }
        })
            .success(function (response) {
                console.log("response");
                console.log(response);
                deferred.resolve(response);

            })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _getDrugs = function () {

        var authData = localStorageService.get('authorizationData');

        return $http.get(serverURL + 'api/Drug/AllDrugs', {
            headers: {
                'Authorization': ' Bearer ' + authData.token
            }
        })
        .then(function (results) {
            console.log(results.data);
            return results;

        });
    };

    doctorServiceFactory.GetDoctor = _getDoctor;
    doctorServiceFactory.GetCurrWeek = _getCurrWeek;
    doctorServiceFactory.GetNextWeek = _getNextWeek;
    doctorServiceFactory.GetPrevWeek = _getPrevWeek;
    doctorServiceFactory.GetCurrWeekTimes = _getCurrWeekTimes;
    doctorServiceFactory.GetNextWeekTimes = _getNextWeekTimes;
    doctorServiceFactory.GetPrevWeekTimes = _getPrevWeekTimes;
    doctorServiceFactory.CurrWeekTimes = _currWeekTimes;
    doctorServiceFactory.getVisit = _getVisit;
    doctorServiceFactory.getTimeById = _getTimeById;
    doctorServiceFactory.getProcedures = _getProcedures;
    doctorServiceFactory.saveVisit = _saveVisit;
    doctorServiceFactory.getClientById = _getClientById;
    doctorServiceFactory.getClients = _getClients;
    doctorServiceFactory.getDrugs = _getDrugs;

    return doctorServiceFactory;

}]);