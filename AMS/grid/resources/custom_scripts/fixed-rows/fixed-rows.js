oboutGrid.prototype.initializeScrollingSettingsOld = oboutGrid.prototype.initializeScrollingSettings;
oboutGrid.prototype.initializeScrollingSettings = function () {
    this.initializeScrollingSettingsOld();

    var tempThis = this;

    if (typeof (this.GridVerticalScrollerHelper) == 'undefined') {
        this.GridVerticalScrollerHelper = document.createElement('DIV');
        this.GridVerticalScrollerHelper.className = 'ob_gVSHelper';
        this.GridBodyContainer.appendChild(this.GridVerticalScrollerHelper);

        var innerScroller = document.createElement('DIV');
        this.GridVerticalScrollerHelper.appendChild(innerScroller);

        this.GridVerticalScrollerHelper.onscroll = function () { tempThis.synchronizeFixedRows(); };
    }

    window.setTimeout(this.ID + '.initializeFixedRows();', 150);
}

oboutGrid.prototype.executeOnCallbackEventsOld = oboutGrid.prototype.executeOnCallbackEvents;
oboutGrid.prototype.executeOnCallbackEvents = function () {
    this.executeOnCallbackEventsOld();

    this.initializeFixedRows();
}

oboutGrid.prototype.initializeFixedRows = function () {
    this.GridVerticalScrollerHelper.style.height = this.GridBodyContainer.offsetHeight + 'px';
    this.GridVerticalScrollerHelper.firstChild.style.height = this.GridBodyContainer.firstChild.firstChild.offsetHeight + 'px';
    this.GridBodyContainer.firstChild.style.height = this.GridBodyContainer.offsetHeight + 'px';

    this.GridVerticalScrollerHelper.scrollTop = '1';
    
    var rowsContainer = this.GridBodyContainer.firstChild.firstChild.childNodes[1];
    for (var i = 0; i < this.NumberOfFixedRows; i++) {
        rowsContainer.childNodes[i].className += ' ob_gRFixed';
    }
}

oboutGrid.prototype.synchronizeFixedRows = function () {
    var rowsContainer = this.GridBodyContainer.firstChild.firstChild.childNodes[1];
    var rowHeight = rowsContainer.firstChild.offsetHeight;

    var rowsToScroll = Math.ceil(this.NumberOfFixedRows + this.GridVerticalScrollerHelper.scrollTop / rowHeight - 1);

    for (var i = this.NumberOfFixedRows; i < rowsToScroll; i++) {
        if (rowsContainer.childNodes[i] && rowsContainer.childNodes[i].style.display != 'none') {
            rowsContainer.childNodes[i].style.display = 'none';
        }
    }

    for (var i = rowsToScroll; i < rowsContainer.childNodes.length; i++) {
        if (rowsContainer.childNodes[i] && rowsContainer.childNodes[i].style.display != '') {
            rowsContainer.childNodes[i].style.display = '';
        } else {
            break;
        }
    }
}        