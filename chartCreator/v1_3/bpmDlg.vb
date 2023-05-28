Imports System.Windows.Forms

Public Class bpmDlg


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        For Each row As DataGridViewRow In DataGridView1.Rows
            Dim bpm As Decimal = CDec(row.Cells.Item(2).Value)
            If bpm = 0 Then
                MessageBox.Show("You must enter a BPM value for every measure that a change occurs!")
                mainForm.btnViewChart.Enabled = False
                mainForm.btnSave.Enabled = False
                Exit Sub
            Else
                Dim msrNum As Integer = CInt(row.Cells.Item(0).Value)
                Dim beatNum As Integer = CInt(row.Cells.Item(1).Value)
                Dim bpmVal As Decimal = CDec(row.Cells.Item(2).Value)
                For i As Integer = 0 To mainForm.bpmMarkers.GetUpperBound(0) - 1
                    Dim iMarker As bpmMarker = mainForm.bpmMarkers(i)
                    If iMarker.Measure = msrNum And iMarker.Beat = beatNum Then
                        mainForm.bpmMarkers(i).BPM = bpmVal
                        Exit For
                    End If
                Next
            End If
        Next

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub


    Private Sub bpmDlg_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        DataGridView1.Rows.Clear()
        For m As Integer = 0 To mainForm.bpmMarkers.GetUpperBound(0) - 1
            Dim thisMarker As bpmMarker = mainForm.bpmMarkers(m)

            'Add row
            DataGridView1.Rows.Add(New String() {thisMarker.Measure.ToString, thisMarker.Beat.ToString, thisMarker.BPM.ToString})
        Next

    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Dim copyValue As Decimal = CType(DataGridView1.Rows.Item(e.RowIndex).Cells.Item(2).Value, Decimal)
        For i As Integer = e.RowIndex To DataGridView1.Rows.Count - 1
            DataGridView1.Rows.Item(i).Cells.Item(2).Value = copyValue
        Next

    End Sub
End Class
