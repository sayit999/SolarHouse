Imports System.Collections.ObjectModel

Public Class BusinessReportDlg

    Enum ReportType
        business_report
        amendment_report
    End Enum

    Public Const REPORT_SUBMITTED_DDL_TITLE As String = "<submitted>"
    Public Const REPORT_TO_SUBMIT_DDL_TITLE As String = "<to submit>"
    Public Const REPORT_TO_LOAD_DDL_TITLE As String = "<to load>"
    Public Const REPORT_LOADED_DDL_TITLE As String = "<loaded>"

    Public isModified As Boolean = False

    Public catDataSet As DataTable = Nothing
    Public qtyUomDataSet As DataTable = Nothing

    Public productsEntityDataSet As DataTable = Nothing
    Public expenseCategoriesEntityDataSet As DataTable = Nothing
    Public suppliersEntityDataSet As DataTable = Nothing

    Public salesDataSet As DataTable = Nothing
    Public purchasesDataSet As DataTable = Nothing
    Public debtPaymentsDataSet As DataTable = Nothing
    Public expensesDataSet As DataTable = Nothing

    Private busReports As List(Of ReportFile)

    Public isLoading As Boolean = False

    Public minPostedTranDate As Date
    Public maxPostedTranDate As Date

    Dim reptType As ReportType

    Dim businessReportsToRetStatus As ReportFile.BusinessReportStatus = ReportFile.BusinessReportStatus.to_submit

    Public Sub changeBusinessReportsToRetStatus(toBusinessReportsToRetStatus As ReportFile.BusinessReportStatus)
        Select Case toBusinessReportsToRetStatus
            Case ReportFile.BusinessReportStatus.loaded
                loadedRdBtn.Select()
            Case ReportFile.BusinessReportStatus.to_load
                toLoadRdBtn.Select()
            Case ReportFile.BusinessReportStatus.to_submit
                toSubmitRdBtn.Select()
            Case ReportFile.BusinessReportStatus.submitted
                submittedRdBtn.Select()
        End Select
        businessReportsToRetStatus = toBusinessReportsToRetStatus
    End Sub

    Public Function isDlgLoading() As Boolean
        Return isLoading
    End Function

    Private Sub refreshBusinessReportsAvailComboBox(Optional selPrevBusinessReportFile As String = Nothing)
        'load business reports
        Dim prevSelIndex As Integer = reportsAvailComboBox.SelectedIndex
        reportsAvailComboBox.Items.Clear()
        Dim i As Integer
        busReports = ReportFile.listBusinessReports(businessReportsToRetStatus)
        For i = 0 To busReports.Count - 1
            If busReports(i).isSubmitted Then
                reportsAvailComboBox.Items.Add(busReports(i).fileName)
            ElseIf busReports(i).isToSubmit Then
                reportsAvailComboBox.Items.Add(busReports(i).fileName)
            ElseIf busReports(i).isToLoad Then
                reportsAvailComboBox.Items.Add(busReports(i).fileName)
            ElseIf busReports(i).isLoaded Then
                reportsAvailComboBox.Items.Add(busReports(i).fileName)
            End If
        Next

        If (Not IsNothing(selPrevBusinessReportFile)) Then
            For r As Integer = 0 To busReports.Count - 1
                If busReports(r).Equals(selPrevBusinessReportFile) Then
                    reportsAvailComboBox.SelectedIndex = r
                End If
            Next
        End If

    End Sub

    Private Sub loadedRdBtn_CheckedChanged(sender As Object, e As EventArgs) Handles loadedRdBtn.CheckedChanged
        businessReportsToRetStatus = ReportFile.BusinessReportStatus.loaded
        refreshBusinessReportsAvailComboBox()
    End Sub

    Private Sub toLoadRdBtn_CheckedChanged(sender As Object, e As EventArgs) Handles toLoadRdBtn.CheckedChanged
        businessReportsToRetStatus = ReportFile.BusinessReportStatus.to_load
        refreshBusinessReportsAvailComboBox()
    End Sub

    Private Sub submittedRdBtn_CheckedChanged(sender As Object, e As EventArgs) Handles submittedRdBtn.CheckedChanged
        businessReportsToRetStatus = ReportFile.BusinessReportStatus.submitted
        refreshBusinessReportsAvailComboBox()
    End Sub

    Private Sub toSubmitRdBtn_CheckedChanged(sender As Object, e As EventArgs) Handles toSubmitRdBtn.CheckedChanged
        businessReportsToRetStatus = ReportFile.BusinessReportStatus.to_submit
        refreshBusinessReportsAvailComboBox()
    End Sub

    Private Sub setUIDateRange(fromDate As Date, toDate As Date)
        reportFromDateTxtBox.Text = UIUtil.toDateString(fromDate)
        reportToDateTxtBox.Text = UIUtil.toDateString(toDate)
    End Sub

    Private Sub getUIDateRange(ByRef fromDate As Date, ByRef toDate As Date)
        If UIUtil.isValidDate(reportFromDateTxtBox.Text) Then
            fromDate = UIUtil.parseDate(reportFromDateTxtBox.Text)
        Else
            fromDate = Nothing
        End If

        If UIUtil.isValidDate(reportToDateTxtBox.Text) Then
            toDate = UIUtil.parseDate(reportToDateTxtBox.Text)
        Else
            toDate = Nothing
        End If

    End Sub

    Private Sub retrieveBusinessReportBtn_Click(sender As Object, e As EventArgs) Handles retrieveBusinessReportBtn.Click
        If reportsAvailComboBox.SelectedIndex < 0 Then
            MessageBox.Show(Me, "Select Report to Retrieve")
            Return
        End If

        Dim businessReportFileToSel As ReportFile = busReports(reportsAvailComboBox.SelectedIndex)

        If Not isReportModified() OrElse MessageBox.Show("Report has been modified. If you continue ALL data will be DELETED.  Continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            If businessReportFileToSel.isLoaded OrElse businessReportFileToSel.isSubmitted OrElse businessReportFileToSel.isToSubmit OrElse businessReportFileToSel.isToLoad Then

                Dim serv As BusinessReportService = New BusinessReportService(Me)
                isLoading = True
                UseWaitCursor = True
                enableButtons(False)
                MainForm.showProgress(-1, "Loading report...")
                Dim rprt As BusinessReportDAO.BusinessReport = serv.retrieveBusinessReport(businessReportFileToSel)
                reptType = If(rprt.isAnAmendmentToPostedTrans, ReportType.amendment_report, ReportType.business_report)
                MainForm.showProgress(20, "Report read...")
                wipeOutEnteredData()
                setUIDateRange(rprt.fromDate, rprt.toDate)
                MainForm.showProgress(20, "Loading report into UI...")
                If Not IsNothing(rprt) Then
                    loadBusinessReportDataIntoUI(rprt)
                End If
                MainForm.showProgress(20, "Report loaded into UI...")
                reportViewedStateChanged()
                MainForm.showProgress(20, "Summary recalculated...")
                isLoading = False
                If Not businessReportFileToSel.isSubmitted AndAlso Not businessReportFileToSel.isLoaded Then
                    validateBusinessReportsComponents()
                End If
                MainForm.showProgress(20, "Report validated")


                recalcSummary()
                isModified = False
                UseWaitCursor = False
                MainForm.showProgress()
            End If
        End If

    End Sub

    Private Function ensureUserNotifiedOfModifiedReport() As Boolean
        Return Not isReportModified() OrElse MessageBox.Show("Report has been modified. If you continue ALL data will be DELETED.  Continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes
    End Function

    Private Function retrieveLastReportSubmitted() As BusinessReportDAO.ReportLoadSubmitStatusVO
        Dim serv As New BusinessReportService(Me)
        Return serv.retrieveLastReportSubmitted(BusinessReportDAO.ReportType.business)
    End Function

    Private Sub newBusinessReportBtn_Click(sender As Object, e As EventArgs) Handles newBusinessReportBtn.Click
        ' If Not isReportModified() OrElse MessageBox.Show("Report has been modified. If you continue ALL data will be DELETED.  Continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
        If ensureUserNotifiedOfModifiedReport() Then
            reptType = ReportType.business_report
            wipeOutEnteredData(False)
            refreshBusinessReportsAvailComboBox()
            reportViewedStateChanged()

            Dim vo As BusinessReportDAO.ReportLoadSubmitStatusVO = retrieveLastReportSubmitted()
            If (Not IsNothing(vo)) Then
                reportFromDateTxtBox.Text = UIUtil.toDateString(vo.reportTo.AddDays(1))
                reportFromDateTxtBox.ReadOnly = True
                cashBroughtFwdTextBox.Text = UIUtil.toAmtString(vo.cashCounted)
                reportToDateTxtBox.Select()
            Else
                reportFromDateTxtBox.Text = UIUtil.toDateString(Today)
            End If
            reportToDateTxtBox.Text = UIUtil.toDateString(Today)
            Me.isModified = False

        End If
    End Sub

    Private Sub newAmendmentReportBtn_Click(sender As Object, e As EventArgs) Handles newAmendmentReportBtn.Click
        'If Not isReportModified() OrElse MessageBox.Show("Report has been modified. If you continue ALL data will be DELETED.  Continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
        If ensureUserNotifiedOfModifiedReport() Then
            Dim fromDate As Date
            Dim toDate As Date
            getUIDateRange(fromDate, toDate)
            Dim dlg As New DateRangeDlg(fromDate, toDate, minPostedTranDate, maxPostedTranDate)
            If dlg.ShowDialog() = DialogResult.OK Then
                reptType = ReportType.amendment_report
                fromDate = dlg.getFromDate
                toDate = dlg.getToDate
                isLoading = True
                UseWaitCursor = True
                Dim serv As New BusinessReportService(Me)
                wipeOutEnteredData()
                refreshBusinessReportsAvailComboBox()
                setUIDateRange(fromDate, toDate)
                Dim rprt As BusinessReportDAO.BusinessReport = serv.retrievePostedBusinessTransactions(fromDate, toDate)
                If Not IsNothing(rprt) Then
                    loadBusinessReportDataIntoUI(rprt)
                End If
                reportViewedStateChanged()
                isLoading = False
                recalcSummary()
                UseWaitCursor = False
                isModified = False
                MainForm.showProgress()
            End If
        End If
    End Sub

    Protected Sub populateDatasetsFromDB()
        If IsNothing(catDataSet) Then
            SolarHouseDao.loadProductCategories(catDataSet)
            catDataSet.AcceptChanges()
        End If

        If IsNothing(qtyUomDataSet) Then
            SolarHouseDao.loadQtyUOMs(qtyUomDataSet)
            qtyUomDataSet.AcceptChanges()
        End If

        SolarHouseDao.loadProducts(productsEntityDataSet)
        productsEntityDataSet.AcceptChanges()

        SolarHouseDao.loadSuppliers(suppliersEntityDataSet)
        suppliersEntityDataSet.AcceptChanges()

        SolarHouseDao.loadExpenseCategories(expenseCategoriesEntityDataSet)
        expenseCategoriesEntityDataSet.AcceptChanges()
    End Sub

    Private Function getFileSelIndex(fileName As String, folder As String)
        For i As Integer = 0 To busReports.Count - 1
            If (busReports(i).Equals(folder, fileName)) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Private Sub BusinessReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '  refreshBusinessReportsAvailComboBox(Nothing)
        populateDatasetsFromDB()

        Me.salesGrdView.setupGridView()
        Me.purchasesGrdView.setupGridView()
        Me.debtPaymentGrdView.setupGridView()
        Me.expensesGrdView.setupGridView()

        isModified = False
        newBusinessReportBtn_Click(Nothing, Nothing)
        ' newBusinessReportBtn.PerformClick()
        'wipeOutEnteredData(False)
        'reportViewedStateChanged()

        refreshPostedTranDateDRange()
        toSubmitRdBtn.Checked = True
        showPostedAmendedRecsChkBox.Checked = False

        fromDateErrorPic.init(Me)
        toDateErrorPic.init(Me)
        cashBroughtForwardErrorPic.init(Me)
        countedCashErrorPic.init(Me)

        Me.Height = businessReportTabControl.Height + businessReportTabControl.Location.Y + 50

    End Sub

    Protected Sub refreshPostedTranDateDRange()
        Dim serv As New BusinessReportService(Me)
        serv.getPostedTransactionDateRange(minPostedTranDate, maxPostedTranDate)
    End Sub


    Public Function getProdInventoryQty(prodCode As String) As Integer
        For r As Integer = 0 To productsEntityDataSet.Rows.Count - 1
            If (productsEntityDataSet.Rows(r).RowState <> DataRowState.Deleted AndAlso UIUtil.subsIfEmpty(productsEntityDataSet.Rows(r).Item("product_code"), "") = prodCode) Then
                getProdInventoryQty = UIUtil.zeroIfEmpty(productsEntityDataSet.Rows(r).Item("qty_available"))
                Exit Function
            End If
        Next
        getProdInventoryQty = 0
    End Function

    Public Function locateRow(keyVal As String, keyColName As String, dt As DataTable) As DataRow
        Dim r As Integer
        For r = 0 To dt.Rows.Count - 1
            If (dt.Rows(r).RowState <> DataRowState.Deleted AndAlso dt.Rows(r).Item(keyColName).Equals(keyVal)) Then
                locateRow = dt.Rows(r)
                Exit Function
            End If
        Next
        locateRow = Nothing
    End Function


    Public Function isValidCode(code As String, dt As DataTable, Optional codeColName As String = "code") As Boolean
        Dim row As DataRow = locateRow(code, codeColName, dt)
        isValidCode = Not IsNothing(row)
    End Function

    Public Function lookupName(code As String, dt As DataTable, Optional codeColName As String = "code", Optional nameColName As String = "name") As String
        Dim name As String = Nothing
        Dim row As DataRow = locateRow(code, codeColName, dt)
        If Not IsNothing(row) Then
            name = row.Item(nameColName)
        End If
        lookupName = name
    End Function

    Protected Sub getReportsInDateRange(fromDate As Date, toDate As Date, ByRef submitted As List(Of String), ByRef toSubmit As List(Of String))
        submitted = New List(Of String)
        toSubmit = New List(Of String)
        For i As Integer = 0 To busReports.Count - 1
            If Not (busReports(i).toDate < fromDate OrElse busReports(i).fromDate > toDate) Then
                If busReports(i).isSubmitted Then
                    submitted.Add(busReports(i).fileName)
                ElseIf busReports(i).toDate <> toDate OrElse busReports(i).fromDate <> fromDate Then
                    toSubmit.Add(busReports(i).fileName)
                End If
            End If

        Next
    End Sub

    Public Function ensureValidReportFromAndToDates()
        Dim isError As Boolean = False

        fromDateErrorPic.setErrorText("")
        toDateErrorPic.setErrorText("")

        If Not UIUtil.isValidDate(reportFromDateTxtBox.Text) Then
            fromDateErrorPic.setErrorText("Enter a valid report From date ")
            isError = True
        End If

        If Not UIUtil.isValidDate(reportToDateTxtBox.Text) Then
            toDateErrorPic.setErrorText("Enter a valid report To date ")
            isError = True
        End If

        If (isError) Then
            Return False
        End If

        Dim fromDate As Date = UIUtil.parseDate(reportFromDateTxtBox.Text)
        Dim toDate As Date = UIUtil.parseDate(reportToDateTxtBox.Text)

        If (toDate > Today()) Then
            toDateErrorPic.setErrorText("To Date cannot be in the future ")
            isError = False
        End If

        If (fromDate > Today()) Then
            fromDateErrorPic.setErrorText("From Date cannot be in the future ")
            isError = False
        End If

        If (toDate < fromDate) Then
            fromDateErrorPic.setErrorText("From Date has to be less than To Date ")
            toDateErrorPic.setErrorText("To Date has to be greater than From Date ")
            isError = False
        End If

        Dim txt As String
        If (isAnAmendementReport()) Then
            'If fromDate > maxPostedTranDate Then
            '    txt = "Transaction from: " + UIUtil.toDateString(minPostedTranDate) + " to " + UIUtil.toDateString(maxPostedTranDate) + " have been posted"
            '    fromDateErrorPic.setErrorText(txt + " From Date has to be less than or same as " + UIUtil.toDateString(maxPostedTranDate))
            'End If
            'If Not (fromDate >= minPostedTranDate AndAlso toDate <= maxPostedTranDate) Then
            '    txt = "Amendment period From and To Dates have to be Posted Transactions Dates  " + UIUtil.toDateString(minPostedTranDate) + " and " + UIUtil.toDateString(maxPostedTranDate)
            '    fromDateErrorPic.setErrorText(txt + " From Date has to be greater than " + UIUtil.toDateString(minPostedTranDate))
            '    toDateErrorPic.setErrorText(txt + " To Date has to be less than " + UIUtil.toDateString(maxPostedTranDate))
            '    Return False
            'End If
        ElseIf isBusinessReport() Then
            If (fromDate < maxPostedTranDate) Then
                fromDateErrorPic.setErrorText("Transactions upto " + UIUtil.toDateString(maxPostedTranDate) + " have been posted.  To change amend transactions before " + UIUtil.toDateString(maxPostedTranDate))
                isError = True

            End If
        End If


        Return Not isError
    End Function

    Public Function lookupID(code As String, dt As DataTable, Optional codeColName As String = "code", Optional idColName As String = "id")
        Dim id As Integer = Nothing
        Dim row As DataRow = locateRow(code, codeColName, dt)
        If Not IsNothing(row) Then
            id = row.Item(idColName)
        End If
        lookupID = id
    End Function

    Private Function validateBusinessReportsComponents()

        Dim businessReportsInErr As String = ""
        Dim rprtFromDate As Date
        Dim rprtToDate As Date
        Dim isErrorDates As Boolean = False

        If (Not getBusinessReportToFromDates(rprtFromDate, rprtToDate)) Then
            If (Not StringUtil.isEmpty(businessReportsInErr)) Then
                businessReportsInErr += ", "
            End If
            businessReportsInErr += "Dates"
        End If

        If isBusinessReport() Then
            Dim isCashError As Boolean = False
            cashBroughtForwardErrorPic.setErrorText("")
            countedCashErrorPic.setErrorText("")
            If (UIUtil.isEmpty(cashBroughtFwdTextBox.Text)) Then
                cashBroughtForwardErrorPic.setErrorText("Cash Brought Forward has to be entered")
                isCashError = True
            Else
                Dim cashVal As Integer = UIUtil.zeroIfEmpty(cashBroughtFwdTextBox.Text)
                If cashVal <= 0 Then
                    cashBroughtForwardErrorPic.setErrorText("Cash Brought Forward cannot be negative or zero")
                    isCashError = True
                End If

            End If

            If (UIUtil.isEmpty(maunuallyCountedCashTxtBox.Text)) Then
                countedCashErrorPic.setErrorText("Cash you Counted has to be entered")
                isCashError = True
            Else
                Dim cashVal As Integer = UIUtil.zeroIfEmpty(maunuallyCountedCashTxtBox.Text)
                If cashVal <= 0 Then
                    countedCashErrorPic.setErrorText("Cash you Counted cannot be negative or zero")
                    isCashError = True

                End If
            End If

            If (isCashError) Then
                If (Not StringUtil.isEmpty(businessReportsInErr)) Then
                    businessReportsInErr += ", "
                End If
                businessReportsInErr += "Cash"
            End If
        End If



        If Not (Me.salesGrdView.validateRows()) Then
            If (Not StringUtil.isEmpty(businessReportsInErr)) Then
                businessReportsInErr += ", "
            End If
            businessReportsInErr += "Sales"
        End If

        If Not (purchasesGrdView.validateRows()) Then
            If (Not StringUtil.isEmpty(businessReportsInErr)) Then
                businessReportsInErr += ", "
            End If
            businessReportsInErr += "Purchases"
        End If

        If Not (expensesGrdView.validateRows()) Then
            If (Not StringUtil.isEmpty(businessReportsInErr)) Then
                businessReportsInErr += ", "
            End If
            businessReportsInErr += "Expenses"
        End If

        If Not (debtPaymentGrdView.validateRows) Then
            If (Not StringUtil.isEmpty(businessReportsInErr)) Then
                businessReportsInErr += ", "
            End If
            businessReportsInErr += "Debt Payments"
        End If

        Dim isReportValid As Boolean = True
        If (businessReportsInErr.Length > 0) Then
            MessageBox.Show(Me, "Found errors in: " + businessReportsInErr)
            isReportValid = False
        End If

        Return isReportValid

    End Function


    Private Sub validateBusReportBtn_Click(sender As Object, e As EventArgs) Handles validateBusReportBtn.Click
        If (validateBusinessReportsComponents()) Then
            MessageBox.Show(Me, "Report is valid")
        End If
    End Sub

    Private Sub enableButtons(isEnable As Boolean)
        ' retrieveBusinessReportBtn.Enabled = isEnable
        saveBusReportBtn.Enabled = isEnable
        validateBusReportBtn.Enabled = isEnable
        commitBusReportBtn.Enabled = isEnable
        reEmailBtn.Enabled = isEnable
        insertTranRowBtn.Enabled = isEnable
        deleteTranRowBtn.Enabled = isEnable
        deleteReportBtn.Enabled = isEnable
    End Sub

    Private Sub saveBusReport_Click(sender As Object, e As EventArgs) Handles saveBusReportBtn.Click
        UseWaitCursor = True
        If ensureValidReportFromAndToDates() Then
            Dim serv As BusinessReportService = New BusinessReportService(Me)
            Dim businessReport As BusinessReportDAO.BusinessReport = loadBusinessReportDataFromUI()
            enableButtons(False)
            If (serv.saveBusinessReport(businessReport)) Then
                ' acceptChanges()
                reportViewedStateChanged()
                refreshBusinessReportsAvailComboBox(businessReport.absXmlFileName)
                isModified = False
                MessageBox.Show(Me, "saved")
            End If
            enableButtons(True)
        End If
        UseWaitCursor = False
    End Sub

    Protected Function getBusinessReportFileSelected() As ReportFile
        Return If(reportsAvailComboBox.SelectedIndex < 0, Nothing, busReports(reportsAvailComboBox.SelectedIndex))
    End Function

    Public Function isReportSubmitted() As Boolean
        Dim fileSel As ReportFile = getBusinessReportFileSelected()
        Return If(IsNothing(fileSel), False, fileSel.isSubmitted())
    End Function

    Public Function isReportLoaded() As Boolean
        Dim fileSel As ReportFile = getBusinessReportFileSelected()
        Return If(IsNothing(fileSel), False, fileSel.isLoaded())
    End Function

    Public Function isReportToLoad() As Boolean
        Dim fileSel As ReportFile = getBusinessReportFileSelected()
        Return If(IsNothing(fileSel), False, fileSel.isToLoad())
    End Function

    Public Function isReportToSubmit() As Boolean
        Dim fileSel As ReportFile = getBusinessReportFileSelected()
        Return If(IsNothing(fileSel), False, fileSel.isToSubmit())
    End Function

    Public Function isAnAmendementReport()
        Return reptType = ReportType.amendment_report
    End Function

    Public Function isBusinessReport()
        Return reptType = ReportType.business_report
    End Function

    Protected Sub reportViewedStateChanged()
        If isReportSubmitted() Then
            Me.BackColor = Color.FromArgb(0, 210, 0)
        ElseIf isReportToSubmit() Then
            Me.BackColor = Color.FromArgb(147, 255, 147)
        ElseIf isReportLoaded() Then
            Me.BackColor = Color.FromArgb(255, 255, 100)
        ElseIf isReportToLoad() Then
            Me.BackColor = Color.FromArgb(255, 255, 227)
        ElseIf isReportToLoad() Then
            Me.BackColor = Color.FromArgb(239, 239, 239)
        Else
            Me.BackColor = Color.FromArgb(240, 240, 240)
        End If


        If isReportSubmitted() OrElse isReportLoaded() OrElse isReportToLoad() Then
            reportFromDateTxtBox.ReadOnly = True
            reportToDateTxtBox.ReadOnly = True
            cashBroughtFwdTextBox.ReadOnly = True
            maunuallyCountedCashTxtBox.ReadOnly = True

            salesGrdView.makeItReadOnly(True)
            purchasesGrdView.makeItReadOnly(True)
            expensesGrdView.makeItReadOnly(True)
            debtPaymentGrdView.makeItReadOnly(True)

            enableButtons(False)
            If isReportLoaded() Then
                reEmailBtn.Enabled = False
                validateBusReportBtn.Enabled = False
                commitBusReportBtn.Enabled = False
                commitBusReportBtn.Text = "load"
            ElseIf isReportToLoad() Then
                validateBusReportBtn.Enabled = True
                reEmailBtn.Enabled = False
                commitBusReportBtn.Enabled = True
                commitBusReportBtn.Text = "load"
            Else
                validateBusReportBtn.Enabled = False
                reEmailBtn.Enabled = True
                commitBusReportBtn.Enabled = False
                commitBusReportBtn.Text = "submit"
            End If

            MainForm.AppToolStripStatusLabel.Text = "Commmited"

        Else

            reportFromDateTxtBox.ReadOnly = False
            reportToDateTxtBox.ReadOnly = False
            cashBroughtFwdTextBox.ReadOnly = False
            maunuallyCountedCashTxtBox.ReadOnly = False

            salesGrdView.makeItReadOnly(False)
            purchasesGrdView.makeItReadOnly(False)
            expensesGrdView.makeItReadOnly(False)
            debtPaymentGrdView.makeItReadOnly(False)

            enableButtons(True)

            reEmailBtn.Enabled = False
            MainForm.AppToolStripStatusLabel.Text = "Not Commmited"

        End If

        If (isAnAmendementReport()) Then
            reportFromDateTxtBox.ReadOnly = True
            reportToDateTxtBox.ReadOnly = True
            cashBroughtFwdTextBox.ReadOnly = True
            maunuallyCountedCashTxtBox.ReadOnly = True
        End If

    End Sub

    Protected Sub acceptChanges()
        isModified = False
        productsEntityDataSet.AcceptChanges()
        expenseCategoriesEntityDataSet.AcceptChanges()
        suppliersEntityDataSet.AcceptChanges()
    End Sub

    Private Sub commitBusReportBtn_Click(sender As Object, e As EventArgs) Handles commitBusReportBtn.Click
        If (Not validateBusinessReportsComponents()) Then
            Exit Sub
        End If

        Dim affectedPrdsQtyAndAcb As DataTable
        Dim isCommited As Boolean = False
        Dim commitMesg As String = If(isReportToLoad(), "Load", "Submit")
        Dim serv As BusinessReportService = New BusinessReportService(Me)
        Dim businessReport As BusinessReportDAO.BusinessReport = loadBusinessReportDataFromUI()
        If Not businessReport.isEmpty() Then
            If MessageBox.Show(Me, "Do want to " + commitMesg + " the report?", commitMesg + " the Report?", MessageBoxButtons.YesNo) = DialogResult.No Then
                Exit Sub
            End If
            UseWaitCursor = True
            enableButtons(False)
            MainForm.showProgress(0, "Committing Business Report.  Preparing...")
            If (isReportToLoad()) Then
                changeBusinessReportsToRetStatus(ReportFile.BusinessReportStatus.to_load)
                Dim businessReportFileSel As ReportFile = getBusinessReportFileSelected()
                If Not IsNothing(businessReportFileSel) Then
                    businessReport.absXmlFileName = businessReportFileSel.getAbsFileName
                    isCommited = serv.loadBusinessReport(businessReport)
                Else
                    MessageBox.Show(Me, "Error. No file selected to load")
                End If
            Else
                changeBusinessReportsToRetStatus(ReportFile.BusinessReportStatus.to_submit)
                isCommited = serv.submitBusinessReport(businessReport, affectedPrdsQtyAndAcb)
            End If

            If (isCommited) Then
                acceptChanges()
                refreshPostedTranDateDRange()
                If (Not businessReport.isAnAmendmentToPostedTrans AndAlso Not IsNothing(affectedPrdsQtyAndAcb)) Then
                    Dim dlg As New SubmittedProdsAcbAndQtyDlg(affectedPrdsQtyAndAcb)
                    dlg.ShowDialog()
                End If
                changeBusinessReportsToRetStatus(If(businessReportsToRetStatus = ReportFile.BusinessReportStatus.to_load, ReportFile.BusinessReportStatus.loaded, ReportFile.BusinessReportStatus.submitted))
            Else
                MessageBox.Show(Me, "Failed to commit report")
            End If
            refreshBusinessReportsAvailComboBox(businessReport.absXmlFileName)
            reportViewedStateChanged()
            UseWaitCursor = False
            MainForm.showProgress()

        Else
            MessageBox.Show(Me, "Report is empty.  Nothing to commit")
        End If

    End Sub

    Private Function lookUpDTRow(code As String, codeColName As String, dt As DataTable) As DataRow
        Dim colVal As String
        For r As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(r).RowState = DataRowState.Deleted Then
                colVal = dt.Rows(r)(codeColName, DataRowVersion.Original).ToString
            Else
                colVal = dt.Rows(r).Item(codeColName)
            End If
            If (code = colVal) Then
                Return dt.Rows(r)
            End If
        Next
        Return Nothing
    End Function


    Private Function loadEntitiesChangedIntoDataTable(busRprt As BusinessReportDAO.BusinessReport)
        Dim prod As BusinessReportDAO.ProductModifiedVO
        Dim exp As BusinessReportDAO.ExpenseCategoryModifiedVO
        Dim supplier As BusinessReportDAO.SupplierModifiedVO

        Dim tableRow As DataRow
        ' Dim dtRowInd As Integer

        Dim dataTable As DataTable
        Dim entityVO As BusinessReportDAO.BusinessReportEntityVO
        Dim codeColName As String

        Dim entityCollection As Collection

        For type As Integer = 1 To 3

            Select Case type
                Case 1
                    dataTable = productsEntityDataSet
                    entityCollection = busRprt.productsModified
                    codeColName = "product_code"
                Case 2
                    dataTable = expenseCategoriesEntityDataSet
                    entityCollection = busRprt.expenseCategoriesModified
                    codeColName = "expense_category_code"
                Case 3
                    dataTable = suppliersEntityDataSet
                    entityCollection = busRprt.suppliersModified
                    codeColName = "supplier_code"
            End Select

            If IsNothing(entityCollection) Then
                Continue For
            End If

            For r As Integer = 1 To entityCollection.Count
                entityVO = entityCollection(r)
                tableRow = Nothing
                Select Case entityVO.modType
                    Case BusinessReportDAO.ReportTypeEntityModType.delete
                        tableRow = lookUpDTRow(entityVO.code, codeColName, dataTable)
                        'If Not IsNothing(dtRowInd) Then
                        '    tableRow = dataTable.Rows(dtRowInd)
                        'End If
                    Case BusinessReportDAO.ReportTypeEntityModType.add
                        tableRow = dataTable.Rows.Add()
                    Case BusinessReportDAO.ReportTypeEntityModType.update
                        tableRow = lookUpDTRow(entityVO.code, codeColName, dataTable)
                        'If Not IsNothing(dtRowInd) Then
                        '    tableRow = dataTable.Rows(dtRowInd)
                        'End If
                End Select

                If IsNothing(tableRow) Then
                    Continue For
                End If

                If entityVO.modType = BusinessReportDAO.ReportTypeEntityModType.delete Then
                    tableRow.Delete()
                ElseIf entityVO.modType = BusinessReportDAO.ReportTypeEntityModType.update OrElse entityVO.modType = BusinessReportDAO.ReportTypeEntityModType.add Then
                    Select Case type
                        Case 1
                            prod = entityVO
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("product_code")) = prod.code
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("product_name")) = prod.name
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("qty_uom_name")) = prod.qtyUOM
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("product_category_name")) = prod.catCodeName
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("qty_available")) = prod.qtyAvail
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("acb_cost")) = prod.acbCost
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("retail_discount_room_percentage")) = prod.retDiscRoomPercentage
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("min_ret_gross_profit_margin_percentage")) = prod.minRetGrossProfitMarginPercentage
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("retail_sale_price")) = prod.retSalePrice
                            tableRow.Item(productsEntityDataSet.Columns.IndexOf("comments")) = prod.comments
                        Case 2
                            exp = entityVO
                            tableRow.Item(expenseCategoriesEntityDataSet.Columns.IndexOf("expense_category_code")) = exp.code
                            tableRow.Item(expenseCategoriesEntityDataSet.Columns.IndexOf("expense_category_name")) = exp.name
                            tableRow.Item(expenseCategoriesEntityDataSet.Columns.IndexOf("expense_category_description")) = exp.desc
                        Case 3
                            supplier = entityVO
                            tableRow.Item(suppliersEntityDataSet.Columns.IndexOf("supplier_code")) = supplier.code
                            tableRow.Item(suppliersEntityDataSet.Columns.IndexOf("supplier_name")) = supplier.name
                            tableRow.Item(suppliersEntityDataSet.Columns.IndexOf("comments")) = supplier.comments
                    End Select
                End If

            Next
        Next

    End Function

    Private Function loadProductsEntitiesChangedIntoVO(busRprt As BusinessReportDAO.BusinessReport)
        Dim vo As BusinessReportDAO.ProductModifiedVO
        Dim dt As DataTable
        Dim i As Integer
        Dim changeType As BusinessReportDAO.ReportTypeEntityModType

        busRprt.productsModified = New Collection
        For changeType = BusinessReportDAO.ReportTypeEntityModType.add To BusinessReportDAO.ReportTypeEntityModType.update
            Select Case changeType
                Case BusinessReportDAO.ReportTypeEntityModType.add
                    dt = productsEntityDataSet.GetChanges(DataRowState.Added)
                Case BusinessReportDAO.ReportTypeEntityModType.delete
                    dt = productsEntityDataSet.GetChanges(DataRowState.Deleted)
                Case BusinessReportDAO.ReportTypeEntityModType.update
                    dt = productsEntityDataSet.GetChanges(DataRowState.Modified)
            End Select

            If IsNothing(dt) Then
                Continue For
            End If

            For i = 0 To dt.Rows.Count - 1
                vo = New BusinessReportDAO.ProductModifiedVO
                vo.modType = changeType
                vo.id = -1
                If changeType = BusinessReportDAO.ReportTypeEntityModType.delete Then
                    vo.id = UIUtil.subsIfEmpty(dt.Rows(i)("product_id", DataRowVersion.Original), -1)
                    vo.code = dt.Rows(i)("product_code", DataRowVersion.Original).ToString
                    vo.name = dt.Rows(i)("product_name", DataRowVersion.Original).ToString
                    vo.qtyUOM = dt.Rows(i)("qty_uom_name", DataRowVersion.Original).ToString
                    vo.catCodeName = dt.Rows(i)("product_category_name", DataRowVersion.Original).ToString
                    vo.qtyAvail = UIUtil.zeroIfEmpty(dt.Rows(i)("qty_available", DataRowVersion.Original))
                    vo.acbCost = UIUtil.zeroIfEmpty(dt.Rows(i)("acb_cost", DataRowVersion.Original))
                    vo.retDiscRoomPercentage = UIUtil.zeroIfEmpty(dt.Rows(i)("retail_discount_room_percentage", DataRowVersion.Original))
                    vo.minRetGrossProfitMarginPercentage = UIUtil.zeroIfEmpty(dt.Rows(i)("min_ret_gross_profit_margin_percentage", DataRowVersion.Original))
                    vo.retSalePrice = UIUtil.zeroIfEmpty(dt.Rows(i)("retail_sale_price", DataRowVersion.Original))
                    vo.comments = dt.Rows(i)("comments", DataRowVersion.Original).ToString
                Else
                    vo.id = UIUtil.subsIfEmpty(dt.Rows(i).Item(dt.Columns.IndexOf("product_id")), -1)
                    vo.code = dt.Rows(i).Item(dt.Columns.IndexOf("product_code")).ToString
                    vo.name = dt.Rows(i).Item(dt.Columns.IndexOf("product_name")).ToString
                    vo.qtyUOM = dt.Rows(i).Item(dt.Columns.IndexOf("qty_uom_name")).ToString
                    vo.catCodeName = dt.Rows(i).Item(dt.Columns.IndexOf("product_category_name")).ToString
                    vo.qtyAvail = UIUtil.zeroIfEmpty(dt.Rows(i).Item(dt.Columns.IndexOf("qty_available")))
                    vo.acbCost = UIUtil.zeroIfEmpty(dt.Rows(i).Item(dt.Columns.IndexOf("acb_cost")))
                    vo.retDiscRoomPercentage = UIUtil.zeroIfEmpty(dt.Rows(i).Item(dt.Columns.IndexOf("retail_discount_room_percentage")))
                    vo.minRetGrossProfitMarginPercentage = UIUtil.zeroIfEmpty(dt.Rows(i).Item(dt.Columns.IndexOf("min_ret_gross_profit_margin_percentage")))
                    vo.retSalePrice = UIUtil.zeroIfEmpty(dt.Rows(i).Item(dt.Columns.IndexOf("retail_sale_price")))
                    vo.comments = dt.Rows(i).Item(dt.Columns.IndexOf("comments")).ToString
                End If
                busRprt.productsModified.Add(vo)
            Next
        Next

    End Function

    Private Function loadExpenseCategoriesEntitiesChangedIntoVO(busRprt As BusinessReportDAO.BusinessReport)

        Dim vo As BusinessReportDAO.ExpenseCategoryModifiedVO
        Dim dt As DataTable
        Dim i As Integer

        busRprt.expenseCategoriesModified = New Collection
        For changeType = BusinessReportDAO.ReportTypeEntityModType.add To BusinessReportDAO.ReportTypeEntityModType.update
            Select Case changeType
                Case BusinessReportDAO.ReportTypeEntityModType.add
                    dt = expenseCategoriesEntityDataSet.GetChanges(DataRowState.Added)
                Case BusinessReportDAO.ReportTypeEntityModType.delete
                    dt = expenseCategoriesEntityDataSet.GetChanges(DataRowState.Deleted)
                Case BusinessReportDAO.ReportTypeEntityModType.update
                    dt = expenseCategoriesEntityDataSet.GetChanges(DataRowState.Modified)
            End Select
            If IsNothing(dt) Then
                Continue For
            End If
            For i = 0 To dt.Rows.Count - 1
                vo = New BusinessReportDAO.ExpenseCategoryModifiedVO
                vo.modType = changeType
                vo.id = -1
                If changeType = BusinessReportDAO.ReportTypeEntityModType.delete Then
                    vo.id = UIUtil.subsIfEmpty(dt.Rows(i)("expense_category_id", DataRowVersion.Original), -1)
                    vo.code = dt.Rows(i)("expense_category_code", DataRowVersion.Original).ToString
                    vo.name = dt.Rows(i)("expense_category_name", DataRowVersion.Original).ToString
                    vo.desc = dt.Rows(i)("expense_category_description", DataRowVersion.Original).ToString
                Else
                    vo.id = UIUtil.subsIfEmpty(dt.Rows(i).Item(dt.Columns.IndexOf("expense_category_id")), -1)
                    vo.code = dt.Rows(i).Item(dt.Columns.IndexOf("expense_category_code")).ToString
                    vo.name = dt.Rows(i).Item(dt.Columns.IndexOf("expense_category_name")).ToString
                    vo.desc = dt.Rows(i).Item(dt.Columns.IndexOf("expense_category_description")).ToString
                End If
                busRprt.expenseCategoriesModified.Add(vo)
            Next
        Next

    End Function

    Private Function loadSuppliersEntitiesChangedIntoVO(busRprt As BusinessReportDAO.BusinessReport)
        Dim vo As BusinessReportDAO.SupplierModifiedVO
        Dim dt As DataTable = Nothing
        Dim i As Integer

        busRprt.suppliersModified = New Collection
        For changeType = BusinessReportDAO.ReportTypeEntityModType.add To BusinessReportDAO.ReportTypeEntityModType.update
            Select Case changeType
                Case BusinessReportDAO.ReportTypeEntityModType.add
                    dt = suppliersEntityDataSet.GetChanges(DataRowState.Added)
                Case BusinessReportDAO.ReportTypeEntityModType.delete
                    dt = suppliersEntityDataSet.GetChanges(DataRowState.Deleted)
                Case BusinessReportDAO.ReportTypeEntityModType.update
                    dt = suppliersEntityDataSet.GetChanges(DataRowState.Modified)
            End Select
            If IsNothing(dt) Then
                Continue For
            End If
            For i = 0 To dt.Rows.Count - 1
                vo = New BusinessReportDAO.SupplierModifiedVO
                vo.modType = changeType
                vo.id = -1

                If changeType = BusinessReportDAO.ReportTypeEntityModType.delete Then
                    vo.id = UIUtil.subsIfEmpty(dt.Rows(i)("supplier_id", DataRowVersion.Original), -1)
                    vo.code = dt.Rows(i)("supplier_code", DataRowVersion.Original).ToString
                    vo.name = dt.Rows(i)("supplier_name", DataRowVersion.Original).ToString
                    vo.comments = dt.Rows(i)("comments", DataRowVersion.Original).ToString
                Else
                    vo.id = UIUtil.subsIfEmpty(dt.Rows(i).Item(dt.Columns.IndexOf("supplier_id")), -1)
                    vo.code = dt.Rows(i).Item(dt.Columns.IndexOf("supplier_code")).ToString
                    vo.name = dt.Rows(i).Item(dt.Columns.IndexOf("supplier_name")).ToString
                    vo.comments = dt.Rows(i).Item(dt.Columns.IndexOf("comments")).ToString
                End If
                busRprt.suppliersModified.Add(vo)
            Next
        Next

    End Function

    Public Function getBusinessReportToFromDates(ByRef fromDate As Date, ByRef toDate As Date)

        If ensureValidReportFromAndToDates() Then
            fromDate = UIUtil.parseDate(reportFromDateTxtBox.Text)
            toDate = UIUtil.parseDate(reportToDateTxtBox.Text)
            Return True
        Else
            Return False
        End If
    End Function


    Private Function loadBusinessReportDataFromUI() As BusinessReportDAO.BusinessReport
        Dim businessReport As New BusinessReportDAO.BusinessReport

        businessReport.isAnAmendmentToPostedTrans = (reptType = ReportType.amendment_report)
        getBusinessReportToFromDates(businessReport.fromDate, businessReport.toDate)

        businessReport.cashBroughtForward = UIUtil.parseDouble(cashBroughtFwdTextBox.Text)
        businessReport.cashCounted = UIUtil.parseDouble(maunuallyCountedCashTxtBox.Text)

        loadProductsEntitiesChangedIntoVO(businessReport)
        loadExpenseCategoriesEntitiesChangedIntoVO(businessReport)
        loadSuppliersEntitiesChangedIntoVO(businessReport)

        salesGrdView.loadBusinessReportDataFromGrid(businessReport)
        purchasesGrdView.loadBusinessReportDataFromGrid(businessReport)
        expensesGrdView.loadBusinessReportDataFromGrid(businessReport)
        debtPaymentGrdView.loadBusinessReportDataFromGrid(businessReport)

        loadBusinessReportDataFromUI = businessReport
    End Function


    Private Sub loadBusinessReportDataIntoUI(businessReport As BusinessReportDAO.BusinessReport)
        populateDatasetsFromDB()

        cashBroughtFwdTextBox.Text = UIUtil.toAmtString(businessReport.cashBroughtForward)
        maunuallyCountedCashTxtBox.Text = UIUtil.toAmtString(businessReport.cashCounted)

        loadEntitiesChangedIntoDataTable(businessReport)
        salesGrdView.loadBusinessReportDataIntoGrid(businessReport)
        purchasesGrdView.loadBusinessReportDataIntoGrid(businessReport)
        expensesGrdView.loadBusinessReportDataIntoGrid(businessReport)
        debtPaymentGrdView.loadBusinessReportDataIntoGrid(businessReport)
    End Sub

    Private Sub wipeOutEnteredData(Optional isDefaultToFromBusinessDates As Boolean = False)
        Me.reportFromDateTxtBox.Text = ""
        Me.reportFromDateTxtBox.ReadOnly = False
        Me.reportToDateTxtBox.Text = ""
        Me.reportToDateTxtBox.ReadOnly = False

        salesGrdView.clearAllEntries()
        purchasesGrdView.clearAllEntries()
        expensesGrdView.clearAllEntries()
        debtPaymentGrdView.clearAllEntries()

        populateDatasetsFromDB()
        reportViewedStateChanged()

        'clear summary fields
        salesTxtBox.Text = ""
        totProfitsTxtBox.Text = ""
        expensesTxtBox.Text = ""
        debtPaymentTxtBox.Text = ""
        cashPurchasesTxtBox.Text = ""
        creditPurchasesTxtBox.Text = ""
        totProfitsTxtBox.Text = ""
        expectedCashFlowTxtBox.Text = ""
        cashBroughtFwdTextBox.Text = ""
        maunuallyCountedCashTxtBox.Text = ""


        If (isDefaultToFromBusinessDates) Then
            setUIDateRange(Today(), Today())
        End If

        fromDateErrorPic.setErrorText("")
        toDateErrorPic.setErrorText("")
        cashBroughtForwardErrorPic.setErrorText("")
        countedCashErrorPic.setErrorText("")


        Me.isModified = False
    End Sub

    Private Sub deleteReportBtn_Click(sender As Object, e As EventArgs) Handles deleteReportBtn.Click
        If reportsAvailComboBox.SelectedIndex < 0 Then
            MessageBox.Show(Me, "Select Report to Delete")
            Return
        End If

        Dim businessReportFileSel As ReportFile = getBusinessReportFileSelected()
        If Not IsNothing(businessReportFileSel) Then
            If businessReportFileSel.isSubmitted Then
                MessageBox.Show(Me, "Submitted file cannot be deleted")
            ElseIf MessageBox.Show(Me, "Do you wish to delete business report ?", "Delete Report", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Dim fileToDel As String = MiscUtil.appendPathComponent(businessReportFileSel.folderName, businessReportFileSel.fileName)
                Try
                    My.Computer.FileSystem.DeleteFile(fileToDel)
                Catch ex As System.IO.FileNotFoundException
                    ' ignore
                End Try
                wipeOutEnteredData()
                refreshBusinessReportsAvailComboBox(Nothing)
                reportViewedStateChanged()
            End If
        Else
            MessageBox.Show("Error no file to delete selected")
        End If
    End Sub

    Private Function isReportModified() As Boolean
        Return isModified
    End Function

    Public Sub recalcSummary()

        If (isDlgLoading()) Then
            Return
        End If

        UseWaitCursor = True
        Dim totSales As Double
        Dim totProfits As Double

        Dim cashPurchases As Double
        Dim creditPurchases As Double

        salesGrdView.getSaleTotals(totSales, totProfits)
        purchasesGrdView.getPurchaseTotals(cashPurchases, creditPurchases)

        creditPurchasesTxtBox.Text = FormatNumber(-1 * creditPurchases, 0, TriState.True, TriState.False, True)
        totProfitsTxtBox.Text = FormatNumber(totProfits, 0, TriState.True, TriState.False, True)

        salesTxtBox.Text = FormatNumber(totSales, 0, TriState.True, TriState.False, True)
        expensesTxtBox.Text = FormatNumber(-1 * expensesGrdView.getTotalExpenses(), 0, TriState.True, TriState.False, True)
        cashPurchasesTxtBox.Text = FormatNumber(-1 * cashPurchases, 0, TriState.True, TriState.False, True)
        debtPaymentTxtBox.Text = FormatNumber(-1 * debtPaymentGrdView.getTotalDebtPaid(), 0, TriState.True, TriState.False, True)

        colorNegNumber(salesTxtBox)
        colorNegNumber(totProfitsTxtBox)
        colorNegNumber(expensesTxtBox)
        colorNegNumber(debtPaymentTxtBox)

        colorNegNumber(cashOutflowTxtBox)
        colorNegNumber(cashPurchasesTxtBox)
        colorNegNumber(creditPurchasesTxtBox)
        colorNegNumber(expensesTxtBox)

        isModified = True
        UseWaitCursor = False
    End Sub

    Protected Sub getTranDates(ByRef minDate As Date, ByRef maxDate As Date)
        minDate = New Date(9999, 1, 1)
        maxDate = New Date(1970, 1, 1)
        Dim tranMinDate As Date
        Dim tranMaxDate As Date

        salesGrdView.getTransactionDates(tranMinDate, tranMaxDate)
        If tranMaxDate > maxDate Then
            maxDate = tranMaxDate
        End If
        If tranMinDate < minDate Then
            minDate = tranMinDate
        End If
        expensesGrdView.getTransactionDates(tranMinDate, tranMaxDate)
        If tranMaxDate > maxDate Then
            maxDate = tranMaxDate
        End If
        If tranMinDate < minDate Then
            minDate = tranMinDate
        End If
        purchasesGrdView.getTransactionDates(tranMinDate, tranMaxDate)
        If tranMaxDate > maxDate Then
            maxDate = tranMaxDate
        End If
        If tranMinDate < minDate Then
            minDate = tranMinDate
        End If
        If tranMinDate < minDate Then
            minDate = tranMinDate
        End If
        debtPaymentGrdView.getTransactionDates(tranMinDate, tranMaxDate)
        If tranMaxDate > maxDate Then
            maxDate = tranMaxDate
        End If
        If tranMinDate < minDate Then
            minDate = tranMinDate
        End If


    End Sub

    Private Sub recalcCashOutflow(sender As Object, e As EventArgs) Handles salesTxtBox.TextChanged, expensesTxtBox.TextChanged, debtPaymentTxtBox.TextChanged, cashPurchasesTxtBox.TextChanged
        If (isDlgLoading()) Then
            Return
        End If

        Dim tot As Double = 0
        tot += UIUtil.removeNumberFormating(salesTxtBox.Text)
        tot += UIUtil.removeNumberFormating(expensesTxtBox.Text)
        tot += UIUtil.removeNumberFormating(debtPaymentTxtBox.Text)
        tot += UIUtil.removeNumberFormating(cashPurchasesTxtBox.Text)
        cashOutflowTxtBox.Text = FormatNumber(tot, 0, TriState.True, TriState.False, True)
    End Sub

    Private Sub colorNegNumber(txtBox As TextBox)
        If (UIUtil.zeroIfEmptyStr(txtBox.Text) < 0) Then
            txtBox.ForeColor = Color.Red
        Else
            txtBox.ForeColor = Color.Black
        End If
    End Sub
    Private Sub recalcExpectedCash(sender As Object, e As EventArgs) Handles cashBroughtFwdTextBox.TextChanged, cashOutflowTxtBox.TextChanged
        If (isDlgLoading()) Then
            Return
        End If

        Dim cashExp As Double = UIUtil.removeNumberFormating(cashOutflowTxtBox.Text) + UIUtil.removeNumberFormating(cashBroughtFwdTextBox.Text)
        expectedCashFlowTxtBox.Text = FormatNumber(cashExp, 0, TriState.True, TriState.False, True)
        colorNegNumber(expectedCashFlowTxtBox)
    End Sub

    Private Sub recalcCashDifference(sender As Object, e As EventArgs) Handles expectedCashFlowTxtBox.TextChanged, maunuallyCountedCashTxtBox.TextChanged
        If (isDlgLoading()) Then
            Return
        End If

        Dim cashDiff As Double = UIUtil.removeNumberFormating(maunuallyCountedCashTxtBox.Text) - UIUtil.removeNumberFormating(expectedCashFlowTxtBox.Text)
        countedVersusExpCashTxtBox.Text = FormatNumber(cashDiff, 0, TriState.True, TriState.False, True)
        colorNegNumber(countedVersusExpCashTxtBox)
    End Sub

    Private Sub cashBroughtFwdTextBox_Enter(sender As Object, e As EventArgs) Handles cashBroughtFwdTextBox.Enter
        If (isDlgLoading()) Then
            Return
        End If
        If (Not UIUtil.isEmpty(cashBroughtFwdTextBox.Text)) Then
            cashBroughtFwdTextBox.Text = UIUtil.removeNumberFormating(cashBroughtFwdTextBox.Text)
            colorNegNumber(cashBroughtFwdTextBox)
        End If

    End Sub

    Private Sub cashBroughtFwdTextBox_Leave(sender As Object, e As EventArgs) Handles cashBroughtFwdTextBox.Leave
        If (isDlgLoading()) Then
            Return
        End If

        cashBroughtFwdTextBox.Text = FormatNumber(Double.Parse(UIUtil.removeNumberFormating(cashBroughtFwdTextBox.Text)), 0, TriState.True, TriState.False, True)
        colorNegNumber(cashBroughtFwdTextBox)

    End Sub

    Private Sub maunuallyCountedCashTxtBox_Enter(sender As Object, e As EventArgs) Handles maunuallyCountedCashTxtBox.Enter
        If (isDlgLoading()) Then
            Return
        End If

        If (Not UIUtil.isEmpty(maunuallyCountedCashTxtBox.Text)) Then
            maunuallyCountedCashTxtBox.Text = UIUtil.removeNumberFormating(maunuallyCountedCashTxtBox.Text)
            colorNegNumber(maunuallyCountedCashTxtBox)
        End If

    End Sub

    Private Sub maunuallyCountedCashTxtBox_Leave(sender As Object, e As EventArgs) Handles maunuallyCountedCashTxtBox.Leave
        If (isDlgLoading()) Then
            Return
        End If


        maunuallyCountedCashTxtBox.Text = FormatNumber(Double.Parse(UIUtil.removeNumberFormating(maunuallyCountedCashTxtBox.Text)), 0, TriState.True, TriState.False, True)
        colorNegNumber(maunuallyCountedCashTxtBox)
    End Sub

    Private Sub insertTranRowBtn_Click(sender As Object, e As EventArgs) Handles insertTranRowBtn.Click
        Dim tabPageSel As TabPage = businessReportTabControl.SelectedTab
        If tabPageSel.Name = "salesReportTab" Then
            salesGrdView.insertRowAtCurrentPos()
        ElseIf tabPageSel.Name = "purchasesReportTab" Then
            purchasesGrdView.insertRowAtCurrentPos()
        ElseIf tabPageSel.Name = "expensesReportTab" Then
            expensesGrdView.insertRowAtCurrentPos()
        ElseIf tabPageSel.Name = "debtPaymentsReportTab" Then
            debtPaymentGrdView.insertRowAtCurrentPos()
        End If
    End Sub

    Private Sub deleteTranRowBtn_Click(sender As Object, e As EventArgs) Handles deleteTranRowBtn.Click
        Dim tabPageSel As TabPage = businessReportTabControl.SelectedTab
        If tabPageSel.Name = "salesReportTab" Then
            salesGrdView.deleteCurrentRow()
        ElseIf tabPageSel.Name = "purchasesReportTab" Then
            purchasesGrdView.deleteCurrentRow()
        ElseIf tabPageSel.Name = "expensesReportTab" Then
            expensesGrdView.deleteCurrentRow()
        ElseIf tabPageSel.Name = "debtPaymentsReportTab" Then
            debtPaymentGrdView.deleteCurrentRow()
        End If
    End Sub

    Private Sub reportFromDateTxtBox_TextChanged(sender As Object, e As EventArgs) Handles reportFromDateTxtBox.TextChanged
        If (isDlgLoading()) Then
            Return
        End If
        isModified = True
    End Sub

    Private Sub reportToDateTxtBox_TextChanged(sender As Object, e As EventArgs) Handles reportToDateTxtBox.TextChanged
        If (isDlgLoading()) Then
            Return
        End If
        isModified = True
    End Sub


    Private Sub reportFromDateTxtBox_Leave(sender As Object, e As EventArgs) Handles reportFromDateTxtBox.Leave
        If UIUtil.isValidDate(reportFromDateTxtBox.Text) Then
            reportFromDateTxtBox.Text = UIUtil.toDateString(UIUtil.parseDate(reportFromDateTxtBox.Text))
        End If
    End Sub

    Private Sub reportToDateTxtBox_Leave(sender As Object, e As EventArgs) Handles reportToDateTxtBox.Leave
        If UIUtil.isValidDate(reportToDateTxtBox.Text) Then
            reportToDateTxtBox.Text = UIUtil.toDateString(UIUtil.parseDate(reportToDateTxtBox.Text))
        End If
    End Sub

    Private Sub BusinessReportDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = Not ensureUserNotifiedOfModifiedReport()
        ' e.Cancel = Not (Not isReportModified() OrElse MessageBox.Show(Me, "Report has been modified.  If you continue, changes will be lost. Continue ?", "Close Report", MessageBoxButtons.YesNo) = DialogResult.Yes)
    End Sub

    Private Sub reEmailBtn_Click(sender As Object, e As EventArgs) Handles reEmailBtn.Click
        If Not isReportSubmitted() Then
            MessageBox.Show(Me, "Only submitted reports can be re-emailed")
        Else
            If MessageBox.Show(Me, "Do you wish to re-email the submitted report?", "Email Report", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Dim serv As BusinessReportService = New BusinessReportService(Me)
                Dim fromDate As Date
                Dim toDate As Date
                MainForm.showProgress(-1, "Preparing to send email...")
                If (getBusinessReportToFromDates(fromDate, toDate)) Then
                    Dim fileSel As ReportFile = getBusinessReportFileSelected()
                    MainForm.showProgress(50, "Sending email...")
                    If (serv.emailBusinessReport(fromDate, toDate, fileSel.getAbsFileName())) Then
                        MessageBox.Show(Me, "Report Emailed")
                    End If
                    MainForm.showProgress()
                End If

            End If
        End If

    End Sub

    Private Sub showPostedAmendedRecsChkBox_CheckedChanged(sender As Object, e As EventArgs) Handles showPostedAmendedRecsChkBox.CheckedChanged
        salesGrdView.Refresh()
        purchasesGrdView.Refresh()
        expensesGrdView.Refresh()
        debtPaymentGrdView.Refresh()
    End Sub


End Class