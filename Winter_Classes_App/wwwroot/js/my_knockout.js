var ViewModel = function (jobTitle, company, location, salaryFrom, salaryTo, validUntil, description) { 
    this.jobTitle = ko.observable(jobTitle);
    this.company = ko.observable(company);
    this.location = ko.observable(location);
    this.salaryFrom = ko.observable(salaryFrom);
    this.salaryTo = ko.observable(salaryTo);
    this.validUntil = ko.observable(validUntil);
    this.description = ko.observable(description);

    this.jobTitlePreview = ko.computed(function () {
        return this.jobTitle();
    }, this);
    this.locationPreview = ko.computed(function () {
        return this.location();
    }, this);
    this.companyPreview = ko.computed(function () {
        var id = this.company() - 1;
        return document.getElementById('CompanyId').options[id].text;
    }, this);
    this.salaryFromPreview = ko.computed(function () {
        return this.salaryFrom();
    }, this);
    this.salaryToPreview = ko.computed(function () {
        return this.salaryTo();
    }, this);
    this.validUntilPreview = ko.computed(function () {
        return this.validUntil();
    }, this);
    this.descriptionPreview = ko.computed(function () {
        return this.description();
    }, this);
};

ko.applyBindings(new ViewModel("", "1", "", "", "", "", ""));

// helpers
function getSelectedText(elementId) {
    var elt = document.getElementById(elementId);

    if (elt.selectedIndex == -1)
        return null;

    return elt.options[elt.selectedIndex].text;
}


