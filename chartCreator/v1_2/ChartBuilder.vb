Option Strict On
Public Module ChartBuilder

    Private builtMeasures(0) As String

    Public Sub Build(ByVal notesArray() As String)
        chartDlg.DataGridView1.Rows.Clear()
        chartDlg.DataGridView1.Columns.Item(0).Width = 50
        chartDlg.DataGridView1.Columns.Item(5).Visible = True
        chartDlg.DataGridView1.Columns.Item(6).Visible = True
        For n As Integer = 1 To notesArray.GetUpperBound(0)
            Dim note As String = notesArray(n)
            Dim noteParts() As String = note.Split(New Char() {"-"c})
            Dim noteColor As String = noteParts(0)
            Dim noteAction As String = noteParts(1)
            Dim noteMeasure As String = noteParts(2)
            Dim notePxVal As String = noteParts(3)
            Dim notePxDur As String = noteParts(4)
            'to be continued...


            ' Add the rows of raw note data
            chartDlg.DataGridView1.Rows.Add(New String() {"?", noteColor, noteAction, "?", noteMeasure, notePxVal, notePxDur})
        Next

        'Reset song length
        mainForm.songLength = 0

        'Build the measures table from entered BPM values
        buildMeasures()

    End Sub

    Private Sub buildMeasures()
        Dim buildBpm As Decimal = 0
        Dim elapsed As Decimal = 0

        'MeasureNum-BPM-Length-Start-pxMs

        'Determine number of measures
        Dim nmd() As Integer = CType(mainForm.chartData(2), Integer())
        Dim nMeasures As Integer = nmd(0)
        Dim finalMeasures(nMeasures) As String

        'Build each measure
        For m As Integer = 1 To nMeasures
            Dim start As Decimal = 0
            Dim length As Decimal = 0
            Dim pxVal As Decimal = 0
            Dim quarter As Decimal = 60000
            Dim numBeats As Integer = mainForm.beatMeasures(m)

            'Set the BPM
            For x As Integer = 0 To mainForm.bpmMarkers.GetUpperBound(0) - 1
                Dim bpmMeasure As Integer = mainForm.bpmMarkers(x)
                If m = bpmMeasure Then
                    buildBpm = mainForm.bpmValues(x)
                    If buildBpm = 0 Then
                        Dim errStr As String = "Registered bpm change at measure " + bpmMeasure.ToString + " (index " + x.ToString + ") but buildBpm is zero"
                        MessageBox.Show(errStr, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    Exit For
                End If
            Next

            'Set the length (ms)
            quarter = quarter / buildBpm
            length = quarter * numBeats

            'Set the startTime (ms)
            If m = 1 Then
                start = 0
            Else
                Dim oss() As String = finalMeasures(m - 1).Split((New Char() {"-"c}))
                Dim osStart As Decimal = CType(oss(3), Decimal)
                Dim osLength As Decimal = CType(oss(2), Decimal)
                start = osStart + osLength
            End If

            'Set the ms value of 1px
            pxVal = quarter / 60


            'Build the measure string
            Dim strMeasure As String = m.ToString + "-" + buildBpm.ToString + "-" + length.ToString + "-" + start.ToString + "-" + pxVal.ToString
            finalMeasures(m) = strMeasure

            'Add the length of this measure to the song length
            mainForm.songLength = mainForm.songLength + CInt(length)
        Next

        'Keep built measures
        ReDim builtMeasures(nMeasures)
        builtMeasures = finalMeasures

        'Calculate note timing
        buildNotes()
    End Sub

    Private Sub buildNotes()
        Dim dgvRows As DataGridViewRowCollection = chartDlg.DataGridView1.Rows

        For r As Integer = 0 To dgvRows.Count - 1
            Dim noteMeasure As Integer = CInt(dgvRows.Item(r).Cells.Item(4).Value)
            Dim mParts() As String = builtMeasures(noteMeasure).Split((New Char() {"-"c}))
            Dim mpx As Decimal = CType(mParts(4), Decimal)
            Dim mms As Decimal = CType(mParts(3), Decimal)
            Dim pxo As Integer = CInt(dgvRows.Item(r).Cells.Item(5).Value)
            Dim pxd As Integer = CInt(dgvRows.Item(r).Cells.Item(6).Value)
            Dim msOffset As Decimal = mpx * pxo
            Dim msDuration As Decimal = mpx * pxd
            Dim noteStart As Decimal = mms + msOffset
            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(0).Value = noteStart
            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(3).Value = msDuration
        Next

        'Sort table by MS
        chartDlg.DataGridView1.Sort(chartDlg.DataGridView1.Columns.Item(0), System.ComponentModel.ListSortDirection.Ascending)

        'Offset the notes to start the chart at the first note
        Dim offsetBy As Decimal = CType(chartDlg.DataGridView1.Rows.Item(0).Cells.Item(0).Value, Decimal)
        For r As Integer = 0 To chartDlg.DataGridView1.Rows.Count - 1
            Dim currentMs As Decimal = CType(chartDlg.DataGridView1.Rows.Item(r).Cells.Item(0).Value, Decimal)
            Dim newMs As Decimal = currentMs - offsetBy
            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(0).Value = newMs
        Next

        'Remove obsolete columns
        chartDlg.DataGridView1.Columns.Item(5).Visible = False
        chartDlg.DataGridView1.Columns.Item(6).Visible = False
        chartDlg.DataGridView1.Columns.Item(0).Width = chartDlg.DataGridView1.Columns.Item(0).Width + 70

        'Determine strum pattern (alternating up/down)
        strumPattern()
    End Sub

    Private Sub strumPattern()
        Dim downStrum As Boolean = True
        Dim dgvRows As DataGridViewRowCollection = chartDlg.DataGridView1.Rows
        For r As Integer = 0 To dgvRows.Count - 1
            Dim ms As Decimal = CType(dgvRows.Item(r).Cells.Item(0).Value, Decimal)
            Dim action As String = dgvRows.Item(r).Cells.Item(2).Value.ToString
            'Skip hammers
            If action = "h" Then
                dgvRows.Item(r).Cells.Item(2).Value = "hammer"
                Continue For
            End If
            'Determine strum direction
            If action = "s" Then
                If downStrum = True Then
                    dgvRows.Item(r).Cells.Item(2).Value = "downstrum"
                    downStrum = False
                Else
                    dgvRows.Item(r).Cells.Item(2).Value = "upstrum"
                    downStrum = True
                End If
            End If
            'Detect 2-note chords to avoid false toggling strum direction
            Try
                Dim ms1 As Decimal = CType(dgvRows.Item(r + 1).Cells.Item(0).Value, Decimal)
                Dim action1 As String = dgvRows.Item(r + 1).Cells.Item(2).Value.ToString
                If ms1 = ms And action1 = action Then
                    dgvRows.Item(r + 1).Cells.Item(2).Value = "-"
                End If
            Catch ex As Exception
            End Try
            'Detect 3-note chords to avoid false toggling strum direction
            Try
                Dim ms2 As Decimal = CType(dgvRows.Item(r + 2).Cells.Item(0).Value, Decimal)
                Dim action2 As String = dgvRows.Item(r + 2).Cells.Item(2).Value.ToString
                If ms2 = ms And action2 = action Then
                    dgvRows.Item(r + 2).Cells.Item(2).Value = "-"
                End If
            Catch ex As Exception
            End Try
        Next

        'Prepare the chart data to be saved
        prepareChart()
    End Sub

    Private Sub prepareChart()
        'Build a string from each note in the chart and save it in array chartNotes
        ' so that it can be saved to a chart file that will be read by the bot program
        Dim dgvRows As DataGridViewRowCollection = chartDlg.DataGridView1.Rows
        Dim chartNotes(dgvRows.Count) As String
        For r As Integer = 0 To dgvRows.Count - 1
            Dim ms As String = dgvRows.Item(r).Cells.Item(0).Value.ToString
            Dim color As String = dgvRows.Item(r).Cells.Item(1).Value.ToString
            Dim action As String = dgvRows.Item(r).Cells.Item(2).Value.ToString
            Dim duration As String = dgvRows.Item(r).Cells.Item(3).Value.ToString
            'Convert action to a char representation
            If action = "hammer" Or action = "-" Then
                action = "X"
            End If
            If action = "upstrum" Then
                action = "U"
            End If
            If action = "downstrum" Then
                action = "D"
            End If
            'Build the chart note string
            Dim noteString As String = ms + "-" + color + "-" + action + "-" + duration
            chartNotes(r) = noteString
        Next
        'Save the built chart notes
        mainForm.chartData.Insert(1, chartNotes)

        'Enable saving of the bot file
        mainForm.btnSave.Enabled = True
    End Sub
End Module
