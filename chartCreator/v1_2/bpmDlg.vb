Imports System.Windows.Forms

Public Class bpmDlg


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ReDim mainForm.bpmValues(mainForm.bpmMarkers.Length)

        For Each row As DataGridViewRow In DataGridView1.Rows
            Dim bpm As Decimal = CDec(row.Cells.Item(1).Value)
            If bpm = 0 Then
                MessageBox.Show("You must enter a BPM value for every measure that a change occurs!")
                mainForm.btnViewChart.Enabled = False
                mainForm.btnSave.Enabled = False
                Exit Sub
            Else
                Dim msrNum As Integer = CInt(row.Cells.Item(0).Value)
                Dim bpmIndex = Array.IndexOf(mainForm.bpmMarkers, msrNum)
                mainForm.bpmValues(bpmIndex) = bpm
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
        For m As Integer = 0 To mainForm.bpmMarkers.GetUpperBound(0)
            Dim change As Integer = mainForm.bpmMarkers(m)
            Dim bpmVal As Decimal = 0
            If mainForm.bpmValues.Length = mainForm.bpmMarkers.Length + 1 Then
                bpmVal = mainForm.bpmValues(m)
            End If

            'Add row
            DataGridView1.Rows.Add(New String() {change.ToString, bpmVal.ToString})
        Next
        'Remove the extra row
        DataGridView1.Rows.RemoveAt(DataGridView1.Rows.GetLastRow(False))

    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Dim copyValue As Decimal = CType(DataGridView1.Rows.Item(e.RowIndex).Cells.Item(1).Value, Decimal)
        For i As Integer = e.RowIndex To DataGridView1.Rows.Count - 1
            DataGridView1.Rows.Item(i).Cells.Item(1).Value = copyValue
        Next

    End Sub
End Class
