var GridsLoadedOnPage = new Array();
var GridsLoadedOnPageRowOffsets = null;

oboutGrid.prototype.initRowDragging = function (record) {
    this.PreviousMouseMoveEventHandler = document.onmousemove;
    this.PreviousMouseUpEventHandler = document.onmouseup;
    this.PreviousOnSelectStartEventHandler = document.onselectstart;

    var tempThis = this;

    if (this.DraggingRowsContainer == null) {
        this.DraggingRowsContainer = document.createElement('DIV');
        this.DraggingRowsContainer.style.display = 'none';
        this.DraggingRowsContainer.className = 'grid-rows-dragging-container';
        document.body.appendChild(this.DraggingRowsContainer);

        var table = document.createElement('TABLE');
        table.className = this.GridBodyContainer.firstChild.firstChild.className;
        var tbody = document.createElement('TBODY');
        table.appendChild(tbody);
        this.DraggingRowsContainer.appendChild(table);

        this.createRowDraggingDestinationIndicator();
    }

    this.DraggingRowsStarted = false;
    this.RowDraggingDestinationRowIndex = -1;
    this.RowDraggingDestinationOffsetIndex = -1;

    document.onselectstart = function () { return false; };
    document.onmousemove = function (e) { tempThis.performRowDragging(e); }
    document.onmouseup = function (e) { tempThis.endRowDragging(e); return false; }

    return false;
}

oboutGrid.prototype.performRowDragging = function (event) {
    var body = this.getBodyTableBody();

    if (!this.DraggingRowsStarted) {
        var draggingBody = this.DraggingRowsContainer.firstChild.firstChild;
        while (draggingBody.firstChild) {
            draggingBody.removeChild(draggingBody.firstChild);
        }
        for (var i = 0; i < this.SelectedRecordsCollection.length; i++) {
            this.DraggingRowsContainer.firstChild.firstChild.appendChild(this.SelectedRecordsCollection[i].Record.cloneNode(true));
        }

        GridsLoadedOnPageRowOffsets = new Array();

        for (var j = 0; j < GridsLoadedOnPage.length; j++) {
            var gridBody = GridsLoadedOnPage[j].getBodyTableBody();

            for (var i = 0; i < gridBody.childNodes.length; i++) {
                var offsets = new Object();
                offsets.x = this.getLeft(gridBody.childNodes[i]);
                offsets.y = this.getTop(gridBody.childNodes[i]);
                offsets.width = GridsLoadedOnPage[j].GridBodyContainer.offsetWidth;
                offsets.height = gridBody.childNodes[i].offsetHeight;
                offsets.grid = GridsLoadedOnPage[j];
                offsets.rowIndex = i;

                GridsLoadedOnPageRowOffsets.push(offsets);
            }
        }

        this.DraggingRowsStarted = true;
    }

    if (this.PageSelectedRecords.length > 0) {
        this.DraggingRowsContainer.style.display = '';
    } else {
        this.DraggingRowsContainer.style.display = 'none';
    }

    var mouseCoords = this.getMouseCoords(event);
    var mouseX = mouseCoords[0] - 7;
    var mouseY = mouseCoords[1] - 7;

    this.DraggingRowsContainer.style.left = mouseX + 'px';
    this.DraggingRowsContainer.style.top = mouseY + 'px';

    this.RowDraggingDestinationIndicator.style.display = 'none';

    this.RowDraggingDestinationOffsetIndex = -1;
    this.RowDraggingDestinationRowIndex = -1;

    for (var i = 0; i < GridsLoadedOnPageRowOffsets.length; i++) {
        if (GridsLoadedOnPageRowOffsets[i].x < mouseX && GridsLoadedOnPageRowOffsets[i].x + GridsLoadedOnPageRowOffsets[i].width > mouseX &&
            GridsLoadedOnPageRowOffsets[i].y < mouseY && GridsLoadedOnPageRowOffsets[i].y + GridsLoadedOnPageRowOffsets[i].height > mouseY
            ) {

            this.RowDraggingDestinationIndicator.style.display = '';
            this.RowDraggingDestinationIndicator.style.left = GridsLoadedOnPageRowOffsets[i].x + 'px';
            this.RowDraggingDestinationIndicator.style.width = GridsLoadedOnPageRowOffsets[i].width + 'px';

            this.RowDraggingDestinationOffsetIndex = i;

            if (mouseY - GridsLoadedOnPageRowOffsets[i].y < GridsLoadedOnPageRowOffsets[i].height / 2) {
                this.RowDraggingDestinationRowIndex = GridsLoadedOnPageRowOffsets[i].rowIndex;
                this.RowDraggingDestinationIndicator.style.top = GridsLoadedOnPageRowOffsets[i].y - this.RowDraggingDestinationIndicator.offsetHeight + 'px';
            } else {
                this.RowDraggingDestinationRowIndex = GridsLoadedOnPageRowOffsets[i].rowIndex + 1;
                this.RowDraggingDestinationIndicator.style.top = GridsLoadedOnPageRowOffsets[i].y + GridsLoadedOnPageRowOffsets[i].height - this.RowDraggingDestinationIndicator.offsetHeight + 'px';
            }

            break;
        }
    }

}

