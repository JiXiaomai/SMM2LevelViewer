<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.P = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Card = New MaterialSkin.Controls.MaterialCard()
        Me.Pic = New System.Windows.Forms.PictureBox()
        CType(Me.P, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Card.SuspendLayout()
        CType(Me.Pic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'P
        '
        Me.P.Location = New System.Drawing.Point(12, 12)
        Me.P.Name = "P"
        Me.P.Size = New System.Drawing.Size(360, 338)
        Me.P.TabIndex = 0
        Me.P.TabStop = False
        '
        'ToolTip1
        '
        '
        'Card
        '
        Me.Card.AutoSize = True
        Me.Card.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Card.Controls.Add(Me.Pic)
        Me.Card.Depth = 0
        Me.Card.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Card.Location = New System.Drawing.Point(433, 23)
        Me.Card.Margin = New System.Windows.Forms.Padding(14)
        Me.Card.MouseState = MaterialSkin.MouseState.HOVER
        Me.Card.Name = "Card"
        Me.Card.Padding = New System.Windows.Forms.Padding(14)
        Me.Card.Size = New System.Drawing.Size(167, 134)
        Me.Card.TabIndex = 1
        '
        'Pic
        '
        Me.Pic.Location = New System.Drawing.Point(0, 0)
        Me.Pic.Name = "Pic"
        Me.Pic.Size = New System.Drawing.Size(80, 80)
        Me.Pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Pic.TabIndex = 0
        Me.Pic.TabStop = False
        '
        'Form3
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(767, 537)
        Me.Controls.Add(Me.Card)
        Me.Controls.Add(Me.P)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "Form3"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "里世界"
        CType(Me.P, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Card.ResumeLayout(False)
        Me.Card.PerformLayout()
        CType(Me.Pic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Card As MaterialSkin.Controls.MaterialCard
    Friend WithEvents Pic As PictureBox
    Friend WithEvents P As PictureBox
End Class
