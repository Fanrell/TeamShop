var i = 1;
$().ready(function () {
    $("#close-cookie-warn").click(function () {
            var e = new Date;
            e.setTime((new Date).getTime() + 31536e6),
                document.cookie = "cookiesok=1;expires=" + e,
                $("#cookie-warn").hide("slow")
        }),
        -1 === ("; " + document.cookie + ";").indexOf("; " + escape("cookiesok") + "=") && $("#cookie-warn").show(),
        $("#btn1").click(function () {
            $("#form1").show(),
                $("#header").hide()
        }),
        $("#btn2").click(function () {
            $("#other").show(), $("#header").hide()
        })


    if ($(window).width() > 1024) {

        $(function () {


            $(document).scroll(function () {
                var $nav = $(".navbar-fixed-top");
                var $header = $("#header");


                $nav.toggleClass('scrolled2', $(this).scrollTop() > $header.height());



            });


        });
    }



});
