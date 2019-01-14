
$(document).one('ready', function () {
    GetGroup();
    GetCategory();
    //GetItem();
    //GetLength();
    GetDataForMenu();
    /*GetDINCode();
    GetDimension();
    GetInterFace();
    */
    // InitSearchValue();
    //$('.translation-links a').click(function () {
        
    //    var lang = $(this).data('lang');
    //    //alert(lang);
    //    var $frame = $('.goog-te-menu-frame:first');
        
    //    if (!$frame.size()>0) {
            
    //        alert("Error: Could not find Google translate frame.");
    //        return false;
    //    }
    //    $frame.contents().find('.goog-te-menu2-item div span.text').get(lang).click();
    //    //$frame.contents().find('.goog-te-menu2-item span.text:contains(' + lang + ')').get(1).click();
    //    return false;
    //});
});
function showpopup(code,name,picture,price)
{
    var html = "";
    html = "<table><tr><td> <img src='/Content/ProductImage/"+ picture+"' style='width:100%'>" + "</td></tr><tr><td>" + name + "</td></tr><tr><td>$ " + price + "</td></tr></table>";

    jQuery.facebox(html);
}

function linkto(e)
{
    // var id = $(e).attr('id');
    var activeSlide = $(e).find('.viewSlide.active')//.hasClass('active');

    var imageCode = $(activeSlide).find('img').attr('id');
   // let activeView = $('div.viewSlide.active');
    //alert(imageCode);
    window.location = "/Home/Product?Code=" + imageCode;
}
function checkOcp(input) {
    //var text = input.html()
        /*if(str1.indexOf(str2) != -1){
    if(input.html)*/
    //alert(input);
    //alert(input.attr('id'));
    alert(input.html());
}

function addValueToCheckbox()
{

    $('.allqty').each(function () { //iterate all listed checkbox items
        myFunc($(this));
    });
    //alert(this.attr("data"));
}
function myFunc(input)
{
    alert(input.val());
}
$("#chkbox_all").change(function () {
    //$("#" + id).is(":checked")
    //if ($(this).attr('checked')) {
    if ($("#chkbox_all").is(":checked"))
    {
        $('.chkbox').each(function () { //iterate all listed checkbox items
            this.checked = "checked"; //change ".checkbox" checked status
        });
    } else {
        $('.chkbox').each(function () { //iterate all listed checkbox items
            this.checked = ""; //change ".checkbox" checked status
        });
    }
});
$("#Size_Of_Page").change(function () {
    //$("#productfom").submit();
    window.location.href = window.location.pathname + "?Page_No=1&Page_Size=" + this.value;
});

