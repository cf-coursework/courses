﻿'use strict';

const uri = 'api/todo';
let todos = null;

function getCount(data) {
    const elem = $('#counter');
    let name = 'to-do';
    if (data) {
        if (data > 1) {
            name = 'to-dos';
        }
        elem.text(`${data} ${name}`);
    } else {
        elem.text(`No ${name}`);
    }
}

$(document).ready(function () {
    getData();
});

function getData() {
    $.ajax({
        type: 'GET', // In $.ajax() settings object, 'type' is alias for 'method'
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $('#todos');

            $(tBody).empty();

            getCount(data.length);

            $.each(data, function (key, item) { // $.each() is generic iterator function
                const tr = $('<tr></tr>')
                    .append(
                        $('<td></td>').append(
                            $('<input/>', {
                                type: 'checkbox', // Missing quotes here was enough to break my page
                                disabled: true,
                                checked: item.isComplete
                            })
                        )
                    )
                    .append($('<td></td>').text(item.name))
                    .append(
                        $('<td></td>').append(
                            $('<button>Edit</button>').on('click', function () {
                                editItem(item.id);
                            })
                        )
                    )
                    .append(
                        $('<td></td>').append(
                            $('<button>Delete</button>').on('click', function () {
                                deleteItem(item.id);
                            })
                        )
                    );

                tr.appendTo(tBody);
            });

            todos = data;
        }
    });
}

function addItem() {
    const item = {
        name: $('#add-name').val(),
        isComplete: false
    };

    $.ajax({
        type: 'POST',
        url: uri,
        accepts: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Something went wrong!');
        },
        success: function (result) {
            getData();
            $('#add-name').val('');
        }
    });
}

function deleteItem(id) {
    $.ajax({
        url: `${uri}/${id}`,
        type: 'DELETE',
        success: function (result) {
            getData();
        }
    });
}

function editItem(id) {
    $.each(todos, function (key, item) {
        if (item.id === id) {
            $('#edit-name').val(item.name);
            $('#edit-id').val(item.id);
            $('#edit-isComplete')[0].checked = item.isComplete;
        }
    });
    $('#spoiler').css({ display: "block" });
}

$('.my-form').on('submit', function () {
    const item = {
        name: $('#edit-name').val(),
        isComplete: $('#edit-isComplete').is(':checked'),
        id: $('#edit-id').val() // Wrongly added semicolon here was enough to break page
    };

    $.ajax({
        url: `${uri}/${$('#edit-id').val()}`,
        type: 'PUT',
        accepts: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify(item),
        success: function (result) {
            getData();
        }
    });

    closeInput();
    return false;
});

function closeInput() {
    $('#spoiler').css({ display: "none" });
}
