$(function () {
    $("#slider").slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        prevArrow:
            '<button type="button" class="slick-prev"><span class="fa fa-angle-left"></span></button>',
        nextArrow:
            '<button type="button" class="slick-next"><span class="fa fa-angle-right"></span></button>',
        responsive: [
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });
});
