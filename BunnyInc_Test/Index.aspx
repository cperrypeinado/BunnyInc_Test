<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Index.aspx.vb" Inherits="BunnyInc_Test.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #btnLinkedin {
            height: 29px;
        }
        #lblProfle {
            text-align: center;
        }
        #lblHead {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        &nbsp;<br />
        <br />
        <table style="width:100%;" id="tblIngreso" runat="server">
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: center">Insert your TorreBio ID</td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <input id="txtPersonID" type="text" width="50%" runat="server" /></td>
            </tr>
            <tr>
                <td style="text-align: center">
    
        <input id="btnLinkedin" type="button" value="Merge LinkedIn profile" runat="server"/></td>
            </tr>
        </table>
        <br />
        <table style="width:100%;">
            <tr>
                <td style="text-align: center">
                    <img id="imgFotoPerfil" alt="" src="" runat="server"/></td>
            </tr>
            <tr>
                <td><h1>
                    <div runat="server" id="lblProfle">
                    </div></h1>
                </td>
            </tr>
            <tr>
                <td><h1>
                    <div runat="server" id="lblHead">
                    </div></h1></td>
            </tr>
        </table>
        <br />
        </div>
    </form>
</body>
</html>
