<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.FileSync.FileManager.ascx.cs" Inherits="AMS.FileSync.FileManager" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>
<style>
    div table 
    {
        width:auto;
        margin: inherit;
    }
</style>
<script type="text/javascript">

    function OnProgress(progressBar) {
        var extraData = progressBar.getExtraData();
        if (extraData) {
            var div;
            if (extraData.charAt(0) == '.') {
                div = document.getElementById("divError");
                div.innerHTML += "\n" + extraData;
            }
            else {
                div = document.getElementById("divInfo");
                div.innerHTML = extraData;
            }
        }
    }
    function cleanError() {
    
        div = document.getElementById("divError");
        div.innerHTML = " ";
    }
</script>
<table style="width: 67%;  margin-left: 8%;">
    <tr>
        <td>
            <div >
                <iz:FileManager ID="FileManager1" runat="server" Height="450" Width="800">
                    <RootDirectories>
                        <iz:RootDirectory DirectoryPath="~/Repository/" Text="Repositorio" />
                    </RootDirectories>
                    <FileTypes>
                        <iz:FileType Extensions="jpg" Name="Imagen" SmallImageUrl="../img/Icons/16x16/image.png" LargeImageUrl="../img/Icons/32x32/image.png" />
                        <iz:FileType Extensions="flv" Name="Video" SmallImageUrl="../img/Icons/16x16/video.png" LargeImageUrl="../img/Icons/32x32/video.png" />
                    </FileTypes>
                </iz:FileManager>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnSync" runat="server" Text="Sincronizar Kioskos" OnClientClick="cleanError()" Visible="true" class="noEspera"/>
        </td>
    </tr>
    <tr>
        <td>
            <eo:ProgressBar ID="progressBar" runat="server" StartTaskButton="btnSync" OnRunTask="progressBar_RunTask"
            ShowPercentage="True" ClientSideOnValueChanged="OnProgress" 
            BorderColor="Black" BorderWidth="1px" Width="800px" Height="16px" IndicatorColor="LightBlue" style="  margin: auto;"/>
        </td>
    </tr>
</table>

<div id="divInfo"></div>
<div id="divError" runat="server"></div>
