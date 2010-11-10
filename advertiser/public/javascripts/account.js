// account.js

function account_create()
{
    name = $('new_account')['account_name'].getValue();
    email = $('new_account')['account_email'].getValue();
    password = $('new_account')['account_password'].getValue();
    
    var account = {'name': name, 'email' : email, 'password' : password.md5() };       
   
    new Ajax.Request("accounts", {
	method: 'post',
	parameters: account ,
	onSuccess: function(transport) {
	    alert(transport);
	}
    });
 
}

function new_onload()
{
    $('account_name').focus();
}

