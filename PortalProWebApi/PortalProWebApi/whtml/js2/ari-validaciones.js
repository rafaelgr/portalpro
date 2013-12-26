// Función de control de NIF
// Retorna: 1 = NIF ok, 2 = CIF ok, 3 = NIE ok, -1 = NIF error, -2 = CIF error, -3 = NIE error, 0 = ??? error
function valida_nif_cif_nie(a) {
    var temp = a.toUpperCase();
    var cadenadni = "TRWAGMYFPDXBNJZSQVHLCKE";

    if (temp != '') {
        //si no tiene un formato valido devuelve error
        if ((!/^[A-Z]{1}[0-9]{7}[A-Z0-9]{1}$/.test(temp) && !/^[T]{1}[A-Z0-9]{8}$/.test(temp)) && !/^[0-9]{8}[A-Z]{1}$/.test(temp)) {
            return 0;
        }

        //comprobacion de NIFs estandar
        if (/^[0-9]{8}[A-Z]{1}$/.test(temp)) {
            posicion = a.substring(8, 0) % 23;
            letra = cadenadni.charAt(posicion);
            var letradni = temp.charAt(8);
            if (letra == letradni) {
                return 1;
            }
            else {
                return -1;
            }
        }


        //algoritmo para comprobacion de codigos tipo CIF
        suma = parseInt(a.charAt(2)) + parseInt(a.charAt(4)) + parseInt(a.charAt(6));

        for (i = 1; i < 8; i += 2) {
            temp1 = 2 * parseInt(a.charAt(i));
            temp1 += '';
            temp1 = temp1.substring(0, 1);
            temp2 = 2 * parseInt(a.charAt(i));
            temp2 += '';
            temp2 = temp2.substring(1, 2);
            if (temp2 == '') {
                temp2 = '0';
            }

            suma += (parseInt(temp1) + parseInt(temp2));
        }
        suma += '';
        n = 10 - parseInt(suma.substring(suma.length - 1, suma.length));

        //comprobacion de NIFs especiales (se calculan como CIFs)
        if (/^[KLM]{1}/.test(temp)) {
            if (a.charAt(8) == String.fromCharCode(64 + n)) {
                return 1;
            }
            else {
                return -1;
            }
        }

        //comprobacion de CIFs
        if (/^[ABCDEFGHJNPQRSUVW]{1}/.test(temp)) {
            temp = n + '';
            if (a.charAt(8) == String.fromCharCode(64 + n) || a.charAt(8) == parseInt(temp.substring(temp.length - 1, temp.length))) {
                return 2;
            }
            else {
                return -2;
            }
        }

        //comprobacion de NIEs
        //T
        if (/^[T]{1}[A-Z0-9]{8}$/.test(temp)) {
            if (a.charAt(8) == /^[T]{1}[A-Z0-9]{8}$/.test(temp)) {
                return 3;
            }
            else {
                return -3;
            }
        }
        //XYZ
        if (/^[XYZ]{1}/.test(temp)) {
            temp = temp.replace('X', '0')
            temp = temp.replace('Y', '1')
            temp = temp.replace('Z', '2')
            pos = temp.substring(0, 8) % 23;

            if (a.toUpperCase().charAt(8) == cadenadni.substring(pos, pos + 1)) {
                return 3;
            }
            else {
                return -3;
            }
        }
    }

    return 0;
}

// Función auxiliar para la validación de NIF
function str_replace(search, position, replace, subject) {
    var f = search, r = replace, s = subject, p = position;
    var ra = r instanceof Array, sa = s instanceof Array, f = [].concat(f), r = [].concat(r), i = (s = [].concat(s)).length;

    while (j = 0, i--) {
        if (s[i]) {
            while (s[p] = s[p].split(f[j]).join(ra ? r[j] || "" : r[0]), ++j in f) { };
        }
    };

    return sa ? s : s[0];
}


