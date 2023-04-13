var dataTale;

$(document).ready (function ()
{
    loadDataTable();
});

function  loadDataTable()
{
    dataTale = $("#tblData").DataTable({
        "ajax":{
            "url":"https://localhost:7151/Admin/Document/GetAll"
        },
        "columns":[
            {"data": "name","width":"15%"},
            {"data": "type","width":"15%"},
            {"data": "creationDate","width":"15%"},
            {"data": "staff","width":"15%"},
            {
                "data" : "id",
                "render": function (data){
                    return`
       
                <div class="w-75 btn-group" role="group">
                    <a href="/Admin/Document/Upsert?id=${data}"><i class="bi bi-pencil"></i> Edit</a>
                    <a onClick=Delete("/Admin/Document/Delete/${data}")
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
				
                            </div>

                    `
                },width: "15%"
            }
        ]
    });
}
function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
