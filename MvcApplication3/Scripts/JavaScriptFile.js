$(document).ready(function () {
    var ErrorBlock = $('body').find('ErrorBlock');
    if ($('body').find('ErrorBlock').attr('data-erroroccured').val == ' True') {
        ErrorBlock.slideToggle();
    } 
});