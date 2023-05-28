Imports System.Windows.Forms

Public Class timingDlg

    Dim timing_measures As Integer = 0

    Private Function anyZeros()
        Dim zeros As Boolean = False
        For z As Integer = 0 To DataGridView1.Rows.Count - 1
            Try
                Dim zVal As Integer = CInt(DataGridView1.Rows.Item(z).Cells.Item(1).Value)
                If zVal <= 0 Then
                    zeros = True
                    Exit For
                End If
            Catch ex As Exception
                zeros = True
            End Try
        Next
        Return zeros
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Dim unused As Integer = CInt(lblUnused.Text)
            If unused = 0 Then
                Dim rowZeros As Boolean = anyZeros()
                If rowZeros = False Then
                    mainForm.lblMeasures.Text = timing_measures.ToString
                    ReDim mainForm.beatMeasures(timing_measures)
                    For m As Integer = 0 To DataGridView1.Rows.Count - 1
                        mainForm.beatMeasures(m) = CInt(DataGridView1.Rows.Item(m).Cells.Item(1).Value)
                    Next
                    mainForm.btnScan.Enabled = True
                    Me.DialogResult = System.Windows.Forms.DialogResult.OK
                    Me.Close()
                Else
                    MessageBox.Show("A measure cannot have 0 beats!", "Incorrect Timing", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("You must enter valid time values for every measure!", "Incorrect Timing", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("You must enter the number of measures in the chart!", "Incorrect Timing", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub timingDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If mainForm.lblMeasures.Text = "?" Then
            DataGridView1.Rows.Clear()
            Label2.ForeColor = Color.Red
            txtNumMeasures.Text = ""
            txtNumMeasures.Focus()
            txtNumMeasures.Focus()
            Label1.ForeColor = Color.Black
            lblUnused.ForeColor = Color.Black
            lblUnused.Text = "N/A"
        Else
            Label2.ForeColor = Color.Black
            txtNumMeasures.Text = mainForm.lblMeasures.Text
            timing_measures = CInt(mainForm.lblMeasures.Text)
            initData()
        End If
    End Sub

    Private Sub txtNumMeasures_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNumMeasures.KeyUp
        If txtNumMeasures.Text.Length > 0 Then
            Try
                If CInt(txtNumMeasures.Text) = timing_measures Then
                    btnSetMeasures.Enabled = False
                Else
                    btnSetMeasures.Enabled = True
                End If
            Catch ex As Exception
                btnSetMeasures.Enabled = False
            End Try
        Else
            btnSetMeasures.Enabled = False
        End If
    End Sub

    Private Sub btnSetMeasures_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetMeasures.Click
        Try
            timing_measures = CInt(txtNumMeasures.Text)
            Label2.ForeColor = Color.Black
            btnSetMeasures.Enabled = False
            initData()
        Catch ex As Exception
            MessageBox.Show("Invalid number of measures!")
        End Try
    End Sub

    Private Sub initData()
        DataGridView1.Rows.Clear()
        Dim used As Integer = 0
        For i As Integer = 1 To timing_measures
            If used + 4 <= mainForm.numBeats Then
                DataGridView1.Rows.Add(New String() {i.ToString, "4"})
                used = used + 4
            Else
                DataGridView1.Rows.Add(New String() {i.ToString, "0"})
            End If
        Next
        Dim unused As Integer = mainForm.numBeats - used
        lblUnused.Text = unused.ToString
        If unused > 0 Then
            Label1.ForeColor = Color.Red
            lblUnused.ForeColor = Color.Red
        Else
            Label1.ForeColor = Color.Black
            lblUnused.ForeColor = Color.Black
        End If
    End Sub

    Private Sub countUnused()
        Dim unused As Integer = mainForm.numBeats
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            Dim val As Integer = CInt(DataGridView1.Rows.Item(i).Cells.Item(1).Value)
            unused = unused - val
        Next
        lblUnused.Text = unused.ToString
        If unused <> 0 Then
            Label1.ForeColor = Color.Red
            lblUnused.ForeColor = Color.Red
        Else
            Label1.ForeColor = Color.Black
            lblUnused.ForeColor = Color.Black
        End If
    End Sub

    Private Sub DataGridView1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        Dim cValue As String = DataGridView1.Rows.Item(e.RowIndex).Cells.Item(1).Value
        countUnused()
        Try
            Dim cvi As Integer = CInt(cValue)
            Dim unused As Integer = CInt(lblUnused.Text)
            If unused < 0 Then
                Dim correctVal As Integer = unused + cvi
                DataGridView1.Rows.Item(e.RowIndex).Cells.Item(1).Value = correctVal.ToString
                MessageBox.Show("Too many beats")
            End If

        Catch ex As Exception
            MessageBox.Show("Cant do it")
        End Try
        countUnused()

    End Sub
End Class
