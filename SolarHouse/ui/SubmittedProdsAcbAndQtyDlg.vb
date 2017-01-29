Public Class SubmittedProdsAcbAndQtyDlg
    Dim affectedPrdsQtyAndAcb As DataTable

    Public Sub New(affectedPrdsQtyAndAcb As DataTable)

        ' This call is required by the designer.
        InitializeComponent()

        Me.affectedPrdsQtyAndAcb = affectedPrdsQtyAndAcb

    End Sub

    Private Sub ProductTransHistoryDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim serv As New BusinessReportService(Me)

        submittedPrdsAcbAndQtyListView.View = View.Details
        submittedPrdsAcbAndQtyListView.GridLines = False
        submittedPrdsAcbAndQtyListView.FullRowSelect = True
        submittedPrdsAcbAndQtyListView.BackColor = Color.FromArgb(244, 255, 234)


        Dim itemVals(5) As String
        Dim item As ListViewItem
        If Not IsNothing(affectedPrdsQtyAndAcb) Then
            For i As Integer = 0 To affectedPrdsQtyAndAcb.Rows.Count - 1
                itemVals(0) = affectedPrdsQtyAndAcb.Rows(i).Item("product_code").ToString
                itemVals(1) = affectedPrdsQtyAndAcb.Rows(i).Item("product_name").ToString
                itemVals(2) = UIUtil.toAmtString(affectedPrdsQtyAndAcb.Rows(i).Item("qty_available"))
                itemVals(3) = affectedPrdsQtyAndAcb.Rows(i).Item("qty_uom_name").ToString
                itemVals(4) = UIUtil.toAmtString(affectedPrdsQtyAndAcb.Rows(i).Item("acb_cost"))
                item = New ListViewItem(itemVals)
                item.BackColor = If(i Mod 2 = 0, Color.FromArgb(242, 249, 255), Color.FromArgb(202, 228, 255))
                submittedPrdsAcbAndQtyListView.Items.Add(item)
            Next
        End If


    End Sub
End Class