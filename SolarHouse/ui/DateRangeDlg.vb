Public Class DateRangeDlg
    Private minTranDate As Date
    Private maxTranDate As Date

    Public Sub New(fromDate As Date, toDate As Date, minTranDate As Date, maxTranDate As Date)

        ' This call is required by the designer.
        InitializeComponent()
        If (Not IsNothing(fromDate)) Then
            fromDateTxtBx.Text = UIUtil.toDateString(fromDate)
        End If
        If (Not IsNothing(toDate)) Then
            toDateTxtBx.Text = UIUtil.toDateString(toDate)
        End If
        Me.minTranDate = minTranDate
        Me.maxTranDate = maxTranDate

    End Sub
    Private Sub DateRangeDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Function getFromDate() As Date
        Return UIUtil.parseDate(fromDateTxtBx.Text)
    End Function

    Public Function getToDate() As Date
        Return UIUtil.parseDate(toDateTxtBx.Text)
    End Function

    Private Sub okBtn_Click(sender As Object, e As EventArgs) Handles okBtn.Click
        Dim isError As Boolean = False
        If Not UIUtil.isValidDate(fromDateTxtBx.Text) Then
            MessageBox.Show("enter a valid from date")
            isError = True
        ElseIf Not UIUtil.isValidDate(toDateTxtBx.Text) Then
            MessageBox.Show("enter a valid to date")
            isError = True
        Else
            Dim fromDate As Date = getFromDate()
            Dim toDate As Date = getToDate()
            If fromDate < minTranDate OrElse fromDate > maxTranDate Then
                MessageBox.Show("from date has to be between posted period " + UIUtil.toDateString(minTranDate) + " and " + UIUtil.toDateString(maxTranDate))
                isError = True
            ElseIf toDate < minTranDate OrElse toDate > maxTranDate Then
                MessageBox.Show("to date has to be between posted period " + UIUtil.toDateString(minTranDate) + " and " + UIUtil.toDateString(maxTranDate))
                isError = True
            ElseIf fromDate > toDate Then
                MessageBox.Show("from date has to be less than to date")
                isError = True
            End If
        End If



        If Not isError Then
            Me.DialogResult = DialogResult.OK
        End If

    End Sub
End Class