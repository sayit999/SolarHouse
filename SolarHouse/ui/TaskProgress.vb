Imports System.Threading

Public Class TaskProgress
    Private trd As Thread
    Private inc As Integer = -1
    Private numTasks As Integer = 0

    Private incCalculated As Integer


    Public Sub New(numTasks As Integer, title As String)
        ' This call is required by the designer.
        InitializeComponent()
        Me.Text = title
        Me.numTasks = numTasks
        incCalculated = 100 / numTasks
    End Sub

    Public Sub setMessage(message As String)
        progressMessageTxtBox.Text = message
    End Sub

    Public Sub increment(mesg As String)
        setMessage(mesg)
        setProgressInc(incCalculated)
    End Sub

    Public Sub setProgressInc(progressInc As Integer)
        inc = progressInc


        'Dim newval As Integer
        'Dim isDone As Boolean = False
        'newval = (progressBar.Value + progressInc)
        'If newval > progressBar.Maximum Then
        '    newval = progressBar.Maximum
        '    isDone = True
        'ElseIf newval < progressBar.Minimum Then
        '    newval = progressBar.Minimum
        'End If

        'progressBar.Increment(progressInc)
        'progressBar.Refresh()
        'If Not isDone Then
        '    inc = -1
        '    '  Thread.Sleep(100)
        'Else
        '    'Me.Close()
        '    'Me.Dispose()
        'End If

    End Sub

    'Delegate Sub IntArgReturningVoidDelegate([integer] As Integer)

    'Private Sub threadSafeIncProgress([integer] As Integer)
    '    Dim isDone As Boolean = False

    '    If Me.progressBar.InvokeRequired Then
    '        Dim d As New IntArgReturningVoidDelegate(AddressOf threadSafeIncProgress)
    '        Me.Invoke(d, New Object() {[integer]})
    '    Else
    '        progressBar.Value = [integer]
    '    End If

    'End Sub

    'Private Sub doProgress()
    '    Dim newval As Integer
    '    Dim isDone As Boolean = False
    '    Do
    '        If (inc <> -1) Then

    '            newval = progressBar.Value + inc
    '            If newval > progressBar.Maximum Then
    '                newval = progressBar.Maximum
    '                isDone = True
    '            ElseIf newval < progressBar.Minimum Then
    '                newval = progressBar.Minimum
    '            End If

    '            threadSafeIncProgress(newval)

    '            If Not isDone Then
    '                inc = -1
    '                Thread.Sleep(100)
    '            Else
    '                Me.Close()
    '                Me.Dispose()
    '            End If

    '        End If

    '    Loop
    'End Sub

    Private Sub TaskProgress_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' progressBar.Style = ProgressBarStyle.Blocks
        'progressBar.Minimum = 1
        'progressBar.Maximum = 100
        'progressBar.Value = 99

        'trd = New Thread(AddressOf doProgress)
        'trd.IsBackground = True
        'trd.Start()
    End Sub

End Class