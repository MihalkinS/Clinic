clinicApp.controller('commentCtrl', ['$scope', '$routeParams', '$location', 'authService', 'commentService', function ($scope, $routeParams, $location, authService, commentService) {

    if (!authService.authentication.isAuth || ((authService.authentication.userRole != 'Client') && 
        (authService.authentication.userRole != 'Doctor'))) {
        $location.path("/");
    };

    $scope.comments = [];
    $scope.doctors = [];
    $scope.currDoctorAccount = {};

    $scope.text = '';

    $scope.currDoctor = 1;
    $scope.flagOfFirstLoad = true;

    $scope.loadData = function () {

        commentService.getDoctors().then(function (results) {

            if ($scope.flagOfFirstLoad) {
                $scope.doctors = results;
                $scope.currDoctorAccount = results[0];
                $scope.flagOfFirstLoad = false;
            };


            commentService.getComments($scope.currDoctorAccount.Id).then(function (results) {

                $scope.comments = results;

            });

        });

    };
      
    $scope.loadData();


    $scope.postComment = function (text) {

        commentService.postComment($scope.currDoctorAccount.Id, text).then(function (results) {

            $scope.loadData();

            $scope.text = "";
        });
    };
     
    $scope.nextDoctor = function () {
        if ($scope.doctors.length > $scope.currDoctor) {
            $scope.currDoctor = $scope.currDoctor + 1;
            $scope.currDoctorAccount = $scope.doctors[$scope.currDoctor - 1];

            $scope.loadData();

        }
        else {
            $scope.currDoctor = 1;
            $scope.currDoctorAccount = $scope.doctors[$scope.currDoctor - 1];

            $scope.loadData();
        }
    }

    $scope.prevDoctor = function () {
        if ($scope.currDoctor > 1) {
            $scope.currDoctor = $scope.currDoctor - 1;
            $scope.currDoctorAccount = $scope.doctors[$scope.currDoctor - 1];

            $scope.loadData();
        }
        else {
            $scope.currDoctor = $scope.doctors.length;
            $scope.currDoctorAccount = $scope.doctors[$scope.currDoctor - 1];

            $scope.loadData();
        }
    }


}]);