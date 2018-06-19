var app = angular.module("amsApp", []);

//<helpinfo info=''></helpinfo>
//Muestra un icono de pregunta que despliega un ToolTip para ayudas complementarias.
app.directive("helpinfo", function () {
    return {
        restrict: "E",
        scope: {
            info: "@info"
        },
        template: "<a href='#' title='{{info}}' class='tooltip'><img src='../img/AMS.Help2.png' title='' class='lupa'/></a>"
    }
});      