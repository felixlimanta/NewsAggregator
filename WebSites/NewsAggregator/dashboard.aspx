<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<base target="dashboard" />
	<title> Dashboard </title>
</head>
<body runat="server" id="bd">
	<br />
	<br />
    <div style="color:white"> Keyword:
    <form runat="server">
        <asp:TextBox ID="key" runat="server"></asp:TextBox>
        <br />
        <br />
        Algoritma:
        <br />
        <asp:RadioButton ID="b" name="b" runat="server" GroupName="Algorithm" checked="true" />
        Boyer-Moore
        <br />
        <asp:RadioButton ID="k" name="k" runat="server" GroupName="Algorithm" />
        KMP
        <br />
        <asp:RadioButton ID="r" name="r" runat="server" GroupName="Algorithm" />
        Regex
        <br />
        <br />
        <asp:Button ID="bttn" name="bttn" Text="Cari Berita" runat="server" OnClick="PatternMatch" />
        <br />
        <br />
        <hr />
        <br />
        <span runat="server" id="OutSpan" name="OutSpan"></span>
    </form>
    <br />
    </div>
</body>
</html>
