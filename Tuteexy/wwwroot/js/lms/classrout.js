﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "language": {
            "lengthMenu": "Display _MENU_ records per page",
            "zeroRecords": "Nothing found - sorry",
            "info": "Showing page _PAGE_ of _PAGES_",
            "infoEmpty": "No records available",
            "infoFiltered": "(filtered from _MAX_ total records)"
        },
        "autoWidth": false,
        "pageLength": 100,
        "ordering": false,
        "scrollX": true,
        "ajax": {
            "url": "/Lms/ClassRout/GetAll"
        },
        "columns": [
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div>
                                <a class="a-pointer" href="/Lms/ClassRout/Upsert/${data}" data-toggle="tooltip" data-placement="top" title="Edit">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a class="a-pointer" onclick=Delete("/Lms/ClassRout/Delete/${data}") data-toggle="tooltip" data-placement="top" title="Delete">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                },
            },
            { "data": "classname" },
            { "data": "day" },
            { "data": "p1" },
            { "data": "p2" },
            { "data": "p3" },
            { "data": "p4" },
            { "data": "p5" },
            { "data": "p6" },
            { "data": "p7" },
            { "data": "p8" },
            { "data": "p9" },
            { "data": "p10" }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}