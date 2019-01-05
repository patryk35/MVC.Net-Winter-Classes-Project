let searchString = null;
function reload(api_uri, search = "") {
    var currentPage = 1;
    searchString = search
    fetchData(1);
    $('#updatePanel').on('click', '.footerContent a', function (e) {
        e.preventDefault();
        pageNo = parseInt($(this).html());
        currentPage = pageNo;
        searchString = search;
        fetchData(currentPage);
    });

    function fetchData(pageNo) {
        var $loading = "<div class='loading'>Please wait...</div>";
        $('#updatePanel').prepend($loading);
        $.ajax({
            url: api_uri,
            type: 'GET',
            data: { pageNo: pageNo, searchString: searchString },
            dataType: 'json',
            success: function (data) {
                var $table = $('<table/>').addClass('table table-striped table-hover');
                var $header = $('<thead/>').addClass('thead-dark').html('<tr><th scope = "col">Name</th ><th scope="col">Options</th></tr>');
                $table.append($header);
                $.each(data.pagingModel, function (i, emp) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(emp.name));
                    $row.append($('<td/>').html("<a href='/Companies/Edit/" + emp.id + "'>Edit</a> | <a href ='/Companies/Delete/" + emp.id + "'>Delete</a>"));
                    $table.append($row);
                });

                var totalPage = parseInt(data.totalPage);
                var $footer = $('<tr/>');
                var $footerTD = $('<td/>').attr('colspan', 8).addClass('footerContent');

                if (totalPage > 0) {
                    for (var i = 1; i <= totalPage; i++) {
                        var $page = $('<span/>').addClass((i == currentPage) ? "current" : "");
                        $page.html((i == currentPage) ? i : "<a href='#'>" + i + "</a>");
                        $footerTD.append($page);
                    }
                    $footer.append($footerTD);
                }
                $table.append($footer);

                $('#updatePanel').html($table);
            },
            error: function () {
                alert('Error! Please try again.');
            }
        }).done(function () {

            // $loading.remove();
        });
    }
}



var Company = {
    Name: ""
}



/*$('#submitButton').click(function (e) {
    e.preventDefault();
    onAddOffer($(this));

})*/


function genValidationMessage(obj) {
    var message = "";
    if (obj != null) {
        for (i = 0; i < obj.length; i++) {
            message += obj[i];
            message += " ";
        }
    } else {
        message = "";
    }
    return message;
}

/*Functions for AJAX POST*/
function onAddCompany(item) {
    var form = $('#formAdd');
    var formData = $(form).serialize();
    var options = {};
    options.url = "/api/CompanyApi";
    options.type = "POST";
    var obj = Company;
    options.data = formData;
    console.log(options.data)
    console.log(options)

    options.success = function (msg) {
        alert(msg.responseText);
       window.location.replace("http://jobofferspmdevelop.azurewebsites.net/Companies");
    },
        options.error = function (msg) {
            obj = JSON.parse(msg.responseText);
            document.getElementById('nameValidation').textContent = genValidationMessage(obj.Name)

        };
    $.ajax(options);
}
