Imports System.IO

Public Class MainForm

    Public Shared Function getApplicationDataFolder() As String
        Dim path As String = Application.LocalUserAppDataPath
        Return path.Substring(0, path.LastIndexOf("\"))
    End Function

    Private Sub BusinessReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BusinessReportToolStripMenuItem.Click
        BusinessReportDlg.ShowDialog()
    End Sub

    Private Sub BusinessReportTBBtn_Click(sender As Object, e As EventArgs) Handles BusinessReportTBBtn.Click
        BusinessReportToolStripMenuItem_Click(sender, e)
    End Sub

    Private Function getMySqlBackUpFolder()
        Dim appFolder As String = MainForm.getApplicationDataFolder
        Return MiscUtil.appendPathComponent(appFolder, My.Settings.backup_dir)
    End Function

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' ensure app folders exist
            If (Not Directory.Exists(Application.UserAppDataPath)) Then
                FileSystem.MkDir(Application.UserAppDataPath)
            End If
            If (Not Directory.Exists(ReportFile.getToSubmitFolderName())) Then
                FileSystem.MkDir(ReportFile.getToSubmitFolderName())
            End If
            If (Not Directory.Exists(ReportFile.getSubmitedFolderName())) Then
                FileSystem.MkDir(ReportFile.getSubmitedFolderName())
            End If

            If (Not Directory.Exists(ReportFile.getToLoadFolderName())) Then
                FileSystem.MkDir(ReportFile.getToLoadFolderName())
            End If

            If (Not Directory.Exists(ReportFile.getLoadedFolderName())) Then
                FileSystem.MkDir(ReportFile.getLoadedFolderName())
            End If

            If (Not Directory.Exists(BusinessReportDAO.getBackupFolder())) Then
                FileSystem.MkDir(BusinessReportDAO.getBackupFolder())
            End If


            If (Not Directory.Exists(getMySqlBackUpFolder())) Then
                FileSystem.MkDir(getMySqlBackUpFolder())
            End If

            Me.Text = "Solar House Arusha - " + My.Application.Info.Version.ToString
        Catch ex As IOException
            MessageBox.Show(Me, "Failed to create required folders.  Reason:" + ex.ToString)
        End Try

        'highlight DB we using
        If LCase(My.Settings.database_name).IndexOf("test") <> -1 Then
            Me.BackColor = Color.FromArgb(206, 255, 219)
        Else
            Me.BackColor = Color.FromArgb(255, 193, 193)
        End If

        MainToolStrip.BackColor = Me.BackColor
        AppProgressBar.Style = ProgressBarStyle.Blocks
        AppProgressBar.Minimum = 0
        AppProgressBar.Maximum = 100

        runMySqlScript()

    End Sub


    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox.Show()
    End Sub

    Public Sub showPogressAsMarquee()
        AppProgressBar.ProgressBar.Style = ProgressBarStyle.Marquee
        AppProgressBar.ProgressBar.Show()
    End Sub

    Public Sub showProgress(Optional inc As Integer = -1, Optional statusMsg As String = "")
        If inc < 0 Then
            AppProgressBar.ProgressBar.Value = 0
        Else
            AppProgressBar.Increment(inc)
        End If

        AppToolStripStatusLabel.Text = statusMsg
    End Sub

    Private Sub runMySqlScript()
        Dim serv As BusinessReportService = New BusinessReportService(Me)
        Dim files As String() = Directory.GetFiles(".")
        Dim sqlFiles As New List(Of String)
        For i As Integer = 0 To files.Length - 1
            If (files(i).LastIndexOf(".sql") <> -1) Then
                sqlFiles.Add(files(i))
            End If
        Next
        sqlFiles.Sort()
        For i As Integer = 0 To sqlFiles.Count - 1
            If (Not serv.runMySqlScript(sqlFiles(i))) Then
                MessageBox.Show(Me, "Failed to execute Sql Script " + sqlFiles(i))
            End If
            Dim fileName As String = Path.GetFileName(sqlFiles(i))
            Dim nameSufx As String = Now().ToString()
            nameSufx = nameSufx.Replace("/", "-")
            nameSufx = nameSufx.Replace(":", "-")
            My.Computer.FileSystem.MoveFile(sqlFiles(i), MiscUtil.appendPathComponent(getMySqlBackUpFolder(), fileName + "." + nameSufx))

        Next
    End Sub

    Private Sub SolarCalcTBBtn_Click(sender As Object, e As EventArgs) Handles SolarCalcTBBtn.Click
        MessageBox.Show(Me, "To be implemented")
    End Sub

    Private Sub RestoreDatabaseBtn_Click(sender As Object, e As EventArgs) Handles RestoreDatabaseBtn.Click
        If MessageBox.Show(Me, "You will DELETE current data.  Do you want to restore the database ?", "Restore Database", MessageBoxButtons.YesNo) = DialogResult.No Then
            Return
        End If
        Try
            Dim fileDlg As New OpenFileDialog
            fileDlg.InitialDirectory = BusinessReportDAO.getBackupFolder()
            If fileDlg.ShowDialog() = DialogResult.OK Then
                Dim dao As New BusinessReportDAO
                If (dao.restoreDatabase(fileDlg.FileName)) Then
                    MessageBox.Show(Me, "Database restored. Restore file :" + fileDlg.FileName)
                Else
                    MessageBox.Show(Me, "Failed to restore database. Restore file: " + fileDlg.FileName)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, "Failed to restore database.  reason:" + ex.ToString)
        End Try


    End Sub

    Private Sub BackupDatabaseBtn_Click(sender As Object, e As EventArgs) Handles BackupDatabaseBtn.Click
        Try
            Dim dao As New BusinessReportDAO
            Dim backupFile As String = dao.backUpDatabase()
            MessageBox.Show(Me, "Database backedup into file :" + backupFile)
        Catch ex As Exception
            MessageBox.Show(Me, "Failed to backup database.  reason:" + ex.ToString)
        End Try

    End Sub

    Private Sub EmailBackupDatabaseBtn_Click(sender As Object, e As EventArgs) Handles EmailBackupDatabaseBtn.Click
        Try
            Dim fileDlg As New OpenFileDialog
            fileDlg.InitialDirectory = BusinessReportDAO.getBackupFolder()
            If fileDlg.ShowDialog() = DialogResult.OK Then
                Dim serv As New BusinessReportService(Me)
                If (serv.emailDBBackup(fileDlg.FileName)) Then
                    MessageBox.Show(Me, "Emailed backup file:" + fileDlg.FileName)
                Else
                    MessageBox.Show(Me, "Failed to email backup file: " + fileDlg.FileName)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, "Failed to email backup.  reason:" + ex.ToString)
        End Try
    End Sub

    Private Sub CalcQtyACBBtn_Click(sender As Object, e As EventArgs) Handles CalcQtyACBBtn.Click
        Dim serv As New BusinessReportService(Me)
        If serv.recalcProductAcbAndQty() Then
            MessageBox.Show("Inventory ACB and Qty Recalculated")
        End If
    End Sub

End Class
