//Check Email
var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
var verifyCallback = function (response) {

    alert(response);
};
var capChaForgotPass;

var onloadCallback = function () {
    try {

        capChaForgotPass = grecaptcha.render('capChaForgotPass', {
            'sitekey': '6LdfXUQUAAAAALJ0txgE-csMnc1l5mHu74P4TsjN',
            'theme': 'light'
        });
       

    } catch (e) {

    }

};
function CheckCapcha(response) {
    var url = '/Account/CheckCapcha';
   
    var check = false;
    $.ajax({
        async: false,
        'url': url,
        'type': 'POST',
        'content-Type': 'application/x-www-form-urlencoded',
        'dataType': 'json',
        'data': {'response': response},
        'success': function (result) {
            check = result.result;
            console.log(check);
        },
        'error': function (xmlHttpRequest, textStatus, errorThrown) {

        }

    });

    return check;
}

$("#btn-fogot").on('click', function (e) {
    e.preventDefault();

    var t = 0;
    var error = "";
    if ($('#txtEmail-fogot').val() === "") {
        error += "- Email do not Empty<br>";
        t++;
    } else {
        if (!emailReg.test($('#txtEmail-fogot').val())) {
            error += "- Please enter a valid email address. <br>";
            t++;
        }
    }
    if (CheckCapcha(grecaptcha.getResponse(capChaForgotPass)) === false) {
        error += "- Please verify that you are not a robot. <br>";
        t++;
    }

    if (t > 0) {
        $('.alert-danger').removeClass("hidden");
        $('.show-alert').html(error);
        grecaptcha.reset(capChaForgotPass);
    }

    if (t === 0) {
        $('.alert-danger').addClass("hidden");
        FogotPass();
    }
});
function FogotPass() {
    var url = '/Account/FogotPass';
    $.ajax({
        'url': url,
        'type': 'POST',
        'content-Type': 'application/x-www-form-urlencoded',
        'dataType': 'json',
        'data': $('form#myformFogotPass').serialize(),
        'success': function (result) {
            console.log(result);
            if (result.result === 1) {
                $('.alert-success').removeClass("hidden");
                $('.show-alert-success').html('- Please check email to get password. <br>');
              

            }
            else if (result.result === 2) {
                $('.alert-success').addClass("hidden");
                $('.alert-danger').removeClass("hidden");
                grecaptcha.reset(capChaForgotPass);
                $('.show-alert').html('- Email not exist.');
            }
            else {
                $('.alert-success').addClass("hidden");
                $('.alert-danger').removeClass("hidden");
                grecaptcha.reset(capChaForgotPass);
                $('.show-alert').html('- Send email failed!.');
            }
        },
        'error': function (xmlHttpRequest, textStatus, errorThrown) {

        }
    });
}
