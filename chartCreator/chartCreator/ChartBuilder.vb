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
                Dim quarter As Decimal = 30000
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
                mspx = quarter / 30

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
            If pxo < 30 Then
                noteBeat = 1
            ElseIf pxo >= 30 And pxo < 60 Then
                noteBeat = 2
            ElseIf pxo >= 60 And pxo < 90 Then
                noteBeat = 3
            ElseIf pxo >= 90 And pxo < 120 Then
                noteBeat = 4
            ElseIf pxo >= 120 And pxo < 150 Then
                noteBeat = 5
            ElseIf pxo >= 150 And pxo < 180 Then
                noteBeat = 6
            ElseIf pxo >= 180 And pxo < 210 Then
                noteBeat = 7
            ElseIf pxo >= 210 And pxo < 240 Then
                noteBeat = 8
            End If

            'If pxo < 60 Then
            'noteBeat = 1
            'ElseIf pxo >= 60 And pxo < 120 Then
            'noteBeat = 2
            'ElseIf pxo >= 120 And pxo < 180 Then
            'noteBeat = 3
            'ElseIf pxo >= 180 And pxo < 240 Then
            'noteBeat = 4
            'End If

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
                        Dim pxoFix As Integer = (noteBeat * 30) - 30
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

            'Prevent unnecessary button releases
            fixNoteDurations()

            'Prepare the chart data to be saved
            prepareChart()
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
    End Sub

    Private Sub fixNoteDurations()
        'Set holds
        setNoteHolds()

        'Set end holds
        setEndHolds()

        'Set the note durations to the end of the hold
        Dim dgvRows As DataGridViewRowCollection = chartDlg.DataGridView1.Rows
        For r As Integer = 0 To dgvRows.Count - 1
            If dgvRows.Item(r).Cells.Item(7).Value Is Nothing Then
                'Skip (not a hold)
                Continue For
            Else
                Dim hVal As String = dgvRows.Item(r).Cells.Item(7).Value.ToString
                If hVal = "hold" Then
                    'Determine the hIndex
                    Dim hIndex As String = dgvRows.Item(r).Cells.Item(8).Value.ToString

                    'Find the end of the hold
                    Dim holdEndIndex As Integer
                    For e As Integer = r + 1 To dgvRows.Count - 1
                        If dgvRows.Item(e).Cells.Item(7).Value Is Nothing Then
                            'Skip (not a hold)
                            Continue For
                        Else
                            'Determine if this is the same hold index
                            Dim checkIndex As String = dgvRows.Item(e).Cells.Item(8).Value.ToString
                            If checkIndex <> hIndex Then
                                'Skip (not the hold we are looking for)
                                Continue For
                            Else
                                'Check if this is the end of the hold
                                If dgvRows.Item(e).Cells.Item(7).Value.ToString = "end" Then
                                    holdEndIndex = e
                                    Exit For
                                End If
                            End If
                        End If
                    Next

                    'Set the duration of this note to the end of the hold
                    Dim myMs As Decimal = CType(dgvRows.Item(r).Cells.Item(0).Value, Decimal)
                    Dim endMs As Decimal = CType(dgvRows.Item(holdEndIndex).Cells.Item(0).Value, Decimal)
                    Dim endDuration As Decimal = CType(dgvRows.Item(holdEndIndex).Cells.Item(3).Value, Decimal)
                    Dim newDuration As Decimal = endMs - myMs + endDuration
                    dgvRows.Item(r).Cells.Item(3).Value = newDuration.ToString
                End If
            End If
        Next
    End Sub

    Private Sub setNoteHolds()
        Dim dgvRows As DataGridViewRowCollection = chartDlg.DataGridView1.Rows
        For r As Integer = 0 To dgvRows.Count - 1
            Try
                Dim thisMs As Decimal = CType(dgvRows.Item(r).Cells.Item(0).Value, Decimal)
                Dim thisColor As String = dgvRows.Item(r).Cells.Item(1).Value.ToString
                Dim thisDuration As Decimal = CType(dgvRows.Item(r).Cells.Item(3).Value, Decimal)

                'Check the next 2 notes (in case current note is part of chord)
                Dim next1Ms As Decimal = CType(dgvRows.Item(r + 1).Cells.Item(0).Value, Decimal)
                Dim next2Ms As Decimal = CType(dgvRows.Item(r + 2).Cells.Item(0).Value, Decimal)
                Dim chord2 As Boolean = False
                Dim chord3 As Boolean = False

                If thisMs = next1Ms Then
                    'This note is part of a 2-chord
                    chord2 = True

                    If thisMs = next2Ms Then
                        'This note is part of a 3-chord
                        chord3 = True
                    End If
                End If

                If chord2 = True And chord3 = True Then
                    'This note is part of a 3-chord; Get the color of the next 2 notes
                    Dim chord2Color As String = dgvRows.Item(r + 1).Cells.Item(1).Value.ToString
                    Dim chord3Color As String = dgvRows.Item(r + 2).Cells.Item(1).Value.ToString

                    'Check the following 3 notes (after this chord) ms/color against thisNote & chord2 & chord3 to see if we need to hold any
                    Dim nextNoteColor1 As String = dgvRows.Item(r + 3).Cells.Item(1).Value.ToString
                    Dim nextNoteMs1 As Decimal = CType(dgvRows.Item(r + 3).Cells.Item(0).Value, Decimal)
                    Dim nextNoteColor2 As String = dgvRows.Item(r + 4).Cells.Item(1).Value.ToString
                    Dim nextNoteMs2 As Decimal = CType(dgvRows.Item(r + 4).Cells.Item(0).Value, Decimal)
                    Dim nextNoteColor3 As String = dgvRows.Item(r + 5).Cells.Item(1).Value.ToString
                    Dim nextNoteMs3 As Decimal = CType(dgvRows.Item(r + 5).Cells.Item(0).Value, Decimal)

                    If nextNoteMs1 <> nextNoteMs2 Then
                        'Next note is not a chord; Only check color of the following note after this chord to see if we should hold either one
                        If nextNoteColor1 = thisColor Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'Following note is same color as thisNote, hold thisNote
                        ElseIf nextNoteColor1 = chord2Color Then
                            chartDlg.DataGridView1.Rows.Item(r + 1).Cells.Item(7).Value = "hold"    'Following note is same color as chord2, hold chord2
                        ElseIf nextNoteColor1 = chord3Color Then
                            chartDlg.DataGridView1.Rows.Item(r + 2).Cells.Item(7).Value = "hold"    'Following note is same color as chord3, hold chord3
                        End If
                    ElseIf nextNoteMs1 = nextNoteMs2 And nextNoteMs1 <> nextNoteMs3 Then
                        'Next 2 notes are a chord; check colors of the following 2 notes after this chord to see if we should hold either one
                        If thisColor = nextNoteColor1 Or thisColor = nextNoteColor2 Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'One of the notes in the following 2-chord is same color as thisNote, hold thisNote
                        End If
                        If chord2Color = nextNoteColor1 Or chord2Color = nextNoteColor2 Then
                            chartDlg.DataGridView1.Rows.Item(r + 1).Cells.Item(7).Value = "hold"    'One of the notes in the following 2-chord is same color as chord2, hold chord2
                        End If
                        If chord3Color = nextNoteColor1 Or chord3Color = nextNoteColor2 Then
                            chartDlg.DataGridView1.Rows.Item(r + 2).Cells.Item(7).Value = "hold"    'One of the notes in the following 2-chord is same color as chord3, hold chord3
                        End If

                    Else
                        'Next 3 notes are a chord; check colors of the following 3 notes after this chord to see if we should hold either one
                        If thisColor = nextNoteColor1 Or thisColor = nextNoteColor2 Or thisColor = nextNoteColor3 Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'One of the notes in the following 3-chord is same color as thisNote, hold thisNote
                        End If
                        If chord2Color = nextNoteColor1 Or chord2Color = nextNoteColor2 Or chord2Color = nextNoteColor3 Then
                            chartDlg.DataGridView1.Rows.Item(r + 1).Cells.Item(7).Value = "hold"    'One of the notes in the following 3-chord is same color as chord2, hold chord2
                        End If
                        If chord3Color = nextNoteColor1 Or chord3Color = nextNoteColor2 Or chord3Color = nextNoteColor3 Then
                            chartDlg.DataGridView1.Rows.Item(r + 2).Cells.Item(7).Value = "hold"    'One of the notes in the following 3-chord is same color as chord3, hold chord3
                        End If
                    End If

                    'Skip ahead 3 notes
                    r = r + 2


                ElseIf chord2 = True And chord3 = False Then
                    'This note is part of a 2-chord; Get the next note color (chord2)
                    Dim chord2Color As String = dgvRows.Item(r + 1).Cells.Item(1).Value.ToString

                    'Check the following 3 notes (after this chord) ms/color against thisNote & chord2 to see if we need to hold either one
                    Dim nextNoteColor1 As String = dgvRows.Item(r + 2).Cells.Item(1).Value.ToString
                    Dim nextNoteMs1 As Decimal = CType(dgvRows.Item(r + 2).Cells.Item(0).Value, Decimal)
                    Dim nextNoteColor2 As String = dgvRows.Item(r + 3).Cells.Item(1).Value.ToString
                    Dim nextNoteMs2 As Decimal = CType(dgvRows.Item(r + 3).Cells.Item(0).Value, Decimal)
                    Dim nextNoteColor3 As String = dgvRows.Item(r + 4).Cells.Item(1).Value.ToString
                    Dim nextNoteMs3 As Decimal = CType(dgvRows.Item(r + 4).Cells.Item(0).Value, Decimal)

                    If nextNoteMs1 <> nextNoteMs2 Then
                        'Next note is not a chord; Only check color of the following note after this chord to see if we should hold either one
                        If nextNoteColor1 = thisColor Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'Following note is same color as thisNote, hold thisNote
                        ElseIf nextNoteColor1 = chord2Color Then
                            chartDlg.DataGridView1.Rows.Item(r + 1).Cells.Item(7).Value = "hold"    'Following note is same color as chord2, hold chord2
                        End If
                    ElseIf nextNoteMs1 = nextNoteMs2 And nextNoteMs1 <> nextNoteMs3 Then
                        'Next 2 notes are a chord; check colors of the following 2 notes after this chord to see if we should hold either one
                        If thisColor = nextNoteColor1 Or thisColor = nextNoteColor2 Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'One of the notes in the following 2-chord is same color as thisNote, hold thisNote
                        End If
                        If chord2Color = nextNoteColor1 Or chord2Color = nextNoteColor2 Then
                            chartDlg.DataGridView1.Rows.Item(r + 1).Cells.Item(7).Value = "hold"    'One of the notes in the following 2-chord is same color as chord2, hold chord2
                        End If

                    Else
                        'Next 3 notes are a chord; check colors of the following 3 notes after this chord to see if we should hold either one
                        If thisColor = nextNoteColor1 Or thisColor = nextNoteColor2 Or thisColor = nextNoteColor3 Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'One of the notes in the following 3-chord is same color as thisNote, hold thisNote
                        End If
                        If chord2Color = nextNoteColor1 Or chord2Color = nextNoteColor2 Or chord2Color = nextNoteColor3 Then
                            chartDlg.DataGridView1.Rows.Item(r + 1).Cells.Item(7).Value = "hold"    'One of the notes in the following 3-chord is same color as chord2, hold chord2
                        End If
                    End If

                    'Skip ahead 2 notes
                    r = r + 1

                Else
                    'This note is not part of a chord; Check the following 3 notes color against thisNote to see if we need to hold it
                    Dim nextNoteColor1 As String = dgvRows.Item(r + 1).Cells.Item(1).Value.ToString
                    Dim nextNoteColor2 As String = dgvRows.Item(r + 2).Cells.Item(1).Value.ToString
                    Dim nextNoteColor3 As String = dgvRows.Item(r + 3).Cells.Item(1).Value.ToString
                    Dim nextNoteMs3 As Decimal = CType(dgvRows.Item(r + 3).Cells.Item(0).Value, Decimal)

                    If next1Ms <> next2Ms Then
                        'Next note is not a chord; Only check color of the following note after thisNote to see if we should hold it
                        If thisColor = nextNoteColor1 Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'Following note is same color as thisNote, hold thisNote
                        End If
                    ElseIf next1Ms = next2Ms And next1Ms <> nextNoteMs3 Then
                        'Next 2 notes are a chord; check colors of the following 2 notes after thisNote to see if we should hold it
                        If thisColor = nextNoteColor1 Or thisColor = nextNoteColor2 Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'One of the notes in the following 2-chord is same color as thisNote, hold thisNote
                        End If
                    Else
                        'Next 3 notes are a chord; check colors of the following 3 notes after thisNote to see if we should hold it
                        If thisColor = nextNoteColor1 Or thisColor = nextNoteColor2 Or thisColor = nextNoteColor3 Then
                            chartDlg.DataGridView1.Rows.Item(r).Cells.Item(7).Value = "hold"        'One of the notes in the following 3-chord is same color as thisNote, hold thisNote
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub setEndHolds()
        Dim gHolds As Integer = 0
        Dim rHolds As Integer = 0
        Dim yHolds As Integer = 0
        Dim bHolds As Integer = 0
        Dim oHolds As Integer = 0

        Dim numHoldsEnded As Integer = 0
        Dim holdsEnded(0) As String

        Dim dgvRows As DataGridViewRowCollection = chartDlg.DataGridView1.Rows
        For r As Integer = 0 To dgvRows.Count - 1
            Dim isHold As Boolean = False
            Dim thisColor As String = dgvRows.Item(r).Cells.Item(1).Value.ToString
            Dim hIndex As String = thisColor
            If dgvRows.Item(r).Cells.Item(7).Value Is Nothing Then
                isHold = False
            Else
                Dim checkHold As String = dgvRows.Item(r).Cells.Item(7).Value.ToString

                If checkHold = "hold" Then
                    isHold = True

                    If dgvRows.Item(r).Cells.Item(8).Value Is Nothing Then
                        'Determine and set the hold index
                        If thisColor = "G" Then
                            hIndex += gHolds.ToString
                        ElseIf thisColor = "R" Then
                            hIndex += rHolds.ToString
                        ElseIf thisColor = "Y" Then
                            hIndex += yHolds.ToString
                        ElseIf thisColor = "B" Then
                            hIndex += bHolds.ToString
                        ElseIf thisColor = "O" Then
                            hIndex += oHolds.ToString
                        End If
                        dgvRows.Item(r).Cells.Item(8).Value = hIndex
                    Else
                        'skip row if this hold has ended
                        hIndex = dgvRows.Item(r).Cells.Item(8).Value.ToString
                        If Array.IndexOf(holdsEnded, hIndex) >= 0 Then
                            Continue For
                        End If
                    End If
                End If
            End If

            If isHold = True Then
                'Check every next row that is the same color until no hold is found
                For nr As Integer = r + 1 To dgvRows.Count - 1
                    Dim nrColor As String = dgvRows.Item(nr).Cells.Item(1).Value.ToString
                    If nrColor = thisColor Then
                        'Set hold index
                        dgvRows.Item(nr).Cells.Item(8).Value = hIndex

                        'Check if hold exists
                        Dim holdExists As Boolean = False
                        If dgvRows.Item(nr).Cells.Item(7).Value Is Nothing Then
                            holdExists = False
                        Else
                            Dim checkHoldExists As String = dgvRows.Item(nr).Cells.Item(7).Value.ToString
                            If checkHoldExists = "hold" Then
                                holdExists = True
                            End If
                        End If

                        'Set "end" if no hold exists
                        If holdExists = False Then
                            dgvRows.Item(nr).Cells.Item(7).Value = "end"
                            numHoldsEnded = numHoldsEnded + 1
                            ReDim Preserve holdsEnded(numHoldsEnded)
                            holdsEnded(numHoldsEnded - 1) = hIndex
                            If thisColor = "G" Then
                                gHolds = gHolds + 1
                            ElseIf thisColor = "R" Then
                                rHolds = rHolds + 1
                            ElseIf thisColor = "Y" Then
                                yHolds = yHolds + 1
                            ElseIf thisColor = "B" Then
                                bHolds = bHolds + 1
                            ElseIf thisColor = "O" Then
                                oHolds = oHolds + 1
                            End If
                            Exit For
                        End If
                    Else
                        Continue For
                    End If
                Next
            Else
                Continue For
            End If
        Next
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
