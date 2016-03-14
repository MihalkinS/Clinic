clinicApp.controller('doctorCtrl', ['$scope', '$location', 'authService', 'doctorService', 'dryService', function ($scope, $location, authService, doctorService, dryService) {

    if (!authService.authentication.isAuth || authService.authentication.userRole != "Doctor") {
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



    }, function (error) {

        alert(error.data.message);

    });


    

    

    $scope.currWeekDown = function (id, dayId) {

        doctorService.GetPrevWeek(id, dayId).then(function (results) {

            $scope.currWeek = results;

            doctorService.GetPrevWeekTimes(id, dayId).then(function (results) {

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

        doctorService.GetNextWeek(id, dayId).then(function (results) {

            if (results != null) {

                $scope.currWeek = results;

                doctorService.GetNextWeekTimes(id, dayId).then(function (results) {

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