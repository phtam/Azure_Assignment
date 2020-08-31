// validation username
$(document).ready(function () {
    $("#txtUsername_In_Manage").keyup(function () {
        $.ajax({
            type: "POST",
            url: '/Values/CheckUsername',
            data: {
                username: $(this).val()
            },
            cache: false,
            datatype: "json",
        }).done(function (data) {
            $("#notice_is_valid_username").empty();
            $("#notice_is_valid_username").append(data);
        });
    });
});
// validation password
$(document).ready(function () {
    $("#txtPassword_In_Manage").keyup(function () {
        $.ajax({
            type: "POST",
            url: '/Values/CheckPassword',
            data: {
                password: $(this).val()
            },
            cache: false,
            datatype: "json",
        }).done(function (data) {
            $("#notice_is_valid_password").empty();
            $("#notice_is_valid_password").append(data);
        });
    });
});
// validation email
$(document).ready(function () {
    $("#txtEmail_In_Manage").keyup(function () {
        $.ajax({
            type: "POST",
            url: '/Values/CheckEmail',
            data: {
                emailAddress: $(this).val()
            },
            cache: false,
            datatype: "json",
        }).done(function (data) {
            $("#notice_is_valid_email").empty();
            $("#notice_is_valid_email").append(data);
        });
    });
});