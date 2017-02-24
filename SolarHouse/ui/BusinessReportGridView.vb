Imports System.ComponentModel

Public Class BusinessReportGridView
    Inherits SolarHouseDataGridView

    Protected isProgramaticChange As Boolean = False
    Protected readOnlyCols As List(Of Integer) = New List(Of Integer)


    Public Class TransactionDateComparer
        Implements IComparer

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim rowX As DataGridViewRow = CType(x, DataGridViewRow)
            Dim rowY As DataGridViewRow = CType(y, DataGridViewRow)
            Dim medDate As New Date(5000, 12, 1)
            Dim maxDate As New Date(9999, 12, 1)
            Dim tranXDate As Date = UIUtil.parseDate(rowX.Cells(0).Value, Nothing)

            If (UIUtil.isNothingDate(tranXDate)) Then
                tranXDate = If(isRowEmpty(rowX), maxDate, medDate)
            End If
            Dim tranYDate As Date = UIUtil.parseDate(rowY.Cells(0).Value, Nothing)
            If (UIUtil.isNothingDate(tranYDate)) Then
                tranYDate = If(isRowEmpty(rowY), maxDate, medDate)
            End If
            Return tranXDate.CompareTo(tranYDate)
        End Function


    End Class


    Public Overrides Sub setupGridView()
        MyBase.setupGridView()

        For c As Integer = 0 To Columns.Count - 1
            If Columns(c).ReadOnly Then
                readOnlyCols.Add(c)
            End If
            Columns(c).SortMode = DataGridViewColumnSortMode.Programmatic
        Next


        defaultRows()

    End Sub

    Protected Sub defaultRows()
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()
        If (dlg.isAnAmendementReport()) Then
            Return
        Else
            For i As Integer = 1 To 40
                Rows().Add()
            Next
        End If


    End Sub

    Public Sub makeItReadOnly(isToMakeReadOnly As Boolean)
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()
        For i As Integer = 0 To Rows.Count - 1
            ' If (Not IsNothing(dlg) AndAlso dlg.isAnAmendementReport()) OrElse (isTransactionPosted(i) OrElse isReversalTransaction(i)) Then
            If (isTransactionPosted(i) OrElse isReversalTransaction(i)) Then
                Rows(i).ReadOnly = True
            Else
                Rows(i).ReadOnly = isToMakeReadOnly
            End If
        Next
    End Sub

    Public Overridable Function validateRows()
        Sort(New TransactionDateComparer)
        Dim isValid As Boolean = True
        Dim rowsCnt As Integer = Me.RowCount
        Dim r As Integer
        For r = 0 To RowCount - 1
            If Not validateRow(r) Then
                isValid = False
            End If
        Next

        Return isValid
    End Function

    Protected Overrides Function validateRow(row As Integer) As Boolean
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()
        If (isProgramaticChange OrElse (Not IsNothing(dlg) AndAlso dlg.isDlgLoading)) Then
            Return True
        End If
        If (dlg.isReportSubmitted() OrElse dlg.isReportLoaded() OrElse isTransactionPosted(row)) Then
            Return True
        End If
        Return MyBase.validateRow(row)
    End Function

    Public Sub getTransactionDates(ByRef minDate As Date, ByRef maxDate As Date)
        minDate = New Date(9999, 1, 1)
        maxDate = New Date(1970, 1, 1)
        Dim curDate As Date
        For r As Integer = 0 To Rows.Count - 1
            curDate = UIUtil.parseDate(Rows(r).Cells(0).Value)
            If curDate > maxDate Then
                maxDate = curDate
            End If
            If curDate < minDate Then
                minDate = curDate
            End If
        Next
    End Sub

    Protected Overrides Sub OnRowValidating(e As DataGridViewCellCancelEventArgs)
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()
        'If (dlg.isAnAmendementReport() AndAlso Not isRowEmpty(e.RowIndex)) Then
        '    Rows(e.RowIndex).Cells(getIsAmendmentColName()).Value = UIUtil.toBinaryBooleanString(True)
        'End If
        MyBase.OnRowValidating(e)
    End Sub

    Protected Overrides Sub doValidateRow(row As Integer, ByRef result As RowValidationResult)

        MyBase.doValidateRow(row, result)
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()

        If (StringUtil.isEmpty(Rows(row).Cells(0).Value)) Then
            addError(result, "Has to be entered", row, 0)
        Else
            Dim fromDate As Date
            Dim toDate As Date
            Dim toDateStr As String
            Dim fromDateStr As String

            If (dlg.getBusinessReportToFromDates(fromDate, toDate)) Then
                toDateStr = UIUtil.toDateString(toDate)
                fromDateStr = UIUtil.toDateString(fromDate)
                If (StringUtil.isEmpty(Rows(row).Cells(0).Value)) Then
                    addError(result, "date has to be entered", row, 0)
                ElseIf (Not UIUtil.isValidDate(Rows(row).Cells(0).Value)) Then
                    addError(result, "invalid date", row, 0)
                Else
                    Dim vDate As Date = UIUtil.parseDate(Rows(row).Cells(0).Value)
                    If vDate < fromDate OrElse vDate > toDate Then
                        addError(result, "has to be between " + fromDateStr + " and " + toDateStr, row, 0)
                    End If
                End If
            End If

        End If

        If (isAmendmentRow(row) AndAlso StringUtil.isEmpty(Rows(row).Cells(getCommentColName()).Value)) Then
            addError(result, "Why are you amending a posted period? Enter Comment", row, Columns(getCommentColName()).Index)
        End If
    End Sub

    Protected Function isAmendmentRow(row As Integer) As Boolean
        Return UIUtil.toBoolean(Rows(row).Cells(getIsAmendmentColName()).Value)
    End Function

    Protected Overrides Function getBusinessReportDlg() As BusinessReportDlg
        getBusinessReportDlg = CType(Parent.Parent.Parent, BusinessReportDlg)
    End Function

    Public Function isValidCode(code As String, dt As DataTable) As Boolean
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()
        Return dlg.isValidCode(code, dt)
    End Function


    Public Overridable Sub loadBusinessReportDataFromGrid(busRprt As BusinessReportDAO.BusinessReport)

    End Sub

    Public Overridable Sub loadBusinessReportDataIntoGrid(busRprt As BusinessReportDAO.BusinessReport)
        Me.Refresh()
    End Sub

    Protected Overrides Sub OnCellValueChanged(e As DataGridViewCellEventArgs)
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()
        If isProgramaticChange OrElse (Not IsNothing(dlg) AndAlso dlg.isDlgLoading) Then
            Return
        End If

        If (e.RowIndex >= RowCount Or e.RowIndex < 0) Then
            Exit Sub
        End If

        gridCellValueChanged(e.ColumnIndex, e.RowIndex)

        If Not IsNothing(getBusinessReportDlg()) Then
            getBusinessReportDlg().recalcSummary()
        End If

        If e.ColumnIndex = 1 And e.RowIndex >= 0 Then
            Rows(e.RowIndex).Cells(e.ColumnIndex).Value = UCase(Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        End If


        MyBase.OnCellValueChanged(e)
    End Sub

    Protected Overridable Sub gridCellValueChanged(colInd As Integer, rowInd As Integer)

    End Sub



    Protected Overrides Sub OnCellBeginEdit(e As DataGridViewCellCancelEventArgs)
        Dim row As Integer = e.RowIndex
        If (isRowEmpty(row)) Then
            defaultNewRow(row)
        End If

        defaultTransDate(row)
        MyBase.OnCellBeginEdit(e)
    End Sub

    Protected Sub entityDataSetModified(isEntityDSMod As Boolean)
        If (isEntityDSMod) Then
            getBusinessReportDlg().isModified = True
        End If

    End Sub

    Protected Sub defaultTransDate(row As Integer)
        Dim toDate As Date
        Dim fromDate As Date

        Dim curDateCell As DataGridViewCell = Rows(row).Cells(0)
        If UIUtil.isNothingDate(UIUtil.parseDate(curDateCell.Value, Nothing)) Then

            If (curDateCell.RowIndex > 0) Then
                curDateCell.Value = Rows(row - 1).Cells(0).Value
            Else
                Dim dlg As BusinessReportDlg = getBusinessReportDlg()
                If (dlg.getBusinessReportToFromDates(fromDate, toDate) AndAlso dlg.ensureValidReportFromAndToDates()) Then
                    curDateCell.Value = UIUtil.toDateString(fromDate)
                End If

            End If
            Refresh()
        End If
    End Sub

    Protected Overridable Sub defaultNewRow(row As Integer)

    End Sub

    Protected Sub deleteAllRows()
        While Rows.Count > 0
            Rows.RemoveAt(0)
        End While
    End Sub
    Public Sub clearAllEntries()
        isProgramaticChange = True
        deleteAllRows()
        defaultRows()
        isProgramaticChange = False
    End Sub

    Protected Function getNumber(row As Integer, col As Integer) As Integer
        Return UIUtil.zeroIfEmpty(Me.Rows(row).Cells(col).Value)
    End Function

    Public Overrides Function insertRow(rowAt As Integer)
        rowAt += 1
        Me.Rows.Insert(rowAt, 1)
        Me.CurrentCell = Me.Rows(rowAt).Cells(0)
        BeginEdit(True)
        insertRow = rowAt
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()
        If (Not IsNothing(dlg) AndAlso dlg.isAnAmendementReport) Then
            Rows(rowAt).Cells(getIsAmendmentColName()).Value = UIUtil.toBinaryBooleanString(True)
            Rows(rowAt).ReadOnly = False
            Me.Refresh()
        End If
    End Function

    Protected Overrides Sub colorCell(e As DataGridViewCellFormattingEventArgs)
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()

        If dlg.isAnAmendementReport AndAlso Not dlg.showPostedAmendedRecsChkBox.Checked Then
            If isTransactionPosted(e.RowIndex) Then
                If (Me.Columns(e.ColumnIndex).ReadOnly) Then
                    e.CellStyle.BackColor = Color.FromArgb(187, 255, 187)
                Else
                    e.CellStyle.BackColor = Color.FromArgb(219, 255, 219)
                End If
            ElseIf isReversalTransaction(e.RowIndex) Then

                If (Me.Columns(e.ColumnIndex).ReadOnly) Then
                    e.CellStyle.BackColor = Color.FromArgb(255, 175, 175)
                Else
                    e.CellStyle.BackColor = Color.FromArgb(255, 232, 232)
                End If
            ElseIf UIUtil.toBoolean(Rows(e.RowIndex).Cells(getIsAmendmentColName()).Value) Then
                If (Me.Columns(e.ColumnIndex).ReadOnly) Then
                    e.CellStyle.BackColor = Color.FromArgb(255, 249, 128)
                Else
                    e.CellStyle.BackColor = Color.FromArgb(255, 254, 223)
                End If
            End If
        Else
            If isReversalTransaction(e.RowIndex) Then
                If (Me.Columns(e.ColumnIndex).ReadOnly) Then
                    e.CellStyle.BackColor = Color.FromArgb(255, 175, 175)
                Else
                    e.CellStyle.BackColor = Color.FromArgb(255, 232, 232)
                End If
            ElseIf UIUtil.toBoolean(Rows(e.RowIndex).Cells(getIsAmendmentColName()).Value) Then
                If (Me.Columns(e.ColumnIndex).ReadOnly) Then
                    e.CellStyle.BackColor = Color.FromArgb(255, 249, 128)
                Else
                    e.CellStyle.BackColor = Color.FromArgb(255, 254, 223)
                End If
            ElseIf isTransactionPosted(e.RowIndex) Then
                If (Me.Columns(e.ColumnIndex).ReadOnly) Then
                    e.CellStyle.BackColor = Color.FromArgb(187, 255, 187)
                Else
                    e.CellStyle.BackColor = Color.FromArgb(219, 255, 219)
                End If
            Else
                MyBase.colorCell(e)
            End If
        End If
    End Sub

    Protected Overrides Sub OnCellDoubleClick(e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then
            Return
        End If
        If isTransactionPosted(e.RowIndex) Then
            MessageBox.Show("Posted transactions cannot be edited. Reverse(delete) the transaction and Add a new one")
        ElseIf isReversalTransaction(e.RowIndex) Then
            Dim dlg As New PostedReversalCommentDlg(Rows(e.RowIndex).Cells(getCommentColName()).Value)
            If (dlg.ShowDialog() = DialogResult.OK) Then
                Rows(e.RowIndex).Cells(getCommentColName()).Value = dlg.getComment
            End If
        Else
            MyBase.OnCellDoubleClick(e)
        End If

    End Sub

    Protected Overridable Function getCommentColName() As String

    End Function

    Protected Overridable Function getIsAmendmentColName() As String

    End Function

    Protected Overridable Function isTransactionPosted(row As Integer) As Boolean
        Return False
    End Function

    Protected Overridable Function isReversalTransaction(row As Integer) As Boolean
        Return False
    End Function

    Protected Overridable Sub indicateTransactionReversed(row As Integer, Optional isReversed As Boolean = True)

    End Sub

    Protected Overridable Sub indicateTransactionPosted(row As Integer, Optional isPosted As Boolean = True)

    End Sub


    Protected Overridable Function reversePostedTransaction(row As Integer) As Boolean
        Dim dlg As New PostedReversalCommentDlg("")
        If (dlg.ShowDialog() = DialogResult.OK) Then
            Dim trans As DataGridViewRow = CType(Rows(row).Clone, DataGridViewRow)
            For index As Int32 = 0 To Rows(row).Cells.Count - 1
                trans.Cells(index).Value = Rows(row).Cells(index).Value
            Next
            Me.Rows.Insert(row, trans)
            Rows(row).Cells(getCommentColName()).Value = dlg.getComment()
            Rows(row).Cells(getIsAmendmentColName()).Value = UIUtil.toBinaryBooleanString(True)
            indicateTransactionReversed(row, True)
            indicateTransactionPosted(row, False)
            Return True
        Else
            Return False
        End If

    End Function

    Public Function deleteRow(entityName As String, Optional usrPrmptCol1 As Integer = 0, Optional usrPrmptCol2 As Integer = 1)
        Dim rowTxt As String
        Dim isRowDeleted As Boolean = False

        Dim isRowPosted As Boolean = isTransactionPosted(CurrentRow.Index)
        rowTxt = CurrentRow.Cells(usrPrmptCol1).Value + " " + CurrentRow.Cells(usrPrmptCol2).Value
        If MessageBox.Show(Me, "Do you wish to " + If(isRowPosted, " reverse ", " delete ") + entityName + " row " + rowTxt, entityName, MessageBoxButtons.YesNo) = DialogResult.Yes Then
            If Not isRowPosted Then
                Try
                    Me.Rows.RemoveAt(CurrentRow.Index)
                    isRowDeleted = True
                Catch ex As System.InvalidOperationException
                    ' cant delete uncommited row
                End Try
            Else
                reversePostedTransaction(CurrentRow.Index)
            End If
        End If
        deleteRow = isRowDeleted
    End Function

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        If (e.KeyCode = Keys.Insert) Then
            insertRow(CurrentRow.Index)
            e.Handled = True
        End If
        MyBase.OnKeyUp(e)
    End Sub

    Public Overridable Sub deleteCurrentRow()
        Throw New NotImplementedException
    End Sub

End Class
