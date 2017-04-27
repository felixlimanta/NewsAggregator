<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NewsAggregator._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        .jumbotron {
            background-color: lightgray;
        }
    </style>

    <div class="jumbotron">
        <h1>One Aggregator</h1>
        <p class="lead">One Aggregator is a news aggregator by TVOne Group designed to help readers find news through number of sources collected into one place.</p>
    </div>

    <div style="background-image: url(TubesStima3BG.jpg);">
        Keyword:
        <asp:TextBox ID="key" runat="server"></asp:TextBox>
        <br />
        <br />
        Algorithm:
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
        <asp:Button ID="bttn" name="bttn" Text="Search" runat="server" OnClick="PatternMatch" class="btn btn-default" />
        <br />
        <hr />
        <span runat="server" id="OutSpan" name="OutSpan"></span>
    </div>

</asp:Content>
