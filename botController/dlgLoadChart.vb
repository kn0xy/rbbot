Imports System.Windows.Forms
Imports System.IO.Ports
Imports System.Threading

Public Class dlgLoadChart

    Dim loadingCharts As Boolean = False
    Dim loadingNotes As Boolean = False
    Dim lcfThread As New Threading.Thread(AddressOf loadChartFiles)
    Dim sp As SerialPort
    Dim chartsLoaded As Integer
    Dim notesLoaded As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim si As String = ListBox1.SelectedItem
        Dim theChart As String = si.Replace(vbCr, "").Replace(vbLf, "")
        Dim sip As String() = theChart.Split(New Char() {"_"c})
        Dim chartName As String = sip(0)
        mainForm.loadedChart = theChart
        mainForm.lblBotChart.Text = chartName
        mainForm.txtConsole.AppendText(vbNewLine + "Loaded chart file: " + theChart + vbNewLine + vbNewLine)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgLoadChart_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub dlgLoadChart_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'stock the list box
        ListBox1.Items.Clear()
        sp = mainForm.serialPort
        loadingCharts = True
        chartsLoaded = 0
        Try
            lcfThread.Start()
        Catch ex As Exception
            MessageBox.Show("lcfThread already started")
        End Try

        mainForm.serialPort.WriteLine("cd ~/bot/charts && ls -l --color=never")
        mainForm.serialPort.WriteLine("")


    End Sub

    Private Sub addChartToList(ByVal chartFile As String)
        If ListBox1.InvokeRequired Then
            Dim d As New delegateAddChartToList(AddressOf addChartToList)
            Me.Invoke(d, New Object() {chartFile})
        Else
            ListBox1.Items.Add(chartFile)
        End If
    End Sub

    Delegate Sub delegateAddChartToList(ByVal cf As String)

    Private Sub finishedLoadingCharts()
        loadingCharts = False
        chartsLoaded = 0
    End Sub

    Private Sub loadChartFiles()
        While loadingCharts
            Try
                Dim outline As String = sp.ReadLine()
                If outline.Contains("pi@raspberrypi:~/bot/charts$") And chartsLoaded > 0 Then
                    loadingCharts = False
                    finishedLoadingCharts()
                    Exit Sub
                Else
                    If outline.Contains("-r") Then
                        'split string by spaces
                        Dim chartparts As String() = outline.Split(New Char() {" "c})
                        For Each cp As String In chartparts
                            If cp.Contains("guitar_expert") Or cp.Contains("bass_expert") Then
                                addChartToList(cp)
                                chartsLoaded = chartsLoaded + 1
                            End If
                        Next
                    End If
                End If


            Catch generatedExceptionName As TimeoutException
                MessageBox.Show("Timed out")
                Exit Sub

            Catch ioe As System.IO.IOException
                Me.dlgLoadChart_Load(Me, New System.EventArgs)
                Exit Sub

            End Try
        End While
    End Sub

    Private Sub loadChartNotes()
        While loadingNotes
            Try
                Dim outline As String = sp.ReadLine()
                If outline.Contains("pi@raspberrypi:~/bot/charts$") And notesLoaded > 0 Then
                    loadingCharts = False
                    finishedLoadingCharts()
                    Exit Sub
                Else
                    If outline.Contains("-r") Then
                        'split string by spaces
                        Dim chartparts As String() = outline.Split(New Char() {" "c})
                        For Each cp As String In chartparts
                            If cp.Contains("guitar_expert") Or cp.Contains("bass_expert") Then
                                addChartToList(cp)
                                notesLoaded = notesLoaded + 1
                            End If
                        Next
                    End If
                End If


            Catch generatedExceptionName As TimeoutException
                Exit Sub

            Catch ioe As System.IO.IOException
                Exit Sub

            End Try
        End While
    End Sub


    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        OK_Button.Enabled = True
    End Sub
End Class
