'use strict';
clinicApp.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serverURL = 'http://localhost:49845/';

    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: '',
        userRole: 'anonym',
        id: ''
    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.userRole = 'anonym';
        _authentication.id = '';

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.userRole = authData.role;
            _authentication.id = authData.id;
        }

    };

    var _saveRegistration = function (registration) {

        _logOut();
        var deferred = $q.defer();

        $http.post(serverURL + 'api/account/register', registration)
        .success(function (response) {
            deferred.resolve(response);
        })
            .error(function (err, status) {
                deferred.reject(err);
            });

        return deferred.promise;

    };

    var _login = function (loginData) {

        var data = "username=" + loginData.userName + "&password=" + loginData.password + "&grant_type=password";
        var deferred = $q.defer();

        console.log(data)

        $http.post(serverURL + 'token', data, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        })
            .success(function (response) {

                localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, role: response.role, id: response.id });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.userRole = response.role;
            _authentication.id = response.id;

            console.log('------');
            console.log(response);
            console.log('------');

            deferred.resolve(response);

            })
            .error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };


    authServiceFactory.login = _login;
    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.logOut = _logOut;

    return authServiceFactory;

}]);