$("#Size_Of_PageCart").change(function () {
    window.location.href = window.location.pathname + "?Page_No=1&Page_Size=" + this.value;
});
$("#qtydown").click(function () {
    var iNum = parseInt($("#qty").val());
    if (iNum == 1) {

    }
    else
    {
        iNum--;
    }
    $("#qty").val(iNum);
});
function bulkitemup(i)
{
    $("#" + i).val();
    var iNum = parseInt($("#" + i).val());
    
    iNum++;
    $("#" + i).val(iNum);
}
function bulkitemdown(i)
{
    $("#" + i).val();
    var iNum = parseInt($("#" + i).val());
    if (iNum == 1) {

    }
    else {
        iNum--;
    }
    $("#" + i).val(iNum);
}
$("#qtyup").click(function () {
    var iNum = parseInt($("#qty").val());
    iNum++;
    $("#qty").val(iNum);
});
function InitSearchValue()
{
    if ($("#vddlPosition").val() != "")
    {
        alert($("#vddlPosition").val());
        //$("#ddlPosition").val($("#vddlPosition").val());
        $("#ddlPosition > [value=" + $("#vddlPosition").val() + "]").attr("selected", "true")
    }
    if ($("#vddlGetGroup").val() != "") {
        //$("#ddlGetGroup").val($("#vddlGetGroup").val());
        $("#ddlGetGroup > [value=" + $("#vddlGetGroup").val() + "]").attr("selected", "true")
    }
    if ($("#vddlGetCategory").val() != "") {
       // $("#ddlGetCategory").val($("#ddlGetCategory").val());
        $("#ddlGetCategory > [value=" + $("#ddlGetCategory").val() + "]").attr("selected", "true")
    }
}
function GetGroup() {
    var procemessage = "<option value='0'> Please wait...</option>";
    $("#ddlGetGroup").html(procemessage).show();
    var url = "/Home/GetGroup/";

    $.ajax({
        url: url,
        data: { /*LinhVucId: $('#LinhVuc').val()*/ },
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select Group</option>";
            for (var x = 0; x < data.length; x++) {
                if ($("#vddlGetGroup").val() == data[x].Value) {
                    markup += "<option value=" + data[x].Value + " selected>" + data[x].Text + "</option>";
                }
                else
                {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                
            }
            $("#ddlGetGroup").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function GetDataForMenu() {
    var procemessage = "<div value='0'> Loading Menu...</div>";
    $("#menu").html(procemessage).show();
    //var url = "/Home/GetMetaGroup/";
    var url = "/Home/GetMasterGroup/";
    $.ajax({
        url: url,
        data: {/* NganhId: $('#ddlNganh').val()*/ },
        cache: false,
        type: "POST",
        success: function (data) {
            console.log(data);
            var markup = "";
            for (var x = 0; x < data.length; x++) {
                markup += "<div id=cat" + data[x].Value + " class='submenu1' onClick=getSubmenu1(" + data[x].Value + ") style='background-color:#235d91;color: white;margin: 10px 0px;padding: 9px;font-size:18px;font-weight: bold;'>" + data[x].Text + "<img src='../Content/menu/down-arrow.png' class='menunavidown' style='float:right;display:none;' id='down" + data[x].Value + "' /><img src='../Content/menu/vertical-arrow.png' class='menunaviver' style='float:right;' id='ver" + data[x].Value + "' /></div>";
            }
            $("#menu").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });

}
function getSubmenu1(cat)
{
    var catId = "cat"+cat;
    $("#submenu1").remove();
    if ($("#isMenu1Open").val() == 'yes') {
        $("#isMenu1Open").val('no');
        $(".menunaviver").css({ 'display': '' });
        $(".menunavidown").css({ 'display': 'none' });
        return;
    }
    else
    {
        $("#isMenu1Open").val('yes');
    }
    /*$("#submenu2").remove();*/
    $("#" + catId).after("<div id='submenu1' style='height: 200px; overflow-y: scroll;max-height:fix-content;'></div>");

    $(".menunaviver").css({ 'display': '' });
    $(".menunavidown").css({ 'display': 'none' });

    $("#down" + cat).css({ 'display': '' });
    $("#ver" + cat).css({ 'display': 'none' });


    var markup = "";
    var procemessage = "<div value='0'> Please wait...</div>";
    $("#submenu1").html(procemessage).show();
    var url = "/Home/GetCatalogue/";

    $.ajax({
        url: url,
        data: { metagroupId: cat },
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "";
            for (var x = 0; x < data.length; x++) {
                markup += "<div id=cate" + data[x].Value + " class='submenu1' onClick=getSubmenu2(" + data[x].Value + ") style='background-color:#eeeeee;color: #0f497d;margin: 2px 0px;padding: 9px;font-size:18px;font-weight: bold;'>" + data[x].Text + "<img src='../Content/menu/down-arrow2.png' class='menunavidown1' style='float:right;display:none;' id='down1" + data[x].Value + "' /><img src='../Content/menu/vertical-arrow2.png' class='menunaviver1' style='float:right;' id='ver1" + data[x].Value + "' /></div>";
            }
            $("#submenu1").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function getSubmenu2(cat) {
    var catId = "cate" + cat;
    $("#submenu2").remove();
    if ($("#isMenu2Open").val() == 'yes') {
        $("#isMenu2Open").val('no');
        $(".menunaviver1").css({ 'display': '' });
        $(".menunavidown1").css({ 'display': 'none' });
        return;
    }
    else {
        $("#isMenu2Open").val('yes');
    }
    $("#" + catId).after("<div id='submenu2' style='height: 200px; max-height:fix-content;'></div>");

    $(".menunaviver1").css({ 'display': '' });
    $(".menunavidown1").css({ 'display': 'none' });

    $("#down1" + cat).css({ 'display': '' });
    $("#ver1" + cat).css({ 'display': 'none' });


    var markup = "";
    var procemessage = "<div value='0'> Please wait...</div>";
    $("#submenu2").html(procemessage).show();
    var url = "/Home/GetCategoryByCatId/";

    $.ajax({
        url: url,
        data: { CatId: cat },
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "";
            for (var x = 0; x < data.length; x++) {
                markup += "<a href='http://shop.janere.ee/Home/Products?categoryid=" + data[x].Value + "'><div id=cate" + data[x].Value + " class='submenu2' style='color:#666666;margin: 2px 0px;padding: 9px;font-size:18px;font-weight: bold;'>" + data[x].Text + "</div></a>";
            }
            $("#submenu2").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function GetCategory() {
    var procemessage = "<option value='0'> Please wait...</option>";
    $("#ddlGetCategory").html(procemessage).show();
    var url = "/Home/GetCategory/";

    $.ajax({
        url: url,
        data: {/* NganhId: $('#ddlNganh').val()*/ },
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select Category</option>";
            for (var x = 0; x < data.length; x++) {
                if ($("#vddlGetCategory").val() == data[x].Value) {
                    markup += "<option value=" + data[x].Value + " selected>" + data[x].Text + "</option>";
                }
                else {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
            }
            $("#ddlGetCategory").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });

}
function GetItem() {
    var procemessage = "<option value='0'> Please wait...</option>";
    $("#ddlGetItem").html(procemessage).show();
    var url = "/Home/GetItem/";

    $.ajax({
        url: url,
        data: {/* MonId: $('#ddlMon').val() */},
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select GetItem</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#ddlGetItem").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });

}
function GetDINCode() {
    var procemessage = "<option value='0'> Please wait...</option>";
    $("#ddlGetDINCode").html(procemessage).show();
    var url = "/Home/GetDINCode/";

    $.ajax({
        url: url,
        data: {/* ChuDeLonId: $('#ddlChuDeLon').val()*/},
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select DINCode</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#ddlGetDINCode").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function GetDimension() {
    var procemessage = "<option value='0'> Please wait...</option>";
    $("#ddlGetDimension").html(procemessage).show();
    var url = "/Home/GetDememsion/";

    $.ajax({
        url: url,
        data: {/* ChuDeLonId: $('#ddlChuDeLon').val()*/},
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select Dememsion</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#ddlGetDimension").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function GetLength() {
    var procemessage = "<option value='0'> Please wait...</option>";
    $("#ddlLength").html(procemessage).show();
    var url = "/Home/GetLength/";

    $.ajax({
        url: url,
        data: {/* ChuDeLonId: $('#ddlChuDeLon').val()*/},
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select Length</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#ddlLength").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function GetInterFace() {
    var procemessage = "<option value='0'> Please wait...</option>";
    $("#ddlGetInterFace").html(procemessage).show();
    var url = "/Home/GetInterFace/";

    $.ajax({
        url: url,
        data: {/* ChuDeLonId: $('#ddlChuDeLon').val() */},
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select InterFace</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#ddlGetInterFace").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function GetMaterial() {
    var procemessage = "<option value='0'> Please wait...</option>";
    $("#ddlGetMaterial").html(procemessage).show();
    var url = "/Home/GetMaterial/";

    $.ajax({
        url: url,
        data: {/* ChuDeLonId: $('#ddlChuDeLon').val()*/ },
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select Material</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#ddlGetMaterial").html(markup).show();
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}

/*$("#blink").blink({
    speedIn: 500,
    speedOut: 300,
    viewTime: 6000
});
01
$("#blink").blink({
    items: true,
    navigation: true,
    prevText: '◄ Prev',
    nextText: 'Next ►'
});*/

/** DROPDOWNMENU*
jQuery(document).ready(function () {
    $(".dropdown").hover(
        function () {
            $('.dropdown-menu', this).fadeIn("fast");
        },
        function () {
            $('.dropdown-menu', this).fadeOut("fast");
        });
});*/
// mobile menu
$('.mobile_menu').click(function(){
    $('.wrap-head-main-menu').slideToggle();
});
// table in mobile
if($(window).width()<480){
    $('table').wrap('<div class="wrap_table"></div>')
}


function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

$(document).on('click', '.btn-language', function () {
    var lang = $(this).attr('data-language');
    setCookie(lang, lang, 365);
    GetLanguage(lang);
});
$(function () {
    //load lan

    var vi = getCookie("English");
  

    if (vi != null) {

        GetLanguage(vi);
    }
    else {
        setCookie("English", "English", 365);
        GetLanguage("English");
    }
   





});

function GetLanguage(lang) {
    var url = "/Home/GetLanguage";
    $.ajax
   ({
       type: "POST",
       url: url, //LyricsloadMore
       data: JSON.stringify({ lan: lang }),
       dataType: "json",
       contentType: "application/json;charset=utf-8",
       success: function (data) {
           console.log(data);
           $.each(data, function (i, o) {
               if (o.code.lastIndexOf("ip_") != -1)
                   $('.' + o.code).val(o.name);
               else if (o.code.lastIndexOf("pl_") != -1)
                   $('.' + o.code).attr("placeholder", o.name);
               else
                   $('.' + o.code).html(o.name);

           });


       }
   });
}
