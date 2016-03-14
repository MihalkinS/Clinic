clinicApp.controller('clientCtrl', ['$scope', '$location', 'authService', 'clientService', 'dryService', function ($scope, $location, authService, clientService, dryService) {

    if (!authService.authentication.isAuth || authService.authentication.userRole != "Client") {
        $location.path("/");
    };
    

    $scope.DRYcurrWeekTimes = [];

    $scope.doctors = {};

    $scope.currDoctorAccount = {};

    $scope.currWeek = {};

    $scope.currWeekTimes = {};

    $scope.currDoctor = 1;

    $scope.visits = {};

    $scope.visitsCostSum = 0;

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

                $scope.fisrtDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[0].id);
                $scope.secondDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[1].id);
                $scope.thirdDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[2].id);
                $scope.fourthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[3].id);
                $scope.fifthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[4].id);
                $scope.sixthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[5].id);
                $scope.seventhDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[6].id);

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

    clientService.getVisits().then(function (results) {

        console.log(results);
        console.log(results);

        $scope.visits = results;

        var sum = 0;

        for (var visit in $scope.visits) {
            sum = sum + $scope.visits[visit].Cost;
            console.log(visit);
        };

        $scope.visitsCostSum = sum;

    });

    $scope.reloadData = function (id) {
        clientService.GetCurrWeek(id).then(function (results) {
            $scope.currWeek = results;

            clientService.GetCurrWeekTimes(id).then(function (results) {

                $scope.cleanArrays();
                $scope.currWeekTimes = results;

                $scope.fisrtDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[0].id);
                $scope.secondDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[1].id);
                $scope.thirdDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[2].id);
                $scope.fourthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[3].id);
                $scope.fifthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[4].id);
                $scope.sixthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[5].id);
                $scope.seventhDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[6].id);

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

                $scope.fisrtDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[0].id);
                $scope.secondDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[1].id);
                $scope.thirdDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[2].id);
                $scope.fourthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[3].id);
                $scope.fifthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[4].id);
                $scope.sixthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[5].id);
                $scope.seventhDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[6].id);
               


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

                    $scope.fisrtDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[0].id);
                    $scope.secondDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[1].id);
                    $scope.thirdDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[2].id);
                    $scope.fourthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[3].id);
                    $scope.fifthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[4].id);
                    $scope.sixthDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[5].id);
                    $scope.seventhDayMass = dryService.timeIntervalForADay($scope.currWeekTimes, $scope.currWeek[6].id);

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