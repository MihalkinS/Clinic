clinicApp.controller('clientVisitCtrl', ['$scope', '$location', '$routeParams', 'authService', 'clientService', function ($scope, $location, $routeParams, authService, clientService) {

    if (!authService.authentication.isAuth || authService.authentication.userRole != "Client") {
        $location.path("/");
    };

    $scope.redirectToCalendar = function () {
        $location.path("/client/calendar/");
    };


    $scope.time = {};
    $scope.doctor = {};
    $scope.procedures = [];
    $scope.clientId = authService.authentication.id;

    $scope.visit = {
        timeId: "",
        procedureId: "",
        clientId: authService.authentication.id,
        doctorId: "",
        dayId: ""
    };

    clientService.getTimeById($routeParams.id).then(function (results) {

        $scope.time = results;

        clientService.getDoctorById($scope.time.DoctorId).then(function (results) {

            $scope.doctor = results.data;

        }, function (error) {

            alert(error.data.message);
        });

    }, function (error) {

        alert(error.data.message);
    });

    clientService.getProcedures().then(function (results) {

        $scope.procedures = results.data;
        console.log($scope.procedures);

    }, function (error) {

        alert(error.data.message);

    });

    $scope.saveVisit = function (visitForm, procId) {
        if (visitForm.$valid) {

            $scope.visit.timeId = $routeParams.id;
            $scope.visit.doctorId = $scope.time.DoctorId;
            $scope.visit.procedureId = procId;
            $scope.visit.dayId = $scope.time.DayId;

            console.log($scope.visit);

            clientService.saveVisit($scope.visit).then(function (results) {
                $location.path("/client/calendar/");
            },function (error) {
                alert(error);
            })
        };
    };

}]);