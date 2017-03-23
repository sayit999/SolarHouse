Public Class SolarHouseDataGridView
    Inherits DataGridView
    ' attibutes

    Public Const ROW_VALIDATION_MESSAGE As Integer = 99999

    Private prevRowEntered As Integer = -1
    Private curRowEntered As Integer = -1

    Private prevRowHighlighted As Integer = -1

    Protected rowsChanged As Collection = New Collection

    Enum ValidationMessageType
        IS_ERROR
        IS_WARNING
        IS_VALID
    End Enum

    Public Class RowValidationColMessage
        Public col As Integer
        Public validationMessage As String
        Public validationMessageType As ValidationMessageType

        Public Sub setMessageAsRowMessage()
            col = ROW_VALIDATION_MESSAGE
        End Sub
    End Class

    Protected Shared Function isRowEmpty(row As DataGridViewRow)
        For c = 0 To row.Cells.Count - 1
            If Not (StringUtil.isEmpty(row.Cells(c).Value)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Protected Function isRowEmpty(row As Integer)
        If row >= 0 And row < RowCount Then
            Return isRowEmpty(Rows(row))
        Else
            Return True
        End If
    End Function

    Protected Overridable Sub setupDateEntryMasksFormat()
        Dim c As Integer
        For c = 0 To Me.ColumnCount - 1
            If TypeOf Me.Columns(c) Is MaskedEditColumn Then
                Dim col As MaskedEditColumn = DirectCast(Me.Columns(c), MaskedEditColumn)
                col.Mask = "00/00/0000"
            End If
        Next
    End Sub


    Public Overridable Sub setupGridView()
        Dim style As DataGridViewCellStyle = Me.ColumnHeadersDefaultCellStyle
        style.BackColor = Color.DarkCyan
        Me.ColumnHeadersDefaultCellStyle = style

        setupDateEntryMasksFormat()

        ' colorGrid()

        Dim dgvColumnHeaderStyle As New DataGridViewCellStyle()
        dgvColumnHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvColumnHeaderStyle.BackColor = Color.FromArgb(0, 193, 0)
        ColumnHeadersDefaultCellStyle = dgvColumnHeaderStyle
    End Sub

    Public Class RowValidationResult
        Public validationMessages As Collection = New Collection

    End Class

    Protected Function isControlSPressed(e As KeyEventArgs)
        isControlSPressed = (e.KeyCode = Keys.KeyCode.S) And (e.Modifiers = Keys.Control)
    End Function


    Private Function findNextEditableCol(curCol As Integer, grdVw As SolarHouseDataGridView)
        Dim colFnd As Integer = -1
        Dim i As Integer

        For i = curCol To grdVw.ColumnCount - 1
            If grdVw.Columns(i).Visible AndAlso Not grdVw.Columns(i).ReadOnly Then
                colFnd = i
                Exit For
            End If
        Next
        findNextEditableCol = colFnd


    End Function

    Public Function moveToNextEditableColumn(grdVw As SolarHouseDataGridView)
        Dim row As Integer = grdVw.CurrentRow.Index
        Dim col As Integer
        col = grdVw.CurrentCell.ColumnIndex

        col = findNextEditableCol(col, grdVw)
        If col = -1 Then
            col = findNextEditableCol(0, grdVw)
            row += 1
        End If

        If col <> -1 Then
            If row >= grdVw.RowCount Then
                'grdVw.Rows.Insert(row - 1)
                grdVw.Rows.Add()
            End If
            grdVw.CurrentCell = grdVw.Item(col, row)
            If (Not grdVw.CurrentCell.Displayed) Then
                grdVw.FirstDisplayedCell = grdVw.CurrentCell
            End If
        End If

    End Function


    Protected Overrides Sub OnCellDoubleClick(e As DataGridViewCellEventArgs)
        If Not Me.CurrentCell.ReadOnly Then
            BeginEdit(True)
        End If
    End Sub

    Protected Overrides Sub OnCellValidated(e As DataGridViewCellEventArgs)
        MyBase.OnCellValidated(e)
        Dim cell As DataGridViewCell = Rows(e.RowIndex).Cells(e.ColumnIndex)
        If Me.Columns(e.ColumnIndex).DefaultCellStyle.Format = "N0" And IsNumeric(cell.Value) Then
            If Not StringUtil.isEmpty(cell.Value) Then
                cell.Value = FormatNumber(cell.Value, 0, TriState.True, TriState.False, True)
            End If
        End If

    End Sub

    Protected Overrides Sub OnCellFormatting(e As DataGridViewCellFormattingEventArgs)
        If Not (UIUtil.isBetween(0, Me.ColumnCount - 1, e.ColumnIndex)) Or Not (UIUtil.isBetween(0, Me.RowCount, e.RowIndex)) Then
            Exit Sub
        End If
        colorCell(e)
    End Sub

    Protected Overrides Sub OnRowValidating(e As DataGridViewCellCancelEventArgs)

        validateRow(e.RowIndex)
    End Sub


    Protected Overridable Function getBusinessReportDlg() As BusinessReportDlg
        Throw New NotImplementedException
    End Function

    Protected Overrides Sub OnRowLeave(e As DataGridViewCellEventArgs)
        ' validateRow(e.RowIndex)
    End Sub

    Protected Sub setRowColValidationMesg(mesgType As ValidationMessageType, row As Integer, col As Integer, mesg As String)
        Dim dt As DataTable = Me.DataSource
        Dim errorTxtPrefix As String = getMessagePrefix(mesgType)
        Me.Rows(row).Cells(col).ErrorText = errorTxtPrefix + mesg
        If (Not IsNothing(dt)) Then
            dt.Rows(row).SetColumnError(col, errorTxtPrefix + mesg)
        End If
        Rows(row).ErrorText = "Row has errors or warnings"
    End Sub

    Protected Overridable Function validateRow(row As Integer) As Boolean
        If Not IsNothing(getBusinessReportDlg()) AndAlso getBusinessReportDlg().isDlgLoading Then
            Return True
        End If

        If (isRowEmpty(row)) Then
            Return True
            Exit Function
        End If

        Dim dt As DataTable = Me.DataSource
        Rows(row).ErrorText = ""
        Dim colCnt As Integer = Me.ColumnCount
        Dim c As Integer

        If (Not IsNothing(dt)) Then
            dt.Rows(row).ClearErrors()
        End If
        Rows(row).ErrorText = ""
        For c = 0 To colCnt - 1
            Rows(row).Cells(c).ErrorText = Nothing
        Next
        Dim result As RowValidationResult = New RowValidationResult
        doValidateRow(row, result)

        Dim isValid As Boolean = True
        Dim cnt As Integer = result.validationMessages.Count
        If (cnt > 0) Then
            Dim errorTxtPrefix As String = ""
            For i = 1 To cnt
                If (result.validationMessages(i).validationMessageType = ValidationMessageType.IS_ERROR) Then
                    isValid = False
                End If
                setRowColValidationMesg(result.validationMessages(i).validationMessageType, row, result.validationMessages(i).col, result.validationMessages(i).validationMessage)

                'errorTxtPrefix = getMessagePrefix(result.validationMessages(i).validationMessageType)
                'Me.Rows(row).Cells(result.validationMessages(i).col).ErrorText = errorTxtPrefix + result.validationMessages(i).validationMessage
                'If (Not IsNothing(dt)) Then
                '    dt.Rows(row).SetColumnError(result.validationMessages(i).col, errorTxtPrefix + result.validationMessages(i).validationMessage)
                'End If
            Next
            'If cnt > 0 Then
            '    Rows(row).ErrorText = "Row has errors or warnings"
            'End If
        End If
        Return isValid
    End Function

    Protected Sub addWarning(ByRef result As RowValidationResult, mesg As String, row As Integer, col As Integer)
        If col = ROW_VALIDATION_MESSAGE And result.validationMessages.Count > 0 Then
            Throw New Exception("there cant only be one Row Level Validation Message")
        End If
        result.validationMessages.Add(newValidationMessage(ValidationMessageType.IS_WARNING, mesg, row, col))
    End Sub

    Protected Sub addError(ByRef result As RowValidationResult, mesg As String, row As Integer, col As Integer)
        If col = ROW_VALIDATION_MESSAGE And result.validationMessages.Count > 0 Then
            Throw New Exception("there cant only be one Row Level Validation Message")
        End If
        result.validationMessages.Add(newValidationMessage(ValidationMessageType.IS_ERROR, mesg, row, col))

    End Sub

    Protected Sub addWarning(ByRef result As RowValidationResult, mesg As String, row As Integer, col As String)
        addWarning(result, mesg, row, Columns(col).Index)
    End Sub

    Protected Sub addError(ByRef result As RowValidationResult, mesg As String, row As Integer, col As String)
        addError(result, mesg, row, Columns(col).Index)
    End Sub

    Protected Function newValidationMessage(type As ValidationMessageType, mesg As String, row As Integer, col As Integer)
        Dim validMessage As RowValidationColMessage = New RowValidationColMessage
        validMessage.col = col
        If Not IsNothing(CurrentRow) Then
            mesg = UIUtil.subsIfEmpty(Rows(row).Cells(col).Value, "").ToString + " " + mesg
        End If
        validMessage.validationMessage = mesg
        validMessage.validationMessageType = type
        newValidationMessage = validMessage
    End Function

    Protected Function isNumericCol(colInd As Integer) As Boolean
        Return Columns(colInd).DefaultCellStyle.Format.IndexOf("N") >= 0
    End Function

    Protected Overridable Sub doValidateRow(row As Integer, ByRef result As RowValidationResult)
        Dim c As Integer
        For c = 0 To ColumnCount - 1
            If Not StringUtil.isEmpty(Rows(row).Cells(c).Value) Then
                If (isNumericCol(c)) Then
                    If Not IsNumeric(Rows(row).Cells(c).Value) Then
                        addError(result, "is not a Number", row, c)
                    End If
                ElseIf TypeOf Me.Columns(c) Is MaskedEditColumn Then
                    If Not UIUtil.isValidDate(Rows(row).Cells(c).Value) Then
                        addError(result, "is not a Date", row, c)
                    End If
                End If
            End If

        Next
    End Sub

    Protected Overridable Sub colorCell(e As DataGridViewCellFormattingEventArgs)
        Dim rdOnlyColor As Color
        Dim rdWrtColor As Color

        If (Me.Rows(e.RowIndex).Cells(e.ColumnIndex).IsInEditMode) Then
            rdOnlyColor = Color.FromArgb(108, 255, 108)
            rdWrtColor = Color.FromArgb(189, 255, 189)
        Else
            rdOnlyColor = Color.FromArgb(100, 177, 255)
            rdWrtColor = Color.FromArgb(183, 253, 253)
        End If

        If (Me.Columns(e.ColumnIndex).ReadOnly) Then
            e.CellStyle.BackColor = rdOnlyColor
        Else
            e.CellStyle.BackColor = rdWrtColor
        End If

        If (isNumericCol(e.ColumnIndex)) Then
            If (UIUtil.zeroIfEmpty(Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) < 0) Then
                Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.ForeColor = Color.Red
            Else
                Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.ForeColor = Color.Black
            End If
        End If
    End Sub

    Protected Function getMessagePrefix(msgTyp As ValidationMessageType)
        Select Case msgTyp
            Case ValidationMessageType.IS_ERROR
                getMessagePrefix = "Error: "
            Case ValidationMessageType.IS_WARNING
                getMessagePrefix = "Warning: "
            Case ValidationMessageType.IS_VALID
                getMessagePrefix = ""
            Case Else
                Throw New ArgumentException("No ValidationMessageType for mod type:" + msgTyp)
        End Select
    End Function

    Protected Overrides Sub OnCellBeginEdit(e As DataGridViewCellCancelEventArgs)

        MyBase.OnCellBeginEdit(e)
    End Sub

    Protected Overrides Sub OnCellEndEdit(e As DataGridViewCellEventArgs)
        MyBase.OnCellEndEdit(e)
    End Sub

    Public Function insertRowAtCurrentPos()
        Dim insRow As Integer = If(IsNothing(CurrentCell), 0, CurrentCell.RowIndex + 1)

        If (insRow > Rows.Count) Then
            insRow = Rows.Count
        End If
        Return insertRow(insRow)
    End Function

    Public Overridable Function insertRow(rowAt As Integer)
        Throw New NotImplementedException
    End Function

    Public Overridable Sub deleteRow(row As Integer, parent As BusinessReportDlg)
        Throw New NotImplementedException
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        If e.KeyCode = Keys.Tab Then
            moveToNextEditableColumn(Me)
            e.Handled = True
        ElseIf isNumericCol(CurrentCell.ColumnIndex) Then
            e.SuppressKeyPress = Not IsNumeric(CurrentCell.Value)
        End If

        MyBase.OnKeyUp(e)
    End Sub

    Protected Overrides Sub OnCellPainting(e As DataGridViewCellPaintingEventArgs)
        MyBase.OnCellPainting(e)
        'If (Not StringUtil.isEmpty(e.ErrorText)) Then
        '    e.Paint(e.ClipBounds, DataGridViewPaintParts.All And Not (DataGridViewPaintParts.ErrorIcon))
        '    Dim container As Drawing2D.GraphicsContainer = e.Graphics.BeginContainer()
        '    e.Graphics.TranslateTransform(-1 * e.CellBounds.Width, 0)
        '    e.Paint(e.ClipBounds, DataGridViewPaintParts.ErrorIcon)
        '    e.Graphics.EndContainer(container)
        '    e.Handled = True
        'End If

    End Sub

    Protected Function isValidDataGridViewRow(row As Integer) As Boolean
        Return row >= 0 AndAlso row < Rows.Count
    End Function


End Class
