//Check Email
var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
var verifyCallback = function (response) {

    alert(response);
};
var capChaForgotPass, capchaAskForPrice;

var onloadCallback = function () {
    try {

        capChaForgotPass = grecaptcha.render('capChaForgotPass', {
            'sitekey': '6LdfXUQUAAAAALJ0txgE-csMnc1l5mHu74P4TsjN',
            'theme': 'light'
        });
        capchaAskForPrice = grecaptcha.render('capchaAskForPrice', {
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


$('.btnSenAskPrice').on('click', function () {
   

    var t = 0;
    var error = "";
    if ($('#name').val() === "") {
        error += "- Name do not Empty<br>";
        t++;
    }
    if ($('#phone').val() === "") {
        error += "- Phone do not Empty<br>";
        t++;
    }
    if ($('#qty').val() === "") {
        error += "- Quantity do not Empty<br>";
        t++;
    }
    if ($('#email').val() === "") {
        error += "- Email do not Empty<br>";
        t++;
    } else {
        if (!emailReg.test($('#email').val())) {
            error += "- Please enter a valid email address. <br>";
            t++;
        }
    }
    //if (CheckCapcha(grecaptcha.getResponse(capchaAskForPrice)) === false) {
    //    error += "- Please verify that you are not a robot. <br>";
    //    t++;
    //}

    if (t > 0) {
        $('.alert-danger').removeClass("hidden");
        $('.show-alert').html(error);
        //grecaptcha.reset(capchaAskForPrice);
    }

    if (t === 0) {
        $('.alert-danger').addClass("hidden");
        SendAskForPrice();
    }
});
function SendAskForPrice() {
    var url = '/Account/SendAskForPrice';
    $.ajax({
        'url': url,
        'type': 'POST',
        'content-Type': 'application/x-www-form-urlencoded',
        'dataType': 'json',
        'data': $('form#formAskForPrice').serialize(),
        'success': function (result) {
            console.log(result);
            if (result.result === 1) {
                $('.alert-success').removeClass("hidden");
                $('.show-alert-success').html('- Email Sended. <br>');


            }
            else {
                $('.alert-success').addClass("hidden");
                $('.alert-danger').removeClass("hidden");
                //grecaptcha.reset(capchaAskForPrice);
                $('.show-alert').html('- Send email failed!.');
            }
        },
        'error': function (xmlHttpRequest, textStatus, errorThrown) {

        }
    });
}
$(".checkNumber").keydown(function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A, Command+A
        (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40) || (e.keyCode == 231)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
});