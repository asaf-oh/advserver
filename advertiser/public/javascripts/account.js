// account.js

function account_create()
{
    name = $('new_account')['account_name'].getValue();
    email = $('new_account')['account_email'].getValue();
    password = $('new_account')['account_password'].getValue();
   
    new Ajax.Request("accounts/create", {
	method: 'post',
	onSuccess: function(transport) {
	    alert(transport);
	}
    });
 
}

function new_onload()
{
    $('account_name').focus();
}

