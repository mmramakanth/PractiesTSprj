module routerApp.Mainctrl {
    "use strict";

    class MainListCtrl {
        scope: any; testval: string;
        static $inject = ['$scope',  'MainctrlService'];
        constructor(private $scope: ng.IScope, private myservice: routerApp.Mainctrl.MainctrlService) {

            this.scope = $scope;
            this.scope.testval = "fdsfdsf fsdfsd";
            alert(this.scope.testval);
            this.scope.Test = function (val) {
                alert(val);
            }
        }
        getMyAudits(val): void {
            var data = this.myservice.getData();
            this.scope.testval = data;
           alert(data);
        }

        
    }



    angular.module('routerApp')
        .controller('UserListCtrl', MainListCtrl);
}