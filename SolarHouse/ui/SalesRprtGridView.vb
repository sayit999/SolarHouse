Imports System.ComponentModel

Public Class SalePurchaseQtyVO
    Public tranOn As Date
    Public qty As Integer
    Public isSale As Boolean
End Class

Public Class SalesRprtGridView
    Inherits BusinessReportGridView

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        Me.RowTemplate.Height = 24
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Public Overrides Sub setupGridView()
        MyBase.setupGridView()
    End Sub

    'Public Function mergeSalesAndPurchases(prdCode As String) As List(Of SalePurchaseQtyVO)
    '    Dim tran As SalePurchaseQtyVO
    '    Dim dlg As BusinessReportDlg = getBusinessReportDlg()
    '    Dim purchasesGrdView As PurchasesRprtGridView = dlg.purchasesGrdView
    '    Dim tranDate As Date

    '    Dim merged As List(Of SalePurchaseQtyVO) = New List(Of SalePurchaseQtyVO)
    '    For pr As Integer = 0 To purchasesGrdView.Rows.Count - 1
    '        tranDate = UIUtil.parseDate(purchasesGrdView.Rows(pr).Cells(purchasesGrdView.Columns("purchasePurchasedOn").Index).Value)
    '        If (purchasesGrdView.Rows(pr).Cells(purchasesGrdView.Columns("purchaseProductCode").Index).Value = prdCode AndAlso Not IsNothing(tranDate)) Then
    '            tran = New SalePurchaseQtyVO
    '            tran.isSale = False
    '            tran.tranOn = tranDate
    '            tran.qty = UIUtil.zeroIfEmpty(purchasesGrdView.Rows(pr).Cells(purchasesGrdView.Columns("purchaseQty").Index).Value)
    '            merged.Add(tran)
    '        End If
    '    Next

    '    Dim isIns As Boolean = False
    '    For sr As Integer = 0 To Rows.Count - 1
    '        tranDate = UIUtil.parseDate(Rows(sr).Cells(Columns("saleSoldOn").Index).Value, Nothing)
    '        If (Rows(sr).Cells(Columns("saleProductCode").Index).Value = prdCode AndAlso Not IsNothing(tranDate)) Then
    '            tran = New SalePurchaseQtyVO
    '            tran.isSale = True
    '            tran.tranOn = tranDate
    '            tran.qty = UIUtil.zeroIfEmpty(Rows(sr).Cells(Columns("saleQtySold").Index).Value)
    '            For r As Integer = 0 To merged.Count - 1
    '                If merged(r).tranOn > tran.tranOn Then
    '                    merged.Insert(r + 1, tran)
    '                    isIns = True
    '                    Exit For
    '                End If

    '            Next
    '            If Not isIns Then
    '                merged.Add(tran)
    '            End If
    '        End If
    '    Next
    '    mergeSalesAndPurchases = merged
    'End Function

    'Protected Function checkForNegativeInventory(prdCode As String) As Boolean
    '    Dim isFndErrors As Boolean = False
    '    Dim dlg As BusinessReportDlg = getBusinessReportDlg()
    '    Dim productsEntityDataSet As DataTable = dlg.productsEntityDataSet

    '    Dim qty As Integer
    '    Dim mergedTrans As List(Of SalePurchaseQtyVO)
    '    qty = dlg.getProdInventoryQty(prdCode)
    '    mergedTrans = mergeSalesAndPurchases(prdCode)
    '    For cr = 0 To mergedTrans.Count - 1
    '        If (mergedTrans(cr).isSale) Then
    '            qty -= mergedTrans(cr).qty
    '            If qty < 0 Then
    '                isFndErrors = True
    '            End If
    '        Else
    '            qty += mergedTrans(cr).qty
    '        End If
    '    Next
    '    checkForNegativeInventory = Not isFndErrors
    'End Function

    Protected Function areSaleAndPurchasesTransactionsAreValid(prdCode As String) As Boolean
        Dim dlg As BusinessReportDlg = getBusinessReportDlg()


        Dim purchasesGrdView As PurchasesRprtGridView = dlg.purchasesGrdView

        If dlg.purchasesGrdView.isProdPurchaseInErr(prdCode) Then
            Return False
        End If

        For sr As Integer = 0 To Rows.Count - 1
            If UIUtil.subsIfEmpty(Rows(sr).Cells("saleProductCode").Value, "") = prdCode AndAlso Not StringUtil.isEmpty(Rows(sr).ErrorText) Then
                For c = 0 To ColumnCount - 1
                    If Not StringUtil.isEmpty(Rows(sr).Cells(c).ErrorText) AndAlso Rows(sr).Cells(c).ErrorText.IndexOf("negative stock") = -1 Then
                        Return False
                    End If
                Next

            End If
        Next

        Return True

    End Function

    Private Sub insertIntoHistNotPostedTran(tran As BusinessReportDAO.ProdTransactionHistoryVO, tranHist As List(Of BusinessReportDAO.ProdTransactionHistoryVO))
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

    Private Sub mergeNotPostedTransactionsIntoHist(prodCode As String, ByRef tranHist As List(Of BusinessReportDAO.ProdTransactionHistoryVO))
        Dim tran As BusinessReportDAO.ProdTransactionHistoryVO

        Dim dlg As BusinessReportDlg = getBusinessReportDlg()
        Dim purchasesGrdView As PurchasesRprtGridView = dlg.purchasesGrdView
        Dim tranDate As Date
        Dim rowProdCode As String

        Dim merged As List(Of SalePurchaseQtyVO) = New List(Of SalePurchaseQtyVO)
        For pr As Integer = 0 To purchasesGrdView.Rows.Count - 1
            If Not isTransactionPosted(pr) Then
                rowProdCode = purchasesGrdView.Rows(pr).Cells("purchaseProductCode").Value
                tranDate = UIUtil.parseDate(purchasesGrdView.Rows(pr).Cells(purchasesGrdView.Columns("purchasePurchasedOn").Index).Value)
                If (rowProdCode = prodCode AndAlso Not IsNothing(tranDate)) Then
                    tran = New BusinessReportDAO.ProdTransactionHistoryVO
                    tran.tranTyp = BusinessReportDAO.TransactionType.purchase
                    tran.prodCode = rowProdCode
                    tran.tranDate = tranDate
                    tran.qty = UIUtil.zeroIfEmpty(purchasesGrdView.Rows(pr).Cells(purchasesGrdView.Columns("purchaseQty").Index).Value)
                    tran.acb = 0
                    insertIntoHistNotPostedTran(tran, tranHist)
                End If
            End If
        Next

        For sr As Integer = 0 To Rows.Count - 1
            If Not isTransactionPosted(sr) Then
                rowProdCode = Rows(sr).Cells("saleProductCode").Value
                tranDate = UIUtil.parseDate(Rows(sr).Cells(Columns("saleSoldOn").Index).Value, Nothing)
                If (rowProdCode = prodCode AndAlso Not IsNothing(tranDate)) Then
                    tran = New BusinessReportDAO.ProdTransactionHistoryVO
                    tran.tranTyp = BusinessReportDAO.TransactionType.sale
                    tran.prodCode = rowProdCode
                    tran.tranDate = tranDate
                    tran.qty = UIUtil.zeroIfEmpty(Rows(sr).Cells(Columns("saleQtySold").Index).Value)
                    tran.acb = 0
                    insertIntoHistNotPostedTran(tran, tranHist)
                End If
            End If
        Next
    End Sub

    Private Function doesHistHaveNegInventory(tranHist As List(Of BusinessReportDAO.ProdTransactionHistoryVO)) As Boolean
        For r As Integer = 0 To tranHist.Count - 1
            If tranHist(r).qtyAvail < 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function validateNoNegativeInventory() As Boolean
        Dim tranHist As List(Of BusinessReportDAO.ProdTransactionHistoryVO)
        Dim prdsChecked As New List(Of String)
        Dim serv As New BusinessReportService(getBusinessReportDlg)
        Dim isValid As Boolean = True
        Dim prodCode As String
        For r As Integer = 0 To RowCount - 1
            prodCode = UIUtil.subsIfEmpty(Rows(r).Cells("saleProductCode").Value, "")
            If (Not prdsChecked.Exists(Function(x) x = prodCode)) Then
                prdsChecked.Add(prodCode)
                tranHist = serv.retrieveTransactionHistory(prodCode)
                mergeNotPostedTransactionsIntoHist(prodCode, tranHist)
                serv.calcAcbQtyAvail(tranHist)
                If doesHistHaveNegInventory(tranHist) Then
                    For sr As Integer = 0 To Rows.Count - 1
                        If (Rows(sr).Cells(Columns("saleProductCode").Index).Value = prodCode) Then
                            setRowColValidationMesg(ValidationMessageType.IS_ERROR, sr, Columns("saleQtySold").Index, "Sale causes negative stock. Sold more than in stock")
                            isValid = False
                        End If
                    Next
                End If
            End If
        Next
        Return isValid
    End Function

    Public Overrides Function validateRows()
        If Not MyBase.validateRows() Then
            Return False
        End If
        Return validateNoNegativeInventory()

        'Dim prdCode As String = Rows(row).Cells("saleProductCode").Value
        'If areSaleAndPurchasesTransactionsAreValid(prdCode) AndAlso Not checkForNegativeInventory(prdCode) Then
        '    addError(result, "Sale causes negative stock. Sold more than in stock", row, "saleQtySold")
        'End If

    End Function

    Protected Overrides Sub doValidateRow(row As Integer, ByRef result As RowValidationResult)
        If isReversalTransaction(row) Then
            If (StringUtil.isEmpty(Rows(row).Cells(getCommentColName()).Value)) Then
                addError(result, "Comment has to be entered", row, Columns(getCommentColName()).Index)
            End If
        Else
            MyBase.doValidateRow(row, result)
            If (StringUtil.isEmpty(Me.Rows(row).Cells("saleProductCode").Value)) Then
                addError(result, "Product code cannot be empty.", row, "saleProductCode")
            ElseIf (Not getBusinessReportDlg().isValidCode(Me.Rows(row).Cells("saleProductCode").Value, getBusinessReportDlg().productsEntityDataSet, "product_code")) Then
                addError(result, "Product code is invalid.", row, "saleProductCode")
            End If

            If (StringUtil.isEmpty(Me.Rows(row).Cells("saleQtySold").Value)) Then
                addError(result, "Qty cannot be empty", row, "saleQtySold")
            ElseIf UIUtil.zeroIfEmpty(Me.Rows(row).Cells("saleQtySold").Value) < 0 Then
                addError(result, "Sale Qty cannot be negative", row, "saleQtySold")
            End If

            If (StringUtil.isEmpty(Me.Rows(row).Cells("saleSaleAmt").Value)) Then
                addError(result, "Sale amount cannot be empty", row, "saleSaleAmt")
            End If
            If (Not StringUtil.isEmpty(Me.Rows(row).Cells("saleProfit").Value) AndAlso Me.Rows(row).Cells("saleProfit").Value < 0) Then
                addWarning(result, "Profit is negative", row, "saleProfit")
                If StringUtil.isEmpty(Me.Rows(row).Cells("saleComments").Value) Then
                    addError(result, "Enter reason why profit is negeative", row, "saleComments")
                End If
            End If
            If (Not StringUtil.isEmpty(Me.Rows(row).Cells("saleProfitPercentage").Value) AndAlso Me.Rows(row).Cells("saleProfit").Value < 0) Then
                addWarning(result, "Profit is negative", row, "saleProfitPercentage")
            End If


        End If


    End Sub

    Protected Overrides Sub defaultNewRow(row As Integer)
        MyBase.defaultNewRow(row)
        Rows(row).Cells(Columns("saleQtySold").Index).Value = 1
        Rows(row).Cells(Columns("saleTotalCost").Index).Value = 0
        Rows(row).Cells(Columns("saleSaleAmt").Index).Value = 0
        Rows(row).Cells(Columns("saleProfit").Index).Value = 0
        Rows(row).Cells(Columns("saleProfitPercentage").Index).Value = 0
    End Sub


    Protected Overrides Sub gridCellValueChanged(colInd As Integer, rowInd As Integer)

        If Me.Columns("saleProductCode").Index = colInd Then
            Dim row As DataRow = getBusinessReportDlg().locateRow(Me.Rows(rowInd).Cells("saleProductCode").Value, "product_code", getBusinessReportDlg().productsEntityDataSet)
            If (Not IsNothing(row)) Then
                Me.Rows(rowInd).Cells("saleProductName").Value = row.Item("product_name")
                Me.Rows(rowInd).Cells("saleUomName").Value = row.Item("qty_uom_name")
                Me.Rows(rowInd).Cells("saleProdAcbCost").Value = row.Item("acb_cost")
            Else
                Me.Rows(rowInd).Cells("saleProductName").Value = ""
                Me.Rows(rowInd).Cells("saleUomName").Value = ""
                Me.Rows(rowInd).Cells("saleProdAcbCost").Value = 0
            End If
            Refresh()
        End If

        If Me.Columns("saleProdAcbCost").Index = colInd Then
            Me.Rows(rowInd).Cells("saleTotalCost").Value = getNumber(rowInd, Columns("saleProdAcbCost").Index) * getNumber(rowInd, Columns("saleQtySold").Index)
            Refresh()
        End If

        If Me.Columns("saleTotalCost").Index = colInd Then
            Me.Rows(rowInd).Cells("saleProfit").Value = getNumber(rowInd, Columns("saleSaleAmt").Index) - getNumber(rowInd, Columns("saleTotalCost").Index)
            Refresh()
        End If

        If Me.Columns("saleProfit").Index = colInd Then
            Dim totCost As Double = getNumber(rowInd, Columns("saleTotalCost").Index)
            If (totCost = 0) Then
                Me.Rows(rowInd).Cells("saleProfitPercentage").Value = 0
            Else
                Me.Rows(rowInd).Cells("saleProfitPercentage").Value = FormatNumber(Math.Round((Me.Rows(rowInd).Cells("saleProfit").Value / totCost) * 100), 0, TriState.True, TriState.False, True) + "%"
            End If
            Refresh()
        End If

        If Me.Columns("saleQtySold").Index = colInd Then
            Me.Rows(rowInd).Cells("saleTotalCost").Value = getNumber(rowInd, Columns("saleProdAcbCost").Index) * getNumber(rowInd, Columns("saleQtySold").Index)
            Refresh()
        End If

        If Me.Columns("saleSaleAmt").Index = colInd Then
            Me.Rows(rowInd).Cells("saleProfit").Value = getNumber(rowInd, Columns("saleSaleAmt").Index) - getNumber(rowInd, Columns("saleTotalCost").Index)
            Refresh()
        End If



    End Sub


    Public Sub getSaleTotals(ByRef totSales As Double, ByRef totProfits As Double)
        totSales = 0
        totProfits = 0
        For i = 0 To (RowCount - 1)
            If (Not isRowEmpty(i)) Then
                totSales += UIUtil.removeNumberFormating(Rows(i).Cells("saleSaleAmt").Value)
                totProfits += UIUtil.removeNumberFormating(Rows(i).Cells("saleProfit").Value)
            End If
        Next i
    End Sub

    Public Overrides Sub loadBusinessReportDataFromGrid(busRprt As BusinessReportDAO.BusinessReport)
        MyBase.loadBusinessReportDataFromGrid(busRprt)
        Dim sales As New Collection
        Dim sale As BusinessReportDAO.SaleVO

        For i = 0 To (RowCount - 1)
            If (Not isRowEmpty(i) AndAlso Not isTransactionPosted(i)) Then
                sale = New BusinessReportDAO.SaleVO
                sale.transactionId = UIUtil.subsIfEmpty(Rows(i).Cells("saleSaleId").Value, BusinessReportDAO.NULL_NUMBER)
                sale.tranDate = UIUtil.parseDate(Rows(i).Cells("saleSoldOn").Value, Nothing)
                sale.prodCode = Rows(i).Cells("saleProductCode").Value
                sale.qty = Rows(i).Cells("saleQtySold").Value
                sale.priceSold = Rows(i).Cells("saleSaleAmt").Value
                sale.comments = Rows(i).Cells("saleComments").Value
                sale.isReversal = UIUtil.toBoolean(Rows(i).Cells("isSalePostedReversal").Value)
                sale.isPosted = UIUtil.toBoolean(Rows(i).Cells("isSalePosted").Value)
                sale.isAmendment = UIUtil.toBoolean(Rows(i).Cells("is_sale_amendment").Value)
                sales.Add(sale)
            End If
        Next i
        busRprt.sales = sales
    End Sub

    Protected Overrides Function getCommentColName() As String
        Return "saleComments"
    End Function

    Protected Overrides Function getIsAmendmentColName() As String
        Return "is_sale_amendment"
    End Function

    Public Overrides Sub loadBusinessReportDataIntoGrid(busRprt As BusinessReportDAO.BusinessReport)
        MyBase.loadBusinessReportDataIntoGrid(busRprt)
        Dim sales As Collection = busRprt.sales
        If IsNothing(sales) Then
            Return
        End If

        Dim acb As Integer
        Dim totCost As Integer
        Dim sale As BusinessReportDAO.SaleVO
        For i = 1 To (sales.Count)
            If (i > Rows.Count) Then
                Rows.Add()
            End If
            sale = sales(i)
            Dim row As DataRow = getBusinessReportDlg().locateRow(sale.prodCode, "product_code", getBusinessReportDlg().productsEntityDataSet)
            If (Not IsNothing(row)) Then
                Rows(i - 1).Cells("saleProductName").Value = row.Item("product_name")
                Rows(i - 1).Cells("saleUomName").Value = row.Item("qty_uom_name")
                acb = row.Item("acb_cost")
            Else
                acb = 0
            End If
            Rows(i - 1).Cells("saleSaleId").Value = sale.transactionId
            Rows(i - 1).Cells("saleSoldOn").Value = UIUtil.toDateString(sale.tranDate)
            Rows(i - 1).Cells("saleProductCode").Value = sale.prodCode
            Rows(i - 1).Cells("saleQtySold").Value = UIUtil.zeroIfEmpty(sale.qty)
            Rows(i - 1).Cells("saleProdAcbCost").Value = acb
            totCost = acb * Rows(i - 1).Cells("saleQtySold").Value
            Rows(i - 1).Cells("saleTotalCost").Value = totCost

            Rows(i - 1).Cells("saleSaleAmt").Value = UIUtil.toAmtString(sale.priceSold)
            Rows(i - 1).Cells("saleProfit").Value = sale.priceSold - Rows(i - 1).Cells("saleTotalCost").Value
            If (totCost = 0) Then
                Me.Rows(i - 1).Cells("saleProfitPercentage").Value = 0
            Else
                Me.Rows(i - 1).Cells("saleProfitPercentage").Value = FormatNumber(Math.Round((Me.Rows(i - 1).Cells("saleProfit").Value / totCost) * 100), 0, TriState.True, TriState.False, True) + "%"
            End If
            Rows(i - 1).Cells("saleComments").Value = sale.comments
            Rows(i - 1).Cells("isSalePosted").Value = UIUtil.toBinaryBooleanString(sale.isPosted)
            Rows(i - 1).Cells("isSalePostedReversal").Value = UIUtil.toBinaryBooleanString(sale.isReversal)
            Rows(i - 1).Cells("is_sale_amendment").Value = UIUtil.toBinaryBooleanString(sale.isAmendment)

        Next i

        MyBase.loadBusinessReportDataIntoGrid(busRprt)

    End Sub

    Protected Overrides Sub OnCellBeginEdit(e As DataGridViewCellCancelEventArgs)
        MyBase.OnCellBeginEdit(e)
        If (Columns(e.ColumnIndex).Name = "saleQtySold") Then
            If StringUtil.isEmpty(CurrentCell.Value) Then
                CurrentCell.Value = 1
            End If
        End If

    End Sub

    Protected Overrides Sub indicateTransactionPosted(row As Integer, Optional isPosted As Boolean = True)
        Rows(row).Cells("isSalePosted").Value = UIUtil.toBinaryBooleanString(isPosted)
    End Sub

    Protected Overrides Function isTransactionPosted(row As Integer) As Boolean
        Return UIUtil.toBoolean(Rows(row).Cells("isSalePosted").Value)
    End Function

    Protected Overrides Function isReversalTransaction(row As Integer) As Boolean
        Return UIUtil.toBoolean(Rows(row).Cells("isSalePostedReversal").Value)

    End Function

    Protected Overrides Sub indicateTransactionReversed(row As Integer, Optional isReversed As Boolean = True)
        isProgramaticChange = True
        Rows(row).Cells("isSalePostedReversal").Value = UIUtil.toBinaryBooleanString(isReversed)
        Me.Rows(row).Cells("saleProfit").Value = -1 * Me.Rows(row).Cells("saleProfit").Value
        ' Me.Rows(row).Cells("saleProfitPercentage").Value = "-" + UIUtil.parseDouble(Me.Rows(row).Cells("saleProfitPercentage").Value)
        Me.Rows(row).Cells("saleQtySold").Value = -1 * UIUtil.zeroIfEmpty(Me.Rows(row).Cells("saleQtySold").Value)
        Me.Rows(row).Cells("saleSaleAmt").Value = -1 * UIUtil.zeroIfEmpty(Me.Rows(row).Cells("saleSaleAmt").Value)
        isProgramaticChange = False
    End Sub




    Public Overrides Sub deleteCurrentRow()
        deleteRow("Sale", 0, 1)
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        If Not Me.getBusinessReportDlg().isReportSubmitted() AndAlso isControlSPressed(e) Then
            If Columns(CurrentCell.ColumnIndex).Name = "saleProductCode" Then
                Dim dlg As ProductsDlg = New ProductsDlg(getBusinessReportDlg)
                dlg.codeSelected = CurrentCell.Value
                dlg.ShowDialog(Me)
                If (dlg.DialogResult = DialogResult.OK) Then
                    If dlg.isEntitySelected Then
                        CurrentCell.Value = dlg.codeSelected
                        Rows(CurrentCell.RowIndex).Cells("saleProductName").Value = dlg.nameSelected
                    End If

                    Me.CommitEdit(DataGridViewDataErrorContexts.Commit)
                    Refresh()
                    entityDataSetModified(dlg.wasModified())
                End If
                e.Handled = True
            End If
        End If

        MyBase.OnKeyUp(e)
    End Sub

End Class
