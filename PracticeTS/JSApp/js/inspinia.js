/**
 * INSPINIA - Responsive Admin Theme
 * 2.5
 *
 * Custom scripts
 */

$(document).ready(function () {


    // Full height of sidebar
    function fix_height() {
        var heightWithoutNavbar = $("body > #wrapper").height() - 61;
        $(".sidebard-panel").css("min-height", heightWithoutNavbar + "px");

        var navbarHeigh = $('nav.navbar-default').height();
        var navbarHeigh1 = $('nav.navbar-default').css("height");
        var wrapperHeigh = $('#wrapper').height();
        var wrapperHeigh1 = $('#page-wrapper').css("height");
        var h1 = $(document).height();
        var h2 = $(window).height();

        var h3 = $(window).innerHeight();
        //h1 = 400;
        //$('#wrapper').css("max-height", h1 + "px")
        //$('#page-wrapper').css("min-height", (h1-61) + "px")
        if(navbarHeigh > wrapperHeigh){
            //$('#page-wrapper').css("min-height", navbarHeigh + "px");
        }

        if(navbarHeigh < wrapperHeigh){
            //$('#page-wrapper').css("min-height", $(window).height()  + "px");
        }

        if ($('body').hasClass('fixed-nav')) {
            if (navbarHeigh > wrapperHeigh) {
                $('#page-wrapper').css("min-height", navbarHeigh - 60 + "px");
            } else {
                $('#page-wrapper').css("min-height", $(window).height() - 60 + "px");
            }
        }

    }

    $(window).bind("load resize scroll", function() {
        if(!$("body").hasClass('body-small')) {
                fix_height();
        }
    })

    // Move right sidebar top after scroll
    $(window).scroll(function(){
        if ($(window).scrollTop() > 0 && !$('body').hasClass('fixed-nav') ) {
            $('#right-sidebar').addClass('sidebar-top');
        } else {
            $('#right-sidebar').removeClass('sidebar-top');
        }
    });


    setTimeout(function(){
        fix_height();
    });

    $(window).bind("load resize", function () {
        if ($(document).width() < 769) {
            $('body').addClass('body-small');
            fix_height();
        } else {
            $('body').removeClass('body-small');
            fix_height();
        }
    })
});

// Minimalize menu when screen is less than 768px
//$(function() {
//    $(window).bind("load resize", function() {
//        if ($(document).width() < 769) {
//            $('body').addClass('body-small');
//        } else {
//            $('body').removeClass('body-small');
//            fix_height();
//        }
//    })
//})
