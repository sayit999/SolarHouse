Imports System.Net.Mail
Imports System
Imports System.Net
Imports System.Net.Mime
Imports System.Threading
Imports System.ComponentModel
Imports System.IO
Public Class EmailService
    Public Const TEMP_EMAIL_FILE_INDICATOR As String = "$$$"




    Public Shared Sub deletePrevTempEmailFiles()
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(ReportFile.getToSubmitFolderName(), FileIO.SearchOption.SearchTopLevelOnly, "*" + TEMP_EMAIL_FILE_INDICATOR + "*")
            Try
                File.Delete(foundFile)
            Catch ex As System.IO.IOException
            End Try
        Next
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(ReportFile.getSubmitedFolderName(), FileIO.SearchOption.SearchTopLevelOnly, "*" + TEMP_EMAIL_FILE_INDICATOR + "*")
            Try
                File.Delete(foundFile)
            Catch ex As System.IO.IOException
            End Try
        Next
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(ReportFile.getLoadedFolderName(), FileIO.SearchOption.SearchTopLevelOnly, "*" + TEMP_EMAIL_FILE_INDICATOR + "*")
            Try
                File.Delete(foundFile)
            Catch ex As System.IO.IOException
            End Try
        Next
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(ReportFile.getToLoadFolderName(), FileIO.SearchOption.SearchTopLevelOnly, "*" + TEMP_EMAIL_FILE_INDICATOR + "*")
            Try
                File.Delete(foundFile)
            Catch ex As System.IO.IOException
            End Try
        Next
    End Sub

    Protected Function createTempFileToEmail(xmlFileToSubmitAbs As String) As String
        Dim fileName As String
        Dim folder As String
        ReportFile.sepFileNameFromFolder(xmlFileToSubmitAbs, folder, fileName)
        fileName = fileName + TEMP_EMAIL_FILE_INDICATOR + Format(Now(), "yyyy-MM-dd-H-mm-ss")
        Dim destFile As String = MiscUtil.appendPathComponent(folder, fileName)
        File.Copy(xmlFileToSubmitAbs, destFile, True)
        Return destFile
    End Function

    Public Sub emailFile(reportTitle As String, sendEmailTo As String, absFileLoc As String)
        Dim smtpServer As SmtpClient = Nothing
        Dim eMail As MailMessage = Nothing
        Try
            smtpServer = New SmtpClient
            eMail = New MailMessage()
            smtpServer.UseDefaultCredentials = False
            smtpServer.Credentials = New Net.NetworkCredential("solarhousearusha@gmail.com", "arusha239")
            smtpServer.Port = 587
            smtpServer.EnableSsl = True
            smtpServer.Host = "smtp.gmail.com"

            eMail = New MailMessage()
            eMail.From = New MailAddress("solarhousearusha@gmail.com")

            Dim emailTo() As String = sendEmailTo.Split(";")
            For i As Integer = 0 To emailTo.Length - 1
                If Not StringUtil.isEmpty(emailTo(i)) Then
                    eMail.To.Add(emailTo(i).Trim())
                End If
            Next

            eMail.Subject = reportTitle
            eMail.IsBodyHtml = False
            eMail.Body = reportTitle

            eMail.Attachments.Add(New Attachment(createTempFileToEmail(absFileLoc)))

            smtpServer.Send(eMail)
        Finally
            If IsNothing(eMail) Then
                eMail.Dispose()
            End If
            If IsNothing(smtpServer) Then

                smtpServer.Dispose()
            End If

        End Try
    End Sub

End Class
