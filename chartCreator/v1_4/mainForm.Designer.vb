<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblImgFile = New System.Windows.Forms.Label()
        Me.btnOpenImg = New System.Windows.Forms.Button()
        Me.gbChartInfo = New System.Windows.Forms.GroupBox()
        Me.lblBpmChanges = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblOcount = New System.Windows.Forms.Label()
        Me.lblBcount = New System.Windows.Forms.Label()
        Me.lblYcount = New System.Windows.Forms.Label()
        Me.lblRcount = New System.Windows.Forms.Label()
        Me.lblGcount = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblHammers = New System.Windows.Forms.Label()
        Me.lblStrums = New System.Windows.Forms.Label()
        Me.lblMeasures = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnScan = New System.Windows.Forms.Button()
        Me.lblScanVal = New System.Windows.Forms.Label()
        Me.lblScanInfo = New System.Windows.Forms.Label()
        Me.iProgressBar = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.iMenu = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenChartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveChartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseChartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TimingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnEnterBpm = New System.Windows.Forms.Button()
        Me.btnViewChart = New System.Windows.Forms.Button()
        Me.gbChartInfo.SuspendLayout()
        Me.iMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Chart Image:"
        '
        'lblImgFile
        '
        Me.lblImgFile.ForeColor = System.Drawing.Color.RoyalBlue
        Me.lblImgFile.Location = New System.Drawing.Point(85, 34)
        Me.lblImgFile.Name = "lblImgFile"
        Me.lblImgFile.Size = New System.Drawing.Size(164, 13)
        Me.lblImgFile.TabIndex = 1
        Me.lblImgFile.Text = "no image selected!"
        '
        'btnOpenImg
        '
        Me.btnOpenImg.Location = New System.Drawing.Point(295, 27)
        Me.btnOpenImg.Name = "btnOpenImg"
        Me.btnOpenImg.Size = New System.Drawing.Size(75, 23)
        Me.btnOpenImg.TabIndex = 2
        Me.btnOpenImg.Text = "Open"
        Me.btnOpenImg.UseVisualStyleBackColor = True
        '
        'gbChartInfo
        '
        Me.gbChartInfo.Controls.Add(Me.lblBpmChanges)
        Me.gbChartInfo.Controls.Add(Me.Label2)
        Me.gbChartInfo.Controls.Add(Me.lblOcount)
        Me.gbChartInfo.Controls.Add(Me.lblBcount)
        Me.gbChartInfo.Controls.Add(Me.lblYcount)
        Me.gbChartInfo.Controls.Add(Me.lblRcount)
        Me.gbChartInfo.Controls.Add(Me.lblGcount)
        Me.gbChartInfo.Controls.Add(Me.Label13)
        Me.gbChartInfo.Controls.Add(Me.Label12)
        Me.gbChartInfo.Controls.Add(Me.Label11)
        Me.gbChartInfo.Controls.Add(Me.Label10)
        Me.gbChartInfo.Controls.Add(Me.Label9)
        Me.gbChartInfo.Controls.Add(Me.lblHammers)
        Me.gbChartInfo.Controls.Add(Me.lblStrums)
        Me.gbChartInfo.Controls.Add(Me.lblMeasures)
        Me.gbChartInfo.Controls.Add(Me.Label8)
        Me.gbChartInfo.Controls.Add(Me.Label7)
        Me.gbChartInfo.Controls.Add(Me.Label5)
        Me.gbChartInfo.ForeColor = System.Drawing.Color.RoyalBlue
        Me.gbChartInfo.Location = New System.Drawing.Point(15, 60)
        Me.gbChartInfo.Name = "gbChartInfo"
        Me.gbChartInfo.Size = New System.Drawing.Size(226, 151)
        Me.gbChartInfo.TabIndex = 3
        Me.gbChartInfo.TabStop = False
        Me.gbChartInfo.Text = "Chart Details"
        '
        'lblBpmChanges
        '
        Me.lblBpmChanges.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBpmChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBpmChanges.Location = New System.Drawing.Point(70, 125)
        Me.lblBpmChanges.Name = "lblBpmChanges"
        Me.lblBpmChanges.Size = New System.Drawing.Size(42, 15)
        Me.lblBpmChanges.TabIndex = 26
        Me.lblBpmChanges.Text = "N/A"
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(6, 115)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 33)
        Me.Label2.TabIndex = 25
        Me.Label2.Text = "BPM Changes:"
        '
        'lblOcount
        '
        Me.lblOcount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOcount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOcount.Location = New System.Drawing.Point(165, 126)
        Me.lblOcount.Name = "lblOcount"
        Me.lblOcount.Size = New System.Drawing.Size(42, 15)
        Me.lblOcount.TabIndex = 23
        Me.lblOcount.Text = "N/A"
        '
        'lblBcount
        '
        Me.lblBcount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBcount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBcount.Location = New System.Drawing.Point(165, 98)
        Me.lblBcount.Name = "lblBcount"
        Me.lblBcount.Size = New System.Drawing.Size(42, 15)
        Me.lblBcount.TabIndex = 22
        Me.lblBcount.Text = "N/A"
        '
        'lblYcount
        '
        Me.lblYcount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYcount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYcount.Location = New System.Drawing.Point(165, 70)
        Me.lblYcount.Name = "lblYcount"
        Me.lblYcount.Size = New System.Drawing.Size(42, 15)
        Me.lblYcount.TabIndex = 21
        Me.lblYcount.Text = "N/A"
        '
        'lblRcount
        '
        Me.lblRcount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRcount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRcount.Location = New System.Drawing.Point(165, 42)
        Me.lblRcount.Name = "lblRcount"
        Me.lblRcount.Size = New System.Drawing.Size(42, 15)
        Me.lblRcount.TabIndex = 20
        Me.lblRcount.Text = "N/A"
        '
        'lblGcount
        '
        Me.lblGcount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGcount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGcount.Location = New System.Drawing.Point(165, 14)
        Me.lblGcount.Name = "lblGcount"
        Me.lblGcount.Size = New System.Drawing.Size(42, 15)
        Me.lblGcount.TabIndex = 19
        Me.lblGcount.Text = "N/A"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(132, 128)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(18, 13)
        Me.Label13.TabIndex = 17
        Me.Label13.Text = "O:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(132, 100)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(17, 13)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "B:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(133, 72)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(17, 13)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "Y:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(132, 44)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(18, 13)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "R:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(132, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(18, 13)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "G:"
        '
        'lblHammers
        '
        Me.lblHammers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHammers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHammers.Location = New System.Drawing.Point(70, 88)
        Me.lblHammers.Name = "lblHammers"
        Me.lblHammers.Size = New System.Drawing.Size(42, 15)
        Me.lblHammers.TabIndex = 10
        Me.lblHammers.Text = "N/A"
        '
        'lblStrums
        '
        Me.lblStrums.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStrums.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStrums.Location = New System.Drawing.Point(70, 51)
        Me.lblStrums.Name = "lblStrums"
        Me.lblStrums.Size = New System.Drawing.Size(42, 15)
        Me.lblStrums.TabIndex = 9
        Me.lblStrums.Text = "N/A"
        '
        'lblMeasures
        '
        Me.lblMeasures.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMeasures.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMeasures.Location = New System.Drawing.Point(70, 14)
        Me.lblMeasures.Name = "lblMeasures"
        Me.lblMeasures.Size = New System.Drawing.Size(42, 15)
        Me.lblMeasures.TabIndex = 6
        Me.lblMeasures.Text = "N/A"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(6, 90)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(54, 13)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "Hammers:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(6, 53)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(42, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Strums:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(6, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Measures:"
        '
        'btnScan
        '
        Me.btnScan.Enabled = False
        Me.btnScan.Location = New System.Drawing.Point(247, 64)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(123, 23)
        Me.btnScan.TabIndex = 4
        Me.btnScan.Text = "Scan Image"
        Me.btnScan.UseVisualStyleBackColor = True
        '
        'lblScanVal
        '
        Me.lblScanVal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblScanVal.BackColor = System.Drawing.Color.Transparent
        Me.lblScanVal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblScanVal.Location = New System.Drawing.Point(245, 92)
        Me.lblScanVal.Name = "lblScanVal"
        Me.lblScanVal.Size = New System.Drawing.Size(41, 18)
        Me.lblScanVal.TabIndex = 5
        Me.lblScanVal.Text = "0"
        Me.lblScanVal.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblScanVal.Visible = False
        '
        'lblScanInfo
        '
        Me.lblScanInfo.AutoSize = True
        Me.lblScanInfo.BackColor = System.Drawing.Color.Transparent
        Me.lblScanInfo.Location = New System.Drawing.Point(283, 92)
        Me.lblScanInfo.Name = "lblScanInfo"
        Me.lblScanInfo.Size = New System.Drawing.Size(87, 13)
        Me.lblScanInfo.TabIndex = 6
        Me.lblScanInfo.Text = "seconds elapsed"
        Me.lblScanInfo.Visible = False
        '
        'iProgressBar
        '
        Me.iProgressBar.Location = New System.Drawing.Point(15, 217)
        Me.iProgressBar.Name = "iProgressBar"
        Me.iProgressBar.Size = New System.Drawing.Size(355, 23)
        Me.iProgressBar.Step = 1
        Me.iProgressBar.TabIndex = 7
        '
        'lblProgress
        '
        Me.lblProgress.Location = New System.Drawing.Point(163, 243)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(59, 23)
        Me.lblProgress.TabIndex = 8
        Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'iMenu
        '
        Me.iMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.InfoToolStripMenuItem})
        Me.iMenu.Location = New System.Drawing.Point(0, 0)
        Me.iMenu.Name = "iMenu"
        Me.iMenu.Size = New System.Drawing.Size(382, 24)
        Me.iMenu.TabIndex = 9
        Me.iMenu.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenChartToolStripMenuItem, Me.SaveChartToolStripMenuItem, Me.CloseChartToolStripMenuItem, Me.ToolStripMenuItem1, Me.TimingToolStripMenuItem, Me.ToolStripSeparator1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenChartToolStripMenuItem
        '
        Me.OpenChartToolStripMenuItem.Name = "OpenChartToolStripMenuItem"
        Me.OpenChartToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.OpenChartToolStripMenuItem.Text = "Open Chart"
        '
        'SaveChartToolStripMenuItem
        '
        Me.SaveChartToolStripMenuItem.Enabled = False
        Me.SaveChartToolStripMenuItem.Name = "SaveChartToolStripMenuItem"
        Me.SaveChartToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.SaveChartToolStripMenuItem.Text = "Save Chart"
        '
        'CloseChartToolStripMenuItem
        '
        Me.CloseChartToolStripMenuItem.Enabled = False
        Me.CloseChartToolStripMenuItem.Name = "CloseChartToolStripMenuItem"
        Me.CloseChartToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.CloseChartToolStripMenuItem.Text = "Close Chart"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(132, 6)
        '
        'TimingToolStripMenuItem
        '
        Me.TimingToolStripMenuItem.Enabled = False
        Me.TimingToolStripMenuItem.Name = "TimingToolStripMenuItem"
        Me.TimingToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.TimingToolStripMenuItem.Text = "&Timing"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(132, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'InfoToolStripMenuItem
        '
        Me.InfoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.ChangeLogToolStripMenuItem})
        Me.InfoToolStripMenuItem.Name = "InfoToolStripMenuItem"
        Me.InfoToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.InfoToolStripMenuItem.Text = "Info"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'ChangeLogToolStripMenuItem
        '
        Me.ChangeLogToolStripMenuItem.Name = "ChangeLogToolStripMenuItem"
        Me.ChangeLogToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ChangeLogToolStripMenuItem.Text = "Change &Log"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(247, 188)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(123, 23)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save Chart"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnEnterBpm
        '
        Me.btnEnterBpm.Enabled = False
        Me.btnEnterBpm.Location = New System.Drawing.Point(247, 130)
        Me.btnEnterBpm.Name = "btnEnterBpm"
        Me.btnEnterBpm.Size = New System.Drawing.Size(123, 23)
        Me.btnEnterBpm.TabIndex = 11
        Me.btnEnterBpm.Text = "Verify BPM Values"
        Me.btnEnterBpm.UseVisualStyleBackColor = True
        '
        'btnViewChart
        '
        Me.btnViewChart.Enabled = False
        Me.btnViewChart.Location = New System.Drawing.Point(247, 159)
        Me.btnViewChart.Name = "btnViewChart"
        Me.btnViewChart.Size = New System.Drawing.Size(123, 23)
        Me.btnViewChart.TabIndex = 12
        Me.btnViewChart.Text = "Build Chart"
        Me.btnViewChart.UseVisualStyleBackColor = True
        '
        'mainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(382, 274)
        Me.Controls.Add(Me.btnViewChart)
        Me.Controls.Add(Me.btnEnterBpm)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.iProgressBar)
        Me.Controls.Add(Me.lblScanInfo)
        Me.Controls.Add(Me.lblScanVal)
        Me.Controls.Add(Me.btnScan)
        Me.Controls.Add(Me.gbChartInfo)
        Me.Controls.Add(Me.btnOpenImg)
        Me.Controls.Add(Me.lblImgFile)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.iMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MainMenuStrip = Me.iMenu
        Me.MaximizeBox = False
        Me.Name = "mainForm"
        Me.Text = "RB Bot Chart Creator"
        Me.gbChartInfo.ResumeLayout(False)
        Me.gbChartInfo.PerformLayout()
        Me.iMenu.ResumeLayout(False)
        Me.iMenu.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblImgFile As System.Windows.Forms.Label
    Friend WithEvents btnOpenImg As System.Windows.Forms.Button
    Friend WithEvents gbChartInfo As System.Windows.Forms.GroupBox
    Friend WithEvents btnScan As System.Windows.Forms.Button
    Friend WithEvents lblScanVal As System.Windows.Forms.Label
    Friend WithEvents lblScanInfo As System.Windows.Forms.Label
    Friend WithEvents iProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents iMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenChartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveChartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseChartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents InfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblHammers As System.Windows.Forms.Label
    Friend WithEvents lblStrums As System.Windows.Forms.Label
    Friend WithEvents lblMeasures As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblOcount As System.Windows.Forms.Label
    Friend WithEvents lblBcount As System.Windows.Forms.Label
    Friend WithEvents lblYcount As System.Windows.Forms.Label
    Friend WithEvents lblRcount As System.Windows.Forms.Label
    Friend WithEvents lblGcount As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblBpmChanges As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnEnterBpm As System.Windows.Forms.Button
    Friend WithEvents btnViewChart As System.Windows.Forms.Button
    Friend WithEvents TimingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ChangeLogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
