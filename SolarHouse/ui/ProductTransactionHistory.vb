Public Class ProductTransactionHistory

    Private commitedProdsTransHistory As New Dictionary(Of String, List(Of BusinessReportDAO.ProdTransactionHistoryVO))
    Private salesGrdView As SalesRprtGridView
    Private purchasesRprtGridView As PurchasesRprtGridView
    Private businessReportDlg As BusinessReportDlg


    Public Sub New(businessReportDlg As BusinessReportDlg)
        Me.businessReportDlg = businessReportDlg
        salesGrdView = businessReportDlg.salesGrdView
        purchasesRprtGridView = businessReportDlg.purchasesGrdView

    End Sub

    Public Sub reset()
        commitedProdsTransHistory.Clear()
    End Sub

    Public Function getNegTransactonForProd(prodCode As String) As List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        Dim negTrans As List(Of BusinessReportDAO.ProdTransactionHistoryVO) = New List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        Dim prodTrans As List(Of BusinessReportDAO.ProdTransactionHistoryVO) = getCurProdTransHist(prodCode)
        For r As Integer = 0 To prodTrans.Count - 1
            If (prodTrans(r).qtyAvail < 0) Then
                negTrans.Add(prodTrans(r))
            End If
        Next
        Return negTrans
    End Function

    Public Function getProductAcb(prodCode As String) As Integer
        Dim prodTrans As List(Of BusinessReportDAO.ProdTransactionHistoryVO) = getCurProdTransHist(prodCode)
        Return If(prodTrans.Count <= 0, 0, prodTrans(prodTrans.Count - 1).acb)
    End Function

    Public Sub prodPurchaseChanged(prodCode As String)
        salesGrdView.prodPurchasedChanged(prodCode)
    End Sub

    Public Function getCurProdTransHist(prodCode As String) As List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        Dim prodTrans As List(Of BusinessReportDAO.ProdTransactionHistoryVO) = getCommitedProductTransactions(prodCode)
        Dim unCommitedProdTrans As List(Of BusinessReportDAO.ProdTransactionHistoryVO)

        unCommitedProdTrans = getUncommitedPurchaseTransactions(prodCode)
        For pr As Integer = 0 To unCommitedProdTrans.Count - 1
            insertIntoHistUncommitedTran(unCommitedProdTrans(pr), prodTrans)
        Next

        unCommitedProdTrans = getUncommitedSaleTransactions(prodCode)
        For sr As Integer = 0 To unCommitedProdTrans.Count - 1
            insertIntoHistUncommitedTran(unCommitedProdTrans(sr), prodTrans)
        Next
        Dim serv As BusinessReportService = New BusinessReportService(businessReportDlg)
        serv.calcAcbQtyAvail(prodTrans)
        Return prodTrans
    End Function



    ' private

    Private Sub insertIntoHistUncommitedTran(tran As BusinessReportDAO.ProdTransactionHistoryVO, tranHist As List(Of BusinessReportDAO.ProdTransactionHistoryVO))
        Dim isIns As Boolean = False
        For r As Integer = 0 To tranHist.Count - 1
            If tranHist(r).tranDate > tran.tranDate Then
                tranHist.Insert(r, tran)
                isIns = True
                Exit For
            End If
        Next
        If (Not isIns) Then
            tranHist.Add(tran)
        End If
    End Sub



    Private Function getUncommitedSaleTransactions(prodCode As String) As List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        Dim uncommitedSales As New List(Of BusinessReportDAO.ProdTransactionHistoryVO)

        Dim sale As BusinessReportDAO.SaleVO
        Dim tran As BusinessReportDAO.ProdTransactionHistoryVO
        Dim sales As Collection = salesGrdView.loadSalesReportDataFromGrid()
        For i As Integer = 1 To sales.Count
            sale = sales(i)
            If (sale.prodCode = prodCode AndAlso Not IsNothing(sale.tranDate)) Then
                tran = New BusinessReportDAO.ProdTransactionHistoryVO
                tran.tranTyp = BusinessReportDAO.TransactionType.sale
                tran.prodCode = sale.prodCode
                tran.tranDate = sale.tranDate
                tran.qty = sale.qty
                tran.uom = sale.qtyUomName
                tran.amount = sale.totalSaleAmt
                tran.acb = 0

                tran.purchasedFrom = ""
                tran.comments = sale.comments
                tran.isReversal = sale.isReversal
                tran.isAmendment = sale.isAmendment
                tran.isCommited = False
                uncommitedSales.Add(tran)
            End If
        Next
        Return uncommitedSales
    End Function


    Private Function getUncommitedPurchaseTransactions(prodCode As String) As List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        Dim uncommitedPurchases As New List(Of BusinessReportDAO.ProdTransactionHistoryVO)

        Dim tran As BusinessReportDAO.ProdTransactionHistoryVO
        Dim purchase As BusinessReportDAO.PurchaseVO
        Dim purchases As Collection = purchasesRprtGridView.loadPurchasesReportDataFromGrid()
        For i As Integer = 1 To purchases.Count
            purchase = purchases(i)
            If (purchase.prodCode = prodCode AndAlso Not IsNothing(purchase.tranDate)) Then
                tran = New BusinessReportDAO.ProdTransactionHistoryVO
                tran.tranTyp = BusinessReportDAO.TransactionType.purchase
                tran.prodCode = purchase.prodCode
                tran.tranDate = purchase.tranDate
                tran.qty = purchase.qty
                tran.uom = purchase.qtyUomName
                tran.amount = purchase.totalPurchaseAmt
                tran.acb = 0
                tran.purchasedFrom = purchase.supplierName
                tran.comments = purchase.comments
                tran.isReversal = purchase.isReversal
                tran.isAmendment = purchase.isAmendment
                tran.isCommited = False
                uncommitedPurchases.Add(tran)
            End If
        Next
        Return uncommitedPurchases
    End Function


    Private Function getCommitedProductTransactions(prodCode As String) As List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        If Not commitedProdsTransHistory.ContainsKey(prodCode) Then
            Dim serv As BusinessReportService = New BusinessReportService(businessReportDlg)
            commitedProdsTransHistory.Add(prodCode, serv.retrieveTransactionHistory(prodCode))
        End If
        Dim cloned As List(Of BusinessReportDAO.ProdTransactionHistoryVO) = New List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        cloned.AddRange(commitedProdsTransHistory.Item(prodCode))
        Return cloned
    End Function

End Class
