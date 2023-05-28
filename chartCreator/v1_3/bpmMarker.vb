Public Class bpmMarker
    Private mParent As Integer
    Private bParent As Integer
    Private bpmValue As Decimal
    Private pxMs As Decimal

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


End Class
