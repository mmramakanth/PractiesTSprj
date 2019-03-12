var routerApp;
(function (routerApp) {
    var Mainctrl;
    (function (Mainctrl) {
        "use strict";
        var MainctrlService = (function () {
            function MainctrlService($http) {
                this.$http = $http;
            }
            MainctrlService.prototype.getData = function () {
                return "success";
            };
            return MainctrlService;
        }());
        Mainctrl.MainctrlService = MainctrlService;
        angular.module("routerApp").service("MainctrlService", MainctrlService);
    })(Mainctrl = routerApp.Mainctrl || (routerApp.Mainctrl = {}));
})(routerApp || (routerApp = {}));
//# sourceMappingURL=MainService.js.map