/**
 * index.js
 * - All our useful JS goes here, awesome!
 */
(function ($) {

    var a = 0;
    $('#btn-sidebars').on('click', function (e) {
        e.preventDefault();
        if (a == 0) {
            a = 1;
            $('#content').removeClass('content')
            $('.main-sidebar').removeClass('show-sidebar').addClass('hide-sidebar');
        } else if (a == 1) {
            a = 0;
            $('#content').addClass('content')
            $('.main-sidebar').removeClass('hide-sidebar').addClass('show-sidebar');
        }
    });

}(jQuery)) 



function checkScroll() {
    var stickyDiv = document.getElementById('myStickyDiv');
    var row = document.querySelector('.layout-sticky');
    var scrollPosition = window.scrollY;
    var targetPosition = row.offsetTop;

    if (scrollPosition >= targetPosition) {
        stickyDiv.classList.add('sticky');
        stickyDiv.style.top = '150px';  
        stickyDiv.style.right = '8px';
    } else {
        stickyDiv.classList.remove('sticky');
        stickyDiv.style.top = '150px';
        stickyDiv.style.right = '8px';
    }
}

window.addEventListener('scroll', checkScroll);