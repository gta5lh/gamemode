var loginActive = true;

//смена фона
$(function () {
    var transTime = 5000;
    setInterval(function () {
        if ($('.bg-grad.active').index() == ($('.bg-grad').length - 1)) {
            $('.bg-grad.active').removeClass('active');
            $('.bg-grad').eq(0).addClass('active');
        }
        else {
            var curIndex = $('.bg-grad.active').index();
            $('.bg-grad.active').removeClass('active');
            $('.bg-grad').eq(curIndex + 1).addClass('active');
        }
    }, transTime);
});

$(document).keypress(function (e) {
    if (e.which == 13) {
        if (loginActive) {
            document.getElementById("login_btn").click();
        } else {
            document.getElementById("register_btn").click();
        }
    }
});

$("#login_btn").click(function () {
    $('#login_email_error').html("");
    $('#login_password_error').html("");

    let loginRequest = {
        email: $("#login_email").val(),
        password: $("#login_password").val(),
    };

    mp.trigger("LoginSubmitted", JSON.stringify(loginRequest));
});

$("#register_btn").click(function () {
    $('#register_email_error').html("");
    $('#register_username_error').html("");
    $('#register_password_error').html("");
    $('#register_confirm_password_error').html("");

    let registerRequest = {
        email: $("#register_email").val(),
        username: $("#register_username").val(),
        password: $("#register_password").val(),
        confirmPassword: $("#register_confirm_password").val(),
    };

    mp.trigger("RegisterSubmitted", JSON.stringify(registerRequest));
});

$(".info-item .btn").click(function () {
    $(".container").toggleClass("log-in");
    loginActive = !loginActive;
});

function logIn() {
    $('#login_email_error').html("");
    $('#register_email_error').html("");
    $(".container").addClass("active");
}

function loginFailed(invalidFieldNames) {
    invalidFieldNames.forEach((item, index) => {
        switch (item) {
            case "email":
                $('#login_email_error').html("Неверный email");
                break;

            case "password":
                $('#login_password_error').html("Неверный пароль");
                break;

            case "banned":
                $('#login_email_error').html("Ваш аккаунт заблокирован администратором");
                break;

            case "already_online":
                $('#login_email_error').html("Игрок под вашим аккаунтом уже авторизован");
                break;
        }
    });
}

function registerFailed(invalidFieldNames) {
    invalidFieldNames.forEach((item, index) => {
        switch (item) {
            case "email":
                $('#register_email_error').html("Введите существующий email");
                break;

            case "username":
                $('#register_username_error').html("Никнейм должен содержать <br>минимум 1 символ, максимум 32");
                break;

            case "password":
                $('#register_password_error').html("Пароль должен содержать <br>минимум 6, максимум 50 символов");
                break;

            case "confirm_password":
                $('#register_confirm_password_error').html("Пароли должны совпадать");
                break;

            case "email_already_exists":
                $('#register_email_error').html("Email уже занят");
                break;

            case "username_already_exists":
                $('#register_username_error').html("Никнейм уже занят");
                break;

            case "internal_server_error":
                $('#register_email_error').html("Что-то пошло не так, попробуйте еще раз");
                break;
        }
    });
}

function wait() {
    if (loginActive) {
        $('#login_email_error').html("Подождите перед повторной попыткой");
    } else {
        $('#register_email_error').html("Подождите перед повторной попыткой");
    }
}