function validarIBAN(iban) {
    // comprobamos la longitud de cadena
    if (iban.length != 24) {
        return false;
    }
    // ahora hay que partir en 4 + 20
    var codPais = iban.substring(0, 2);
    var codIBAN = iban.substring(2, 4);
    var codCuenta = iban.substring(4);

    // ahora calculamos el IBAN que debería ser
    // tomamos las dos primeras posiciones y las transformamos a letras
    letra1 = codPais.substring(0, 1);
    letra2 = codPais.substring(1, 2);
    // obtenemos los valores correspondientes (son números de dos cifras)
    num1 = getnumIBAN(letra1);
    num2 = getnumIBAN(letra2);
    // junto con dos ceros montamos una cola para situar al final de la cuenta
    var cuentaFormateada = codCuenta + String(num1) + String(num2) + "00";
    aux = "";
    // este enredo es debido a la longitud del número, lo procesamos en grupos de seis
    var n = 0;
    while (cuentaFormateada != "") {
        if (cuentaFormateada.length >= 6) {
            aux = aux + cuentaFormateada.substring(0, 6);
            cuentaFormateada = cuentaFormateada.substring(6);
        } else {
            aux = aux + cuentaFormateada;
            cuentaFormateada = "";
        }
        n = parseInt(aux);
        n = n % 97;
        aux = String(n);
    }
    var c = 98 - n;
    var cf = String(c);
    if (c < 10) {
        cf = "0" + String(c);
    }
    if (cf != codIBAN) {
        return false;
    }
    return true;
}


function getnumIBAN(letra) {
    ls_letras = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    return ls_letras.search(letra) + 10;
}


function validarCuenta(i_entidad, i_oficina, i_digito, i_cuenta) {
    // VALIDACIÓN DE CUALQUIER LIBRETA DE CUALQUIER ENTIDAD BANCARIA.
    // Funcion recibe como parámetro la entidad, la oficina, 
    // el digito (concatenación del de control entidad-oficina y del de control entidad)
    // y la cuenta. Espera los valores con 0's a la izquierda.
    // Devuelve true o false.
    // NOTAS:
    //	 Formato deseado de los parámetros:
    //	 - i_entidad (4)
    //	 - i_oficina (4)
    //	 - i_digito (2)
    //	 - i_cuenta (10)
    var wtotal, wcociente, wresto;
    if (i_entidad.length != 4) {
        return false;
    }
    if (i_oficina.length != 4) {
        return false;
    }
    if (i_digito.length != 2) {
        return false;
    }
    if (i_cuenta.length != 10) {
        return false;
    }
    wtotal = i_entidad.charAt(0) * 4;
    wtotal += i_entidad.charAt(1) * 8;
    wtotal += i_entidad.charAt(2) * 5;
    wtotal += i_entidad.charAt(3) * 10;
    wtotal += i_oficina.charAt(0) * 9;
    wtotal += i_oficina.charAt(1) * 7;
    wtotal += i_oficina.charAt(2) * 3;
    wtotal += i_oficina.charAt(3) * 6;
    // busco el resto de dividir wtotal entre 11
    wcociente = Math.floor(wtotal / 11);
    wresto = wtotal - (wcociente * 11);
    //
    wtotal = 11 - wresto;
    if (wtotal == 11) {
        wtotal = 0;
    }
    if (wtotal == 10) {
        wtotal = 1;
    }
    if (wtotal != i_digito.charAt(0)) {
        return false;
    }
    //hemos validado la entidad y oficina
    //-----------------------------------
    wtotal = i_cuenta.charAt(0) * 1;
    wtotal += i_cuenta.charAt(1) * 2;
    wtotal += i_cuenta.charAt(2) * 4;
    wtotal += i_cuenta.charAt(3) * 8;
    wtotal += i_cuenta.charAt(4) * 5;
    wtotal += i_cuenta.charAt(5) * 10;
    wtotal += i_cuenta.charAt(6) * 9;
    wtotal += i_cuenta.charAt(7) * 7;
    wtotal += i_cuenta.charAt(8) * 3;
    wtotal += i_cuenta.charAt(9) * 6;

    // busco el resto de dividir wtotal entre 11
    wcociente = Math.floor(wtotal / 11);
    wresto = wtotal - (wcociente * 11);
    //
    wtotal = 11 - wresto;
    if (wtotal == 11) { wtotal = 0; }
    if (wtotal == 10) { wtotal = 1; }

    if (wtotal != i_digito.charAt(1)) {
        //alert(wtotal+' y no '+i_digito.charAt(1));
        return false;
    }
    // hemos validado la cuenta corriente

    return true;
}