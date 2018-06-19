oboutGrid.prototype.addColumnSet = function (level, startColumnIndex, endColumnIndex, text) {
    if (typeof (this.GridColumnSetsContainer) == 'undefined') {
        this.GridColumnSetsContainer = document.createElement('DIV');
        this.GridColumnSetsContainer.className = 'ob_gCSCont';

        this.GridMainContainer.appendChild(this.GridColumnSetsContainer);
    }

    if (typeof (this.ColumnSets) == 'undefined') {
        this.ColumnSets = new Array();
    }

    if (typeof (this.ColumnSets[level]) == 'undefined') {
        this.ColumnSets[level] = new Array();

        this.GridHeaderContainer.firstChild.style.marginTop = (level + 1) * 25 + 'px';

        var levelContainer = document.createElement('DIV');
        levelContainer.className = "ob_gCSContLevel";
        levelContainer.style.width = this.GridHeaderContainer.firstChild.firstChild.offsetWidth + 'px';
        this.GridColumnSetsContainer.appendChild(levelContainer);
    }

    var columnSet = document.createElement('DIV');
    columnSet.className = 'ob_gCSet';
    this.GridColumnSetsContainer.childNodes[level].appendChild(columnSet);

    var columnSetContent = document.createElement('DIV');
    columnSetContent.innerHTML = text;
    columnSet.appendChild(columnSetContent);

    var columnSetWidth = 0;
    for (var i = startColumnIndex; i <= endColumnIndex; i++) {
        if (this.ColumnsCollection[i].Visible) {
            columnSetWidth += this.ColumnsCollection[i].Width;
        }
    }

    columnSet.style.width = columnSetWidth + 'px';
    if (endColumnIndex < this.ColumnsCollection.length - 1) {
        var tempLevel = level;
        if (!(level == 0 || this.GridHeaderContainer.firstChild.childNodes[endColumnIndex + 1].style.top)) {
            tempLevel -= 1;
        }

        var newTop = (-1 - tempLevel) * (25);

        this.GridHeaderContainer.firstChild.childNodes[endColumnIndex + 1].style.top = newTop + 'px';
    }

    var columnSetObject = new Object();
    columnSetObject.Level = level;
    columnSetObject.StartColumnIndex = startColumnIndex;
    columnSetObject.EndColumnIndex = endColumnIndex;
    columnSetObject.ColumnSet = columnSet;

    this.ColumnSets[level].push(columnSetObject);
}


oboutGrid.prototype.resizeColumnSets = function () {
    for (var level = 0; level < this.ColumnSets.length; level++) {
        for (var i = 0; i < this.ColumnSets[level].length; i++) {
            var columnSetWidth = 0;
            for (var j = this.ColumnSets[level][i].StartColumnIndex; j <= this.ColumnSets[level][i].EndColumnIndex; j++) {
                if (this.ColumnsCollection[j].Visible) {
                    columnSetWidth += this.ColumnsCollection[j].Width;
                }
            }

            this.ColumnSets[level][i].ColumnSet.style.width = columnSetWidth + 'px';
        }
    }
}

oboutGrid.prototype.resizeColumnOld = oboutGrid.prototype.resizeColumn;
oboutGrid.prototype.resizeColumn = function (columnIndex, amountToResize, keepGridWidth) {
    this.resizeColumnOld(columnIndex, amountToResize, keepGridWidth);

    this.resizeColumnSets();
}

oboutGrid.prototype.synchronizeBodyHorizontalScrollingOld = oboutGrid.prototype.synchronizeBodyHorizontalScrolling;
oboutGrid.prototype.synchronizeBodyHorizontalScrolling = function () {
    this.synchronizeBodyHorizontalScrollingOld();

    this.GridHeaderContainer.firstChild.style.marginLeft = -1 * this.GridBodyContainer.firstChild.scrollLeft + 'px';
    this.GridColumnSetsContainer.scrollLeft = this.GridBodyContainer.firstChild.scrollLeft;
}
