Option Strict On
Public Module Scanner

    Dim chartRows As Integer

    Public Sub loadImage(ByVal imgFilename As String, ByVal imgSafename As String)
        'Update UI
        mainForm.lblImgFile.Text = imgSafename
        mainForm.OpenChartToolStripMenuItem.Enabled = False
        mainForm.CloseChartToolStripMenuItem.Enabled = True
        mainForm.btnOpenImg.Enabled = False
        mainForm.TimingToolStripMenuItem.Enabled = True

        'Load the image into memory
        mainForm.chartImage = Image.FromFile(imgFilename)
        chartRows = CInt((mainForm.chartImage.Height - 130) / 130)
        mainForm.chartRows = chartRows
        ReDim mainForm.beatRows(chartRows)
        checkImage()
    End Sub

    Public Sub checkImage()
        Dim chartImg As Bitmap = CType(mainForm.chartImage, Bitmap)
        Dim chartWhite As Color = chartImg.GetPixel(500, 5)
        Dim guessMeasures As Integer = chartRows * 4
        Dim nBeats As Integer = 0
        For r As Integer = 0 To chartRows - 1
            Dim row As Integer = r + 1
            Dim rb As Integer = 0
            Dim y As Integer = 130 * r + 100
            For b As Integer = 1 To 16
                Dim x As Integer = 60 * b + 25
                Dim px As Color = chartImg.GetPixel(x, y)
                If px <> chartWhite Then
                    nBeats = nBeats + 1
                    rb = rb + 1
                End If
            Next
            mainForm.beatRows(r) = rb
        Next

        mainForm.numBeats = nBeats
        If nBeats Mod 4 = 0 Then
            Dim nMeasures As Integer = CInt(nBeats / 4)
            If nMeasures <> guessMeasures Then
                Dim userConfirm As String = "Detected " + nMeasures.ToString + " measures." + vbNewLine + "Is this correct?"
                If MessageBox.Show(userConfirm, "Timing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    ReDim mainForm.beatMeasures(nMeasures)
                    For i As Integer = 0 To nMeasures - 1
                        mainForm.beatMeasures(i) = 4
                    Next
                    mainForm.lblMeasures.Text = nMeasures.ToString
                    mainForm.btnScan.Enabled = True
                Else
                    mainForm.lblMeasures.Text = "?"
                    MessageBox.Show("Timing needs to be configured manually.", "Measures", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End If
            Else
                ReDim mainForm.beatMeasures(nMeasures)
                For i As Integer = 0 To nMeasures - 1
                    mainForm.beatMeasures(i) = 4
                Next
                mainForm.lblMeasures.Text = nMeasures.ToString
                mainForm.btnScan.Enabled = True

            End If
        Else
            mainForm.lblMeasures.Text = "?"
            MessageBox.Show("Timing needs to be configured manually.", "Off Beat", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End If

    End Sub

    Public Sub scanRows()

    End Sub


    Public savedMeasures() As Bitmap
    Public Sub scanMeasures()
        'Update UI
        mainForm.btnScan.Enabled = False
        mainForm.btnScan.Text = "Scanning chart..."
        mainForm.lblScanVal.Text = "0"
        mainForm.lblScanVal.Visible = True
        mainForm.lblScanInfo.Text = "of " + mainForm.lblMeasures.Text + " measures"
        mainForm.lblScanInfo.Visible = True
        mainForm.lblProgress.Text = "0%"
        Application.DoEvents()

        'Declare color for BPM check
        Dim cBpm As Color = My.Resources.bpm.GetPixel(2, 6)

        'Scan each row
        Dim chartImg As Bitmap = CType(mainForm.chartImage, Bitmap)
        Dim numMeasures As Integer = CInt(mainForm.lblMeasures.Text)
        Dim numRows As Integer = mainForm.chartRows
        Dim progress As Double = 5          'we are starting at 5%
        Dim progIncDbl As Double = (45 / numMeasures)
        Dim measuresAdded As Integer = 0
        Dim cropsAdded As Integer = 1
        ReDim savedMeasures(numMeasures)
        For r As Integer = 1 To numRows
            Dim rbi As Integer = r - 1
            Dim rowBound As Integer = mainForm.beatRows(rbi)
            For b As Integer = 1 To rowBound
                Dim msrBeats As Integer = mainForm.beatMeasures(measuresAdded)
                b = b - 1 + msrBeats
                Dim msrEnd As Integer = 60 * b + 24
                Dim msrWidth As Integer = msrBeats * 60
                Dim msrStart As Integer = msrEnd - msrWidth
                'Create crop of current measure
                Dim ry As Integer = 100         'top of measure
                ry -= 5                         'see all the notes on the green track
                If r > 1 Then
                    Dim yc As Integer = 130 * r
                    yc -= 130
                    ry += yc
                End If
                Dim mRect As New Rectangle(msrStart, ry, msrWidth, 59)
                Try
                    Dim crop As Bitmap = chartImg.Clone(mRect, chartImg.PixelFormat)
                    savedMeasures(cropsAdded) = crop
                Catch ex As Exception
                End Try
                'Check for BPM marker
                Dim yM As Integer = ry - 13
                Dim bpmRect As New Rectangle(msrStart, yM, 4, 4)
                Dim bpmCrop As Bitmap = chartImg.Clone(bpmRect, chartImg.PixelFormat)
                Dim cropColor As Color = bpmCrop.GetPixel(2, 2)
                If cropColor = cBpm Then
                    Dim msrNum As Integer = measuresAdded + 1
                    ReDim Preserve mainForm.bpmMarkers(mainForm.bpmMarkers.GetUpperBound(0) + 1)
                    mainForm.bpmMarkers(mainForm.bpmMarkers.GetUpperBound(0) - 1) = msrNum
                    mainForm.lblBpmChanges.Text = mainForm.bpmMarkers.GetUpperBound(0).ToString
                End If

                'update UI and internal counter
                mainForm.lblScanVal.Text = cropsAdded.ToString
                cropsAdded += 1
                measuresAdded += 1
                progress += progIncDbl
                Dim progInc As Integer = CInt(progress)
                mainForm.iProgressBar.Value = progInc
                mainForm.lblProgress.Text = progInc.ToString + "%"
                Application.DoEvents()
            Next
            'For m As Integer = 1 To 4
            '    'Create crop of current measure
            '    Dim ry As Integer = 100         'top of measure
            '    ry -= 5                         'see all the notes on the green track
            '    If r > 1 Then
            '        Dim yc As Integer = 130 * r
            '        yc -= 130
            '        ry += yc
            '    End If
            '    Dim xc As Integer = 240 * m     'end of measure
            '    Dim rxe As Integer = 25 + xc    'compensate for left padding
            '    Dim rx As Integer = rxe - 241   'start of measure
            '    Dim mRect As New Rectangle(rx, ry, 242, 59)
            '    Try
            '        Dim crop As Bitmap = chartImg.Clone(mRect, chartImg.PixelFormat)
            '        savedMeasures(cropsAdded) = crop
            '    Catch ex As Exception
            '    End Try
            '    'Check for BPM marker
            '    Dim yM As Integer = ry - 13
            '    Dim bpmRect As New Rectangle(rx, yM, 4, 4)
            '    Dim bpmCrop As Bitmap = chartImg.Clone(bpmRect, chartImg.PixelFormat)
            '    Dim cropColor As Color = bpmCrop.GetPixel(2, 2)
            '    If cropColor = cBpm Then
            '        Dim msrNum As Integer = r * 4 - 4 + m
            '        ReDim Preserve mainForm.bpmMarkers(mainForm.bpmMarkers.GetUpperBound(0) + 1)
            '        mainForm.bpmMarkers(mainForm.bpmMarkers.GetUpperBound(0) - 1) = msrNum
            '        mainForm.lblBpmChanges.Text = mainForm.bpmMarkers.GetUpperBound(0).ToString
            '    End If

            '    'update UI and internal counter
            '    mainForm.lblScanVal.Text = cropsAdded.ToString
            '    cropsAdded += 1
            '    progress += progIncDbl
            '    Dim progInc As Integer = CInt(progress)
            '    mainForm.iProgressBar.Value = progInc
            '    mainForm.lblProgress.Text = progInc.ToString + "%"
            '    Application.DoEvents()
            'Next
        Next
        'Update UI and move on
        mainForm.lblMeasures.Text = numMeasures.ToString
        mainForm.iProgressBar.Value = 50
        mainForm.lblProgress.Text = mainForm.iProgressBar.Value.ToString + "%"
        readMeasures()
    End Sub

    Public savedNotes() As String
    Public Sub readMeasures()
        'Update UI
        mainForm.btnScan.Text = "Processing notes..."
        mainForm.lblScanVal.Text = "0"
        mainForm.lblScanInfo.Text = "notes found"

        'Find colors
        Dim snote As Bitmap = My.Resources.snote
        Dim gNote As Bitmap = My.Resources.gnote
        Dim rNote As Bitmap = My.Resources.rnote
        Dim yNote As Bitmap = My.Resources.ynote
        Dim bNote As Bitmap = My.Resources.bnote
        Dim oNote As Bitmap = My.Resources.onote
        Dim cGray As Color = snote.GetPixel(2, 4)
        Dim cBlack As Color = gNote.GetPixel(2, 0)
        Dim cGreen As Color = gNote.GetPixel(2, 4)
        Dim cRed As Color = rNote.GetPixel(2, 4)
        Dim cYellow As Color = yNote.GetPixel(2, 4)
        Dim cBlue As Color = bNote.GetPixel(2, 4)
        Dim cOrange As Color = oNote.GetPixel(2, 4)

        'Declare variables
        Dim strums As Integer = 0
        Dim hammers As Integer = 0
        Dim holds As Integer = 0
        Dim rtNotes As Integer = 0
        Dim otNotes As Integer = 0
        Dim gnotes As Integer = 0
        Dim rnotes As Integer = 0
        Dim ynotes As Integer = 0
        Dim bnotes As Integer = 0
        Dim onotes As Integer = 0
        Dim spnotes As Integer = 0
        Dim notesAdded As Integer = 0
        Dim gPressed As Integer = 0
        Dim rPressed As Integer = 0
        Dim yPressed As Integer = 0
        Dim bPressed As Integer = 0
        Dim oPressed As Integer = 0

        'Scan each measure for notes
        Dim nMeasures As Integer = savedMeasures.GetUpperBound(0)
        Dim progIncDbl As Double = 50 / nMeasures
        Dim progress As Double = 50             'starting at 50%
        For msr As Integer = 1 To nMeasures
            Dim msrImg As Bitmap = savedMeasures(msr)

            ' ---------------------- Check the GREEN track ---------------------------
            For cgt As Integer = 0 To msrImg.Width - 3
                Dim cG As New Rectangle(cgt, 0, 3, 11)
                Dim checkG As Bitmap = msrImg.Clone(cG, msrImg.PixelFormat)
                Dim glb As Color = checkG.GetPixel(0, 5)     'left border
                Dim gp4 As Color = checkG.GetPixel(2, 0)     'green plus 4 (strum)
                Dim gp3 As Color = checkG.GetPixel(2, 1)     'green plus 3 (hammeron)
                Dim gt As Color = checkG.GetPixel(2, 4)      'track line
                Dim gt1 As Color = checkG.GetPixel(2, 5)     'track line
                Dim gt2 As Color = checkG.GetPixel(2, 6)     'track line
                Dim gm3 As Color = checkG.GetPixel(2, 9)     'green minus 4 (hammer)
                Dim gm4 As Color = checkG.GetPixel(2, 10)    'green minus 4 (strum)
                Dim isHammer As Boolean = False
                Dim isStrum As Boolean = False

                'Determine if we have a GREEN strum note
                If gp4 = cBlack And gm4 = cBlack And gp3 = cGreen And gt = cGreen And glb = cGreen Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    gnotes += 1
                    notesAdded += 1
                    gPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "G-s-" + msr.ToString + "-" + cgt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    mainForm.lblGcount.Text = gnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY strum note
                If gp4 = cBlack And gm4 = cBlack And gp3 = cGray And gt = cGray And glb = cGray Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    gnotes += 1
                    notesAdded += 1
                    gPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "G-s-" + msr.ToString + "-" + cgt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    mainForm.lblGcount.Text = gnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GREEN hammer note
                If gp3 = cBlack And gm3 = cBlack And gt = cGreen And glb = cGreen Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    gnotes += 1
                    notesAdded += 1
                    gPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "G-h-" + msr.ToString + "-" + cgt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    mainForm.lblGcount.Text = gnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY hammer note
                If gp3 = cBlack And gm3 = cBlack And gt = cGray And glb = cGray Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    gnotes += 1
                    notesAdded += 1
                    gPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "G-h-" + msr.ToString + "-" + cgt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    '
                    mainForm.lblGcount.Text = gnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GREEN hold
                If isStrum = False And isHammer = False And gt = cGreen And gt1 = cGreen And gt2 = cGreen Then
                    If gPressed > 0 Then
                        Dim lastNote() As String = savedNotes(gPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(gPressed) = modNote
                    End If
                End If
                'Determine if we have a GRAY (green) hold
                If isStrum = False And isHammer = False And gt = cGray And gt1 = cGray And gt2 = cGray Then
                    If gPressed > 0 Then
                        Dim lastNote() As String = savedNotes(gPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(gPressed) = modNote
                    End If
                End If
                checkG.Dispose()    'free up memory
            Next


            ' ---------------------- Check the RED track ---------------------------
            For crt As Integer = 0 To msrImg.Width - 3
                Dim cR As New Rectangle(crt, 12, 3, 11)
                Dim checkR As Bitmap = msrImg.Clone(cR, msrImg.PixelFormat)
                Dim rlb As Color = checkR.GetPixel(0, 5)      'red left border
                Dim rp4 As Color = checkR.GetPixel(2, 0)     'red plus 4 (strum)
                Dim rp3 As Color = checkR.GetPixel(2, 1)     'red plus 3 (hammeron)
                Dim rt As Color = checkR.GetPixel(2, 4)      'track line
                Dim rt1 As Color = checkR.GetPixel(2, 5)     'track line
                Dim rt2 As Color = checkR.GetPixel(2, 6)     'track line
                Dim rm3 As Color = checkR.GetPixel(2, 9)     'red minus 3 (hammer)
                Dim rm4 As Color = checkR.GetPixel(2, 10)    'red minus 4 (strum)
                Dim isHammer As Boolean = False
                Dim isStrum As Boolean = False



                'Determine if we have a RED strum note
                If rp4 = cBlack And rm4 = cBlack And rp3 = cRed And rt = cRed And rlb = cRed Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    rnotes += 1
                    notesAdded += 1
                    rPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "R-s-" + msr.ToString + "-" + crt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    mainForm.lblRcount.Text = rnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY strum note
                If rp4 = cBlack And rm4 = cBlack And rp3 = cGray And rt = cGray And rlb = cGray Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    rnotes += 1
                    notesAdded += 1
                    rPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "R-s-" + msr.ToString + "-" + crt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    mainForm.lblRcount.Text = rnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a RED hammer note
                If rp3 = cBlack And rm3 = cBlack And rt = cRed And rlb = cRed Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    rnotes += 1
                    notesAdded += 1
                    rPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "R-h-" + msr.ToString + "-" + crt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    mainForm.lblRcount.Text = rnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY hammer note
                If rp3 = cBlack And rm3 = cBlack And rt = cGray And rlb = cGray Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    rnotes += 1
                    notesAdded += 1
                    rPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "R-h-" + msr.ToString + "-" + crt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    mainForm.lblRcount.Text = rnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a RED hold
                If isStrum = False And isHammer = False And rt = cRed And rt1 = cRed And rt2 = cRed Then
                    If rPressed > 0 Then
                        Dim lastNote() As String = savedNotes(rPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(rPressed) = modNote
                    End If
                End If
                'Determine if we have a GRAY (red) hold
                If isStrum = False And isHammer = False And rt = cGray And rt1 = cGray And rt2 = cGray Then
                    If rPressed > 0 Then
                        Dim lastNote() As String = savedNotes(rPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(rPressed) = modNote
                    End If
                End If
                checkR.Dispose()    'free up memory
            Next


            ' ---------------------- Check the YELLOW track ---------------------------
            For cyt As Integer = 0 To msrImg.Width - 3
                Dim cY As New Rectangle(cyt, 24, 3, 11)
                Dim checkY As Bitmap = msrImg.Clone(cY, msrImg.PixelFormat)
                Dim ylb As Color = checkY.GetPixel(0, 5)     'Yellow left border
                Dim yp4 As Color = checkY.GetPixel(2, 0)     'Yellow plus 4 (strum)
                Dim yp3 As Color = checkY.GetPixel(2, 1)     'Yellow plus 3 (hammeron)
                Dim yt As Color = checkY.GetPixel(2, 4)      'track line
                Dim yt1 As Color = checkY.GetPixel(2, 5)     'track line
                Dim yt2 As Color = checkY.GetPixel(2, 6)     'track line
                Dim ym3 As Color = checkY.GetPixel(2, 9)     'Yellow minus 4 (hammer)
                Dim ym4 As Color = checkY.GetPixel(2, 10)    'Yellow minus 4 (strum)
                Dim isHammer As Boolean = False
                Dim isStrum As Boolean = False

                'Determine if we have a YELLOW strum note
                If yp4 = cBlack And ym4 = cBlack And yp3 = cYellow And yt = cYellow And ylb = cYellow Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    ynotes += 1
                    notesAdded += 1
                    yPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "Y-s-" + msr.ToString + "-" + cyt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    '
                    mainForm.lblYcount.Text = ynotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY strum note
                If yp4 = cBlack And ym4 = cBlack And yp3 = cGray And yt = cGray And ylb = cGray Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    ynotes += 1
                    notesAdded += 1
                    yPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "Y-s-" + msr.ToString + "-" + cyt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    '
                    mainForm.lblYcount.Text = ynotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a YELLOW hammer note
                If yp3 = cBlack And ym3 = cBlack And yt = cYellow And ylb = cYellow Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    ynotes += 1
                    notesAdded += 1
                    yPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "Y-h-" + msr.ToString + "-" + cyt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    '
                    mainForm.lblYcount.Text = ynotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY hammer note
                If yp3 = cBlack And ym3 = cBlack And yt = cGray And ylb = cGray Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    ynotes += 1
                    notesAdded += 1
                    yPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "Y-h-" + msr.ToString + "-" + cyt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    '
                    mainForm.lblYcount.Text = ynotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a YELLOW hold
                If isStrum = False And isHammer = False And yt = cYellow And yt1 = cYellow And yt2 = cYellow Then
                    If yPressed > 0 Then
                        Dim lastNote() As String = savedNotes(yPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(yPressed) = modNote
                    End If
                End If
                'Determine if we have a GRAY (yellow) hold
                If isStrum = False And isHammer = False And yt = cGray And yt1 = cGray And yt2 = cGray Then
                    If yPressed > 0 Then
                        Dim lastNote() As String = savedNotes(yPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(yPressed) = modNote
                    End If
                End If

                checkY.Dispose()    'free up memory
            Next




            ' ---------------------- Check the BLUE track ---------------------------
            For cbt As Integer = 0 To msrImg.Width - 3
                Dim cB As New Rectangle(cbt, 36, 3, 11)
                Dim checkB As Bitmap = msrImg.Clone(cB, msrImg.PixelFormat)
                Dim blb As Color = checkB.GetPixel(0, 5)     'Blue left border
                Dim bp4 As Color = checkB.GetPixel(2, 0)     'Blue plus 4 (strum)
                Dim bp3 As Color = checkB.GetPixel(2, 1)     'Blue plus 3 (hammeron)
                Dim blt As Color = checkB.GetPixel(2, 4)     'track line
                Dim bt1 As Color = checkB.GetPixel(2, 5)     'track line
                Dim bt2 As Color = checkB.GetPixel(2, 6)     'track line
                Dim bm3 As Color = checkB.GetPixel(2, 9)     'Blue minus 4 (hammer)
                Dim bm4 As Color = checkB.GetPixel(2, 10)    'Blue minus 4 (strum)
                Dim isStrum As Boolean = False
                Dim isHammer As Boolean = False

                'Determine if we have a BLUE strum note
                If bp4 = cBlack And bm4 = cBlack And bp3 = cBlue And blt = cBlue And blb = cBlue Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    bnotes += 1
                    notesAdded += 1
                    bPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "B-s-" + msr.ToString + "-" + cbt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    mainForm.lblBcount.Text = bnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY strum note
                If bp4 = cBlack And bm4 = cBlack And bp3 = cGray And blt = cGray And blb = cGray Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    bnotes += 1
                    notesAdded += 1
                    bPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "B-s-" + msr.ToString + "-" + cbt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    mainForm.lblBcount.Text = bnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a BLUE hammer note
                If bp3 = cBlack And bm3 = cBlack And blt = cBlue And blb = cBlue Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    bnotes += 1
                    notesAdded += 1
                    bPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "B-h-" + msr.ToString + "-" + cbt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    mainForm.lblBcount.Text = bnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY hammer note
                If bp3 = cBlack And bm3 = cBlack And blt = cGray And blb = cGray Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    bnotes += 1
                    notesAdded += 1
                    bPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "B-h-" + msr.ToString + "-" + cbt.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    mainForm.lblBcount.Text = bnotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a BLUE hold
                If isStrum = False And isHammer = False And blt = cBlue And bt1 = cBlue And bt2 = cBlue Then
                    If bPressed > 0 Then
                        Dim lastNote() As String = savedNotes(bPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(bPressed) = modNote
                    End If
                End If
                'Determine if we have a GRAY (blue) hold
                If isStrum = False And isHammer = False And blt = cGray And bt1 = cGray And bt2 = cGray Then
                    If bPressed > 0 Then
                        Dim lastNote() As String = savedNotes(bPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(bPressed) = modNote
                    End If
                End If
                checkB.Dispose()    'free up memory
            Next



            ' ---------------------- Check the ORANGE track ---------------------------
            For cot As Integer = 0 To msrImg.Width - 3
                Dim cO As New Rectangle(cot, 48, 3, 11)
                Dim checkO As Bitmap = msrImg.Clone(cO, msrImg.PixelFormat)
                Dim olb As Color = checkO.GetPixel(0, 5)     'Orange left border
                Dim op4 As Color = checkO.GetPixel(2, 0)     'Orange plus 4 (strum)
                Dim op3 As Color = checkO.GetPixel(2, 1)     'Orange plus 3 (hammeron)
                Dim ot As Color = checkO.GetPixel(2, 4)      'track line
                Dim ot1 As Color = checkO.GetPixel(2, 5)     'track line
                Dim ot2 As Color = checkO.GetPixel(2, 6)     'track line
                Dim om3 As Color = checkO.GetPixel(2, 9)     'Orange minus 4 (hammer)
                Dim om4 As Color = checkO.GetPixel(2, 10)    'Orange minus 4 (strum)
                Dim isStrum As Boolean = False
                Dim isHammer As Boolean = False

                'Determine if we have a ORANGE strum note
                If op4 = cBlack And om4 = cBlack And op3 = cOrange And ot = cOrange And olb = cOrange Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    onotes += 1
                    notesAdded += 1
                    oPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "O-s-" + msr.ToString + "-" + cot.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    mainForm.lblOcount.Text = onotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY strum note
                If op4 = cBlack And om4 = cBlack And op3 = cGray And ot = cGray And olb = cGray Then
                    isStrum = True
                    strums += 1
                    rtNotes += 1
                    onotes += 1
                    notesAdded += 1
                    oPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "O-s-" + msr.ToString + "-" + cot.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblStrums.Text = strums.ToString
                    mainForm.lblOcount.Text = onotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a ORANGE hammer note
                If op3 = cBlack And om3 = cBlack And ot = cOrange And olb = cOrange Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    onotes += 1
                    notesAdded += 1
                    oPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "O-h-" + msr.ToString + "-" + cot.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString
                    mainForm.lblOcount.Text = onotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a GRAY hammer note
                If op3 = cBlack And om3 = cBlack And ot = cGray And olb = cGray Then
                    isHammer = True
                    hammers += 1
                    rtNotes += 1
                    onotes += 1
                    notesAdded += 1
                    oPressed = notesAdded
                    'Save the note and update UI
                    ReDim Preserve savedNotes(notesAdded)
                    savedNotes(notesAdded) = "O-h-" + msr.ToString + "-" + cot.ToString + "-3"
                    Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                    actualNotes += 1
                    mainForm.lblScanVal.Text = actualNotes.ToString
                    mainForm.lblHammers.Text = hammers.ToString

                    mainForm.lblOcount.Text = onotes.ToString
                    Application.DoEvents()
                End If
                'Determine if we have a ORANGE hold
                If isStrum = False And isHammer = False And ot = cOrange And ot1 = cOrange And ot2 = cOrange Then
                    If oPressed > 0 Then
                        Dim lastNote() As String = savedNotes(oPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(oPressed) = modNote
                    End If
                End If
                'Determine if we have a GRAY (orange) hold
                If isStrum = False And isHammer = False And ot = cGray And ot1 = cGray And ot2 = cGray Then
                    If oPressed > 0 Then
                        Dim lastNote() As String = savedNotes(oPressed).Split(New Char() {"-"c})
                        Dim lastDuration As Integer = CInt(lastNote(4))
                        lastDuration = lastDuration + 1
                        lastNote(4) = lastDuration.ToString
                        Dim modNote As String = String.Join("-", lastNote)
                        savedNotes(oPressed) = modNote
                    End If
                End If
                checkO.Dispose()    'free up memory
            Next

            'Update UI progress
            progress += progIncDbl
            Dim progInc As Integer = CInt(progress)
            mainForm.iProgressBar.Value = progInc
            mainForm.lblProgress.Text = progInc.ToString + "%"
        Next

        'Fix Inflated Durations
        mainForm.lblProgress.Text = "99%"
        For sn As Integer = 1 To savedNotes.GetUpperBound(0)
            Dim np() As String = savedNotes(sn).Split(New Char() {"-"c})
            Dim nd As Integer = CInt(np(4))
            nd = nd - 2
            If nd < 3 Then
                nd = 3
            End If
            np(4) = nd.ToString
            Dim fixedDuration = String.Join("-", np)
            savedNotes(sn) = fixedDuration
        Next

        'Save temp data and stop scan
        mainForm.chartData.Add(savedNotes)
        Dim nMsr(2) As Integer
        nMsr(0) = nMeasures
        nMsr(1) = savedNotes.Length
        mainForm.chartData.Add(nMsr)
        mainForm.scanRunning = False

        'Update UI
        mainForm.btnScan.Text = "Finished!"
        mainForm.iProgressBar.Value = 100
        mainForm.lblProgress.Text = "100%"
        mainForm.btnEnterBpm.Enabled = True

    End Sub


End Module

