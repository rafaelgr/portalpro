/*-------------------------------------------------------------------
ari-general:
Contiene funciones generales que se utilizan en todos las páginas
---------------------------------------------------------------------*/
/*
*   Set and Get Cookies
*   this funtions come from http://www.w3schools.com/js/js_cookies.asp
*   they are used in forms in order to and retrieve
*   field's values in a cookie
*/
function are_cookies_enabled() {
    var cookieEnabled = (navigator.cookieEnabled) ? true : false;
    if (typeof navigator.cookieEnabled == "undefined" && !cookieEnabled) {
        document.cookie = "testcookie";
        cookieEnabled = (document.cookie.indexOf("testcookie") != -1) ? true : false;
    }
    return (cookieEnabled);
}

function setCookie(c_name, value, exdays) {
    if (!are_cookies_enabled()) {
        alert("NO COOKIES");
    }
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}

function deleteCookie(c_name) {
    document.cookie = encodeURIComponent(c_name) + "=deleted; expires=" + new Date(0).toUTCString();
}

function ari_formatErrorMessage(msg) {
    var s = "<h5 class='text-error'>" + msg.Message + "</h5>";
    if (msg.ExceptionMessage != undefined) {
        s += "<p class='text-warning'>" + msg.ExceptionMessage + "</p>";
    }
    return s;
}


function generalAjaxErrorTreatment() {
    // tratamiento común de AJAX
    $.ajaxSetup({
        error: function (xhr, textStatus, errorThrwon) {
            console.log("Error AJAX\n" + xhr.responseText);
            // tratar el error producido
            var message = "[" + xhr.status + "] " + xhr.statusText;
            // controlar si está devolviendo una página web, un objeto JSON o XML
            if (xhr.responseText != "") {
                try {
                    message = JSON.parse(xhr.responseText);
                    message = ari_formatErrorMessage(message);
                } catch (err) {
                    if (typeof(xhr.responseText) != "undefined")
                        message = xhr.responseText;
                }
            }
            bootbox.dialog(message, [
                {
                    "label": "OK",
                    "class": "btn-small btn-success",
                    "callback": function () {
                    }
                }
            ]);
        }
    });
}

function loadTopBar() {
    // load menu-superior
    $.ajax({
        type: 'GET',
        url: "topbar.html",
        dataType: 'html',
        success: function (html, textStatus) {
            $("#topbar").html(html);
        },
    });
}
function loadTopBarProveedor() {
    // load menu-superior
    $.ajax({
        type: 'GET',
        url: "topbar2.html",
        dataType: 'html',
        success: function (html, textStatus) {
            $("#topbar").html(html);
        },
    });
}
function loadSideBar() {
    // load menu-lateral
    $.ajax({
        type: 'GET',
        url: "sidebar.html",
        dataType: 'html',
        success: function (html, textStatus) {
            $("#sidebar").html(html);
            //ace.js (hay que activar la funcionalidad del menu
            handle_side_menu();
        },
    });
}
function loadSideBarProveedor() {
    // load menu-lateral
    $.ajax({
        type: 'GET',
        url: "sidebar2.html",
        dataType: 'html',
        success: function (html, textStatus) {
            $("#sidebar").html(html);
            //ace.js (hay que activar la funcionalidad del menu
            handle_side_menu();
        },
    });
}


function checkAutorization() {
    // leemos si hay una cookie con el tique
    var tk = getCookie("ari_tique");
    // sin no la hay directamente al login
    if (typeof tk == "undefined") {
        console.log("Tique no definido");
        window.open("login.html", '_self');
    }
    // si la hay verificamos si sigue activa
    // solicitamos el tique a la API
    $.ajax({
        type: "PUT",
        url: ari_hosts.webapi + "/api/Login?tk=" + tk,
        dataType: "json",
        contentType: "application/json",
        success: function (data, textStatus) {
            // si sigue activa la renovamos y guardamos el tique en la cookie
            var tique = data;
            setCookie("ari_tique", tique.Codigo, 1);
            setCookie("ari_usuario", JSON.stringify(data.Usuario), 1);
            $("#login-name").text(data.Usuario.Nombre);
            console.log("Tique renovado");
        },
        error: function (xhr, textStatus, errorThrwon) {
            // tratar el error producido
            var message = "[" + xhr.status + "] " + xhr.statusText;
            // controlar si está devolviendo una página web, un objeto JSON o XML
            if (xhr.responseText != "") {
                try {
                    message = JSON.parse(xhr.responseText);
                    message = ari_formatErrorMessage(message);
                } catch (err) {
                    if (typeof (xhr.responseText) != "undefined")
                        message = xhr.responseText;
                }
            }
            bootbox.dialog(message, [
                {
                    "label": "OK",
                    "class": "btn-small btn-success",
                    "callback": function () {
                        window.open("login.html", '_self');
                    }
                }
            ]);
        }
    });
}

function checkAutorizationProveedor() {
    // leemos si hay una cookie con el tique
    var tk = getCookie("ari_tique");
    // sin no la hay directamente al login
    if (typeof tk == "undefined") {
        console.log("Tique no definido");
        window.open("login.html", '_self');
    }
    // si la hay verificamos si sigue activa
    // solicitamos el tique a la API
    $.ajax({
        type: "PUT",
        url: ari_hosts.webapi + "/api/LoginProveedor?tk=" + tk,
        dataType: "json",
        contentType: "application/json",
        success: function (data, textStatus) {
            // si sigue activa la renovamos y guardamos el tique en la cookie
            var tique = data;
            setCookie("ari_tique", tique.Codigo, 1);
            setCookie("ari_usuario", JSON.stringify(data.UsuarioProveedor), 1);
            $("#login-name").text(data.UsuarioProveedor.Nombre);
            console.log("Tique renovado");
        },
        error: function (xhr, textStatus, errorThrwon) {
            // tratar el error producido
            var message = "[" + xhr.status + "] " + xhr.statusText;
            // controlar si está devolviendo una página web, un objeto JSON o XML
            if (xhr.responseText != "") {
                try {
                    message = JSON.parse(xhr.responseText);
                    message = ari_formatErrorMessage(message);
                } catch (err) {
                    if (typeof (xhr.responseText) != "undefined")
                        message = xhr.responseText;
                }
            }
            bootbox.dialog(message, [
                {
                    "label": "OK",
                    "class": "btn-small btn-success",
                    "callback": function () {
                        window.open("login.html", '_self');
                    }
                }
            ]);
        }
    });
}

// gup stands from Get Url Parameters
function gup(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}

// searchInArry(array, id)
// It searches inside the "array" of Json object
// elements that match the "id" attribute
function searchInArray(array, id) {
    var result = $.grep(array, function (e) {
        return e.id == id;
    });
    return result;
}


function moniker() {
    alert("MONIKER");
}

