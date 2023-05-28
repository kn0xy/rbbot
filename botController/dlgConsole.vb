Imports System.Windows.Forms
Imports System.IO.Ports
Imports System.Threading

Public Class dlgConsole
    Dim sp As SerialPort
    Dim _continue As Boolean = False
    Dim readThread As New Threading.Thread(AddressOf Read)
    Dim message As String
    Dim sc As StringComparer = StringComparer.OrdinalIgnoreCase

    Private Sub dlgConsole_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        _continue = False
        sp.Close()
        mainForm.txtConsole.AppendText("Console closed!" + vbNewLine + vbNewLine)
        mainForm.ConsoleModeToolStripMenuItem.Checked = False
        Me.Dispose()
    End Sub

    Private Sub dlgConsole_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtMyConsole.AppendText("Opening serial port..." + vbNewLine)
        sp = New SerialPort("COM3", 115200, Parity.None, 8, 1)
        Try
            sp.Open()
            _continue = True
            readThread.Start()
            txtMyConsole.AppendText("Type 'quit' to send ^C" + vbNewLine)
            sp.WriteLine("")
            sp.WriteLine("")
        Catch ex As Exception
            txtMyConsole.AppendText("Serial port in use! Closing other instance(s)..." + vbNewLine + vbNewLine)
            mainForm.serialPort.Close()
            mainForm.lblAuthed.Text = "No"
            mainForm.lblAuthed.ForeColor = Color.Red
            mainForm.cbPorts.Enabled = True
            mainForm.btnConnect.Enabled = True
            mainForm.btnAddChart.Enabled = False
            mainForm.btnLoadChart.Enabled = False
            mainForm.pnlBotControl.Visible = False
            mainForm.serialReadTimer.Enabled = False
            mainForm.llblShutdown.Visible = False
            mainForm.btnBotAction.Enabled = False
            mainForm.lblBotChart.Text = "N/A"
            mainForm.lblBotNotes.Text = "N/A"
            mainForm.lblBotStatus.Text = "N/A"
            mainForm.lblBotTime.Text = "N/A"
            dlgConsole_Load(sender, e)
        End Try
        
    End Sub

    Public Sub postMessage(ByVal msg As String)
        If txtMyConsole.InvokeRequired Then
            Dim d As New pmDel(AddressOf postMessage)
            Me.Invoke(d, New Object() {msg})
        Else
            txtMyConsole.AppendText(msg + vbNewLine)
        End If
    End Sub

    Delegate Sub pmDel(ByVal text As String)


    Public Sub Read()

        While _continue
            Try
                Dim message As String = sp.ReadLine()

                postMessage(message)
            Catch generatedExceptionName As TimeoutException
                Exit Sub

            Catch ioe As System.IO.IOException
                Exit Sub

            End Try
        End While

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim msg As String = TextBox1.Text
        If msg = "quit" Then
            sp.Write(New Byte() {3}, 0, 1)
        Else
            sp.WriteLine(msg)
        End If

        TextBox1.Text = ""


    End Sub

    Private Sub OK_Button_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.Close()
    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyData = Keys.Enter Then
            Button1_Click(sender, e)
        End If
    End Sub
End Class
