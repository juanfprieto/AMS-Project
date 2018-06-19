/* author: metroid */
/* Creation date: 19/09/2003 */
//ventanas POP-UP


function OpenMainWin()
{

	window.open("AMS.Web.index.aspx?", "AMS", "width=screen.width,height=screen.height,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=yes,history=no");

}


function OpenWinStatGraph(element, mode, type, xDatas, yDatas)
{

	window.open("AMS.Web.Stats.aspx?element=" + element + "&mode=" + mode + "&type=" + type + "&xDatas=" + xDatas + "&yDatas=" + yDatas + "", "StatGraph", "width=640,height=330,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=yes,history=no");

}