oboutGrid.prototype.endRowDragging = function (event) {
    if (this.RowDraggingDestinationOffsetIndex != -1) {

        var body = GridsLoadedOnPageRowOffsets[this.RowDraggingDestinationOffsetIndex].grid.getBodyTableBody();
        var destinationSibling = body.childNodes[this.RowDraggingDestinationRowIndex];

        if (body.childNodes.length == 1 && body.firstChild.firstChild.className == 'ob_gNRM') {
            body.removeChild(body.firstChild);
            destinationSibling = null;
        }

        var recordsToMove = new Array();
        for (var i = 0; i < this.SelectedRecordsCollection.length; i++) {
            recordsToMove.push(this.SelectedRecordsCollection[i].Record);
        }

        if (GridsLoadedOnPageRowOffsets[this.RowDraggingDestinationOffsetIndex].grid != this) {
            for (var i = 0; i < recordsToMove.length; i++) {
                this.deselectRecord(this.getElementIndex(recordsToMove[i]));
            }
        }

        for (var i = 0; i < recordsToMove.length; i++) {
            if (destinationSibling) {
                body.insertBefore(recordsToMove[i], destinationSibling);
            } else {
                body.appendChild(recordsToMove[i]);
            }
        }

        this.updateRowsCssClasses();

        if (GridsLoadedOnPageRowOffsets[this.RowDraggingDestinationOffsetIndex].grid != this) {
            this.assignBodyEvents();
            GridsLoadedOnPageRowOffsets[this.RowDraggingDestinationOffsetIndex].grid.assignBodyEvents();
            GridsLoadedOnPageRowOffsets[this.RowDraggingDestinationOffsetIndex].grid.updateRowsCssClasses();

        }
    }

    document.onmousemove = this.PreviousMouseMoveEventHandler;
    document.onmouseup = this.PreviousMouseUpEventHandler;
    document.onselectstart = this.PreviousOnSelectStartEventHandler;

    this.DraggingRowsContainer.style.display = 'none';
    this.DraggingRowsStarted = false;

    this.getClientSideModel();
    if (this.RowDraggingDestinationOffsetIndex != -1) {
        GridsLoadedOnPageRowOffsets[this.RowDraggingDestinationOffsetIndex].grid.getClientSideModel();
    }

    if (this.RowDraggingDestinationOffsetIndex != -1 && typeof (this.OnClientRowsDropped) != 'undefined') {
        this.OnClientRowsDropped(this, GridsLoadedOnPageRowOffsets[this.RowDraggingDestinationOffsetIndex].grid);
    }

    if (this.Rows.length == 0) {
        this.refresh();
    }

    this.RowDraggingDestinationIndicator.style.display = 'none';
    this.RowDraggingDestinationRowIndex = -1;
    this.RowDraggingDestinationOffsetIndex = -1;
}

oboutGrid.prototype.createRowDraggingDestinationIndicator = function () {
    this.RowDraggingDestinationIndicator = document.createElement('DIV');
    this.RowDraggingDestinationIndicator.className = 'grid-rows-dragging-indicator';
    this.RowDraggingDestinationIndicator.style.display = 'none';
    document.body.appendChild(this.RowDraggingDestinationIndicator);

}

oboutGrid.prototype.updateRowsCssClasses = function () {
    var body = this.getBodyTableBody();
    for (var i = 0; i < body.childNodes.length; i++) {
        if (body.childNodes[i].className != 'ob_gRS') {
            body.childNodes[i].className = (i % 2 == 0) ? 'ob_gR' : 'ob_gRA';
        } else {
            for (var j = 0; j < this.SelectedRecordsCollection.length; j++) {
                if (body.childNodes[i] == this.SelectedRecordsCollection[j].Record) {
                    this.SelectedRecordsCollection[j].PreviousClassName = (i % 2 == 0) ? 'ob_gR' : 'ob_gRA';
                }
            }
        }
    }
}

