Imports System.Windows.Forms
Imports System.IO

Public Class dlgAddChart

    Dim write1, write2, write3, write4 As Boolean
    Dim chartReader As New StreamReader(mainForm.addChartFile)
    Dim chartTitle As String
    Dim chartLines As Integer
    Dim linesWritten As Integer
    Dim writeProgress As Integer


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        writeTimer.Enabled = True
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgAddChart_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim notesRead As Integer = 0
        Dim lineReader As New StreamReader(mainForm.addChartFile)
        For line As Integer = 1 To 3
            Dim thisLine As String = lineReader.ReadLine()
            If line = 2 Then
                Dim parts As String() = thisLine.Split(New Char() {" "c})
                lblAddVersion.Text = parts(2)
            End If
            If line = 3 Then
                chartTitle = thisLine + ".bot"
                Dim parts As String() = thisLine.Split(New Char() {"_"c})
                lblAddChart.Text = parts(0)
            End If
        Next
        Do While lineReader.Peek() >= 0
            Dim noteLine As String = lineReader.ReadLine()
            If noteLine <> "" Then
                notesRead += 1
            End If
        Loop
        chartLines = notesRead + 3
        lblAddNotes.Text = notesRead.ToString
        linesWritten = 0

    End Sub

    Private Sub writeTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles writeTimer.Tick
        If write1 = False Then
            mainForm.serialPort.WriteLine("cd ~/bot/wincon/addchart && cat > " + chartTitle)
            write1 = True
        End If

        If write1 = True And write2 = False Then
            writeTimer.Interval = 10
            If chartReader.Peek() >= 0 Then
                Dim thisChartLine As String = chartReader.ReadLine()
                mainForm.serialPort.WriteLine(thisChartLine)

                'update the progress bar
                linesWritten += 1
                writeProgress = (linesWritten / chartLines) * 100
                ProgressBar1.Value = writeProgress
            Else
                write2 = True
                writeTimer.Interval = 100
            End If
        End If

        If write2 = True And write3 = False Then
            mainForm.serialPort.Write(New Byte() {4}, 0, 1)
            write3 = True
        End If

        If write3 = True And write4 = False Then
            mainForm.serialPort.WriteLine("mv ./" + chartTitle + " ~/bot/charts/" + chartTitle)
            mainForm.txtConsole.AppendText(vbNewLine + "Successfully added new chart: " + chartTitle + vbNewLine)
            OK_Button.Enabled = True
            Cancel_Button.Enabled = True
            write3 = False
            write2 = False
            write1 = False
            writeProgress = 0
            ProgressBar1.Value = 0
            writeTimer.Enabled = False
            Me.Close()
            Me.Dispose()
        End If
    End Sub
End Class
