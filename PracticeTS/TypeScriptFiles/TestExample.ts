module TestExampleCtrl {
    interface ITestExample {
        cancel: () => void;
    }

    class ITestExamplectl implements ITestExample {
        item: any;
        constructor(name: string = "test") {
            alert(2);
        }
        cancel(): void {
            alert();
        }
    }
}
//angular.module('app')
//    .controller('Controller', TestExampleCtrl);