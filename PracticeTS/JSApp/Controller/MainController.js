var routerApp;
(function (routerApp) {
    var Mainctrl;
    (function (Mainctrl) {
        "use strict";
        var MainListCtrl = (function () {
            function MainListCtrl($scope, myservice) {
                this.$scope = $scope;
                this.myservice = myservice;
                this.scope = $scope;
                this.scope.testval = "fdsfdsf fsdfsd";
                alert(this.scope.testval);
                this.scope.Test = function (val) {
                    alert(val);
                };
            }
            MainListCtrl.prototype.getMyAudits = function (val) {
                var data = this.myservice.getData();
                this.scope.testval = data;
                alert(data);
            };
            MainListCtrl.$inject = ['$scope', 'MainctrlService'];
            return MainListCtrl;
        }());
        angular.module('routerApp')
            .controller('UserListCtrl', MainListCtrl);
    })(Mainctrl = routerApp.Mainctrl || (routerApp.Mainctrl = {}));
})(routerApp || (routerApp = {}));
//# sourceMappingURL=MainController.js.map