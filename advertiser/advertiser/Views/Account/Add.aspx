<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<script> src="http://o.aolcdn.com/dojo/1.5/dojo/dojo.xd.js" type="text/javascript"></script>
	<title>Add Account</title>	
</head>
<body>
	<div>
		<h1> <%= Html.Encode(  ViewData["Result"] ) %> </h1>
	</div>
</body>
<html>
