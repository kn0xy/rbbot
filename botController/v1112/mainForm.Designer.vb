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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mainForm))
        Me.txtConsole = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.llblShutdown = New System.Windows.Forms.LinkLabel()
        Me.lblAuthed = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.cbPorts = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.loginTimer = New System.Windows.Forms.Timer(Me.components)
        Me.serialReadTimer = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblBotTime = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblBotNotes = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblBotChart = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblBotStatus = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnAddChart = New System.Windows.Forms.Button()
        Me.btnLoadChart = New System.Windows.Forms.Button()
        Me.btnBotAction = New System.Windows.Forms.Button()
        Me.pnlBotControl = New System.Windows.Forms.Panel()
        Me.btnYellow = New System.Windows.Forms.Button()
        Me.btnRed = New System.Windows.Forms.Button()
        Me.btnGreen = New System.Windows.Forms.Button()
        Me.btnDown = New System.Windows.Forms.Button()
        Me.btnUp = New System.Windows.Forms.Button()
        Me.pnlBtnStatus = New System.Windows.Forms.Panel()
        Me.pnlBsDownstrum = New System.Windows.Forms.Panel()
        Me.pnlBsUpstrum = New System.Windows.Forms.Panel()
        Me.pnlBsOrange = New System.Windows.Forms.Panel()
        Me.pnlBsBlue = New System.Windows.Forms.Panel()
        Me.pnlBsYellow = New System.Windows.Forms.Panel()
        Me.pnlBsRed = New System.Windows.Forms.Panel()
        Me.pnlBsGreen = New System.Windows.Forms.Panel()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.botTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.TippyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConsoleModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ChangelogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.pnlBotControl.SuspendLayout()
        Me.pnlBtnStatus.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtConsole
        '
        Me.txtConsole.BackColor = System.Drawing.SystemColors.Control
        Me.txtConsole.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConsole.ForeColor = System.Drawing.Color.Blue
        Me.txtConsole.Location = New System.Drawing.Point(218, 32)
        Me.txtConsole.Multiline = True
        Me.txtConsole.Name = "txtConsole"
        Me.txtConsole.ReadOnly = True
        Me.txtConsole.Size = New System.Drawing.Size(323, 105)
        Me.txtConsole.TabIndex = 0
        Me.txtConsole.Text = "Knoxy RB Bot Controller v1.2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Last updated 06/24/2018" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "v1.0        11/19/2015" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & _
            "v1.1        06/23/2018" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "v1.2        06/24/2018" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.llblShutdown)
        Me.GroupBox1.Controls.Add(Me.lblAuthed)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.btnConnect)
        Me.GroupBox1.Controls.Add(Me.cbPorts)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox1.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox1.Location = New System.Drawing.Point(12, 27)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(199, 110)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connection"
        '
        'llblShutdown
        '
        Me.llblShutdown.AutoSize = True
        Me.llblShutdown.Location = New System.Drawing.Point(135, 67)
        Me.llblShutdown.Name = "llblShutdown"
        Me.llblShutdown.Size = New System.Drawing.Size(55, 13)
        Me.llblShutdown.TabIndex = 6
        Me.llblShutdown.TabStop = True
        Me.llblShutdown.Text = "Shutdown"
        Me.llblShutdown.Visible = False
        '
        'lblAuthed
        '
        Me.lblAuthed.AutoSize = True
        Me.lblAuthed.ForeColor = System.Drawing.Color.Red
        Me.lblAuthed.Location = New System.Drawing.Point(88, 67)
        Me.lblAuthed.Name = "lblAuthed"
        Me.lblAuthed.Size = New System.Drawing.Size(21, 13)
        Me.lblAuthed.TabIndex = 5
        Me.lblAuthed.Text = "No"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(6, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Authenticated:"
        '
        'btnConnect
        '
        Me.btnConnect.Enabled = False
        Me.btnConnect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnConnect.Location = New System.Drawing.Point(133, 32)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(60, 23)
        Me.btnConnect.TabIndex = 2
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'cbPorts
        '
        Me.cbPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPorts.FormattingEnabled = True
        Me.cbPorts.Location = New System.Drawing.Point(41, 34)
        Me.cbPorts.Name = "cbPorts"
        Me.cbPorts.Size = New System.Drawing.Size(77, 21)
        Me.cbPorts.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(6, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Port:"
        '
        'loginTimer
        '
        Me.loginTimer.Interval = 500
        '
        'serialReadTimer
        '
        Me.serialReadTimer.Interval = 25
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblBotTime)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.lblBotNotes)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.lblBotChart)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.lblBotStatus)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox2.Location = New System.Drawing.Point(12, 143)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 116)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Bot Info"
        '
        'lblBotTime
        '
        Me.lblBotTime.AutoSize = True
        Me.lblBotTime.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBotTime.Location = New System.Drawing.Point(70, 82)
        Me.lblBotTime.Name = "lblBotTime"
        Me.lblBotTime.Size = New System.Drawing.Size(27, 13)
        Me.lblBotTime.TabIndex = 7
        Me.lblBotTime.Text = "N/A"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(6, 82)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(33, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Time:"
        '
        'lblBotNotes
        '
        Me.lblBotNotes.AutoSize = True
        Me.lblBotNotes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBotNotes.Location = New System.Drawing.Point(70, 60)
        Me.lblBotNotes.Name = "lblBotNotes"
        Me.lblBotNotes.Size = New System.Drawing.Size(27, 13)
        Me.lblBotNotes.TabIndex = 5
        Me.lblBotNotes.Text = "N/A"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(6, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Notes:"
        '
        'lblBotChart
        '
        Me.lblBotChart.AutoSize = True
        Me.lblBotChart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBotChart.Location = New System.Drawing.Point(70, 38)
        Me.lblBotChart.Name = "lblBotChart"
        Me.lblBotChart.Size = New System.Drawing.Size(27, 13)
        Me.lblBotChart.TabIndex = 3
        Me.lblBotChart.Text = "N/A"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(6, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Chart:"
        '
        'lblBotStatus
        '
        Me.lblBotStatus.AutoSize = True
        Me.lblBotStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBotStatus.Location = New System.Drawing.Point(70, 16)
        Me.lblBotStatus.Name = "lblBotStatus"
        Me.lblBotStatus.Size = New System.Drawing.Size(27, 13)
        Me.lblBotStatus.TabIndex = 1
        Me.lblBotStatus.Text = "N/A"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(6, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Status:"
        '
        'btnAddChart
        '
        Me.btnAddChart.Enabled = False
        Me.btnAddChart.Location = New System.Drawing.Point(218, 149)
        Me.btnAddChart.Name = "btnAddChart"
        Me.btnAddChart.Size = New System.Drawing.Size(75, 23)
        Me.btnAddChart.TabIndex = 4
        Me.btnAddChart.Text = "Add Chart"
        Me.btnAddChart.UseVisualStyleBackColor = True
        '
        'btnLoadChart
        '
        Me.btnLoadChart.Enabled = False
        Me.btnLoadChart.Location = New System.Drawing.Point(218, 192)
        Me.btnLoadChart.Name = "btnLoadChart"
        Me.btnLoadChart.Size = New System.Drawing.Size(75, 23)
        Me.btnLoadChart.TabIndex = 5
        Me.btnLoadChart.Text = "Load Chart"
        Me.btnLoadChart.UseVisualStyleBackColor = True
        '
        'btnBotAction
        '
        Me.btnBotAction.Enabled = False
        Me.btnBotAction.Location = New System.Drawing.Point(218, 235)
        Me.btnBotAction.Name = "btnBotAction"
        Me.btnBotAction.Size = New System.Drawing.Size(75, 23)
        Me.btnBotAction.TabIndex = 6
        Me.btnBotAction.Text = "Start Bot"
        Me.btnBotAction.UseVisualStyleBackColor = True
        '
        'pnlBotControl
        '
        Me.pnlBotControl.Controls.Add(Me.btnYellow)
        Me.pnlBotControl.Controls.Add(Me.btnRed)
        Me.pnlBotControl.Controls.Add(Me.btnGreen)
        Me.pnlBotControl.Controls.Add(Me.btnDown)
        Me.pnlBotControl.Controls.Add(Me.btnUp)
        Me.pnlBotControl.Location = New System.Drawing.Point(324, 149)
        Me.pnlBotControl.Name = "pnlBotControl"
        Me.pnlBotControl.Size = New System.Drawing.Size(217, 67)
        Me.pnlBotControl.TabIndex = 7
        Me.pnlBotControl.Visible = False
        '
        'btnYellow
        '
        Me.btnYellow.BackColor = System.Drawing.Color.Yellow
        Me.btnYellow.Location = New System.Drawing.Point(123, 4)
        Me.btnYellow.Name = "btnYellow"
        Me.btnYellow.Size = New System.Drawing.Size(54, 53)
        Me.btnYellow.TabIndex = 4
        Me.btnYellow.UseVisualStyleBackColor = False
        '
        'btnRed
        '
        Me.btnRed.BackColor = System.Drawing.Color.Red
        Me.btnRed.Location = New System.Drawing.Point(63, 4)
        Me.btnRed.Name = "btnRed"
        Me.btnRed.Size = New System.Drawing.Size(54, 53)
        Me.btnRed.TabIndex = 3
        Me.btnRed.UseVisualStyleBackColor = False
        '
        'btnGreen
        '
        Me.btnGreen.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnGreen.Location = New System.Drawing.Point(3, 4)
        Me.btnGreen.Name = "btnGreen"
        Me.btnGreen.Size = New System.Drawing.Size(54, 53)
        Me.btnGreen.TabIndex = 2
        Me.btnGreen.UseVisualStyleBackColor = False
        '
        'btnDown
        '
        Me.btnDown.Location = New System.Drawing.Point(183, 32)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(25, 25)
        Me.btnDown.TabIndex = 1
        Me.btnDown.Text = "▼"
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Location = New System.Drawing.Point(183, 4)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(25, 25)
        Me.btnUp.TabIndex = 0
        Me.btnUp.Text = "▲"
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'pnlBtnStatus
        '
        Me.pnlBtnStatus.Controls.Add(Me.pnlBsDownstrum)
        Me.pnlBtnStatus.Controls.Add(Me.pnlBsUpstrum)
        Me.pnlBtnStatus.Controls.Add(Me.pnlBsOrange)
        Me.pnlBtnStatus.Controls.Add(Me.pnlBsBlue)
        Me.pnlBtnStatus.Controls.Add(Me.pnlBsYellow)
        Me.pnlBtnStatus.Controls.Add(Me.pnlBsRed)
        Me.pnlBtnStatus.Controls.Add(Me.pnlBsGreen)
        Me.pnlBtnStatus.Location = New System.Drawing.Point(324, 225)
        Me.pnlBtnStatus.Name = "pnlBtnStatus"
        Me.pnlBtnStatus.Size = New System.Drawing.Size(217, 34)
        Me.pnlBtnStatus.TabIndex = 8
        Me.pnlBtnStatus.Visible = False
        '
        'pnlBsDownstrum
        '
        Me.pnlBsDownstrum.BackColor = System.Drawing.SystemColors.Control
        Me.pnlBsDownstrum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBsDownstrum.Location = New System.Drawing.Point(174, 19)
        Me.pnlBsDownstrum.Name = "pnlBsDownstrum"
        Me.pnlBsDownstrum.Size = New System.Drawing.Size(28, 10)
        Me.pnlBsDownstrum.TabIndex = 5
        '
        'pnlBsUpstrum
        '
        Me.pnlBsUpstrum.BackColor = System.Drawing.SystemColors.Control
        Me.pnlBsUpstrum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBsUpstrum.Location = New System.Drawing.Point(174, 3)
        Me.pnlBsUpstrum.Name = "pnlBsUpstrum"
        Me.pnlBsUpstrum.Size = New System.Drawing.Size(28, 10)
        Me.pnlBsUpstrum.TabIndex = 4
        '
        'pnlBsOrange
        '
        Me.pnlBsOrange.BackColor = System.Drawing.SystemColors.Control
        Me.pnlBsOrange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBsOrange.Location = New System.Drawing.Point(140, 3)
        Me.pnlBsOrange.Name = "pnlBsOrange"
        Me.pnlBsOrange.Size = New System.Drawing.Size(28, 28)
        Me.pnlBsOrange.TabIndex = 3
        '
        'pnlBsBlue
        '
        Me.pnlBsBlue.BackColor = System.Drawing.SystemColors.Control
        Me.pnlBsBlue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBsBlue.Location = New System.Drawing.Point(106, 3)
        Me.pnlBsBlue.Name = "pnlBsBlue"
        Me.pnlBsBlue.Size = New System.Drawing.Size(28, 28)
        Me.pnlBsBlue.TabIndex = 2
        '
        'pnlBsYellow
        '
        Me.pnlBsYellow.BackColor = System.Drawing.SystemColors.Control
        Me.pnlBsYellow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBsYellow.Location = New System.Drawing.Point(72, 3)
        Me.pnlBsYellow.Name = "pnlBsYellow"
        Me.pnlBsYellow.Size = New System.Drawing.Size(28, 28)
        Me.pnlBsYellow.TabIndex = 1
        '
        'pnlBsRed
        '
        Me.pnlBsRed.BackColor = System.Drawing.SystemColors.Control
        Me.pnlBsRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBsRed.Location = New System.Drawing.Point(38, 3)
        Me.pnlBsRed.Name = "pnlBsRed"
        Me.pnlBsRed.Size = New System.Drawing.Size(28, 28)
        Me.pnlBsRed.TabIndex = 1
        '
        'pnlBsGreen
        '
        Me.pnlBsGreen.BackColor = System.Drawing.SystemColors.Control
        Me.pnlBsGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBsGreen.Location = New System.Drawing.Point(4, 3)
        Me.pnlBsGreen.Name = "pnlBsGreen"
        Me.pnlBsGreen.Size = New System.Drawing.Size(28, 28)
        Me.pnlBsGreen.TabIndex = 0
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.DefaultExt = "bot"
        Me.OpenFileDialog.Filter = "RB Bot Chart|*.bot"
        Me.OpenFileDialog.Title = "Select Chart File"
        '
        'botTimer
        '
        Me.botTimer.Interval = 1
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TippyToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(553, 24)
        Me.MenuStrip1.TabIndex = 9
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'TippyToolStripMenuItem
        '
        Me.TippyToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConsoleModeToolStripMenuItem, Me.ToolStripMenuItem1, Me.ChangelogToolStripMenuItem})
        Me.TippyToolStripMenuItem.Name = "TippyToolStripMenuItem"
        Me.TippyToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.TippyToolStripMenuItem.Text = "1337"
        '
        'ConsoleModeToolStripMenuItem
        '
        Me.ConsoleModeToolStripMenuItem.CheckOnClick = True
        Me.ConsoleModeToolStripMenuItem.Name = "ConsoleModeToolStripMenuItem"
        Me.ConsoleModeToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.ConsoleModeToolStripMenuItem.Text = "Open Console"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(157, 6)
        '
        'ChangelogToolStripMenuItem
        '
        Me.ChangelogToolStripMenuItem.Name = "ChangelogToolStripMenuItem"
        Me.ChangelogToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.ChangelogToolStripMenuItem.Text = "View Changelog"
        '
        'mainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(553, 272)
        Me.Controls.Add(Me.pnlBtnStatus)
        Me.Controls.Add(Me.pnlBotControl)
        Me.Controls.Add(Me.btnBotAction)
        Me.Controls.Add(Me.btnLoadChart)
        Me.Controls.Add(Me.btnAddChart)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.txtConsole)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "mainForm"
        Me.Text = "Knoxy RB Bot Controller"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.pnlBotControl.ResumeLayout(False)
        Me.pnlBtnStatus.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtConsole As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents cbPorts As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents loginTimer As System.Windows.Forms.Timer
    Friend WithEvents lblAuthed As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents serialReadTimer As System.Windows.Forms.Timer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblBotChart As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblBotStatus As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblBotTime As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblBotNotes As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnAddChart As System.Windows.Forms.Button
    Friend WithEvents btnLoadChart As System.Windows.Forms.Button
    Friend WithEvents btnBotAction As System.Windows.Forms.Button
    Friend WithEvents pnlBotControl As System.Windows.Forms.Panel
    Friend WithEvents btnGreen As System.Windows.Forms.Button
    Friend WithEvents btnDown As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents btnYellow As System.Windows.Forms.Button
    Friend WithEvents btnRed As System.Windows.Forms.Button
    Friend WithEvents pnlBtnStatus As System.Windows.Forms.Panel
    Friend WithEvents pnlBsGreen As System.Windows.Forms.Panel
    Friend WithEvents pnlBsRed As System.Windows.Forms.Panel
    Friend WithEvents pnlBsOrange As System.Windows.Forms.Panel
    Friend WithEvents pnlBsBlue As System.Windows.Forms.Panel
    Friend WithEvents pnlBsYellow As System.Windows.Forms.Panel
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents llblShutdown As System.Windows.Forms.LinkLabel
    Friend WithEvents botTimer As System.Windows.Forms.Timer
    Friend WithEvents pnlBsDownstrum As System.Windows.Forms.Panel
    Friend WithEvents pnlBsUpstrum As System.Windows.Forms.Panel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents TippyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConsoleModeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ChangelogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