oboutGrid.prototype.addGridToPageGrids = function () {
    var add = true;
    for (var i in GridsLoadedOnPage) {
        if (GridsLoadedOnPage[i] == this) {
            add = false;
        }
    }

    if (add) {
        GridsLoadedOnPage.push(this);
    }
}

oboutGrid.prototype.assignBodyEvents = function () {
    this.addGridToPageGrids();

    var oTempThis = this;

    this.TempGridBody = this.getBodyTableBody(); this.RemovableDOMObjects.push(['TempGridBody', false]);

    var bHoverEffectsAttached = false;
    if (this.AllowRecordSelection == true) {
        for (var i = 0; i < this.TempGridBody.childNodes.length; i++) {
            if (this.isBodyRecord(this.TempGridBody.childNodes[i])) {
                this.TempGridBody.childNodes[i].onmousedown = function (e) {
                    if (this.className.indexOf('ob_gRS') == -1) {
                        if (!e) e = window.event;
                        if (!e.ctrlKey && !e.shiftKey) {
                            oTempThis.restorePreviousSelectedRecord();
                        }
                        oTempThis.selectRecordByClick(e, this);
                        oTempThis.rowClicked = this;
                    } else {
                        oTempThis.rowClicked = null;
                    }

                    return oTempThis.initRowDragging();
                }

                this.TempGridBody.childNodes[i].onclick = function (e) {
                    if (this.className.indexOf('ob_gRS') != -1 && oTempThis.rowClicked != this) {
                        oTempThis.selectRecordByClick(e, this);
                    }
                }

                if (this.EnableRecordHover == true) {
                    this.TempGridBody.childNodes[i].onmouseover = function () { oTempThis.manageHoverEffects(this, true); };
                    this.TempGridBody.childNodes[i].onmouseout = function () { oTempThis.manageHoverEffects(this, false); };
                    bHoverEffectsAttached = true;
                }
            }
        }

        // stopping event bubbling / propagation for the Edit/Delete columns
        for (var i = 0; i < this.ColumnsCollection.length; i++) {
            var stopEventBubbling = false;
            if (this.ColumnsCollection[i].AllowEdit == true || this.ColumnsCollection[i].AllowDelete == true) {
                stopEventBubbling = true;
            } else if (this.ColumnsCollection[i].ColumnType == 'CheckBoxSelectColumn') {
                stopEventBubbling = true;
            }

            if (stopEventBubbling) {
                var cellIndex = i;
                for (var j = 0; j < this.TempGridBody.childNodes.length; j++) {
                    if (this.isBodyRecord(this.TempGridBody.childNodes[j]) && this.TempGridBody.childNodes[j].childNodes[cellIndex]) {
                        this.TempGridBodyCell = this.TempGridBody.childNodes[j].childNodes[cellIndex];

                        if (this.ColumnsCollection[cellIndex].ColumnType == 'CheckBoxSelectColumn') {
                            this.TempGridBodyCell = this.getCheckBoxSelector(this.TempGridBodyCell);
                        } else {
                            this.stopAllEventsPropagation(this.TempGridBodyCell);
                        }

                        if (this.ColumnsCollection[cellIndex].ColumnType == 'CheckBoxSelectColumn') {
                            if (this.TempGridBodyCell.type == 'checkbox') {
                                this.stopAllEventsPropagation(this.TempGridBodyCell);
                                this.TempGridBodyCell.onclick = function (e) { oTempThis.toggleRecordSelection(this, e); };
                            } else {
                                var checkBox = eval(this.TempGridBodyCell.value);
                                this.stopAllEventsPropagation(this.TempGridBodyCell.nextSibling, true);
                                checkBox.Container.onclick = function (e) { var checkBox = eval(this.previousSibling.value); checkBox.handleClick(); oTempThis.stopEventPropagation(e); };
                                checkBox.ClientSideEvents.OnCheckedChanged = function (sender) { oTempThis.toggleRecordSelection(sender) };
                            }
                        }
                    }
                }
            }
        }
    }

    if (this.AllowGrouping == true) {

    }

    if (this.EnableRecordHover == true && bHoverEffectsAttached == false) {
        for (var i = 0; i < this.TempGridBody.childNodes.length; i++) {
            this.TempGridBody.childNodes[i].onmouseover = function () { oTempThis.manageHoverEffects(this, true); };
            this.TempGridBody.childNodes[i].onmouseout = function () { oTempThis.manageHoverEffects(this, false); };
        }
    }
}