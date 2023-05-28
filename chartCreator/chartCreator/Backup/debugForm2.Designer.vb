<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class debugForm2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.dgcMeasure = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dgcBeat = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dgcLine = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dgcNote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dgcType = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dgcDuration = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(287, 387)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgcMeasure, Me.dgcBeat, Me.dgcLine, Me.dgcNote, Me.dgcType, Me.dgcDuration})
        Me.DataGridView1.Location = New System.Drawing.Point(12, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(421, 368)
        Me.DataGridView1.TabIndex = 1
        '
        'dgcMeasure
        '
        Me.dgcMeasure.HeaderText = "Measure"
        Me.dgcMeasure.Name = "dgcMeasure"
        Me.dgcMeasure.ReadOnly = True
        Me.dgcMeasure.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgcMeasure.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dgcMeasure.Width = 55
        '
        'dgcBeat
        '
        Me.dgcBeat.HeaderText = "Beat"
        Me.dgcBeat.Name = "dgcBeat"
        Me.dgcBeat.ReadOnly = True
        Me.dgcBeat.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgcBeat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dgcBeat.Width = 50
        '
        'dgcLine
        '
        Me.dgcLine.HeaderText = "Line"
        Me.dgcLine.Name = "dgcLine"
        Me.dgcLine.ReadOnly = True
        Me.dgcLine.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgcLine.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dgcLine.Width = 50
        '
        'dgcNote
        '
        Me.dgcNote.HeaderText = "Note"
        Me.dgcNote.Name = "dgcNote"
        Me.dgcNote.Width = 50
        '
        'dgcType
        '
        Me.dgcType.HeaderText = "Type"
        Me.dgcType.Name = "dgcType"
        Me.dgcType.ReadOnly = True
        Me.dgcType.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgcType.Width = 50
        '
        'dgcDuration
        '
        Me.dgcDuration.HeaderText = "Duration"
        Me.dgcDuration.Name = "dgcDuration"
        Me.dgcDuration.ReadOnly = True
        Me.dgcDuration.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        '
        'debugForm2
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(445, 428)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "debugForm2"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Chart Data"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgcMeasure As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcBeat As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcLine As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcNote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcDuration As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
