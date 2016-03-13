clinicApp.controller('clientCtrl', ['$scope', '$location', 'authService', 'clientService', function ($scope, $location, authService, clientService) {

    if (!authService.authentication.isAuth || authService.authentication.userRole != "Client") {
        $location.path("/");
    };
    
    $scope.doctors = {};

    $scope.currDoctorAccount = {};

    $scope.currWeek = {};

    $scope.currWeekTimes = {};

    $scope.currDoctor = 1;

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



    clientService.GetDoctors().then(function (results) {

        $scope.doctors = results;
        $scope.currDoctorAccount = results[0];

        console.log($scope.currDoctorAccount);

        clientService.GetCurrWeek($scope.currDoctorAccount.Id).then(function (results) {

            $scope.currWeek = results;

            clientService.GetCurrWeekTimes($scope.currDoctorAccount.Id).then(function (results) {

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

                $scope.timeInterval.sort();


            }, function (error) {

                alert(error.data.message);

            });

        }, function (error) {

            alert(error.data.message);

        });



    }, function (error) {

        alert(error.data.message);

    });


    $scope.reloadData = function (id) {
        clientService.GetCurrWeek(id).then(function (results) {
            $scope.currWeek = results;

            clientService.GetCurrWeekTimes(id).then(function (results) {

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

                $scope.timeInterval.sort();

            }, function (error) {

                alert(error.data.message);

            });

        }, function (error) {

            alert(error.data.message);

        });
    }

    $scope.nextDoctor = function () {
        if ($scope.doctors.length > $scope.currDoctor) {
            $scope.currDoctor = $scope.currDoctor + 1;
            $scope.currDoctorAccount = $scope.doctors[$scope.currDoctor - 1];

            $scope.reloadData($scope.currDoctorAccount.Id);

        }
        else {
            $scope.currDoctor = 1;
            $scope.currDoctorAccount = $scope.doctors[$scope.currDoctor - 1];

            $scope.reloadData($scope.currDoctorAccount.Id);
        }
    }

    $scope.prevDoctor = function () {
        if ($scope.currDoctor > 1) {
            $scope.currDoctor = $scope.currDoctor - 1;
            $scope.currDoctorAccount = $scope.doctors[$scope.currDoctor - 1];
        }
        else {
            $scope.currDoctor = $scope.doctors.length;
            $scope.currDoctorAccount = $scope.doctors[$scope.currDoctor - 1];
        }
    }


    $scope.currWeekDown = function (id, dayId) {

        clientService.GetPrevWeek(id, dayId).then(function (results) {

            $scope.currWeek = results;

            clientService.GetPrevWeekTimes(id, dayId).then(function (results) {

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

        clientService.GetNextWeek(id, dayId).then(function (results) {

            if (results != null) {

                $scope.currWeek = results;

                clientService.GetNextWeekTimes(id, dayId).then(function (results) {

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