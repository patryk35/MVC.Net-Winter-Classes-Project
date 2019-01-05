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
