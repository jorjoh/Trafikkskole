<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="resultater.aspx.cs" Inherits="SkakkjørtTrafikkPrøve.resultater" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>RESULTATER</h1>
        <br />
        <asp:Image ID="Image1" runat="server" ImageUrl="" />
        <br />
        <span style="font-size: 22pt;"><asp:Label ID="Label2" runat="server" Text=""></asp:Label></span>
        <br />
        <br />
        Du svarte feil på følgende spørsmål:
        <br />
        <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server">
            <Columns>
                <asp:BoundField DataField="Text" HeaderText="Spørsmål"/>
                <asp:BoundField DataField="Value" HeaderText="Fasit"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>