!function(){"use strict";function i(i){var r=this;r.trips=[],r.newTrip={},r.isBusy=!0,r.errorMessage="",i.get("/api/trips").then(function(i){angular.copy(i.data,r.trips)},function(i){r.errorMessage="Failed to load data: "+i})["finally"](function(){r.isBusy=!1}),r.addTrip=function(){r.isBusy=!0,r.errorMessage="",i.post("/api/trips",r.newTrip).then(function(i){r.trips.push(i.data)},function(i){r.errorMessage="failed to save new trip"})["finally"](function(){r.isBusy=!1}),r.newTrip={}}}angular.module("app-trips").controller("tripsController",i)}();