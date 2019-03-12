
module routerApp.Mainctrl {
    "use strict";
    export class MainctrlService {
        constructor(private $http: ng.IHttpService) {
        }
        getData(): string {
        
            return "success";
        }

    }
    angular.module("routerApp").service("MainctrlService", MainctrlService);
}