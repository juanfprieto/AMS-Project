// geryon style preload emails
function geryonPreloadImage(path)
{
    //list of image that will be preloaded\    
    var preImageLinks = new Array("CloseDown.gif","CloseOut.gif","CloseOver.gif","MaximizeDown.gif","MaximizeOut.gif","MaximizeOver.gif","RestoreDownDown.gif","RestoreDownOut.gif","RestoreDownOver.gif");
    
    //preload action
    for(var i=0;i<preImageLinks.length;i++)
    {
        var img = new Image();
        img.src = path+"/"+preImageLinks[i];
        img.style.display = "none";
        document.body.insertBefore(img,document.body.firstChild);
    }
}
