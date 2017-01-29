Imports System.ComponentModel

Public Class BusinessReportEntityDlg


    Private prevRowHighlighted As Integer = -1
    Protected rowsChanged
    Public codeSelected As String = Nothing
    Public nameSelected As String = Nothing


    Protected businessReportDlg As BusinessReportDlg = Nothing

    Public isEntitySelected As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        businessReportDlg = Nothing
    End Sub

    Public Sub New(dlg As BusinessReportDlg)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        businessReportDlg = dlg
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        Dim dtOrg As DataTable
        Dim grdView = getBusinessRprtEntityGridView(getBusinessReportDlg())
        If Not IsNothing(grdView) Then
            dtOrg = grdView.DataSource
            grdView.DataSource = dtOrg.Copy
        End If
        If Not IsNothing(codeSelected) Then

        End If

        MyBase.OnLoad(e)
    End Sub

    Protected Overridable Function getBusinessRprtEntityGridView(busDlg As BusinessReportDlg) As BusinessRprtEntityGridView
        getBusinessRprtEntityGridView = Nothing
    End Function



    Protected Function insertRowAtCurrentPos()
        Dim grdView As BusinessRprtEntityGridView = Me.getGridView()
        insertRowAtCurrentPos = grdView.insertRowAtCurrentPos()
    End Function

    Protected Sub initializeSearchEntitiesText(searchEntitiesText As TextBox)
        If Not IsNothing(codeSelected) Then
            searchEntitiesText.Text = codeSelected
        End If
    End Sub

    Protected Function deleteRow(entityName As String, Optional usrPrmptCol1 As Integer = 0, Optional usrPrmptCol2 As Integer = 1)

        Dim grdView As BusinessRprtEntityGridView = Me.getGridView()

        If grdView.SelectedRows.Count <> 1 Then
            MessageBox.Show(Me, grdView.SelectedRows.Count.ToString + " selected.  Select the row to delete")
            Return False
        End If

        Dim row As Integer = grdView.SelectedRows(0).Index
        Dim rowTxt As String

        rowTxt = grdView.Rows(row).Cells(usrPrmptCol1).Value + " " + grdView.Rows(row).Cells(usrPrmptCol2).Value
        If MessageBox.Show(Me, "Do you wish to delete row " + rowTxt, entityName, MessageBoxButtons.YesNo) = DialogResult.Yes Then
            grdView.deleteRow(row, getBusinessReportDlg())
            deleteRow = True
        Else
            deleteRow = False
        End If
    End Function

    Protected Function getBusinessReportDlg() As BusinessReportDlg
        Return businessReportDlg
    End Function

    Public Sub highlightRowsMatchingSearchString(searchStr As String)
        Dim grdView As BusinessRprtEntityGridView = Me.getGridView()
        grdView.highlightRowsMatchingSearchString(searchStr)
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If Me.DialogResult = DialogResult.Cancel Then
            Dim grdView = Me.getGridView()
            If grdView.CurrentCell.IsInEditMode Then
                Dim dt As DataTable = grdView.DataSource
                grdView.EndEdit()
                Dim x As Boolean = grdView.CurrentCell.IsInEditMode
            End If
            If Not grdView.isModified OrElse MessageBox.Show(Me, "Report has been modified. Do you wish to save the changes?", "Save Changes?", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                e.Cancel = Not saveBtnClicked()
            End If
        End If

    End Sub

    Protected Function saveBtnClicked()
        Dim grdView = Me.getGridView()
        If IsNothing(grdView) Then
            MessageBox.Show(Me, "Error: grdView var is Nothing.")
            Return False
            Exit Function
        End If

        If (Not grdView.validate()) Then
            MessageBox.Show(Me, "Errors found.  Fix the errors before proceeding")
            Return False
        Else
            Dim dt As DataTable = grdView.DataSource
            Me.setDataSet(dt)
            Me.DialogResult = DialogResult.OK
            Return True
            Me.Close()
        End If
    End Function

    Public Function wasModified() As Boolean
        Return Me.getGridView().isModified
    End Function

    Protected Overridable Sub setDataSet(ds As DataTable)
        MessageBox.Show(Me, "Override this")
    End Sub

    Public Overridable Function getDataSet() As DataTable
        getDataSet = Nothing
    End Function

    Protected Overridable Function getGridView() As BusinessRprtEntityGridView
        getGridView = Nothing
    End Function

    Protected Overridable Sub setGridView(grdVw As BusinessRprtEntityGridView)
        MessageBox.Show(Me, "Override this")
    End Sub

    Protected Sub selectEntityBtnClicked()
        Dim isOkToClose As Boolean = True
        If (wasModified()) Then
            If MessageBox.Show(Me, "Report has been modified. Do you want to save changes?", "Save Changes?", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                isOkToClose = saveBtnClicked()
            End If
        End If

        If isOkToClose Then
            If (getGridView().selectCurrentRow()) Then
                DialogResult = DialogResult.OK
            End If
        End If
    End Sub





    Protected Function isControlKeyPressed(e As KeyEventArgs) As Boolean
        Return e.Modifiers = Keys.Control
    End Function
End Class