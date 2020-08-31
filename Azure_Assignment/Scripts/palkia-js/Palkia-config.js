// File input
$(function () {
    bsCustomFileInput.init();
});
// Active Slidebar
$(function () {
    var href = window.location.href;
    $('nav ul li a').each(function (e, i) {
        if (href.indexOf($(this).attr('href')) >= 0) {
            $(this).addClass('active');
        }
    });
});
// Summernote
$(function () {
    $('#summernote').summernote()
    $('#summernote_Specification').summernote()
})

$("input[data-bootstrap-switch]").each(function () {
    $(this).bootstrapSwitch('state', $(this).prop('checked'));
});

$('#txa_display_Specification').summernote('disable');
$('#txa_display_Description').summernote('disable');
$('#txa_display_Blog_Content').summernote('disable');
$('#txa_display_Blog_Comment').summernote('disable');