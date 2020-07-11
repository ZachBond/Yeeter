<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Exercise06.aspx.cs" Inherits="WebApp.Exercises.Exercise06" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Exercise 6</h1>
    

    <div class="row">
        <div class="col-md-6">
            <asp:Label runat="server" Text="Enter a Team ID:"></asp:Label>&nbsp;&nbsp;
            <asp:TextBox id="IDArg" runat="server"></asp:TextBox>&nbsp;&nbsp;
            <asp:Button id="Fetch" runat="server" Text="Fetch" OnClick="Fetch_Click" />
            <asp:Label id="MessageLabel" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <asp:Label runat="server" Text="Team ID:"></asp:Label>&nbsp;&nbsp;
            <asp:Label id="TeamID" runat="server" ></asp:Label>
            <br />
            <asp:Label runat="server" Text="Team Name:"></asp:Label>&nbsp;&nbsp;
            <asp:Label id="TeamName" runat="server" ></asp:Label>
        </div>
    </div>
</asp:Content>
