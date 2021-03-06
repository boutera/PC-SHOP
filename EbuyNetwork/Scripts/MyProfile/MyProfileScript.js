$(document).ready(function () {

});

var ValidateForm = function () {

    var firstName = $('#jfirstname').val();
    var lastName = $('#jlastname').val();
    var password = $('#jpassword').val();
    var password2 = $('#jpassword2').val();
    var address = $('#jaddress').val();
    var CIN = $('#jCIN').val();
    var regx = /[\w]{2,}@[\w]{2,}\.[\w]{2,}/;

    if (firstName == "") {
        BootstrapDialog.alert('Please enter first name');
        return false;
    }
    else if (lastName == "") {
        BootstrapDialog.alert('Please enter last name');
        return false;
    }
    else if (password.length<8) {
        BootstrapDialog.alert('Please enter a login password with more than 8 charactors');
        return false;
    }
    else if (password!=password2) {
        BootstrapDialog.alert('Please re-enter your password properly');
        return false;
    }
    else if (address=="") {
        BootstrapDialog.alert('Please fill your permanent address');
        return false;
    }
    else if (CIN == "") {
        BootstrapDialog.alert('Please fill your CIN');
        return false;
    }
    else if (!$('#jacceptterms').prop("checked")) {
        BootstrapDialog.alert('Please accept our terms and conditions before sign up');
        return false;
    }
    else {
        return true;
    }
};
