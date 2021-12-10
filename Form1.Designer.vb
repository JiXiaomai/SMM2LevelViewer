<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.TextBox9 = New System.Windows.Forms.TextBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PicBot = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.PicM0 = New System.Windows.Forms.PictureBox()
        Me.PicM1 = New System.Windows.Forms.PictureBox()
        Me.PicM2 = New System.Windows.Forms.PictureBox()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicBot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicM0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicM1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicM2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(6, 20)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(70, 21)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "LOAD"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(82, 19)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(327, 21)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.Text = "E:\VB\SMM2VIEWER\bin\Debug\MAP\Course_data_000"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(6, 19)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(173, 300)
        Me.TextBox2.TabIndex = 2
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(185, 20)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(173, 300)
        Me.TextBox3.TabIndex = 8
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(364, 20)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(173, 300)
        Me.TextBox4.TabIndex = 9
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 39)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(100, 21)
        Me.Button2.TabIndex = 19
        Me.Button2.Text = "Map A"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.TextBox4)
        Me.GroupBox1.Location = New System.Drawing.Point(522, 66)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(549, 327)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        Me.GroupBox1.Visible = False
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(260, 39)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(181, 21)
        Me.Button4.TabIndex = 23
        Me.Button4.Text = "保存地图 Save image"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'TextBox9
        '
        Me.TextBox9.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TextBox9.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox9.Location = New System.Drawing.Point(12, 12)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New System.Drawing.Size(100, 21)
        Me.TextBox9.TabIndex = 24
        Me.TextBox9.Text = "SQG-9NT-9GF"
        Me.TextBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TextBox9.WordWrap = False
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(118, 12)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(136, 21)
        Me.Button5.TabIndex = 25
        Me.Button5.Text = "加载地图 Load Level"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Controls.Add(Me.TextBox1)
        Me.GroupBox2.Location = New System.Drawing.Point(522, 8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(497, 52)
        Me.GroupBox2.TabIndex = 27
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "test"
        Me.GroupBox2.Visible = False
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(118, 39)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(100, 21)
        Me.Button8.TabIndex = 36
        Me.Button8.Text = "Map B"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'TrackBar1
        '
        Me.TrackBar1.Location = New System.Drawing.Point(260, 12)
        Me.TrackBar1.Maximum = 6
        Me.TrackBar1.Minimum = 2
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(104, 45)
        Me.TrackBar1.TabIndex = 39
        Me.TrackBar1.Value = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(370, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 12)
        Me.Label1.TabIndex = 40
        Me.Label1.Text = "缩放Zoom:16"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 12
        Me.ListBox1.Location = New System.Drawing.Point(12, 66)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(100, 208)
        Me.ListBox1.TabIndex = 42
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.ItemHeight = 12
        Me.ListBox2.Location = New System.Drawing.Point(118, 66)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(100, 208)
        Me.ListBox2.TabIndex = 43
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(229, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 12)
        Me.Label2.TabIndex = 48
        Me.Label2.Text = "LvlInfo"
        '
        'PicBot
        '
        Me.PicBot.BackColor = System.Drawing.Color.LightGray
        Me.PicBot.Location = New System.Drawing.Point(12, 448)
        Me.PicBot.Name = "PicBot"
        Me.PicBot.Size = New System.Drawing.Size(100, 100)
        Me.PicBot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PicBot.TabIndex = 50
        Me.PicBot.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 681)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 12)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "LvlInfo"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(146, 681)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 12)
        Me.Label4.TabIndex = 52
        Me.Label4.Text = "LvlInfo"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(280, 681)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 12)
        Me.Label5.TabIndex = 53
        Me.Label5.Text = "LvlInfo"
        '
        'PicM0
        '
        Me.PicM0.BackColor = System.Drawing.Color.Transparent
        Me.PicM0.Location = New System.Drawing.Point(14, 550)
        Me.PicM0.Name = "PicM0"
        Me.PicM0.Size = New System.Drawing.Size(128, 128)
        Me.PicM0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicM0.TabIndex = 55
        Me.PicM0.TabStop = False
        '
        'PicM1
        '
        Me.PicM1.BackColor = System.Drawing.Color.Transparent
        Me.PicM1.Location = New System.Drawing.Point(148, 550)
        Me.PicM1.Name = "PicM1"
        Me.PicM1.Size = New System.Drawing.Size(128, 128)
        Me.PicM1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicM1.TabIndex = 56
        Me.PicM1.TabStop = False
        '
        'PicM2
        '
        Me.PicM2.BackColor = System.Drawing.Color.Transparent
        Me.PicM2.Location = New System.Drawing.Point(282, 550)
        Me.PicM2.Name = "PicM2"
        Me.PicM2.Size = New System.Drawing.Size(128, 128)
        Me.PicM2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicM2.TabIndex = 57
        Me.PicM2.TabStop = False
        '
        'Timer2
        '
        Me.Timer2.Interval = 60
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(453, 284)
        Me.Controls.Add(Me.PicM2)
        Me.Controls.Add(Me.PicM1)
        Me.Controls.Add(Me.PicM0)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PicBot)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox9)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "马里奥制造2关卡机器人 SMM2 Level Viewer v1.0"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicBot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicM0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicM1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicM2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button4 As Button
    Friend WithEvents TextBox9 As TextBox
    Friend WithEvents Button5 As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Button8 As Button
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents Label1 As Label
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents Label2 As Label
    Friend WithEvents PicBot As PictureBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents PicM0 As PictureBox
    Friend WithEvents PicM1 As PictureBox
    Friend WithEvents PicM2 As PictureBox
    Friend WithEvents Timer2 As Timer
End Class
