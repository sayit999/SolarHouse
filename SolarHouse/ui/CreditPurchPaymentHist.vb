Public Class CreditPurchPaymentHistDlg

    Protected serv As New BusinessReportService(Me)

    Protected supplierDt As DataTable = Nothing
    Protected supplierDebtPurhPaymentHistDt As DataTable = Nothing

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me.AutoSize = True

    End Sub

    Private Sub loadSupplierComboBx()

        serv.getSuppliers(False, supplierDt)
        For i As Integer = 0 To supplierDt.Rows.Count - 1
            supplierNameCmbBx.Items.Add(supplierDt.Rows(i).Item(1))
        Next


    End Sub

    Private Sub ProductTransHistoryDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadSupplierComboBx()

    End Sub


    Private Sub loadSupplierHist(suplrCode As String)
        serv.retrieveCrdPurDebtPymntHist(suplrCode, supplierDebtPurhPaymentHistDt)
        creditPurchPaymentHistoryGrdVw.DataSource = supplierDebtPurhPaymentHistDt
        Dim balance As Integer = 0
        For i As Integer = 0 To supplierDebtPurhPaymentHistDt.Rows.Count - 1
            ' amount
            balance += UIUtil.zeroIfEmpty(creditPurchPaymentHistoryGrdVw.Item(creditPurchPaymentHistoryGrdVw.Columns("amt").Index, i).Value)
            ' balance
            creditPurchPaymentHistoryGrdVw.Item(creditPurchPaymentHistoryGrdVw.Columns("balance").Index, i).Value = balance

            If creditPurchPaymentHistoryGrdVw.Item(creditPurchPaymentHistoryGrdVw.Columns("is_reversal").Index, i).Value = 1 Then
                creditPurchPaymentHistoryGrdVw.Rows(i).DefaultCellStyle.BackColor = Color.FromArgb(255, 175, 175)
            ElseIf creditPurchPaymentHistoryGrdVw.Item(creditPurchPaymentHistoryGrdVw.Columns("is_amendment").Index, i).Value = 1 Then
                creditPurchPaymentHistoryGrdVw.Rows(i).DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 128)
            ElseIf creditPurchPaymentHistoryGrdVw.Item(creditPurchPaymentHistoryGrdVw.Columns("tran_type").Index, i).Value = "Manunuzi" Then
                creditPurchPaymentHistoryGrdVw.Rows(i).DefaultCellStyle.BackColor = Color.FromArgb(202, 228, 255)
            Else
                creditPurchPaymentHistoryGrdVw.DefaultCellStyle.BackColor = Color.FromArgb(189, 255, 189)
            End If
        Next

    End Sub



    Private Sub lookupSupplierPaymentHistBtn_Click(sender As Object, e As EventArgs) Handles lookupSupplierPaymentHistBtn.Click
        loadSupplierHist(supplierDt.Rows(supplierNameCmbBx.SelectedIndex).Item(0))
    End Sub

End Class