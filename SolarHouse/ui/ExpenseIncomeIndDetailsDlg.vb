Public Class ExpenseIncomeIndDetailsDlg
    Public Class MonthlyIncExpVO
        Public month As Date
        Public grossProfit As Integer
        Public estOperatingExpense As Integer

        Public Function monthlyProfit() As Integer
            Return grossProfit - estOperatingExpense
        End Function

    End Class

    Private expenseIncomeIndicator As ExpenseIncomeIndicator

    Public isStartFromJun2018 As Boolean

    Public estExpensesLst As Collection = Nothing
    Public profitsThisMonthLst As New Collection
    Public expensesEachMonthTillCurMonth As List(Of ExpenseIncomeIndicator.MonthlyExpVO)

    Private profitsThisMonth As Double

    Public workDaysThisMonthIncToday As Integer
    Public workDaysInMonth As Integer

    Public remainingWorkDaysExpBalance As Double
    Public todaysDte As Date


    Public Sub New(expenseIncomeIndicator As ExpenseIncomeIndicator)
        ' This call is required by the designer.
        InitializeComponent()
        Me.expenseIncomeIndicator = expenseIncomeIndicator
    End Sub


    Private Sub loadProfit(profitsThisMonthLst As Collection)
        Dim sale As BusinessReportDAO.SaleVO
        For i = 1 To (profitsThisMonthLst.Count)
            salesProfitThisMonthGrdVw.Rows.Add()
        Next i
        Dim profit As Double = 0

        For i = 1 To (profitsThisMonthLst.Count)
            sale = profitsThisMonthLst(i)
            If sale.isReversal Then
                salesProfitThisMonthGrdVw.Rows(i - 1).DefaultCellStyle.BackColor = Color.Red
            ElseIf sale.isAmendment Then
                salesProfitThisMonthGrdVw.Rows(i - 1).DefaultCellStyle.BackColor = Color.Yellow
            ElseIf i Mod 2 = 0 Then
                salesProfitThisMonthGrdVw.Rows(i - 1).DefaultCellStyle.BackColor = Color.FromArgb(239, 254, 255)
            Else
                salesProfitThisMonthGrdVw.Rows(i - 1).DefaultCellStyle.BackColor = Color.White
            End If
            salesProfitThisMonthGrdVw.Rows(i - 1).Cells("sold_on").Value = UIUtil.toDateString(sale.tranDate)
            salesProfitThisMonthGrdVw.Rows(i - 1).Cells("product_name").Value = sale.prodName
            salesProfitThisMonthGrdVw.Rows(i - 1).Cells("qty").Value = UIUtil.zeroIfEmpty(sale.qty)
            salesProfitThisMonthGrdVw.Rows(i - 1).Cells("sale").Value = UIUtil.zeroIfEmpty(sale.totalSaleAmt)
            salesProfitThisMonthGrdVw.Rows(i - 1).Cells("cost").Value = UIUtil.zeroIfEmpty(sale.totalCostOfProd)
            profit = sale.profit
            salesProfitThisMonthGrdVw.Rows(i - 1).Cells("profit").Value = UIUtil.zeroIfEmpty(sale.profit)
            salesProfitThisMonthGrdVw.Rows(i - 1).Cells("comments").Value = sale.comments
        Next i
    End Sub

    Private Function estimatedExpenseVOComparer(v1 As BusinessReportDAO.EstimatedExpenseVO, v2 As BusinessReportDAO.EstimatedExpenseVO) As Integer
        If (v1.expenseAmt > v2.expenseAmt) Then
            Return -1
        ElseIf (v1.expenseAmt < v2.expenseAmt) Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private Sub loadEstExpenses(estExpensesCollection As Collection)
        Dim exp As BusinessReportDAO.EstimatedExpenseVO
        Dim estExpensesLst = New List(Of BusinessReportDAO.EstimatedExpenseVO)
        For i = 1 To (estExpensesCollection.Count)
            estExpensesGrdVw.Rows.Add()
            exp = estExpensesCollection(i)
            estExpensesLst.Add(exp)

        Next i

        estExpensesLst.Sort(AddressOf estimatedExpenseVOComparer)
        For i = 0 To (estExpensesLst.Count - 1)
            exp = estExpensesLst(i)
            estExpensesGrdVw.Rows(i).Cells("est_exp_name").Value = exp.expenseCategoryName
            If (exp.expenseType = BusinessReportDAO.EstimatedExpenseType.monthly) Then
                estExpensesGrdVw.Rows(i).Cells("est_expense_type").Value = "Kila Mwezi"
            Else
                estExpensesGrdVw.Rows(i).Cells("est_expense_type").Value = "Kila Siku"
            End If

            estExpensesGrdVw.Rows(i).Cells("est_exp_amount").Value = UIUtil.zeroIfEmpty(exp.expenseAmt)
            estExpensesGrdVw.Rows(i).Cells("est_exp_comments").Value = exp.comments
        Next i

        UIUtil.shadeAlternateGridRows(estExpensesGrdVw)
        estExpensesGrdVw.Sort(estExpensesGrdVw.Columns("est_exp_amount"), System.ComponentModel.ListSortDirection.Descending)
    End Sub

    Private Function getTotalProfitForMonth(month As Date, profitsThisMonthLst As Collection) As Double
        Dim total As Integer = 0
        Dim sale As BusinessReportDAO.SaleVO
        For i = 1 To (profitsThisMonthLst.Count)
            sale = profitsThisMonthLst(i)
            If (sale.tranDate.Month = month.Month AndAlso sale.tranDate.Year = month.Year) Then
                total += sale.profit
            End If
        Next
        Return total
    End Function

    Private Function summarizeMonthlyIncExps(expensesEachMonthTillCurMonth As List(Of ExpenseIncomeIndicator.MonthlyExpVO), profitsThisMonthLst As Collection) As List(Of MonthlyIncExpVO)
        Dim monthlyIncExpLst As List(Of MonthlyIncExpVO) = New List(Of MonthlyIncExpVO)
        Dim monthlyExpVO As ExpenseIncomeIndicator.MonthlyExpVO
        Dim monthlyIncExpVO As MonthlyIncExpVO
        For i As Integer = 0 To expensesEachMonthTillCurMonth.Count - 1
            monthlyExpVO = expensesEachMonthTillCurMonth(i)
            monthlyIncExpVO = New MonthlyIncExpVO
            monthlyIncExpVO.month = monthlyExpVO.month
            monthlyIncExpVO.estOperatingExpense = monthlyExpVO.estOperatingExpense
            monthlyIncExpVO.grossProfit = getTotalProfitForMonth(monthlyIncExpVO.month, profitsThisMonthLst)
            monthlyIncExpLst.Add(monthlyIncExpVO)
        Next
        Return monthlyIncExpLst
    End Function

    Private Sub loadMonthlyIncExpenseSummary(expensesEachMonthTillCurMonth As List(Of ExpenseIncomeIndicator.MonthlyExpVO),
                                             profitsThisMonthLst As Collection,
                                             ByRef totalExp As Integer,
                                             ByRef totalProfit As Integer)

        Dim monthlyIncExpLst As List(Of MonthlyIncExpVO) = summarizeMonthlyIncExps(expensesEachMonthTillCurMonth, profitsThisMonthLst)

        For i = 0 To (monthlyIncExpLst.Count - 1)
            monthlyIncExpSummaryGrdVw.Rows.Add()
        Next i

        Dim monthlyIncExpVO As MonthlyIncExpVO
        For i = 0 To (monthlyIncExpLst.Count - 1)
            monthlyIncExpVO = monthlyIncExpLst(i)

            If (monthlyIncExpVO.month.Month = todaysDte.Month And monthlyIncExpVO.month.Year = todaysDte.Year) Then
                monthlyIncExpSummaryGrdVw.Rows(i).Cells("month").Value = todaysDte.ToString("01 - ") + todaysDte.ToString("dd MMMM, yyyy")
            Else
                monthlyIncExpSummaryGrdVw.Rows(i).Cells("month").Value = monthlyIncExpVO.month.ToString("MMMM")
            End If

            monthlyIncExpSummaryGrdVw.Rows(i).Cells("total_gross_profit").Value = UIUtil.zeroIfEmpty(monthlyIncExpVO.grossProfit)
            monthlyIncExpSummaryGrdVw.Rows(i).Cells("total_monthly_expenses").Value = UIUtil.zeroIfEmpty(monthlyIncExpVO.estOperatingExpense)
            monthlyIncExpSummaryGrdVw.Rows(i).Cells("monthly_net_profit").Value = UIUtil.zeroIfEmpty(monthlyIncExpVO.monthlyProfit)

            totalExp += monthlyIncExpVO.estOperatingExpense
            totalProfit += monthlyIncExpVO.grossProfit

        Next i

        UIUtil.shadeAlternateGridRows(monthlyIncExpSummaryGrdVw)

        summaryPeriodTxtBox.Text = expenseIncomeIndicator.getExpenseIncTitleLabel()
        totPeriodProfitTxtBox.Text = UIUtil.toAmtString(totalProfit)
        totPeriodOperatingExpTxtBox.Text = UIUtil.toAmtString(totalExp)
        netProfitTxtBox.Text = UIUtil.toAmtString(totalProfit - totalExp)
        If totalProfit - totalExp < 0 Then
            netProfitLabel.Text = "Hasara"
        Else
            netProfitLabel.Text = "Faida"
        End If

    End Sub

    Private Sub ExpenseIncomeIndDetailsDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim totalGrossProfitSoFar As Integer
        Dim totalEstExpensesSoFar As Integer

        loadProfit(profitsThisMonthLst)
        loadEstExpenses(estExpensesLst)
        loadMonthlyIncExpenseSummary(expensesEachMonthTillCurMonth, profitsThisMonthLst, totalEstExpensesSoFar, totalGrossProfitSoFar)

        titleTxtBox.Text = expenseIncomeIndicator.getExpenseIncTitleLabel()

        Dim balanceText As String = "Mwezi huu mpaka leo (" + todaysDte.ToString("dd/MM/yyyy") + ") "
        Dim balance As Integer = totalGrossProfitSoFar - totalEstExpensesSoFar
        estProfitLossTxtBox.Text = UIUtil.toAmtString(UIUtil.zeroIfEmpty(balance))
        If balance <= 0 Then
            balanceText += "tume pata Hasara ya shs " + UIUtil.toAmtString(UIUtil.zeroIfEmpty(balance)) + "/="
            estProfitLossTxtBox.BackColor = Color.FromArgb(243, 149, 153)
        Else
            balanceText += "tume pata faida ya shs " + UIUtil.toAmtString(UIUtil.zeroIfEmpty(balance)) + "/=. HONGERA HATUJA fidia mpaka leo"
            estProfitLossTxtBox.BackColor = Color.FromArgb(98, 249, 140)
        End If

        estProfitLossTxtBox.Text = balanceText
        Dim remainingWorkDaysProditText = ""
        If (Me.remainingWorkDaysExpBalance > 0) Then
            remainingWorkDaysProditText += "Lzm upate faida shs " + UIUtil.toAmtString(UIUtil.zeroIfEmpty((Me.remainingWorkDaysExpBalance))) + "/= kila siku "
            If (workDaysInMonth - workDaysThisMonthIncToday) > 0 Then
                remainingWorkDaysProditText += UIUtil.toAmtString(UIUtil.zeroIfEmpty(workDaysInMonth - workDaysThisMonthIncToday)) + " zilizo baki mwezi wa " + UIUtil.toAmtString(todaysDte.Month)
                remainingWorkDaysProditText += " au mwisho wa mwezi uta pata hasara "
            Else
                remainingWorkDaysProditText += " LEO au uta pata hasara mwezi huu"
            End If


            profitToMakeForRemainingDaysTxtBox.BackColor = Color.FromArgb(243, 149, 153)
        Else
            remainingWorkDaysProditText = ""
        End If
        profitToMakeForRemainingDaysTxtBox.Text = remainingWorkDaysProditText


    End Sub

End Class