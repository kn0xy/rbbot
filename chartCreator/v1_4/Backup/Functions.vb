Option Strict On
Public Module Functions


    Public Sub loadImage(ByVal imgFilename As String, ByVal imgSafename As String)
        'Update UI
        mainForm.lblImgFile.Text = imgSafename
        mainForm.OpenChartToolStripMenuItem.Enabled = False
        mainForm.CloseChartToolStripMenuItem.Enabled = True
        mainForm.btnOpenImg.Enabled = False
        mainForm.btnScan.Enabled = True

        'Load the image into memory
        mainForm.chartImage = Image.FromFile(imgFilename)
    End Sub

    Public Sub scanRows()
        'Update UI
        mainForm.btnScan.Enabled = False
        mainForm.btnScan.Text = "Scanning rows..."
        mainForm.lblScanVal.Text = "0"
        mainForm.lblScanVal.Visible = True
        mainForm.lblScanInfo.Text = "rows discovered"
        mainForm.lblScanInfo.Visible = True
        mainForm.lblProgress.Text = "0%"
        Application.DoEvents()

        'Grab the first row marker
        Dim chartImg As Bitmap = CType(mainForm.chartImage, Bitmap)
        Dim rowMarker As Bitmap = My.Resources.rowMarker
        Dim row1 As New Rectangle(20, 60, 10, 14)
        Dim numRows As Integer = 1
        Try
            Dim cropped As Bitmap = chartImg.Clone(row1, chartImg.PixelFormat)
            Dim px1 As Color = cropped.GetPixel(5, 5)
            Dim px2 As Color = rowMarker.GetPixel(5, 5)
            If px1 <> px2 Then
                MessageBox.Show("Not a valid chart image!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            'After the first row is captured, loop at increments of 130px on the Y axis to discover all rows
            Dim searchingForRows As Boolean = True
            Do Until searchingForRows = False
                Dim top As Integer = 130 * numRows
                top += 60                       'Dont forget the first row!
                Dim row As New Rectangle(20, top, 10, 14)
                Try
                    Dim bmp As Bitmap = chartImg.Clone(row, chartImg.PixelFormat)
                    Dim px As Color = bmp.GetPixel(5, 5)
                    If px <> px2 Then
                        searchingForRows = False
                    Else
                        numRows += 1
                    End If
                    'Free memory and update UI
                    bmp.Dispose()
                    mainForm.lblScanVal.Text = numRows.ToString
                    Application.DoEvents()
                Catch ex As Exception
                    searchingForRows = False
                End Try
            Loop
        Catch ex As Exception
            MessageBox.Show("Not a valid chart image!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            mainForm.scanRunning = False
            Application.Restart()
        End Try

        'Update UI and start next step
        Dim numMeasures As Integer = numRows * 4
        numMeasures += 4            'Add an extra row just in case
        mainForm.lblMeasures.Text = numMeasures.ToString
        mainForm.iProgressBar.Value = 5
        mainForm.lblProgress.Text = "5%"
        scanMeasures()
    End Sub



    Public savedMeasures() As Bitmap
    Public Sub scanMeasures()
        'Update UI
        mainForm.btnScan.Text = "Processing data..."
        mainForm.lblScanVal.Text = "0"
        mainForm.lblScanInfo.Text = "of " + mainForm.lblMeasures.Text + " measures"

        'Scan each row
        Dim chartImg As Bitmap = CType(mainForm.chartImage, Bitmap)
        Dim numMeasures As Integer = CInt(mainForm.lblMeasures.Text)
        Dim numRows As Integer = CInt(numMeasures / 4)
        Dim progress As Double = 5          'we are starting at 5%
        Dim progIncDbl As Double = (45 / numMeasures)

        Dim cropsAdded As Integer = 1
        ReDim savedMeasures(numMeasures)
        For r As Integer = 1 To numRows
            For m As Integer = 1 To 4
                'Create crop of current measure
                Dim ry As Integer = 100         'top of measure
                ry -= 5                         'see all the notes on the green track
                If r > 1 Then
                    Dim yc As Integer = 130 * r
                    yc -= 130
                    ry += yc
                End If
                Dim xc As Integer = 240 * m     'end of measure
                Dim rxe As Integer = 24 + xc    'compensate for left padding
                Dim rx As Integer = rxe - 241   'start of measure
                Dim mRect As New Rectangle(rx, ry, 242, 59)
                Try
                    Dim crop As Bitmap = chartImg.Clone(mRect, chartImg.PixelFormat)
                    savedMeasures(cropsAdded) = crop
                Catch ex As Exception
                End Try
                'update UI and internal counter
                mainForm.lblScanVal.Text = cropsAdded.ToString
                cropsAdded += 1
                progress += progIncDbl
                Dim progInc As Integer = CInt(progress)
                mainForm.iProgressBar.Value = progInc
                mainForm.lblProgress.Text = progInc.ToString + "%"
                Application.DoEvents()
            Next
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
        mainForm.btnScan.Text = "Scanning notes..."
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
        Dim gHold As Boolean = False
        Dim rHold As Boolean = False
        Dim yHold As Boolean = False
        Dim bHold As Boolean = False
        Dim oHold As Boolean = False
        Dim ghi As Integer = 0
        Dim rhi As Integer = 0
        Dim yhi As Integer = 0
        Dim bhi As Integer = 0
        Dim ohi As Integer = 0
        Dim ghd As Integer = 0
        Dim rhd As Integer = 0
        Dim yhd As Integer = 0
        Dim bhd As Integer = 0
        Dim ohd As Integer = 0

        'Scan each measure for notes
        Dim nMeasures As Integer = savedMeasures.GetUpperBound(0)
        Dim progIncDbl As Double = 50 / nMeasures
        Dim progress As Double = 50             'starting at 50%
        For msr As Integer = 1 To nMeasures
            Dim msrImg As Bitmap = savedMeasures(msr)
            'Break up the measure into 4 beats
            For bt As Integer = 1 To 4
                'Scan each beat twice (20 times each), at reg/off intervals
                Dim bw As Integer = 60 * bt     'right side of our crop
                Dim bx As Integer = bw - 60     'left side of our crop
                'Regular Timing Scan
                Dim rgLine As Integer = 1
                For rts As Integer = bx + 1 To bw   'produces 20 noteboxes at regular time
                    'Check the GREEN track
                    Dim holdGreen As Boolean = False
                    Dim cG As New Rectangle(rts, 0, 3, 11)
                    Try
                        Dim checkG As Bitmap = msrImg.Clone(cG, msrImg.PixelFormat)
                        Dim glb As Color = checkG.GetPixel(0, 5)     'left border
                        Dim gp4 As Color = checkG.GetPixel(2, 0)     'green plus 4 (strum)
                        Dim gp3 As Color = checkG.GetPixel(2, 1)     'green plus 3 (hammeron)
                        Dim gt As Color = checkG.GetPixel(2, 4)      'track line
                        Dim gt1 As Color = checkG.GetPixel(2, 5)     'track line
                        Dim gt2 As Color = checkG.GetPixel(2, 6)     'track line
                        Dim gm3 As Color = checkG.GetPixel(2, 9)     'green minus 4 (hammer)
                        Dim gm4 As Color = checkG.GetPixel(2, 10)    'green minus 4 (strum)
                        'Determine if we have a GREEN hold note
                        If gm3 <> cGreen And gm3 <> cBlack And gm4 <> cBlack And gt1 = cGreen And gt = cGreen And gt2 = cGreen Then
                            If gHold = False Then
                                'Set the last GREEN note found to a hold
                                ghi = notesAdded                  'green hold index
                                Dim str As String = savedNotes(ghi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "G" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not GREEN, try another note back
                                        ghi = notesAdded - 1
                                        str = savedNotes(ghi)
                                        strData = Split(str, "-")
                                        If strData(0) = "G" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not GREEN, try 3 (final)
                                            ghi = notesAdded - 2
                                            str = savedNotes(ghi)
                                            strData = Split(str, "-")
                                            If strData(0) = "G" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the G track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(ghi) = newstr
                                'Now add the current hold
                                holds += 1
                                ghd += 1        'green hold duration
                                mainForm.lblHolds.Text = holds.ToString
                                holdGreen = True
                                gHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                ghd += 1
                                holdGreen = True
                                gHold = True
                            End If
                            End If
                        'Determine if we have a GRAY hold note
                        If gm3 <> cGray And gm3 <> cBlack And gm4 <> cBlack And gt1 = cGray And gt = cGray And gt2 = cGray Then
                            If gHold = False Then
                                'Set the last GREEN note found to a hold
                                ghi = notesAdded                  'green hold index
                                Dim str As String = savedNotes(ghi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "G" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not GREEN, try another note back
                                        ghi = notesAdded - 1
                                        str = savedNotes(ghi)
                                        strData = Split(str, "-")
                                        If strData(0) = "G" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not GREEN, try 3 (final)
                                            ghi = notesAdded - 2
                                            str = savedNotes(ghi)
                                            strData = Split(str, "-")
                                            If strData(0) = "G" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the G track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(ghi) = newstr
                                'Now add the current hold
                                holds += 1
                                ghd += 1        'green hold duration
                                mainForm.lblHolds.Text = holds.ToString
                                holdGreen = True
                                gHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                ghd += 1
                                holdGreen = True
                                gHold = True
                            End If
                        End If
                        'Determine if we have a GREEN strum note
                        If gp4 = cBlack And gm4 = cBlack And gp3 = cGreen And gt = cGreen And glb = cGreen Then
                            strums += 1
                            rtNotes += 1
                            gnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "G-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblGcount.Text = gnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY strum note
                        If gp4 = cBlack And gm4 = cBlack And gp3 = cGray And gt = cGray And glb = cGray Then
                            strums += 1
                            rtNotes += 1
                            gnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "G-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblGcount.Text = gnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GREEN hammer note
                        If gp3 = cBlack And gm3 = cBlack And gt = cGreen And glb = cGreen Then
                            hammers += 1
                            rtNotes += 1
                            gnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "G-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblGcount.Text = gnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY hammer note
                        If gp3 = cBlack And gm3 = cBlack And gt = cGray And glb = cGray Then
                            hammers += 1
                            rtNotes += 1
                            gnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "G-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblGcount.Text = gnotes.ToString
                            Application.DoEvents()
                        End If
                        If holdGreen = False And gHold = True Then
                            'No hold was detected on this line, so mark the end of the hold and turn this off
                            Dim noteData() As String = Split(savedNotes(ghi), "-")
                            Dim overStr As String = noteData(0) + "-" + noteData(1) + "-"
                            overStr += noteData(2) + "-" + noteData(3) + "-"
                            overStr += noteData(4) + "-" + ghd.ToString
                            savedNotes(ghi) = overStr   'Overwrite duration
                            gHold = False               'Finalize the hold
                            ghd = 0
                            ghi = 0
                        End If
                        checkG.Dispose()    'free up memory
                    Catch ex As Exception
                        Dim str As String = "Error occurred on G track at measure " + msr.ToString + ", beat " + bt.ToString + ", line " + rgLine.ToString
                    End Try


                    'Check the RED track

                    Dim holdRed As Boolean = False
                    Dim cR As New Rectangle(rts, 12, 3, 11)
                    Try
                        Dim checkR As Bitmap = msrImg.Clone(cR, msrImg.PixelFormat)
                        Dim rlb As Color = checkR.GetPixel(0, 5)      'red left border
                        Dim rp4 As Color = checkR.GetPixel(2, 0)     'red plus 4 (strum)
                        Dim rp3 As Color = checkR.GetPixel(2, 1)     'red plus 3 (hammeron)
                        Dim rt As Color = checkR.GetPixel(2, 4)      'track line
                        Dim rt1 As Color = checkR.GetPixel(2, 5)     'track line
                        Dim rt2 As Color = checkR.GetPixel(2, 6)     'track line
                        Dim rm3 As Color = checkR.GetPixel(2, 9)     'red minus 4 (hammer)
                        Dim rm4 As Color = checkR.GetPixel(2, 10)    'red minus 4 (strum)

                        'Determine if we have a RED hold note

                        If rm3 <> cRed And rm3 <> cBlack And rm4 <> cBlack And rt1 = cRed And rt = cRed And rt2 = cRed Then

                            If rHold = False Then
                                rhi = notesAdded                  'red hold index
                                'Set the last RED note found to a hold
                                Dim str As String = savedNotes(rhi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "R" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not RED, try another note back
                                        rhi = notesAdded - 1
                                        str = savedNotes(rhi)
                                        strData = Split(str, "-")
                                        If strData(0) = "R" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not RED, try 3 (final)
                                            rhi = notesAdded - 2
                                            str = savedNotes(rhi)
                                            strData = Split(str, "-")
                                            If strData(0) = "R" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the R track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(rhi) = newstr
                                'Now add the current hold
                                holds += 1
                                rhd += 1        'red hold duration
                                mainForm.lblHolds.Text = holds.ToString
                                holdRed = True
                                rHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                rhd += 1
                                holdRed = True
                                rHold = True
                            End If
                        End If
                        'Determine if we have a GRAY hold note
                        If rm3 <> cGray And rm3 <> cBlack And rm4 <> cBlack And rt1 = cGray And rt = cGray And rt2 = cGray Then
                            If rHold = False Then
                                'Set the last RED note found to a hold
                                rhi = notesAdded                  'red hold index
                                Dim str As String = savedNotes(rhi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "R" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not RED, try another note back
                                        rhi = notesAdded - 1
                                        str = savedNotes(rhi)
                                        strData = Split(str, "-")
                                        If strData(0) = "R" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not RED, try 3 (final)
                                            rhi = notesAdded - 2
                                            str = savedNotes(rhi)
                                            strData = Split(str, "-")
                                            If strData(0) = "R" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the R track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(notesAdded) = newstr
                                'Now add the current hold
                                holds += 1
                                rhd += 1            'red hold duration
                                mainForm.lblHolds.Text = holds.ToString
                                holdRed = True
                                rHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                rhd += 1
                                holdRed = True
                                rHold = True
                            End If
                        End If
                        'Determine if we have a RED strum note
                        If rp4 = cBlack And rm4 = cBlack And rp3 = cRed And rt = cRed And rlb = cRed Then
                            strums += 1
                            rtNotes += 1
                            rnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "R-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblRcount.Text = rnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY strum note
                        If rp4 = cBlack And rm4 = cBlack And rp3 = cGray And rt = cGray And rlb = cGray Then
                            strums += 1
                            rtNotes += 1
                            rnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "R-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblRcount.Text = rnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a RED hammer note
                        If rp3 = cBlack And rm3 = cBlack And rt = cRed And rlb = cRed Then
                            hammers += 1
                            rtNotes += 1
                            rnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "R-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblRcount.Text = rnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY hammer note
                        If rp3 = cBlack And rm3 = cBlack And rt = cGray And rlb = cGray Then
                            hammers += 1
                            rtNotes += 1
                            rnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "R-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblRcount.Text = rnotes.ToString
                            Application.DoEvents()
                        End If
                        If holdRed = False And rHold = True Then
                            'No hold was detected on this line, so mark the end of the hold and turn this off
                            Dim noteData() As String = Split(savedNotes(rhi), "-")
                            Dim overStr As String = noteData(0) + "-" + noteData(1) + "-"
                            overStr += noteData(2) + "-" + noteData(3) + "-"
                            overStr += noteData(4) + "-" + rhd.ToString
                            savedNotes(rhi) = overStr   'Overwrite duration
                            rHold = False               'Finalize the hold
                            rhd = 0
                            rhi = 0
                        End If
                        checkR.Dispose()    'free up memory
                    Catch ex As Exception
                        Dim str As String = "Error occurred on R track at measure " + msr.ToString + ", beat " + bt.ToString + ", line " + rgLine.ToString
                    End Try


                    'Check the YELLOW track
                    Dim holdYellow As Boolean = False
                    Dim cY As New Rectangle(rts, 24, 3, 11)
                    Try
                        Dim checkY As Bitmap = msrImg.Clone(cY, msrImg.PixelFormat)
                        Dim ylb As Color = checkY.GetPixel(0, 5)     'Yellow left border
                        Dim yp4 As Color = checkY.GetPixel(2, 0)     'Yellow plus 4 (strum)
                        Dim yp3 As Color = checkY.GetPixel(2, 1)     'Yellow plus 3 (hammeron)
                        Dim yt As Color = checkY.GetPixel(2, 4)      'track line
                        Dim yt1 As Color = checkY.GetPixel(2, 5)     'track line
                        Dim yt2 As Color = checkY.GetPixel(2, 6)     'track line
                        Dim ym3 As Color = checkY.GetPixel(2, 9)     'Yellow minus 4 (hammer)
                        Dim ym4 As Color = checkY.GetPixel(2, 10)    'Yellow minus 4 (strum)
                        'Determine if we have a YELLOW hold note
                        If ym3 <> cYellow And ym3 <> cBlack And ym4 <> cBlack And yt1 = cYellow And yt = cYellow And yt2 = cYellow Then
                            If yHold = False Then
                                'Set the last YELLOW note found to a hold
                                yhi = notesAdded                  'yellow hold index
                                Dim str As String = savedNotes(yhi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "Y" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not YELLOW, try another note back
                                        yhi = notesAdded - 1
                                        str = savedNotes(yhi)
                                        strData = Split(str, "-")
                                        If strData(0) = "Y" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not YELLOW, try 3 (final)
                                            yhi = notesAdded - 2
                                            str = savedNotes(yhi)
                                            strData = Split(str, "-")
                                            If strData(0) = "Y" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the Y track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(yhi) = newstr
                                'Now track the current hold
                                holds += 1
                                yhd += 1
                                mainForm.lblHolds.Text = holds.ToString
                                holdYellow = True
                                yHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                yhd += 1
                                holdYellow = True
                                yHold = True
                            End If
                        End If
                        'Determine if we have a GRAY hold note
                        If ym3 <> cGray And ym3 <> cBlack And ym4 <> cBlack And yt1 = cGray And yt = cGray And yt2 = cGray Then
                            If yHold = False Then
                                'Set the last YELLOW note found to a hold
                                yhi = notesAdded                  'yellow hold index
                                Dim str As String = savedNotes(yhi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "Y" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not YELLOW, try another note back
                                        yhi = notesAdded - 1
                                        str = savedNotes(yhi)
                                        strData = Split(str, "-")
                                        If strData(0) = "Y" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not YELLOW, try 3 (final)
                                            yhi = notesAdded - 2
                                            str = savedNotes(yhi)
                                            strData = Split(str, "-")
                                            If strData(0) = "Y" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the Y track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(yhi) = newstr
                                'Now add the current hold
                                holds += 1
                                yhd += 1
                                mainForm.lblHolds.Text = holds.ToString
                                holdYellow = True
                                yHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                yhd += 1
                                holdYellow = True
                                yHold = True
                            End If
                        End If
                        'Determine if we have a YELLOW strum note
                        If yp4 = cBlack And ym4 = cBlack And yp3 = cYellow And yt = cYellow And ylb = cYellow Then
                            strums += 1
                            rtNotes += 1
                            ynotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "Y-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblYcount.Text = ynotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY strum note
                        If yp4 = cBlack And ym4 = cBlack And yp3 = cGray And yt = cGray And ylb = cGray Then
                            strums += 1
                            rtNotes += 1
                            ynotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "Y-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblYcount.Text = ynotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a YELLOW hammer note
                        If yp3 = cBlack And ym3 = cBlack And yt = cYellow And ylb = cYellow Then
                            hammers += 1
                            rtNotes += 1
                            ynotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "Y-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblYcount.Text = ynotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY hammer note
                        If yp3 = cBlack And ym3 = cBlack And yt = cGray And ylb = cGray Then
                            hammers += 1
                            rtNotes += 1
                            ynotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "Y-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblYcount.Text = ynotes.ToString
                            Application.DoEvents()
                        End If
                        If holdYellow = False And yHold = True Then
                            'No hold was detected on this line, so mark the end of the hold and turn this off
                            Dim noteData() As String = Split(savedNotes(yhi), "-")
                            Dim overStr As String = noteData(0) + "-" + noteData(1) + "-"
                            overStr += noteData(2) + "-" + noteData(3) + "-"
                            overStr += noteData(4) + "-" + yhd.ToString
                            savedNotes(yhi) = overStr   'Overwrite duration
                            yHold = False               'Finalize the hold
                            yhd = 0
                            yhi = 0
                        End If
                        checkY.Dispose()    'free up memory
                    Catch ex As Exception
                        Dim str As String = "Error occurred on Y track at measure " + msr.ToString + ", beat " + bt.ToString + ", line " + rgLine.ToString
                    End Try


                    'Check the BLUE track
                    Dim holdBlue As Boolean = False
                    Dim cB As New Rectangle(rts, 36, 3, 11)
                    Try
                        Dim checkB As Bitmap = msrImg.Clone(cB, msrImg.PixelFormat)
                        Dim blb As Color = checkB.GetPixel(0, 5)     'Blue left border
                        Dim bp4 As Color = checkB.GetPixel(2, 0)     'Blue plus 4 (strum)
                        Dim bp3 As Color = checkB.GetPixel(2, 1)     'Blue plus 3 (hammeron)
                        Dim blt As Color = checkB.GetPixel(2, 4)     'track line
                        Dim bt1 As Color = checkB.GetPixel(2, 5)     'track line
                        Dim bt2 As Color = checkB.GetPixel(2, 6)     'track line
                        Dim bm3 As Color = checkB.GetPixel(2, 9)     'Blue minus 4 (hammer)
                        Dim bm4 As Color = checkB.GetPixel(2, 10)    'Blue minus 4 (strum)
                        'Determine if we have a BLUE hold note
                        If bm3 <> cBlue And bm3 <> cBlack And bm4 <> cBlack And bt1 = cBlue And blt = cBlue And bt2 = cBlue Then
                            If bHold = False Then
                                'Set the last BLUE note found to a hold
                                bhi = notesAdded                  'blue hold index
                                Dim str As String = savedNotes(bhi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "B" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not BLUE, try another note back
                                        bhi = notesAdded - 1
                                        str = savedNotes(bhi)
                                        strData = Split(str, "-")
                                        If strData(0) = "B" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not BLUE, try 3 (final)
                                            bhi = notesAdded - 2
                                            str = savedNotes(bhi)
                                            strData = Split(str, "-")
                                            If strData(0) = "B" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the B track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(bhi) = newstr
                                'Now track the current hold
                                holds += 1
                                bhd += 1
                                mainForm.lblHolds.Text = holds.ToString
                                holdBlue = True
                                bHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                bhd += 1
                                holdBlue = True
                                bHold = True
                            End If
                        End If
                        'Determine if we have a GRAY hold note
                        If bm3 <> cGray And bm3 <> cBlack And bm4 <> cBlack And bt1 = cGray And blt = cGray And bt2 = cGray Then
                            If bHold = False Then
                                'Set the last BLUE note found to a hold
                                bhi = notesAdded                  'blue hold index
                                Dim str As String = savedNotes(bhi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "B" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not BLUE, try another note back
                                        bhi = notesAdded - 1
                                        str = savedNotes(bhi)
                                        strData = Split(str, "-")
                                        If strData(0) = "B" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not BLUE, try 3 (final)
                                            bhi = notesAdded - 2
                                            str = savedNotes(bhi)
                                            strData = Split(str, "-")
                                            If strData(0) = "B" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the B track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(bhi) = newstr
                                'Now add the current hold
                                holds += 1
                                bhd += 1
                                mainForm.lblHolds.Text = holds.ToString
                                holdBlue = True
                                bHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                bhd += 1
                                holdBlue = True
                                bHold = True
                            End If
                        End If
                        'Determine if we have a BLUE strum note
                        If bp4 = cBlack And bm4 = cBlack And bp3 = cBlue And blt = cBlue And blb = cBlue Then
                            strums += 1
                            rtNotes += 1
                            bnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "B-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblBcount.Text = bnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY strum note
                        If bp4 = cBlack And bm4 = cBlack And bp3 = cGray And blt = cGray And blb = cGray Then
                            strums += 1
                            rtNotes += 1
                            bnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "B-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblBcount.Text = bnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a BLUE hammer note
                        If bp3 = cBlack And bm3 = cBlack And blt = cBlue And blb = cBlue Then
                            hammers += 1
                            rtNotes += 1
                            bnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "B-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblBcount.Text = bnotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY hammer note
                        If bp3 = cBlack And bm3 = cBlack And blt = cGray And blb = cGray Then
                            hammers += 1
                            rtNotes += 1
                            bnotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "B-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblBcount.Text = bnotes.ToString
                            Application.DoEvents()
                        End If
                        If holdBlue = False And bHold = True Then
                            'No hold was detected on this line, so mark the end of the hold and turn this off
                            Dim noteData() As String = Split(savedNotes(bhi), "-")
                            Dim overStr As String = noteData(0) + "-" + noteData(1) + "-"
                            overStr += noteData(2) + "-" + noteData(3) + "-"
                            overStr += noteData(4) + "-" + bhd.ToString
                            savedNotes(bhi) = overStr   'Overwrite duration
                            bHold = False               'Finalize the hold
                            bhd = 0
                            bhi = 0
                        End If
                        checkB.Dispose()    'free up memory
                    Catch ex As Exception
                        Dim str As String = "Error occurred on B track at measure " + msr.ToString + ", beat " + bt.ToString + ", line " + rgLine.ToString
                    End Try


                    'Check the ORANGE track
                    Dim holdOrange As Boolean = False
                    Dim cO As New Rectangle(rts, 48, 3, 11)
                    Try
                        Dim checkO As Bitmap = msrImg.Clone(cO, msrImg.PixelFormat)
                        Dim olb As Color = checkO.GetPixel(0, 5)     'Orange left border
                        Dim op4 As Color = checkO.GetPixel(2, 0)     'Orange plus 4 (strum)
                        Dim op3 As Color = checkO.GetPixel(2, 1)     'Orange plus 3 (hammeron)
                        Dim ot As Color = checkO.GetPixel(2, 4)      'track line
                        Dim ot1 As Color = checkO.GetPixel(2, 5)     'track line
                        Dim ot2 As Color = checkO.GetPixel(2, 6)     'track line
                        Dim om3 As Color = checkO.GetPixel(2, 9)     'Orange minus 4 (hammer)
                        Dim om4 As Color = checkO.GetPixel(2, 10)    'Orange minus 4 (strum)
                        'Determine if we have a ORANGE hold note
                        If op3 <> cOrange And op3 <> cBlack And op4 <> cBlack And ot1 = cOrange And ot = cOrange And ot2 = cOrange Then
                            If oHold = False Then
                                'Set the last ORANGE note found to a hold
                                ohi = notesAdded                  'orange hold index
                                Dim str As String = savedNotes(ohi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "O" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not ORANGE, try another note back
                                        ohi = notesAdded - 1
                                        str = savedNotes(ohi)
                                        strData = Split(str, "-")
                                        If strData(0) = "O" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not ORANGE, try 3 (final)
                                            ohi = notesAdded - 2
                                            str = savedNotes(ohi)
                                            strData = Split(str, "-")
                                            If strData(0) = "O" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the O track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(ohi) = newstr
                                'Now track the current hold
                                holds += 1
                                ohd += 1
                                mainForm.lblHolds.Text = holds.ToString
                                holdOrange = True
                                oHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                ohd += 1
                                holdOrange = True
                                oHold = True
                            End If
                        End If
                        'Determine if we have a GRAY hold note
                        If op3 <> cGray And op3 <> cBlack And op4 <> cBlack And ot1 = cGray And ot = cGray And ot2 = cGray Then
                            If oHold = False Then
                                'Set the last ORANGE note found to a hold
                                ohi = notesAdded                  'orange hold index
                                Dim str As String = savedNotes(ohi)
                                Dim strData() As String = Split(str, "-")
                                Try
                                    If strData(0) = "O" Then
                                        strData(1) += "h"
                                        Exit Try
                                    Else
                                        'the last note was not ORANGE, try another note back
                                        ohi = notesAdded - 1
                                        str = savedNotes(ohi)
                                        strData = Split(str, "-")
                                        If strData(0) = "O" Then
                                            strData(1) += "h"
                                            Exit Try
                                        Else
                                            '2 notes back was not ORANGE, try 3 (final)
                                            ohi = notesAdded - 2
                                            str = savedNotes(ohi)
                                            strData = Split(str, "-")
                                            If strData(0) = "O" Then
                                                strData(1) += "h"
                                                Exit Try
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("An error occurred registering a hold note on the O track")
                                End Try
                                Dim newstr As String = strData(0) + "-"
                                newstr += strData(1) + "-"
                                newstr += strData(2) + "-"
                                newstr += strData(3) + "-"
                                newstr += strData(4) + "-" + strData(5)
                                savedNotes(ohi) = newstr
                                'Now add the current hold
                                holds += 1
                                ohd += 1
                                mainForm.lblHolds.Text = holds.ToString
                                holdOrange = True
                                oHold = True
                                Application.DoEvents()
                            Else
                                'Continue the hold
                                ohd += 1
                                holdOrange = True
                                oHold = True
                            End If
                        End If
                        'Determine if we have a ORANGE strum note
                        If op4 = cBlack And om4 = cBlack And op3 = cOrange And ot = cOrange And olb = cOrange Then
                            strums += 1
                            rtNotes += 1
                            onotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "O-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblOcount.Text = onotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY strum note
                        If op4 = cBlack And om4 = cBlack And op3 = cGray And ot = cGray And olb = cGray Then
                            strums += 1
                            rtNotes += 1
                            onotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "O-s-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblStrums.Text = strums.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblOcount.Text = onotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a ORANGE hammer note
                        If op3 = cBlack And om3 = cBlack And ot = cOrange And olb = cOrange Then
                            hammers += 1
                            rtNotes += 1
                            onotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "O-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblOcount.Text = onotes.ToString
                            Application.DoEvents()
                        End If
                        'Determine if we have a GRAY hammer note
                        If op3 = cBlack And om3 = cBlack And ot = cGray And olb = cGray Then
                            hammers += 1
                            rtNotes += 1
                            onotes += 1
                            notesAdded += 1
                            'Save the note and update UI
                            ReDim Preserve savedNotes(notesAdded)
                            savedNotes(notesAdded) = "O-h-" + msr.ToString + "-" + bt.ToString + "-" + rgLine.ToString + "-1"
                            Dim actualNotes As Integer = CInt(mainForm.lblScanVal.Text)
                            actualNotes += 1
                            mainForm.lblScanVal.Text = actualNotes.ToString
                            mainForm.lblHammers.Text = hammers.ToString
                            mainForm.lbl16notes.Text = rtNotes.ToString
                            mainForm.lblOcount.Text = onotes.ToString
                            Application.DoEvents()
                        End If
                        If holdOrange = False And oHold = True Then
                            'No hold was detected on this line, so mark the end of the hold and turn this off
                            Dim noteData() As String = Split(savedNotes(ohi), "-")
                            Dim overStr As String = noteData(0) + "-" + noteData(1) + "-"
                            overStr += noteData(2) + "-" + noteData(3) + "-"
                            overStr += noteData(4) + "-" + ohd.ToString
                            savedNotes(ohi) = overStr   'Overwrite duration
                            oHold = False               'Finalize the hold
                            ohd = 0
                            ohi = 0
                        End If
                        checkO.Dispose()    'free up memory
                    Catch ex As Exception
                        Dim str As String = "Error occurred on O track at measure " + msr.ToString + ", beat " + bt.ToString + ", line " + rgLine.ToString
                    End Try



                    rts += 2            'skip to the next notebox
                    rgLine += 1         'regular timing scanLine index
                Next
                'off timing = bx
            Next
            'Update UI progress
            progress += progIncDbl
            Dim progInc As Integer = CInt(progress)
            mainForm.iProgressBar.Value = progInc
            mainForm.lblProgress.Text = progInc.ToString + "%"
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
        mainForm.btnDetails.Enabled = True
        mainForm.btnSave.Enabled = True

    End Sub


End Module

