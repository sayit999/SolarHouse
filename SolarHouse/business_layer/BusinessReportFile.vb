Imports System.IO
Imports System.Collections.ObjectModel



Public Class ReportFile
    Enum BusinessReportStatus
        submitted
        to_submit
        to_load
        loaded
        new_report
        posted_report
    End Enum

    Public reportStatus As BusinessReportStatus = BusinessReportStatus.to_submit
    Public fileName As String = ""
    Public folderName As String = ""
    Public fromDate As Date = Nothing
    Public toDate As Date = Nothing

    Public Const FILE_EXT As String = ".xml"

    Public Overloads Function Equals(folder As String, nameOfFile As String) As Boolean
        Return nameOfFile = fileName AndAlso folder = folderName
    End Function

    Public Overloads Function Equals(absFile As String) As Boolean
        Return (folderName + "\" + fileName).Equals(absFile)
    End Function

    Public Overrides Function Equals(obj As [Object]) As Boolean
        Return TypeOf obj Is ReportFile AndAlso Not IsNothing(obj) AndAlso obj.fileName = fileName AndAlso obj.folderName = folderName
    End Function

    Public Function getAbsFileName() As String
        Return MiscUtil.appendPathComponent(folderName, fileName)
    End Function

    Public Function isToSubmit() As Boolean
        Return reportStatus = BusinessReportStatus.to_submit
    End Function

    Public Function isSubmitted() As Boolean
        Return reportStatus = BusinessReportStatus.submitted
    End Function

    Public Function isLoaded() As Boolean
        Return reportStatus = BusinessReportStatus.loaded
    End Function

    Public Function isPosted() As Boolean
        Return reportStatus = BusinessReportStatus.posted_report
    End Function

    Public Function isNew() As Boolean
        Return reportStatus = BusinessReportStatus.new_report
    End Function

    Public Function isToLoad() As Boolean
        Return reportStatus = BusinessReportStatus.to_load
    End Function
    Public Shared Sub sepFileNameFromFolder(absPath As String, ByRef folder As String, ByRef fileName As String)
        folder = ""
        fileName = ""

        Dim ind As Integer = absPath.LastIndexOf("\")
        If (ind <> -1) Then
            folder = absPath.Substring(0, ind)
            fileName = absPath.Substring(ind + 1)
        End If
    End Sub

    Public Shared Function listBusinessReports(status As BusinessReportStatus) As List(Of ReportFile)

        Dim filePath As String
        Dim i As Integer
        Dim file As ReportFile
        Dim coll As ReadOnlyCollection(Of String)
        Dim collectionToReturn As List(Of ReportFile) = New List(Of ReportFile)
        Dim dao As BusinessReportDAO = New BusinessReportDAO

        EmailService.deletePrevTempEmailFiles()

        Select Case status
            Case BusinessReportStatus.submitted
                filePath = getSubmitedFolderName()
            Case BusinessReportStatus.loaded
                filePath = getLoadedFolderName()
            Case BusinessReportStatus.to_load
                filePath = getToLoadFolderName()
            Case Else
                filePath = getToSubmitFolderName()
        End Select

        If (Not IsNothing(filePath)) Then
            coll = My.Computer.FileSystem.GetFiles(filePath)
            For i = 0 To coll.Count - 1
                If (coll(i).IndexOf(EmailService.TEMP_EMAIL_FILE_INDICATOR) = -1) Then
                    file = New ReportFile
                    file.reportStatus = status
                    sepFileNameFromFolder(coll(i), file.folderName, file.fileName)
                    dao.readReportDates(coll(i), file.fromDate, file.toDate)
                    collectionToReturn.Add(file)
                End If
            Next
        Else
            file = New ReportFile
            file.reportStatus = status
            collectionToReturn.Add(file)
        End If

        Return collectionToReturn
    End Function

    Public Shared Function getToLoadFolderName()
        Dim appFolder As String = MainForm.getApplicationDataFolder
        Return MiscUtil.appendPathComponent(appFolder, My.Settings.business_reports_to_load)
    End Function

    Public Shared Function getLoadedFolderName()
        Dim appFolder As String = MainForm.getApplicationDataFolder
        Return MiscUtil.appendPathComponent(appFolder, My.Settings.business_reports_loaded)
    End Function

    Public Shared Function getToSubmitFolderName()
        Dim appFolder As String = MainForm.getApplicationDataFolder
        getToSubmitFolderName = MiscUtil.appendPathComponent(appFolder, My.Settings.business_reports_to_submit_dir)
    End Function

    Public Shared Function getSubmitedFolderName()
        Dim appFolder As String = MainForm.getApplicationDataFolder
        getSubmitedFolderName = MiscUtil.appendPathComponent(appFolder, My.Settings.business_reports_submitted_dir)
    End Function


    Public Shared Function getXMLFileNameToSubmit(fromDate As Date, toDate As Date, Optional reprtFolder As String = Nothing, Optional isAnAmendmentToPostedTrans As Boolean = False) As String
        Dim fileName As String
        fileName = If(isAnAmendmentToPostedTrans, "amendment report ", "business report ") + Replace(fromDate.ToShortDateString, "/", "-") + "  " + Replace(toDate.ToShortDateString, "/", "-")
        If (fileName.IndexOf(FILE_EXT) = -1) Then
            fileName += FILE_EXT
        End If

        If (Not StringUtil.isEmpty(reprtFolder)) Then
            fileName = MiscUtil.appendPathComponent(reprtFolder, fileName)
        End If
        getXMLFileNameToSubmit = fileName
    End Function

End Class
