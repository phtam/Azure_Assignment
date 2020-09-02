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
$('#xyz').summernote('disable');

function CallPrint(strid) {
    var prtContent = document.getElementById(strid);
    var WinPrint = window.open('', '', 'letf=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
    WinPrint.document.write(prtContent.innerHTML);
    WinPrint.document.close();
    WinPrint.focus();
    WinPrint.print();
    WinPrint.close();
    prtContent.innerHTML = strOldOne;
}

