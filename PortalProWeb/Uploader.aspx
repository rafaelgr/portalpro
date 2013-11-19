<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Uploader.aspx.cs" Inherits="Uploader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <telerik:RadStyleSheetManager id="RadStyleSheetManager1" runat="server" />
        <link href="css/ace.min.css" rel="stylesheet" />
        <link href="css/bootstrap.min.css" rel="stylesheet" />
    </head>
    <body>
        <form id="form1" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                </Scripts>
            </telerik:RadScriptManager>
            <script type="text/javascript">
                //Put your JavaScript code here.
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
                function doNothing() {
                }
                function alertClose() {
                    window.close();
                }
                function alertReturn(fileName, url, path) {
                    //
                    alert("El fichero " + fileName + " se ha cargado correctamente");
                    // The cookie depends on the Form
                    var data = null;
                    var formId = gup("FormId");
                    var fieldId = gup("FieldId");
                    var returnUrl = gup("ReturnUrl");
                    switch (formId)
                    {
                        case "Factura":
                            data = JSON.parse(getCookie("factura"));
                            switch (fieldId) {
                                case "PDF":
                                    data.FacturaPdf.url = url;
                                    data.FacturaPdf.path = path;
                                    break;
                            }
                            setCookie("factura", JSON.stringify(data), 1);
                            returnUrl = "factura.html?caller=loader";
                            break;
                        case "Pedido":
                            data = JSON.parse(getCookie("pedido"));
                            switch (fieldId) {
                                case "PDF":
                                    data.PedidoPdf.url = url;
                                    data.PedidoPdf.path = path;
                                    break;
                            }
                            setCookie("pedido", JSON.stringify(data), 1);
                            returnUrl = "pedido.html?caller=loader";
                            break;
                        case "Proveedor":
                            data = JSON.parse(getCookie("proveedor"));
                            // la carga se realiza en fucnión del tipo, que hay que obtener
                            var tipoId = fieldId.substring(4);
                            for (i = 0; i < data.Documentos.length; i++) {
                                var d = data.Documentos[i];
                                if (d != null) {
                                    if (d.TipoDocumento.TipoDocumentoId == tipoId) {
                                        d.DescargaUrl = url;
                                        d.NomFichero = path;
                                    }
                                }
                            }
                            setCookie("proveedor", JSON.stringify(data), 1);
                            returnUrl = "proveedor.html?caller=loader";
                            break;
                        case "SolicitudProveedor":
                            data = JSON.parse(getCookie("SolicitudProveedor"));
                            // la carga se realiza en fucnión del tipo, que hay que obtener
                            var tipoId = fieldId.substring(4);
                            for (i = 0; i < data.Documentos.length; i++) {
                                var d = data.Documentos[i];
                                if (d != null) {
                                    if (d.TipoDocumento.TipoDocumentoId == tipoId) {
                                        d.DescargaUrl = url;
                                        d.NomFichero = fileName;
                                    }
                                }
                            }
                            setCookie("SolicitudProveedor", JSON.stringify(data), 1);
                            returnUrl = "solicitudproveedorfrm.html?caller=loader";
                            break;
                    }
                    if (data != null) {
                        window.opener.location.replace(returnUrl);
                        window.close();
                    } else {
                        alert("No hay una cookie sobre la que devolver resultados");
                    }
                }
            </script>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            </telerik:RadAjaxManager>
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" Skin="Metro"></telerik:RadSkinManager>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
            <div class="container-fluid">
                <div id="TitleZone" class="row-fluid table-header">
                    <asp:Label ID="lblTitle" runat="server" Text="Cargar ficheros"></asp:Label>
                </div>
                <div class="row-fluid">
                    <table id="TLoader">
                        <tr>
                            <td>
                                <div class="span12">
                                    <asp:Label ID="lblAddtionalInformation" runat="server" Text="Información adicional:"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="span12">
                                    <telerik:RadUpload ID="rdUploader" runat="server" MaxFileInputsCount="1" ControlObjectsVisibility="None" InputSize="100" CssClass="span12">
                                        <Localization Select="Seleccionar..." />
                                    </telerik:RadUpload>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="span12" style="text-align:right;">
                                    <telerik:RadButton ID="btnLoader" runat="server" Text="Cargar Fichero" OnClick="btnLoader_Click"></telerik:RadButton>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="span12">
                                    <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
                                    <telerik:RadProgressArea ID="RadProgressArea1" runat="server" Width="400px"></telerik:RadProgressArea>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </form>
    </body>
</html>
