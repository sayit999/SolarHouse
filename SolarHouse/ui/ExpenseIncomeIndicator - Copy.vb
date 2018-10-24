Public Class ExpenseIncomeIndicator
    Inherits PictureBox

    Public Shared BEGINNING_JUNE_2018 As Date = New Date(2018, 6, 1)

    'Private estExpensesLst As List(Of BusinessReportDAO.EstimatedExpenseVO) = Nothing
    'Private profitsThisMonthExclTodayLst As List(Of BusinessReportDAO.SaleVO) = Nothing
    'Private curSaleReportProfitLst As List(Of BusinessReportDAO.SaleVO) = New List(Of BusinessReportDAO.SaleVO)

    Private estExpensesLst As Collection = Nothing
    Private commitedProfitsLst As Collection = Nothing
    Private uncommitedSaleReportProfitLst As Collection = New Collection


    Private estCurrentMonthWorkDayExp As Double
    Private estExpensesTillToday As Double
    Private totCommitedProfits As Double = Nothing
    Private uncommitedProfit As Double = 0

    Private todaysDte As Date = Date.Today

    Dim workDaysElapsedInclToday As Integer
    Dim endOfCurrentMonth As Date
    Dim totWorkDaysInCurrentMonth As Integer
    Dim currentMonthsRemainingDailyWorkdayProfit As Double


    Dim profitsThisMonthLst As List(Of BusinessReportDAO.SaleVO) = New List(Of BusinessReportDAO.SaleVO)

    Public Sub New()
        workDaysElapsedInclToday = computeNumOfElapsedWorkDays(todaysDte)
        endOfCurrentMonth = New Date(todaysDte.Year, todaysDte.Month, Date.DaysInMonth(todaysDte.Year, todaysDte.Month))
        totWorkDaysInCurrentMonth = computeNumOfElapsedWorkDays(endOfCurrentMonth)

        refreshCommitedProfits()

        estCurrentMonthWorkDayExp = computeEstWorkDayExpenses(totWorkDaysInCurrentMonth)
        estExpensesTillToday = estCurrentMonthWorkDayExp * workDaysElapsedInclToday

        currentMonthsRemainingDailyWorkdayProfit = computeMonthsRemainingDailyWorkdayProfit()


    End Sub


    Public Sub refreshCntrl(curSaleReportProfitLstNew As Collection)
        uncommitedProfit = 0
        Dim sale As BusinessReportDAO.SaleVO
        uncommitedSaleReportProfitLst.Clear()
        ' Me.curSaleReportProfitLst = MiscUtil.addAll(Me.curSaleReportProfitLst, curSaleReportProfitLst)
        For r As Integer = 1 To curSaleReportProfitLstNew.Count
            sale = curSaleReportProfitLstNew(r)
            If Not sale.isPosted AndAlso (sale.tranDate.Year = todaysDte.Year AndAlso sale.tranDate.Month = todaysDte.Month) Then
                uncommitedProfit += sale.priceSold - sale.totalCostOfProd
                uncommitedSaleReportProfitLst.Add(sale)
            End If

        Next

        currentMonthsRemainingDailyWorkdayProfit = computeMonthsRemainingDailyWorkdayProfit()
        Me.Invalidate()
    End Sub

    Public Sub reportSubmitted()
        refreshCommitedProfits()
        Me.Invalidate()
    End Sub

    Public Function isStartFromJun2018() As Boolean
        Return (TypeOf Parent Is BusinessReportDlg) AndAlso CType(Parent, BusinessReportDlg).startFromJun2018ChkBox.Checked
    End Function

    Private Sub refreshCommitedProfits()
        totCommitedProfits = Nothing
        totCommitedProfits = computeTotCommitedProfits()
    End Sub

    Private Function retrieveEstExpenses() As Collection
        If IsNothing(estExpensesLst) Then
            Dim serv As BusinessReportService = New BusinessReportService(Parent)
            estExpensesLst = MiscUtil.addAll(estExpensesLst, serv.retriveEstimatedExpenses())
        End If
        Return estExpensesLst
    End Function

    Private Function computeNumOfElapsedWorkDays(dte As Date)
        Dim curMonth = dte.Month
        Dim workDays As Integer = 0
        While dte.Month = curMonth
            If dte.DayOfWeek <> DayOfWeek.Sunday Then
                workDays = workDays + 1
            End If
            dte = dte.AddDays(-1)
        End While
        Return workDays
    End Function

    Sub initializeControl()
        uncommitedProfit = 0
        uncommitedSaleReportProfitLst.Clear()
    End Sub


    Public Sub showIndicator(isShow As Boolean)
        Dim p As BusinessReportDlg = Parent
        p.expenseIncIndTxtBox.Visible = isShow
        Me.Visible = isShow
        If isShow Then
            initializeControl()
        End If
    End Sub

    Private Function computeEstWorkDayExpenses(totWorkDaysInMonth As Integer) As Double
        Dim curMonth = todaysDte.Month

        Dim workDayTot As Double = 0
        Dim monthlyTot As Double = 0
        Dim estExpenses As Collection = retrieveEstExpenses()
        For r As Integer = 1 To estExpenses.Count
            If estExpenses(r).expenseType = BusinessReportDAO.EstimatedExpenseType.monthly Then
                monthlyTot += estExpenses(r).expenseAmt
            Else
                workDayTot += estExpenses(r).expenseAmt
            End If
        Next

        Return (workDayTot) + (monthlyTot / totWorkDaysInMonth)

    End Function


    Private Function computeMonthsRemainingDailyWorkdayProfit() As Double
        Dim remainingDaysExpBalance As Double = ((estCurrentMonthWorkDayExp * totWorkDaysInCurrentMonth) - (totCommitedProfits + uncommitedProfit))
        If ((totWorkDaysInCurrentMonth - workDaysElapsedInclToday) <= 0) Then
            Return remainingDaysExpBalance
        Else
            Return remainingDaysExpBalance / (totWorkDaysInCurrentMonth - workDaysElapsedInclToday)
        End If

    End Function

    Private Function retrieveCommitedProfits(isStartFromJun2018 As Boolean) As Collection
        Dim startDate As Date
        If (isStartFromJun2018) Then
            startDate = BEGINNING_JUNE_2018
        Else
            startDate = New Date(todaysDte.Year, todaysDte.Month, 1)
        End If

        Dim endDate As Date = Me.endOfCurrentMonth

        Dim serv As BusinessReportService = New BusinessReportService(Parent)
        Return serv.retriveProfitsFromDateRange(startDate, endDate)
    End Function

    Private Function computeTotEstExpensesTillToday()

    End Function

    Private Function computeTotCommitedProfits()

        If IsNothing(totCommitedProfits) Then
            commitedProfitsLst = retrieveCommitedProfits(isStartFromJun2018())
            totCommitedProfits = 0
            For r As Integer = 1 To commitedProfitsLst.Count
                totCommitedProfits += commitedProfitsLst(r).priceSold - commitedProfitsLst(r).totalCostOfProd
            Next
        End If
        Return totCommitedProfits
    End Function

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

    Private Sub drawIncomeExpenseDiscrepancyInd(bndryRect As Rectangle, e As PaintEventArgs)
        If (TypeOf Parent Is BusinessReportDlg) Then
            Dim p As BusinessReportDlg = Parent
            p.expenseIncIndTxtBox.Text = "mwezi " + String.Format(todaysDte.Month) + " mwaka " + String.Format(todaysDte.Year)
        End If


        Dim font As Font = New Font("Ariel", 8)
        Dim brush As SolidBrush
        Dim fmt As StringFormat = New StringFormat()
        Dim profitTillToday = uncommitedProfit + Me.totCommitedProfits
        Dim balanceTillToday As Integer = profitTillToday - estExpensesTillToday
        If balanceTillToday < 0 Then
            brush = New SolidBrush(System.Drawing.Color.Red)
        Else
            brush = New SolidBrush(System.Drawing.Color.Black)
        End If
        Dim strNum = If(balanceTillToday > 0, "Faida: ", "Hasara: ") + FormatNumber(balanceTillToday, 0, TriState.True, TriState.False, True)
        strNum += "(" + UIUtil.toAmtString(UIUtil.zeroIfEmpty(workDaysElapsedInclToday)) + ")"
        Dim sz As Size = TextRenderer.MeasureText(strNum, font)
        e.Graphics.DrawString(strNum, font, brush, bndryRect.Left, bndryRect.Top + (bndryRect.Height - sz.Height) * 0.5, fmt)

        If currentMonthsRemainingDailyWorkdayProfit < 0 Then
            currentMonthsRemainingDailyWorkdayProfit = 0
        End If
        brush.Color = System.Drawing.Color.Black
        If ((totWorkDaysInCurrentMonth - workDaysElapsedInclToday) > 0) Then
            strNum = FormatNumber((currentMonthsRemainingDailyWorkdayProfit * (totWorkDaysInCurrentMonth - workDaysElapsedInclToday)), 0, TriState.True, TriState.False, True)
            If (currentMonthsRemainingDailyWorkdayProfit > 0) Then
                strNum += "(" + UIUtil.toAmtString(UIUtil.zeroIfEmpty(currentMonthsRemainingDailyWorkdayProfit)) + " x " + UIUtil.toAmtString(UIUtil.zeroIfEmpty(totWorkDaysInCurrentMonth - workDaysElapsedInclToday)) + ")"
            End If
        Else
            strNum = FormatNumber((currentMonthsRemainingDailyWorkdayProfit), 0, TriState.True, TriState.False, True)
        End If


        sz = TextRenderer.MeasureText(strNum, font)
        e.Graphics.DrawString(strNum, font, brush, bndryRect.Right - sz.Width, bndryRect.Top + (bndryRect.Height - sz.Height) * 0.5, fmt)


        font.Dispose()
        brush.Dispose()
        fmt.Dispose()

    End Sub

    Private Sub drawIncomeExpenseInd(bndryRect As Rectangle, e As PaintEventArgs)
        Dim br As Brush
        Dim bndryPen As New Pen(Color.Black)
        Dim myBndryRect As Rectangle = New Rectangle(bndryRect.Left + 5, bndryRect.Top, bndryRect.Width - 10, bndryRect.Height - 5)
        Dim drawingRectExp As Rectangle = New Rectangle(myBndryRect.Left, myBndryRect.Top, myBndryRect.Width, myBndryRect.Height * 0.5 - 2)
        Dim drawingRectInc As Rectangle = New Rectangle(myBndryRect.Left, drawingRectExp.Bottom + 4, myBndryRect.Width, myBndryRect.Height - drawingRectExp.Height - 4)

        Dim maxnum As Double
        Dim commitedProfitsThisMonth As Integer = computeTotCommitedProfits()
        If uncommitedProfit + commitedProfitsThisMonth > estExpensesTillToday Then
            maxnum = uncommitedProfit + commitedProfitsThisMonth
        Else
            maxnum = estExpensesTillToday
        End If

        Dim txtbr = New Drawing.SolidBrush(Color.Black)
        Dim font As Font = New Font("Ariel", 8)
        Dim fmt As StringFormat = New StringFormat()

        ' e.Graphics.DrawRectangle(bndryPen, drawingRectExp)
        br = New Drawing.SolidBrush(Color.Red)
        drawingRectExp.Width = (drawingRectExp.Width * estExpensesTillToday) / (maxnum)
        e.Graphics.FillRectangle(br, drawingRectExp)

        Dim strNum = FormatNumber(-1 * estExpensesTillToday, 0, TriState.True, TriState.False, True)
        Dim sz As Size = TextRenderer.MeasureText(strNum, font)
        e.Graphics.DrawString(strNum, font, txtbr, drawingRectExp.Left + (bndryRect.Width - sz.Width) * 0.5, drawingRectExp.Top + 2, fmt)


        drawingRectInc.Width = (drawingRectInc.Width * (uncommitedProfit + commitedProfitsThisMonth)) / (maxnum)
        ' e.Graphics.DrawRectangle(bndryPen, drawingRectInc)
        br = New Drawing.SolidBrush(Color.LawnGreen)
        e.Graphics.FillRectangle(br, drawingRectInc)

        strNum = FormatNumber(uncommitedProfit + commitedProfitsThisMonth, 0, TriState.True, TriState.False, True)
        sz = TextRenderer.MeasureText(strNum, font)
        e.Graphics.DrawString(strNum, font, txtbr, drawingRectInc.Left + (bndryRect.Width - sz.Width) * 0.5, drawingRectInc.Top + 2, fmt)


        bndryPen.Dispose()
        br.Dispose()
        txtbr.Dispose()
        fmt.Dispose()
        font.Dispose()

    End Sub

    Private Sub ExpenseIncomeIndicator_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Dim bndryPen As New Pen(Color.Black, 1)
        Dim drawingRect As Rectangle = New Rectangle(DisplayRectangle.Left, DisplayRectangle.Top, DisplayRectangle.Width - 1, DisplayRectangle.Height - 1)


        e.Graphics.DrawRectangle(bndryPen, drawingRect)
        bndryPen.Dispose()

        Dim diffLblRect As Rectangle

        diffLblRect = DisplayRectangle
        diffLblRect.Height = diffLblRect.Height * 0.4
        drawIncomeExpenseDiscrepancyInd(diffLblRect, e)

        Dim incExpIndRect As Rectangle = New Rectangle(diffLblRect.Left, diffLblRect.Bottom, diffLblRect.Width, DisplayRectangle.Height - diffLblRect.Height)
        drawIncomeExpenseInd(incExpIndRect, e)


    End Sub

    Private Function addCurDaysSalesProfit(profitsThisMonthExclToday As List(Of BusinessReportDAO.SaleVO)) As List(Of BusinessReportDAO.SaleVO)
        Dim profitsThisMonth As New List(Of BusinessReportDAO.SaleVO)
        profitsThisMonth.AddRange(profitsThisMonthExclToday)

        For r As Integer = 0 To profitsThisMonthExclToday.Count - 1

        Next
        Return profitsThisMonth
    End Function

    Protected Overrides Sub OnMouseDoubleClick(e As MouseEventArgs)
        Dim dlg = New ExpenseIncomeIndDetailsDlg()
        dlg.estExpensesLst = estExpensesLst

        dlg.profitsThisMonthLst.Clear()
        dlg.profitsThisMonthLst = MiscUtil.addAll(dlg.profitsThisMonthLst, Me.commitedProfitsLst)
        dlg.profitsThisMonthLst = MiscUtil.addAll(dlg.profitsThisMonthLst, Me.uncommitedSaleReportProfitLst)

        dlg.estExpensesThisMonth = estExpensesTillToday
        dlg.workDaysThisMonthIncToday = workDaysElapsedInclToday
        dlg.workDaysInMonth = totWorkDaysInCurrentMonth
        dlg.remainingWorkDaysExpBalance = currentMonthsRemainingDailyWorkdayProfit
        dlg.todaysDte = todaysDte
        dlg.ShowDialog()

    End Sub
End Class
