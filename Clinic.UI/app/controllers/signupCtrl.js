'use strict';
clinicApp.controller('signupCtrl', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    if (authService.authentication.isAuth) {
        $location.path("/");
    };

    $scope.viewForm = true;

    $scope.errorMessage = '';

    $scope.loginData = {
        userName: "",
        email: "",
        phoneNumber:"",
        password: "",
        confirmPassword: "",
        role: "Client",
        firstName: "",
        lastName: "",
        middleName: "",
        petName: "",
        color: "",
        breed: ""
    };

    $scope.redirectToMainPage = function () {
        $location.path("/");
    };

    $scope.register = function (loginData, signUpForm) {
            
        console.log(loginData);

            if (loginData.password !== loginData.confirmPassword) {
                $scope.errorMessage = 'Пароли не совпадают!';
                return;
            };
            
            
            authService.saveClientRegistration(loginData).then(function (response) {

                $scope.errorMessage = '';
                $scope.viewForm = false;

            },
            function (err) {
                alert(err);
                $scope.errorMessage = err.error_description;
            });

    };



}]);