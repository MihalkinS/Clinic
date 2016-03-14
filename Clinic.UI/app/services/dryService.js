'use strict';
clinicApp.factory('dryService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var dryServiceFactory = {};

    var _timeIntervalForADay = function (allIntervalsMass, currDayId) {

        var resultMass = [];

        for (var i = 0; i < allIntervalsMass.length; i++) {

            if (allIntervalsMass[i].DayId == currDayId) {
                resultMass.push(allIntervalsMass[i].ClientId);
                resultMass.push(allIntervalsMass[i].DayId);
                resultMass.push(allIntervalsMass[i].TimeId);
                resultMass.push(allIntervalsMass[i].VisitId);
            };
        }

        return resultMass;
    }

    dryServiceFactory.timeIntervalForADay = _timeIntervalForADay;

        return dryServiceFactory;

}]);