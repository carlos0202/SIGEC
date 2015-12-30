//tripsController.js
(function(){
	"use strict";
	
	angular.module("app-trips")
		.controller("tripsController", tripsController);
		
	function tripsController($http){
		var vm = this;

		vm.trips = [];
		vm.newTrip = {};
		vm.isBusy = true;
		vm.errorMessage = "";
		$http.get('/api/trips')
			.then(function(response){
				//success
				angular.copy(response.data, vm.trips);
			}, function(error){
				//failure
				vm.errorMessage = "Failed to load data: " + error; 
			}).finally(function(){
				vm.isBusy = false;
			});
		
		vm.addTrip = function(){
			vm.isBusy = true;
			vm.errorMessage = "";
			$http.post('/api/trips', vm.newTrip)
			 	.then(function(response){
					 vm.trips.push(response.data);
				 }, function(error){
					 vm.errorMessage = "failed to save new trip";
				 }).finally(function(){
					 vm.isBusy = false;
				 });
			vm.newTrip = {};
		}
	}
})();