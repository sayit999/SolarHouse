Public Class SubmittedProductAcbAndQtyDlg
    Protected prodCode As String
    Public Sub New(prodCode As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.prodCode = prodCode
        Me.AutoSize = True

    End Sub

    Private Sub ProductTransHistoryDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim serv As New BusinessReportService(Me)

        productTransHistoryListView.View = View.Details
        productTransHistoryListView.GridLines = False
        productTransHistoryListView.FullRowSelect = True
        productTransHistoryListView.BackColor = Color.FromArgb(244, 255, 234)


        Dim itemVals(10) As String
        Dim item As ListViewItem
        Dim lst As List(Of BusinessReportDAO.ProdTransactionHistoryVO) = serv.retrieveTransactionHistory(prodCode)

        If Not IsNothing(lst) Then
            For r As Integer = 0 To lst.Count - 1
                If StringUtil.isEmpty(productLbl.Text) Then
                    productLbl.Text = UIUtil.subsIfEmpty(productLbl.Text, lst(r).prodCode) + " " + UIUtil.subsIfEmpty(productLbl.Text, lst(r).prodName)
                End If

                itemVals(0) = UIUtil.toDateString(lst(r).tranDate)
                itemVals(1) = If(lst(r).tranTyp = BusinessReportDAO.TransactionType.purchase, "purchase", "sale")
                itemVals(2) = If(lst(r).tranTyp = BusinessReportDAO.TransactionType.purchase, If(lst(r).qty < 0, "", "+") + lst(r).qty.ToString, If(lst(r).qty < 0, "+", "") + (-1 * lst(r).qty).ToString)
                itemVals(3) = lst(r).uom
                itemVals(4) = If(lst(r).qty = 0, 0, UIUtil.toAmtString(lst(r).amount / lst(r).qty))
                itemVals(5) = lst(r).purchasedFrom
                itemVals(6) = UIUtil.toAmtString(lst(r).qtyAvail)

                itemVals(7) = UIUtil.toAmtString(lst(r).acb)
                itemVals(8) = lst(r).comments
                itemVals(9) = lst(r).prodCode
                item = New ListViewItem(itemVals)
                If lst(r).isReversal Then
                    item.BackColor = Color.FromArgb(255, 175, 175)
                ElseIf lst(r).isAmendment Then
                    item.BackColor = Color.FromArgb(255, 249, 128)
                ElseIf lst(r).tranTyp = BusinessReportDAO.TransactionType.purchase Then
                    item.BackColor = Color.FromArgb(202, 228, 255)
                Else
                    item.BackColor = Color.FromArgb(189, 255, 189)
                End If
                If lst(r).qtyAvail < 0 Then
                    item.SubItems.Item(6).ForeColor = Color.Red
                End If

                productTransHistoryListView.Items.Add(item)

            Next
        End If

    End Sub
End Class