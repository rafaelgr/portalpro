/*
* Arrays objects and another things in order to localizate
* KendoUI controls
* es-ES version
*/

// kendo Grid
var ari_command_es_ES = [
    { name: "edit", text: { edit: "Editar", update: "Actualizar", cancel: "Cancelar" } },
    { name: "destroy", text: "Eliminar" }
];

var ari_filterable_es_ES = {
    messages: {
        info: "Opciones:", // sets the text on top of the filter menu
        filter: "Filtrar", // sets the text for the "Filter" button
        clear: "Quitar filtro", // sets the text for the "Clear" button

        // when filtering boolean numbers
        isTrue: "Cierto", // sets the text for "isTrue" radio button
        isFalse: "Falso", // sets the text for "isFalse" radio button

        //changes the text of the "And" and "Or" of the filter menu
        and: "AND",
        or: "OR"
    },
    operators: {
        //filter menu for "string" type columns
        string: {
            eq: "Igual a",
            neq: "No igual a",
            startswith: "Comienza por",
            contains: "Contiene",
            endswith: "Termina con"
        },
        //filter menu for "number" type columns
        number: {
            eq: "Igual a",
            neq: "No igual a",
            gte: "Mayor o igual que",
            gt: "Mayor que",
            lte: "Menor o igual que",
            lt: "Menor que"
        },
        //filter menu for "date" type columns
        date: {
            eq: "Igual a",
            neq: "No igual a",
            gte: "Después o igual que",
            gt: "Después de",
            lte: "Antes o igual que",
            lt: "Antes de"
        },
        //filter menu for foreign key values
        enums: {
            eq: "Igual a",
            neq: "No igual a"
        }
    }
};

var ari_groupable_es_ES = {
    messages: {
        empty: "Arrastre una columna para agrupar"
    }
};

var ari_columnMenu_es_ES = {
    messages: {
        sortAscending: "Ordenar ascendente",
        sortDescending: "Ordenar descendente",
        filter: "Filtrar",
        columns: "Columnas"
    }
};

var ari_pageable_es_ES = {
    messages: {
        display: "{0} - {1} de {2} registros", //{0} is the index of the first record on the page, {1} - index of the last record on the page, {2} is the total amount of records
        empty: "No hay registros",
        page: "Página",
        of: "de {0}", //{0} is total amount of pages
        itemsPerPage: "registros por página",
        first: "Primera página",
        previous: "Página anterior",
        next: "Página siguiente",
        last: "Última página",
        refresh: "Actualizar"
    }
};

