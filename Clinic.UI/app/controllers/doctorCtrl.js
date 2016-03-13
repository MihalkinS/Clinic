clinicApp.controller('doctorCtrl', ['$scope', '$location', 'authService', 'doctorService', function ($scope, $location, authService, doctorService) {

    if (!authService.authentication.isAuth || authService.authentication.userRole != "Doctor") {
        console.log("slkdjglskdgjlskdgjlsdg");
        console.log(authService.authentication.isAuth);
        console.log(authService.authentication.userRole);
        $location.path("/");
    };

    $scope.doctors = {};

    $scope.currDoctorAccount = {};

    $scope.currWeek = {};

    $scope.currWeekTimes = {};

    $scope.currDoctor = 1;

    $scope.doctorId = authService.authentication.id;

    $scope.timeInterval = [];
    $scope.fisrtDayMass = [];
    $scope.secondDayMass = [];
    $scope.thirdDayMass = [];
    $scope.fourthDayMass = [];
    $scope.fifthDayMass = [];
    $scope.sixthDayMass = [];
    $scope.seventhDayMass = [];

    $scope.cleanArrays = function () {
        $scope.timeInterval = [];
        $scope.fisrtDayMass = [];
        $scope.secondDayMass = [];
        $scope.thirdDayMass = [];
        $scope.fourthDayMass = [];
        $scope.fifthDayMass = [];
        $scope.sixthDayMass = [];
        $scope.seventhDayMass = [];
    }



    doctorService.GetDoctor($scope.doctorId).then(function (results) {

        $scope.doctor = results;

        doctorService.GetCurrWeek($scope.doctorId).then(function (results) {

            $scope.currWeek = results;

            doctorService.GetCurrWeekTimes($scope.doctorId).then(function (results) {

                $scope.currWeekTimes = results;

                for (var i = 0; i < $scope.currWeekTimes.length; i++) {
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[0].id) {
                        $scope.fisrtDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.fisrtDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.fisrtDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.fisrtDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[1].id) {
                        $scope.secondDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.secondDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.secondDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.secondDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[2].id) {
                        $scope.thirdDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.thirdDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.thirdDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.thirdDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[3].id) {
                        $scope.fourthDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.fourthDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.fourthDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.fourthDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[4].id) {
                        $scope.fifthDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.fifthDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.fifthDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.fifthDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[5].id) {
                        $scope.sixthDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.sixthDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.sixthDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.sixthDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[6].id) {
                        $scope.seventhDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.seventhDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.seventhDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.seventhDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                };

                for (var i = 0; i < $scope.currWeekTimes.length; i++) {
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[0].id) {
                        $scope.timeInterval.push($scope.currWeekTimes[i].HM);

                    };
                };

            }, function (error) {

                alert(error.data.message);

            });

        }, function (error) {

            alert(error.data.message);

        });



    }, function (error) {

        alert(error.data.message);

    });


    

    

    $scope.currWeekDown = function (id, dayId) {

        doctorService.GetPrevWeek(id, dayId).then(function (results) {

            $scope.currWeek = results;

            doctorService.GetPrevWeekTimes(id, dayId).then(function (results) {

                $scope.cleanArrays();
                $scope.currWeekTimes = results;

                for (var i = 0; i < $scope.currWeekTimes.length; i++) {
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[0].id) {
                        $scope.fisrtDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.fisrtDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.fisrtDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.fisrtDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[1].id) {
                        $scope.secondDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.secondDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.secondDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.secondDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[2].id) {
                        $scope.thirdDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.thirdDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.thirdDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.thirdDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[3].id) {
                        $scope.fourthDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.fourthDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.fourthDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.fourthDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[4].id) {
                        $scope.fifthDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.fifthDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.fifthDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.fifthDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[5].id) {
                        $scope.sixthDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.sixthDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.sixthDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.sixthDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[6].id) {
                        $scope.seventhDayMass.push($scope.currWeekTimes[i].ClientId);
                        $scope.seventhDayMass.push($scope.currWeekTimes[i].DayId);
                        $scope.seventhDayMass.push($scope.currWeekTimes[i].TimeId);
                        $scope.seventhDayMass.push($scope.currWeekTimes[i].VisitId);
                    };
                };

                for (var i = 0; i < $scope.currWeekTimes.length; i++) {
                    if ($scope.currWeekTimes[i].DayId == $scope.currWeek[0].id) {
                        $scope.timeInterval.push($scope.currWeekTimes[i].HM);

                    };
                };

            }, function (error) {

                alert(error.data.message);

            });

        }, function (error) {

            alert(error.data.message);

        });

    };

    $scope.currWeekUp = function (id, dayId) {

        doctorService.GetNextWeek(id, dayId).then(function (results) {

            if (results != null) {

                $scope.currWeek = results;

                doctorService.GetNextWeekTimes(id, dayId).then(function (results) {

                    $scope.cleanArrays();
                    $scope.currWeekTimes = results;

                    console.log($scope.currWeekTimes);

                    for (var i = 0; i < $scope.currWeekTimes.length; i++) {
                        if ($scope.currWeekTimes[i].DayId == $scope.currWeek[0].id) {
                            $scope.fisrtDayMass.push($scope.currWeekTimes[i].ClientId);
                            $scope.fisrtDayMass.push($scope.currWeekTimes[i].DayId);
                            $scope.fisrtDayMass.push($scope.currWeekTimes[i].TimeId);
                            $scope.fisrtDayMass.push($scope.currWeekTimes[i].VisitId);
                        };
                        if ($scope.currWeekTimes[i].DayId == $scope.currWeek[1].id) {
                            $scope.secondDayMass.push($scope.currWeekTimes[i].ClientId);
                            $scope.secondDayMass.push($scope.currWeekTimes[i].DayId);
                            $scope.secondDayMass.push($scope.currWeekTimes[i].TimeId);
                            $scope.secondDayMass.push($scope.currWeekTimes[i].VisitId);
                        };
                        if ($scope.currWeekTimes[i].DayId == $scope.currWeek[2].id) {
                            $scope.thirdDayMass.push($scope.currWeekTimes[i].ClientId);
                            $scope.thirdDayMass.push($scope.currWeekTimes[i].DayId);
                            $scope.thirdDayMass.push($scope.currWeekTimes[i].TimeId);
                            $scope.thirdDayMass.push($scope.currWeekTimes[i].VisitId);
                        };
                        if ($scope.currWeekTimes[i].DayId == $scope.currWeek[3].id) {
                            $scope.fourthDayMass.push($scope.currWeekTimes[i].ClientId);
                            $scope.fourthDayMass.push($scope.currWeekTimes[i].DayId);
                            $scope.fourthDayMass.push($scope.currWeekTimes[i].TimeId);
                            $scope.fourthDayMass.push($scope.currWeekTimes[i].VisitId);
                        };
                        if ($scope.currWeekTimes[i].DayId == $scope.currWeek[4].id) {
                            $scope.fifthDayMass.push($scope.currWeekTimes[i].ClientId);
                            $scope.fifthDayMass.push($scope.currWeekTimes[i].DayId);
                            $scope.fifthDayMass.push($scope.currWeekTimes[i].TimeId);
                            $scope.fifthDayMass.push($scope.currWeekTimes[i].VisitId);
                        };
                        if ($scope.currWeekTimes[i].DayId == $scope.currWeek[5].id) {
                            $scope.sixthDayMass.push($scope.currWeekTimes[i].ClientId);
                            $scope.sixthDayMass.push($scope.currWeekTimes[i].DayId);
                            $scope.sixthDayMass.push($scope.currWeekTimes[i].TimeId);
                            $scope.sixthDayMass.push($scope.currWeekTimes[i].VisitId);
                        };
                        if ($scope.currWeekTimes[i].DayId == $scope.currWeek[6].id) {
                            $scope.seventhDayMass.push($scope.currWeekTimes[i].ClientId);
                            $scope.seventhDayMass.push($scope.currWeekTimes[i].DayId);
                            $scope.seventhDayMass.push($scope.currWeekTimes[i].TimeId);
                            $scope.seventhDayMass.push($scope.currWeekTimes[i].VisitId);
                        };
                    };

                    for (var i = 0; i < $scope.currWeekTimes.length; i++) {
                        if ($scope.currWeekTimes[i].DayId == $scope.currWeek[0].id) {
                            $scope.timeInterval.push($scope.currWeekTimes[i].HM);

                        };
                    };

                }, function (error) {

                    alert(error.data.message);

                });

            }


        }, function (error) {

            alert(error.data.message);

        });

    };

}]);