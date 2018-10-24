<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.AppMenuStrip = New System.Windows.Forms.MenuStrip()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BusinessReportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainToolStrip = New System.Windows.Forms.ToolStrip()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.BusinessReportTBBtn = New System.Windows.Forms.ToolStripButton()
        Me.SolarCalcTBBtn = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.BackupDatabaseBtn = New System.Windows.Forms.ToolStripButton()
        Me.RestoreDatabaseBtn = New System.Windows.Forms.ToolStripButton()
        Me.EmailBackupDatabaseBtn = New System.Windows.Forms.ToolStripButton()
        Me.CalcQtyACBBtn = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.AppProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.AppToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.debtPaymentHistoryButton = New System.Windows.Forms.ToolStripButton()
        Me.lowStockBtn = New System.Windows.Forms.ToolStripButton()
        Me.AppMenuStrip.SuspendLayout()
        Me.MainToolStrip.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'AppMenuStrip
        '
        Me.AppMenuStrip.AutoSize = False
        Me.AppMenuStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.AppMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.AppMenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.AppMenuStrip.Name = "AppMenuStrip"
        Me.AppMenuStrip.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
        Me.AppMenuStrip.Size = New System.Drawing.Size(1300, 23)
        Me.AppMenuStrip.TabIndex = 0
        Me.AppMenuStrip.Text = "MenuStrip1"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BusinessReportToolStripMenuItem, Me.ToolStripSeparator1})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 19)
        Me.EditToolStripMenuItem.Text = "&Edit"
        '
        'BusinessReportToolStripMenuItem
        '
        Me.BusinessReportToolStripMenuItem.Name = "BusinessReportToolStripMenuItem"
        Me.BusinessReportToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.BusinessReportToolStripMenuItem.Text = "Business &Report"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(154, 6)
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 19)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'MainToolStrip
        '
        Me.MainToolStrip.AutoSize = False
        Me.MainToolStrip.BackColor = System.Drawing.Color.Silver
        Me.MainToolStrip.ImageScalingSize = New System.Drawing.Size(30, 30)
        Me.MainToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator2, Me.BusinessReportTBBtn, Me.SolarCalcTBBtn, Me.ToolStripSeparator4, Me.BackupDatabaseBtn, Me.RestoreDatabaseBtn, Me.EmailBackupDatabaseBtn, Me.CalcQtyACBBtn, Me.debtPaymentHistoryButton, Me.lowStockBtn})
        Me.MainToolStrip.Location = New System.Drawing.Point(0, 23)
        Me.MainToolStrip.Name = "MainToolStrip"
        Me.MainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.MainToolStrip.Size = New System.Drawing.Size(1300, 98)
        Me.MainToolStrip.Stretch = True
        Me.MainToolStrip.TabIndex = 1
        Me.MainToolStrip.Text = "ToolStrip1"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 98)
        '
        'BusinessReportTBBtn
        '
        Me.BusinessReportTBBtn.AutoSize = False
        Me.BusinessReportTBBtn.BackColor = System.Drawing.Color.White
        Me.BusinessReportTBBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BusinessReportTBBtn.Image = CType(resources.GetObject("BusinessReportTBBtn.Image"), System.Drawing.Image)
        Me.BusinessReportTBBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.BusinessReportTBBtn.ImageTransparentColor = System.Drawing.Color.White
        Me.BusinessReportTBBtn.Name = "BusinessReportTBBtn"
        Me.BusinessReportTBBtn.Size = New System.Drawing.Size(142, 90)
        Me.BusinessReportTBBtn.Text = "Business Report"
        Me.BusinessReportTBBtn.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        '
        'SolarCalcTBBtn
        '
        Me.SolarCalcTBBtn.AutoSize = False
        Me.SolarCalcTBBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SolarCalcTBBtn.Image = CType(resources.GetObject("SolarCalcTBBtn.Image"), System.Drawing.Image)
        Me.SolarCalcTBBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.SolarCalcTBBtn.ImageTransparentColor = System.Drawing.Color.White
        Me.SolarCalcTBBtn.Name = "SolarCalcTBBtn"
        Me.SolarCalcTBBtn.Size = New System.Drawing.Size(114, 186)
        Me.SolarCalcTBBtn.Text = "Solar Calculations"
        Me.SolarCalcTBBtn.ToolTipText = "Solar Calculations"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 98)
        '
        'BackupDatabaseBtn
        '
        Me.BackupDatabaseBtn.AutoSize = False
        Me.BackupDatabaseBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BackupDatabaseBtn.Image = CType(resources.GetObject("BackupDatabaseBtn.Image"), System.Drawing.Image)
        Me.BackupDatabaseBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.BackupDatabaseBtn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BackupDatabaseBtn.Name = "BackupDatabaseBtn"
        Me.BackupDatabaseBtn.Size = New System.Drawing.Size(147, 186)
        Me.BackupDatabaseBtn.Text = "Backup Database"
        '
        'RestoreDatabaseBtn
        '
        Me.RestoreDatabaseBtn.AutoSize = False
        Me.RestoreDatabaseBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.RestoreDatabaseBtn.Image = CType(resources.GetObject("RestoreDatabaseBtn.Image"), System.Drawing.Image)
        Me.RestoreDatabaseBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.RestoreDatabaseBtn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.RestoreDatabaseBtn.Name = "RestoreDatabaseBtn"
        Me.RestoreDatabaseBtn.Size = New System.Drawing.Size(147, 186)
        Me.RestoreDatabaseBtn.Text = "Restore Database"
        '
        'EmailBackupDatabaseBtn
        '
        Me.EmailBackupDatabaseBtn.AutoSize = False
        Me.EmailBackupDatabaseBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.EmailBackupDatabaseBtn.Image = CType(resources.GetObject("EmailBackupDatabaseBtn.Image"), System.Drawing.Image)
        Me.EmailBackupDatabaseBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.EmailBackupDatabaseBtn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.EmailBackupDatabaseBtn.Name = "EmailBackupDatabaseBtn"
        Me.EmailBackupDatabaseBtn.Size = New System.Drawing.Size(147, 186)
        Me.EmailBackupDatabaseBtn.Text = "Email Backuped Database "
        '
        'CalcQtyACBBtn
        '
        Me.CalcQtyACBBtn.AutoSize = False
        Me.CalcQtyACBBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CalcQtyACBBtn.Image = CType(resources.GetObject("CalcQtyACBBtn.Image"), System.Drawing.Image)
        Me.CalcQtyACBBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.CalcQtyACBBtn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CalcQtyACBBtn.Name = "CalcQtyACBBtn"
        Me.CalcQtyACBBtn.Size = New System.Drawing.Size(147, 186)
        Me.CalcQtyACBBtn.Text = "ToolStripButton1"
        Me.CalcQtyACBBtn.ToolTipText = "Calculate ACB & Qty"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AppProgressBar, Me.AppToolStripStatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 730)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(1300, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'AppProgressBar
        '
        Me.AppProgressBar.Name = "AppProgressBar"
        Me.AppProgressBar.Size = New System.Drawing.Size(75, 16)
        '
        'AppToolStripStatusLabel
        '
        Me.AppToolStripStatusLabel.Name = "AppToolStripStatusLabel"
        Me.AppToolStripStatusLabel.Size = New System.Drawing.Size(0, 17)
        '
        'debtPaymentHistoryButton
        '
        Me.debtPaymentHistoryButton.AutoSize = False
        Me.debtPaymentHistoryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.debtPaymentHistoryButton.Image = CType(resources.GetObject("debtPaymentHistoryButton.Image"), System.Drawing.Image)
        Me.debtPaymentHistoryButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.debtPaymentHistoryButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.debtPaymentHistoryButton.Name = "debtPaymentHistoryButton"
        Me.debtPaymentHistoryButton.Size = New System.Drawing.Size(147, 186)
        Me.debtPaymentHistoryButton.Text = "ToolStripButton1"
        Me.debtPaymentHistoryButton.ToolTipText = "Historia ya malipo and manunuzi ya mkopo"
        '
        'lowStockBtn
        '
        Me.lowStockBtn.AutoSize = False
        Me.lowStockBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.lowStockBtn.Image = CType(resources.GetObject("lowStockBtn.Image"), System.Drawing.Image)
        Me.lowStockBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.lowStockBtn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.lowStockBtn.Name = "lowStockBtn"
        Me.lowStockBtn.Size = New System.Drawing.Size(147, 186)
        Me.lowStockBtn.Text = "Mzigo yaku agiza"
        Me.lowStockBtn.ToolTipText = "Mzigo yaku agiza"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1300, 752)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MainToolStrip)
        Me.Controls.Add(Me.AppMenuStrip)
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "MainForm"
        Me.Text = "Solar House Arusha"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.AppMenuStrip.ResumeLayout(False)
        Me.AppMenuStrip.PerformLayout()
        Me.MainToolStrip.ResumeLayout(False)
        Me.MainToolStrip.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AppMenuStrip As MenuStrip
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BusinessReportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MainToolStrip As ToolStrip
    Friend WithEvents BusinessReportTBBtn As ToolStripButton
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AppProgressBar As ToolStripProgressBar
    Friend WithEvents AppToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents SolarCalcTBBtn As ToolStripButton
    Friend WithEvents RestoreDatabaseBtn As ToolStripButton
    Friend WithEvents BackupDatabaseBtn As ToolStripButton
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents EmailBackupDatabaseBtn As ToolStripButton
    Friend WithEvents CalcQtyACBBtn As ToolStripButton
    Friend WithEvents debtPaymentHistoryButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents lowStockBtn As System.Windows.Forms.ToolStripButton
End Class
