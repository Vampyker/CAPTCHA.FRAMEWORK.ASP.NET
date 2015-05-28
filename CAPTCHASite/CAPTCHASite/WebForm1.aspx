<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="CAPTCHASite.WebForm1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="uc" Namespace="ASPNET_Captcha" Assembly="ASPNET_Captcha" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="rsv" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <uc:ASPNET_Captcha ID="ucCaptcha" runat="server" Align = "Middle" Color = "#FF0000" Visible = false />
        <rsv:CaptchaControl ID="msCaptcha" runat="server" 
            CaptchaLength="5" CaptchaHeight = "60" CaptchaWidth="200" 
            CaptchaLineNoise="None" CaptchaMinTimeout="5" CaptchaMaxTimeout="240" ForeColor = "#00FFCC"
            BackColor = "White" CaptchaChars="ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" FontColor = "Red" Visible = false />
        <br />
        <asp:TextBox ID="txtCaptcha" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="Submit" />
        <br />
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        =============================================================================================================
        <br />
        <br />
        <asp:Label ID="lblTest" runat = "server" Text="Test Section" />
        <br />
        <asp:Label ID="lbl" runat = "server" Text="How Many Tests?" />        
        <asp:TextBox ID="txtTestLength" runat = "server" Text="" />
        <br />
        <asp:Button ID="btnAutomateTest" runat = "server" Text="Run Automated Test" OnClick="PerformTest"/>
        <br />
        <asp:Button ID="btnResetTracker" runat = "server" Text="Reset Data" OnClick="ResetData" />
        <br />
        <table border = 1>
            <tr>
                <td>
                    <asp:Label ID="lblDescription" runat = "server" Text="Description" />
                </td>
                <td>
                    <asp:Label ID="lblSuccess" runat = "server" Text="Success" />
                </td>
                <td>
                    <asp:Label ID="lblTotal" runat = "server" Text="Total" />
                </td>
                <td>
                    <asp:Label ID="lblRatio" runat = "server" Text="Ratio" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtDescription1" runat = "server" Text="Math Captcha" ReadOnly=true />
                </td>
                <td>
                    <asp:TextBox ID="txtSuccess1" runat = "server" Text="0" ReadOnly=true />
                </td>
                <td>
                    <asp:TextBox ID="txtTotal1" runat = "server" Text="0" ReadOnly=true />
                </td>
                <td>
                    <asp:TextBox ID="txtRatio1" runat = "server" Text="0.5" ReadOnly=true />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtDescription2" runat = "server" Text="Image Captcha" ReadOnly=true />
                </td>
                <td>
                    <asp:TextBox ID="txtSuccess2" runat = "server" Text="0" ReadOnly=true />
                </td>
                <td>
                    <asp:TextBox ID="txtTotal2" runat = "server" Text="0" ReadOnly=true />
                </td>
                <td>
                    <asp:TextBox ID="txtRatio2" runat = "server" Text="0.5" ReadOnly=true />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>


