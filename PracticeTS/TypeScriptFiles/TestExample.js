var TestExampleCtrl;
(function (TestExampleCtrl) {
    var ITestExamplectl = (function () {
        function ITestExamplectl(name) {
            if (name === void 0) { name = "test"; }
            alert(2);
        }
        ITestExamplectl.prototype.cancel = function () {
            alert();
        };
        return ITestExamplectl;
    }());
})(TestExampleCtrl || (TestExampleCtrl = {}));
//angular.module('app')
//    .controller('Controller', TestExampleCtrl); 
//# sourceMappingURL=TestExample.js.map