Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Public Class mainForm

    Public chartImage As Image
    Public chartData As New ArrayList
    Public hasSaved As Boolean = False
    Public scanRunning As Boolean = False

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
            Functions.loadImage(dialog.FileName, dialog.SafeFileName)
            Dim fn(1) As String
            fn(0) = dialog.SafeFileName
            chartData.Capacity = 3
            chartData.Insert(0, fn)
        End If


    End Sub

    Private Sub DebugToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DebugToolStripMenuItem.Click
        debugForm.Show()
    End Sub

    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        scanRunning = True
        Functions.scanRows()
    End Sub

    Private Sub OpenChartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenChartToolStripMenuItem.Click
        Dim dialog As New OpenFileDialog()
        dialog.Filter = "png charts|*.png"

        If dialog.ShowDialog() <> DialogResult.OK Then
            Return
        Else
            Functions.loadImage(dialog.FileName, dialog.SafeFileName)
            Dim fn(1) As String
            fn(0) = dialog.SafeFileName
            chartData.Capacity = 3
            chartData.Insert(0, fn)
        End If
    End Sub

    Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        debugForm2.Show()

    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click

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
        Dim notes() As String = chartData(1)
        Dim nma() As Integer = chartData(2)
        Dim nMeasures As Integer = nma(0)
        Dim nNotes As Integer = nma(1)

        'Prepare data to be written
        Dim sb As New StringBuilder()
        sb.AppendLine("KCDATA")
        sb.AppendLine("======")
        sb.AppendLine(nMeasures.ToString)
        sb.AppendLine(nNotes.ToString)
        For n = 1 To notes.GetUpperBound(0)
            sb.AppendLine(notes(n))
        Next

        'Write data to new chart data file on the desktop
        Dim mydocpath As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim filename As String = "\" + chartName + ".kcdata"
        Using outfile As New StreamWriter(mydocpath & filename)
            outfile.Write(sb.ToString())
        End Using

        'Notify user and update internal status
        MessageBox.Show("Chart data file saved to desktop!", "Success")
        hasSaved = True
    End Sub
End Class
