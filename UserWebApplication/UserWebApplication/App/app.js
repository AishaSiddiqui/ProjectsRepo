var app = angular.module('myApp', ['ngRoute']);

app.config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {

    $routeProvider.when('/Login', {
        templateUrl: 'App/templates/Login.html',
        controller: 'userController'
    }).when('/ForgotPassword', {
        templateUrl: 'App/templates/ForgotPassword.html',
        controller: 'userController'
    });
    $routeProvider.when('/UserList', {
        templateUrl: 'App/templates/UserList.html',
        controller:'userController'
    }).when('/AddUser', {
        templateUrl: 'App/templates/AddUser.html',
        controller: 'userController'
    }).when('/EditUser/:userId', {
        templateUrl: 'App/templates/EditUser.html',
        controller: 'userController'
    }).when('/DeleteUser/:userId', {
        templateUrl: 'App/templates/DeleteUser.html',
        controller: 'userController'
    })
    .otherwise({
        controller:'userController'
    })
}]);
