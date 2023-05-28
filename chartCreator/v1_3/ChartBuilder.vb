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
        Dim fmBuilt As Integer = 0
        Dim markersSet As Integer = 0
        Dim buildBpm As Decimal = 0
        Dim elapsed As Decimal = 0

        'MeasureNum-BeatNum-BPM-Length-Start-pxMs

        'Determine number of measures
        Dim nmd() As Integer = CType(mainForm.chartData(2), Integer())
        Dim nMeasures As Integer = nmd(0)
        Dim finalMeasures(nMeasures) As String

        'Build each measure
        For m As Integer = 1 To nMeasures
            Dim mspx As Decimal = 0
            Dim start As Decimal = 0
            Dim length As Decimal = 0

            Dim numBeats As Integer = mainForm.beatMeasures(m - 1)
            Dim nbm As Decimal = CDec(numBeats / 4)

            'Update the build BPM
            For b As Integer = 1 To numBeats
                Dim quarter As Decimal = 60000
                For fbm As Integer = markersSet To mainForm.bpmMarkers.GetUpperBound(0) - 1
                    If mainForm.bpmMarkers(fbm).Measure = m Then
                        If mainForm.bpmMarkers(fbm).Beat = b Then
                            buildBpm = mainForm.bpmMarkers(fbm).BPM
                            markersSet += 1
                        End If
                    End If
                Next

                'Determine the beat length (ms)
                Try
                    If buildBpm = 0 Then
                        buildBpm = mainForm.bpmMarkers(mainForm.bpmMarkers.GetUpperBound(0) - 2).BPM
                    End If
                    quarter = quarter / buildBpm
                Catch ex As Exception
                    MessageBox.Show("could not determine beat length (measure " + m.ToString + ", beat " + b.ToString + ")" + vbNewLine + ex.ToString)
                End Try

                '#  try the following line if its still not fixed::
                '#  quarter = (quarter / buildBpm) * nbm

                'Determine the ms value of 1px
                mspx = quarter / 60

                'Set the startTime (ms)
                If m = 1 And b = 1 Then
                    start = 0
                Else

                    Dim oss() As String = finalMeasures(fmBuilt - 1).Split((New Char() {"-"c}))
                    Dim osStart As Decimal = CType(oss(4), Decimal)
                    Dim osLength As Decimal = CType(oss(3), Decimal)
                    start = osStart + osLength
                End If

                'Create the FM data
                Dim strMeasure As String = m.ToString + "-" + b.ToString + "-" + buildBpm.ToString + "-" + quarter.ToString + "-" + start.ToString + "-" + mspx.ToString
                finalMeasures(fmBuilt) = strMeasure
                ReDim Preserve finalMeasures(fmBuilt + 1)
                fmBuilt += 1

                'Add the length of this measure to the song length
                mainForm.songLength = mainForm.songLength + CInt(quarter)
            Next

        Next

        'Keep built measures
        ReDim builtMeasures(fmBuilt)
        builtMeasures = finalMeasures

        'Calculate note timing
        buildNotes()
    End Sub

    Private Sub buildNotes()
        Dim dgvRows As DataGridViewRowCollection = chartDlg.DataGridView1.Rows

        For r As Integer = 0 To dgvRows.Count - 1
            'Determine the note measure, pixel offset, and pixel duration
            Dim noteMeasure As Integer = CInt(dgvRows.Item(r).Cells.Item(4).Value)
            Dim pxo As Integer = CInt(dgvRows.Item(r).Cells.Item(5).Value)
            Dim pxd As Integer = CInt(dgvRows.Item(r).Cells.Item(6).Value)

            'Determine the number of beats in the parent measure
            Dim nBeats As Integer = mainForm.beatMeasures(noteMeasure - 1)

            'Determine the parent beat (based on pixel offset)
            Dim noteBeat As Integer
            If pxo < 60 Then
                noteBeat = 1
            ElseIf pxo >= 60 And pxo < 120 Then
                noteBeat = 2
            ElseIf pxo >= 120 And pxo < 180 Then
                noteBeat = 3
            ElseIf pxo >= 180 And pxo < 240 Then
                noteBeat = 4
            End If

            'Determine the FM (builtMeasures) index
            For f As Integer = 0 To builtMeasures.GetUpperBound(0) - 1
                Dim mParts() As String = builtMeasures(f).Split((New Char() {"-"c}))
                Dim fMsr As Integer = CInt(mParts(0))
                Dim fBeat As Integer = CInt(mParts(1))
                If fMsr = noteMeasure And fBeat = noteBeat Then
                    Try
                        'Determine the MS values for this note
                        Dim mpx As Decimal = CType(mParts(5), Decimal)
                        Dim mms As Decimal = CType(mParts(4), Decimal)
                        Dim pxoFix As Integer = (noteBeat * 60) - 60
                        If pxo = 0 Then
                            pxoFix = 0
                        Else
                            pxoFix = pxo - pxoFix
                        End If

                        Dim msOffset As Decimal = mpx * pxoFix
                        Dim msDuration As Decimal = mpx * pxd
                        Dim noteStart As Decimal = mms + msOffset
                        chartDlg.DataGridView1.Rows.Item(r).Cells.Item(0).Value = noteStart
                        chartDlg.DataGridView1.Rows.Item(r).Cells.Item(3).Value = msDuration
                    Catch ex As Exception
                        MessageBox.Show(ex.ToString + vbNewLine + vbNewLine + "measure " + noteMeasure.ToString + ", beat " + noteBeat.ToString)
                    End Try

                    Exit For
                End If
            Next
        Next

        Try
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
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
        
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
