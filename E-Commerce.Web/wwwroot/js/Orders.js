var dtble;
$(document).ready(function () {
    console.log("wjh");
    loaddata();
});

function loaddata() {
    dtble = $("#myTable").DataTable({
        ajax: {
            url: "/Admin/Order/GetAllOrders",
            dataSrc: ''

        },
        "columns": [
            { "data": "id" },
            { "data": "user.name" },
            { "data": "user.email" },
            { "data": "orderStatus" },
            { "data": "totalPrice" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a href="/Admin/Order/OrderDetails?OrderId=${data}" class="btn btn-warning">Details</a>
                            
                            `
                }

            }



        ]
    });
}

//function DeleteItem(url) {
//    Swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                type: "DELETE",
//                url: url,
//                success: function (data) {
//                    if (data.success) {
//                        toastr.success(data.message);
//                        dtble.ajax.reload();
//                    }
//                    else {
//                        toastr.error(data.message);
//                    }
//                }
//            })
//        }
//    })
//}

