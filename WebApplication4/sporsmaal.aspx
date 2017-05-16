<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="sporsmaal.aspx.cs" Inherits="SkakkjørtTrafikkPrøve.test" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
            <h1>
                <asp:Label ID="spmText" runat="server"></asp:Label>
            </h1>
            <asp:Label ID="sporsmaalText" runat="server"></asp:Label>
            <asp:Image id="Image1" runat="server" style="float: right; width: 300px; height: 300px; padding: 10px;" ImageAlign="Right"/>
            &nbsp;<br />
            <asp:RadioButton ID="RadioButton1" runat="server" GroupName="1" Checked="False" OnCheckedChanged="RadioButton1_CheckedChanged" /> <asp:Literal ID="Radio1" runat="server"></asp:Literal>
            <br />
            <asp:RadioButton ID="RadioButton2" runat="server" GroupName="1" Checked="False" OnCheckedChanged="RadioButton2_CheckedChanged" /> <asp:Literal ID="Radio2" runat="server"></asp:Literal>
            <br />
            <asp:RadioButton ID="RadioButton3" runat="server" GroupName="1" Checked="False" OnCheckedChanged="RadioButton3_CheckedChanged" /> <asp:Literal ID="Radio3" runat="server"></asp:Literal>
            <br />
            <asp:Label ID="feilmelding" runat="server" Visible="False"></asp:Label>
            &nbsp;<asp:Label ID="Label3" runat="server" Visible="False"></asp:Label>
            <br />
            <asp:Button ID="btnSvar" runat="server" Text="Svar" OnClick="Button1_Click" />
            &nbsp;<asp:Button ID="btnFortsett" runat="server" Width="234px" OnClick="Button2_Click" />
        </div>
</asp:Content>
