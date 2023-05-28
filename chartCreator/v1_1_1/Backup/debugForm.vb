Imports System.Windows.Forms

Public Class debugForm

    Public ry As Integer = 13
    Public testRect As Rectangle

    Public mArray() As Bitmap

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub debugForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = "1"
        Dim ub As Integer = Functions.savedMeasures.GetUpperBound(0)
        ReDim mArray(ub)
        For i As Integer = 1 To ub
            mArray(i) = Functions.savedMeasures(i)
        Next
        PictureBox1.Image = mArray(1)
        Label3.Text = 13
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Button2.Enabled = True
        Dim count As Integer = CInt(Label1.Text)
        Dim prevCount As Integer = count - 1
        PictureBox1.Image = mArray(prevCount)
        Label1.Text = prevCount.ToString
        If prevCount = 1 Then
            Button1.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Button1.Enabled = True
        Dim count As Integer = CInt(Label1.Text)
        Dim nextCount As Integer = count + 1
        PictureBox1.Image = mArray(nextCount)
        Label1.Text = nextCount.ToString
        If nextCount = mArray.GetUpperBound(0) Then
            Button2.Enabled = False
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim cG As New Rectangle(1, 0, 3, 11)
        Dim cR As New Rectangle(1, ry, 239, 11)
        Dim g As Graphics = PictureBox1.CreateGraphics()
        g.DrawRectangle(Pens.Green, cG)
        g.DrawRectangle(Pens.Red, cR)

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        ry = ry - 1
        Label3.Text = ry.ToString



    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        ry = ry + 1
        Label3.Text = ry.ToString

    End Sub
End Class
