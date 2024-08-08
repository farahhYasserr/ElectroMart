var dtble;
$(document).ready(function () {
    LoadData();
});

function LoadData() {
    dtble = $('#myTable').DataTable({
        ajax: {
            url: '/Admin/Users/GetAllUsers',
            dataSrc: ''
        },
        columns: [
            { data: 'name' },
            { data: 'email' },
            { data: 'address' },
            { data: 'city' },
            {
                data: null,
                render: function (data, type, row) {
                    let lockUnlockButton = '';
                    const lockoutEndDate = data.lockoutEnd ? new Date(data.lockoutEnd) : null;
                    const now = new Date();

                    if (lockoutEndDate === null || lockoutEndDate < now) {
                        lockUnlockButton = `
                            <button onClick="LockUnLock('${data.id}')" class="btn btn-success btn-sm">
                                <i class="fas fa-lock-open"></i>
                            </button>
                        `;
                    } else {
                        lockUnlockButton = `
                            <button onClick="LockUnLock('${data.id}')" class="btn btn-danger btn-sm">
                                <i class="fas fa-lock"></i>
                            </button>
                        `;
                    }

                    return `
                        ${lockUnlockButton}
                         <a onClick=DeleteItem("/Admin/Users/Delete/${data.id}") class="btn btn-danger btn-sm">Delete</a>
                  `;
                }
            }
        ]
    });
}

function LockUnLock(id) {

    $.ajax({
        url: "/Admin/Users/LockUnlock/" + id,
        type: "GET",
        success: function (data) {

            dtble.ajax.reload();
        },
        error: function (xhr, status, error) {

        }
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

