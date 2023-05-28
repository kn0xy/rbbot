Public Class bpmMarker
    Private mParent As Integer
    Private bParent As Integer
    Private bpmValue As Decimal
    Private pxMs As Decimal
    Private btStart As Integer
    Private tAxis As Integer

    Property Measure() As Integer
        Get
            Return mParent
        End Get
        Set(ByVal mVal As Integer)
            mParent = mVal
        End Set
    End Property

    Property Beat() As Integer
        Get
            Return bParent
        End Get
        Set(ByVal bVal As Integer)
            bParent = bVal
        End Set
    End Property

    Property BPM() As Decimal
        Get
            Return bpmValue
        End Get
        Set(ByVal Value As Decimal)
            bpmValue = Value
            If Value > 0 Then
                pxMs = (60000 / Value) / 60
            End If
        End Set
    End Property

    Property BeatStart() As Integer
        Get
            Return btStart
        End Get
        Set(ByVal value As Integer)
            btStart = value
        End Set
    End Property

    Property TrackAxis() As Integer
        Get
            Return tAxis
        End Get
        Set(ByVal value As Integer)
            tAxis = value
        End Set
    End Property

    Public Function getBpm(ByVal beatStart As Integer, ByVal trackAxis As Integer)
        Dim chartImg As Bitmap = CType(mainForm.chartImage, Bitmap)
        Dim yr As Integer = trackAxis - 17
        Dim bpmRect1 As New Rectangle(beatStart + 13, yr, 5, 8)
        Dim bpmRect2 As New Rectangle(beatStart + 19, yr, 5, 8)
        Dim bpmRect3 As New Rectangle(beatStart + 25, yr, 5, 8)
        Dim bpmRect4 As New Rectangle(beatStart + 31, yr, 5, 8)
        Dim bpmRect5 As New Rectangle(beatStart + 37, yr, 5, 8)

        Dim bpmCheck1 As String = checkCrop(chartImg.Clone(bpmRect1, chartImg.PixelFormat))
        Dim bpmCheck2 As String = checkCrop(chartImg.Clone(bpmRect2, chartImg.PixelFormat))
        Dim bpmCheck3 As String = checkCrop(chartImg.Clone(bpmRect3, chartImg.PixelFormat))
        Dim bpmCheck4 As String = checkCrop(chartImg.Clone(bpmRect4, chartImg.PixelFormat))
        Dim bpmCheck5 As String = checkCrop(chartImg.Clone(bpmRect5, chartImg.PixelFormat))

        Dim strBpm As String = bpmCheck1
        If bpmCheck2 <> "-1" Then
            If bpmCheck2 = "10" Then
                strBpm += "."
            Else
                strBpm += bpmCheck2
            End If

        End If
        If bpmCheck3 <> "-1" Then
            If bpmCheck3 = "10" Then
                strBpm += "."
            Else
                strBpm += bpmCheck3
            End If
        End If
        If bpmCheck4 <> "-1" Then
            If bpmCheck4 = "10" Then
                strBpm += "."
            Else
                strBpm += bpmCheck4
            End If
        End If
        If bpmCheck5 <> "-1" Then
            If bpmCheck5 = "10" Then
                strBpm += "."
            Else
                strBpm += bpmCheck5
            End If
        End If

        Return strBpm
    End Function

    Private Function checkCrop(ByVal numCrop As Bitmap)
        Dim cropNum As Integer = -1
        Dim cropPixels(5, 8) As Color

        For x As Integer = 0 To 4
            For y As Integer = 0 To 7
                cropPixels(x, y) = numCrop.GetPixel(x, y)
            Next
        Next

        For b As Integer = 0 To 10
            Dim moveOn As Boolean = False
            Dim foundMatch As Boolean = True
            For cx As Integer = 0 To 4

                For cy As Integer = 0 To 7
                    If mainForm.bpmChecks(b, cx, cy) = cropPixels(cx, cy) Then
                        Continue For
                    Else
                        foundMatch = False
                        moveOn = True
                        Exit For
                    End If
                Next
                If moveOn = True Then
                    Exit For
                End If
            Next

            If foundMatch = False Then
                Continue For
            Else
                cropNum = b
                Exit For
            End If
        Next

        Return cropNum.ToString
    End Function


End Class
