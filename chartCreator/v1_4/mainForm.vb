Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Public Class mainForm

    Public chartImage As Image
    Public chartRows As Integer = 0
    Public chartData As New ArrayList
    Public hasSaved As Boolean = False
    Public scanRunning As Boolean = False
    Public bpmMarkers(0) As bpmMarker
    Public numBeats As Integer = 0
    Public beatRows(0) As Integer
    Public beatMeasures(0) As Integer
    Public builtMeasures(0) As String
    Public songLength As Integer = 0
    Public bpmChecks(11, 5, 8) As Color

    Private Sub mainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If scanRunning = True Then
            If MessageBox.Show("Closing now will cancel the current operation.", "Cancel Scan?", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) = Windows.Forms.DialogResult.OK Then
                e.Cancel = False
            Else
                e.Cancel = True
            End If
            Exit Sub
        End If
        Dim dataCount As Integer
        Try
            dataCount = chartData.Count
        Catch ex As Exception
            dataCount = 0
        End Try
        If dataCount > 0 Then
            If hasSaved = False Then
                If MessageBox.Show("You have unsaved data that will be lost if you exit!", "Really Quit?", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) = Windows.Forms.DialogResult.OK Then
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnOpenImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenImg.Click
        Dim dialog As New OpenFileDialog()
        dialog.Filter = "png charts|*.png"

        If dialog.ShowDialog() <> DialogResult.OK Then
            Return
        Else
            Scanner.loadImage(dialog.FileName, dialog.SafeFileName)
            Dim fn(1) As String
            fn(0) = dialog.SafeFileName
            chartData.Capacity = 2
            chartData.Insert(0, fn)
        End If


    End Sub

    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        scanRunning = True
        Scanner.scanMeasures()
    End Sub

    Private Sub OpenChartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenChartToolStripMenuItem.Click
        Dim dialog As New OpenFileDialog()
        dialog.Filter = "png charts|*.png"

        If dialog.ShowDialog() <> DialogResult.OK Then
            Return
        Else
            Scanner.loadImage(dialog.FileName, dialog.SafeFileName)
            Dim fn(1) As String
            fn(0) = dialog.SafeFileName
            chartData.Capacity = 2
            chartData.Insert(0, fn)
        End If
    End Sub

    Private Sub CloseChartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseChartToolStripMenuItem.Click
        Application.Restart()

    End Sub

    Private Function cancelScan()
        Return False

    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Localize data to current scope
        Dim cna() As String = chartData(0)
        Dim chartName As String = cna(0)
        Dim chartTitle As String = chartName.Replace("_blank.png", "")
        Dim notes() As String = chartData(1)

        'Prepare data to be written
        Dim sb As New StringBuilder()
        sb.AppendLine("# KNOXY RB BOT CHART")
        sb.AppendLine("# VERSION 1.4")
        sb.AppendLine(chartTitle)
        For n = 0 To notes.GetUpperBound(0)
            sb.AppendLine(notes(n))
        Next

        ' Save file
        Dim sfd As New System.Windows.Forms.SaveFileDialog
        Dim filename As String = chartTitle + ".bot"
        sfd.Title = "Save Bot Chart"
        sfd.FileName = filename
        sfd.Filter = "bot file (*.bot)|*.bot"
        If sfd.ShowDialog() = DialogResult.OK Then
            Using outfile As New StreamWriter(sfd.OpenFile())
                outfile.Write(sb.ToString())
            End Using

            'Notify user and update internal status
            MessageBox.Show("Chart data file saved successfully!", "Success")
            hasSaved = True

        End If


    End Sub


    Private Sub btnEnterBpm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnterBpm.Click
        If bpmDlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            btnViewChart.Enabled = True
        Else
            btnViewChart.Enabled = False
        End If
    End Sub

    Private Sub btnViewChart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewChart.Click
        chartDlg.Show()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub

    Private Sub TimingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimingToolStripMenuItem.Click
        timingDlg.ShowDialog()
    End Sub

    Private Sub ChangeLogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeLogToolStripMenuItem.Click
        changesDlg.ShowDialog()
    End Sub

    Private Sub initBpmChecks()
        For xBpm As Integer = 0 To 4
            For yBpm As Integer = 0 To 7
                bpmChecks(0, xBpm, yBpm) = My.Resources.bmp_0.GetPixel(xBpm, yBpm)
                bpmChecks(1, xBpm, yBpm) = My.Resources.bmp_1.GetPixel(xBpm, yBpm)
                bpmChecks(2, xBpm, yBpm) = My.Resources.bmp_2.GetPixel(xBpm, yBpm)
                bpmChecks(3, xBpm, yBpm) = My.Resources.bmp_3.GetPixel(xBpm, yBpm)
                bpmChecks(4, xBpm, yBpm) = My.Resources.bmp_4.GetPixel(xBpm, yBpm)
                bpmChecks(5, xBpm, yBpm) = My.Resources.bmp_5.GetPixel(xBpm, yBpm)
                bpmChecks(6, xBpm, yBpm) = My.Resources.bmp_6.GetPixel(xBpm, yBpm)
                bpmChecks(7, xBpm, yBpm) = My.Resources.bmp_7.GetPixel(xBpm, yBpm)
                bpmChecks(8, xBpm, yBpm) = My.Resources.bmp_8.GetPixel(xBpm, yBpm)
                bpmChecks(9, xBpm, yBpm) = My.Resources.bmp_9.GetPixel(xBpm, yBpm)
                bpmChecks(10, xBpm, yBpm) = My.Resources.bmp_pt.GetPixel(xBpm, yBpm)
            Next
        Next

    End Sub

    Private Sub mainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initBpmChecks()

    End Sub
End Class
