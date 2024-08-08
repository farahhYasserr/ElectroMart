var dtble;
$(document).ready(function () {

    loaddata();
    $('#imgfile').change(function (event) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#ImagePreview').attr('src', e.target.result);
        }
        if (event.target.files[0]) {
            reader.readAsDataURL(event.target.files[0]);
        }
    });
});

function loaddata() {
    dtble = $("#myTable").DataTable({
        "ajax": {
            url: "/Admin/Product/GetAllProducts",
            dataSrc: ''
        },
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <a href="/Admin/Product/Edit/${data.id}" class="btn btn-success btn-sm">Edit</a>
                        <a onClick=DeleteItem("/Admin/Product/Delete/${data.id}") class="btn btn-danger btn-sm">Delete</a>
                    `;
                }
            }
        ],

    });
}

function DeleteItem(URL) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: URL,
                type: "Delete",
                success: function (data) {
                    dtble.ajax.reload()
                }
            });
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });

        }
    });
}