

Public Class BusinessRprtEntityGridView
    Inherits SolarHouseDataGridView

    Private nxtSelRow As Integer = -1
    Protected matchingRows As New List(Of Integer)

    Protected Function areRowsEqual(r1 As DataRow, r2 As DataRow) As Boolean
        If r1.ItemArray.Count <> r2.ItemArray.Count Then
            Return False
        End If
        For ri As Integer = 0 To r1.ItemArray.Count - 1
            If Not colsEqual(r1.ItemArray(ri), r2.ItemArray(ri)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Protected Function colsEqual(col1 As Object, col2 As Object) As Boolean
        If UIUtil.isEmpty(col1) AndAlso UIUtil.isEmpty(col2) Then
            Return True
        ElseIf UIUtil.isEmpty(col1) AndAlso Not UIUtil.isEmpty(col2) Then
            Return False
        ElseIf Not UIUtil.isEmpty(col1) AndAlso UIUtil.isEmpty(col2) Then
            Return False
        Else
            Return col1.Equals(col2)
        End If
    End Function

    Protected Function isAlreadyInOrg(orgdt As DataTable, row As DataRow) As Boolean
        For r = 0 To orgdt.Rows.Count - 1
            If areRowsEqual(orgdt.Rows(r), row) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function validate() As Boolean
        Dim isFndErrors = False
        Dim dt As DataTable = Me.DataSource

        For r As Integer = 0 To Rows.Count - 1
            If dt.Rows(r).RowState = DataRowState.Added OrElse dt.Rows(r).RowState = DataRowState.Added Then
                If Not validateRow(r) Then
                    isFndErrors = True
                End If
            End If
        Next

        Return Not isFndErrors
    End Function

    Protected Function isNewChanges(dt1 As DataTable, dt2 As DataTable)
        dt1 = If(Not IsNothing(dt1) AndAlso dt1.Rows.Count <= 0, Nothing, dt1)
        dt2 = If(Not IsNothing(dt2) AndAlso dt2.Rows.Count <= 0, Nothing, dt2)
        Return (IsNothing(dt1) And Not IsNothing(dt2)) OrElse (Not IsNothing(dt1) And IsNothing(dt2)) OrElse (Not IsNothing(dt1) AndAlso (dt1.Rows.Count <> dt2.Rows.Count))
    End Function

    Public Function isModified() As Boolean
        Dim dlg As BusinessReportEntityDlg = getBusinessReportEntityDlg()
        Dim orgdt As DataTable = dlg.getDataSet()

        Dim curChanges As DataTable
        Dim orgChanges As DataTable

        curChanges = Me.DataSource.GetChanges(DataRowState.Added)
        orgChanges = orgdt.GetChanges(DataRowState.Added)
        If isNewChanges(curChanges, orgChanges) Then
            Return True
        Else
            If (Not IsNothing(curChanges)) Then
                For r As Integer = 0 To curChanges.Rows.Count - 1
                    If Not isAlreadyInOrg(orgChanges, curChanges.Rows(r)) Then
                        Return True
                    End If
                Next
            End If
        End If
        curChanges = Me.DataSource.GetChanges(DataRowState.Deleted)
        orgChanges = orgdt.GetChanges(DataRowState.Deleted)
        If isNewChanges(curChanges, orgChanges) Then
            Return True
        End If

        curChanges = Me.DataSource.GetChanges(DataRowState.Modified)
        If (Not IsNothing(curChanges)) Then
            For r As Integer = 0 To curChanges.Rows.Count - 1
                If Not isAlreadyInOrg(orgdt, curChanges.Rows(r)) Then
                    Return True
                End If
            Next
        End If

        Return False
    End Function

    Public Overridable Sub setupGridView()
        MyBase.setupGridView()
        ' Sort(Columns(0), System.ComponentModel.ListSortDirection.Ascending)
    End Sub

    Public Function rowHasColErrors(row As Integer)
        Dim c As Integer
        For c = 0 To Me.ColumnCount
            If (Not StringUtil.isEmpty(Me.Rows(row).Cells(c).ErrorText)) Then
                rowHasColErrors = True
                Exit For
            End If
        Next
        rowHasColErrors = False
    End Function

    Public Function selectCurrentRow()
        If SelectedRows.Count > 1 Then
            MessageBox.Show(Me, "More than 1 row selected.  Select the row to return")
            Return False
        ElseIf SelectedRows.Count <= 0 Then
            MessageBox.Show(Me, "Nothing selected.  Select the row to return")
            Return False
        End If

        Dim dlg As BusinessReportEntityDlg = CType(Parent, BusinessReportEntityDlg)
        dlg.codeSelected = SelectedRows(0).Cells("code").Value
        dlg.nameSelected = SelectedRows(0).Cells("entity_name").Value

        dlg.isEntitySelected = True

        Return True
    End Function

    Protected Function locateGridRow(code As String) As Integer
        For r = 0 To Me.RowCount - 1
            If (Rows(r).Cells("code").Value = code) Then
                Return r
            End If
        Next
        Return -1
    End Function

    Protected Function isUnique(val As String, colName As String)
        Dim rows As Integer = Me.RowCount
        Dim r As Integer
        val = UCase(val)
        Dim count As Integer = 0
        For r = 0 To Me.RowCount - 1
            If (Not StringUtil.isEmpty(Me.Rows(r).Cells(colName).Value)) Then
                If (UCase(Me.Rows(r).Cells(colName).Value) = val) Then
                    count += 1
                End If
                If (count > 1) Then
                    Exit For
                End If
            End If
        Next
        isUnique = (count <= 1)
    End Function

    Protected Overrides Sub doValidateRow(row As Integer, ByRef result As RowValidationResult)
        If StringUtil.isEmpty(Rows(row).Cells("code").Value) Then
            addError(result, "Code cannot be empty", row, Rows(row).Cells("code").ColumnIndex)
        ElseIf Not isUnique(Rows(row).Cells("code").Value, "code") Then
            addError(result, "Code has to be unique", row, Rows(row).Cells("code").ColumnIndex)
        End If

        If StringUtil.isEmpty(Rows(row).Cells("entity_name").Value) Then
            addError(result, "Name cannot be empty", row, Rows(row).Cells("entity_name").ColumnIndex)
        ElseIf Not isUnique(Rows(row).Cells("entity_name").Value, "entity_name") Then
            addError(result, "Name has to be unique", row, Rows(row).Cells("entity_name").ColumnIndex)
        End If

        Dim c As Integer
        Dim cName As String
        Dim dt As DataTable = Me.DataSource
        For c = 0 To Me.ColumnCount - 1
            cName = dt.Columns(c).ColumnName
            Dim typ As Type = dt.Columns(c).DataType
            If (typ.Name = "Int32") Or (typ.Name = "Double") Or (typ.Name = "Single") Then
                If Not IsDBNull(Rows(row).Cells(c).Value) Then
                    If (Rows(row).Cells(c).Value < 0) Then
                        addError(result, "Negative values not allowed", row, c)
                    End If
                End If
            End If
        Next
    End Sub


    Protected Overrides Sub OnDataError(displayErrorDialogIfNoHandler As Boolean, e As DataGridViewDataErrorEventArgs)
        Dim s As String
        s = Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        e.Cancel = False
        s = ""
        Dim type As Type = Rows(e.RowIndex).Cells(e.ColumnIndex).ValueType

        Dim ex As Exception = e.Exception
        If (ex.Message.Contains("format")) Then
            MessageBox.Show(Me, "Value Invalid")
        End If

        displayErrorDialogIfNoHandler = False
    End Sub

    Protected Overrides Function getBusinessReportDlg() As BusinessReportDlg
        Dim myDlg As BusinessReportEntityDlg = CType(Parent, BusinessReportEntityDlg)
        getBusinessReportDlg = CType(myDlg.Owner, BusinessReportDlg)
    End Function

    Public Function isValidCode(code As String, dt As DataTable, Optional codeColName As String = "code") As Boolean
        isValidCode = getBusinessReportDlg().isValidCode(code, dt, codeColName)
    End Function

    Public Function lookupName(code As String, dt As DataTable, Optional codeColName As String = "code", Optional nameColName As String = "name") As String
        lookupName = getBusinessReportDlg().lookupName(code, dt, codeColName, nameColName)
    End Function

    Protected Function lookupId(code As String, dt As DataTable, Optional codeColName As String = "code", Optional idColName As String = "id")
        lookupId = getBusinessReportDlg().lookupID(code, dt, codeColName, idColName)
    End Function

    Public Overrides Function insertRow(rowAt As Integer)
        rowAt += 1
        insertEntityRow(rowAt)
        Return rowAt
    End Function

    Protected Overridable Function insertEntityRow(rowAt As Integer) As DataRow
        Dim row As DataRow = Me.DataSource.NewRow()
        Dim dt As DataTable = Me.DataSource
        dt.Rows.InsertAt(row, rowAt)

        Return row
    End Function

    Protected Function codeIsUsed(code As Object) As Boolean
        Dim entityType As String
        If TypeOf Me Is ExpenseCategoriesEntityGridView Then
            entityType = "expense_category"
        ElseIf TypeOf Me Is ProductsEntityGridView Then
            entityType = "product"
        ElseIf TypeOf Me Is SuppliersEntityGridView Then
            entityType = "supplier"
        End If

        codeIsUsed = Not StringUtil.isEmpty(code) AndAlso SolarHouseDao.isCodeBeingUsed(code, entityType)

    End Function

    Protected Function locateDTRowForCode(code As String) As Integer
        Dim dt As DataTable = DataSource
        For i As Integer = 0 To dt.Rows.Count
            If (UIUtil.subsIfEmpty(dt.Rows(i).ItemArray(0), "") = code) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Public Overrides Sub deleteRow(row As Integer, parent As BusinessReportDlg)
        Dim code As String = UIUtil.subsIfEmpty(Rows(row).Cells(Columns("code").Index).Value, "")
        If Not codeIsUsed(code) OrElse Not isUnique(Rows(row).Cells(Columns("code").Index).Value, "code") Then
            Dim rowToDel As Integer = locateDTRowForCode(code)
            If (rowToDel <> -1) Then
                Me.DataSource.Rows(row).Delete()
            End If
        Else
            MessageBox.Show(Me, "Code:" + code + " is currently being used. Cannot delete or change")
        End If
    End Sub

    Protected Overrides Sub OnCellClick(e As DataGridViewCellEventArgs)
        CurrentRow.Selected = True
    End Sub


    Protected Overrides Sub OnCellValidated(e As DataGridViewCellEventArgs)
        MyBase.OnCellValidated(e)
        Dim cell As DataGridViewCell = Rows(e.RowIndex).Cells(e.ColumnIndex)
        If e.ColumnIndex = Columns("code").Index Then
            If (Not StringUtil.isEmpty(cell.Value) AndAlso UCase(cell.Value) <> cell.Value) Then
                cell.Value = UCase(cell.Value)
            End If
        End If
    End Sub


    Protected Overrides Sub OnCellBeginEdit(e As DataGridViewCellCancelEventArgs)
        If e.ColumnIndex = Columns("code").Index Then
            e.Cancel = codeIsUsed(CurrentCell.Value) AndAlso isUnique(CurrentCell.Value, "code")
            If (e.Cancel) Then
                MessageBox.Show(Me, "Code:" + CurrentCell.Value + " is being used. Cannot change or delete")
            End If
        ElseIf e.ColumnIndex = 0 Then
        End If
        MyBase.OnCellBeginEdit(e)

    End Sub

    Protected Overrides Sub OnCellValueChanged(e As DataGridViewCellEventArgs)
        MyBase.OnCellValueChanged(e)
    End Sub

    Protected Function getBusinessReportEntityDlg() As BusinessReportEntityDlg
        Return CType(Parent, BusinessReportEntityDlg)

    End Function

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.Handled = selectCurrentRow()
            If (e.Handled) Then
                Dim dlg As BusinessReportEntityDlg = getBusinessReportEntityDlg()
                dlg.DialogResult = DialogResult.OK
            End If
        End If
        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)
        If isControlUPressed(e) OrElse isControlDPressed(e) Then
            gotoNextSelectedRow(e)
        End If
    End Sub

    Protected Function parseCodeNumber(codePrefix As String, codeColVal As Object) As Integer
        If IsNothing(codeColVal) Then
            Return -1
        End If
        Dim codeValStr As String = codeColVal.ToString
        If (codeValStr.IndexOf(codePrefix) = -1) Then
            Return -1
        End If
        Dim codeNumAsStr As String = codeValStr.Substring(codePrefix.Length)
        Return If(IsNumeric(codeNumAsStr), Integer.Parse(codeNumAsStr), -1)
    End Function

    Protected Function findTheNextCode(codeColName As String, codePrefix As String) As String
        Dim dt As DataTable = Me.DataSource
        Dim maxCode As Integer = -1
        Dim codeNum As Integer
        For r As Integer = 0 To dt.Rows.Count - 1
            If (dt.Rows(r).RowState <> DataRowState.Deleted) Then
                codeNum = parseCodeNumber(codePrefix, dt.Rows(r).Item(codeColName))
                If codeNum <> -1 AndAlso maxCode < codeNum Then
                    maxCode = codeNum
                End If
            End If
        Next

        Dim nextCodeNumStr As String = If(maxCode < 0, "0", (maxCode + 1).ToString)

        For i As Integer = nextCodeNumStr.Length + 1 To 3
            codePrefix += "0"
        Next
        Return codePrefix + nextCodeNumStr
    End Function


    Public Function isControlUPressed(e As KeyEventArgs)
        Return (e.KeyCode = Keys.KeyCode.U) And (e.Modifiers = Keys.Control)
    End Function

    Public Function isControlDPressed(e As KeyEventArgs)
        Return (e.KeyCode = Keys.KeyCode.D) And (e.Modifiers = Keys.Control)
    End Function

    Protected Overrides Sub colorCell(e As DataGridViewCellFormattingEventArgs)
        If matchingRows.IndexOf(e.RowIndex) = -1 Then
            MyBase.colorCell(e)
        End If

    End Sub

    Public Sub gotoNextSelectedRow(e As KeyEventArgs)
        If isControlUPressed(e) Then
            If (nxtSelRow > 0) Then
                nxtSelRow -= 1
            Else
                nxtSelRow = 0
            End If
        ElseIf isControlDPressed(e) Then
            If (nxtSelRow >= matchingRows.Count - 1) Then
                nxtSelRow = matchingRows.Count - 1
            Else
                nxtSelRow += 1
            End If
        End If

        If nxtSelRow >= 0 AndAlso nxtSelRow < matchingRows.Count Then
            Me.Select()

            If FirstDisplayedScrollingRowIndex <> matchingRows(nxtSelRow) Then
                FirstDisplayedScrollingRowIndex = matchingRows(nxtSelRow)
            End If
            Rows(matchingRows(nxtSelRow)).Selected = True
        End If

    End Sub

    Private Function allCodeWordsInLookupCodeText(ByVal code As String, ByVal lookupCodeText As String)
        Dim codesArray() As String
        Dim i0 As Integer

        If Len(Trim(lookupCodeText)) = 0 Then
            allCodeWordsInLookupCodeText = False
        Else
            codesArray = Split(code)

            Dim fnd As Boolean = True
            For i0 = 0 To UBound(codesArray)
                If InStr(lookupCodeText, codesArray(i0)) <= 0 Then
                    fnd = False
                    Exit For
                End If
            Next i0

            allCodeWordsInLookupCodeText = fnd
        End If
    End Function

    Public Sub highlightRowsMatchingSearchString(searchStr As String)
        matchingRows.Clear()

        If (StringUtil.isEmpty(searchStr)) Then
            Return
        End If

        Dim row As Integer
        Dim i As Integer
        Dim txt As String
        Dim srchText As String = UCase(searchStr)
        Dim prodSrchTxt As String
        Dim firstSelectedRow As Integer = -1

        For row = 0 To Rows.Count - 1
            prodSrchTxt = UCase(Rows(row).Cells("code").Value + " " + Rows(row).Cells("entity_name").Value)
            txt = ""
            For i = 0 To ColumnCount - 1
                If Not IsNothing(Rows(row).Cells(i).Value) Then
                    txt += " " + Rows(row).Cells(i).Value.ToString
                Else
                    txt += " "
                End If

            Next

            If (allCodeWordsInLookupCodeText(srchText, prodSrchTxt)) Then
                matchingRows.Add(row)
                Rows(row).DefaultCellStyle.BackColor = Color.FromArgb(255, 232, 151)
                If (firstSelectedRow = -1) Then
                    firstSelectedRow = row
                End If
            End If

        Next
        If (firstSelectedRow <> -1) Then
            FirstDisplayedScrollingRowIndex = firstSelectedRow
            Rows(firstSelectedRow).Selected = True
        End If

        Refresh()
    End Sub

End Class
