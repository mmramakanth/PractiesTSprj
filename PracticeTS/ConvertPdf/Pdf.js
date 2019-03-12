var routerApp = angular.module('MyApp', ['ngSanitize']);
routerApp.controller("MyController", ["$scope", "$http", function ($scope, $http) {
    var filter; $scope.repdate;
    var scope = $scope; $scope.geninfo = {}; $scope.focuasinfo = {}; $scope.aspecttypes = ['Environment', 'Social', 'Human Rights', 'Anti Corruption'], $scope.dashboardinfo = {}; $scope.currentyear = 0;
    $scope.waterdischargesummary = {}; $scope.waterdatasummary = {}; $scope.assosiatedmaterialdatasummary = {}; $scope.dieaseldatasummary = {};
    $scope.electrictydatasummary = {}; $scope.heatingoildatasummary = {}; $scope.materialpackaging = {}; $scope.naturalgasdatasummary = {};
    $scope.rawmaterial = {}; $scope.solidwastedatasummary = {}; $scope.airemission = {}; $scope.spills = {}; $scope.activesubstances = {}; $scope.producedunits = {};
    $scope.reportbanners = {}; $scope.antibiotic = {}; $scope.qcomments = [];
    $scope.emissionsdata = []; $scope.nitrogenemissions = []; $scope.emissionheatandnatural = []; $scope.emissiondieselupdown = []; $scope.emissionco2equivalents = []; $scope.tspemissions = []; $scope.acidifyinggasses = [];
    var testval;
    var date = new Date();
    $scope.repdate = new Date();
    var id = GetQueryStringParams('id'); var periodId = GetQueryStringParams('periodId');
    alert(id); alert(periodId);
    $(document).ready(function () {
        var questiondata = [{ name: 'waterdischargesummary', type: 'M3' }, { name: 'waterdatasummary', type: 'M3' }, { name: 'assosiatedmaterialdatasummary', type: 'Tonnes' },
            { name: 'dieaseldatasummary', type: 'Liters' }, { name: 'electrictydatasummary', type: 'kwh' }, { name: 'heatingoildatasummary', type: 'Liters' }, { name: 'materialpackaging', type: 'Tonnes' },
            { name: 'naturalgasdatasummary', type: 'M3' }, { name: 'rawmaterial', type: 'Tonnes' }, { name: 'solidwastedatasummary', type: 'Kg' }, { name: 'airemission', type: 'Mg (milligrams)' },
            { name: 'spills', type: 'Kg' }, { name: 'activesubstances', type: 'Tonnes' }, { name: 'antibiotic', type: 'Treatments' }]
        $("#btnchart").click(function () {
            for (var q = 0; q < questiondata.length; q++) {
                var qname = questiondata[q]["name"];
                var stringdata = ($("#" + qname + "_hid").val());
                var parsedata = [];
                if (stringdata != "") {
                    parsedata = JSON.parse(stringdata);
                }
                var options1 = {
                    animationEnabled: true,
                    theme: "light2",
                    title: {
                        text: questiondata[q]["type"],
                        fontColor: "#4f92CE",
                    },
                    height: 150,
                    axisX: {


                    },
                    axisY: {
                        title: "",
                        suffix: "",
                        labelFontSize: 15,

                    },
                    toolTip: {
                        content: "{label}: {y}"
                    },
                    legend: {
                        cursor: "pointer",
                        verticalAlign: "top",
                        horizontalAlign: "right",
                        dockInsidePlotArea: false,
                        itemclick: toogleDataSeries1
                    },

                    data: [{
                        type: "line",
                        showInLegend: false,
                        markerType: "circle",
                        markerSize: 5,
                        lineThickness: 1,
                        color: "#87C330",
                        dataPoints: parsedata
                    }]
                };
                $("#" + qname + "_chart").CanvasJSChart(options1);
                function toogleDataSeries1(e) {
                    if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                        e.dataSeries.visible = false;
                    } else {
                        e.dataSeries.visible = true;
                    }
                    e.chart.render();
                }
            }

        });
    });
    $http({
        method: "Post",
        url: 'http://testreporting.respaunce.com/api/report/getpublishpdfinfo/' + id + '/period/' + periodId,
    }).then(function (result) {
        if (result.data.focuasareinfo.aspectlst.length > 0) {
            $scope.aspecttypes = [];
            angular.forEach(result.data.focuasareinfo.aspectlst, function (val, key) {
                $scope.aspecttypes.push(val.aspecttype);
            });
            angular.copy(result.data.generalnfo, $scope.geninfo);
            angular.copy(result.data.getbanners, $scope.reportbanners);
            angular.copy(result.data.focuasareinfo, $scope.focuasinfo);
            angular.copy(result.data.dashboardinfo, $scope.dashboardinfo);
            angular.copy(result.data.questioncomments, $scope.qcomments);
            angular.copy(result.data.waterdischargesummary, $scope.waterdischargesummary);
            $("#waterdischargesummary_hid").val(angular.toJson(result.data.waterdischargesummary.chartdata));
            angular.copy(result.data.waterdatasummary, $scope.waterdatasummary);
            $("#waterdatasummary_hid").val(angular.toJson(result.data.waterdatasummary.chartdata));
            angular.copy(result.data.assosiatedmaterialdatasummary, $scope.assosiatedmaterialdatasummary);
            $("#assosiatedmaterialdatasummary_hid").val(angular.toJson(result.data.assosiatedmaterialdatasummary.chartdata));
            angular.copy(result.data.dieaseldatasummary, $scope.dieaseldatasummary);
            $("#dieaseldatasummary_hid").val(angular.toJson(result.data.dieaseldatasummary.chartdata));
            angular.copy(result.data.electrictydatasummary, $scope.electrictydatasummary);
            $("#electrictydatasummary_hid").val(angular.toJson(result.data.electrictydatasummary.chartdata));
            angular.copy(result.data.heatingoildatasummary, $scope.heatingoildatasummary);
            $("#heatingoildatasummary_hid").val(angular.toJson(result.data.heatingoildatasummary.chartdata));
            angular.copy(result.data.materialpackaging, $scope.materialpackaging);
            $("#materialpackaging_hid").val(angular.toJson(result.data.materialpackaging.chartdata));
            angular.copy(result.data.naturalgasdatasummary, $scope.naturalgasdatasummary);
            $("#naturalgasdatasummary_hid").val(angular.toJson(result.data.naturalgasdatasummary.chartdata));
            angular.copy(result.data.rawmaterial, $scope.rawmaterial);
            $("#rawmaterial_hid").val(angular.toJson(result.data.rawmaterial.chartdata));
            angular.copy(result.data.solidwastedatasummary, $scope.solidwastedatasummary);
            $("#solidwastedatasummary_hid").val(angular.toJson(result.data.solidwastedatasummary.chartdata));
            angular.copy(result.data.airemission, $scope.airemission);
            $("#airemission_hid").val(angular.toJson(result.data.airemission.chartdata));
            angular.copy(result.data.spills, $scope.spills);
            $("#spills_hid").val(angular.toJson(result.data.spills.chartdata));
            angular.copy(result.data.activesubstances, $scope.activesubstances);
            $("#activesubstances_hid").val(angular.toJson(result.data.activesubstances.chartdata));
            angular.copy(result.data.activesubstances, $scope.activesubstances);
            $("#antibiotic_hid").val(angular.toJson(result.data.antibiotic.chartdata));
            angular.copy(result.data.antibiotic, $scope.antibiotic);
            // $("#producedunits_hid").val(angular.toJson(result.data.producedunits.chartdata));
            $("#btnchart").trigger("click");
            angular.copy(result.data.reportemissions, $scope.emissionsdata);
            angular.copy(result.data.emissionheatingandnatural, $scope.emissionheatandnatural);
            angular.copy(result.data.emissiondieselupdownstream, $scope.emissiondieselupdown);
            angular.copy(result.data.emissionco2equivalents, $scope.emissionco2equivalents);
            angular.copy(result.data.nitrogenemissions, $scope.nitrogenemissions);
            angular.copy(result.data.tspemissions, $scope.tspemissions);
            angular.copy(result.data.acidifyinggasses, $scope.acidifyinggasses)
            $scope.currentyear = $scope.geninfo != null ? $scope.geninfo.endyear : date.getFullYear();
        }
        console.log(result.data);
    });
    function GetQueryStringParams(sParam) {
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == sParam) {
                return sParameterName[1];
            }
        }
    }
}]);
