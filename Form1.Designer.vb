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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
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
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PS = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Button2 = New MaterialSkin.Controls.MaterialButton()
        Me.Button8 = New MaterialSkin.Controls.MaterialButton()
        Me.TextBox9 = New MaterialSkin.Controls.MaterialTextBox2()
        Me.Button5 = New MaterialSkin.Controls.MaterialButton()
        Me.Button4 = New MaterialSkin.Controls.MaterialButton()
        Me.MaterialTextBox21 = New MaterialSkin.Controls.MaterialTextBox2()
        Me.Button3 = New MaterialSkin.Controls.MaterialButton()
        Me.MaterialComboBox1 = New MaterialSkin.Controls.MaterialComboBox()
        Me.TxtZoom = New MaterialSkin.Controls.MaterialTextBox2()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicBot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicM0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicM1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicM2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PS, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TextBox1.Size = New System.Drawing.Size(455, 21)
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
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.TextBox4)
        Me.GroupBox1.Location = New System.Drawing.Point(950, 66)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(549, 327)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        Me.GroupBox1.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Controls.Add(Me.TextBox1)
        Me.GroupBox2.Location = New System.Drawing.Point(950, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(549, 52)
        Me.GroupBox2.TabIndex = 27
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "test"
        Me.GroupBox2.Visible = False
        '
        'TrackBar1
        '
        Me.TrackBar1.Location = New System.Drawing.Point(381, 93)
        Me.TrackBar1.Maximum = 6
        Me.TrackBar1.Minimum = 2
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(104, 45)
        Me.TrackBar1.TabIndex = 39
        Me.TrackBar1.Value = 4
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 12
        Me.ListBox1.Location = New System.Drawing.Point(5, 96)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(181, 196)
        Me.ListBox1.TabIndex = 42
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.ItemHeight = 12
        Me.ListBox2.Location = New System.Drawing.Point(192, 96)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(181, 196)
        Me.ListBox2.TabIndex = 43
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(378, 141)
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
        'Timer1
        '
        '
        'PS
        '
        Me.PS.Enabled = False
        Me.PS.Location = New System.Drawing.Point(5, 365)
        Me.PS.Name = "PS"
        Me.PS.Size = New System.Drawing.Size(300, 50)
        Me.PS.TabIndex = 62
        Me.PS.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(544, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 12)
        Me.Label6.TabIndex = 65
        Me.Label6.Text = "DecData"
        '
        'Button2
        '
        Me.Button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.[Default]
        Me.Button2.Depth = 0
        Me.Button2.HighEmphasis = True
        Me.Button2.Icon = Nothing
        Me.Button2.Location = New System.Drawing.Point(5, 51)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Button2.MouseState = MaterialSkin.MouseState.HOVER
        Me.Button2.Name = "Button2"
        Me.Button2.NoAccentTextColor = System.Drawing.Color.Empty
        Me.Button2.Size = New System.Drawing.Size(69, 36)
        Me.Button2.TabIndex = 66
        Me.Button2.Text = "表世界"
        Me.Button2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined
        Me.Button2.UseAccentColor = False
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button8.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.[Default]
        Me.Button8.Depth = 0
        Me.Button8.HighEmphasis = True
        Me.Button8.Icon = Nothing
        Me.Button8.Location = New System.Drawing.Point(192, 51)
        Me.Button8.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Button8.MouseState = MaterialSkin.MouseState.HOVER
        Me.Button8.Name = "Button8"
        Me.Button8.NoAccentTextColor = System.Drawing.Color.Empty
        Me.Button8.Size = New System.Drawing.Size(69, 36)
        Me.Button8.TabIndex = 67
        Me.Button8.Text = "里世界"
        Me.Button8.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined
        Me.Button8.UseAccentColor = False
        Me.Button8.UseVisualStyleBackColor = True
        '
        'TextBox9
        '
        Me.TextBox9.AnimateReadOnly = False
        Me.TextBox9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.TextBox9.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.TextBox9.Depth = 0
        Me.TextBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.TextBox9.HideSelection = True
        Me.TextBox9.LeadingIcon = Nothing
        Me.TextBox9.Location = New System.Drawing.Point(192, 6)
        Me.TextBox9.MaxLength = 32767
        Me.TextBox9.MouseState = MaterialSkin.MouseState.OUT
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.TextBox9.PrefixSuffix = MaterialSkin.Controls.MaterialTextBox2.PrefixSuffixTypes.Prefix
        Me.TextBox9.PrefixSuffixText = "ID"
        Me.TextBox9.ReadOnly = False
        Me.TextBox9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox9.SelectedText = ""
        Me.TextBox9.SelectionLength = 0
        Me.TextBox9.SelectionStart = 0
        Me.TextBox9.ShortcutsEnabled = True
        Me.TextBox9.Size = New System.Drawing.Size(181, 36)
        Me.TextBox9.TabIndex = 68
        Me.TextBox9.TabStop = False
        Me.TextBox9.Text = "SQG-9NT-9GF"
        Me.TextBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TextBox9.TrailingIcon = Nothing
        Me.TextBox9.UseSystemPasswordChar = False
        Me.TextBox9.UseTallSize = False
        '
        'Button5
        '
        Me.Button5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button5.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.[Default]
        Me.Button5.Depth = 0
        Me.Button5.HighEmphasis = True
        Me.Button5.Icon = Nothing
        Me.Button5.Location = New System.Drawing.Point(380, 6)
        Me.Button5.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Button5.MouseState = MaterialSkin.MouseState.HOVER
        Me.Button5.Name = "Button5"
        Me.Button5.NoAccentTextColor = System.Drawing.Color.Empty
        Me.Button5.Size = New System.Drawing.Size(64, 36)
        Me.Button5.TabIndex = 69
        Me.Button5.Text = "加载"
        Me.Button5.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained
        Me.Button5.UseAccentColor = False
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button4.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.[Default]
        Me.Button4.Depth = 0
        Me.Button4.HighEmphasis = True
        Me.Button4.Icon = Nothing
        Me.Button4.Location = New System.Drawing.Point(452, 6)
        Me.Button4.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Button4.MouseState = MaterialSkin.MouseState.HOVER
        Me.Button4.Name = "Button4"
        Me.Button4.NoAccentTextColor = System.Drawing.Color.Empty
        Me.Button4.Size = New System.Drawing.Size(85, 36)
        Me.Button4.TabIndex = 70
        Me.Button4.Text = "保存图片"
        Me.Button4.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained
        Me.Button4.UseAccentColor = False
        Me.Button4.UseVisualStyleBackColor = True
        '
        'MaterialTextBox21
        '
        Me.MaterialTextBox21.AnimateReadOnly = False
        Me.MaterialTextBox21.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.MaterialTextBox21.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.MaterialTextBox21.Depth = 0
        Me.MaterialTextBox21.Enabled = False
        Me.MaterialTextBox21.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.MaterialTextBox21.HideSelection = True
        Me.MaterialTextBox21.LeadingIcon = Nothing
        Me.MaterialTextBox21.Location = New System.Drawing.Point(5, 323)
        Me.MaterialTextBox21.MaxLength = 32767
        Me.MaterialTextBox21.MouseState = MaterialSkin.MouseState.OUT
        Me.MaterialTextBox21.Name = "MaterialTextBox21"
        Me.MaterialTextBox21.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.MaterialTextBox21.PrefixSuffix = MaterialSkin.Controls.MaterialTextBox2.PrefixSuffixTypes.Prefix
        Me.MaterialTextBox21.PrefixSuffixText = "OCR"
        Me.MaterialTextBox21.ReadOnly = False
        Me.MaterialTextBox21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.MaterialTextBox21.SelectedText = ""
        Me.MaterialTextBox21.SelectionLength = 0
        Me.MaterialTextBox21.SelectionStart = 0
        Me.MaterialTextBox21.ShortcutsEnabled = True
        Me.MaterialTextBox21.Size = New System.Drawing.Size(181, 36)
        Me.MaterialTextBox21.TabIndex = 71
        Me.MaterialTextBox21.TabStop = False
        Me.MaterialTextBox21.Text = "110,260,300,50"
        Me.MaterialTextBox21.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaterialTextBox21.TrailingIcon = Nothing
        Me.MaterialTextBox21.UseSystemPasswordChar = False
        Me.MaterialTextBox21.UseTallSize = False
        '
        'Button3
        '
        Me.Button3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button3.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.[Default]
        Me.Button3.Depth = 0
        Me.Button3.Enabled = False
        Me.Button3.HighEmphasis = True
        Me.Button3.Icon = Nothing
        Me.Button3.Location = New System.Drawing.Point(192, 323)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Button3.MouseState = MaterialSkin.MouseState.HOVER
        Me.Button3.Name = "Button3"
        Me.Button3.NoAccentTextColor = System.Drawing.Color.Empty
        Me.Button3.Size = New System.Drawing.Size(82, 36)
        Me.Button3.TabIndex = 72
        Me.Button3.Text = "开启OCR"
        Me.Button3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained
        Me.Button3.UseAccentColor = False
        Me.Button3.UseVisualStyleBackColor = True
        '
        'MaterialComboBox1
        '
        Me.MaterialComboBox1.AutoResize = False
        Me.MaterialComboBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.MaterialComboBox1.Depth = 0
        Me.MaterialComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.MaterialComboBox1.DropDownHeight = 118
        Me.MaterialComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MaterialComboBox1.DropDownWidth = 121
        Me.MaterialComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Me.MaterialComboBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.MaterialComboBox1.FormattingEnabled = True
        Me.MaterialComboBox1.IntegralHeight = False
        Me.MaterialComboBox1.ItemHeight = 29
        Me.MaterialComboBox1.Items.AddRange(New Object() {"yohane.cc", "tgrcode.com"})
        Me.MaterialComboBox1.Location = New System.Drawing.Point(5, 7)
        Me.MaterialComboBox1.MaxDropDownItems = 4
        Me.MaterialComboBox1.MouseState = MaterialSkin.MouseState.OUT
        Me.MaterialComboBox1.Name = "MaterialComboBox1"
        Me.MaterialComboBox1.Size = New System.Drawing.Size(181, 35)
        Me.MaterialComboBox1.StartIndex = 0
        Me.MaterialComboBox1.TabIndex = 73
        Me.MaterialComboBox1.UseTallSize = False
        '
        'TxtZoom
        '
        Me.TxtZoom.AnimateReadOnly = False
        Me.TxtZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.TxtZoom.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.TxtZoom.Depth = 0
        Me.TxtZoom.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.TxtZoom.HideSelection = True
        Me.TxtZoom.LeadingIcon = Nothing
        Me.TxtZoom.Location = New System.Drawing.Point(380, 51)
        Me.TxtZoom.MaxLength = 32767
        Me.TxtZoom.MouseState = MaterialSkin.MouseState.OUT
        Me.TxtZoom.Name = "TxtZoom"
        Me.TxtZoom.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.TxtZoom.PrefixSuffix = MaterialSkin.Controls.MaterialTextBox2.PrefixSuffixTypes.Prefix
        Me.TxtZoom.PrefixSuffixText = "缩放"
        Me.TxtZoom.ReadOnly = True
        Me.TxtZoom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtZoom.SelectedText = ""
        Me.TxtZoom.SelectionLength = 0
        Me.TxtZoom.SelectionStart = 0
        Me.TxtZoom.ShortcutsEnabled = False
        Me.TxtZoom.Size = New System.Drawing.Size(105, 36)
        Me.TxtZoom.TabIndex = 74
        Me.TxtZoom.TabStop = False
        Me.TxtZoom.Text = "×16"
        Me.TxtZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TxtZoom.TrailingIcon = Nothing
        Me.TxtZoom.UseSystemPasswordChar = False
        Me.TxtZoom.UseTallSize = False
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(795, 428)
        Me.Controls.Add(Me.TxtZoom)
        Me.Controls.Add(Me.MaterialComboBox1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.MaterialTextBox21)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.TextBox9)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PS)
        Me.Controls.Add(Me.PicM2)
        Me.Controls.Add(Me.PicM1)
        Me.Controls.Add(Me.PicM0)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PicBot)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "关卡机器人v3.0A"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicBot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicM0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicM1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicM2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents TrackBar1 As TrackBar
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
    Friend WithEvents Timer1 As Timer
    Friend WithEvents PS As PictureBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Button2 As MaterialSkin.Controls.MaterialButton
    Friend WithEvents Button8 As MaterialSkin.Controls.MaterialButton
    Friend WithEvents TextBox9 As MaterialSkin.Controls.MaterialTextBox2
    Friend WithEvents Button5 As MaterialSkin.Controls.MaterialButton
    Friend WithEvents Button4 As MaterialSkin.Controls.MaterialButton
    Friend WithEvents MaterialTextBox21 As MaterialSkin.Controls.MaterialTextBox2
    Friend WithEvents Button3 As MaterialSkin.Controls.MaterialButton
    Friend WithEvents MaterialComboBox1 As MaterialSkin.Controls.MaterialComboBox
    Friend WithEvents TxtZoom As MaterialSkin.Controls.MaterialTextBox2
End Class
