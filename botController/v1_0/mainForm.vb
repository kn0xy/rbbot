Imports System.IO.Ports
Imports System.Threading

Public Class mainForm

    Public WithEvents serialPort As SerialPort

    'Globals
    Dim usernameEntered As Boolean = False
    Dim loggedIn As Boolean = False
    Dim botReady As Boolean = False
    Dim botInitialized As Boolean = False
    Dim botStartTime As DateTime
    Dim timeElapsed As TimeSpan
    Public loadingCharts As Boolean = False
    Public loadingNotes As Boolean = False
    Public loadedChart As String
    Dim chartNotes As Integer
    Public addChartFile As String
    Dim shuttingDown As Boolean = False

    'Strum release timers
    Dim uPress As Integer = 0
    Dim dPress As Integer = 0

    'User control button toggles
    Dim userControlButton As Boolean = False
    Dim ucbGreen As Boolean = False
    Dim ucbRed As Boolean = False
    Dim ucbYellow As Boolean = False
    Dim ucbDown As Boolean = False
    Dim ucbUp As Boolean = False


    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If loggedIn = True Then
            'detect up arrow key
            If keyData = Keys.Up Then
                If botTimer.Enabled = False Then
                    txtConsole.AppendText("Up-strum...")
                    serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn U press")
                    ucbUp = True
                End If
                Return True
            End If

            'detect down arrow key
            If keyData = Keys.Down Then
                If botTimer.Enabled = False Then
                    txtConsole.AppendText("Down-strum...")
                    serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn D press")
                    ucbDown = True
                End If

                Return True
            End If

            'detect enter key
            If keyData = Keys.Enter Then
                If botTimer.Enabled = False Then
                    If botReady = False Then
                        txtConsole.AppendText("Pressing Green...")
                        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn G press")
                        ucbGreen = True
                    Else
                        startBot()
                    End If
                End If
                Return True
            End If

            'detect B key
            If keyData = Keys.B Then
                If botTimer.Enabled = False Then
                    txtConsole.AppendText("Pressing Red...")
                    serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn R press")
                    ucbRed = True
                End If
                Return True
            End If

            'detect Y key
            If keyData = Keys.Y Then
                If botTimer.Enabled = False Then
                    txtConsole.AppendText("Pressing Yellow...")
                    serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn Y press")
                    ucbYellow = True
                End If
                Return True
            End If
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub btnGreen_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnGreen.MouseDown
        txtConsole.AppendText("Pressing Green...")
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn G press")
    End Sub

    Private Sub btnGreen_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnGreen.MouseUp
        txtConsole.AppendText("Released!" + vbNewLine)
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn G release")
    End Sub

    Private Sub btnRed_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnRed.MouseDown
        txtConsole.AppendText("Pressing Red...")
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn R press")
    End Sub

    Private Sub btnRed_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnRed.MouseUp
        txtConsole.AppendText("Released!" + vbNewLine)
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn R release")
    End Sub

    Private Sub btnYellow_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnYellow.MouseDown
        txtConsole.AppendText("Pressing Yellow...")
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn Y press")
    End Sub

    Private Sub btnYellow_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnYellow.MouseUp
        txtConsole.AppendText("Released!" + vbNewLine)
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn Y release")
    End Sub

    Private Sub btnUp_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnUp.MouseDown
        txtConsole.AppendText("Up-strum...")
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn U press")
    End Sub

    Private Sub btnUp_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnUp.MouseUp
        txtConsole.AppendText("Released!" + vbNewLine)
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn U release")
    End Sub

    Private Sub btnDown_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnDown.MouseDown
        txtConsole.AppendText("Down-strum...")
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn D press")
    End Sub

    Private Sub btnDown_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnDown.MouseUp
        txtConsole.AppendText("Released!" + vbNewLine)
        serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn D release")
    End Sub

    Private Sub startBot()
        serialPort.WriteLine("")
        botStartTime = Now
        lblBotStatus.Text = "Playing"
        txtConsole.AppendText("Bot started!" + vbNewLine)
        botReady = False
        botTimer.Enabled = True
    End Sub

    Private Sub mainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Obtain available ports and update drop-down menu
        Dim ports As String() = System.IO.Ports.SerialPort.GetPortNames()
        cbPorts.Items.Clear()
        For Each port In ports
            cbPorts.Items.Add(port)
        Next
    End Sub


    Private Sub cbPorts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPorts.SelectedIndexChanged
        btnConnect.Enabled = True
    End Sub

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        serialPort = New SerialPort(cbPorts.SelectedItem.ToString, 115200, Parity.None, 8, 1)
        serialPort.Open()

        If (serialPort.IsOpen) Then
            txtConsole.Text = "Connected...waiting for authentication." + vbNewLine
            btnConnect.Enabled = False
            cbPorts.Enabled = False
            serialReadTimer.Enabled = True
            serialPort.WriteLine("")
            If Me.loggedIn = True Then
                loggedInUpdateUI()
            End If
        End If
    End Sub

    Private Sub loggedInUpdateUI()
        lblAuthed.Text = "Yes"
        lblAuthed.ForeColor = Color.Green
        lblBotStatus.Text = "Idle"
        lblBotChart.Text = "None"
        btnAddChart.Enabled = True
        btnLoadChart.Enabled = True
        pnlBotControl.Visible = True
        botStartTime = Now
        txtConsole.AppendText("Authenticated!" + vbNewLine)
        llblShutdown.Visible = True
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles loginTimer.Tick
        lblAuthed.Text = "..."

        If usernameEntered = False Then
            serialPort.WriteLine("pi")
            usernameEntered = True
        Else
            serialPort.WriteLine("raspberry")
            loggedIn = True
        End If

        If loggedIn = True Then
            loggedInUpdateUI()
            loginTimer.Enabled = False
        End If
    End Sub

    Private Sub serialReadTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles serialReadTimer.Tick


        If Me.loggedIn = False Then
            Dim indata As String = serialPort.ReadExisting()
            If indata.Contains("login") Then
                If loginTimer.Enabled = False Then
                    loginTimer.Enabled = True
                End If
            End If

            If indata.Contains("pi@raspberrypi") Then
                Me.loggedIn = True
                loggedInUpdateUI()
            End If
        End If

        If loggedIn = True Then
            timeElapsed = Now().Subtract(botStartTime)
            lblBotTime.Text = String.Format("{0:00}:{1:00}:{2:00}", CInt(timeElapsed.Hours), CInt(timeElapsed.Minutes), CInt(timeElapsed.Seconds))
        Else
            txtConsole.Text = "Connected...waiting for authentication." + vbNewLine
        End If

        'handle chart listing
        If loadingCharts = True Then
            Dim indata As String = serialPort.ReadExisting()
            Dim parts As String() = indata.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
            Dim chartLine As String
            For Each chartLine In parts
                If chartLine.Contains("pi@raspberrypi:~/bot/charts") Then
                    loadingCharts = False
                Else
                    If chartLine.Contains("-r") Then
                        'split string by spaces
                        Dim chartparts As String() = chartLine.Split(New Char() {" "c})
                        For Each cp As String In chartparts
                            If cp.Contains("guitar_expert") Then
                                dlgLoadChart.ListBox1.Items.Add(cp)
                            End If
                        Next
                    End If
                End If
            Next
        End If

        'handle chart selection
        If loadingNotes = True Then
            Dim indata As String = serialPort.ReadExisting()
            Dim parts As String() = indata.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
            Dim chartVersion As String = "# VERSION 1.4"
            For Each outline As String In parts
                If outline.Contains("pi@raspberrypi:~/bot/charts") Then
                    loadingNotes = False
                    Exit For
                Else
                    If outline.Contains("# VERSION") Then
                        If outline.Contains("# VERSION 1.4") Then
                            'do nothing
                        Else
                            MessageBox.Show("Chart version is out-of-date!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End If
                    Try
                        Dim numLines As Integer = CInt(outline)
                        chartNotes = numLines - 4
                        lblBotNotes.Text = chartNotes.ToString
                        loadingNotes = False
                        Exit For
                    Catch ex As Exception
                    End Try
                End If
            Next
        End If

        'handle shutdown
        If shuttingDown = True Then
            Dim indata As String = serialPort.ReadExisting()
            txtConsole.AppendText(indata)
            lblAuthed.Text = "No"
            lblAuthed.ForeColor = Color.Red
            lblBotStatus.Text = "N/A"
            lblBotChart.Text = "N/A"
            lblBotTime.Text = "N/A"
            btnAddChart.Enabled = False
            btnLoadChart.Enabled = False
            btnBotAction.Enabled = False
            pnlBotControl.Visible = False
        End If

        'handle bot initialization
        If botReady = True Then
            If botInitialized = False Then
                Dim command As String = "cd ~/bot/wincon && sudo ./wincon " + loadedChart
                serialPort.WriteLine(command)
                botInitialized = True
            Else
                Dim indata As String = serialPort.ReadExisting()
                If indata.Contains("cd ~/bot") Then
                    'do nothing
                Else
                    txtConsole.AppendText(indata)
                End If
            End If
        End If

        'handle user control button releases
        If ucbGreen = True Then
            txtConsole.AppendText("Released!" + vbNewLine)
            serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn G release")
            ucbGreen = False
        End If
        If ucbRed = True Then
            txtConsole.AppendText("Released!" + vbNewLine)
            serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn R release")
            ucbRed = False
        End If
        If ucbYellow = True Then
            txtConsole.AppendText("Released!" + vbNewLine)
            serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn Y release")
            ucbYellow = False
        End If
        If ucbDown = True Then
            txtConsole.AppendText("Released!" + vbNewLine)
            serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn D release")
            ucbDown = False
        End If
        If ucbUp = True Then
            txtConsole.AppendText("Released!" + vbNewLine)
            serialPort.WriteLine("cd ~/bot/wincon && sudo ./gtrBtn U release")
            ucbUp = False
        End If


    End Sub

    Private Sub btnLoadChart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadChart.Click
        If dlgLoadChart.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'obtain chartfile version and number of notes
            serialPort.WriteLine("head -2 " + loadedChart + " | tail -1 && cat ./" + loadedChart + " | wc -l")
            loadingNotes = True
            'enable bot start
            btnBotAction.Enabled = True
        Else
            btnBotAction.Enabled = False
            lblBotChart.Text = "None"
            lblBotNotes.Text = "N/A"
            loadedChart = vbNullString
        End If
    End Sub

    Private Sub btnAddChart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddChart.Click
        If OpenFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            dlgAddChart.ShowDialog()
        End If
    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog.FileOk
        addChartFile = OpenFileDialog.FileName
    End Sub

    Private Sub llblShutdown_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llblShutdown.LinkClicked
        llblShutdown.Enabled = False
        txtConsole.AppendText(vbNewLine + vbNewLine + "Shutting down Raspberry Pi..." + vbNewLine)
        serialPort.WriteLine("sudo shutdown -h now")
        shuttingDown = True
    End Sub

    Private Sub btnBotAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBotAction.Click
        botStartTime = Now
        If btnBotAction.Text = "Start Bot" Then
            btnAddChart.Enabled = False
            btnLoadChart.Enabled = False
            pnlBotControl.Visible = False
            llblShutdown.Enabled = False
            lblBotStatus.Text = "Ready"
            botReady = True
            btnBotAction.Text = "Stop Bot"
            pnlBtnStatus.Visible = True
        Else
            serialPort.Write(New Byte() {3}, 0, 1)
            stopBot()
            txtConsole.AppendText(vbNewLine + "Bot stopped!" + vbNewLine)
        End If

    End Sub

    Private Sub stopBot()
        btnAddChart.Enabled = True
        btnLoadChart.Enabled = True
        pnlBotControl.Visible = True
        llblShutdown.Enabled = True
        lblBotStatus.Text = "Idle"
        lblBotChart.Text = "None"
        lblBotNotes.Text = "N/A"
        btnBotAction.Text = "Start Bot"
        btnBotAction.Enabled = False
        botReady = False
        botInitialized = False
        botTimer.Enabled = False
        For Each ctrl As Control In pnlBtnStatus.Controls
            ctrl.BackColor = Control.DefaultBackColor
        Next
        pnlBtnStatus.Visible = False
        botStartTime = Now
    End Sub

    Delegate Sub botColorButtons(ByVal color As String, ByVal time As Integer, ByVal notes As String)
    Public Sub setColorButtons(ByVal color As String, ByVal time As Integer, ByVal notes As String)
        Dim releaseButton As New botBtn
        Dim releaseThread As New Thread(AddressOf releaseButton.Press)
        releaseButton.Button = color
        releaseButton.Time = time
        releaseButton.sender = Me

        lblBotNotes.Text = notes + " / " + chartNotes.ToString

        If color = "G" Then
            pnlBsGreen.BackColor = Drawing.Color.Green
        End If
        If color = "R" Then
            pnlBsRed.BackColor = Drawing.Color.Red
        End If
        If color = "Y" Then
            pnlBsYellow.BackColor = Drawing.Color.Yellow
        End If
        If color = "B" Then
            pnlBsBlue.BackColor = Drawing.Color.Blue
        End If
        If color = "O" Then
            pnlBsOrange.BackColor = Drawing.Color.Orange
        End If

        releaseThread.Start()
    End Sub

    Delegate Sub botBtnRelease(ByVal button As String)
    Public Sub ReleaseButton(ByVal button As String)
        If button = "G" Then
            pnlBsGreen.BackColor = Control.DefaultBackColor
        End If
        If button = "R" Then
            pnlBsRed.BackColor = Control.DefaultBackColor
        End If
        If button = "Y" Then
            pnlBsYellow.BackColor = Control.DefaultBackColor
        End If
        If button = "B" Then
            pnlBsBlue.BackColor = Control.DefaultBackColor
        End If
        If button = "O" Then
            pnlBsOrange.BackColor = Control.DefaultBackColor
        End If
    End Sub

    Private Class botBtn
        Public sender As mainForm
        Public Button As String
        Public Time As Integer
        Public Sub Press()
            Thread.Sleep(Time)
            sender.pnlBtnStatus.Invoke(New botBtnRelease(AddressOf sender.ReleaseButton), New Object() {Button})
        End Sub
    End Class

    Delegate Sub botStrumButtons(ByVal updown As String)
    Public Sub setStrumButton(ByVal updown As String)
        If updown = "D" Then
            pnlBsDownstrum.BackColor = Drawing.Color.DarkGray
            dPress = 10
        Else
            pnlBsUpstrum.BackColor = Drawing.Color.DarkGray
            uPress = 10
        End If
    End Sub

    Delegate Sub dBotFinished(ByVal endline As String)
    Public Sub botFinished(ByVal endline As String)
        stopBot()
        txtConsole.AppendText(vbNewLine + endline + vbNewLine)
    End Sub

    Private Sub botTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles botTimer.Tick


        'downstrum
        If dPress > 0 Then
            dPress = dPress - 1
        Else
            pnlBsDownstrum.BackColor = Control.DefaultBackColor
        End If

        'upstrum
        If uPress > 0 Then
            uPress = uPress - 1
        Else
            pnlBsUpstrum.BackColor = Control.DefaultBackColor
        End If

    End Sub

    Private Sub serialPort_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles serialPort.DataReceived
        If botTimer.Enabled = True Then
            While serialPort.BytesToRead > 0
                Dim line As String = serialPort.ReadLine()
                If line.Contains("Bot finished") Then
                    txtConsole.Invoke(New dBotFinished(AddressOf botFinished), New Object() {line})
                Else
                    Try
                        Dim lineParts() As String = line.Split("-"c)
                        pnlBtnStatus.Invoke(New botColorButtons(AddressOf setColorButtons), New Object() {lineParts(3), CInt(lineParts(4)), lineParts(0)})
                        If lineParts(2) <> "X" Then
                            pnlBtnStatus.Invoke(New botStrumButtons(AddressOf setStrumButton), New Object() {lineParts(2)})
                        End If
                    Catch ex As Exception
                    End Try

                End If
            End While
        End If
    End Sub

End Class
