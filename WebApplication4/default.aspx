<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SkakkjørtTrafikkPrøve._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Velkommen til Skakkjørt Trafikkskole</h1>
        <p class="lead">Teorieksamen</p>
        <p class="lead">Tid: 60min</p>
        <p class="lead">Antall oppgaver: 20</p>
        <p class="lead">Skriv inn brukernavn
            <asp:TextBox ID="txtBoxBrukernavn" runat="server"></asp:TextBox>
        </p>
        <p class="lead">
            <asp:Label ID="feilmelding" runat="server" Text="Label"></asp:Label>

        </p>
        <p>
            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary btn-lg" OnClick="Button1_Click" Text="Start Test &gt;&gt;" Width="156px" />
        </p>
    </div>

</asp:Content>
