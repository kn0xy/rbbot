Imports System.Windows.Forms


Public Class chartDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub chartDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Build the chart
        ChartBuilder.Build(mainForm.chartData(1))

        'Sort the notes by px value and measure
        Column3.ValueType = GetType(Integer)
        Column3.SortMode = DataGridViewColumnSortMode.Automatic
        Column3.SortMode = DataGridViewColumnSortMode.NotSortable

        'Calculate chart data
        Dim totalTime As Integer = mainForm.songLength / 1000
        Dim min As Integer = totalTime / 60
        Dim sec As Integer = totalTime Mod 60
        Dim slStr As String = DataGridView1.Rows.Count.ToString + " notes  (" + min.ToString + "m " + sec.ToString + "s)"
        lblSongLength.Text = slStr
    End Sub
End Class
