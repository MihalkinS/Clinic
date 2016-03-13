'use strict';
clinicApp.factory('adminService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serverURL = 'http://localhost:49845/';

    var adminServiceFactory = {};

    var _doctors = [];

    var _getDoctors = function () {

        var authData = localStorageService.get('authorizationData');

        return $http.get(serverURL + 'api/User/GetUsersInRole?role=Doctor', {
            headers: {
                'Authorization': ' Bearer ' + authData.token
            }
        })
        .then(function (results) {
            _doctors = results.data;
            return results;

        });
    };

    var _deleteDoctor = function (id) {

        var authData = localStorageService.get('authorizationData');

        return $http.delete(serverURL + 'api/User/' + id, {
            headers: {
                'Authorization': ' Bearer ' + authData.token
            }
        })
        .then(function (results) {
            _doctors = results.data;
            return results;
        });
    };

    var _drugs = {};

    var _drug = {};

    var _procedures = {};



    var _getDrugs = function () {

        var authData = localStorageService.get('authorizationData');

        return $http.get(serverURL + 'api/Drug/AllDrugs', {
            headers: {
                'Authorization': ' Bearer ' + authData.token
            }
        })
        .then(function (results) {

            _drugs = results.data;
            console.log(results.data);
            return results;

        });
    };

    var _getDrugById = function (id) {

        var authData = localStorageService.get('authorizationData');

        return $http.get(serverURL + 'api/Drug?drugId=' + id, {
            headers: {
                'Authorization': ' Bearer ' + authData.token
            }
        })
        .then(function (results) {
            console.log(results.data);
            _drug = results.data;
            return results;

        });
    };

    var _addDrug = function (newDrug) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http.post(serverURL + 'api/Drug', newDrug, { 
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

    var _addDrugInStorage = function (drug) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http({
            url: serverURL + 'api/Drug/AddDrug', 
            method: "POST",
            params: { drugId: drug.Id, count: drug.count },
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

    var _getProcedures = function () {

        return $http.get(serverURL + 'api/Procedure')
        .then(function (results) {

            _procedures = results.data;
            return results;

        });

    };

    var _saveProcedure = function (newProcedure) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http.post(serverURL + 'api/Procedure', newProcedure, {
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

    var _deleteProcedure = function (procedureId) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        console.log(procedureId);

        $http.delete(serverURL + 'api/procedure?procedureId=' + procedureId, {
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

    var _saveDoctor = function (newDoctor) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http.post(serverURL + 'api/account/RegisterDoctor', newDoctor, {
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


    adminServiceFactory.getDoctors = _getDoctors;
    adminServiceFactory.deleteDoctor = _deleteDoctor;
    adminServiceFactory.doctors = _doctors;
    adminServiceFactory.drugs = _drugs;
    adminServiceFactory.getDrugs = _getDrugs;
    adminServiceFactory.getDrugById = _getDrugById;
    adminServiceFactory.addDrug = _addDrug;
    adminServiceFactory.addDrugInStorage = _addDrugInStorage;
    adminServiceFactory.getProcedures = _getProcedures;
    adminServiceFactory.procedures = _procedures;
    adminServiceFactory.saveProcedure = _saveProcedure;
    adminServiceFactory.saveDoctor = _saveDoctor;
    adminServiceFactory.deleteProcedure = _deleteProcedure;


    return adminServiceFactory;

}]);