'use strict';
clinicApp.factory('commentService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serverURL = 'http://localhost:49845/';

    var commentServiceFactory = {};

    
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
    

    var _getComments = function (DoctorId) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http.get(serverURL + 'api/Comment?doctorId=' + DoctorId, {
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
    
    
    var _postComment = function (doctorId, comment) {

        var authData = localStorageService.get('authorizationData');

        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: serverURL + 'api/Comment',
            data: { doctorId: doctorId, Text: comment },
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
    
    commentServiceFactory.getDoctors = _getDoctors;
    commentServiceFactory.getComments = _getComments;
    commentServiceFactory.postComment = _postComment;

    return commentServiceFactory;

}]);