Imports System.Windows.Forms
Imports System.IO.Ports

Public Class dlgLoadChart

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim si As String = ListBox1.SelectedItem
        Dim sip As String() = si.Split(New Char() {"_"c})
        Dim chartName As String = sip(0)
        mainForm.loadedChart = si
        mainForm.lblBotChart.Text = chartName
        mainForm.txtConsole.AppendText(vbNewLine + "Loaded chart file: " + si + vbNewLine + vbNewLine)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgLoadChart_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'stock the list box
        ListBox1.Items.Clear()
        mainForm.loadingCharts = True
        mainForm.serialPort.WriteLine("cd ~/bot/charts && ls -l --color=never")


    End Sub


    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        OK_Button.Enabled = True
    End Sub
End Class
