app.controller("userController", ['$scope', '$http', '$location', '$routeParams', function ($scope, $http, $location, $routeParams) {
    $scope.ListofUser;
    $scope.Status;
    $scope.logindata={User_Name:null, User_Password:null};
    $scope.useremail;
    $scope.Close = function () {
        $location.path('/UserList');
    }
    $scope.GetAllUsers = function () {
        //Get all user and bind with html table
        $http.get('api/user/GetAllUsers/').success(function (data) {
            $scope.ListofUser = data;
        })
        .error(function (data) {
            $scope.status = "data not found";
            alert(data);
        });
    }
    //Add new user
    $scope.Add = function () {
        var userData = {
            FirstName: $scope.FirstName,
            LastName: $scope.LastName,
            User_Name: $scope.User_Name,
            User_Password: $scope.User_Password,
            User_Email: $scope.User_Email,
        };
        debugger;
        $http.post('api/user/AddUser/', userData).success(function (data) {
            $location.path('/UserList');
        }).error(function (data) {
            console.log(data);
            alert(data);
            $scope.error = "Something went wrong when adding new user" + data.Exception;
        });
    }

    //Fill the user records for update
    if ($routeParams.userId!=null) {
        $scope.Id = $routeParams.userId;
    }
    if(angular.isDefined($scope.Id))
    $http.get('api/user/GetUser/' + $scope.Id).success(function (data) {
        FirstName= $scope.FirstName,
        LastName      =data.LastName,
        User_Name     =data.User_Name,
        User_Password =data.User_Password,
        User_Email    =data.User_Email
    });

    //Update the user records
    $scope.Update = function () {
        debugger;
        var userData = {
            _id: $scope.Id,
            FirstName: $scope.FirstName,
            LastName: $scope.LastName,
            User_Name: $scope.User_Name,
            User_Password: $scope.User_Password,
            User_Email: $scope.User_Email
        };
        if ($scope.Id != null) {
            $http.put('api/user/UpdateUser/', userData).success(function (data) {
                $location.path('/UserList');
            }).error(function (data) {
                console.log(data);
                alert(data);
                $scope.error = "something went wrong when updating the user" + data.Exception;
            });
        }
    }
    //Delete the selected user from the list
    $scope.Delete = function () {
        if ($scope.Id != null) {

            $http.delete('api/user/DeleteUser/' + $scope.Id).success(function (data) {
                $location.path('/UserList');
            }).error(function (data) {
                console.log(data);
                alert(data);
                $scope.error = "Something wrong when Deleting User " + data.ExceptionMessage;
            });
        }
    }

    $scope.Login = function (logindata) {
        if (angular.isDefined(logindata) && logindata!=null) {
            if (logindata.User_Name == null || logindata.User_Password == null) {
                alert("Please Provide complete user data to proceed for login");
            }
            else {
                var userData = {
                    FirstName: null,
                    LastName: null,
                    User_Name: logindata.User_Name,
                    User_Password: logindata.User_Password,
                    User_Email:null,
                };
                $http.post('api/user/Login/', userData).success(function (data) {
                    alert(data);
                    $location.path('/UserList');
                }).error(function (data) {
                    console.log(data);
                    alert(data);
                    $scope.error = "Something wrong during login " + data.ExceptionMessage;
                });
            }
        }        
    }
    $scope.ForgotPassword = function (useremail) {
        if (angular.isDefined(useremail) && useremail != null) {
            var userData = {
                FirstName: null,
                LastName: null,
                User_Name: null,
                User_Password: null,
                User_Email: useremail,
            };
            $http.post('api/user/SendEmail/', userData).success(function (data) {
                $location.path('/Login');
                alert(data);
            }).error(function (data) {
                console.log(data);
                alert(data);
                $scope.error = "Something wrong during login " + data.ExceptionMessage;
            });
        }
    }
}]);