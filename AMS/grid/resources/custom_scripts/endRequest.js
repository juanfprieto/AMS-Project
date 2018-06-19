/*
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
function endRequest(sender, e)
{
    if (e.get_error())
    {
        debug.trace("[" + new Date().toLocaleTimeString() + "] An error occurred while processing the request on the server. Please try again later.");
        e.set_errorHandled(true);
    }
}

Sys.Application.notifyScriptLoaded();
*/