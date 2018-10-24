Public Class ExpenseIncomeIndicator
    Inherits PictureBox

    Public Class MonthlyExpVO
        Public month As Date
        Public estOperatingExpense As Integer
    End Class

    Public Shared BEGINNING_JUNE_2018 As Date = New Date(2018, 6, 1)

    Private todaysDte As Date = Date.Today

    Private isStartFromJun2018 As Boolean

    Private estExpensesLst As Collection = Nothing
    Private workDayEstExpTot As Double = 0
    Private monthlyEstExpTot As Double = 0

    ' profits so far
    Private commitedProfitsLst As Collection = Nothing
    Private totCommitedProfits As Double = Nothing

    Private unCommitedSaleReportProfitLst As Collection = New Collection
    Private totUnCommitedProfits As Double = Nothing

    ' expenses so far

    Private expensesEachMonthTillCurMonth As List(Of MonthlyExpVO) = New List(Of MonthlyExpVO)
    Private totEstExpensesTillToday As Double

    Private workDaysElapsedInCurrentMonthInclToday As Integer
    Private totWorkDaysInCurrentMonth As Integer

    Private endOfCurrentMonth As Date

    Private currentMonthsRemainingDailyWorkdayProfit As Double

    Public Sub New()

        estExpensesLst = retrieveEstExpenses()
        computeEstExpensesMetaData(estExpensesLst, workDayEstExpTot, monthlyEstExpTot)
        endOfCurrentMonth = New Date(todaysDte.Year, todaysDte.Month, Date.DaysInMonth(todaysDte.Year, todaysDte.Month))

    End Sub

    Private Sub computeEstExpensesMetaData(estExpensesLst As Collection, ByRef workDayEstExpTot As Double, ByRef monthlyEstExpTot As Double)
        workDayEstExpTot = 0
        monthlyEstExpTot = 0
        For r As Integer = 1 To estExpensesLst.Count
            If estExpensesLst(r).expenseType = BusinessReportDAO.EstimatedExpenseType.monthly Then
                monthlyEstExpTot += estExpensesLst(r).expenseAmt
            Else
                workDayEstExpTot += estExpensesLst(r).expenseAmt
            End If
        Next
    End Sub

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

    Private Sub drawIncomeExpenseDiscrepancyInd(bndryRect As Rectangle, e As PaintEventArgs)
        If (TypeOf Parent Is BusinessReportDlg) Then
            Dim p As BusinessReportDlg = Parent
            p.expenseIncIndTxtBox.Text = Me.getExpenseIncTitleLabel()
        End If


        Dim font As Font = New Font("Ariel", 8)
        Dim brush As SolidBrush
        Dim fmt As StringFormat = New StringFormat()
        Dim profitTillToday = totUnCommitedProfits + Me.totCommitedProfits
        Dim balanceTillToday As Integer = profitTillToday - totEstExpensesTillToday
        If balanceTillToday < 0 Then
            brush = New SolidBrush(System.Drawing.Color.Red)
        Else
            brush = New SolidBrush(System.Drawing.Color.Black)
        End If
        Dim strNum = If(balanceTillToday > 0, "Faida: ", "Hasara: ") + FormatNumber(balanceTillToday, 0, TriState.True, TriState.False, True)
        Dim sz As Size = TextRenderer.MeasureText(strNum, font)
        e.Graphics.DrawString(strNum, font, brush, bndryRect.Left, bndryRect.Top + (bndryRect.Height - sz.Height) * 0.5, fmt)

        If currentMonthsRemainingDailyWorkdayProfit < 0 Then
            currentMonthsRemainingDailyWorkdayProfit = 0
        End If
        brush.Color = System.Drawing.Color.Black
        If ((totWorkDaysInCurrentMonth - workDaysElapsedInCurrentMonthInclToday) > 0) Then
            strNum = FormatNumber((currentMonthsRemainingDailyWorkdayProfit * (totWorkDaysInCurrentMonth - workDaysElapsedInCurrentMonthInclToday)), 0, TriState.True, TriState.False, True)
            If (currentMonthsRemainingDailyWorkdayProfit > 0) Then
                strNum += "(" + UIUtil.toAmtString(UIUtil.zeroIfEmpty(currentMonthsRemainingDailyWorkdayProfit)) + " x " + UIUtil.toAmtString(UIUtil.zeroIfEmpty(totWorkDaysInCurrentMonth - workDaysElapsedInCurrentMonthInclToday)) + ")"
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
        Dim commitedProfitsThisMonth As Integer = computeTotCommitedProfits(Me.commitedProfitsLst)
        If totUnCommitedProfits + commitedProfitsThisMonth > totEstExpensesTillToday Then
            maxnum = totUnCommitedProfits + commitedProfitsThisMonth
        Else
            maxnum = totEstExpensesTillToday
        End If

        Dim txtbr = New Drawing.SolidBrush(Color.Black)
        Dim font As Font = New Font("Ariel", 8)
        Dim fmt As StringFormat = New StringFormat()

        ' e.Graphics.DrawRectangle(bndryPen, drawingRectExp)
        br = New Drawing.SolidBrush(Color.Red)
        drawingRectExp.Width = If(maxnum = 0, drawingRectExp.Width, (drawingRectExp.Width * totEstExpensesTillToday) / (maxnum))
        e.Graphics.FillRectangle(br, drawingRectExp)

        Dim strNum = FormatNumber(-1 * totEstExpensesTillToday, 0, TriState.True, TriState.False, True)
        Dim sz As Size = TextRenderer.MeasureText(strNum, font)
        e.Graphics.DrawString(strNum, font, txtbr, drawingRectExp.Left + (bndryRect.Width - sz.Width) * 0.5, drawingRectExp.Top + 2, fmt)


        drawingRectInc.Width = If(maxnum = 0, drawingRectInc.Width, (drawingRectInc.Width * (totUnCommitedProfits + commitedProfitsThisMonth)) / (maxnum))
        ' e.Graphics.DrawRectangle(bndryPen, drawingRectInc)
        br = New Drawing.SolidBrush(Color.LawnGreen)
        e.Graphics.FillRectangle(br, drawingRectInc)

        strNum = FormatNumber(totUnCommitedProfits + commitedProfitsThisMonth, 0, TriState.True, TriState.False, True)
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

    Private Sub refreshCommitedProfits(isStartFromJun2018 As Boolean)
        commitedProfitsLst = retrieveCommitedProfits(isStartFromJun2018)
        totCommitedProfits = computeTotCommitedProfits(Me.commitedProfitsLst)
    End Sub

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

    Private Function computeTotCommitedProfits(commitedProfitsLst As Collection) As Integer
        Dim totCommitedProfits As Integer = 0
        If Not IsNothing(commitedProfitsLst) Then
            For r As Integer = 1 To commitedProfitsLst.Count
                totCommitedProfits += commitedProfitsLst(r).totalSaleAmt - commitedProfitsLst(r).totalCostOfProd
            Next
        End If
        Return totCommitedProfits
    End Function

    Public Function getExpenseIncTitleLabel() As String
        Dim lbl As String
        If (Me.isStartFromJun2018) Then
            lbl = "01/06/2018 mpaka " + todaysDte.ToString("dd/MM/yyyy")
        Else
            lbl = todaysDte.ToString("01") + " mpaka " + todaysDte.ToString("dd MMMM, yyyy")
        End If
        Return lbl
    End Function

    Private Sub refreshUnCommitedProfits(curSaleReportProfitLstNew As Collection, ByRef unCommitedSaleReportProfitLst As Collection, ByRef totUnCommitedProfits As Double)
        Dim sale As BusinessReportDAO.SaleVO
        unCommitedSaleReportProfitLst.Clear()
        totUnCommitedProfits = 0
        For r As Integer = 1 To curSaleReportProfitLstNew.Count
            sale = curSaleReportProfitLstNew(r)
            If Not sale.isPosted AndAlso (sale.tranDate.Year = todaysDte.Year AndAlso sale.tranDate.Month = todaysDte.Month) Then
                totUnCommitedProfits += (sale.totalSaleAmt - sale.totalCostOfProd)
                unCommitedSaleReportProfitLst.Add(sale)
            End If
        Next
    End Sub


    Private Function refreshExpensesEachMonthTillCurMonth(isStartFromJun2018 As Boolean,
                                                          todaysDte As Date,
                                                          estExpensesLst As Collection,
                                                          workDayEstExpTot As Double,
                                                          monthlyEstExpTot As Double,
                                                          ByRef expensesEachMonthTillCurMonth As List(Of MonthlyExpVO),
                                                          ByRef totEstExpensesTillToday As Double,
                                                          ByRef workDaysElapsedInCurrentMonthInclToday As Integer,
                                                          ByRef totWorkDaysInCurrentMonth As Integer) As List(Of Double)
        expensesEachMonthTillCurMonth.Clear()
        totEstExpensesTillToday = 0
        Dim numOfWorkDays As Integer
        Dim totMonthEstExp As Double
        Dim endDate = New Date(todaysDte.Year, todaysDte.Month, 1)
        Dim dte As Date
        If (isStartFromJun2018) Then
            dte = BEGINNING_JUNE_2018
        Else
            dte = endDate
        End If
        Dim monthlyExp As MonthlyExpVO
        While (dte <= endDate)
            monthlyExp = New MonthlyExpVO
            monthlyExp.month = dte
            If (dte < endDate) Then
                numOfWorkDays = computeNumOfWorkDays(New Date(dte.Year, dte.Month, Date.DaysInMonth(dte.Year, dte.Month)))
                totMonthEstExp = computeEstWorkDayExpenses(workDayEstExpTot, monthlyEstExpTot, numOfWorkDays) * numOfWorkDays
            Else
                numOfWorkDays = computeNumOfWorkDays(todaysDte)
                workDaysElapsedInCurrentMonthInclToday = numOfWorkDays
                totWorkDaysInCurrentMonth = computeNumOfWorkDays(New Date(dte.Year, dte.Month, Date.DaysInMonth(dte.Year, dte.Month)))
                totMonthEstExp = computeEstWorkDayExpenses(workDayEstExpTot, monthlyEstExpTot, totWorkDaysInCurrentMonth) * workDaysElapsedInCurrentMonthInclToday
            End If
            monthlyExp.estOperatingExpense = totMonthEstExp
            expensesEachMonthTillCurMonth.Add(monthlyExp)
            totEstExpensesTillToday += totMonthEstExp
            dte = DateAdd(DateInterval.Month, 1, dte)
        End While

    End Function

    Private Function computeNumOfWorkDays(dte As Date) As Integer
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

    Private Function computeEstWorkDayExpenses(workDayEstExpTot As Double, monthlyEstExpTot As Double, totWorkDaysInMonth As Integer) As Double
        Return (workDayEstExpTot) + (monthlyEstExpTot / totWorkDaysInMonth)
    End Function


    Private Function computeMonthsRemainingDailyWorkdayProfit(workDayEstExpTot As Double,
                                                              monthlyEstExpTot As Double,
                                                              workDaysElapsedInCurrentMonthInclToday As Integer,
                                                              totEstExpensesTillToday As Integer,
                                                              totWorkDaysInCurrentMonth As Integer,
                                                              totCommitedProfits As Double,
                                                              totUnCommitedProfits As Double) As Double
        Dim estCurrentMonthWorkDayExp = computeEstWorkDayExpenses(workDayEstExpTot, monthlyEstExpTot, totWorkDaysInCurrentMonth)

        Dim numOfDaysInRemInMonth As Integer = (totWorkDaysInCurrentMonth - workDaysElapsedInCurrentMonthInclToday)
        Dim remainingDaysExpBalance As Double = ((totEstExpensesTillToday + (estCurrentMonthWorkDayExp * numOfDaysInRemInMonth)) - (totCommitedProfits + totUnCommitedProfits))
        If (numOfDaysInRemInMonth <= 0) Then
            Return remainingDaysExpBalance
        Else
            Return remainingDaysExpBalance / numOfDaysInRemInMonth
        End If

    End Function

    Private Sub refreshCurrentMonthsRemainingDailyWorkdayProfit()
        currentMonthsRemainingDailyWorkdayProfit = computeMonthsRemainingDailyWorkdayProfit(Me.workDayEstExpTot,
                                                                                            Me.monthlyEstExpTot,
                                                                                            Me.workDaysElapsedInCurrentMonthInclToday,
                                                                                            Me.totEstExpensesTillToday,
                                                                                            Me.totWorkDaysInCurrentMonth,
                                                                                            Me.totCommitedProfits,
                                                                                            Me.totUnCommitedProfits)
    End Sub


    Public Sub refreshCntrl(isStartFromJun2018 As Boolean)
        Me.isStartFromJun2018 = isStartFromJun2018
        refreshCommitedProfits(isStartFromJun2018)
        refreshExpensesEachMonthTillCurMonth(isStartFromJun2018,
                                             todaysDte,
                                             estExpensesLst,
                                             workDayEstExpTot,
                                             monthlyEstExpTot,
                                             Me.expensesEachMonthTillCurMonth,
                                             Me.totEstExpensesTillToday,
                                             Me.workDaysElapsedInCurrentMonthInclToday,
                                             Me.totWorkDaysInCurrentMonth)
        refreshCurrentMonthsRemainingDailyWorkdayProfit()
        Me.Invalidate()
    End Sub

    Public Sub refreshCntrl(curSaleReportProfitLstNew As Collection)
        refreshUnCommitedProfits(curSaleReportProfitLstNew, unCommitedSaleReportProfitLst, totUnCommitedProfits)
        refreshCurrentMonthsRemainingDailyWorkdayProfit()
        Me.Invalidate()
    End Sub


    Public Sub reportSubmitted()
        refreshCntrl(isStartFromJun2018)
    End Sub


    Sub initializeControl(isStartFromJun2018 As Boolean)
        Me.isStartFromJun2018 = isStartFromJun2018
        refreshCommitedProfits(isStartFromJun2018)
        refreshUnCommitedProfits(New Collection(), unCommitedSaleReportProfitLst, totUnCommitedProfits)
        refreshExpensesEachMonthTillCurMonth(isStartFromJun2018,
                                             todaysDte,
                                             estExpensesLst,
                                             workDayEstExpTot,
                                             monthlyEstExpTot,
                                             Me.expensesEachMonthTillCurMonth,
                                             Me.totEstExpensesTillToday,
                                             Me.workDaysElapsedInCurrentMonthInclToday,
                                             Me.totWorkDaysInCurrentMonth)
        refreshCurrentMonthsRemainingDailyWorkdayProfit()
        Me.Invalidate()
    End Sub


    Public Sub showIndicator(isShow As Boolean, isStartFromJun2018 As Boolean)
        Dim p As BusinessReportDlg = Parent
        p.expenseIncIndTxtBox.Visible = isShow
        p.profitSummaryIndGrpBox.Visible = isShow
        p.startFromJun2018ChkBox.Visible = isShow
        Me.Visible = isShow
        If isShow Then
            initializeControl(isStartFromJun2018)
        End If
    End Sub


    Protected Overrides Sub OnMouseDoubleClick(e As MouseEventArgs)
        Dim dlg = New ExpenseIncomeIndDetailsDlg(Me)
        dlg.estExpensesLst = estExpensesLst

        dlg.profitsThisMonthLst.Clear()
        dlg.profitsThisMonthLst = MiscUtil.addAll(dlg.profitsThisMonthLst, Me.commitedProfitsLst)
        dlg.profitsThisMonthLst = MiscUtil.addAll(dlg.profitsThisMonthLst, Me.unCommitedSaleReportProfitLst)
        dlg.expensesEachMonthTillCurMonth = Me.expensesEachMonthTillCurMonth

        dlg.workDaysThisMonthIncToday = workDaysElapsedInCurrentMonthInclToday
        dlg.workDaysInMonth = totWorkDaysInCurrentMonth
        dlg.remainingWorkDaysExpBalance = currentMonthsRemainingDailyWorkdayProfit
        dlg.isStartFromJun2018 = isStartFromJun2018
        dlg.todaysDte = todaysDte
        dlg.ShowDialog()

    End Sub


    Private Function retrieveEstExpenses() As Collection
        If IsNothing(estExpensesLst) Then
            Dim serv As BusinessReportService = New BusinessReportService(Parent)
            estExpensesLst = MiscUtil.addAll(estExpensesLst, serv.retriveEstimatedExpenses())
        End If
        Return estExpensesLst
    End Function


End Class
