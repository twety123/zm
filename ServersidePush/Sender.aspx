<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sender.aspx.cs" Inherits="ServersidePush.Sender" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label runat="server" ForeColor="red" Text="请先登录："></asp:Label><br />
            用户名：<asp:TextBox runat="server" ID="tbUserName"></asp:TextBox><asp:Button runat="server" Text="登录" />
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="214783647">
            <Services>
                <asp:ServiceReference Path="~/Dispatcher.asmx" />
            </Services>
        </asp:ScriptManager>
        <script type="text/javascript">
            function waitEvent() {
                
            }

        </script>

    </form>

</body>
</html>
