// account.js


function new_onload()
{
    $('account_name').focus();
}

function create()
{
    name = $('account')['input_name'].getValue();
    email = $('account')['input_email'].getValue();
    password = $('account')['input_password'].getValue();
}