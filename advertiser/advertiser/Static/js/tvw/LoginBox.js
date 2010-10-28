dojo.provide("tvw.LoginBox");

dojo.require("dijit._Widget");
dojo.require("dijit._Templated");
dojo.require("dijit._CssStateMixin");

dojo.declare(
	"tvw.LoginBox",
	[dijit._Widget, dijit._Templated],
	{
		// summary:
		//		A simple GUI for secure login over ajax
	        //              (passes only hashed password to the server)
	    
	    baseClass: "tvwLoginBox",

	    templateString: dojo.cache("dijit", "templates/LoginBox.html"),
	    
	    widgetsInTemplate: true

	}
);
	    
