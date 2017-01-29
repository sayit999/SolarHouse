Imports System.Threading

Public Class RunCmdUtil
    Const SLEEP_AMOUNT_MILLISECS As Integer = 100
    Const MAX_SLEEP_AMT_MILLISECS As Integer = 30 * 1000

    Private WithEvents myProcess As Process
    Private elapsedTime As Integer

    Sub runCmd(cmd As String, arg As String)
        elapsedTime = 0
        myProcess = System.Diagnostics.Process.Start(cmd, arg)

        Do While Not myProcess.HasExited
            elapsedTime += SLEEP_AMOUNT_MILLISECS
            If elapsedTime > MAX_SLEEP_AMT_MILLISECS Then
                Throw New Exception("Waited " + (MAX_SLEEP_AMT_MILLISECS * 0.001).ToString + "secs. Commmand:" + cmd + " did not complete")
            End If
            Thread.Sleep(SLEEP_AMOUNT_MILLISECS)
        Loop
    End Sub


End Class
