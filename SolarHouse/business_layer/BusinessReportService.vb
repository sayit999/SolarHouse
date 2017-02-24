Imports System.IO


Public Class BusinessReportService
    Dim parent As System.Windows.Forms.Form

    Public Sub New(parent As System.Windows.Forms.Form)
        Me.parent = parent
    End Sub

    Public Function runMySqlScript(sqlScriptAbsPath As String) As Boolean
        Dim businessReportDAO As New BusinessReportDAO
        Return businessReportDAO.runMySqlScript(sqlScriptAbsPath)
    End Function

    Public Function saveBusinessReport(ByRef businessReport As BusinessReportDAO.BusinessReport) As Boolean

        Try
            businessReport.absXmlFileName = ReportFile.getXMLFileNameToSubmit(businessReport.fromDate, businessReport.toDate, ReportFile.getToSubmitFolderName(), businessReport.isAnAmendmentToPostedTrans)
            Dim businessReportDAO As New BusinessReportDAO
            businessReportDAO.saveBusinessReportAsXML(businessReport)
            Return True

        Catch ex As Exception
            MessageBox.Show(Me, "Failed to save Business Report.  Reason:" + ex.ToString)
            Return False
        End Try

    End Function

    Public Function emailDBBackup(absBackupFileName As String)
        Try
            Dim serv As New EmailService
            serv.emailFile("Database Backup", My.Settings.email_xml_to, absBackupFileName)
            Return True
        Catch ex As System.Net.Mail.SmtpException
            MessageBox.Show(parent, "No internet/Hamna Mtandao.  Hakikisha Modem imeingizwa kwenye computer.  Jaribu tena")
            Return False
        Catch ex As Exception
            MessageBox.Show(parent, "Failed to Email Backup.  Reason:" + ex.ToString)
            Return False
        End Try

    End Function

    Public Function emailBusinessReport(absXmlFileName As String)
        Try
            Dim businessReportDAO As New BusinessReportDAO
            businessReportDAO.emailReport(absXmlFileName)
            Return True
        Catch ex As System.Net.Mail.SmtpException
            MessageBox.Show(parent, "No internet/Hamna Mtandao.  Hakikisha Modem imeingizwa kwenye computer.  Jaribu tena")
            Return False
        Catch ex As Exception
            MessageBox.Show(parent, "Failed to Email Business Report.  Reason:" + ex.ToString)
            Return False
        End Try

    End Function

    Public Sub getPostedTransactionDateRange(ByRef fromDate As Date, ByRef toDate As Date)
        Dim businessReportDAO As New BusinessReportDAO
        businessReportDAO.getPostedTransactionDateRange(fromDate, toDate)
    End Sub

    Public Function loadBusinessReport(ByRef businessReport As BusinessReportDAO.BusinessReport)
        Dim businessReportDAO As New BusinessReportDAO

        Try
            businessReportDAO.loadBusinessReport(businessReport)
            Return True
        Catch ex As Exception
            MessageBox.Show(parent, "Failed to load Business Report.  Reason:" + ex.ToString)
            Return False
        End Try


    End Function

    Public Function submitBusinessReport(ByRef businessReport As BusinessReportDAO.BusinessReport, ByRef affectedPrdsQtyAndAcb As DataTable)
        Dim businessReportDAO As New BusinessReportDAO

        Try
            businessReport.absXmlFileName = ReportFile.getXMLFileNameToSubmit(businessReport.fromDate, businessReport.toDate, ReportFile.getToSubmitFolderName(), businessReport.isAnAmendmentToPostedTrans)
            Dim unqPrdIdsTransacted As List(Of Integer) = businessReportDAO.submitBusinessReport(businessReport)
            affectedPrdsQtyAndAcb = businessReportDAO.lookupProdQtyAndAcb(unqPrdIdsTransacted)
            Return True
        Catch ex As System.Net.Mail.SmtpException
            MessageBox.Show(parent, "No internet/Hamna Mtandao.  Hakikisha Modem imeingizwa kwenye computer.  Jaribu tena")
            Return False
        Catch ex As Exception
            MessageBox.Show(parent, "Failed to submit Business Report.  Reason:" + ex.ToString)
            Return False
        End Try


    End Function

    Protected Function addPostedToAmendedTrans(postedTrans As Collection, amendedTrans As Collection) As Collection
        If IsNothing(amendedTrans) Then
            Return postedTrans
        ElseIf IsNothing(postedTrans) Then
            Return amendedTrans
        End If

        Dim postedTran As BusinessReportDAO.BusinessTransactionVO
        Dim amendedTran As BusinessReportDAO.BusinessTransactionVO
        Dim wasAdded As Boolean = False
        For r = 1 To amendedTrans.Count
            amendedTran = amendedTrans(r)
            For i = 1 To postedTrans.Count
                postedTran = postedTrans(i)
                wasAdded = False
                If postedTran.tranDate > amendedTran.tranDate Then
                    postedTrans.Add(amendedTran, Nothing, i)
                    wasAdded = True
                    Exit For
                End If
            Next
            If (Not wasAdded) Then
                postedTrans.Add(amendedTran)
            End If
        Next
        Return postedTrans
    End Function

    Public Function retrieveBusinessReport(businessReportFile As ReportFile) As BusinessReportDAO.BusinessReport
        Dim fileName As String = MiscUtil.appendPathComponent(businessReportFile.folderName, businessReportFile.fileName)
        If Not My.Computer.FileSystem.FileExists(fileName) Then
            MessageBox.Show(parent, "Cannot load report.  Report " + fileName + " does not exist")
            Return Nothing
        Else
            Try
                Dim businessReportDAO As New BusinessReportDAO
                Dim rprt As BusinessReportDAO.BusinessReport = businessReportDAO.readBusinessReportFromXML(fileName)
                If rprt.isAnAmendmentToPostedTrans AndAlso Not businessReportFile.isSubmitted Then
                    Dim postedTansRprt As BusinessReportDAO.BusinessReport = retrievePostedBusinessTransactions(rprt.fromDate, rprt.toDate)
                    rprt.sales = addPostedToAmendedTrans(postedTansRprt.sales, rprt.sales)
                    rprt.purchases = addPostedToAmendedTrans(postedTansRprt.purchases, rprt.purchases)
                    rprt.expenses = addPostedToAmendedTrans(postedTansRprt.expenses, rprt.expenses)
                    rprt.debtPayments = addPostedToAmendedTrans(postedTansRprt.debtPayments, rprt.debtPayments)
                End If
                Return rprt
            Catch ex As Exception
                MessageBox.Show(parent, "Failed to retrieve Report. Reason:" + ex.ToString)
                Return Nothing
            End Try

        End If
    End Function

    Public Function retrievePostedBusinessTransactions(fromdate As Date, toDate As Date) As BusinessReportDAO.BusinessReport
        Try
            Dim businessReportDAO As New BusinessReportDAO
            Return businessReportDAO.retrievePostedBusinessTransactions(fromdate, toDate)
        Catch ex As Exception
            MessageBox.Show(parent, "Failed to retrieve posted transactions.  Reason:" + ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Function recalcProductAcbAndQty()
        Try
            Dim businessReportDAO As New BusinessReportDAO
            Return businessReportDAO.recalcAllProductAcbAndQty()
        Catch ex As Exception
            MessageBox.Show(parent, "Failed to recalc Qty and ACB.  Reason:" + ex.ToString)
            Return False
        End Try

    End Function

    Public Sub calcAcbQtyAvail(transHist As List(Of BusinessReportDAO.ProdTransactionHistoryVO))
        Dim businessReportDAO As New BusinessReportDAO
        businessReportDAO.calcAcbQtyAvail(transHist)
    End Sub

    Public Function retrieveTransactionHistory(prodCode As String) As List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        Try
            Dim businessReportDAO As New BusinessReportDAO
            Dim prds As New List(Of String)
            prds.Add(prodCode)
            Return businessReportDAO.retreiveTransactionHistory(Nothing, prds)
        Catch ex As Exception
            MessageBox.Show(parent, "Failed to retrieve Transaction History.  Reason:" + ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Function retrieveLastReportSubmitted(reportTyp As BusinessReportDAO.ReportType) As BusinessReportDAO.ReportLoadSubmitStatusVO
        Try
            Dim businessReportDAO As New BusinessReportDAO
            Return businessReportDAO.retrieveLastReportSubmitted(reportTyp)
        Catch ex As Exception
            MessageBox.Show(parent, "Failed to retrieve Last Report Submitted.  Reason:" + ex.ToString)
            Return Nothing
        End Try
    End Function
End Class
