// وقتی تمام المنت ها لود شد بیا و این فانکشن  رو اجرا کن
$(document).ready(function () {
    $('.Nav-buttoN').click(function () {
        $('.Nav-buttoN').toggleClass('change')
    })
})

$(window).scroll(function () {
    let position = $(this).scrollTop();
    if (position >= 200) {
        //المنتی که اون کلاس رو داره بیا و این کلاس رو بهش اضافه کن
        $('.Nav-menU').addClass('custom-navbar')
    }
    else {
        //المنتی که اون کلاس رو داره بیا و این کلاس رو بهش اضافه کن
        $('.Nav-menU').removeClass('custom-navbar')
    }
})