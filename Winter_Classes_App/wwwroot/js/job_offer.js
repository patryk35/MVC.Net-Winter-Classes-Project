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
                var $header = $('<thead/>').addClass('thead-dark').html('<tr><th scope = "col">Job Title</th ><th scope="col">Company</th><th scope="col">Location</th><th scope="col">Valid Until</th></tr>');
                $table.append($header);
                $.each(data.pagingModel, function (i, emp) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html("<a href='/JobOffers/Details/" + emp.id + "'>" + emp.jobTitle + "</a>"));
                    $row.append($('<td/>').html(emp.company.name));
                    $row.append($('<td/>').html(emp.location));
                    $row.append($('<td/>').html(emp.validUntil));
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

var JobOffer = {
    CompanyId: 0,
    JobTitle: "",
    Location: "",
    SalaryFrom: "",
    SalaryTo: "",
    ValidUntil: "",
    Description: ""
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

/*Function for AJAX POST*/
function onAddOffer(item) {
    var form = $('#formAdd');
    var formData = $(form).serialize();

    var options = {};
    options.url = "/api/JobOffersApi";
    options.type = "POST";
    options.data = formData;
   //options.contentType = "application/json";
    //options.dataType = "html";

    options.success = function (msg) {
        window.location.replace("http://jobofferspmdevelop.azurewebsites.net/JobOffers");
    },
    options.error = function (msg) {
        obj = JSON.parse(msg.responseText);
        document.getElementById('decriptionValidation').textContent = genValidationMessage(obj.Description)
        document.getElementById('jobTitleValidation').textContent = genValidationMessage(obj.JobTitle)
        document.getElementById('companyIdValidation').textContent = genValidationMessage(obj.CompanyId)
        document.getElementById('locationValidation').textContent = genValidationMessage(obj.Location)
        document.getElementById('salaryFromValidation').textContent = genValidationMessage(obj.SalaryFrom)
        document.getElementById('salaryToValidation').textContent = genValidationMessage(obj.SalaryTo)
        document.getElementById('validUntilValidation').textContent = genValidationMessage(obj.ValidUntil)

    };
    $.ajax(options);
}