var dtble;
$(document).ready(function () {
    LoadData();
});

function LoadData() {
    dtble = $('#myTable').DataTable({
        ajax: {
            url: '/Admin/Category/GetAllCategories',
            dataSrc: ''
        },
        columns: [
            { data: 'name' },
            { data: 'description' },
            { data: 'createdDate' },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <a href="/Admin/Category/Edit/${data.id}" class="btn btn-success btn-sm">Edit</a>
                        <a onClick=DeleteItem("/Admin/Category/Delete/${data.id}") class="btn btn-danger btn-sm">Delete</a>
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