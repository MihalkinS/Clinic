clinicApp.controller('doctorVisitCtrl', ['$scope', '$location', '$routeParams', 'authService', 'doctorService', function ($scope, $location, $routeParams, authService, doctorService) {

    if (!authService.authentication.isAuth || authService.authentication.userRole != "Doctor") {
        $location.path("/");
    };

    $scope.redirectToCalendar = function () {
        $location.path("/doctor/calendar/");
    };


    $scope.time = {};
    $scope.doctor = {};

    $scope.procedures = [];
    $scope.clients = {};
    $scope.drugs = [];

    $scope.visit = {
        timeId: "",
        procedureId: "",
        clientId: "",
        drugId: "",
        confirmation: true,
        doctorId: authService.authentication.id,
        dayId: ""
    };

    doctorService.getTimeById($routeParams.id).then(function (results) {

        $scope.time = results;

        doctorService.getClients().then(function (results) {

            $scope.clients = results;
            console.log("clients");
            console.log(results.data);

        }, function (error) {

            alert(error.data.message);
        });

    }, function (error) {

        alert(error.data.message);
    });

    doctorService.getProcedures().then(function (results) {

        $scope.procedures = results.data;
        console.log($scope.procedures);

    }, function (error) {

        alert(error.data);

    });

    doctorService.getDrugs().then(function (results) {

        $scope.drugs = results.data;
        console.log("drugs");
        console.log($scope.drugs);

    }, function (error) {

        alert(error.data);

    });



    $scope.saveVisit = function (visitForm, procId, drugId, clientId) {
        if (visitForm.$valid) {

            console.log("-----------------");
            console.log(procId);
            console.log(drugId);
            console.log(clientId);

            $scope.visit.timeId = $routeParams.id;
            $scope.visit.procedureId = procId;
            $scope.visit.drugId = drugId;
            $scope.visit.clientId = clientId;
            $scope.visit.dayId = $scope.time.DayId;

            console.log($scope.visit);

            doctorService.saveVisit($scope.visit).then(function (results) {
                $location.path("/doctor/calendar/");
            }, function (error) {
                alert(error);
            })
        };
    };

}]);