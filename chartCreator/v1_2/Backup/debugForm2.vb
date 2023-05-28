Imports System.Windows.Forms

Public Class debugForm2

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub debugForm2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim notes() As String = Functions.savedNotes
        Dim ub As Integer = notes.GetUpperBound(0)
        For i As Integer = 1 To ub
            Dim str As String = notes(i)
            Dim strData() As String = Split(str, "-")
            '0 - color
            '1 - type
            '2 - measure
            '3 - beat
            '4 - line
            '5 - duration
            Try
                DataGridView1.Rows.Insert(i - 1, New String() {strData(2), strData(3), strData(4), strData(0), strData(1), strData(5)})
            Catch ex As Exception

            End Try
        Next
    End Sub
End Class
