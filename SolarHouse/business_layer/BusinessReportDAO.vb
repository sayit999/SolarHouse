Imports System.Xml
Imports System.IO
Imports MySql.Data.MySqlClient


Public Class BusinessReportDAO
    Inherits SolarHouseDao

    Public mainForm As MainForm = My.Application.OpenForms("MainForm")
    Public Const NULL_NUMBER As Integer = -9999
    Private Const XML_COMMA_SUBST As String = "<comma>"

    Public Shared Function prodTranKeyisNothing(row As DataRow) As Boolean
        Return row Is Nothing OrElse row.Item("tran_date") Is Nothing OrElse row.Item("product_code") Is Nothing AndAlso row.Item("updated_on") Is Nothing
    End Function

    Private Shared Function computeSortPriority(row As DataRow) As String
        Dim isAmendment As Boolean = row.Item("is_amendment") = 1
        Dim isReversal As Boolean = row.Item("is_reversal") = 1

        If Not isAmendment Then
            Return "1"
        Else
            Return If(isReversal, "2", "3")
        End If


    End Function

    Private Shared Function createCombKey(row As DataRow) As String
        Return row.Item("product_code") + Format(row.Item("tran_date"), "yyyy/MM/dd") + If(row.Item("tran_type") = "purchased", "0", "1") + row.Item("supplier_name") + Format(row.Item("updated_on"), "yyyy/MM/dd HH:mm:ss.fff") + computeSortPriority(row)
    End Function

    Public Shared Function compareProdTrans(ByVal x As Object, ByVal y As Object) As Integer
        Dim rowX As DataRow = CType(x, DataRow)
        Dim rowY As DataRow = CType(y, DataRow)

        If prodTranKeyisNothing(rowX) AndAlso prodTranKeyisNothing(rowY) Then
            Return 0
        ElseIf prodTranKeyisNothing(rowX) Then
            Return -1
        ElseIf prodTranKeyisNothing(rowY) Then
            Return 1
        Else
            Dim combKeyX As String = createCombKey(rowX)
            ' rowX.Item("product_code") + Format(rowX.Item("tran_date"), "yyyy/MM/dd") + If(rowX.Item("tran_type") = "purchased", "0", "1") + Format(rowX.Item("updated_on"), "yyyy/MM/dd HH:mm:ss.fff")
            Dim combKeyY As String = createCombKey(rowY)
            ' rowY.Item("product_code") + Format(rowY.Item("tran_date"), "yyyy/MM/dd") + If(rowY.Item("tran_type") = "purchased", "0", "1") + Format(rowY.Item("updated_on"), "yyyy/MM/dd HH:mm:ss.fff")
            Return combKeyX.CompareTo(combKeyY)
        End If

    End Function
    '===================================================================
    ' data structures
    '===================================================================
    Enum ReportTypeEntityModType
        add
        delete
        update
    End Enum

    Enum TransactionType
        sale
        purchase
    End Enum


    Public Class ProdTransactionHistoryVO
        Public tranDate As Date
        Public tranTyp As TransactionType
        Public productId As Integer
        ' Public supplierName As String
        Public acb As Integer
        Public qtyAvail As Integer
        Public qty As Integer
        Public uom As String
        Public amount As Integer
        Public comments As String
        Public purchasedFrom As String
        Public prodCode As String
        Public prodName As String
        Public isAmendment As Boolean
        Public isReversal As Boolean
        Public updatedOn As Date
        Public isIgnoredInAcbCalc As Boolean
        Public isCommited As Boolean
    End Class

    Public Class BusinessReportEntityVO
        Public modType As ReportTypeEntityModType
        Public id As Integer = -1
        Public code As String
        Public name As String
    End Class

    Public Class SupplierModifiedVO
        Inherits BusinessReportEntityVO
        Public comments As String
    End Class

    Public Class ProductModifiedVO
        Inherits BusinessReportEntityVO
        Public catCodeName As String
        Public qtyAvail As Integer
        Public qtyUOM As String
        Public acbCost As Double
        Public retDiscRoomPercentage As Double
        Public minRetGrossProfitMarginPercentage As Double
        Public retSalePrice As Double
        Public comments As String
        Public isReorder As Integer
        Public lowStockAlertQty As Integer
    End Class

    Public Class ExpenseCategoryModifiedVO
        Inherits BusinessReportEntityVO
        Public desc As String
    End Class

    Public Class BusinessTransactionVO
        Public tranDate As Date
        Public isPosted As Boolean = False
        Public isReversal As Boolean = False
        Public isAmendment As Boolean = False
        Public transactionId As Integer = -1
        Public comments As String = ""
    End Class

    Public Class ExpenseVO
        Inherits BusinessTransactionVO
        Public expCatCode As String
        Public expCatId As Integer
        Public expAmt As Integer

    End Class

    Public Class DebtPaymentVO
        Inherits BusinessTransactionVO
        Public suplrCode As String
        Public suplrId As Integer
        Public debtAmtPaid As Integer
    End Class

    Public Class PurchaseVO
        Inherits BusinessTransactionVO
        Public supplierName As String ' optional
        Public suplrCode As String
        Public suplrId As Integer
        Public qty As Integer
        Public qtyUomName As String ' optional
        Public prodCode As String
        Public prodId As Integer

        Public totalPurchaseAmt As Integer
        Public cashPurchase As Integer
        Public creditPurchase As Integer
    End Class

    Public Class SaleVO
        Inherits BusinessTransactionVO
        Public prodCode As String
        Public prodId As Integer

        Public totalCostOfProd As Integer 'optional
        Public prodName As String ' optional

        Public qty As Integer
        Public qtyUomName As String ' optional

        Public totalSaleAmt As Integer

        Public Function profit() As Double
            Return totalSaleAmt - totalCostOfProd
        End Function
    End Class

    Enum ReportType
        business
        amendment
    End Enum

    Enum ReportSubmitType
        load
        submit
    End Enum

    Public Class ReportLoadSubmitStatusVO
        Public reportFrom As Date
        Public submitType As ReportSubmitType
        Public onDate As Date
        Public reportTo As Date
        Public cashBroughtForward As Integer
        Public cashCounted As Integer
        Public cashExpected As Integer
        Public reportType As ReportType
        Public reportFileLocation As String
        Public reason4CashDiff As String
    End Class

    Enum EstimatedExpenseType
        workday
        monthly
    End Enum

    Public Class EstimatedExpenseVO
        Public expenseCategoryName As String
        Public expenseType As EstimatedExpenseType
        Public expenseAmt As Double
        Public comments As String
    End Class



    Public Class BusinessReport
        Public isAnAmendmentToPostedTrans As Boolean = False
        Public fromDate As Date
        Public toDate As Date
        Public absXmlFileName As String

        Public cashBroughtForward As Double
        Public cashCounted As Double
        Public cashExpected As Double
        Public reason4CashCountedVersusBroughtForwardDiff As String

        Public suppliersModified As Collection
        Public productsModified As Collection
        Public expenseCategoriesModified As Collection


        Public sales As Collection
        Public purchases As Collection
        Public expenses As Collection
        Public debtPayments As Collection

        Public Function isEmpty(c As Collection) As Boolean
            isEmpty = IsNothing(c) OrElse c.Count = 0
        End Function

        Public Function isEmpty() As Boolean
            isEmpty = isEmpty(suppliersModified) AndAlso isEmpty(productsModified) AndAlso isEmpty(expenseCategoriesModified) _
                      AndAlso isEmpty(sales) AndAlso isEmpty(purchases) AndAlso isEmpty(expenses) AndAlso isEmpty(debtPayments)
        End Function

    End Class
    '===================================================================
    ' data structures
    '===================================================================

    Private Function getEntityModTypString(modType As ReportTypeEntityModType)

        Select Case modType
            Case ReportTypeEntityModType.add : getEntityModTypString = "add"
            Case ReportTypeEntityModType.delete : getEntityModTypString = "delete"
            Case ReportTypeEntityModType.update : getEntityModTypString = "update"
            Case Else
                Throw New ArgumentException("No ReportTypeEntityModType for mod type:" + modType)

        End Select
    End Function


    Private Function getEntityModTyp(modTypeStr As String) As ReportTypeEntityModType
        If modTypeStr = "add" Then
            Return ReportTypeEntityModType.add
        ElseIf modTypeStr = "delete" Then
            Return ReportTypeEntityModType.delete
        ElseIf modTypeStr = "update" Then
            Return ReportTypeEntityModType.update
        Else
            Throw New ArgumentException("No od type for " + modTypeStr)
        End If
    End Function

    Protected Function replComma(xmlVal As String) As String
        Return Replace(xmlVal, ",", XML_COMMA_SUBST)
    End Function

    Protected Function addBackComma(xmlVal As String) As String
        Return Replace(xmlVal, XML_COMMA_SUBST, ",")
    End Function

    Public Function readReportDates(absXmlFileName As String, ByRef fromDate As Date, ByRef toDate As Date) As Boolean


        Dim doc As New XmlDocument()
        Try

            doc.Load(absXmlFileName)

            If doc.GetElementsByTagName("business-report").Count <= 0 Then
                Throw New ArgumentException("Business Report invalid.  No 'business-report' XML tag ")
            End If

            Dim root As XmlNode

            root = doc.GetElementsByTagName("business-report")(0)
            fromDate = Nothing
            If Not StringUtil.isEmpty(root.Attributes.GetNamedItem("from_date").InnerText) Then
                fromDate = UIUtil.parseDate(root.Attributes.GetNamedItem("from_date").InnerText, Nothing)
            End If

            toDate = Nothing
            If Not StringUtil.isEmpty(root.Attributes.GetNamedItem("to_date").InnerText) AndAlso UIUtil.isValidDate(root.Attributes.GetNamedItem("to_date").InnerText) Then
                toDate = UIUtil.parseDate(root.Attributes.GetNamedItem("to_date").InnerText)
            End If
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function readBusinessReportFromXML(absXmlFileName As String) As BusinessReport
        Dim businessReport As BusinessReport = New BusinessReport

        businessReport.absXmlFileName = absXmlFileName

        Dim doc As New XmlDocument()
        Try

            doc.Load(absXmlFileName)

            If doc.GetElementsByTagName("business-report").Count <= 0 Then
                Return businessReport
            End If

            Dim root As XmlNode
            Dim reportElements As XmlNodeList
            Dim reportElement As XmlNode
            Dim data As String()

            root = doc.GetElementsByTagName("business-report")(0)
            businessReport.fromDate = Nothing
            If Not StringUtil.isEmpty(root.Attributes.GetNamedItem("from_date").InnerText) Then
                businessReport.fromDate = UIUtil.parseDate(root.Attributes.GetNamedItem("from_date").InnerText)
            End If
            businessReport.toDate = Nothing
            If Not StringUtil.isEmpty(root.Attributes.GetNamedItem("to_date").InnerText) AndAlso UIUtil.isValidDate(root.Attributes.GetNamedItem("to_date").InnerText) Then
                businessReport.toDate = UIUtil.parseDate(root.Attributes.GetNamedItem("to_date").InnerText)
            End If
            If Not IsNothing(root.Attributes.GetNamedItem("is_amendment")) AndAlso Not StringUtil.isEmpty(root.Attributes.GetNamedItem("is_amendment").InnerText) Then
                businessReport.isAnAmendmentToPostedTrans = UIUtil.toBoolean(root.Attributes.GetNamedItem("is_amendment").InnerText)
            End If

            reportElements = root.ChildNodes
            For i As Integer = 0 To reportElements.Count - 1
                reportElement = reportElements(i)
                If reportElement.Name = "report_overall" Then
                    For ci As Integer = 0 To reportElement.ChildNodes.Count - 1
                        If reportElement.ChildNodes(ci).Name = "cash_brought_forward" Then
                            businessReport.cashBroughtForward = UIUtil.parseDouble(reportElement.ChildNodes(ci).InnerText)
                        ElseIf reportElement.ChildNodes(ci).Name = "cash_counted" Then
                            businessReport.cashCounted = UIUtil.parseDouble(reportElement.ChildNodes(ci).InnerText)
                        ElseIf reportElement.ChildNodes(ci).Name = "cash_expected" Then
                            businessReport.cashExpected = UIUtil.parseDouble(reportElement.ChildNodes(ci).InnerText)
                        ElseIf reportElement.ChildNodes(ci).Name = "cash_counted_diff_reason" Then
                            businessReport.reason4CashCountedVersusBroughtForwardDiff = reportElement.ChildNodes(ci).InnerText
                        End If
                    Next
                ElseIf reportElement.Name = "suppliers_modified" Then
                    businessReport.suppliersModified = New Collection
                    Dim supplier As SupplierModifiedVO
                    For ci As Integer = 0 To reportElement.ChildNodes.Count - 1
                        If reportElement.ChildNodes(ci).Name = "supplier_modified_header" Then
                            Continue For
                        End If
                        supplier = New SupplierModifiedVO
                        supplier.modType = getEntityModTyp(reportElement.ChildNodes(ci).Name)
                        data = UIUtil.splitVals(reportElement.ChildNodes(ci).InnerText)
                        For di As Integer = 0 To data.Count - 1
                            Select Case di
                                Case 0
                                    supplier.code = addBackComma(data(0))
                                Case 1
                                    supplier.name = addBackComma(data(1))
                                Case 2
                                    supplier.comments = addBackComma(data(2))
                            End Select
                        Next
                        businessReport.suppliersModified.Add(supplier)
                    Next
                ElseIf reportElements(i).Name = "products_modified" Then
                    businessReport.productsModified = New Collection
                    Dim prodsMod As ProductModifiedVO
                    For ci As Integer = 0 To reportElement.ChildNodes.Count - 1
                        If reportElement.ChildNodes(ci).Name = "product_modified_header" Then
                            Continue For
                        End If
                        prodsMod = New ProductModifiedVO
                        prodsMod.modType = getEntityModTyp(reportElement.ChildNodes(ci).Name)
                        data = UIUtil.splitVals(reportElement.ChildNodes(ci).InnerText)
                        For di As Integer = 0 To data.Count - 1
                            Select Case di
                                Case 0
                                    prodsMod.code = addBackComma(data(0))
                                Case 1
                                    prodsMod.name = addBackComma(data(1))
                                Case 2
                                    prodsMod.catCodeName = addBackComma(data(2))
                                Case 3
                                    prodsMod.qtyAvail = UIUtil.parseDouble(data(3))
                                Case 4
                                    prodsMod.qtyUOM = addBackComma(data(4))
                                Case 5
                                    prodsMod.acbCost = UIUtil.parseDouble(data(5))
                                Case 6
                                    prodsMod.retDiscRoomPercentage = UIUtil.parseDouble(data(6))
                                Case 7
                                    prodsMod.minRetGrossProfitMarginPercentage = UIUtil.parseDouble(data(7))
                                Case 8
                                    prodsMod.retSalePrice = UIUtil.parseDouble(data(8))
                                Case 9
                                    prodsMod.comments = addBackComma(data(9))
                                Case 10
                                    prodsMod.isReorder = UIUtil.parseDouble(data(10))
                                Case 11
                                    prodsMod.lowStockAlertQty = UIUtil.parseDouble(data(11))
                            End Select
                        Next
                        businessReport.productsModified.Add(prodsMod)
                    Next
                ElseIf reportElements(i).Name = "expenses_categories_modified" Then
                    businessReport.expenseCategoriesModified = New Collection
                    Dim exp As ExpenseCategoryModifiedVO
                    For ci As Integer = 0 To reportElement.ChildNodes.Count - 1
                        If reportElement.ChildNodes(ci).Name = "expense_category_modified_header" Then
                            Continue For
                        End If
                        exp = New ExpenseCategoryModifiedVO
                        exp.modType = getEntityModTyp(reportElement.ChildNodes(ci).Name)
                        data = UIUtil.splitVals(reportElement.ChildNodes(ci).InnerText)
                        For di As Integer = 0 To data.Count - 1
                            Select Case di
                                Case 0
                                    exp.code = addBackComma(data(0))
                                Case 1
                                    exp.name = addBackComma(data(1))
                                Case 2
                                    exp.desc = addBackComma(data(2))
                            End Select
                        Next
                        businessReport.expenseCategoriesModified.Add(exp)
                    Next
                ElseIf reportElements(i).Name = "sales" Then
                    businessReport.sales = New Collection
                    Dim sale As SaleVO
                    For ci As Integer = 0 To reportElement.ChildNodes.Count - 1
                        If (reportElement.ChildNodes(ci).Name = "sale") Then
                            sale = New SaleVO
                            data = UIUtil.splitVals(reportElement.ChildNodes(ci).InnerText)
                            For di As Integer = 0 To data.Count - 1
                                Select Case di
                                    Case 0
                                        If UIUtil.isValidDate(data(0)) Then
                                            sale.tranDate = UIUtil.parseDate(data(0), Nothing)
                                        End If
                                    Case 1
                                        sale.prodCode = data(1)
                                    Case 2
                                        sale.qty = UIUtil.parseDouble(data(2))
                                    Case 3
                                        sale.totalSaleAmt = UIUtil.parseDouble(data(3))
                                    Case 4
                                        sale.comments = addBackComma(data(4))
                                    Case 5
                                        sale.isReversal = UIUtil.toBoolean(data(5))
                                    Case 6
                                        sale.isAmendment = UIUtil.toBoolean(data(6))
                                End Select
                            Next
                            businessReport.sales.Add(sale)
                        End If
                    Next
                ElseIf reportElements(i).Name = "purchases" Then
                    businessReport.purchases = New Collection
                    Dim purchase As PurchaseVO
                    For ci As Integer = 0 To reportElement.ChildNodes.Count - 1
                        If (reportElement.ChildNodes(ci).Name = "purchase") Then
                            purchase = New PurchaseVO
                            data = UIUtil.splitVals(reportElement.ChildNodes(ci).InnerText)
                            For di As Integer = 0 To data.Count - 1
                                Select Case di
                                    Case 0
                                        If UIUtil.isValidDate(data(0)) Then
                                            purchase.tranDate = UIUtil.parseDate(data(0), Nothing)
                                        End If
                                    Case 1
                                        purchase.suplrCode = data(1)
                                    Case 2
                                        purchase.qty = UIUtil.parseDouble(data(2))
                                    Case 3
                                        purchase.prodCode = data(3)
                                    Case 4
                                        purchase.cashPurchase = UIUtil.parseDouble(data(4))
                                    Case 5
                                        purchase.creditPurchase = UIUtil.parseDouble(data(5))
                                    Case 6
                                        purchase.comments = addBackComma(data(6))
                                    Case 7
                                        purchase.isReversal = UIUtil.toBoolean(data(7))
                                    Case 8
                                        purchase.isAmendment = UIUtil.toBoolean(data(8))
                                End Select
                            Next
                            businessReport.purchases.Add(purchase)
                        End If
                    Next
                ElseIf reportElements(i).Name = "expenses" Then
                    businessReport.expenses = New Collection
                    Dim exp As ExpenseVO
                    For ci As Integer = 0 To reportElement.ChildNodes.Count - 1
                        If (reportElement.ChildNodes(ci).Name = "expense") Then
                            exp = New ExpenseVO
                            data = UIUtil.splitVals(reportElement.ChildNodes(ci).InnerText)
                            For di As Integer = 0 To data.Count - 1
                                Select Case di
                                    Case 0
                                        If UIUtil.isValidDate(data(0)) Then
                                            exp.tranDate = UIUtil.parseDate(data(0), Nothing)
                                        End If
                                    Case 1
                                        exp.expCatCode = data(1)
                                    Case 2
                                        exp.expAmt = UIUtil.parseDouble(data(2))
                                    Case 3
                                        exp.comments = addBackComma(data(3))
                                    Case 4
                                        exp.isReversal = UIUtil.toBoolean(data(4))
                                    Case 5
                                        exp.isAmendment = UIUtil.toBoolean(data(5))
                                End Select
                            Next
                            businessReport.expenses.Add(exp)
                        End If
                    Next
                ElseIf reportElements(i).Name = "debt_payments" Then
                    businessReport.debtPayments = New Collection
                    Dim debt As DebtPaymentVO
                    For ci As Integer = 0 To reportElement.ChildNodes.Count - 1
                        If (reportElement.ChildNodes(ci).Name = "debt_payment") Then
                            debt = New DebtPaymentVO
                            data = UIUtil.splitVals(reportElement.ChildNodes(ci).InnerText)
                            For di As Integer = 0 To data.Count - 1
                                Select Case di
                                    Case 0
                                        If UIUtil.isValidDate(data(0)) Then
                                            debt.tranDate = UIUtil.parseDate(data(0), Nothing)
                                        End If
                                    Case 1
                                        debt.suplrCode = data(1)
                                    Case 2
                                        debt.debtAmtPaid = UIUtil.parseDouble(data(2))
                                    Case 3
                                        debt.comments = addBackComma(data(3))
                                    Case 4
                                        debt.isReversal = UIUtil.toBoolean(data(4))
                                    Case 5
                                        debt.isAmendment = UIUtil.toBoolean(data(5))
                                End Select
                            Next
                            businessReport.debtPayments.Add(debt)
                        End If
                    Next
                End If
            Next
            Return businessReport
        Catch ex As Exception

        End Try


    End Function

    Public Sub saveBusinessReportAsXML(businessReport As BusinessReport)

        Try
            Dim settings As New XmlWriterSettings
            Dim cntr As Integer
            Dim tranTxt As String

            settings.Indent = True
            settings.IndentChars = vbTab

            Dim writer As XmlWriter = XmlWriter.Create(businessReport.absXmlFileName, settings)

            writer.WriteStartDocument()

            writer.WriteStartElement("business-report")
            writer.WriteAttributeString("from_date", UIUtil.toDateString(businessReport.fromDate))
            writer.WriteAttributeString("to_date", UIUtil.toDateString(businessReport.toDate))

            writer.WriteAttributeString("is_amendment", UIUtil.toBinaryBooleanString(businessReport.isAnAmendmentToPostedTrans))

            ' overall related info
            writer.WriteStartElement("report_overall")
            writer.WriteElementString("cash_brought_forward", businessReport.cashBroughtForward.ToString)
            writer.WriteElementString("cash_counted", businessReport.cashCounted.ToString)
            writer.WriteElementString("cash_expected", businessReport.cashExpected.ToString)
            writer.WriteElementString("cash_counted_diff_reason", businessReport.reason4CashCountedVersusBroughtForwardDiff)
            writer.WriteEndElement()

            ' Supplier Modified 
            Dim supplier As SupplierModifiedVO
            If Not businessReport.isEmpty(businessReport.suppliersModified) Then
                writer.WriteStartElement("suppliers_modified")
                writer.WriteElementString("supplier_modified_header", "supplier_code, supplier_name, supplier_comments")
                For cntr = 1 To businessReport.suppliersModified.Count
                    supplier = businessReport.suppliersModified(cntr)
                    tranTxt = replComma(supplier.code)
                    tranTxt += ", " + replComma(supplier.name)
                    tranTxt += ", " + replComma(supplier.comments)
                    writer.WriteElementString(getEntityModTypString(supplier.modType), tranTxt)
                Next
                writer.WriteEndElement()
            End If

            ' Product Modified
            Dim product As ProductModifiedVO
            If Not businessReport.isEmpty(businessReport.productsModified) Then
                writer.WriteStartElement("products_modified")
                writer.WriteElementString("product_modified_header", "product_code, product_name, product_cat_code, product_qty, product_qty_uom, product_cost, product_ret_disc_room_perentage, product_min_gp_margin_percentage, product_ret_sale_price, product_comments, product_is_reorder, product_low_stock_alert_qty")
                For cntr = 1 To businessReport.productsModified.Count
                    product = businessReport.productsModified(cntr)
                    tranTxt = replComma(product.code)
                    tranTxt += ", " + replComma(product.name)
                    tranTxt += ", " + replComma(product.catCodeName)
                    tranTxt += ", " + product.qtyAvail.ToString
                    tranTxt += ", " + replComma(product.qtyUOM)
                    tranTxt += ", " + replComma(product.acbCost.ToString)
                    tranTxt += ", " + product.retDiscRoomPercentage.ToString
                    tranTxt += ", " + product.minRetGrossProfitMarginPercentage.ToString
                    tranTxt += ", " + product.retSalePrice.ToString
                    tranTxt += ", " + replComma(product.comments)
                    tranTxt += ", " + product.isReorder.ToString
                    tranTxt += ", " + product.lowStockAlertQty.ToString

                    writer.WriteElementString(getEntityModTypString(product.modType), tranTxt)
                Next
                writer.WriteEndElement()
            End If

            ' Expense Category 
            Dim expCat As ExpenseCategoryModifiedVO
            If Not businessReport.isEmpty(businessReport.expenseCategoriesModified) Then
                writer.WriteStartElement("expenses_categories_modified")
                writer.WriteElementString("expense_category_modified_header", "category_code, category_name, category_description")
                For cntr = 1 To businessReport.expenseCategoriesModified.Count
                    expCat = businessReport.expenseCategoriesModified(cntr)
                    tranTxt = replComma(expCat.code)
                    tranTxt += ", " + replComma(expCat.name)
                    tranTxt += ", " + replComma(expCat.desc)
                    writer.WriteElementString(getEntityModTypString(expCat.modType), tranTxt)
                Next
                writer.WriteEndElement()
            End If

            ' Sales
            Dim sale As SaleVO
            If Not businessReport.isEmpty(businessReport.sales) Then
                writer.WriteStartElement("sales")
                writer.WriteElementString("sale_header", "sale_sold_on, sale_prod_code, sale_qty, sale_price, sale_comments,is_reversal,is_amendment")
                For cntr = 1 To businessReport.sales.Count
                    sale = businessReport.sales(cntr)
                    tranTxt = UIUtil.toDateString(sale.tranDate)
                    tranTxt += ", " + sale.prodCode
                    tranTxt += ", " + sale.qty.ToString
                    tranTxt += ", " + sale.totalSaleAmt.ToString
                    tranTxt += ", " + replComma(sale.comments)
                    tranTxt += ", " + UIUtil.toBinaryBooleanString(sale.isReversal)
                    tranTxt += ", " + UIUtil.toBinaryBooleanString(sale.isAmendment)
                    writer.WriteElementString("sale", tranTxt)
                Next
                writer.WriteEndElement()
            End If

            ' Purchases
            Dim purchase As PurchaseVO
            If Not businessReport.isEmpty(businessReport.purchases) Then
                writer.WriteStartElement("purchases")
                writer.WriteElementString("purchase_header", "purchase_purchased_on, purchase_suplr_code, purchase_qty, purchase_prod_code, purchase_cash_amt, purchase_credit_amt, purchase_comments,is_reversal")
                For cntr = 1 To businessReport.purchases.Count
                    purchase = businessReport.purchases(cntr)
                    tranTxt = UIUtil.toDateString(purchase.tranDate)
                    tranTxt += ", " + purchase.suplrCode
                    tranTxt += ", " + purchase.qty.ToString
                    tranTxt += ", " + purchase.prodCode
                    tranTxt += ", " + purchase.cashPurchase.ToString
                    tranTxt += ", " + purchase.creditPurchase.ToString
                    tranTxt += ", " + replComma(purchase.comments)
                    tranTxt += ", " + UIUtil.toBinaryBooleanString(purchase.isReversal)
                    tranTxt += ", " + UIUtil.toBinaryBooleanString(purchase.isAmendment)
                    writer.WriteElementString("purchase", tranTxt)
                Next
                writer.WriteEndElement()
            End If

            ' Expenses
            Dim expense As ExpenseVO
            If Not businessReport.isEmpty(businessReport.expenses) Then
                writer.WriteStartElement("expenses")
                writer.WriteElementString("expense_header", "expense_expensed_on, expense_code, expense_amount, expense_comments,is_reversal")
                For cntr = 1 To businessReport.expenses.Count
                    expense = businessReport.expenses(cntr)
                    tranTxt = UIUtil.toDateString(expense.tranDate)
                    tranTxt += ", " + expense.expCatCode
                    tranTxt += ", " + expense.expAmt.ToString
                    tranTxt += ", " + replComma(expense.comments)
                    tranTxt += ", " + UIUtil.toBinaryBooleanString(expense.isReversal)
                    tranTxt += ", " + UIUtil.toBinaryBooleanString(expense.isAmendment)
                    writer.WriteElementString("expense", tranTxt)
                Next
                writer.WriteEndElement()
            End If

            ' Debt Paid
            Dim debtPaid As DebtPaymentVO
            If Not businessReport.isEmpty(businessReport.debtPayments) Then
                writer.WriteStartElement("debt_payments")
                writer.WriteElementString("debt_payment_header", "debt_payment_paid_on, debt_payment_suplr_code, debt_payment_amount_paid, debt_payment_comments,is_reversal")
                For cntr = 1 To businessReport.debtPayments.Count
                    debtPaid = businessReport.debtPayments(cntr)
                    tranTxt = UIUtil.toDateString(debtPaid.tranDate)
                    tranTxt += ", " + debtPaid.suplrCode
                    tranTxt += ", " + debtPaid.debtAmtPaid.ToString
                    tranTxt += ", " + replComma(debtPaid.comments)
                    tranTxt += ", " + UIUtil.toBinaryBooleanString(debtPaid.isReversal)
                    tranTxt += ", " + UIUtil.toBinaryBooleanString(debtPaid.isAmendment)
                    writer.WriteElementString("debt_payment", tranTxt)
                Next
                writer.WriteEndElement()
            End If

            writer.WriteEndElement()
            writer.WriteEndDocument()
            writer.Flush()
            writer.Close()
        Catch ex As Exception
            MessageBox.Show(mainForm, "Failed to save xml file: " + businessReport.absXmlFileName + " Reason:" + ex.ToString)
            Throw ex
        End Try



    End Sub

    Protected Function moveReport(xmlFileSrcAbs As String, xmlFileDestFolder As String) As String
        Dim folder As String
        Dim fileName As String
        ReportFile.sepFileNameFromFolder(xmlFileSrcAbs, folder, fileName)

        Dim destFile As String = MiscUtil.appendPathComponent(xmlFileDestFolder, fileName)
        Try
            FileSystem.FileCopy(destFile, destFile + "." + Format(Now(), ".yyyy-MM-dd-H-mm-ss"))
        Catch ex As FileNotFoundException
            'that is ok.
        End Try

        FileSystem.FileCopy(xmlFileSrcAbs, destFile)
        File.Delete(xmlFileSrcAbs)
        Return destFile
    End Function

    Public Sub emailReport(absXmlFileName As String)
        Dim serv As EmailService = New EmailService
        Dim title As String = Path.GetFileName(absXmlFileName)
        serv.emailFile(title, My.Settings.email_xml_to, absXmlFileName)
    End Sub


    Protected Function retrievePostedSaleTransactions(fromdate As Date, toDate As Date, connection As MySqlConnection) As Collection
        Dim sql As String

        sql = "select  "
        sql += "      sale.sold_on "
        sql += "      ,product.product_code "
        sql += "      ,product.product_name "
        sql += "      ,sale.qty_sold "
        sql += "      ,qty_uom.qty_uom_name "
        sql += "      ,sale.sale_amt "
        sql += "      ,sale.comments "
        sql += "      ,sale.sale_id "
        sql += "      ,sale.product_id "
        sql += "      ,qty_uom.qty_uom_id "
        sql += "      ,sale.is_amendment "
        sql += "      ,sale.is_reversal "
        sql += "from sale, qty_uom, product "
        sql += "where sale.qty_uom_id = qty_uom.qty_uom_id "
        sql += "and   sale.product_id = product.product_id "
        sql += "and   sale.sold_on >= " + getSqlDate(fromdate, False) + " "
        sql += "and   sale.sold_on <= " + getSqlDate(toDate, True) + " "
        sql += "order by sale.sold_on, product.product_code "

        Dim dt As DataTable = Nothing
        Try
            dt = New DataTable()
            Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
            adp.Fill(dt)
            Dim result As New Collection
            Dim sale As SaleVO
            For r As Integer = 0 To dt.Rows.Count - 1
                sale = New SaleVO
                sale.transactionId = dt.Rows(r).Item("sale_id")
                sale.tranDate = dt.Rows(r).Item("sold_on")
                sale.prodCode = dt.Rows(r).Item("product_code")
                sale.prodId = dt.Rows(r).Item("product_id")
                sale.qty = UIUtil.zeroIfEmpty(dt.Rows(r).Item("qty_sold"))
                sale.totalSaleAmt = UIUtil.zeroIfEmpty(dt.Rows(r).Item("sale_amt"))
                sale.comments = UIUtil.subsIfEmpty(dt.Rows(r).Item("comments"), "")
                sale.isPosted = True
                sale.isAmendment = UIUtil.toBoolean(dt.Rows(r).Item("is_amendment"))
                sale.isReversal = UIUtil.toBoolean(dt.Rows(r).Item("is_reversal"))
                result.Add(sale)
            Next
            Return result
        Finally
            If (Not IsNothing(dt)) Then
                dt.Dispose()
            End If
        End Try
    End Function

    Protected Function retrievePostedPurchaseTransactions(fromdate As Date, toDate As Date, connection As MySqlConnection) As Collection
        Dim sql As String

        sql = "Select purchase.purchase_id, "
        sql += "      purchase.purchased_on, "
        sql += "      purchase.supplier_id, "
        sql += "      supplier.supplier_code, "
        sql += "      supplier.supplier_name, "
        sql += "      purchase.product_id, "
        sql += "      product.product_code, "
        sql += "      product.product_name, "
        sql += "      purchase.amt_purchased, "
        sql += "      purchase.qty_purchased, "
        sql += "      qty_uom.qty_uom_id, "
        sql += "      qty_uom.qty_uom_name, "
        sql += "      purchase.purchase_amt_paid_cash, "
        sql += "      (purchase.amt_purchased - purchase.purchase_amt_paid_cash) As purchase_amt_paid_credit, "
        sql += "      purchase.comments, "
        sql += "      purchase.is_amendment, "
        sql += "      purchase.is_reversal "
        sql += "From supplier, purchase, qty_uom, product "
        sql += "Where purchase.supplier_id = supplier.supplier_id "
        sql += "and   purchase.product_id = product.product_id "
        sql += "and   purchase.qty_uom_id = qty_uom.qty_uom_id "
        sql += "and   purchase.purchased_on >= " + getSqlDate(fromdate, False) + " "
        sql += "and   purchase.purchased_on <= " + getSqlDate(toDate, True) + " "
        sql += "order by purchase.purchased_on, supplier.supplier_code, product.product_code "

        Dim dt As DataTable = Nothing
        Try
            dt = New DataTable()
            Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
            adp.Fill(dt)
            Dim result As New Collection
            Dim purchase As PurchaseVO
            For r As Integer = 0 To dt.Rows.Count - 1
                purchase = New PurchaseVO
                purchase.transactionId = dt.Rows(r).Item("purchase_id")
                purchase.tranDate = dt.Rows(r).Item("purchased_on")
                purchase.suplrCode = dt.Rows(r).Item("supplier_code")
                purchase.suplrId = dt.Rows(r).Item("supplier_id")
                purchase.prodCode = dt.Rows(r).Item("product_code")
                purchase.prodId = dt.Rows(r).Item("product_id")
                purchase.qty = UIUtil.zeroIfEmpty(dt.Rows(r).Item("qty_purchased"))
                purchase.cashPurchase = UIUtil.zeroIfEmpty(dt.Rows(r).Item("purchase_amt_paid_cash"))
                purchase.creditPurchase = UIUtil.zeroIfEmpty(dt.Rows(r).Item("purchase_amt_paid_credit"))
                purchase.comments = UIUtil.subsIfEmpty(dt.Rows(r).Item("comments"), "")
                purchase.isPosted = True
                purchase.isAmendment = UIUtil.toBoolean(dt.Rows(r).Item("is_amendment"))
                purchase.isReversal = UIUtil.toBoolean(dt.Rows(r).Item("is_reversal"))
                result.Add(purchase)
            Next
            Return result
        Finally
            If (Not IsNothing(dt)) Then
                dt.Dispose()
            End If
        End Try
    End Function

    Protected Function retrievePostedExpenseTransactions(fromdate As Date, toDate As Date, connection As MySqlConnection) As Collection
        Dim sql As String

        sql = "Select expense.expense_id, "
        sql += "      expense.expensed_on, "
        sql += "      expense.expense_amt, "
        sql += "      expense.expense_category_id, "
        sql += "      expense_category.expense_category_code, "
        sql += "      expense_category.expense_category_name, "
        sql += "      expense.comments, "
        sql += "      expense.is_amendment, "
        sql += "      expense.is_reversal "
        sql += "From  expense, expense_category "
        sql += "Where expense.expense_category_id = expense_category.expense_category_id "
        sql += "and   expense.expensed_on >= " + getSqlDate(fromdate, False) + " "
        sql += "and   expense.expensed_on <= " + getSqlDate(toDate, True) + " "
        sql += "order by expense.expensed_on, expense_category.expense_category_code "

        Dim dt As DataTable = Nothing
        Try
            dt = New DataTable()
            Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
            adp.Fill(dt)
            Dim result As New Collection
            Dim expense As ExpenseVO
            For r As Integer = 0 To dt.Rows.Count - 1
                expense = New ExpenseVO
                expense.transactionId = dt.Rows(r).Item("expense_id")
                expense.tranDate = dt.Rows(r).Item("expensed_on")
                expense.expCatCode = dt.Rows(r).Item("expense_category_code")
                expense.expCatId = dt.Rows(r).Item("expense_category_id")
                expense.expAmt = UIUtil.zeroIfEmpty(dt.Rows(r).Item("expense_amt"))
                expense.comments = UIUtil.subsIfEmpty(dt.Rows(r).Item("comments"), "")
                expense.isPosted = True
                expense.isAmendment = UIUtil.toBoolean(dt.Rows(r).Item("is_amendment"))
                expense.isReversal = UIUtil.toBoolean(dt.Rows(r).Item("is_reversal"))
                result.Add(expense)
            Next
            Return result
        Finally
            If (Not IsNothing(dt)) Then
                dt.Dispose()
            End If
        End Try
    End Function

    Protected Function retrievePostedDebtPaymentsTransactions(fromdate As Date, toDate As Date, connection As MySqlConnection) As Collection
        Dim sql As String

        sql = " Select payment.payment_id, "
        sql += "       payment.paid_on, "
        sql += "       payment.supplier_id, "
        sql += "       supplier.supplier_code, "
        sql += "       supplier.supplier_name, "
        sql += "       payment.amt_paid, "
        sql += "       payment.comments, "
        sql += "       payment.is_amendment, "
        sql += "       payment.is_reversal "
        sql += "From payment, supplier "
        sql += "Where payment.supplier_id = supplier.supplier_id "
        sql += "and   payment.paid_on >= " + getSqlDate(fromdate, False) + " "
        sql += "and   payment.paid_on <= " + getSqlDate(toDate, True) + " "
        sql += "order by payment.paid_on, supplier.supplier_code "

        Dim dt As DataTable = Nothing
        Try
            dt = New DataTable()
            Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
            adp.Fill(dt)
            Dim result As New Collection
            Dim payment As DebtPaymentVO
            For r As Integer = 0 To dt.Rows.Count - 1
                payment = New DebtPaymentVO
                payment.transactionId = dt.Rows(r).Item("payment_id")
                payment.tranDate = dt.Rows(r).Item("paid_on")
                payment.suplrCode = dt.Rows(r).Item("supplier_code")
                payment.suplrId = dt.Rows(r).Item("supplier_id")
                payment.debtAmtPaid = UIUtil.zeroIfEmpty(dt.Rows(r).Item("amt_paid"))
                payment.comments = UIUtil.subsIfEmpty(dt.Rows(r).Item("comments"), "")
                payment.isPosted = True
                payment.isAmendment = UIUtil.toBoolean(dt.Rows(r).Item("is_amendment"))
                payment.isReversal = UIUtil.toBoolean(dt.Rows(r).Item("is_reversal"))
                result.Add(payment)
            Next
            Return result
        Finally
            If (Not IsNothing(dt)) Then
                dt.Dispose()
            End If
        End Try
    End Function

    Public Function retrievePostedBusinessTransactions(fromdate As Date, toDate As Date) As BusinessReportDAO.BusinessReport
        Dim connection As MySqlConnection = Nothing

        Try
            Dim rep As New BusinessReportDAO.BusinessReport
            rep.fromDate = fromdate
            rep.toDate = toDate
            connection = connectToDB()
            mainForm.showProgress(25, "Loading Sale Transactions...")
            rep.sales = retrievePostedSaleTransactions(fromdate, toDate, connection)
            mainForm.showProgress(25, "Loading Purchase Transactions...")
            rep.purchases = retrievePostedPurchaseTransactions(fromdate, toDate, connection)
            mainForm.showProgress(25, "Loading Expense Transactions...")
            rep.expenses = retrievePostedExpenseTransactions(fromdate, toDate, connection)
            mainForm.showProgress(25, "Loading Debt Payment Transactions...")
            rep.debtPayments = retrievePostedDebtPaymentsTransactions(fromdate, toDate, connection)
            Return rep
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(connection) Then
                connection.Close()
            End If
        End Try

    End Function

    Public Sub loadBusinessReport(businessReport As BusinessReport)
        Dim connection As MySqlConnection = Nothing
        Dim trans As MySqlTransaction = Nothing

        Try
            backUpDatabase()
            Dim mainForm As MainForm = My.Application.OpenForms("MainForm")
            connection = connectToDB()
            trans = connection.BeginTransaction
            Dim unqPrdIdsTransacted As List(Of Integer) = saveBusinessReportIntoDatabase(businessReport, connection)
            mainForm.showProgress(33, "Loading Business Report.")
            businessReport.absXmlFileName = moveReport(businessReport.absXmlFileName, ReportFile.getLoadedFolderName())
            recordLoadSubmitStatus(businessReport, "LOAD", connection)
            mainForm.showProgress(66, "Loading business report.  Backed up XML report.")
            trans.Commit()
            trans = connection.BeginTransaction
            recalcProductAcbAndQty(unqPrdIdsTransacted, connection)
            trans.Commit()
            mainForm.showProgress(100, "Loading business report.  Calculated ACB.")
        Catch ex As Exception
            If Not IsNothing(trans) Then
                trans.Rollback()
            End If

            Throw ex
        Finally
            If Not IsNothing(trans) Then
                trans.Dispose()
            End If
            If Not IsNothing(connection) Then
                connection.Close()
            End If
        End Try

    End Sub

    Public Sub recordLoadSubmitStatus(businessReport As BusinessReport, type As String, cn As MySqlConnection)
        Dim sqlStr As String
        Dim reportType As String = If(businessReport.isAnAmendmentToPostedTrans, "AMENDMENT", "BUSINESS")

        'sqlStr = "delete from report_load_submit_status "
        'sqlStr += "where report_from = " + getSqlDate(businessReport.fromDate) + " "
        'sqlStr += "and type = " + prepStrSql(type) + " "
        'sqlStr += "and report_type = " + prepStrSql(reportType) + " "
        'executeNonRetrievalSql(sqlStr, cn)

        sqlStr = "insert into report_load_submit_status values (" + getSqlDate(businessReport.fromDate) + ", "
        sqlStr += prepStrSql(type) + ","
        sqlStr += prepStrSql(Format(Now(), "yyyy-MM-dd H:mm:ss")) + ","

        sqlStr += getSqlDate(businessReport.toDate) + ","
        sqlStr += businessReport.cashBroughtForward.ToString + ","
        sqlStr += prepStrSql(reportType) + ","
        sqlStr += businessReport.cashCounted.ToString + ", "

        sqlStr += prepStrSql(businessReport.absXmlFileName.Replace("\", "\\")) + ","
        sqlStr += prepStrSql(businessReport.reason4CashCountedVersusBroughtForwardDiff) + ","
        sqlStr += businessReport.cashExpected.ToString
        sqlStr += ")"
        executeNonRetrievalSql(sqlStr, cn)
    End Sub

    Public Function retrieveLastBusinessReportLoadedSubmitted(reportTyp As ReportType) As ReportLoadSubmitStatusVO
        Dim sqlStr As String
        Dim stat As ReportLoadSubmitStatusVO = Nothing

        Dim repType As String = If(reportTyp = ReportType.business, "BUSINESS", "AMENDMENT")
        sqlStr = "select report_from, "
        sqlStr += "type, "
        sqlStr += "report_to, "
        sqlStr += "cash_brought_forward, "
        sqlStr += "cash_counted, "
        sqlStr += "report_type, "
        sqlStr += "report_file_loc, "
        sqlStr += "comments, "
        sqlStr += "expected_cash "
        sqlStr += "From report_load_submit_status "
        sqlStr += "Where report_load_submit_status.report_from = (Select max(report_load_submit_status.report_from) "
        sqlStr += "                                               From report_load_submit_status "
        sqlStr += "                                               Where report_load_submit_status.report_type =  " + prepStrSql(repType)
        sqlStr += "                                               ) "
        sqlStr += "and report_load_submit_status.report_type =  " + prepStrSql(repType)

        Dim dt As DataTable
        Dim colVal As String
        execRetQry(sqlStr, dt)
        If (Not IsNothing(dt) AndAlso dt.Rows.Count > 0) Then
            stat = New ReportLoadSubmitStatusVO
            stat.reportFrom = dt.Rows(0).Item("report_from")
            stat.reportTo = dt.Rows(0).Item("report_to")
            colVal = dt.Rows(0).Item("type")
            stat.submitType = If(UCase(colVal) = "LOAD", ReportSubmitType.load, ReportSubmitType.submit)
            colVal = dt.Rows(0).Item("report_type")
            stat.reportType = If(UCase(colVal) = "BUSINESS", ReportType.business, ReportType.amendment)
            stat.cashBroughtForward = UIUtil.parseDouble(dt.Rows(0).Item("cash_brought_forward"))
            stat.cashCounted = UIUtil.parseDouble(dt.Rows(0).Item("cash_counted"))
            stat.reportFileLocation = dt.Rows(0).Item("report_file_loc")
            stat.cashExpected = UIUtil.parseDouble(dt.Rows(0).Item("expected_cash"))
            stat.reason4CashDiff = StringUtil.isNullThenEmptyStr(dt.Rows(0).Item("comments"))
            Return stat
        End If

    End Function


    Public Function submitBusinessReport(businessReport As BusinessReport) As List(Of Integer)
        Dim connection As MySqlConnection = Nothing
        Dim trans As MySqlTransaction = Nothing

        Try
            backUpDatabase()
            Dim mainForm As MainForm = My.Application.OpenForms("MainForm")
            connection = connectToDB()
            trans = connection.BeginTransaction
            Dim unqPrdIdsTransacted As List(Of Integer) = saveBusinessReportIntoDatabase(businessReport, connection)
            saveBusinessReportAsXML(businessReport)
            mainForm.showProgress(25, "Submiting Business Report.  Prepared XML report.")
            emailReport(businessReport.absXmlFileName)
            mainForm.showProgress(50, "Submiting Business Report.  Emailed XML report...")
            businessReport.absXmlFileName = moveReport(businessReport.absXmlFileName, ReportFile.getSubmitedFolderName())
            recordLoadSubmitStatus(businessReport, "SUBMIT", connection)
            mainForm.showProgress(75, "Submiting Business Report.  Backed up XML report.")
            trans.Commit()
            trans = connection.BeginTransaction
            recalcProductAcbAndQty(unqPrdIdsTransacted, connection)
            trans.Commit()
            mainForm.showProgress(100, "Submiting Business Report.  Calculated ACB.")
            Return unqPrdIdsTransacted
        Catch ex As Exception
            If Not IsNothing(trans) Then
                trans.Rollback()
            End If

            Throw ex
        Finally
            If Not IsNothing(trans) Then
                trans.Dispose()
            End If
            If Not IsNothing(connection) Then
                connection.Close()
            End If
        End Try

    End Function

    Public Function generateBusinessReportName(fromDate As Date, toDate As Date)
        generateBusinessReportName = fromDate + " - " + toDate
    End Function


    '========================================== DATABASE =================================================
    Function getAuditSqlStr(isUpdate As Boolean)
        Dim dateTimeStr As String = Format(Now(), "yyyy-MM-dd H:mm:ss")
        Dim updatedBy As String = "Bulk Uploader"
        Dim sqlStr As String = ""
        If (isUpdate) Then
            sqlStr += "updated_on = '" + dateTimeStr + "', "
            sqlStr += "updated_by = '" + updatedBy + "'"
        Else
            sqlStr = "'" + dateTimeStr + "',"
            sqlStr += "'" + updatedBy + "'"
        End If

        getAuditSqlStr = sqlStr
    End Function

    Protected Sub deleteReportComponentForPeriod(fromDate As Date, toDate As Date, busReprtComponentTblName As String, busReprtComponentDateColName As String, cn As MySqlConnection)
        Dim sqlStr As String
        sqlStr = "delete from " + busReprtComponentTblName + " "
        sqlStr += "where " + busReprtComponentDateColName + " >= " + getSqlDate(fromDate) + " "
        sqlStr += "and   " + busReprtComponentDateColName + " <= " + getSqlDate(toDate) + " "
        executeNonRetrievalSql(sqlStr, cn)
    End Sub

    Protected Sub saveProductsModified(productsModified As Collection, cn As MySqlConnection)
        Dim sqlStr As String

        Dim prd As ProductModifiedVO
        Dim i As Integer
        For i = 1 To productsModified.Count
            prd = productsModified(i)
            Select Case prd.modType
                Case ReportTypeEntityModType.add
                    executeNonRetrievalSql("delete from product where product_code = " + prepStrSql(prd.code), cn)

                    sqlStr = "  INSERT INTO product ( "
                    sqlStr += "  product_id, "
                    sqlStr += "  product_code, "
                    sqlStr += "  product_name, "
                    sqlStr += "  product_category_id, "
                    sqlStr += "  qty_available, "
                    sqlStr += "  qty_uom_id, "
                    sqlStr += "  acb_cost, "
                    sqlStr += "  retail_discount_room_percentage, "
                    sqlStr += "  min_ret_gross_profit_margin_percentage, "
                    sqlStr += "  retail_sale_price, "
                    sqlStr += "  comments, "
                    sqlStr += " low_stock_alert_qty, "
                    sqlStr += " is_reorder, "
                    sqlStr += "  updated_on, "
                    sqlStr += "  updated_by)  "
                    sqlStr += "  VALUES(NULL, "
                    sqlStr += prepStrSql(prd.code) + ", "
                    sqlStr += prepStrSql(prd.name) + ", "
                    sqlStr += lookupProductCategoryID(prd.catCodeName, cn).ToString + ", "
                    sqlStr += prd.qtyAvail.ToString + ", "
                    sqlStr += lookupUOMId(prd.qtyUOM, cn).ToString + ", "
                    sqlStr += prd.acbCost.ToString + ", "
                    sqlStr += prd.retDiscRoomPercentage.ToString + ", "
                    sqlStr += prd.minRetGrossProfitMarginPercentage.ToString + ", "
                    sqlStr += prd.retSalePrice.ToString + ", "
                    sqlStr += prepStrSql(prd.comments) + ", "
                    sqlStr += prd.lowStockAlertQty.ToString + ", "
                    sqlStr += prd.isReorder.ToString + ", "
                    sqlStr += getAuditSqlStr(False) + " "
                    sqlStr += ") "
                    executeNonRetrievalSql(sqlStr, cn)
                Case ReportTypeEntityModType.delete
                    sqlStr = "delete from product where product_code = " + prepStrSql(prd.code)
                    executeNonRetrievalSql(sqlStr, cn)
                Case ReportTypeEntityModType.update
                    sqlStr = "update product set "
                    sqlStr += "  product_name = " + prepStrSql(prd.name) + ",  "
                    sqlStr += "  product_category_id = " + lookupProductCategoryID(prd.catCodeName, cn).ToString + ",  "
                    sqlStr += "  qty_available  = " + prd.qtyAvail.ToString + ",  "
                    sqlStr += "  qty_uom_id =  " + lookupUOMId(prd.qtyUOM, cn).ToString + ",  "
                    sqlStr += "  acb_cost =  " + prd.acbCost.ToString + ",  "
                    sqlStr += "  retail_discount_room_percentage = " + prd.retDiscRoomPercentage.ToString + ",  "
                    sqlStr += "  min_ret_gross_profit_margin_percentage = " + prd.minRetGrossProfitMarginPercentage.ToString + ", "
                    sqlStr += "  retail_sale_price = " + prd.retSalePrice.ToString + ", "
                    sqlStr += "  comments = " + prepStrSql(prd.comments) + ", "
                    sqlStr += "  low_stock_alert_qty = " + prd.lowStockAlertQty.ToString + ", "
                    sqlStr += "  is_reorder = " + prd.isReorder.ToString + ", "
                    sqlStr += getAuditSqlStr(True) + " "
                    sqlStr += "where product_code = " + prepStrSql(prd.code)
                    executeNonRetrievalSql(sqlStr, cn)
            End Select

        Next
    End Sub

    Protected Sub saveExpenseCategoriesModified(expCatsModified As Collection, cn As MySqlConnection)
        Dim sqlStr As String

        Dim expCat As ExpenseCategoryModifiedVO
        Dim i As Integer
        For i = 1 To expCatsModified.Count
            expCat = expCatsModified(i)
            Select Case expCat.modType
                Case ReportTypeEntityModType.add
                    executeNonRetrievalSql("delete from expense_category where expense_category_code = " + prepStrSql(expCat.code), cn)
                    sqlStr = "INSERT INTO expense_category ( "
                    sqlStr += "  expense_category_id, "
                    sqlStr += "  expense_category_code,  "
                    sqlStr += "  expense_category_name, "
                    sqlStr += "  expense_category_description, "
                    sqlStr += "  updated_on, "
                    sqlStr += "  updated_by) VALUES (NULL, "
                    sqlStr += prepStrSql(expCat.code) + ", "
                    sqlStr += prepStrSql(expCat.name) + ", "
                    sqlStr += prepStrSql(expCat.desc) + ", "
                    sqlStr += getAuditSqlStr(False) + " "
                    sqlStr += ") "
                    executeNonRetrievalSql(sqlStr, cn)
                Case ReportTypeEntityModType.delete
                    sqlStr = "delete from expense_category where expense_category_code = " + prepStrSql(expCat.code)
                    executeNonRetrievalSql(sqlStr, cn)
                Case ReportTypeEntityModType.update
                    sqlStr = "update expense_category Set "
                    sqlStr += "  expense_category_name = " + prepStrSql(expCat.name) + ",  "
                    sqlStr += "  expense_category_description = " + prepStrSql(expCat.desc) + ",  "
                    sqlStr += getAuditSqlStr(True) + " "
                    sqlStr += "where expense_category_code = " + prepStrSql(expCat.code)
                    executeNonRetrievalSql(sqlStr, cn)
            End Select
        Next
    End Sub

    Protected Sub saveSuppliersModified(suppliersModified As Collection, cn As MySqlConnection)
        Dim sqlStr As String

        Dim supp As SupplierModifiedVO
        Dim i As Integer
        For i = 1 To suppliersModified.Count
            supp = suppliersModified(i)
            Select Case supp.modType
                Case ReportTypeEntityModType.add
                    executeNonRetrievalSql("delete from supplier where supplier_code = " + prepStrSql(supp.code), cn)
                    sqlStr = "INSERT INTO supplier ( "
                    sqlStr += "  supplier_id, "
                    sqlStr += "  supplier_code,  "
                    sqlStr += "  supplier_name, "
                    sqlStr += "  comments, "
                    sqlStr += "  updated_on, "
                    sqlStr += "  updated_by) VALUES (NULL, "
                    sqlStr += prepStrSql(supp.code) + ", "
                    sqlStr += prepStrSql(supp.name) + ", "
                    sqlStr += prepStrSql(supp.comments) + ", "
                    sqlStr += getAuditSqlStr(False) + " "
                    sqlStr += ") "
                    executeNonRetrievalSql(sqlStr, cn)
                Case ReportTypeEntityModType.delete
                    sqlStr = "delete from supplier where supplier_code = " + prepStrSql(supp.code)
                    executeNonRetrievalSql(sqlStr, cn)
                Case ReportTypeEntityModType.update
                    sqlStr = "update supplier Set "
                    sqlStr += "  supplier_name = " + prepStrSql(supp.name) + ",  "
                    sqlStr += "  comments = " + prepStrSql(supp.comments) + ",  "
                    sqlStr += getAuditSqlStr(True) + " "
                    sqlStr += "where supplier_code = " + prepStrSql(supp.code)
                    executeNonRetrievalSql(sqlStr, cn)
            End Select


        Next
    End Sub

    Protected Sub saveBusinessReportEntities(businessReport As BusinessReport, connection As MySqlConnection)
        saveProductsModified(businessReport.productsModified, connection)
        saveExpenseCategoriesModified(businessReport.expenseCategoriesModified, connection)
        saveSuppliersModified(businessReport.suppliersModified, connection)
    End Sub

    Protected Function saveBusinessReportTransactions(businessReport As BusinessReport, connection As MySqlConnection) As List(Of Integer)
        Dim unqPrdIdsTransacted As List(Of Integer) = New List(Of Integer)
        savePurchases(businessReport.purchases, connection, unqPrdIdsTransacted)
        saveSales(businessReport.sales, connection, unqPrdIdsTransacted)
        saveExpenses(businessReport.expenses, connection)
        saveDebtPaid(businessReport.debtPayments, connection)
        Return unqPrdIdsTransacted
    End Function

    Public Function saveBusinessReportIntoDatabase(businessReport As BusinessReport, connection As MySqlConnection) As List(Of Integer)
        Try
            saveBusinessReportEntities(businessReport, connection)
            Return saveBusinessReportTransactions(businessReport, connection)
        Catch ex As Exception
            MessageBox.Show(mainForm, "Failed To save data into database. Reason:" + ex.ToString)
            Throw ex
        End Try

    End Function



    Function lookupProductCode(prodId As Integer, cn As MySqlConnection) As String
        Dim sqlStr As String = "SELECT product_code FROM product WHERE product_id = " + prodId.ToString + " "
        Dim code As String = executeSingleRetreivalSql(sqlStr, cn)
        If IsNothing(code) Then
            Throw New ArgumentException("No Code for product ID: " + prodId.ToString)
        End If
        lookupProductCode = code
    End Function

    Function lookupProductID(prodCode As String, cn As MySqlConnection) As Integer
        Dim sqlStr As String = "SELECT product_id FROM product WHERE product_code = '" + prodCode + "'"
        Dim id As Integer = executeSingleRetreivalSql(sqlStr, cn)
        If IsNothing(id) Then
            Throw New ArgumentException("No ID for product code: " + prodCode)
        End If
        lookupProductID = id
    End Function

    Function lookupProductCategoryID(catCodeName As String, cn As MySqlConnection) As Integer
        Dim sqlStr As String = "select product_category.product_category_id from product_category where product_category.product_category_name = '" + catCodeName + "'"
        Dim id As Integer = executeSingleRetreivalSql(sqlStr, cn)
        If IsNothing(id) Then
            Throw New ArgumentException("No ID for product category for name: " + catCodeName)
        End If
        lookupProductCategoryID = id
    End Function

    Function lookupProductUOM(prodCode As String, cn As MySqlConnection)
        Dim sqlStr As String = "SELECT qty_uom.qty_uom_name FROM product, qty_uom WHERE product.qty_uom_id = qty_uom.qty_uom_id and product_code = '" + prodCode + "'"
        Dim uom As String = executeSingleRetreivalSql(sqlStr, cn)
        If IsNothing(uom) Then
            Throw New ArgumentException("No UOM Name for product code: " + prodCode)
        End If
        lookupProductUOM = uom
    End Function

    Function lookupUOMId(uom As String, cn As MySqlConnection)
        Dim sqlStr As String = "SELECT qty_uom.qty_uom_id FROM qty_uom WHERE qty_uom.qty_uom_name = '" + uom + "'"
        Dim id As Integer = executeSingleRetreivalSql(sqlStr, cn)
        If IsNothing(id) Then
            Throw New ArgumentException("No ID for uom name: " + uom)
        End If
        lookupUOMId = id
    End Function

    Function lookupSupplierID(supplierCode As String, cn As MySqlConnection)
        Dim sqlStr As String = "SELECT supplier_id FROM supplier WHERE supplier_code = '" + supplierCode + "'"
        Dim id As Integer = executeSingleRetreivalSql(sqlStr, cn)
        If IsNothing(id) Then
            Throw New ArgumentException("No ID for supplier code: " + supplierCode)
        End If
        lookupSupplierID = id
    End Function

    Function lookupExpenseCategoryID(expCatCode As String, cn As MySqlConnection)
        Dim sqlStr As String = "SELECT expense_category_id FROM expense_category WHERE expense_category_code = '" + expCatCode + "'"
        Dim id As Integer = executeSingleRetreivalSql(sqlStr, cn)
        If IsNothing(id) Then
            Throw New ArgumentException("No ID for expense category code: " + expCatCode)
        End If
        lookupExpenseCategoryID = id
    End Function

    Function getSqlDate(d As Date, Optional endOfDay As Boolean = False) As String
        Return "'" + Format(d, "yyyy-MM-dd") + " " + If(endOfDay, "23:59:59", "") + "'"
        ' getSqlDate = "'" + CStr(DatePart("yyyy", d)) + "-" + CStr(DatePart("m", d)) + "-" + CStr(DatePart("d", d)) + "'"
    End Function

    Public Sub saveSales(sales As Collection, cn As MySqlConnection, ByRef unqPrdIdsTransacted As List(Of Integer))
        Dim sale As SaleVO
        Dim sqlStr As String
        Dim i As Integer
        Dim prdId As Integer

        For i = 1 To sales.Count
            sale = sales(i)
            prdId = lookupProductID(sale.prodCode, cn)
            If (Not unqPrdIdsTransacted.Contains(prdId)) Then
                unqPrdIdsTransacted.Add(prdId)
            End If
            sqlStr = "insert into sale values (NULL, "
            sqlStr += getSqlDate(sale.tranDate) + ","
            sqlStr += CStr(prdId) + ","
            sqlStr += "0,"
            sqlStr += UIUtil.zeroIfEmptyStr(sale.totalSaleAmt) + ","
            sqlStr += UIUtil.zeroIfEmptyStr(sale.qty) + ","
            sqlStr += CStr(lookupUOMId(lookupProductUOM(sale.prodCode, cn), cn)) + ", "
            sqlStr += prepStrSql(sale.comments) + " ,"
            sqlStr += UIUtil.toBinaryBooleanString(sale.isAmendment) + " ,"
            sqlStr += UIUtil.toBinaryBooleanString(sale.isReversal) + " ,"
            sqlStr += getAuditSqlStr(False)
            sqlStr += ")"
            executeNonRetrievalSql(sqlStr, cn)
        Next
    End Sub

    Public Sub savePurchases(purchases As Collection, cn As MySqlConnection, ByRef unqPrdIdsTransacted As List(Of Integer))
        Dim sqlStr As String
        Dim prdId As Integer
        Dim i As Integer
        Dim purchase As PurchaseVO
        Dim suplrId As Integer

        For i = 1 To purchases.Count
            purchase = purchases(i)
            prdId = lookupProductID(purchase.prodCode, cn)
            If (Not unqPrdIdsTransacted.Contains(prdId)) Then
                unqPrdIdsTransacted.Add(prdId)
            End If
            suplrId = lookupSupplierID(purchase.suplrCode, cn)
            sqlStr = "insert into purchase values (NULL, "
            sqlStr += getSqlDate(purchase.tranDate) + ", "
            sqlStr += CStr(suplrId) + ", "
            sqlStr += CStr(prdId) + ", "
            sqlStr += (UIUtil.zeroIfEmpty(purchase.cashPurchase) + UIUtil.zeroIfEmpty(purchase.creditPurchase)).ToString + ", "
            sqlStr += UIUtil.zeroIfEmptyStr(purchase.qty) + ", "
            sqlStr += CStr(lookupUOMId(lookupProductUOM(purchase.prodCode, cn), cn)) + ", "
            sqlStr += UIUtil.zeroIfEmptyStr(purchase.cashPurchase.ToString) + ", "
            sqlStr += prepStrSql(purchase.comments) + ", "
            sqlStr += UIUtil.toBinaryBooleanString(purchase.isAmendment) + " ,"
            sqlStr += UIUtil.toBinaryBooleanString(purchase.isReversal) + " ,"
            sqlStr += getAuditSqlStr(False)
            sqlStr += ") "
            executeNonRetrievalSql(sqlStr, cn)

            If purchase.cashPurchase > 0 Then
                insertDebtPaid(purchase.tranDate, suplrId, purchase.cashPurchase, purchase.comments, purchase.isAmendment, purchase.isReversal, cn)
            End If
        Next

    End Sub

    Protected Sub saveExpenses(expenses As Collection, cn As MySqlConnection)
        Dim sqlStr As String
        Dim i As Integer
        Dim expense As ExpenseVO

        For i = 1 To expenses.Count
            expense = expenses(i)
            sqlStr = "insert into expense values (NULL, "
            sqlStr += getSqlDate(expense.tranDate) + ","
            sqlStr += UIUtil.zeroIfEmptyStr(expense.expAmt) + ","
            sqlStr += CStr(lookupExpenseCategoryID(expense.expCatCode, cn)) + ","
            sqlStr += prepStrSql(expense.comments) + ","
            sqlStr += UIUtil.toBinaryBooleanString(expense.isAmendment) + " ,"
            sqlStr += UIUtil.toBinaryBooleanString(expense.isReversal) + " ,"
            sqlStr += getAuditSqlStr(False)
            sqlStr += ")"
            executeNonRetrievalSql(sqlStr, cn)
        Next

    End Sub

    Protected Sub insertDebtPaid(datePaid As Date, suplrId As Integer, amtPaid As Integer, comments As String, isAmendment As Boolean, isReversal As Boolean, cn As MySqlConnection)
        Dim sqlStr As String
        sqlStr = "insert into payment values (NULL,"
        sqlStr += getSqlDate(datePaid) + "," 'date paid
        sqlStr += CStr(suplrId) + ","
        sqlStr += UIUtil.zeroIfEmptyStr(amtPaid) + ","
        sqlStr += prepStrSql(comments) + "," 'comments
        sqlStr += UIUtil.toBinaryBooleanString(isAmendment) + " ,"
        sqlStr += UIUtil.toBinaryBooleanString(isReversal) + " ,"
        sqlStr += getAuditSqlStr(False) + ")"
        executeNonRetrievalSql(sqlStr, cn)
    End Sub

    Protected Sub saveDebtPaid(debtPayments As Collection, cn As MySqlConnection)
        Dim i As Integer
        Dim debtPaid As DebtPaymentVO
        Dim insDebtPassed As Boolean = True
        Dim suplrId As Integer
        For i = 1 To debtPayments.Count
            debtPaid = debtPayments(i)
            suplrId = lookupSupplierID(debtPaid.suplrCode, cn)
            insertDebtPaid(debtPaid.tranDate, suplrId, debtPaid.debtAmtPaid, debtPaid.comments, debtPaid.isAmendment, debtPaid.isReversal, cn)
            If Not (insDebtPassed) Then
                Exit For
            End If
        Next
    End Sub

    Protected Sub updateProductPurchaseQtyAndACB(prodId As Integer, qty As Integer, acb As Double, cn As MySqlConnection, ByRef negProdCodes As List(Of String))


        Dim sqlStr As String

        sqlStr = "UPDATE product SET "
        sqlStr += "acb_cost = " + acb.ToString + ", "
        sqlStr += "qty_available = " + qty.ToString + ", "
        sqlStr += getAuditSqlStr(True)
        sqlStr += "WHERE product_id = " + CStr(prodId)
        executeNonRetrievalSql(sqlStr, cn)
    End Sub


    Public Function recalcAllProductAcbAndQty() As Boolean
        Dim ds As DataTable = Nothing
        Dim connection As MySqlConnection = Nothing
        Dim trans As MySqlTransaction = Nothing

        Try
            ds = New DataTable
            connection = connectToDB()
            Dim adp As MySqlDataAdapter = New MySqlDataAdapter("Select product_id From product Order By product_id", connection)
            adp.Fill(ds)
            Dim prodIds As New List(Of Integer)
            For r As Integer = 0 To ds.Rows.Count - 1
                prodIds.Add(ds.Rows(r).Item(0))
            Next

            trans = connection.BeginTransaction
            Dim isComplete As Boolean = recalcProductAcbAndQty(prodIds, connection)
            trans.Commit()
            Return isComplete
        Catch ex As Exception
            trans.Rollback()
            Throw ex
        Finally
            If Not IsNothing(ds) Then
                ds.Dispose()
            End If
            If (Not IsNothing(connection)) Then
                connection.Close()
            End If
        End Try

    End Function

    Public Function merge(purchaseTbl As DataTable, saleTbl As DataTable) As List(Of DataRow)

        Dim res As New List(Of DataRow)

        For r As Integer = 0 To purchaseTbl.Rows.Count - 1
            res.Add(purchaseTbl.Rows(r))
        Next

        For r As Integer = 0 To saleTbl.Rows.Count - 1
            res.Add(saleTbl.Rows(r))
        Next

        res.Sort(AddressOf compareProdTrans)
        Return res

    End Function

    'Ignore reversed transactions
    Private Function isIgnoreTransactionInAcbCalc(curRow As Integer, transHist As List(Of ProdTransactionHistoryVO)) As Boolean
        Dim curTrans As ProdTransactionHistoryVO
        Dim nextTrans As ProdTransactionHistoryVO

        curTrans = transHist(curRow)
        If (curTrans.isReversal) Then
            Return True
        End If
        If ((curRow + 1) >= transHist.Count) Then
            Return False
        End If

        nextTrans = transHist(curRow + 1)
        Return (curTrans.tranTyp = nextTrans.tranTyp AndAlso curTrans.tranDate = nextTrans.tranDate AndAlso curTrans.prodCode = nextTrans.prodCode AndAlso curTrans.purchasedFrom = nextTrans.purchasedFrom) AndAlso
               (curTrans.qty = -1 * nextTrans.qty AndAlso curTrans.amount = -1 * nextTrans.amount) AndAlso
               nextTrans.isReversal


    End Function

    Public Sub calcAcbQtyAvail(transHist As List(Of ProdTransactionHistoryVO))

        Dim qtyAvail As Integer
        Dim acb As Double
        Dim denom As Double

        Dim prodCode As String = ""

        For i = 0 To transHist.Count - 1

            If prodCode <> transHist(i).prodCode Then
                prodCode = transHist(i).prodCode
                qtyAvail = 0
                acb = 0
            End If

            transHist(i).isIgnoredInAcbCalc = isIgnoreTransactionInAcbCalc(i, transHist)
            If Not transHist(i).isIgnoredInAcbCalc Then
                If (transHist(i).tranTyp = TransactionType.purchase) Then
                    denom = (qtyAvail + transHist(i).qty)
                    If (denom <> 0) Then
                        acb = ((acb * qtyAvail) + UIUtil.zeroIfEmpty(transHist(i).amount)) / denom
                    End If
                    qtyAvail += transHist(i).qty
                ElseIf (transHist(i).tranTyp = TransactionType.sale) Then
                    qtyAvail -= transHist(i).qty
                End If
            End If


            transHist(i).acb = acb
            transHist(i).qtyAvail = qtyAvail
        Next

    End Sub

    Public Sub retrieveCrdPurDebtPymntHist(supplierCode As String, ByRef ds As DataTable)

        Dim connection As MySqlConnection = connectToDB()
        Dim sql As String = " "
        sql += "select "
        sql += "      'Manunuzi' As tran_type, "
        sql += "       purchase.purchased_on as tran_date, "
        sql += "       product.product_code as product_code, "
        sql += "       product.product_name As prod_name, "
        sql += "       purchase.qty_purchased as qty, "
        sql += "       qty_uom.qty_uom_name As qty_uom, "
        sql += "       -1*purchase.amt_purchased as amt, "
        sql += "       supplier.supplier_name as supplier_name, "
        sql += "       purchase.comments  as comments, "
        sql += "       purchase.is_amendment as is_amendment, "
        sql += "       purchase.is_reversal as is_reversal, "
        sql += "       purchase.updated_on as updated_on,"
        sql += "       0 as sort_order "
        sql += "From product, "
        sql += "     purchase, "
        sql += "     qty_uom, "
        sql += "     supplier "
        sql += "Where purchase.product_id = product.product_id "
        sql += "And   purchase.supplier_id = supplier.supplier_id "
        sql += "And   qty_uom.qty_uom_id = product.qty_uom_id "
        ' sql += "And   (purchase.amt_purchased - purchase.purchase_amt_paid_cash) > 0 "
        sql += "And   purchase.purchased_on >= '2016-01-31' "
        sql += "And   supplier.supplier_code = " + prepStrSql(supplierCode) + " "
        sql += "union "
        sql += "select "
        sql += "      'Malipo' as tran_type, "
        sql += "      payment.paid_on as tran_date, "
        sql += "      '' as product_code, "
        sql += "      '' as prod_name, "
        sql += "      '' as qty, "
        sql += "      '' As qty_uom, "
        sql += "      payment.amt_paid as amt, "
        sql += "      supplier.supplier_name as supplier_name, "
        sql += "      payment.comments as comments, "
        sql += "      payment.is_amendment as is_amendment, "
        sql += "      payment.is_reversal as is_reversal, "
        sql += "      payment.updated_on as updated_on, "
        sql += "      1 as sort_order "
        sql += "from payment, "
        sql += "     supplier "
        sql += "where payment.supplier_id = supplier.supplier_id "
        sql += "and   supplier.supplier_code = " + prepStrSql(supplierCode) + " "
        sql += "And   payment.paid_on >= '2016-01-31' "
        sql += "order by tran_date, sort_order, updated_on  "

        Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
        ds = New DataTable()
        adp.Fill(ds)
        connection.Close()

    End Sub

    Public Function retreiveTransactionHistory(Optional prodIds As List(Of Integer) = Nothing, Optional prodCodes As List(Of String) = Nothing) As List(Of ProdTransactionHistoryVO)

        Dim lst As New List(Of ProdTransactionHistoryVO)

        If prodIds Is Nothing AndAlso prodCodes Is Nothing Then
            Return lst
        End If

        Dim prdsSql As String = "and " + If(prodIds Is Nothing, "product.product_code in " + getProdInSql(prodIds, prodCodes), "product.product_id in " + getProdInSql(prodIds, prodCodes))

        Dim dt1 As DataTable = Nothing
        Dim dt2 As DataTable = Nothing
        Dim res As List(Of DataRow)
        Dim adp As MySqlDataAdapter

        Dim connection As MySqlConnection = Nothing
        Dim sqlStr As String

        Try
            connection = connectToDB()
            sqlStr = "select "
            sqlStr += "     'purchased' As tran_type, "
            sqlStr += "     date(purchase.purchased_on) as tran_date, "
            sqlStr += "     product.product_code as product_code, "
            sqlStr += "     product.product_name As prod_name, "
            sqlStr += "     product.product_id As product_id, "
            sqlStr += "     purchase.qty_purchased as qty, "
            sqlStr += "     qty_uom.qty_uom_name As qty_uom, "
            sqlStr += "     purchase.amt_purchased as amt, "
            sqlStr += "     supplier.supplier_name as supplier_name, "
            sqlStr += "      '' acb_cost, "
            sqlStr += "      '' qty_avail, "
            sqlStr += "     purchase.comments  as comments, "
            sqlStr += "     purchase.is_amendment as is_amendment,"
            sqlStr += "     purchase.is_reversal as is_reversal, "
            sqlStr += "     purchase.updated_on "
            sqlStr += "From product, "
            sqlStr += "     purchase, "
            sqlStr += "     qty_uom, "
            sqlStr += "     supplier "
            sqlStr += "Where purchase.product_id = product.product_id "
            sqlStr += prdsSql
            sqlStr += "And   purchase.purchased_on >= '2016-01-31' "
            sqlStr += "And   qty_uom.qty_uom_id = product.qty_uom_id "
            sqlStr += "And   purchase.supplier_id = supplier.supplier_id "
            sqlStr += "order by tran_date, tran_type, purchase.updated_on  "

            adp = New MySqlDataAdapter(sqlStr, connection)
            dt1 = New DataTable
            adp.Fill(dt1)

            sqlStr = "Select  "
            sqlStr += "      'sold' As tran_type,"
            sqlStr += "      date(sale.sold_on) as tran_date, "
            sqlStr += "      product.product_code as product_code, "
            sqlStr += "      product.product_name As prod_name, "
            sqlStr += "      product.product_id As product_id, "
            sqlStr += "      sale.qty_sold as qty, "
            sqlStr += "      qty_uom.qty_uom_name As qty_uom, "
            sqlStr += "      sale.sale_amt as amt, "
            sqlStr += "      '' as supplier_name, "
            sqlStr += "      '' acb_cost, "
            sqlStr += "      '' qty_avail, "
            sqlStr += "      sale.comments as comments, "
            sqlStr += "      sale.is_amendment as is_amendment,"
            sqlStr += "      sale.is_reversal as is_reversal, "
            sqlStr += "     sale.updated_on "
            sqlStr += "From  sale, "
            sqlStr += "      product, "
            sqlStr += "      qty_uom "
            sqlStr += "Where sale.product_id = product.product_id "
            sqlStr += prdsSql
            sqlStr += "And   sale.sold_on >= '2016-01-31' "
            sqlStr += "And qty_uom.qty_uom_id = product.qty_uom_id "
            sqlStr += "order by tran_date, tran_type, sale.updated_on "
            adp = New MySqlDataAdapter(sqlStr, connection)
            dt2 = New DataTable
            adp.Fill(dt2)

            res = merge(dt1, dt2)

            Dim dRow As DataRow
            Dim tVo As ProdTransactionHistoryVO
            For r As Integer = 0 To res.Count - 1
                tVo = New ProdTransactionHistoryVO
                dRow = res(r)
                tVo.tranDate = dRow.Item("tran_date")
                tVo.tranTyp = If(dRow.Item("tran_type") = "purchased", TransactionType.purchase, TransactionType.sale)
                tVo.qty = UIUtil.zeroIfEmpty(dRow.Item("qty"))
                tVo.uom = UIUtil.subsIfEmpty(dRow.Item("qty_uom"), "")
                tVo.purchasedFrom = UIUtil.subsIfEmpty(dRow.Item("supplier_name"), "")
                tVo.amount = UIUtil.zeroIfEmpty(dRow.Item("amt"))
                tVo.comments = UIUtil.subsIfEmpty(dRow.Item("comments"), "")
                tVo.prodCode = UIUtil.subsIfEmpty(dRow.Item("product_code"), "")

                tVo.prodName = UIUtil.subsIfEmpty(dRow.Item("prod_name"), "")

                tVo.productId = dRow.Item("product_id")
                ' tVo.supplierName = dRow.Item("supplier_name")

                tVo.isReversal = UIUtil.toBoolean(dRow.Item("is_reversal"))
                tVo.isAmendment = UIUtil.toBoolean(dRow.Item("is_amendment"))
                tVo.isCommited = True
                tVo.updatedOn = dRow.Item("updated_on")
                lst.Add(tVo)
            Next
            calcAcbQtyAvail(lst)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(dt1) Then
                dt1.Dispose()
            End If
            If Not IsNothing(dt2) Then
                dt2.Dispose()
            End If
            If (Not IsNothing(connection)) Then
                connection.Close()
            End If
        End Try

        Return lst
    End Function

    Private Sub setProdsACBAbdQtyToZero(prodIds As List(Of Integer), cn As MySqlConnection)
        Dim sqlStr As String

        sqlStr = "update product "
        sqlStr += "set qty_available = 0, "
        sqlStr += "acb_cost = 0 "
        sqlStr += "where product_id in " + getProdInSql(prodIds)
        executeNonRetrievalSql(sqlStr, cn)

    End Sub


    Protected Function recalcProductAcbAndQty(prodIds As List(Of Integer), cn As MySqlConnection) As Boolean

        Dim negProdCodes As New List(Of String)
        If (prodIds.Count <= 0) Then
            Return True
        End If

        setProdsACBAbdQtyToZero(prodIds, cn)

        Dim res As List(Of ProdTransactionHistoryVO) = retreiveTransactionHistory(prodIds, Nothing)
        Dim hasProdHitNeg As Boolean = False
        Dim prodId As Integer = -1
        Dim qtyAvail As Integer
        Dim acb As Integer
        For i = 0 To res.Count - 1

            If prodId <> res(i).productId Then
                If (prodId <> -1) AndAlso Not hasProdHitNeg Then
                    updateProductPurchaseQtyAndACB(prodId, qtyAvail, acb, cn, negProdCodes)
                End If
                prodId = res(i).productId
                hasProdHitNeg = False
            End If

            qtyAvail = res(i).qtyAvail
            acb = res(i).acb

            If (qtyAvail < 0 OrElse acb < 0) AndAlso Not hasProdHitNeg Then
                hasProdHitNeg = True
                Dim code As String = res(i).prodCode
                negProdCodes.Add(code + "(" + If(qtyAvail < 0, "qty:" + qtyAvail.ToString, "") + " " + If(acb < 0, "acb:" + acb.ToString, "") + ")")
            End If

        Next

        If (prodId <> -1) AndAlso Not hasProdHitNeg Then
            updateProductPurchaseQtyAndACB(prodId, qtyAvail, acb, cn, negProdCodes)
        End If

        If (negProdCodes.Count > 0) Then
            informUserOfProdsYieldingNegInventory(negProdCodes)
            Return False
        Else
            Return True
        End If

    End Function

    Public Function lookupProdQtyAndAcb(prodIds As List(Of Integer)) As DataTable
        Dim sql As String

        If prodIds.Count <= 0 Then
            Return Nothing
        End If
        Dim connection As MySqlConnection = Nothing

        sql = " Select product.product_code, "
        sql += "       product.product_name, "
        sql += "       qty_uom.qty_uom_name, "
        sql += "       product.qty_available, "
        sql += "       product.acb_cost "
        sql += "From product, "
        sql += "     qty_uom "
        sql += "Where product.qty_uom_id = qty_uom.qty_uom_id "
        sql += "And product.product_id In " + getProdInSql(prodIds)

        Try
            connection = connectToDB()
            Dim dt As New DataTable()
            Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
            adp.Fill(dt)
            Return dt
        Finally
            If (Not IsNothing(connection)) Then
                connection.Close()
            End If
        End Try

    End Function

    Protected Sub informUserOfProdsYieldingNegInventory(prods As List(Of String))
        Dim mesg As String
        mesg = "Report Commited and Submitted, but could Not update ACB And Qty For the following products. They result In -ve Qty Or ACB values: "
        For i As Integer = 0 To prods.Count - 1
            mesg += If(i > 0, ", ", "")
            mesg += prods(i)
        Next
        MessageBox.Show(mainForm, mesg)
    End Sub

    Protected Function getProdInSql(Optional prodIds As List(Of Integer) = Nothing, Optional prodCodes As List(Of String) = Nothing) As String
        Dim prodsSql As String
        Dim i As Integer
        prodsSql = " ("
        If (Not IsNothing(prodIds)) Then
            For i = 0 To prodIds.Count - 1
                If (i > 0) Then
                    prodsSql += ","
                End If
                prodsSql += prodIds(i).ToString
            Next
        Else
            For i = 0 To prodCodes.Count - 1
                If (i > 0) Then
                    prodsSql += ","
                End If
                prodsSql += prepStrSql(prodCodes(i))
            Next
        End If

        prodsSql += ") "
        Return prodsSql
    End Function


    Public Shared Function getBackupFolder()
        Dim backupFileAbsLoc As String = MainForm.getApplicationDataFolder
        Return MiscUtil.appendPathComponent(backupFileAbsLoc, My.Settings.db_backup_dir)
    End Function

    Protected Function ensureValidDatabaseDumpFile(restoreAbsDbFile As String, ByRef errMsg As String)
        Dim sr As StreamReader = Nothing
        Try
            sr = New StreamReader(restoreAbsDbFile)
            Dim line As String
            line = sr.ReadLine()
            If (line.IndexOf("MySQL dump") = -1) Then
                errMsg = "Backup File is not generatered by MySql Dump"
                Return False
            End If
            line = sr.ReadLine()
            line = sr.ReadLine()
            Dim splitArr As String() = line.Split(" ")
            If (splitArr.Count <> 8) Then
                errMsg = "Backup database line does not have the correct amount of values"
                Return False
            End If
            Dim dbNameToRes = splitArr(7).Trim()
            If (dbNameToRes <> My.Settings.database_name) Then
                errMsg = "Wrong database. Application database is " + My.Settings.database_name + " and restore database is " + dbNameToRes
                Return False
            End If

            Return True
        Finally
            If (Not IsNothing(sr)) Then
                sr.Close()
            End If
        End Try

    End Function

    Public Function retriveEstimatedExpenses() As Collection

        Dim sqlStr As String
        Dim connection As MySqlConnection = Nothing
        Try
            connection = connectToDB()
            sqlStr = "Select ec.expense_category_name, ee.type, ee.expense_amt, ifnull(ee.comment, '') "
            sqlStr += "From estimated_expense ee, expense_category ec "
            sqlStr += "Where ee.expense_category_id = ec.expense_category_id "
            sqlStr += " order by ec.expense_category_name"
            Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sqlStr, connection)
            Dim dt As DataTable = New DataTable
            adp.Fill(dt)
            Dim expVo As EstimatedExpenseVO
            Dim expenses As New Collection
            For r As Integer = 0 To dt.Rows.Count - 1
                expVo = New EstimatedExpenseVO
                expVo.expenseCategoryName = dt.Rows(r).Item(0)
                expVo.expenseType = EstimatedExpenseType.monthly
                If dt.Rows(r).Item(1) = "WORKDAY" Then
                    expVo.expenseType = EstimatedExpenseType.workday
                End If
                expVo.expenseAmt = dt.Rows(r).Item(2)
                expVo.comments = dt.Rows(r).Item(3)
                expenses.Add(expVo)
            Next
            Return expenses
        Finally
            If Not IsNothing(connection) Then
                connection.Close()
            End If
        End Try



    End Function


    Public Function retriveProfitsFromDateRange(startDte As Date, endDte As Date) As Collection


        Dim sqlStr As String
        Dim connection As MySqlConnection = Nothing
        Try
            connection = connectToDB()

            sqlStr = "Select s.sold_on, p.product_name, s.qty_sold, s.sale_amt, p.acb_cost*s.qty_sold As cost, (s.sale_amt - (p.acb_cost*s.qty_sold)) As profit, s.is_reversal, s.comments, s.is_amendment "
            sqlStr += "From sale s, product p "
            sqlStr += "Where s.product_id = p.product_id "
            sqlStr += " And s.sold_on >= " + getSqlDate(startDte) + " and s.sold_on <= " + getSqlDate(endDte)
            sqlStr += " order by s.sold_on "
            Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sqlStr, connection)
            Dim dt As DataTable = New DataTable
            adp.Fill(dt)
            Dim vo As SaleVO
            Dim vos As New Collection
            For r As Integer = 0 To dt.Rows.Count - 1
                vo = New SaleVO
                vo.tranDate = dt.Rows(r).Item(0)
                vo.prodName = dt.Rows(r).Item(1)
                vo.qty = dt.Rows(r).Item(2)
                vo.totalSaleAmt = dt.Rows(r).Item(3)
                vo.totalCostOfProd = dt.Rows(r).Item(4)
                vo.isReversal = UIUtil.toBoolean(dt.Rows(r).Item(6))
                vo.comments = dt.Rows(r).Item(7)
                vo.isAmendment = UIUtil.toBoolean(dt.Rows(r).Item(8))
                vos.Add(vo)
            Next

            Return vos

        Finally
            If Not IsNothing(connection) Then
                connection.Close()
            End If
        End Try



    End Function

    Public Sub getPostedTransactionDateRange(ByRef fromDate As Date, ByRef toDate As Date)
        Dim sqlStr As String
        Dim connection As MySqlConnection = Nothing
        Try
            connection = connectToDB()
            sqlStr = "Select min(dte) "
            sqlStr += "from( "
            sqlStr += "     Select min(sale.sold_on) As dte from sale "
            sqlStr += "      union "
            sqlStr += "      Select min(purchase.purchased_on) As dte from purchase "
            sqlStr += "      union "
            sqlStr += "      Select min(payment.paid_on)  As dte from payment "
            sqlStr += "      union "
            sqlStr += "      Select min(expense.expensed_on) As dte from expense "
            sqlStr += ") der "
            fromDate = executeSingleRetreivalSql(sqlStr, connection)
            sqlStr = "Select max(dte) "
            sqlStr += "from( "
            sqlStr += "     Select max(sale.sold_on) As dte from sale "
            sqlStr += "      union "
            sqlStr += "      Select max(purchase.purchased_on) As dte from purchase "
            sqlStr += "      union "
            sqlStr += "      Select max(payment.paid_on)  As dte from payment "
            sqlStr += "      union "
            sqlStr += "      Select max(expense.expensed_on) As dte from expense "
            sqlStr += ") der "
            toDate = executeSingleRetreivalSql(sqlStr, connection)
        Finally
            If Not IsNothing(connection) Then
                connection.Close()
            End If
        End Try

    End Sub

    Public Function runMySqlScript(sqlScriptAbsPath As String) As Boolean
        Dim sqlScriptCmd As String = ControlChars.Quote + My.Settings.mysql_execs_path + My.Settings.mysql_cmd + ControlChars.Quote
        Dim sqlScriptArg As String = "--user=root --password=arusha239 " + My.Settings.database_name + " -e " + ControlChars.Quote + "source " + sqlScriptAbsPath + ControlChars.Quote
        Dim cmdUtil As New RunCmdUtil
        '-e "source batch-file"
        cmdUtil.runCmd(sqlScriptCmd, sqlScriptArg)
        Return True
    End Function

    Public Function restoreDatabase(restoreAbsDbFile As String)
        Dim errMsg As String
        If Not ensureValidDatabaseDumpFile(restoreAbsDbFile, errMsg) Then
            MessageBox.Show(mainForm, "Error. File:" + restoreAbsDbFile + " is not a valid restore file. Reason: " + errMsg)
            Return False
        End If

        Return runMySqlScript(restoreAbsDbFile)

        'Dim restoreCmd As String = ControlChars.Quote + My.Settings.mysql_execs_path + My.Settings.mysql_cmd + ControlChars.Quote
        'Dim cmdUtil As New RunCmdUtil
        ''-e "source batch-file"
        'cmdUtil.runCmd(restoreCmd, "--user=root --password=arusha " + My.Settings.database_name + " -e " + ControlChars.Quote + "source " + restoreAbsDbFile + ControlChars.Quote)
        'Return True
    End Function

    Public Function backUpDatabase()
        Dim minTranDate As Date
        Dim maxTranDate As Date
        getPostedTransactionDateRange(minTranDate, maxTranDate)
        Dim backupFileAbsLoc As String = getBackupFolder()
        backupFileAbsLoc = MiscUtil.appendPathComponent(backupFileAbsLoc, My.Settings.database_name + "#Max Tran Date " + Format(maxTranDate, "yyyy-MM-dd# ") + Format(Now, "yyyy-MM-dd H-mm-ss") + ".sql")
        Dim backupCmd As String = ControlChars.Quote + My.Settings.mysql_execs_path + My.Settings.mysql_backup_cmd + ControlChars.Quote

        Dim cmdUtil As New RunCmdUtil
        cmdUtil.runCmd(backupCmd, "--databases " + My.Settings.database_name + " -uroot -parusha239 --result-file=" + ControlChars.Quote + backupFileAbsLoc + ControlChars.Quote)

        If (Not File.Exists(backupFileAbsLoc)) Then
            Throw New FileNotFoundException("Backup failed.  Backup file:" + backupFileAbsLoc + " not found")
        Else
            Return backupFileAbsLoc
        End If
    End Function

End Class
