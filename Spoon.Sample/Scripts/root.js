angular.module("root", ["ngRoute"])
    .config(["$locationProvider", function ($locationProvider) {
        $locationProvider.html5Mode(true);
        $locationProvider.hashPrefix('!');
    }])
    .config(["$routeProvider", function($routeProvider) {
        $routeProvider
            .when("/home", { templateUrl: "/Content/Views/Home.html" })
            .when("/contact", { templateUrl: "/Content/Views/Contact.html" })
            .when("/about", { templateUrl: "/Content/Views/About.html" })
            .otherwise({ redirectTo: "/home" });
    }])
    .controller("index", []);