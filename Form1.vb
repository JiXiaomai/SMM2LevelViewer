Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports System
Imports System.Diagnostics
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports Tesseract
Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        isMapIO = True
        RefPic()
        Button2.Enabled = True
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs)
        isMapIO = False
        LoadEFile(False)
        InitPng()
        DrawObject(False)

        Form2.P.Image = BMAP1
        Button2.Enabled = True
    End Sub
    Public Sub LoadEFile(IO As Boolean)
        '关卡文件头H00长度200
        LoadLvlData(TextBox1.Text, IO)
        If IO Then
            Label2.Text = "图名：" & LH.Name & vbCrLf
            Label2.Text += "描述：" & LH.Desc & vbCrLf
            Label2.Text += "时间：" & LH.Timer & vbCrLf
            Label2.Text += "风格：" & LH.GameStyle & vbCrLf
            Label2.Text += "版本：" & LH.GameVer & vbCrLf
            Label2.Text += "起点：" & LH.StartY & vbCrLf
            Label2.Text += "终点：" & LH.GoalX & "," & LH.GoalY & vbCrLf
            Label2.Text += "======表世界======" & vbCrLf
            Label2.Text += "主题：" & MapHdr.Theme & vbCrLf
            Label2.Text += "宽度：" & MapHdr.BorR & vbCrLf
            Label2.Text += "高度：" & MapHdr.BorT & vbCrLf
            Label2.Text += "砖块：" & MapHdr.GroundCount & vbCrLf
            Label2.Text += "单位：" & MapHdr.ObjCount & vbCrLf
            Label2.Text += "轨道：" & MapHdr.TrackCount & vbCrLf
            Label2.Text += "卷轴：" & MapHdr.AutoscrollType & vbCrLf
            Label2.Text += "水面：" & MapHdr.LiqSHeight & "-" & MapHdr.LiqEHeight & vbCrLf
        Else
            Label2.Text += "======里世界======" & vbCrLf
            Label2.Text += "主题：" & MapHdr.Theme & vbCrLf
            Label2.Text += "宽度：" & MapHdr.BorR & vbCrLf
            Label2.Text += "高度：" & MapHdr.BorT & vbCrLf
            Label2.Text += "砖块：" & MapHdr.GroundCount & vbCrLf
            Label2.Text += "单位：" & MapHdr.ObjCount & vbCrLf
            Label2.Text += "轨道：" & MapHdr.TrackCount & vbCrLf
            Label2.Text += "卷轴：" & MapHdr.AutoscrollType & vbCrLf
            Label2.Text += "水面：" & MapHdr.LiqSHeight & "-" & MapHdr.LiqEHeight & vbCrLf
        End If
        Dim LInfo() As FieldInfo
        Dim I As FieldInfo
        If IO Then
            LInfo = LH.GetType.GetFields()
            TextBox2.Text = ""
            For Each I In LInfo
                TextBox2.Text += I.Name & ":" & I.GetValue(LH) & vbCrLf
            Next
            TextBox3.Text = "===M0===" & vbCrLf
            '表世界H200长度2DEE0
            LInfo = MapHdr.GetType.GetFields()
            For Each I In LInfo
                TextBox3.Text += I.Name & ":" & I.GetValue(MapHdr).ToString & vbCrLf
            Next
        Else
            TextBox4.Text = "===M1===" & vbCrLf
            '表世界H200长度2DEE0
            LInfo = MapHdr.GetType.GetFields()
            For Each I In LInfo
                TextBox4.Text += I.Name & ":" & I.GetValue(MapHdr).ToString & vbCrLf
            Next
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form2.Show()
        Form2.ClientSize = New Size(500, 500)
        Form2.P.Size = BSIZE1
        Form2.P.Image = BMAP1
    End Sub
    Dim BMAP1, BMAP2 As Bitmap
    Dim BSIZE1, BSIZE2 As Size
    Dim G As Graphics
    Public Sub InitPng()
        Dim i As Integer
        Dim H As Integer, W As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        PZoom = 2 ^ TrackBar1.Value
        BMAP1 = New Bitmap(W * PZoom, H * PZoom)
        G = Graphics.FromImage(BMAP1)
        BSIZE1.Width = W * PZoom
        BSIZE1.Height = H * PZoom
        Form2.P.Size = BSIZE1
        G.InterpolationMode = InterpolationMode.NearestNeighbor
        G.SmoothingMode = SmoothingMode.None

        For i = 0 To H
            G.DrawLine(Pens.LightGray, 0, CSng(i * PZoom), CSng(W * PZoom), CSng(i * PZoom))
            If i Mod 13 = 0 Then
                G.DrawLine(Pens.LightGray, 0, CSng((H - i) * PZoom + 1), CSng(W * PZoom), CSng((H - i) * PZoom + 1))
            End If
            If (H - i) Mod 10 = 0 Then
                G.DrawString((H - i).ToString, Button1.Font, Brushes.Black, 0, CSng(i * PZoom))
            End If
        Next
        For i = 0 To W
            G.DrawLine(Pens.LightGray, CSng(i * PZoom), 0, CSng(i * PZoom), CSng(H * PZoom))
            If i Mod 24 = 0 Then
                G.DrawLine(Pens.LightGray, CSng(i * PZoom + 1), 0, CSng(i * PZoom + 1), CSng(H * PZoom))
            End If
            If i Mod 10 = 9 Then
                G.DrawString((i + 1).ToString, Button1.Font, Brushes.Black, CSng(i * PZoom), 0)
            End If
        Next
        Dim BC1, BC2 As Color
        If MapHdr.Theme = 2 Then
            BC1 = Color.FromArgb(64, 255, 0, 0)
            BC2 = Color.FromArgb(64, 255, 0, 0)
            G.FillRectangle(New SolidBrush(BC2), 0, CSng((H - MapHdr.LiqEHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
            G.FillRectangle(New SolidBrush(BC1), 0, CSng((H - MapHdr.LiqSHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
        ElseIf MapHdr.Theme = 9 Then
            BC1 = Color.FromArgb(64, 0, 0, 255)
            BC2 = Color.FromArgb(64, 0, 0, 255)
            G.FillRectangle(New SolidBrush(BC2), 0, CSng((H - MapHdr.LiqEHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
            G.FillRectangle(New SolidBrush(BC1), 0, CSng((H - MapHdr.LiqSHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
        End If

    End Sub
    Public Sub InitPng2()
        'ReDim OBJ(ObjEng.GetUpperBound(0))
        Dim i As Integer
        'For i = 0 To OBJ.GetUpperBound(0)
        '	Try
        '		OBJ(i) = New Bitmap(PT & "\img\" & LH.GameStyle.ToString & "\obj\" & i.ToString & ".PNG")
        '	Catch
        '	End Try
        'Next
        'FLG(0) = New Bitmap(PT & "\img\" & LH.GameStyle.ToString & "\obj\F0.PNG")
        'FLG(1) = New Bitmap(PT & "\img\" & LH.GameStyle.ToString & "\obj\F1.PNG")
        'FLG(2) = New Bitmap(PT & "\img\" & LH.GameStyle.ToString & "\obj\F2.PNG")

        Dim H As Integer, W As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        PZoom = 2 ^ TrackBar1.Value
        BMAP2 = New Bitmap(W * PZoom, H * PZoom)
        G = Graphics.FromImage(BMAP2)
        BSIZE2.Width = W * PZoom
        BSIZE2.Height = H * PZoom
        Form3.P.Size = BSIZE2
        G.InterpolationMode = InterpolationMode.NearestNeighbor
        For i = 0 To H
            G.DrawLine(Pens.WhiteSmoke, 0, CSng(i * PZoom), CSng(W * PZoom), CSng(i * PZoom))
            If i Mod 13 = 0 Then
                G.DrawLine(Pens.WhiteSmoke, 0, CSng((H - i) * PZoom + 1), CSng(W * PZoom), CSng((H - i) * PZoom + 1))
            End If
            If (H - i) Mod 10 = 0 Then
                G.DrawString((H - i).ToString, Button1.Font, Brushes.Black, 0, CSng(i * PZoom))
            End If
        Next
        For i = 0 To W
            G.DrawLine(Pens.WhiteSmoke, CSng(i * PZoom), 0, CSng(i * PZoom), CSng(H * PZoom))
            If i Mod 24 = 0 Then
                G.DrawLine(Pens.WhiteSmoke, CSng(i * PZoom + 1), 0, CSng(i * PZoom + 1), CSng(H * PZoom))
            End If
            If i Mod 10 = 9 Then
                G.DrawString((i + 1).ToString, Button1.Font, Brushes.Black, CSng(i * PZoom), 0)
            End If
        Next

        Dim BC1, BC2 As Color
        If MapHdr.Theme = 2 Then
            BC1 = Color.FromArgb(64, 255, 0, 0)
            BC2 = Color.FromArgb(64, 255, 0, 0)
            G.FillRectangle(New SolidBrush(BC2), 0, CSng((H - MapHdr.LiqEHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
            G.FillRectangle(New SolidBrush(BC1), 0, CSng((H - MapHdr.LiqSHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
        ElseIf MapHdr.Theme = 9 Then
            BC1 = Color.FromArgb(64, 0, 0, 255)
            BC2 = Color.FromArgb(64, 0, 0, 255)
            G.FillRectangle(New SolidBrush(BC2), 0, CSng((H - MapHdr.LiqEHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
            G.FillRectangle(New SolidBrush(BC1), 0, CSng((H - MapHdr.LiqSHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
        End If

    End Sub
    Public Sub DrawMoveBlock(ID As Byte, EX As Byte, X As Integer, Y As Integer)
        On Error GoTo Err
        Dim H, W, XX, YY As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        XX = X / 160 + 1
        YY = (Y + 80) / 160 + 1
        Dim i As Integer

        Select Case ID
            Case 85
                Select Case MapTrackBlk(EX - 1).Node(0).p1
                    Case 1, 5, 7, 14
                        XX -= 4
                    Case 2, 9, 11, 13

                    Case 3, 6, 10, 16
                        XX -= 2
                        YY -= 2
                    Case 4, 8, 12, 15
                        XX -= 2
                        YY += 2
                End Select
                For i = 0 To MapTrackBlk(EX - 1).NodeCount - 1
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SS.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    'G.DrawString(MapTrackBlk(EX - 1).Node(i).p1, Me.Font, Brushes.Black, (XX) * PZOOM, (H - YY) * PZOOM)
                    Select Case MapTrackBlk(EX - 1).Node(i).p1
                        Case 1 'L
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 2 'R
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 3 'D
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 4 'U
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 5 'LD
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SRD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 6 'DL
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SUL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 7 'LU
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SRU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 8 'UL
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SDL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 9 'RD
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SLD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 10 'DR
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SUR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 11 'RU
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SLU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 12 'UR
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SDR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 13 'RE
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 14 'LE
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 15 'UE
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 16 'DE
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    End Select
                Next
            Case 119
                Select Case MapMoveBlk(EX - 1).Node(0).p1
                    Case 1, 5, 7, 14
                        XX -= 4
                    Case 2, 9, 11, 13

                    Case 3, 6, 10, 16
                        XX -= 2
                        YY -= 2
                    Case 4, 8, 12, 15
                        XX -= 2
                        YY += 2
                End Select
                For i = 0 To MapMoveBlk(EX - 1).NodeCount - 1
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SS.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    'G.DrawString(MapMoveBlk(EX - 1).Node(i).p1, Me.Font, Brushes.Black, (XX) * PZOOM, (H - YY) * PZOOM)
                    Select Case MapMoveBlk(EX - 1).Node(i).p1
                        Case 1 'L
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 2 'R
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 3 'D
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 4 'U
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 5 'LD
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SRD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 6 'DL
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SUL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 7 'LU
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SRU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 8 'UL
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SDL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 9 'RD
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SLD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 10 'DR
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SUR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 11 'RU
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SLU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 12 'UR
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SDR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 13 'RE
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 14 'LE
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 15 'UE
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 16 'DE
                            G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    End Select
                Next
        End Select
Err:
    End Sub
    Public Sub DrawCrp(EX As Byte, X As Integer, Y As Integer)
        On Error GoTo Err
        Dim H, W, XX, YY As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        XX = X / 160 + 1
        YY = (Y + 80) / 160 + 1
        Dim i As Integer

        Select Case MapCrp(EX - 1).Node(0)
            Case 1, 5, 7, 14
                XX -= 4
            Case 2, 9, 11, 13

            Case 3, 6, 10, 16
                XX -= 2
                YY -= 2
            Case 4, 8, 12, 15
                XX -= 2
                YY += 2
        End Select

        For i = 0 To MapCrp(EX - 1).NodeCount - 1
            G.DrawImage(Image.FromFile(PT & "\img\CMN\SS.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
            'G.DrawString(MapCrp(EX - 1).Node(i), Me.Font, Brushes.Black, (XX) * PZOOM, (H - YY) * PZOOM)
            Select Case MapCrp(EX - 1).Node(i)
                Case 1 'L
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX -= 2
                Case 2 'R
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX += 2
                Case 3 'D
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY -= 2
                Case 4 'U
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY += 2
                Case 5 'LD
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SRD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY -= 2
                Case 6 'DL
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SUL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX -= 2
                Case 7 'LU
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SRU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY += 2
                Case 8 'UL
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SDL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX -= 2
                Case 9 'RD
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SLD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY -= 2
                Case 10 'DR
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SUR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX += 2
                Case 11 'RU
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SLU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY += 2
                Case 12 'UR
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SDR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX += 2
                Case 13 'RE
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                Case 14 'LE
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                Case 15 'UE
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                Case 16 'DE
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
            End Select
        Next
Err:
    End Sub
    Public Sub DrawSnake(EX As Byte, X As Integer, Y As Integer, SW As Integer, SH As Integer)
        '蛇砖块
        On Error GoTo Err
        Dim H, W, XX, YY As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16

        YY = (Y + SH * 80) / 160
        If EX < &H10 Then
            XX = (X + SW * 80) / 160
            EX = EX Mod &H10
            Select Case MapSnk(EX - 1).Node(0).Dir
                Case 1, 5, 7
                    XX -= 1
                Case 2, 9, 11

                Case 3, 6, 10
                    XX -= 1
                    YY -= 1
                Case 4, 8, 12
                    XX -= 1
                    YY += 1
            End Select
        Else
            XX = (X - SW * 80) / 160
            EX = EX Mod &H10
            Select Case MapSnk(EX - 1).Node(0).Dir
                Case 1, 5, 7
                    XX -= 1
                Case 2, 9, 11

                Case 3, 6, 10
                    YY -= 1
                Case 4, 8, 12
                    YY += 1
            End Select
        End If


        Dim i As Integer


        For i = 0 To MapSnk(EX - 1).NodeCount - 1
            G.DrawImage(Image.FromFile(PT & "\img\CMN\SS.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
            'G.DrawString(MapSnk(EX - 1).Node(i).Dir, Me.Font, Brushes.Black, (XX + 0.5) * PZOOM, (H - YY - 0.5) * PZOOM)
            Select Case MapSnk(EX - 1).Node(i).Dir
                Case 1 'L
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX -= 1
                Case 2 'R
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX += 1
                Case 3 'D
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY -= 1
                Case 4 'U
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY += 1
                Case 5 'LD
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SRD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY -= 1
                Case 6 'DL
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SUL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX -= 1
                Case 7 'LU
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SRU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY += 1
                Case 8 'UL
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SDL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX -= 1
                Case 9 'RD
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SLD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY -= 1
                Case 10 'DR
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SUR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX += 1
                Case 11 'RU
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SLU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY += 1
                Case 12 'UR
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SDR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX += 1
                Case 13 'RE
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                Case 14 'LE
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                Case 15 'UE
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                Case 16 'DE
                    G.DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
            End Select

        Next

Err:
    End Sub
    Public Sub DrawIce()
        '冰块
        Dim i As Integer, H As Integer
        For i = 0 To MapHdr.IceCount - 1
            If MapIce(i).ID = 0 Then
                G.DrawImage(GetTile(15, 41, 1, 2), MapIce(i).X * PZoom, (MapHdr.BorT \ 16 - 2) * PZoom - MapIce(i).Y * PZoom, PZoom, PZoom * 2)
                For H = 1 To 2
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).Obj += "118,"
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).Flag += ","
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).SubObj += ","
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).SubFlag += ","
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).State += ","
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).SubState += ","
                Next
            Else
                G.DrawImage(GetTile(15, 39, 1, 2), MapIce(i).X * PZoom, (MapHdr.BorT \ 16 - 2) * PZoom - MapIce(i).Y * PZoom, PZoom, PZoom * 2)
                For H = 1 To 2
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).Obj += "118A,"
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).Flag += ","
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).SubObj += ","
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).SubFlag += ","
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).State += ","
                    ObjLocData(NowIO, MapIce(i).X + 1, Int(H + MapIce(i).Y)).SubState += ","
                Next
            End If
        Next
    End Sub
    Public Sub DrawTrack()
        '轨道
        Dim H As Integer, W As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        Dim i As Integer
        For i = 0 To MapHdr.TrackCount - 1
            'LID+1?
            ObjLinkType(MapTrk(i).LID) = 59
            If MapTrk(i).Type < 8 Then
                G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T" & MapTrk(i).Type.ToString & ".PNG"), MapTrk(i).X * PZoom - PZoom, (H - 2) * PZoom - MapTrk(i).Y * PZoom, PZoom * 3, PZoom * 3)
                Select Case MapTrk(i).Type
                    Case 0
                        If TrackNode(MapTrk(i).X + 1 + 1, MapTrk(i).Y + 1) = 1 AndAlso MapTrk(i).F0 = 0 Then
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(i).X * PZoom + PZoom, (H - 1) * PZoom - MapTrk(i).Y * PZoom, PZoom, PZoom)
                        End If
                        If TrackNode(MapTrk(i).X + 1 - 1, MapTrk(i).Y + 1) = 1 AndAlso MapTrk(i).F1 = 0 Then
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(i).X * PZoom - PZoom, (H - 1) * PZoom - MapTrk(i).Y * PZoom, PZoom, PZoom)
                        End If
                    Case 1
                        If TrackNode(MapTrk(i).X + 1, MapTrk(i).Y + 1 + 1) = 1 AndAlso MapTrk(i).F0 = 0 Then
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(i).X * PZoom, (H - 2) * PZoom - MapTrk(i).Y * PZoom, PZoom, PZoom)
                        End If
                        If TrackNode(MapTrk(i).X + 1, MapTrk(i).Y + 1 - 1) = 1 AndAlso MapTrk(i).F1 = 0 Then
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(i).X * PZoom, H * PZoom - MapTrk(i).Y * PZoom, PZoom, PZoom)
                        End If
                    Case 2, 4, 5
                        If TrackNode(MapTrk(i).X + 1 + 1, MapTrk(i).Y + 1 - 1) = 1 AndAlso MapTrk(i).F0 = 0 Then
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(i).X * PZoom + PZoom, H * PZoom - MapTrk(i).Y * PZoom, PZoom, PZoom)
                        End If
                        If TrackNode(MapTrk(i).X + 1 - 1, MapTrk(i).Y + 1 + 1) = 1 AndAlso MapTrk(i).F1 = 0 Then
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(i).X * PZoom - PZoom, (H - 2) * PZoom - MapTrk(i).Y * PZoom, PZoom, PZoom)
                        End If
                    Case 3, 6, 7
                        If TrackNode(MapTrk(i).X + 1 + 1, MapTrk(i).Y + 1 + 1) = 1 AndAlso MapTrk(i).F0 = 0 Then
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(i).X * PZoom + PZoom, (H - 2) * PZoom - MapTrk(i).Y * PZoom, PZoom, PZoom)
                        End If
                        If TrackNode(MapTrk(i).X + 1 - 1, MapTrk(i).Y + 1 - 1) = 1 AndAlso MapTrk(i).F1 = 0 Then
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(i).X * PZoom - PZoom, H * PZoom - MapTrk(i).Y * PZoom, PZoom, PZoom)
                        End If
                End Select
            Else 'Y轨道
                G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T" & MapTrk(i).Type.ToString & ".PNG"), MapTrk(i).X * PZoom - PZoom, (H - 4) * PZoom - MapTrk(i).Y * PZoom, PZoom * 5, PZoom * 5)
                'G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 0).X) * PZOOM, H * PZOOM - (MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 0).Y) * PZOOM, PZOOM, PZOOM)
                'G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 1).X) * PZOOM, H * PZOOM - (MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 1).Y) * PZOOM, PZOOM, PZOOM)
                'G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 2).X) * PZOOM, H * PZOOM - (MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 2).Y) * PZOOM, PZOOM, PZOOM)

                If TrackNode(MapTrk(i).X + TrackYPt(MapTrk(i).Type, 0).X, MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 0).Y) = 1 AndAlso MapTrk(i).F0 = 0 Then
                    G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 0).X) * PZoom, (H - 4) * PZoom - (MapTrk(i).Y - TrackYPt(MapTrk(i).Type, 0).Y) * PZoom, PZoom, PZoom)
                End If
                If TrackNode(MapTrk(i).X + TrackYPt(MapTrk(i).Type, 1).X, MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 1).Y) = 1 AndAlso MapTrk(i).F1 = 0 Then
                    G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 1).X) * PZoom, (H - 4) * PZoom - (MapTrk(i).Y - TrackYPt(MapTrk(i).Type, 1).Y) * PZoom, PZoom, PZoom)
                End If
                If TrackNode(MapTrk(i).X + TrackYPt(MapTrk(i).Type, 2).X, MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 2).Y) = 1 AndAlso MapTrk(i).F2 = 0 Then
                    G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 2).X) * PZoom, (H - 4) * PZoom - (MapTrk(i).Y - TrackYPt(MapTrk(i).Type, 2).Y) * PZoom, PZoom, PZoom)
                End If
            End If
        Next
    End Sub
    Public Sub DrawSlope()
        Dim H As Integer, W As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        Dim i As Integer
        Dim CX, CY As Integer
        'GrdType
        '0无 1方 2陡左上 3陡右上 4陡左下 5陡右下 6端左上 7端右上 8端左下 9端右下
        '
        For i = 0 To MapHdr.ObjCount - 1
            Select Case MapObj(i).ID
                Case 87
                    '缓坡
                    CX = (-0.5 + MapObj(i).X / 160)
                    CY = (-0.5 + MapObj(i).Y / 160)
                    If (MapObj(i).Flag \ &H100000) Mod &H2 = 0 Then
                        '左斜
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                            CSng(CX * PZoom), (H - 1) * PZoom - CSng(CY * PZoom), PZoom, PZoom)
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                            CSng((MapObj(i).W - 1.5 + MapObj(i).X / 160) * PZoom), (H - 1) * PZoom - CSng((CY + MapObj(i).H - 1) * PZoom), PZoom, PZoom)
                        'G.DrawString(GroundNode(CX + 1, CY + 1), Me.Font, Brushes.Black, CSng(CX * PZOOM), (H - 1) * PZOOM - CSng(CY * PZOOM))
                        'G.DrawString(GroundNode(CX + MapObj(i).W, CY + MapObj(i).H), Me.Font, Brushes.Black, CSng((CX + MapObj(i).W - 1) * PZOOM), (H - 1) * PZOOM - CSng((CY + MapObj(i).H - 1) * PZOOM))

                        For j = 1 To MapObj(i).W - 2 Step 2
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\87.PNG"),
                                                CSng((CX + j) * PZoom),
                                                (H - 1) * PZoom - CSng((j / 2 + MapObj(i).Y / 160) * PZoom), PZoom * 2, PZoom * 2)
                            'G.DrawString(GroundNode(CX + 1 + j, CY + 3 + (j \ 2)), Me.Font, Brushes.Black, CSng((CX + j) * PZOOM), (H - 1) * PZOOM - CSng((CY + 1 + (j \ 2)) * PZOOM))
                            'G.DrawString(GroundNode(CX + 1 + j, CY + 2 + (j \ 2)), Me.Font, Brushes.Black, CSng((CX + j) * PZOOM), (H - 1) * PZOOM - CSng((CY + (j \ 2)) * PZOOM))

                            'G.DrawString(GroundNode(CX + 2 + j, CY + 3 + (j \ 2)), Me.Font, Brushes.Black, CSng((CX + j + 1) * PZOOM), (H - 1) * PZOOM - CSng((CY + 1 + (j \ 2)) * PZOOM))
                            'G.DrawString(GroundNode(CX + 2 + j, CY + 2 + (j \ 2)), Me.Font, Brushes.Black, CSng((CX + j + 1) * PZOOM), (H - 1) * PZOOM - CSng((CY + (j \ 2)) * PZOOM))
                        Next
                    Else
                        '右斜
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                            CSng((-0.5 + MapObj(i).X / 160) * PZoom),
                                            (H - 1) * PZoom - CSng((MapObj(i).H - 1.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                            CSng((MapObj(i).W - 1.5 + MapObj(i).X / 160) * PZoom),
                                            (H - 1) * PZoom - CSng((-0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                        'G.DrawString(GroundNode(CX + 1, CY + MapObj(i).H), Me.Font, Brushes.Black, CSng(CX * PZOOM), (H - 1) * PZOOM - CSng((CY + MapObj(i).H - 1) * PZOOM))
                        'G.DrawString(GroundNode(CX + MapObj(i).W, CY + 1), Me.Font, Brushes.Black, CSng((CX + MapObj(i).W - 1) * PZOOM), (H - 1) * PZOOM - CSng(CY * PZOOM))
                        For j = 1 To MapObj(i).W - 2 Step 2

                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\87A.PNG"),
                                                CSng((j - 0.5 + MapObj(i).X / 160) * PZoom),
                                                (H - 1) * PZoom - CSng((-j / 2 + MapObj(i).H - 1 + MapObj(i).Y / 160) * PZoom), PZoom * 2, PZoom * 2)
                            'G.DrawString(GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2)), Me.Font, Brushes.Black,
                            '			 CSng((CX + j) * PZOOM), (H - 1) * PZOOM - CSng((CY + MapObj(i).H - (j \ 2) - 1) * PZOOM))
                            'G.DrawString(GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2) - 1), Me.Font, Brushes.Black,
                            '			 CSng((CX + j) * PZOOM), (H - 1) * PZOOM - CSng((CY + MapObj(i).H - (j \ 2) - 2) * PZOOM))

                            'G.DrawString(GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2)), Me.Font, Brushes.Black,
                            'CSng((CX + j + 1) * PZOOM), (H - 1) * PZOOM - CSng((CY + MapObj(i).H - (j \ 2) - 1) * PZOOM))
                            'G.DrawString(GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2) - 1), Me.Font, Brushes.Black,
                            'CSng((CX + j + 1) * PZOOM), (H - 1) * PZOOM - CSng((CY + MapObj(i).H - (j \ 2) - 2) * PZOOM))
                        Next


                    End If
                Case 88
                    '陡坡
                    CX = (-0.5 + MapObj(i).X / 160)
                    CY = (-0.5 + MapObj(i).Y / 160)
                    If (MapObj(i).Flag \ &H100000) Mod &H2 = 0 Then
                        '左斜
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                            CSng((CX) * PZoom), (H - 1) * PZoom - CSng(CY * PZoom), PZoom, PZoom)
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                            CSng((CX + MapObj(i).W - 1) * PZoom), (H - 1) * PZoom - CSng((CY + MapObj(i).H - 1) * PZoom), PZoom, PZoom)
                        'G.DrawString(GroundNode(CX + 1, CY + 1), Me.Font, Brushes.Black, CSng((CX) * PZOOM), (H - 1) * PZOOM - CSng(CY * PZOOM))
                        'G.DrawString(GroundNode(CX + MapObj(i).W, CY + MapObj(i).H), Me.Font, Brushes.Black, CSng((CX + MapObj(i).W - 1) * PZOOM), (H - 1) * PZOOM - CSng((CY + MapObj(i).H - 1) * PZOOM))
                        For j = 1 To MapObj(i).W - 2
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\88.PNG"),
                                                CSng((CX + j) * PZoom),
                                                (H - 1) * PZoom - CSng((CY + j) * PZoom), PZoom, PZoom * 2)
                            'G.DrawString(GroundNode(CX + 1 + j, CY + 1 + j), Me.Font, Brushes.Black, CSng((CX + j) * PZOOM), (H - 1) * PZOOM - CSng((CY + j) * PZOOM))
                            'G.DrawString(GroundNode(CX + 1 + j, CY + j), Me.Font, Brushes.Black, CSng((CX + j) * PZOOM), (H - 1) * PZOOM - CSng((CY - 1 + j) * PZOOM))

                        Next
                    Else
                        '右斜
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                            CSng(CX * PZoom),
                                            (H - 1) * PZoom - CSng((CY + MapObj(i).H - 1) * PZoom), PZoom, PZoom)
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                            CSng((CX + MapObj(i).W - 1) * PZoom),
                                            (H - 1) * PZoom - CSng(CY * PZoom), PZoom, PZoom)
                        'G.DrawString(GroundNode(CX + MapObj(i).W, CY + 1), Me.Font, Brushes.Black, CSng((CX + MapObj(i).W - 1) * PZOOM), (H - 1) * PZOOM - CSng(CY * PZOOM))
                        'G.DrawString(GroundNode(CX + 1, CY + MapObj(i).W), Me.Font, Brushes.Black, CSng((CX) * PZOOM), (H - 1) * PZOOM - CSng((CY + MapObj(i).H - 1) * PZOOM))

                        For j = 1 To MapObj(i).W - 2
                            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\88A.PNG"),
                                                CSng((CX + j) * PZoom),
                                                (H - 1) * PZoom - CSng((CY - j - 1 + MapObj(i).W) * PZoom), PZoom, PZoom * 2)
                            'G.DrawString(GroundNode(CX + 1 + j, CY - j - 1 + MapObj(i).W), Me.Font, Brushes.Black, CSng((CX + j) * PZOOM), (H - 1) * PZOOM - CSng((CY - j - 2 + MapObj(i).W) * PZOOM))
                            'G.DrawString(GroundNode(CX + 1 + j, CY - j + MapObj(i).W), Me.Font, Brushes.Black, CSng((CX + j) * PZOOM), (H - 1) * PZOOM - CSng((CY - j - 1 + MapObj(i).W) * PZOOM))
                        Next
                    End If
            End Select
        Next
    End Sub
    Public Function GetGrdBold(x As Integer, y As Integer) As Integer
        If GroundNode(x, y + 1) > 1 Then 'U
            GetGrdBold = GroundNode(x, y + 1)
        ElseIf GroundNode(x, y - 1) > 1 Then 'D
            GetGrdBold = GroundNode(x, y - 1)
        Else
            GetGrdBold = 0
        End If
    End Function
    Public Function CalC(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer,
                         x3 As Integer, y3 As Integer, x4 As Integer, y4 As Integer) As Integer
        CalC = IIf(GroundNode(x1, y1) = 0, 0, 1000) + IIf(GroundNode(x2, y2) = 0, 0, 100) + IIf(GroundNode(x3, y3) = 0, 0, 10) + IIf(GroundNode(x4, y4) = 0, 0, 1)
    End Function
    Public Function GetCorCode(x As Integer, y As Integer) As Point
        Dim C As Integer

        Select Case GetGrdBold(x, y)
            Case 2 'D陡左上
                C = CalC(x - 1, y, x - 1, y + 1, x, y + 1, x + 1, y + 1)
                Select Case C
                    Case 0
                        GetCorCode = New Point(5, 30)
                    Case 1, 100, 101
                        GetCorCode = New Point(10, 13)
                    Case 10
                        GetCorCode = New Point(15, 26)
                    Case 11, 111
                        GetCorCode = New Point(9, 27)
                    Case 110
                        GetCorCode = New Point(15, 27)
                    Case 1000, 1001, 1100, 1101
                        GetCorCode = New Point(5, 27)
                    Case 1010
                        GetCorCode = New Point(13, 27)
                    Case 1011
                        GetCorCode = New Point(7, 27)
                    Case 1110
                        GetCorCode = New Point(11, 27)
                    Case 1111
                        GetCorCode = New Point(1, 26)
                End Select
            Case 3 'D陡右上
                C = CalC(x - 1, y + 1, x, y + 1, x + 1, y + 1, x + 1, y)
                Select Case C
                    Case 0, 10, 1000, 1010
                        GetCorCode = New Point(4, 30)
                    Case 1, 11, 1001, 1011
                        GetCorCode = New Point(4, 27)
                    Case 100, 110
                        GetCorCode = New Point(14, 27)
                    Case 101
                        GetCorCode = New Point(12, 27)
                    Case 111
                        GetCorCode = New Point(10, 27)
                    Case 1100, 1110
                        GetCorCode = New Point(8, 27)
                    Case 1101
                        GetCorCode = New Point(6, 27)
                    Case 1111
                        GetCorCode = New Point(0, 26)
                End Select
            Case 4 'U陡左下
                C = CalC(x - 1, y, x - 1, y - 1, x, y - 1, x + 1, y - 1)
                Select Case C
                    Case 0, 1, 100, 101
                        GetCorCode = New Point(5, 29)
                    Case 10, 110
                        GetCorCode = New Point(15, 26)
                    Case 11, 111
                        GetCorCode = New Point(9, 26)
                    Case 1000, 1001, 1100, 1101
                        GetCorCode = New Point(5, 26)
                    Case 1010
                        GetCorCode = New Point(13, 26)
                    Case 1011
                        GetCorCode = New Point(7, 26)
                    Case 1110
                        GetCorCode = New Point(11, 26)
                    Case 1111
                        GetCorCode = New Point(1, 25)
                End Select
            Case 5 'U陡右下
                C = CalC(x - 1, y - 1, x, y - 1, x + 1, y - 1, x + 1, y)
                Select Case C
                    Case 0, 10, 1000, 1010
                        GetCorCode = New Point(4, 29)
                    Case 1, 11, 1001, 1011
                        GetCorCode = New Point(4, 26)
                    Case 100, 110
                        GetCorCode = New Point(14, 26)
                    Case 101
                        GetCorCode = New Point(12, 26)
                    Case 111
                        GetCorCode = New Point(10, 26)
                    Case 1100, 1110
                        GetCorCode = New Point(8, 26)
                    Case 1101
                        GetCorCode = New Point(6, 26)
                    Case 1111
                        GetCorCode = New Point(0, 25)
                End Select
            Case 12 'D缓大左上
                C = CalC(x - 1, y, x - 1, y + 1, x, y + 1, x + 1, y + 1)
                Select Case C
                    Case 0, 1, 100, 101
                        GetCorCode = New Point(5, 30)
                    Case 10, 110
                        GetCorCode = New Point(15, 30)
                    Case 11, 111
                        GetCorCode = New Point(13, 31)
                    Case 1000, 1001, 1100, 1101
                        GetCorCode = New Point(8, 31)
                    Case 1010
                        GetCorCode = New Point(13, 30)
                    Case 1011
                        GetCorCode = New Point(11, 31)
                    Case 1110
                        GetCorCode = New Point(11, 30)
                    Case 1111
                        GetCorCode = New Point(2, 30)
                End Select
            Case 13 'D缓大右上
                C = CalC(x - 1, y + 1, x, y + 1, x + 1, y + 1, x + 1, y)
                Select Case C
                    Case 0, 10, 1000, 1010
                        GetCorCode = New Point(4, 30)
                    Case 1, 11, 1001, 1011
                        GetCorCode = New Point(7, 31)
                    Case 100, 110
                        GetCorCode = New Point(14, 30)
                    Case 101
                        GetCorCode = New Point(12, 30)
                    Case 111
                        GetCorCode = New Point(10, 30)
                    Case 1100, 1110
                        GetCorCode = New Point(12, 31)
                    Case 1101
                        GetCorCode = New Point(10, 31)
                    Case 1111
                        GetCorCode = New Point(1, 30)
                End Select
            Case 14 'U缓大左下
                C = CalC(x - 1, y, x - 1, y - 1, x, y - 1, x + 1, y - 1)
                Select Case C
                    Case 0, 1, 100, 101
                        GetCorCode = New Point(5, 29)
                    Case 10, 110
                        GetCorCode = New Point(15, 29)
                    Case 11, 111
                        GetCorCode = New Point(13, 28)
                    Case 1000, 1001, 1100, 1101
                        GetCorCode = New Point(8, 28)
                    Case 1010
                        GetCorCode = New Point(13, 29)
                    Case 1011
                        GetCorCode = New Point(10, 29)
                    Case 1110
                        GetCorCode = New Point(11, 29)
                    Case 1111
                        GetCorCode = New Point(2, 29)
                End Select
            Case 15 'U缓大右下
                C = CalC(x - 1, y - 1, x, y - 1, x + 1, y - 1, x + 1, y)
                Select Case C
                    Case 0, 10, 1000, 1010
                        GetCorCode = New Point(4, 29)
                    Case 1, 11, 1001, 1011
                        GetCorCode = New Point(7, 28)
                    Case 100, 110
                        GetCorCode = New Point(14, 29)
                    Case 101
                        GetCorCode = New Point(12, 29)
                    Case 111
                        GetCorCode = New Point(10, 29)
                    Case 1100, 1110
                        GetCorCode = New Point(12, 28)
                    Case 1101
                        GetCorCode = New Point(11, 29)
                    Case 1111
                        GetCorCode = New Point(1, 29)
                End Select
            Case 16 'D缓小左上
                C = CalC(x - 1, y, x - 1, y + 1, x, y + 1, x + 1, y + 1)
                Select Case C
                    Case 0, 1, 100, 101
                        GetCorCode = New Point(5, 30)
                    Case 10, 110
                        GetCorCode = New Point(15, 27)
                    Case 11, 111
                        GetCorCode = New Point(9, 27)
                    Case 1000, 1001, 1100, 1101
                        GetCorCode = New Point(9, 31)
                    Case 1010
                        GetCorCode = New Point(9, 30)
                    Case 1011
                        GetCorCode = New Point(15, 31)
                    Case 1110
                        GetCorCode = New Point(7, 30)
                    Case 1111
                        GetCorCode = New Point(3, 30)
                End Select
            Case 17 'D缓小右上
                C = CalC(x - 1, y + 1, x, y + 1, x + 1, y + 1, x + 1, y)
                Select Case C
                    Case 0, 10, 1000, 1010
                        GetCorCode = New Point(4, 30)
                    Case 1, 11, 1001, 1011
                        GetCorCode = New Point(6, 31)
                    Case 100, 110
                        GetCorCode = New Point(14, 27)
                    Case 101
                        GetCorCode = New Point(8, 30)
                    Case 111
                        GetCorCode = New Point(6, 30)
                    Case 1100, 1110
                        GetCorCode = New Point(8, 27)
                    Case 1101
                        GetCorCode = New Point(14, 31)
                    Case 1111
                        GetCorCode = New Point(0, 30)
                End Select
            Case 18 'U缓小左下
                C = CalC(x - 1, y, x - 1, y - 1, x, y - 1, x + 1, y - 1)
                Select Case C
                    Case 0, 1, 100, 101
                        GetCorCode = New Point(5, 29)
                    Case 10, 110
                        GetCorCode = New Point(15, 26)
                    Case 11, 111
                        GetCorCode = New Point(9, 26)
                    Case 1000, 1001, 1100, 1101
                        GetCorCode = New Point(9, 28)
                    Case 1010
                        GetCorCode = New Point(9, 29)
                    Case 1011
                        GetCorCode = New Point(15, 28)
                    Case 1110
                        GetCorCode = New Point(7, 29)
                    Case 1111
                        GetCorCode = New Point(3, 29)
                End Select
            Case 19 'U缓小右下
                C = CalC(x - 1, y - 1, x, y - 1, x + 1, y - 1, x + 1, y)
                Select Case C
                    Case 0, 10, 1000, 1010
                        GetCorCode = New Point(4, 29)
                    Case 1, 11, 1001, 1011
                        GetCorCode = New Point(6, 28)
                    Case 100, 110
                        GetCorCode = New Point(14, 26)
                    Case 101
                        GetCorCode = New Point(8, 29)
                    Case 111
                        GetCorCode = New Point(6, 29)
                    Case 1100, 1110
                        GetCorCode = New Point(8, 26)
                    Case 1101
                        GetCorCode = New Point(14, 28)
                    Case 1111
                        GetCorCode = New Point(0, 29)
                End Select
            Case Else
                GetCorCode = GrdLoc(GetGrdType(GetGrdCode(x - 1, y - 1)))

        End Select
    End Function
    Public Sub DrawGrdCode()
        '绘制地形
        Dim i As Integer, j As Integer
        Dim R As Point
        '斜坡
        Dim H As Integer, W As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        'GrdType
        '0无 1方 2陡左上 3陡右上 4陡左下 5陡右下 6端左上 7端右上 8端左下 9端右下
        '10  11  12缓大左上 13缓大右上 14缓大左下 15缓大右下  16缓小左上 17缓小右上 18缓小左下 19缓小右下
        '20端左上 21端右上 22端左下 23端右下 24    25    26

        For i = 1 To W + 1
            For j = 1 To H + 1

                Select Case GroundNode(i, j)
                    Case 0
                        'G.DrawString(GroundNode(i, j), Me.Font, Brushes.Black, (i - 1) * PZOOM, (H - j) * PZOOM)
                    Case 6, 7, 8, 9, 20, 21, 22, 23
                        R = GrdLoc(GetGrdType(GetGrdCode(i - 1, j - 1)))
                        G.DrawImage(GetTile(R.X, R.Y, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        GroundNode(i, j) = 1
                    Case 2
                        If GroundNode(i, j + 1) = 5 Then
                            G.DrawImage(GetTile(2, 25, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        ElseIf GroundNode(i - 1, j + 1) = 1 Then
                            G.DrawImage(GetTile(1, 27, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        Else
                            G.DrawImage(GetTile(3, 27, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        End If
                    Case 3
                        If GroundNode(i, j + 1) = 4 Then
                            G.DrawImage(GetTile(3, 25, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        ElseIf GroundNode(i + 1, j + 1) = 1 Then
                            G.DrawImage(GetTile(0, 27, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        Else
                            G.DrawImage(GetTile(2, 27, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        End If
                    Case 4
                        If GroundNode(i, j - 1) = 3 Then
                            G.DrawImage(GetTile(3, 24, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        ElseIf GroundNode(i - 1, j - 1) = 1 Then
                            G.DrawImage(GetTile(1, 24, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        Else
                            G.DrawImage(GetTile(3, 26, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        End If
                    Case 5
                        If GroundNode(i, j - 1) = 2 Then
                            G.DrawImage(GetTile(2, 24, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        ElseIf GroundNode(i + 1, j - 1) = 1 Then
                            G.DrawImage(GetTile(0, 24, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        Else
                            G.DrawImage(GetTile(2, 26, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        End If
                    Case 12
                        If GroundNode(i, j + 1) = 19 Then
                            G.DrawImage(GetTile(0, 33, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        ElseIf GroundNode(i - 1, j + 1) = 1 Then
                            G.DrawImage(GetTile(2, 31, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        Else
                            G.DrawImage(GetTile(5, 31, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        End If
                    Case 13
                        If GroundNode(i, j + 1) = 18 Then
                            G.DrawImage(GetTile(3, 33, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        ElseIf GroundNode(i + 1, j + 1) = 1 Then
                            G.DrawImage(GetTile(1, 31, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        Else
                            G.DrawImage(GetTile(4, 31, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        End If
                    Case 14
                        If GroundNode(i, j - 1) = 17 Then
                            G.DrawImage(GetTile(2, 32, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        ElseIf GroundNode(i - 1, j - 1) = 1 Then
                            G.DrawImage(GetTile(2, 28, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        Else
                            G.DrawImage(GetTile(5, 28, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        End If
                    Case 15
                        If GroundNode(i, j - 1) = 16 Then
                            G.DrawImage(GetTile(1, 32, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        ElseIf GroundNode(i + 1, j - 1) = 1 Then
                            G.DrawImage(GetTile(1, 28, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        Else
                            G.DrawImage(GetTile(4, 28, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                        End If
                    Case 16
                        G.DrawImage(GetTile(3, 31, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                    Case 17
                        G.DrawImage(GetTile(0, 31, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                    Case 18
                        G.DrawImage(GetTile(3, 28, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                    Case 19
                        G.DrawImage(GetTile(0, 28, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)

                    Case Else
                        'G.DrawString(GroundNode(i, j), Me.Font, Brushes.Black, (i - 1) * PZOOM, (H - j) * PZOOM)
                End Select
                'G.DrawString(GroundNode(i, j), Me.Font, Brushes.Black, (i - 1) * PZOOM, (H - j) * PZOOM)
                'G.DrawString(GetCorCode(i, j).X & "," & GetCorCode(i, j).Y, Me.Font, Brushes.Black, (i - 1) * PZOOM, (H - j) * PZOOM + 10)
            Next
        Next
        For i = 0 To W
            For j = 0 To H
                If GroundNode(i, j) = 1 Then
                    R = GetCorCode(i, j)
                    G.DrawImage(GetTile(R.X, R.Y, 1, 1), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                    'G.DrawString(GroundNode(i, j), Me.Font, Brushes.Black, (i - 1) * PZOOM, (H - j) * PZOOM)
                    'G.DrawString(GetCorCode(i, j).X & "," & GetCorCode(i, j).Y, Me.Font, Brushes.Black, (i - 1) * PZOOM, (H - j) * PZOOM + 10)

                End If
            Next
        Next
    End Sub
    Public Sub ReGrdCode()
        Dim i As Integer, j As Integer, m As Integer


        '斜坡
        Dim H As Integer, W As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        Dim CX, CY As Integer
        'GrdType
        '0无 1方 2陡左上 3陡右上 4陡左下 5陡右下 6端左上 7端右上 8端左下 9端右下
        '10  11  12缓大左上 13缓大右上 14缓大左下 15缓大右下  16缓小左上 17缓小右上 18缓小左下 19缓小右下
        '20端左上 21端右上 22端左下 23端右下 24    25    26
        For i = 0 To MapHdr.ObjCount - 1
            Select Case MapObj(i).ID
                Case 87
                    '缓坡
                    CX = (-0.5 + MapObj(i).X / 160)
                    CY = (-0.5 + MapObj(i).Y / 160)
                    If (MapObj(i).Flag \ &H100000) Mod &H2 = 0 Then
                        '左斜
                        Select Case GroundNode(CX + 1, CY + 1)
                            Case 0
                                GroundNode(CX + 1, CY + 1) = 23
                            Case 6, 20, 8, 22
                                GroundNode(CX + 1, CY + 1) = 1
                        End Select
                        Select Case GroundNode(CX + MapObj(i).W, CY + MapObj(i).H)
                            Case 0
                                GroundNode(CX + MapObj(i).W, CY + MapObj(i).H) = 20
                            Case 9, 23, 7, 21
                                GroundNode(CX + MapObj(i).W, CY + MapObj(i).H) = 1
                        End Select
                        'GroundNode(CX + 1, CY + 1) = 23
                        'GroundNode(CX + MapObj(i).W, CY + MapObj(i).H) = 20
                        For j = 1 To MapObj(i).W - 2 Step 2
                            GroundNode(CX + 1 + j, CY + 1 + (j \ 2) + 1) = 19
                            Select Case GroundNode(CX + 1 + j, CY + 1 + (j \ 2))
                                Case 0, 6, 20
                                    GroundNode(CX + 1 + j, CY + 1 + (j \ 2)) = 12
                            End Select
                            'GroundNode(CX + 1 + j, CY + 1 + (j \ 2) + 1) = 12
                            Select Case GroundNode(CX + 2 + j, CY + 1 + (j \ 2) + 1)
                                Case 0, 9, 23
                                    GroundNode(CX + 2 + j, CY + 1 + (j \ 2) + 1) = 15
                            End Select
                            'GroundNode(CX + 2 + j, CY + 1 + (j \ 2) + 2) = 15
                            GroundNode(CX + 2 + j, CY + 1 + (j \ 2)) = 16
                        Next
                    Else
                        '右斜
                        Select Case GroundNode(CX + 1, CY + MapObj(i).H)
                            Case 0
                                GroundNode(CX + 1, CY + MapObj(i).H) = 21
                            Case 6, 8, 20, 22
                                GroundNode(CX + 1, CY + MapObj(i).H) = 1
                        End Select
                        Select Case GroundNode(CX + MapObj(i).W, CY + 1)
                            Case 0
                                GroundNode(CX + MapObj(i).W, CY + 1) = 22
                            Case 9, 7, 23, 21
                                GroundNode(CX + MapObj(i).W, CY + 1) = 1
                        End Select
                        'GroundNode(CX + 1, CY + MapObj(i).H) = 21
                        'GroundNode(CX + MapObj(i).W, CY + 1) = 22
                        For j = 1 To MapObj(i).W - 2 Step 2
                            GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2) - 1) = 17
                            Select Case GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2))
                                Case 0, 22, 8
                                    GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2)) = 14
                            End Select
                            'GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2)) = 14
                            Select Case GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2) - 1)
                                Case 0, 21, 7
                                    GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2) - 1) = 13
                            End Select
                            'GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2) - 1) = 13
                            GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2)) = 18
                        Next
                    End If
                Case 88
                    '陡坡
                    CX = (-0.5 + MapObj(i).X / 160)
                    CY = (-0.5 + MapObj(i).Y / 160)
                    If (MapObj(i).Flag \ &H100000) Mod &H2 = 0 Then
                        '左斜
                        Select Case GroundNode(CX + 1, CY + 1)
                            Case 0
                                GroundNode(CX + 1, CY + 1) = 9
                            Case 6, 8, 20, 22
                                GroundNode(CX + 1, CY + 1) = 1
                        End Select
                        Select Case GroundNode(CX + MapObj(i).W, CY + MapObj(i).H)
                            Case 0
                                GroundNode(CX + MapObj(i).W, CY + MapObj(i).H) = 6
                            Case 9, 7, 23, 21
                                GroundNode(CX + MapObj(i).W, CY + MapObj(i).H) = 1
                        End Select
                        'GroundNode(CX + 1, CY + 1) = 9
                        'GroundNode(CX + MapObj(i).W, CY + MapObj(i).H) = 6
                        For j = 1 To MapObj(i).W - 2
                            'GroundNode(CX + 1 + j, CY + 1 + j) = 5
                            Select Case GroundNode(CX + 1 + j, CY + 1 + j)
                                Case 0, 9, 23
                                    GroundNode(CX + 1 + j, CY + 1 + j) = 5
                            End Select
                            'GroundNode(CX + 1 + j, CY + j) = 2
                            Select Case GroundNode(CX + 1 + j, CY + j)
                                Case 0, 6, 20
                                    GroundNode(CX + 1 + j, CY + j) = 2
                            End Select
                        Next
                    Else
                        '右斜
                        Select Case GroundNode(CX + 1, CY + MapObj(i).H)
                            Case 0
                                GroundNode(CX + 1, CY + MapObj(i).H) = 7
                            Case 6, 8, 20, 22
                                GroundNode(CX + 1, CY + MapObj(i).H) = 1
                        End Select
                        Select Case GroundNode(CX + MapObj(i).W, CY + 1)
                            Case 0
                                GroundNode(CX + MapObj(i).W, CY + 1) = 8
                            Case 9, 7, 23, 21
                                GroundNode(CX + MapObj(i).W, CY + 1) = 1
                        End Select

                        'GroundNode(CX + MapObj(i).W, CY + 1) = 8
                        'GroundNode(CX + 1, CY + MapObj(i).W) = 7
                        For j = 1 To MapObj(i).W - 2
                            'GroundNode(CX + 1 + j, CY + MapObj(i).W - j) = 4
                            Select Case GroundNode(CX + 1 + j, CY + MapObj(i).W - j)
                                Case 0, 8, 22
                                    GroundNode(CX + 1 + j, CY + MapObj(i).W - j) = 4
                            End Select
                            'GroundNode(CX + 1 + j, CY + MapObj(i).W - 1 - j) = 3
                            Select Case GroundNode(CX + 1 + j, CY + MapObj(i).W - 1 - j)
                                Case 0, 7, 21
                                    GroundNode(CX + 1 + j, CY + MapObj(i).W - 1 - j) = 3
                            End Select
                        Next
                    End If
            End Select
        Next
        'GrdType
        '0无 1方 2陡左上 3陡右上 4陡左下 5陡右下 6端左上 7端右上 8端左下 9端右下
        '10  11  12缓大左上 13缓大右上 14缓大左下 15缓大右下  16缓小左上 17缓小右上 18缓小左下 19缓小右下
        '20端左上 21端右上 22端左下 23端右下 24    25    26
        For m = 0 To 2
            For i = 0 To W
                For j = 0 To H
                    Select Case GroundNode(i + 1, j + 1)
                        Case 2, 16 'UL
                            If GroundNode(i + 2, j + 1) > 0 Or GroundNode(i + 1, j) > 0 Then GroundNode(i + 1, j + 1) = 1
                        Case 3, 17 'UR
                            If GroundNode(i, j + 1) > 0 Or GroundNode(i + 1, j) > 0 Then GroundNode(i + 1, j + 1) = 1
                        Case 4, 18 'DL
                            If GroundNode(i + 2, j + 1) > 0 Or GroundNode(i + 1, j + 2) > 0 Then GroundNode(i + 1, j + 1) = 1
                        Case 5, 19 'DR
                            If GroundNode(i, j + 1) > 0 Or GroundNode(i + 1, j + 2) > 0 Then GroundNode(i + 1, j + 1) = 1
                        Case 12
                            If GroundNode(i + 1, j) > 0 Or GroundNode(i + 2, j + 1) = 1 Then GroundNode(i + 1, j + 1) = 1
                        Case 13
                            If GroundNode(i + 1, j) > 0 Or GroundNode(i, j + 1) = 1 Then GroundNode(i + 1, j + 1) = 1
                        Case 14
                            If GroundNode(i + 1, j + 2) > 0 Or GroundNode(i + 2, j + 1) = 1 Then GroundNode(i + 1, j + 1) = 1
                        Case 15
                            If GroundNode(i + 1, j + 2) > 0 Or GroundNode(i, j + 1) = 1 Then GroundNode(i + 1, j + 1) = 1
                    End Select
                Next
            Next
        Next
    End Sub
    Public Sub DrawGrd(IO As Boolean)
        Dim i As Integer
        Dim K As Image

        K = GetTile(0, 12, 1, 1) 'Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG")
        If IO Then
            '终点
            Select Case LH.GameStyle
                Case 12621 '1
                    If MapHdr.Theme = 2 Then
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27A.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY - 4) * PZoom, PZoom * 2, PZoom * 4)
                        For i = 0 To 13
                            G.DrawImage(GetTile(15, 15, 1, 1), CSng((LH.GoalX / 10 - 14.5 + i) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY) * PZoom, PZoom, PZoom)
                        Next
                    Else
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY - 11) * PZoom, PZoom, PZoom * 11)
                    End If
                Case 13133 '3
                    If MapHdr.Theme = 2 Then
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27A.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY - 4) * PZoom, PZoom * 2, PZoom * 4)
                        For i = 0 To 13
                            G.DrawImage(GetTile(15, 15, 1, 1), CSng((LH.GoalX / 10 - 14.5 + i) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY) * PZoom, PZoom, PZoom)
                        Next
                    Else
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY - 5) * PZoom, PZoom * 2, PZoom * 2)
                    End If
                Case 22349 'W
                    If MapHdr.Theme = 2 Then
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27A.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY - 4) * PZoom, PZoom * 2, PZoom * 4)
                        For i = 0 To 13
                            G.DrawImage(GetTile(15, 15, 1, 1), CSng((LH.GoalX / 10 - 14.5 + i) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY) * PZoom, PZoom, PZoom)
                        Next
                    Else
                        'For i = 1 To 8
                        '	G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27C.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZOOM), (MapHdr.BorT \ 16 - LH.GoalY - i) * PZOOM, PZOOM, PZOOM)
                        '	G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27E.PNG"), CSng((LH.GoalX / 10 + 1.5) * PZOOM), (MapHdr.BorT \ 16 - LH.GoalY - i) * PZOOM, PZOOM, PZOOM)
                        'Next
                        'G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27B.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZOOM), (MapHdr.BorT \ 16 - LH.GoalY - 9) * PZOOM, PZOOM, PZOOM)
                        'G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27D.PNG"), CSng((LH.GoalX / 10 + 1.5) * PZOOM), (MapHdr.BorT \ 16 - LH.GoalY - 9) * PZOOM, PZOOM, PZOOM)

                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27F.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), CSng(MapHdr.BorT \ 16 - LH.GoalY - 8.5) * PZoom, PZoom, PZoom * 9)
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10) * PZoom), CSng(MapHdr.BorT \ 16 - LH.GoalY - 8) * PZoom, PZoom * 2, PZoom)
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27G.PNG"), CSng((LH.GoalX / 10 + 1.5) * PZoom), CSng(MapHdr.BorT \ 16 - LH.GoalY - 8.5) * PZoom, PZoom, PZoom * 9)

                    End If
                Case 21847 'U
                    If MapHdr.Theme = 2 Then
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27A.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY - 4) * PZoom, PZoom * 2, PZoom * 4)
                        For i = 0 To 13
                            G.DrawImage(GetTile(15, 15, 1, 1), CSng((LH.GoalX / 10 - 14.5 + i) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY) * PZoom, PZoom, PZoom)
                        Next
                    Else
                        G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY - 11) * PZoom, PZoom, PZoom * 11)
                    End If
                Case 22323 '3D
                    G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MapHdr.BorT \ 16 - LH.GoalY - 11) * PZoom, PZoom, PZoom * 11)
            End Select

            '旧代码弃用
            '	'终点地面 10*LH.GoalY 
            '	For j = (LH.GoalX - 5) / 10 To (LH.GoalX - 5) / 10 + 9
            '		For i = 0 To LH.GoalY - 1
            '			R = GrdLoc(GetGrdType(GetGrdCode(j, i)))
            '			G.DrawImage(GetTile(R.X, R.Y, 1, 1), j * PZOOM, (MapHdr.BorT \ 16 - 1) * PZOOM - i * PZOOM, PZOOM, PZOOM)
            '		Next
            '	Next

            '	'起点地面 7*LH.StartY
            '	For j = 0 To 6
            '		For i = 0 To LH.StartY - 1
            '			R = GrdLoc(GetGrdType(GetGrdCode(j, i)))
            '			G.DrawImage(GetTile(R.X, R.Y, 1, 1), j * PZOOM, (MapHdr.BorT \ 16 - 1) * PZOOM - i * PZOOM, PZOOM, PZOOM)
            '		Next
            '	Next

            G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\38.PNG"), 1 * PZoom, (MapHdr.BorT \ 16 - LH.StartY - 3) * PZoom, PZoom * 3, PZoom * 3)
        End If

        ''地面
        'For i = 0 To MapHdr.GroundCount - 1
        '	R = GrdLoc(GetGrdType(GetGrdCode(MapGrd(i).X, MapGrd(i).Y)))
        '	G.DrawImage(GetTile(R.X, R.Y, 1, 1), MapGrd(i).X * PZOOM, (MapHdr.BorT \ 16 - 1) * PZOOM - MapGrd(i).Y * PZOOM, PZOOM, PZOOM)
        'Next
    End Sub
    Public Function GetGrdCode(x As Integer, y As Integer) As String
        Dim S As String = ""
        'C
        S += IIf(GroundNode(x, y + 2) = 0, "0", "1")
        S += IIf(GroundNode(x + 2, y + 2) = 0, "0", "1")
        S += IIf(GroundNode(x, y) = 0, "0", "1")
        S += IIf(GroundNode(x + 2, y) = 0, "0", "1")

        'E ULRD
        S += IIf(GroundNode(x + 1, y + 2) = 0, "0", "1")
        S += IIf(GroundNode(x, y + 1) = 0, "0", "1")
        S += IIf(GroundNode(x + 2, y + 1) = 0, "0", "1")
        S += IIf(GroundNode(x + 1, y) = 0, "0", "1")

        GetGrdCode = S
    End Function
    Public Function GetGrdType(A As String) As Integer
        Dim p As Integer, i As Integer
        p = 0
        For i = 0 To 7
            p += IIf(Strings.Mid(A, 8 - i, 1) = "0", 0, 1) * 2 ^ i
        Next
        GetGrdType = p
    End Function
    Public Sub SetGrdLoc()
        Dim GL As String = "0D,4D,1D,AD,3D,9D,2D,CD,6D,5D,8D,ED,7D,DD,BD,FD,
							0D,4D,1D,2F,3D,9D,2D,4E,6D,5D,8D,0E,7D,DD,BD,8E,
							0D,4D,1D,AD,3D,4F,2D,5E,6D,5D,8D,ED,7D,1E,BD,9E,
							0D,4D,1D,2F,3D,4F,2D,3F,6D,5D,8D,0E,7D,1E,BD,CE,
							0D,4D,1D,AD,3D,9D,2D,CD,6D,5D,8F,2E,7D,DD,6E,AE,
							0D,4D,1D,2F,3D,9D,2D,4E,6D,5D,8F,5F,7D,DD,6E,EE,
							0D,4D,1D,AD,3D,4F,2D,5E,6D,5D,8F,2E,AF,1E,6E,1F,
							0D,4D,1D,2F,3D,4F,2D,3F,6D,5D,8F,5F,7D,1E,6E,BF,
							0D,4D,1D,AD,3D,9D,2D,CD,6D,5D,8D,ED,AF,3E,7E,BE,
							0D,4D,1D,2F,3D,9D,2D,4E,6D,5D,8D,0E,7D,3E,7E,0F,
							0D,4D,1D,AD,3D,4F,2D,5E,6D,5D,8D,ED,AF,7F,7E,FE,
							0D,4D,1D,2F,3D,4F,2D,3F,6D,5D,8D,0E,AF,7F,7E,CF,
							0D,4D,1D,AD,3D,9D,2D,CD,6D,5D,8F,2E,AF,3E,9F,DE,
							0D,4D,1D,2F,3D,9D,2D,4E,6D,5D,8F,5F,AF,3E,9F,DF,
							0D,4D,1D,AD,3D,4F,2D,5E,6D,5D,8F,2E,AF,7F,9F,EF,
							0D,4D,1D,2F,3D,4F,2D,3F,6D,5D,8F,5F,AF,7F,9F,6F"
        Dim GS() As String = GL.Split(",")
        Dim I As Integer
        For I = 0 To 255
            GrdLoc(I).X = Val("&H" & Strings.Left(GS(I), 1))
            GrdLoc(I).Y = Val("&H" & Strings.Right(GS(I), 1))
        Next
    End Sub
    Dim NowIO As Integer
    Public Sub DrawItem(K As String, L As Boolean)
        'On Error Resume Next
        Dim i As Integer, j As Integer, j2 As Integer
        Dim H As Integer, W As Integer, PR As String
        Dim LX, LY, KX, KY As Integer
        Dim PP As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        Dim P As String = PT

        For i = 0 To MapHdr.ObjCount - 1
            PR = ""
            If InStr(K, "/" & MapObj(i).ID.ToString & "/") > 0 Then

                If MapObj(i).ID = 105 Then
                    If (MapObj(i).Flag \ &H400) Mod 2 = 1 Then
                        KY = 0
                    Else
                        KY = -3 * PZoom
                    End If
                    ObjLinkType(MapObj(i).LID + 1) = 105

                    If (MapObj(i).Flag \ &H80) Mod 2 = 1 Then
                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\105A.PNG"),
                        CSng(-1.5 + MapObj(i).X / 160) * PZoom, H * PZoom - CSng(0.5 + MapObj(i).Y / 160) * PZoom + KY, PZoom * 3, PZoom * 5)
                    Else
                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\105.PNG"),
                        CSng(-1.5 + MapObj(i).X / 160) * PZoom, H * PZoom - CSng(0.5 + MapObj(i).Y / 160) * PZoom + KY, PZoom * 3, PZoom * 5)
                    End If
                    'CID
                    If MapObj(i).CID <> -1 Then
                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.Tostring & "\CID\C.PNG"),
                        ' CSNG((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM),
                        'H * PZOOM - CSNG((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM),
                        'PZOOM, PZOOM)
                        If (MapObj(i).CFlag \ &H4) Mod 2 = 1 Then
                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & "A.PNG"), LX, LY + KY, PZoom, PZoom)
                        Else
                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & ".PNG"), LX, LY + KY, PZoom, PZoom)
                        End If
                    End If
                Else

                    Select Case ObjLinkType(MapObj(i).LID + 1)
                        Case 9 '管道L
                            KX = 0
                            KY = ((Math.Min(MapObj(i).W, MapObj(i).H) - 1) / 2) * PZoom
                        Case 105 '夹子L
                            KX = 0
                            KY = -PZoom / 4
                        Case 59 '轨道
                            KX = 0
                            KY = ((Math.Min(MapObj(i).W, MapObj(i).H) - 1) / 2) * PZoom
                        Case 31
                            KX = 0
                            KY = 0 ' 3 * PZOOM
                        Case 106 '树
                            KX = 0
                            KY = 0
                        Case 0
                            KX = 0
                            KY = 0
                    End Select

                    If MapObj(i).LID + 1 = 0 AndAlso Not L Or MapObj(i).LID + 1 > 0 AndAlso L Or MapObj(i).ID = 9 Then
                        Select Case MapObj(i).ID
                            Case 89 '卷轴相机
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(PT & "\IMG\CMR\1.PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 14
                                '蘑菇平台
                                If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    j2 = 3
                                ElseIf (MapObj(i).Flag \ &H80000) Mod 2 = 1 Then
                                    j2 = 4
                                Else
                                    j2 = 2
                                End If
                                For j = 0 To MapObj(i).W - 1
                                    If j = 0 Then
                                        G.DrawImage(GetTile(3, j2, 1, 1),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(i).W - 1 Then
                                        G.DrawImage(GetTile(5, j2, 1, 1),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G.DrawImage(GetTile(4, j2, 1, 1),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                            Case 16
                                '半碰撞地形
                                If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    j2 = 10
                                ElseIf (MapObj(i).Flag \ &H80000) Mod 2 = 1 Then
                                    j2 = 13
                                Else
                                    j2 = 7
                                End If
                                For j = 0 To MapObj(i).W - 1
                                    If j = 0 Then
                                        G.DrawImage(GetTile(j2, 3, 1, 1),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(i).W - 1 Then
                                        G.DrawImage(GetTile(j2 + 2, 3, 1, 1),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G.DrawImage(GetTile(j2 + 1, 3, 1, 1),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                            Case 71
                                '3D半碰撞地形
                                Dim TL, TM, TR As String
                                For j2 = 0 To MapObj(i).H - 1
                                    Select Case j2
                                        Case 0
                                            TL = "71"
                                            TM = "71A"
                                            TR = "71B"
                                        Case MapObj(i).H - 1
                                            TL = "71F"
                                            TM = "71G"
                                            TR = "71H"
                                        Case Else
                                            TL = "71C"
                                            TM = "71D"
                                            TR = "71E"
                                    End Select
                                    For j = 0 To MapObj(i).W - 1
                                        If j = 0 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & TL & ".PNG"),
                                        CSng((j + MapObj(i).X \ 160) * PZoom), (H + j2) * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                        ElseIf j = MapObj(i).W - 1 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & TR & ".PNG"),
                                        CSng((j + MapObj(i).X \ 160) * PZoom), (H + j2) * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                        Else
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & TM & ".PNG"),
                                        CSng((j + MapObj(i).X \ 160) * PZoom), (H + j2) * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                        End If
                                    Next
                                Next
                            Case 17
                                '桥
                                For j = 0 To MapObj(i).W - 1
                                    If j = 0 Then
                                        G.DrawImage(GetTile(0, 2, 1, 2),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    ElseIf j = MapObj(i).W - 1 Then
                                        G.DrawImage(GetTile(2, 2, 1, 2),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    Else
                                        G.DrawImage(GetTile(1, 2, 1, 2),
                                            CSng((j + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    End If
                                Next
                            Case 113, 132
                                '蘑菇跳台 开关跳台
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    For j = 0 To MapObj(i).W - 1
                                        If j = 0 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & "D.PNG"),
                                            CSng((j - MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                        ElseIf j = MapObj(i).W - 1 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & "E.PNG"),
                                            CSng((j - MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                        Else
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & "C.PNG"),
                                            CSng((j - MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                        End If
                                    Next
                                Else
                                    For j = 0 To MapObj(i).W - 1
                                        If j = 0 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & "A.PNG"),
                                            CSng((j - MapObj(i).W / 2 + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                        ElseIf j = MapObj(i).W - 1 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & "B.PNG"),
                                            CSng((j - MapObj(i).W / 2 + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                        Else
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & ".PNG"),
                                            CSng((j - MapObj(i).W / 2 + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                        End If
                                    Next
                                End If

                            Case 66, 67, 90
                                '箭头 单向板 中间旗 
                                Select Case MapObj(i).Flag
                                    Case &H6000040
                                        PR = ""
                                    Case &H6400040
                                        PR = "A"
                                    Case &H6800040
                                        PR = "B"
                                    Case &H6C00040
                                        PR = "C"
                                    Case &H7000040
                                        PR = "D"
                                    Case &H7400040
                                        PR = "E"
                                    Case &H7800040
                                        PR = "F"
                                    Case &H7C00040
                                        PR = "G"
                                End Select
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H / 2 + MapObj(i).Y / 160) * PZoom)
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                                CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 83 '狼牙棒
                                Select Case MapObj(i).Flag
                                    Case &H6000040
                                        PR = ""
                                    Case &H6400040
                                        PR = "A"
                                    Case &H6800040
                                        PR = "B"
                                    Case &H6C00040
                                        PR = "C"
                                    Case &H7000040
                                        PR = "D"
                                    Case &H7400040
                                        PR = "E"
                                    Case &H7800040
                                        PR = "F"
                                    Case &H7C00040
                                        PR = "G"
                                End Select
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H / 2 + MapObj(i).Y / 160) * PZoom)
                                G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"), 0.7),
                                                CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 64
                                '藤蔓
                                For j = 1 To MapObj(i).H
                                    If j = 1 Then
                                        G.DrawImage(GetTile(13, 7, 1, 1),
                                                CSng(-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom, H * PZoom - CSng((j + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(i).H Then
                                        G.DrawImage(GetTile(15, 7, 1, 1),
                                                CSng(-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom, H * PZoom - CSng((j + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G.DrawImage(GetTile(14, 7, 1, 1),
                                                CSng(-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom, H * PZoom - CSng((j + MapObj(i).Y \ 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                            Case 4, 5, 6, 21, 22, 23, 29, 63, 79, 99, 100, 43, 8
                                '4,4A 5 6 8 8A 21 22 23 23A 29 43 49 63 79 79A 92 99 100 100A
                                '砖 问号 硬砖 地面 竹轮 云砖 刺 金币
                                '音符  隐藏 
                                '冰砖  P砖 开关 开关砖

                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PP = 1
                                Else
                                    PP = 0
                                End If
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(GetTile(TileLoc(MapObj(i).ID, PP).X, TileLoc(MapObj(i).ID, PP).Y, 1, 1),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                            Case 108
                                '闪烁砖
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                            Case 106 '树
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H + 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\106.PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * 4, PZoom * 4)
                                For j = 4 To MapObj(i).H - 1
                                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\106A.PNG"),
                                    CSng((-MapObj(i).W / 2 + 1.5 + MapObj(i).X / 160) * PZoom),
                                    (H + j) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom, PZoom)
                                Next
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\106B.PNG"),
                                    CSng((-MapObj(i).W / 2 + 1 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((-0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * 2, PZoom)
                            Case 85, 119
                                '机动砖 轨道砖
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                DrawMoveBlock(MapObj(i).ID, MapObj(i).Ex, MapObj(i).X, MapObj(i).Y)
                            Case 94
                                '斜传送带
                                Dim C1, C2 As Point
                                If (MapObj(i).Flag \ &H400000) Mod 2 = 0 Then
                                    C1 = New Point(8, 0)
                                    C2 = New Point(4, 16)
                                Else
                                    C1 = New Point(13, 24)
                                    C2 = New Point(10, 22)
                                End If
                                If (MapObj(i).Flag \ &H200000) Mod &H2 = 0 Then
                                    '左斜
                                    LX = CSng((-1 + MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                    LY = (H - 0.5 - MapObj(i).H / 2) * PZoom - CSng((-0.5 + MapObj(i).Y / 160) * PZoom)
                                    G.DrawImage(GetTile(C1.X, C1.Y, 1, 1),
                                            CSng((-0.5 + MapObj(i).X / 160) * PZoom), (H - 1) * PZoom - CSng((-0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    G.DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1),
                                            CSng((MapObj(i).W - 1.5 + MapObj(i).X / 160) * PZoom), (H - 1) * PZoom - CSng((MapObj(i).H - 1.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    For j = 1 To MapObj(i).W - 2
                                        G.DrawImage(GetTile(C2.X + 1, C2.Y, 1, 2),
                                                CSng((j - 0.5 + MapObj(i).X / 160) * PZoom), (H - 1) * PZoom - CSng((j - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    Next

                                Else
                                    '右斜
                                    LX = CSng((-1 + MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                    LY = (H - 0.5 - MapObj(i).H / 2) * PZoom - CSng((-0.5 + MapObj(i).Y / 160) * PZoom)
                                    G.DrawImage(GetTile(C1.X, C1.Y, 1, 1),
                                            CSng((-0.5 + MapObj(i).X / 160) * PZoom), (H - 1) * PZoom - CSng((MapObj(i).H - 1.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    G.DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1),
                                            CSng((MapObj(i).W - 1.5 + MapObj(i).X / 160) * PZoom), (H - 1) * PZoom - CSng((-0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    For j = 1 To MapObj(i).W - 2
                                        G.DrawImage(GetTile(C2.X + 4, C2.Y, 1, 2),
                                                CSng((j - 0.5 + MapObj(i).X / 160) * PZoom), (H - 1) * PZoom - CSng((-0.5 - j + MapObj(i).H + MapObj(i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    Next
                                End If


                                If (MapObj(i).Flag \ &H40000) Mod 2 = 0 Then
                                    If (MapObj(i).Flag \ &H8) Mod 2 = 1 Then
                                        G.DrawImage(Image.FromFile(P & "\img\CMN\A1.PNG"), LX, LY, PZoom, PZoom)
                                    Else
                                        G.DrawImage(Image.FromFile(P & "\img\CMN\A0.PNG"), LX, LY, PZoom, PZoom)
                                    End If
                                Else
                                    If (MapObj(i).Flag \ &H8) Mod 2 = 1 Then
                                        G.DrawImage(Image.FromFile(P & "\img\CMN\A3.PNG"), LX, LY, PZoom, PZoom)
                                    Else
                                        G.DrawImage(Image.FromFile(P & "\img\CMN\A2.PNG"), LX, LY, PZoom, PZoom)
                                    End If
                                End If


                            Case 53
                                '传送带
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom)
                                Dim C1 As Point
                                If (MapObj(i).Flag \ &H400000) Mod 2 = 0 Then
                                    C1 = New Point(8, 0)
                                Else
                                    C1 = New Point(13, 24)
                                End If

                                For j = 0 To MapObj(i).W - 1
                                    If j = 0 Then
                                        G.DrawImage(GetTile(C1.X, C1.Y, 1, 1),
                                        CSng((j - 0.5 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(i).W - 1 Then
                                        G.DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1),
                                        CSng((j - 0.5 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G.DrawImage(GetTile(C1.X + 1, C1.Y, 1, 1),
                                        CSng((j - 0.5 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    End If

                                    If (MapObj(i).Flag \ &H40000) Mod 2 = 0 Then
                                        If (MapObj(i).Flag \ &H8) Mod 2 = 1 Then
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\A1.PNG"), LX + CInt((-0.5 + MapObj(i).W / 2) * PZoom), LY, PZoom, PZoom)
                                        Else
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\A0.PNG"), LX + CInt((-0.5 + MapObj(i).W / 2) * PZoom), LY, PZoom, PZoom)
                                        End If
                                    Else
                                        If (MapObj(i).Flag \ &H8) Mod 2 = 1 Then
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\A3.PNG"), LX + CInt((-0.5 + MapObj(i).W / 2) * PZoom), LY, PZoom, PZoom)
                                        Else
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\A2.PNG"), LX + CInt((-0.5 + MapObj(i).W / 2) * PZoom), LY, PZoom, PZoom)
                                        End If
                                    End If
                                Next
                            Case 9
                                '管道
                                ObjLinkType(MapObj(i).LID + 1) = 9
                                '0绿 4红 8蓝 C橙
                                PP = ((MapObj(i).Flag \ &H10000) Mod &H10) \ 4
                                'Select Case (MapObj(i).Flag \ &H10000) Mod &H10
                                '	Case &H0
                                '		PR = "9"
                                '	Case &H4
                                '		PR = "9R"
                                '	Case &H8
                                '		PR = "9U"
                                '	Case &HC
                                '		PR = "9P"
                                'End Select
                                '00右 20左 40上 60下
                                '以相对左下角为准
                                Select Case MapObj(i).Flag Mod &H80
                                    Case &H0 'R
                                        LX = CSng((MapObj(i).H - 1 - 1 - 0.5 + MapObj(i).X / 160) * PZoom)
                                        LY = H * PZoom - CSng((MapObj(i).Y / 160) * PZoom)
                                        For j = 0 To MapObj(i).H - 2
                                            G.DrawImage(GetTile(PipeLoc(PP, 4).X, PipeLoc(PP, 4).Y, 1, 2),
                                                            CSng((j - 0.5 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, 2 * PZoom)
                                        Next
                                        G.DrawImage(GetTile(PipeLoc(PP, 3).X, PipeLoc(PP, 3).Y, 1, 2),
                                                        CSng((j - 0.5 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), PZoom, 2 * PZoom)
                                    Case &H20 'L
                                        LX = CSng((-MapObj(i).H + 1 + 1 - 0.5 + MapObj(i).X / 160) * PZoom)
                                        LY = H * PZoom - CSng((1 + MapObj(i).Y / 160) * PZoom)
                                        For j = 0 To MapObj(i).H - 2
                                            G.DrawImage(GetTile(PipeLoc(PP, 4).X, PipeLoc(PP, 4).Y, 1, 2),
                                                            CSng((-j - 0.5 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(i).Y / 160) * PZoom), PZoom, 2 * PZoom)
                                        Next
                                        G.DrawImage(GetTile(PipeLoc(PP, 2).X, PipeLoc(PP, 2).Y, 1, 2),
                                                        CSng((-j - 0.5 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(i).Y / 160) * PZoom), PZoom, 2 * PZoom)
                                    Case &H40 'U
                                        LX = CSng((+MapObj(i).X / 160) * PZoom)
                                        LY = (H - MapObj(i).H + 1 + 1) * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom)
                                        For j = 0 To MapObj(i).H - 2
                                            G.DrawImage(GetTile(PipeLoc(PP, 5).X, PipeLoc(PP, 5).Y, 2, 1),
                                                            CSng((-0.5 + MapObj(i).X / 160) * PZoom), (H - j) * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), 2 * PZoom, PZoom)
                                        Next
                                        G.DrawImage(GetTile(PipeLoc(PP, 0).X, PipeLoc(PP, 0).Y, 2, 1),
                                                        CSng((-0.5 + MapObj(i).X / 160) * PZoom), (H - j) * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), 2 * PZoom, PZoom)
                                    Case &H60 'D
                                        LX = CSng((-1 + MapObj(i).X / 160) * PZoom)
                                        LY = (H + MapObj(i).H - 1 - 1) * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom)
                                        For j = 0 To MapObj(i).H - 2
                                            G.DrawImage(GetTile(PipeLoc(PP, 5).X, PipeLoc(PP, 5).Y, 2, 1),
                                                            CSng((-1.5 + MapObj(i).X / 160) * PZoom), (H + j) * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), 2 * PZoom, PZoom)
                                        Next
                                        G.DrawImage(GetTile(PipeLoc(PP, 1).X, PipeLoc(PP, 1).Y, 2, 1),
                                                        CSng((-1.5 + MapObj(i).X / 160) * PZoom), (H + j) * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom), 2 * PZoom, PZoom)
                                End Select
                                PR = ((MapObj(i).Flag Mod &H1000000) \ &H100000 - 1).ToString
                                If PR <> "-1" Then
                                    G.DrawImage(Image.FromFile(P & "\img\CMN\C" & PR & ".PNG"), LX, LY, PZoom, PZoom)
                                End If

                            Case 55
                                '门
                                If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                ElseIf (MapObj(i).Flag \ &H80000) Mod 2 = 1 Then
                                    PR = "B"
                                Else
                                    PR = ""
                                End If
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\55" & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                PR = ((MapObj(i).Flag Mod &H800000) \ &H200000).ToString
                                G.DrawImage(Image.FromFile(P & "\img\CMN\C" & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    (H + 1) * PZoom - CSng((MapObj(i).H + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                            Case 97
                                '传送箱
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\97" & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                PR = ((MapObj(i).Flag Mod &H800000) \ &H200000).ToString
                                G.DrawImage(Image.FromFile(P & "\img\CMN\C" & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                            Case 84
                                '蛇
                                For j = 0 To MapObj(i).W - 1
                                    If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\84A.PNG"),
                                                CSng((j - MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\84.PNG"),
                                                CSng((j - MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                                '&H10方向
                                DrawSnake(MapObj(i).Ex, MapObj(i).X, MapObj(i).Y, MapObj(i).W, MapObj(i).H)



                            Case 68, 82
                                '齿轮 甜甜圈

                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = (H - 1.5) * PZoom - CSng((MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & ".PNG"), 0.7),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                            Case 0, 10, 15, 19, 20, 35,
                                 48, 56, 57, 60, 76, 92, 95, 102, 72, 50, 51, 65, 80, 114, 119,
                                  77, 104, 120, 121, 122, 123, 124, 125, 126, 112, 127, 128, 129, 130, 131,
                                    96, 117, 86
                                '板栗  金币 弹簧 炸弹 P POW 蘑菇 
                                ' 无敌星 鱿鱼 鱼
                                '黑花 火球  风  红币 钥匙  地鼠 慢慢龟汽车 跳跳怪 跳跳鼠 蜜蜂 冲刺砖块 尖刺鱼 !方块
                                '奔奔  太阳 库巴七人 木箱 纸糊道具
                                '蚂蚁 斗斗 乌卡
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 33
                                ' 1UP 
                                If MapHdr.Theme = 0 And MapHdr.Flag = 2 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                            Case 74
                                '加邦 
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    If MapHdr.Theme = 6 Then
                                        PR = "B"
                                    Else
                                        PR = "A"
                                    End If
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                            Case 42
                                '飞机
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 OrElse (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + MapObj(i).H - 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    (H + MapObj(i).H - 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * 2, PZoom * 2)
                            Case 34
                                '火花 
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                        PR = "C"
                                    Else
                                        PR = "A"
                                    End If
                                Else
                                    If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                        PR = "B"
                                    Else
                                        PR = ""
                                    End If
                                End If


                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                            Case 81, 116
                                'USA  锤子

                                If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If

                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                            Case 44
                                '大蘑菇

                                If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If

                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 12
                                '咚咚
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = (H + MapObj(i).H / 2 - 0.5) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\12.PNG"), 0.7),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                                If MapObj(i).LID = -1 Then
                                    Select Case MapObj(i).Flag Mod &H100
                                        Case &H40, &H42, &H44
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\E1.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H48, &H4A, &H4C
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\E2.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H50, &H52, &H54
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\E0.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H58, &H5A, &H5C
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\E3.PNG"), LX, LY, PZoom, PZoom)
                                    End Select
                                End If

                            Case 41
                                '幽灵
                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                Select Case LH.GameStyle
                                    Case 22323
                                        If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41D.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Else
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        End If
                                    Case Else
                                        If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41A.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        ElseIf (MapObj(i).Flag \ &H1000000) Mod &H8 = &H4 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41C.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        ElseIf (MapObj(i).Flag \ &H100) Mod 2 = 1 Then
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41B.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Else
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        End If
                                End Select


                            Case 28, 25, 18
                                '钢盔 刺龟 P
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & "A.PNG"),
                                        CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                ElseIf (MapObj(i).Flag \ &H1000000) Mod 8 = &H6 Then
                                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & ".PNG"),
                                        CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                Else
                                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & "B.PNG"),
                                        CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                End If
                            Case 40
                                '小刺龟
                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + MapObj(i).W) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    Select Case (MapObj(i).Flag \ &H1000000) Mod 8
                                    '方向6上 4下 0左 2右
                                        Case &H0 'L
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40B0.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Case &H2 'R
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40B2.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Case &H4 'D
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40B4.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Case &H6 'U
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40B6.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                    End Select
                                Else
                                    Select Case (MapObj(i).Flag \ &H1000000) Mod 8
                                    '方向6上 4下 0左 2右
                                        Case &H0 'L
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40A0.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Case &H2 'R
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40A2.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Case &H4 'D
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40A4.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Case &H6 'U
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40A6.PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                    End Select
                                End If
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                            Case 2
                                '绿花
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "2B"
                                Else
                                    PR = "2A"
                                End If
                                Select Case (MapObj(i).Flag \ &H1000000) Mod &H8
                            '方向6上 4下 0左 2右
                                    Case &H0 'L
                                        LX = CSng((MapObj(i).H / 2 - 1 + MapObj(i).X / 160) * PZoom)
                                        LY = (H + MapObj(i).W + (MapObj(i).W \ 2) / 2) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & PR & "0.PNG"),
                                        CSng((-MapObj(i).W * 3 / 2 + MapObj(i).X / 160) * PZoom),
                                        (H + MapObj(i).W) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(i).W * 2, PZoom * MapObj(i).H)
                                    Case &H2 'R
                                        LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                        LY = (H + MapObj(i).W + (MapObj(i).W \ 2) / 2) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & PR & "2.PNG"),
                                        CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                        (H + MapObj(i).W) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(i).W * 2, PZoom * MapObj(i).H)
                                    Case &H4 'D
                                        LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                        LY = (H + MapObj(i).W) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & PR & "4.PNG"),
                                        CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                        (H + MapObj(i).W) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(i).W, PZoom * MapObj(i).H * 2)
                                    Case &H6 'U
                                        LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                        LY = (H + MapObj(i).H + (MapObj(i).W \ 2)) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & PR & "6.PNG"),
                                        CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                        H * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(i).W, PZoom * MapObj(i).H * 2)
                                End Select
                            Case 107
                                '长长吞食花
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "E"
                                Else
                                    PR = ""
                                End If
                                Select Case MapObj(i).Flag \ &H1000000
                                    Case &H0
                                        PR += "C"
                                    Case &H2
                                        PR += "A"
                                    Case &H4
                                        PR += "B"
                                    Case &H6
                                        PR += ""
                                End Select
                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\107" & PR & ".PNG"), LX, LY, PZoom * 2, PZoom * 2)
                                DrawCrp(MapObj(i).Ex, MapObj(i).X, MapObj(i).Y)
                            Case 32
                                '大炮弹

                                Select Case MapObj(i).Flag
                                    Case &H6000040
                                        PR = ""
                                    Case &H6400040
                                        PR = "A"
                                    Case &H6800040
                                        PR = "B"
                                    Case &H6C00040
                                        PR = "C"
                                    Case &H6000044
                                        PR = "D"
                                    Case &H6400044
                                        PR = "E"
                                    Case &H6800044
                                        PR = "F"
                                    Case &H6C00044
                                        PR = "G"
                                    Case &H7000040
                                        PR = "H"
                                End Select
                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\32" & PR & ".PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 1, 46, 52, 58
                                '慢慢龟，碎碎龟，花花，扳手
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H * 2)
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                            Case 30
                                '裁判
                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 1 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\30.PNG"), LX, LY, PZoom, PZoom * 2)
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\31.PNG"), LX - PZoom \ 2, LY + PZoom \ 2, PZoom * 2, PZoom)
                            Case 31
                                '裁判云
                                ObjLinkType(MapObj(i).LID + 1) = 31
                                LX = CSng((-MapObj(i).W / 2 - 0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom)
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\31.PNG"), LX, LY, PZoom * 2, PZoom)

                            Case 45 '鞋 耀西
                                Select Case LH.GameStyle
                                    Case 21847, 22349 'U W
                                        If MapObj(i).W = 2 Then
                                            PR = "A"
                                        Else
                                            PR = ""
                                        End If
                                    Case Else
                                        If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                            PR = "A"
                                        Else
                                            PR = ""
                                        End If
                                End Select

                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"), LX, LY, PZoom * MapObj(i).W, PZoom * MapObj(i).H * 2)
                                LX = CSng((-MapObj(i).W / 2 + (MapObj(i).W \ 2) / 2 + MapObj(i).X / 160) * PZoom)
                                LY = (H + (MapObj(i).H \ 2) / 2) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY

                            Case 62
                                '库巴
                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                Select Case LH.GameStyle
                                    Case 22323
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & "A.PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                    Case Else
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & ".PNG"),
                                    CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                End Select

                            Case 3
                                '德莱文
                                Select Case LH.GameStyle
                                    Case 22323
                                        If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                            LX = CSng((-MapObj(i).W / 2 + 0.5 + MapObj(i).X / 160) * PZoom)
                                            LY = H * PZoom - CSng((1 + MapObj(i).Y / 160) * PZoom) + KY
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\3B.PNG"),
                                            CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                            (H) * PZoom - CSng((1.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Else
                                            LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                            LY = H * PZoom - CSng((MapObj(i).H * 2 - 1.5 + MapObj(i).Y / 160) * PZoom) + KY
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\3.PNG"),
                                            CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                            (H + 2) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        End If
                                    Case Else
                                        If (MapObj(i).Flag \ &H4000) Mod 2 = 1 Then
                                            LX = CSng((-MapObj(i).W / 2 + 0.5 + MapObj(i).X / 160) * PZoom)
                                            LY = H * PZoom - CSng((1 + MapObj(i).Y / 160) * PZoom) + KY
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\3A.PNG"),
                                            CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                            H * PZoom - CSng((1.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        Else
                                            LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                            LY = H * PZoom - CSng((MapObj(i).H * 2 - 1.5 + MapObj(i).Y / 160) * PZoom) + KY
                                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\3.PNG"),
                                            CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                            H * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(i).W, 2 * PZoom * MapObj(i).H)
                                        End If
                                End Select

                            Case 13
                                '炮台
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "B"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\13" & PR & ".PNG"), 0.7), LX, LY, PZoom * MapObj(i).W, PZoom * 2)
                                For j = 2 To MapObj(i).H - 1
                                    If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\13C.PNG"), 0.7), LX, LY + j * PZoom, PZoom, PZoom)
                                    Else
                                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\13A.PNG"), 0.7), LX, LY + j * PZoom, PZoom, PZoom)
                                    End If
                                Next

                            Case 39
                                '魔法师
                                LX = CSng((2 - MapObj(i).W / 2 - MapObj(i).W + MapObj(i).X / 160) * PZoom)
                                LY = (H + 1) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\39.PNG"), LX - PZoom - PZoom, LY - PZoom + KY, 2 * PZoom * MapObj(i).W + KY, 2 * PZoom * MapObj(i).H)
                            Case 47
                                '小炮
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "E"
                                Else
                                    PR = ""
                                End If
                                Dim ANG As Single, D As String
                                Select Case MapObj(i).Flag \ &H100000
                                    Case &H0, &H4, &H8, &HC, &H10
                                        D = "D"
                                    Case &H2, &H24, &H28, &H2C, &H30
                                        D = "B"
                                    Case &H40, &H44, &H48, &H4C, &H50
                                        D = "C"
                                    Case &H60, &H64, &H68, &H6C, &H70
                                        D = "A"
                                    Case Else
                                        D = "A"
                                End Select

                                Select Case MapObj(i).Flag \ &H100000
                                    'UDLR
                                    Case &HC, &H30, &H64
                                        ANG = 0
                                    Case &H10, &H2C, &H44
                                        ANG = 180
                                    Case &H4, &H4C, &H70
                                        ANG = 270
                                    Case &H24, &H50, &H6C
                                        ANG = 90
                                    'UL UR DL DR
                                    Case &H8, &H60
                                        ANG = 315
                                    Case &H20, &H68
                                        ANG = 45
                                    Case &H0, &H48
                                        ANG = 225
                                    Case &H28, &H40
                                        ANG = 135
                                    Case Else
                                        ANG = 0
                                End Select
                                LX = CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom)
                                G.TranslateTransform(LX + MapObj(i).W * PZoom \ 2, LY + MapObj(i).H * PZoom \ 2)
                                G.RotateTransform(ANG)
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\47" & PR & ".PNG"),
                                    -MapObj(i).W * PZoom \ 2,
                                    -MapObj(i).H * PZoom \ 2, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                'G.RotateTransform(-ANG)
                                'G.TranslateTransform(-LX - MapObj(i).W * PZoom \ 2, -LY - MapObj(i).H * PZoom \ 2)
                                G.ResetTransform()
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\47" & PR & D & ".PNG"),
                                CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 61
                                '汪汪
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                If (MapObj(i).Flag \ &H4) Mod 2 = 0 Then
                                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\61A.PNG"),
                                            CSng((-0.5 + MapObj(i).X / 160) * PZoom),
                                            H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                            PZoom, PZoom)
                                End If
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\61.PNG"),
                                        CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                        H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 78
                                '仙人掌
                                LX = CSng(-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom
                                LY = (H + 1) * PZoom - CSng((MapObj(i).H + MapObj(i).Y \ 160) * PZoom) + KY
                                For j = 0 To MapObj(i).H - 1
                                    If j = MapObj(i).H - 1 Then
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\78.PNG"),
                                                CSng(-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom,
                                                (H - 1) * PZoom - CSng((j + MapObj(i).Y \ 160) * PZoom) + KY,
                                                PZoom, PZoom)
                                    Else
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\78A.PNG"),
                                                CSng(-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom,
                                                (H - 1) * PZoom - CSng((j + MapObj(i).Y \ 160) * PZoom) + KY,
                                                PZoom, PZoom)
                                    End If
                                Next
                            Case 111
                                '机械库巴
                                If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "B"
                                ElseIf (MapObj(i).Flag \ &H80000) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W + 0.5 + MapObj(i).X / 160) * PZoom)
                                LY = (H + 1) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\111" & PR & ".PNG"),
                                    CSng((-MapObj(i).W + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    2 * PZoom * MapObj(i).W, 2 * PZoom * MapObj(i).H)
                            Case 70
                                '大金币 
                                If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                ElseIf (MapObj(i).Flag \ &H80000) Mod 2 = 1 Then
                                    PR = "B"
                                Else
                                    PR = ""
                                End If
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                            CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom),
                                            H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                LX = CSng((-MapObj(i).W / 2 + 0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 1 + MapObj(i).Y / 160) * PZoom) + KY
                            Case 110
                                '刺方块
                                If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                ElseIf (MapObj(i).Flag \ &H80000) Mod 2 = 1 Then
                                    PR = "B"
                                Else
                                    PR = ""
                                End If
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(i).ID.ToString & PR & ".PNG"),
                                            CSng((-MapObj(i).W / 2 + 0.5 + MapObj(i).X / 160) * PZoom),
                                            H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                            Case 98
                                '小库巴
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(i).W + 0.5 + MapObj(i).X / 160) * PZoom)
                                LY = (H + 1) * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\98" & PR & ".PNG"),
                                    CSng((-MapObj(i).W + MapObj(i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(i).H * 2 - 0.5 + MapObj(i).Y / 160) * PZoom) + KY,
                                    2 * PZoom * MapObj(i).W, 2 * PZoom * MapObj(i).H)
                            Case 103
                                '骨鱼
                                LX = CSng((-MapObj(i).W + 0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\103.PNG"), CSng((-MapObj(i).W + MapObj(i).X / 160) * PZoom) + KY, LY, 2 * PZoom * MapObj(i).W, PZoom * MapObj(i).H)

                            Case 91
                                '跷跷板
                                For j = 0 To MapObj(i).W - 1
                                    If j = 0 Then
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\91A.PNG"),
                                                CSng((j - MapObj(i).W \ 2 + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(i).W - 1 Then
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\91B.PNG"),
                                                CSng((j - MapObj(i).W \ 2 + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\91.PNG"),
                                                CSng((j - MapObj(i).W \ 2 + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\91C.PNG"),
                                        CSng((-0.5 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom, PZoom)
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom)
                            Case 36
                                '熔岩台
                                If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                If MapObj(i).LID <> -1 Then
                                    MapObj(i).W = 1
                                End If
                                For j = 0 To MapObj(i).W - 1
                                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\36" & PR & ".PNG"),
                                    CSng((j - MapObj(i).W \ 2 + MapObj(i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom, PZoom)
                                Next
                                LX = CSng((j - 1 - MapObj(i).W \ 2 + MapObj(i).X \ 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY
                            Case 11
                                '升降台
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom) + KY

                                For j = 0 To MapObj(i).W - 1
                                    If (MapObj(i).Flag \ &H4) Mod 2 = 0 Then
                                        If j = 0 Then
                                            PR = "A"
                                        ElseIf j = MapObj(i).W - 1 Then
                                            PR = "B"
                                        Else
                                            PR = ""
                                        End If
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\11" & PR & ".PNG"),
                                        CSng((j - MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom, PZoom)
                                    Else
                                        If j = 0 Then
                                            PR = "D"
                                        ElseIf j = MapObj(i).W - 1 Then
                                            PR = "E"
                                        Else
                                            PR = "C"
                                        End If
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\11" & PR & ".PNG"),
                                        CSng((j - MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom, PZoom)
                                    End If
                                Next

                                If (MapObj(i).Flag \ &H4) Mod 2 = 0 Then
                                    Select Case MapObj(i).Flag Mod &H100
                                        Case &H40
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\D1.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H48
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\D2.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H50
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\D0.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H58
                                            G.DrawImage(Image.FromFile(P & "\img\CMN\D3.PNG"), LX, LY, PZoom, PZoom)
                                    End Select
                                End If

                            Case 54
                                '喷枪
                                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom) + KY
                                Select Case MapObj(i).Flag Mod &H100
                                    Case &H40
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A1.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), (H - 3) * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, 3 * PZOOM * MapObj(i).H)
                                    Case &H48
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A3.PNG"), CSng((-MapObj(i).W / 2 + 1 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, 3 * PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                                    Case &H50
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54B.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A5.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), (H + 1) * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, 3 * PZOOM * MapObj(i).H)
                                    Case &H58
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54C.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A7.PNG"), CSng((-MapObj(i).W / 2 - 3 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, 3 * PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                                    Case &H44
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A2.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), (H - 3) * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, 3 * PZOOM * MapObj(i).H)
                                    Case &H4C
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A4.PNG"), CSng((-MapObj(i).W / 2 + 1 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, 3 * PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                                    Case &H54
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54B.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A6.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), (H + 1) * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, 3 * PZOOM * MapObj(i).H)
                                    Case &H5C
                                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54C.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom) + KY, PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                                        'G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A8.PNG"), CSng((-MapObj(i).W / 2 - 3 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, 3 * PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                                End Select
                            Case 24
                                '火棍
                                LX = CSng(-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom
                                LY = H * PZoom - CSng(MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom
                                G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\24.PNG"),
                                LX, LY, PZoom, PZoom)

                            Case 105
                                '夹子
                                If MapObj(i).Flag Mod &H400 >= &H100 Then
                                    KY = PZoom * 3
                                    ObjLinkType(MapObj(i).LID + 1) = 105
                                Else
                                    KY = 0
                                    ObjLinkType(MapObj(i).LID + 1) = 105
                                End If
                                LX = CSng(-1.5 + MapObj(i).X / 160) * PZoom
                                LY = H * PZoom - CSng(3.5 + MapObj(i).Y / 160) * PZoom + KY

                                If (MapObj(i).Flag \ &H80) Mod 2 = 1 Then
                                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\105A.PNG"), LX, LY, PZoom * 3, PZoom * 5)
                                Else
                                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\105.PNG"), LX, LY, PZoom * 3, PZoom * 5)
                                End If
                        End Select


                        PR = ""
                        'PR += IIf((MapObj(i).Flag \ &H4000) Mod 2 = 1, "M", "")
                        PR += IIf((MapObj(i).Flag \ &H8000) Mod 2 = 1, "P", "")
                        PR += IIf((MapObj(i).Flag \ 2) Mod 2 = 1, "W", "")
                        If PR = "PW" Then PR = "B"
                        If PR.Length > 0 Then
                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & PR & ".PNG"), LX, LY, PZoom \ 2, PZoom \ 2)
                        End If

                        If L And ObjLinkType(MapObj(i).LID + 1) = 59 Then
                            PR = ((MapObj(i).Flag Mod &H400000) \ &H100000).ToString
                            G.DrawImage(Image.FromFile(P & "\img\CMN\D" & PR & ".PNG"), LX, LY, PZoom, PZoom)
                        End If

                    End If
                End If
            End If
        Next


    End Sub
    Public Sub DrawCID()
        Dim i As Integer
        Dim H As Integer, W As Integer, PR As String
        Dim LX, LY As Integer

        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        Dim P As String = PT

        For i = 0 To MapHdr.ObjCount - 1
            LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
            LY = H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom)
            Select Case MapObj(i).CID
                Case -1'无

                Case 44, 81, 116 '状态
                    If (MapObj(i).CFlag \ &H40000) Mod 2 = 1 Then
                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & "A.PNG"), LX, LY, PZoom, PZoom)
                    Else
                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & ".PNG"), LX, LY, PZoom, PZoom)
                    End If
                    G.DrawImage(Image.FromFile(P & "\img\CMN\F1.PNG"), LX, LY, PZoom, PZoom)
                Case 34 '状态火花
                    If (MapObj(i).CFlag \ &H4) Mod 2 = 1 Then
                        If (MapObj(i).CFlag \ &H40000) Mod 2 = 1 Then
                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & "C.PNG"), LX, LY, PZoom, PZoom)
                        Else
                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & "A.PNG"), LX, LY, PZoom, PZoom)
                        End If
                    Else
                        If (MapObj(i).CFlag \ &H40000) Mod 2 = 1 Then
                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & "B.PNG"), LX, LY, PZoom, PZoom)
                        Else
                            G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & ".PNG"), LX, LY, PZoom, PZoom)
                        End If
                    End If
                    G.DrawImage(Image.FromFile(P & "\img\CMN\F1.PNG"), LX, LY, PZoom, PZoom)
                Case 111 '机械库巴
                    If (MapObj(i).CFlag \ &H40000) Mod 2 = 1 Then
                        PR = "B"
                    ElseIf (MapObj(i).CFlag \ &H80000) Mod 2 = 1 Then
                        PR = "A"
                    Else
                        PR = ""
                    End If
                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & PR & ".PNG"), LX, LY, PZoom, PZoom)
                    G.DrawImage(Image.FromFile(P & "\img\CMN\F1.PNG"), LX, LY, PZoom, PZoom)
                Case 76 '加邦
                    If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                        If MapHdr.Theme = 6 Then
                            PR = "B"
                        Else
                            PR = "A"
                        End If
                    Else
                        PR = ""
                    End If
                    G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & PR & ".PNG"), LX, LY, PZoom, PZoom)
                    G.DrawImage(Image.FromFile(P & "\img\CMN\F1.PNG"), LX, LY, PZoom, PZoom)
                Case 33 '1UP
                    If MapHdr.Theme = 1 And MapHdr.Flag = 2 Then
                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & "A.PNG"), LX, LY, PZoom, PZoom)
                    Else
                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & ".PNG"), LX, LY, PZoom, PZoom)
                    End If
                    G.DrawImage(Image.FromFile(P & "\img\CMN\F1.PNG"), LX, LY, PZoom, PZoom)

                Case Else
                    If (MapObj(i).CFlag \ &H4) Mod 2 = 1 Then
                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & "A.PNG"), LX, LY, PZoom, PZoom)
                    Else
                        G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(i).CID.ToString & ".PNG"), LX, LY, PZoom, PZoom)
                    End If
                    G.DrawImage(Image.FromFile(P & "\img\CMN\F1.PNG"), LX, LY, PZoom, PZoom)
            End Select



        Next
    End Sub
    Public Sub DrawFireBar()
        Dim i, j, LX, LY As Integer
        Dim P As String = PT
        Dim H As Integer, W As Integer
        Dim FR As Single
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16
        ''火棍
        ''长度&H40 0000，角度EX/&H38E 38E0
        For i = 0 To MapHdr.ObjCount - 1
            If MapObj(i).ID = 24 Then
                LX = CSng(-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom
                LY = H * PZoom - CSng(MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom
                FR = MapObj(i).Ex / &H38E38E0
                G.TranslateTransform(LX + PZoom \ 2, LY + PZoom \ 2)
                G.RotateTransform(-FR * 5)
                For j = 0 To (MapObj(i).Flag - &H6000000) / &H400000 + 1
                    G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\24A.PNG"), 0.5), -PZoom \ 4 + j * PZoom, -PZoom \ 4, PZoom, PZoom \ 2)
                Next
                'G.RotateTransform(FR * 5)
                'G.TranslateTransform(-LX - PZoom \ 2, -LY - PZoom \ 2)
                G.ResetTransform()
                If (MapObj(i).Flag \ &H8) Mod 2 = 1 Then
                    G.DrawImage(Image.FromFile(P & "\img\CMN\B0.PNG"), LX, LY, PZoom, PZoom)
                Else
                    G.DrawImage(Image.FromFile(P & "\img\CMN\B1.PNG"), LX, LY, PZoom, PZoom)
                End If
            End If
        Next

    End Sub
    Public Sub DrawFire()
        Dim i, LX, LY As Integer
        Dim P As String = PT
        Dim H As Integer, W As Integer
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16

        For i = 0 To MapHdr.ObjCount - 1
            If MapObj(i).ID = 54 Then
                LX = CSng((-0.5 + MapObj(i).X / 160) * PZoom)
                LY = H * PZoom - CSng((0.5 + MapObj(i).Y / 160) * PZoom)
                Select Case MapObj(i).Flag Mod &H100
                    Case &H40
                        '	G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A1.PNG"), 0.5), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), (H - 3) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, 3 * PZoom * MapObj(i).H)
                    Case &H48
                        '	G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A3.PNG"), 0.5), CSng((-MapObj(i).W / 2 + 1 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), 3 * PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                    Case &H50
                        '	G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54B.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A5.PNG"), 0.5), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), (H + 1) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, 3 * PZoom * MapObj(i).H)
                    Case &H58
                        '	G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54C.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A7.PNG"), 0.5), CSng((-MapObj(i).W / 2 - 3 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), 3 * PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                    Case &H44
                        '	G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A2.PNG"), 0.5), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), (H - 3) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, 3 * PZoom * MapObj(i).H)
                    Case &H4C
                        '	G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A4.PNG"), 0.5), CSng((-MapObj(i).W / 2 + 1 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), 3 * PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                    Case &H54
                        '	G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54B.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A6.PNG"), 0.5), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZoom), (H + 1) * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), PZoom * MapObj(i).W, 3 * PZoom * MapObj(i).H)
                    Case &H5C
                        '	G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54C.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * PZOOM), H * PZOOM - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZOOM) + KY, PZOOM * MapObj(i).W, PZOOM * MapObj(i).H)
                        G.DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A8.PNG"), 0.5), CSng((-MapObj(i).W / 2 - 3 + MapObj(i).X / 160) * PZoom), H * PZoom - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * PZoom), 3 * PZoom * MapObj(i).W, PZoom * MapObj(i).H)
                End Select
            End If
        Next

    End Sub
    Public Sub DrawObject(IO As Boolean)

        '3D平台
        '半碰撞
        '蘑菇平台
        '桥 
        '蘑菇跳台 
        '开关跳台
        DrawItem("/132/", False)
        DrawItem("/16/", False)
        DrawItem("/14/", False)
        DrawItem("/17/", False)
        DrawItem("/113/", False)
        DrawItem("/71/", False)


        '箭头 单向板 中间旗 藤蔓 

        DrawItem("/66/67/106/", False)
        DrawItem("/64/", False)
        DrawItem("/90/", False)

        '树 长长吞食花
        DrawItem("/106/107/", False)

        '地面 传送带 开关 开关砖 P砖 冰锥
        '斜坡单独
        ReGrdCode()
        DrawGrd(IO)
        'DrawSlope()
        DrawGrdCode()


        DrawItem("/53/94/99/100/79/", False)
        DrawIce()



        '无LINKE
        '管道 门 蛇 传送箱
        DrawItem("/9/55/84/97/", False)
        '机动砖 轨道砖
        DrawItem("/85/119/", False)
        '夹子
        DrawItem("/105/", False)
        '轨道
        DrawTrack()
        '软砖 问号 硬砖 竹轮 云 音符 隐藏 刺 冰块 闪烁砖 
        DrawItem("/4/5/6/21/22/23/29/43/63/110/108/", False)

        '跷跷板 熔岩台 升降台 
        DrawItem("/91/36/11/", False)

        '狼牙棒
        DrawItem("/83/", False)

        '齿轮 甜甜圈
        DrawItem("/68/82/", False)

        '道具
        DrawItem("/0/1/2/3/8/10/12/13/15/18/19/20/25/28/30/31/32/33/34/35/39/", False)
        DrawItem("/40/41/42/44/45/46/47/48/52/56/57/58/60/61/62/70/74/76/77/78/81/92/95/98/102/103/104/", False)
        DrawItem("/111/120/121/122/123/124/125/126/112/127/128/129/130/131/72/50/51/65/80/114/116/", False)
        DrawItem("/96/117/86/", False)
        '喷枪 火棍
        DrawItem("/24/54/", False)
        'DrawFireBar(False)
        'DrawFire(False)
        '夹子
        DrawItem("/105/", False)
        '轨道
        DrawTrack()
        '夹子
        DrawItem("/105/", True)
        '卷轴相机
        'DrawItem("/89/", False)

        'LINK
        '软砖 问号 硬砖 竹轮 云 音符 隐藏 刺 冰块
        DrawItem("/4/5/6/21/22/23/29/43/63/", True)


        '跷跷板 熔岩台 升降台
        DrawItem("/91/36/11/", True)

        '齿轮 甜甜圈
        DrawItem("/68/82/", True)

        '道具
        DrawItem("/0/1/2/3/8/10/12/13/15/18/19/20/25/28/30/31/32/33/34/35/39/", True)
        DrawItem("/40/41/42/44/45/46/47/48/52/56/57/58/60/61/62/70/74/76/77/78/81/92/95/98/102/103/104/", True)
        DrawItem("/111/120/121/122/123/124/125/126/112/127/128/129/130/131/72/50/51/65/80/114/116/", True)
        DrawItem("/96/117/86/", True)

        DrawCID()

        '喷枪 火棍
        DrawItem("/24/54/", True)
        DrawFireBar()
        DrawFire()

        '透明管
        DrawCPipe()

    End Sub
    Public Sub DrawCPipe()
        Dim i As Integer, J As Integer, K As Integer
        Dim H As Integer, W As Integer, CP As String
        H = MapHdr.BorT \ 16
        W = MapHdr.BorR \ 16

        For i = 0 To MapHdr.ClearPipCount - 1
            For J = 0 To MapCPipe(i).NodeCount - 1
                Select Case J
                    Case 0
                        For K = 0 To MapCPipe(i).Node(J).H - 1
                            Select Case MapCPipe(i).Node(J).Dir
                                Case 0 'R
                                    If K = 0 Then
                                        CP = "C"
                                    ElseIf MapCPipe(i).NodeCount = 1 And K = MapCPipe(i).Node(J).H - 1 Then
                                        CP = "E"
                                    Else
                                        CP = "D"
                                    End If
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(i).Node(J).X + K) * PZoom,
                                                    (H - 1 - MapCPipe(i).Node(J).Y) * PZoom,
                                                    PZoom, 2 * PZoom)
                                Case 1 'L
                                    If K = 0 Then
                                        CP = "E"
                                    ElseIf MapCPipe(i).NodeCount = 1 And K = MapCPipe(i).Node(J).H - 1 Then
                                        CP = "C"
                                    Else
                                        CP = "D"
                                    End If
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(i).Node(J).X - K) * PZoom,
                                                    (H - 1 - MapCPipe(i).Node(J).Y - 1) * PZoom,
                                                     PZoom, 2 * PZoom)
                                Case 2 'U
                                    If K = 0 Then
                                        CP = ""
                                    ElseIf MapCPipe(i).NodeCount = 1 And K = MapCPipe(i).Node(J).H - 1 Then
                                        CP = "B"
                                    Else
                                        CP = "A"
                                    End If
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    MapCPipe(i).Node(J).X * PZoom,
                                                    (H - 1 - MapCPipe(i).Node(J).Y - K) * PZoom,
                                                    2 * PZoom, PZoom)
                                Case 3 'D
                                    If K = 0 Then
                                        CP = "B"
                                    ElseIf MapCPipe(i).NodeCount = 1 And K = MapCPipe(i).Node(J).H - 1 Then
                                        CP = ""
                                    Else
                                        CP = "A"
                                    End If
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(i).Node(J).X - 1) * PZoom,
                                                    (H - 1 - MapCPipe(i).Node(J).Y + K) * PZoom,
                                                    2 * PZoom, PZoom)
                            End Select
                        Next
                    Case MapCPipe(i).NodeCount - 1
                        For K = 0 To MapCPipe(i).Node(J).H - 1
                            Select Case MapCPipe(i).Node(J).Dir
                                Case 0 'R
                                    CP = IIf(K = MapCPipe(i).Node(J).H - 1, "E", "D")
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(i).Node(J).X + K) * PZoom,
                                                    (H - 1 - MapCPipe(i).Node(J).Y) * PZoom,
                                                    PZoom, 2 * PZoom)
                                Case 1 'L
                                    CP = IIf(K = MapCPipe(i).Node(J).H - 1, "C", "D")
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(i).Node(J).X - K) * PZoom,
                                                    (H - 1 - MapCPipe(i).Node(J).Y - 1) * PZoom,
                                                     PZoom, 2 * PZoom)
                                Case 2 'U
                                    CP = IIf(K = MapCPipe(i).Node(J).H - 1, "B", "A")
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    MapCPipe(i).Node(J).X * PZoom,
                                                    (H - 1 - MapCPipe(i).Node(J).Y - K) * PZoom,
                                                    2 * PZoom, PZoom)
                                Case 3 'D
                                    CP = IIf(K = MapCPipe(i).Node(J).H - 1, "", "A")
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(i).Node(J).X - 1) * PZoom,
                                                    (H - 1 - MapCPipe(i).Node(J).Y + K) * PZoom,
                                                    2 * PZoom, PZoom)
                            End Select
                        Next
                    Case Else
                        If MapCPipe(i).Node(J).type >= 3 And MapCPipe(i).Node(J).type <= 10 Then
                            Select Case MapCPipe(i).Node(J).type
                                Case 3, 7 'RU DL
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93G.PNG"),
                                                        (MapCPipe(i).Node(J).X) * PZoom,
                                                        (H - 2 - MapCPipe(i).Node(J).Y) * PZoom,
                                                        2 * PZoom, 2 * PZoom)
                                Case 4, 9 'RD UL
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93H.PNG"),
                                                        (MapCPipe(i).Node(J).X) * PZoom,
                                                        (H - 2 - MapCPipe(i).Node(J).Y) * PZoom,
                                                        2 * PZoom, 2 * PZoom)
                                Case 6, 10 'UR LD
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93J.PNG"),
                                                        (MapCPipe(i).Node(J).X) * PZoom,
                                                        (H - 2 - MapCPipe(i).Node(J).Y) * PZoom,
                                                        2 * PZoom, 2 * PZoom)
                                Case 5, 8 'DR LU
                                    G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93F.PNG"),
                                                        (MapCPipe(i).Node(J).X) * PZoom,
                                                        (H - 2 - MapCPipe(i).Node(J).Y) * PZoom,
                                                        2 * PZoom, 2 * PZoom)
                            End Select
                            'G.DrawString(MapCPipe(i).Node(J).type.ToString, Me.Font, Brushes.Black, (MapCPipe(i).Node(J).X) * PZOOM, (H - 2 - MapCPipe(i).Node(J).Y) * PZOOM)

                        Else
                            For K = 0 To MapCPipe(i).Node(J).H - 1
                                Select Case MapCPipe(i).Node(J).Dir
                                    Case 0 'R
                                        G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93D.PNG"),
                                                        (MapCPipe(i).Node(J).X + K) * PZoom,
                                                        (H - 1 - MapCPipe(i).Node(J).Y) * PZoom,
                                                        PZoom, 2 * PZoom)
                                    Case 1 'L
                                        G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93D.PNG"),
                                                        (MapCPipe(i).Node(J).X - K) * PZoom,
                                                        (H - 1 - MapCPipe(i).Node(J).Y - 1) * PZoom,
                                                         PZoom, 2 * PZoom)
                                    Case 2 'U
                                        G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93A.PNG"),
                                                        MapCPipe(i).Node(J).X * PZoom,
                                                        (H - 1 - MapCPipe(i).Node(J).Y - K) * PZoom,
                                                        2 * PZoom, PZoom)
                                    Case 3 'D
                                        G.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93A.PNG"),
                                                        (MapCPipe(i).Node(J).X - 1) * PZoom,
                                                        (H - 1 - MapCPipe(i).Node(J).Y + K) * PZoom,
                                                        2 * PZoom, PZoom)
                                End Select
                            Next
                        End If

                End Select
            Next
        Next
    End Sub
    Dim MapMove As Boolean = False, MMX As Integer, MMY As Integer
    Dim MapMove2 As Boolean = False, MMX2 As Integer, MMY2 As Integer
    Private Sub PicMap_MouseDown(sender As Object, e As MouseEventArgs)
        MMX = e.X
        MMY = e.Y
        MapMove = True
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        On Error Resume Next
        Form2.P.Image.Save(PT & "\" & TextBox9.Text & "-0.PNG", Imaging.ImageFormat.Png)
        Form3.P.Image.Save(PT & "\" & TextBox9.Text & "-1.PNG", Imaging.ImageFormat.Png)
        MsgBox("已保存地图至" & PT & "\" & TextBox9.Text)
    End Sub
    Private Sub RefPic()
        Form2.P.Left = 0
        Form2.P.Top = 0

        ListBox1.Items.Clear()
        ListBox2.Items.Clear()

        NowIO = 0
        LoadEFile(True)
        MapWidth(0) = MapHdr.BorR \ 16
        MapHeight(0) = MapHdr.BorT \ 16
        Dim DN As String = IIf(MapHdr.Flag = 2, "A", "")
        Tile = Image.FromFile(PT & "\img\TILE\" & LH.GameStyle & "-" & MapHdr.Theme.ToString & DN & ".png")
        TileW = Tile.Width \ 16
        InitPng()
        DrawObject(True)
        RefList(ListBox1, True)
        'FORM2.P.Image = B
        Form2.P.Image = BMAP1
        'ObjInfo()

        NowIO = 1
        Form3.P.Left = 0
        Form3.P.Top = 0
        LoadEFile(False)
        MapWidth(1) = MapHdr.BorR \ 16
        MapHeight(1) = MapHdr.BorT \ 16
        DN = IIf(MapHdr.Flag = 2, "A", "")
        Tile = Image.FromFile(PT & "\img\TILE\" & LH.GameStyle & "-" & MapHdr.Theme.ToString & DN & ".png")
        TileW = Tile.Width \ 16
        InitPng2()
        DrawObject(False)
        RefList(ListBox2, False)
        Form3.P.Image = BMAP2
        'ObjInfo()
        'GetLvlInfo()
    End Sub
    Private Declare Function DoFileDownload Lib "shdocvw.dll" (ByVal lpszFile As String) As Integer
    Private Declare Function URLDownloadToFile Lib "urlmon" Alias "URLDownloadToFileA" _
(ByVal pCaller As Integer, ByVal szURL As String, ByVal szFileName As String, ByVal dwReserved As Integer, ByVal lpfnCB As Integer) As Integer
    Dim isMapIO As Boolean = True
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim I As Integer
        Dim F As FileInfo
        TextBox9.Text = TextBox9.Text.Replace("-", "")
        TextBox9.Text = TextBox9.Text.Replace(" ", "")
        TextBox9.Text = UCase(Strings.Left(TextBox9.Text, 9))

        Label6.Text = DateTime.Now.ToString & " 校验图号"
        '检查图号
        Dim LInfo As String = ""
        I = Code2Num(TextBox9.Text, LInfo)
        Select Case I
            Case 0 '关卡ID
                Label6.Text += vbCrLf & LInfo
            Case 1 '工匠ID
                Label6.Text += vbCrLf & DateTime.Now.ToString & " 此ID为工匠ID"
                Label6.Text += vbCrLf & LInfo
                Exit Sub
            Case Else '错误ID
                Label6.Text += vbCrLf & DateTime.Now.ToString & " 校验失败，图号输入有误"
                Label6.Text += vbCrLf & LInfo
                Exit Sub
        End Select

        '====================
        If IO.File.Exists(PT & "\decrypt_data\" & TextBox9.Text & ".DEC") Then
            F = New FileInfo(PT & "\decrypt_data\" & TextBox9.Text & ".DEC")
            If F.Length = 376768 Then
                I = 2
            Else
                I = 0
            End If
        ElseIf IO.File.Exists(PT & "\download_data\" & TextBox9.Text & ".BCD") Then
            F = New FileInfo(PT & "\download_data\" & TextBox9.Text & ".BCD")
            If F.Length = 376832 Then
                I = 1
            Else
                I = 0
            End If
        Else
            I = 0
        End If

        Select Case I
            Case 0 '文件不存在，下载新地图
                Label6.Text += vbCrLf & DateTime.Now.ToString & " 从服务器下载地图"
                Label6.Text += vbCrLf & DateTime.Now.ToString & " /mm2/level_data/" & TextBox9.Text
                Application.DoEvents()
                '下载文件
                Try
                    Dim wc = New WebClient
                    wc.DownloadFile("https://" & MaterialComboBox1.Text & "/mm2/level_data/" & TextBox9.Text, PT & "\download_data\" & TextBox9.Text & ".BCD")
                    Do
                        Application.DoEvents()
                        If IO.File.Exists(PT & "\download_data\" & TextBox9.Text & ".BCD") Then
                            F = New FileInfo(PT & "\download_data\" & TextBox9.Text & ".BCD")
                            If F.Length = 376832 Then
                                DeMap2(TextBox9.Text & ".BCD", TextBox9.Text & ".DEC")
                                Label6.Text += vbCrLf & DateTime.Now.ToString & " 解析地图"
                                Label6.Text += vbCrLf & DateTime.Now.ToString & " 已加载地图"
                                TextBox1.Text = TextBox9.Text & ".DEC"
                                isMapIO = True
                                RefPic()
                                Exit Sub
                            End If
                        End If
                        I += 1
                        Threading.Thread.Sleep(1000)
                    Loop Until I > 30
                    Label6.Text += vbCrLf & DateTime.Now.ToString & " 下载地图超时"
                Catch ex As Exception
                    Label6.Text += vbCrLf & DateTime.Now.ToString & " " & ex.Message
                End Try

            Case 1 '存在BCD地图文件
                Label6.Text += vbCrLf & DateTime.Now.ToString & " 解析地图"
                DeMap2(TextBox9.Text & ".BCD", TextBox9.Text & ".DEC")
                Label6.Text += vbCrLf & DateTime.Now.ToString & " 已从BCD文件加载地图"
                TextBox1.Text = TextBox9.Text & ".DEC"
                isMapIO = True
                RefPic()
            Case 2 '存在DEC文档，直接读取
                Label6.Text += vbCrLf & DateTime.Now.ToString & " 已从DEC文件加载地图"
                TextBox1.Text = TextBox9.Text & ".DEC"
                isMapIO = True
                RefPic()
        End Select

    End Sub
    Sub DeMap2(InF As String, OutF As String) '解密BCD地图文件
        Dim k() = File.ReadAllBytes(PT & "\download_data\" & InF)
        Dim r() = Decrypt(k)
        File.WriteAllBytes(PT & "\decrypt_data\" & OutF, r)
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs)
        Form2.P.Image = GetTile(0, 0, 1, 1)
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form3.Show()
        Form3.ClientSize = New Size(500, 500)
        Form3.P.Size = BSIZE2
        Form3.P.Image = BMAP2
    End Sub
    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        PZOOM = 2 ^ TrackBar1.Value
        TxtZoom.Text = "×" & PZoom.ToString
    End Sub

    Private Function SetObjInfo(idx As Integer, flag As Integer) As ObjStr
        Dim PR, PB As String
        With SetObjInfo
            .Obj = ""
            .Flag = ""
            .State = ""
            .SubObj = ""
            .SubFlag = ""
            .SubState = ""
        End With
        Select Case idx
            Case -1 'NOTHING啥也妹有
            Case 89'卷轴相机
            Case 9 '管道
            Case 93'透明管
            Case 14, 16, 71 '平台
                SetObjInfo.Obj = idx.ToString
            Case 17 '桥
                SetObjInfo.Obj = idx.ToString
            Case 87, 88'斜坡
            Case 53, 94'传送带
            Case 105'夹子
            Case 55, 97 '门 传送箱
                SetObjInfo.Obj = idx.ToString
                SetObjInfo.Flag = Strings.Mid("ABCDEFGHJ", ((flag Mod &H800000) \ &H200000) + 1, 1)
            Case 34 '火花 
                If (flag \ &H4) Mod 2 = 1 Then
                    If (flag \ &H40000) Mod 2 = 1 Then
                        PB = "C"
                    Else
                        PB = "A"
                    End If
                Else
                    If (flag \ &H40000) Mod 2 = 1 Then
                        PB = "B"
                    Else
                        PB = ""
                    End If
                End If
                PR = ""
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
            Case 44, 81, 116 '弹力球 大蘑菇 锤子
                If (flag \ &H40000) Mod 2 = 1 Then
                    PB = "A"
                Else
                    PB = ""
                End If
                PR = ""
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
            Case 74 '加邦
                If (flag \ &H4) Mod 2 = 1 Then
                    If MapHdr.Theme = 6 Then
                        PB = "B"
                    Else
                        PB = "A"
                    End If
                Else
                    PB = ""
                End If
                PR = ""
                PR += IIf((flag \ &H4000) Mod 2 = 1, "M", "")
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
            Case 78 '仙人掌
                If MapHdr.Theme = 6 Then
                    PB = "A"
                Else
                    PB = ""
                End If
                PR = ""
                PR += IIf((flag \ &H4000) Mod 2 = 1, "M", "")
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
            Case 3 '德莱文
                PB = IIf((flag \ &H4000) Mod 2 = 1, "A", "")
                PR = ""
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
            Case 70, 111, 110 '大金币 机械酷巴 尖刺砖块
                If (flag \ &H40000) Mod 2 = 1 Then
                    PB = "A"
                ElseIf (flag \ &H80000) Mod 2 = 1 Then
                    PB = "B"
                Else
                    PB = ""
                End If

                PR = ""
                PR += IIf((flag \ &H4000) Mod 2 = 1, "M", "")
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
            Case 33 '1UP
                If MapHdr.Theme = 0 And MapHdr.Flag = 2 Then
                    PB = "A"
                Else
                    PB = ""
                End If
                PR = ""
                PR += IIf((flag \ &H4000) Mod 2 = 1, "M", "")
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
            Case 45 '鞋 耀西
                Select Case LH.GameStyle
                    Case 21847, 22349 'U W
                        If (flag \ &H4000) Mod 2 = 1 Then
                            PB = "A"
                        Else
                            PB = ""
                        End If
                        PR = ""
                    Case Else
                        If (flag \ &H4) Mod 2 = 1 Then
                            PB = "A"
                        Else
                            PB = ""
                        End If
                        PR = ""
                        PR += IIf((flag \ &H4000) Mod 2 = 1, "M", "")
                End Select
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
            Case Else
                If (flag \ &H4) Mod 2 = 1 Then
                    PB = "A"
                Else
                    PB = ""
                End If
                PR = ""
                PR += IIf((flag \ &H4000) Mod 2 = 1, "M", "")
                PR += IIf((flag \ &H8000) Mod 2 = 1, "P", "")
                PR += IIf((flag \ 2) Mod 2 = 1, "W", "")
                SetObjInfo.Obj = idx.ToString & PB
                SetObjInfo.Flag = PR
        End Select
    End Function

    Private Sub RefList(L As ListBox, IO As Boolean)
        Dim i As Integer, P As Integer
        Dim PB As String
        Dim J1, J2 As ObjStr
        Dim Offset, ADDR As Integer
        Offset = IIf(IO, &H201, &H2E0E1)
        For i = 0 To MapHdr.ObjCount - 1
            If MapObj(i).CID = -1 Then
                'Offset +&H48 + &H0 + M * &H20
                ADDR = Offset + &H48 + &H0 + i * &H20
                L.Items.Add(Hex(ADDR) & " " & GetItemName(MapObj(i).ID, LH.GameStyle).ToString & " " & (0.5 + MapObj(i).X / 160).ToString & "," & (0.5 + MapObj(i).Y / 160).ToString)
            Else
                'Offset +&H48 + &H0 + M * &H20
                ADDR = Offset + &H48 + &H0 + i * &H20
                L.Items.Add(Hex(ADDR) & " " & GetItemName(MapObj(i).ID, LH.GameStyle).ToString & "(" & GetItemName(MapObj(i).CID, LH.GameStyle) & ") " & (0.5 + MapObj(i).X / 160).ToString & "," & (0.5 + MapObj(i).Y / 160).ToString)
            End If

            Select Case MapObj(i).ID
                Case 9 '管道PIPE
                Case 93'透明管道CLEAR PIPE
                Case 14, 16, 71 '平台
                    For W = 1 To MapObj(i).W
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H + MapObj(i).Y / 160)).Obj += MapObj(i).ID.ToString & ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H + MapObj(i).Y / 160)).Flag += ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H + MapObj(i).Y / 160)).SubObj += ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H + MapObj(i).Y / 160)).SubFlag += ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H + MapObj(i).Y / 160)).State += ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H + MapObj(i).Y / 160)).SubState += ","
                    Next
                Case 17 '桥
                    For W = 1 To MapObj(i).W
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H - 1 + MapObj(i).Y / 160)).Obj += MapObj(i).ID.ToString & ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H - 1 + MapObj(i).Y / 160)).Flag += ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H - 1 + MapObj(i).Y / 160)).SubObj += ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H - 1 + MapObj(i).Y / 160)).SubFlag += ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H - 1 + MapObj(i).Y / 160)).State += ","
                        ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(MapObj(i).H - 1 + MapObj(i).Y / 160)).SubState += ","
                    Next
                Case 87, 88 '斜坡 SLOPE
                Case 53, 94 '传送带
                Case 105 '夹子
                Case 97 '传送箱
                    P = ((MapObj(i).Flag Mod &H800000) \ &H200000)
                    If (MapObj(i).Flag \ &H4) Mod 2 = 1 Then
                        PB = "A"
                    Else
                        PB = ""
                    End If
                    For W = 1 To MapObj(i).W
                        For H = 1 To MapObj(i).H
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).Obj += MapObj(i).ID.ToString & PB & ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).Flag += Strings.Mid("ABCDEFGHJ", P + 1, 1) & ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubObj += ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubFlag += ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).State += ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubState += ","
                        Next
                    Next
                Case 55 '门 
                    P = ((MapObj(i).Flag Mod &H800000) \ &H200000)
                    If (MapObj(i).Flag \ &H40000) Mod 2 = 1 Then
                        PB = "A"
                    ElseIf (MapObj(i).Flag \ &H80000) Mod 2 = 1 Then
                        PB = "B"
                    Else
                        PB = ""
                    End If
                    For W = 1 To MapObj(i).W
                        For H = 1 To MapObj(i).H
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).Obj += MapObj(i).ID.ToString & PB & ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).Flag += Strings.Mid("ABCDEFGHJ", P + 1, 1) & ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubObj += ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubFlag += ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).State += ","
                            ObjLocData(NowIO, Int(W - 0.5 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubState += ","
                        Next
                    Next
                Case 34, 44, 81 '花 弹力球 USA 大蘑菇
                    J1 = SetObjInfo(MapObj(i).ID, MapObj(i).Flag)
                    For W = 1 To MapObj(i).W
                        For H = 1 To MapObj(i).H
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).Obj += J1.Obj & ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).Flag += J1.Flag & ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).State += J1.State & ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubObj += ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubFlag += ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubState += ","
                        Next
                    Next
                Case Else
                    J1 = SetObjInfo(MapObj(i).ID, MapObj(i).Flag)
                    J2 = SetObjInfo(MapObj(i).CID, MapObj(i).CFlag)
                    For W = 1 To MapObj(i).W
                        For H = 1 To MapObj(i).H
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).Obj += J1.Obj & ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).Flag += J1.Flag & ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).State += J1.State & ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubObj += J2.Obj & ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubFlag += J2.Flag & ","
                            ObjLocData(NowIO, Int(W - MapObj(i).W / 2 + MapObj(i).X / 160), Int(H + MapObj(i).Y / 160)).SubState += J2.State & ","
                        Next
                    Next
            End Select
        Next
    End Sub
    Dim OCRRect As New Rectangle(0, 0, 1, 1)
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Timer1.Enabled = Not Timer1.Enabled
        Button3.Text = If(Timer1.Enabled, "ON", "OFF")
    End Sub
    Public Sub ObjInfo()

        Dim i As Integer, M As Integer
        Dim s As String = "==OBJ==" & vbCrLf
        'OBJ参数详情
        s += "ID" & vbTab & "ID" & vbTab & "X" & vbTab & "Y" & vbTab & "FLAG" & vbTab & "CID" & vbTab & "CFLAG" & vbTab & "HEX" & vbTab &
            "LID" & vbTab & "SID" & vbTab & "W" & vbTab & "H" & vbCrLf

        For i = 0 To MapHdr.ObjCount - 1
            s += ObjEng(MapObj(i).ID) & vbTab & MapObj(i).ID & vbTab & MapObj(i).X & vbTab & MapObj(i).Y & vbTab
            s += Hex(MapObj(i).Flag) & vbTab & MapObj(i).CID & vbTab & Hex(MapObj(i).CFlag) & vbTab & Hex(MapObj(i).Ex) & vbTab
            s += MapObj(i).LID & vbTab & MapObj(i).SID & vbTab & MapObj(i).W & vbTab & MapObj(i).H & vbCrLf

        Next

        s += "==TRACK==" & vbCrLf
        s += "UN" & vbTab & "X" & vbTab & "Y" & vbTab & "HFLAG" & vbTab & "TYPE" & vbTab & "LID" & vbTab & "HK0,HK1" & vbCrLf
        For i = 0 To MapHdr.TrackCount - 1
            s += MapTrk(i).UN & vbTab & MapTrk(i).X & vbTab & MapTrk(i).Y & vbTab & Hex(MapTrk(i).Flag) & vbTab &
            MapTrk(i).Type & vbTab & MapTrk(i).LID & vbTab & Hex(MapTrk(i).K0) & vbTab & Hex(MapTrk(i).K1) & vbCrLf
        Next


        s += "==CPIPE==" & vbCrLf
        For M = 0 To MapHdr.ClearPipCount - 1
            s += "INDEX" & vbTab & "NC" & vbTab & "N" & vbTab & "HFLAG" & vbTab & "TYPE" & vbTab & "HLID" & vbTab & "HK0,HK1" & vbCrLf
            s += MapCPipe(M).Index & vbTab & MapCPipe(M).NodeCount & vbCrLf
            s += "TYPE" & vbTab & "INDEX" & vbTab & "X" & vbTab & "Y" & vbTab & "W" & vbTab & "H" & vbTab & "DIR" & vbCrLf
            For i = 0 To MapCPipe(M).NodeCount - 1
                s += MapCPipe(M).Node(i).type & vbTab & MapCPipe(M).Node(i).index & vbTab & MapCPipe(M).Node(i).X & vbTab & MapCPipe(M).Node(i).Y & vbTab &
                MapCPipe(M).Node(i).W & vbTab & MapCPipe(M).Node(i).H & vbTab & MapCPipe(M).Node(i).Dir & vbCrLf
            Next
        Next
        TextBox4.Text += s


    End Sub
    Private Sub PicMap2_MouseUp(sender As Object, e As MouseEventArgs)
        MapMove2 = False
    End Sub

    Public Function GetPage(ByVal url As String) As String
        On Error GoTo Err
        Dim hRqst As HttpWebRequest = HttpWebRequest.Create(url)

        hRqst.ContentType = "application/x-www-form-urlencoded"
        hRqst.Method = "GET"
        Dim streamData As Stream

        Dim hRsp As HttpWebResponse = hRqst.GetResponse()
        streamData = hRsp.GetResponseStream()

        Dim readStream As New IO.StreamReader(streamData, System.Text.Encoding.UTF8)
        GetPage = readStream.ReadToEnd()
        streamData.Close()
        Exit Function
Err:
        GetPage = ""
    End Function

    Private Function ConInfo(a As String) As JObject
        ConInfo = JObject.Parse(a)
    End Function
    Dim G2 As Graphics, B2 As New Bitmap(1, 1)
    Public Function GetImageFromByteArray(ByVal bytes As Byte()) As Bitmap
        Return CType(Bitmap.FromStream(New IO.MemoryStream(bytes)), Bitmap)
    End Function
    Public Function GetByteArrayFromImage(ByVal img As Bitmap) As Byte()
        Dim ms As New System.IO.MemoryStream
        img.Save(ms, Imaging.ImageFormat.Bmp)
        Dim outBytes(CInt(ms.Length - 1)) As Byte
        ms.Seek(0, System.IO.SeekOrigin.Begin)
        ms.Read(outBytes, 0, CInt(ms.Length))
        Return outBytes
    End Function

    Dim G0 As Graphics, B0 As New Bitmap（300, 30）
    Dim UR2 As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PT = Application.StartupPath
        Dim I, J, K As Integer
        UR2 = ""
        For I = 1 To UR.Length Step 6
            J = Int(Mid(UR, I, 3))
            K = Int(Mid(UR, I + 3, 3))
            UR2 += Chr(J Xor K)
        Next

        G2 = Graphics.FromImage(B2)
        G0 = Graphics.FromImage(B0)
        Me.AllowDrop = True
        Form2.P.AllowDrop = True
        Form3.P.AllowDrop = True
        SetTileLoc()
        SetGrdLoc()

        MiiB(0) = New Bitmap(128, 128)
        MiiB(1) = New Bitmap(128, 128)
        MiiB(2) = New Bitmap(128, 128)
        MiiG(0) = Graphics.FromImage(MiiB(0))
        MiiG(1) = Graphics.FromImage(MiiB(1))
        MiiG(2) = Graphics.FromImage(MiiB(2))

        TrackYPt(8, 0) = New Point(4, 0)
        TrackYPt(8, 1) = New Point(0, 2)
        TrackYPt(8, 2) = New Point(4, 4)
        TrackYPt(12, 0) = New Point(4, 0)
        TrackYPt(12, 1) = New Point(0, 2)
        TrackYPt(12, 2) = New Point(4, 4)

        TrackYPt(9, 0) = New Point(0, 0)
        TrackYPt(9, 1) = New Point(0, 4)
        TrackYPt(9, 2) = New Point(4, 2)
        TrackYPt(13, 0) = New Point(0, 0)
        TrackYPt(13, 1) = New Point(0, 4)
        TrackYPt(13, 2) = New Point(4, 2)

        TrackYPt(10, 0) = New Point(0, 0)
        TrackYPt(10, 1) = New Point(4, 0)
        TrackYPt(10, 2) = New Point(2, 4)
        TrackYPt(14, 0) = New Point(0, 0)
        TrackYPt(14, 1) = New Point(4, 0)
        TrackYPt(14, 2) = New Point(2, 4)

        TrackYPt(11, 0) = New Point(2, 0)
        TrackYPt(11, 1) = New Point(0, 4)
        TrackYPt(11, 2) = New Point(4, 4)
        TrackYPt(15, 0) = New Point(2, 0)
        TrackYPt(15, 1) = New Point(0, 4)
        TrackYPt(15, 2) = New Point(4, 4)

    End Sub
    Public Sub SetTileLoc()
        '4,4A 5 6 8 8A 21 22 23 23A 29 43 49 63 79 79A 92 99 100 100A
        TileLoc(4, 0) = New Point(1, 0)
        TileLoc(4, 1) = New Point(2, 43)
        TileLoc(5, 0) = New Point(2, 0)
        TileLoc(6, 0) = New Point(6, 0)
        TileLoc(8, 0) = New Point(7, 0)
        TileLoc(8, 1) = New Point(0, 17)
        TileLoc(21, 0) = New Point(0, 4)
        TileLoc(22, 0) = New Point(6, 6)
        TileLoc(23, 0) = New Point(4, 0)
        TileLoc(23, 1) = New Point(6, 5)
        TileLoc(29, 0) = New Point(3, 0)
        TileLoc(43, 0) = New Point(2, 4)
        TileLoc(49, 0) = New Point(15, 15)
        TileLoc(63, 0) = New Point(8, 7)
        TileLoc(79, 0) = New Point(1, 43)
        TileLoc(79, 1) = New Point(0, 43)
        TileLoc(92, 0) = New Point(0, 16)
        TileLoc(99, 0) = New Point(2, 23)
        TileLoc(100, 0) = New Point(3, 22)
        TileLoc(100, 1) = New Point(2, 21)

        'pipe loc
        'UDLRVH
        'GRBO
        PipeLoc(0, 0) = New Point(14, 0)
        PipeLoc(0, 1) = New Point(14, 2)
        PipeLoc(0, 2) = New Point(11, 0)
        PipeLoc(0, 3) = New Point(13, 0)
        PipeLoc(0, 4) = New Point(12, 0)
        PipeLoc(0, 5) = New Point(14, 1)

        PipeLoc(1, 0) = New Point(6, 37)
        PipeLoc(1, 1) = New Point(12, 37)
        PipeLoc(1, 2) = New Point(4, 24)
        PipeLoc(1, 3) = New Point(6, 24)
        PipeLoc(1, 4) = New Point(5, 24)
        PipeLoc(1, 5) = New Point(6, 38)

        PipeLoc(2, 0) = New Point(10, 37)
        PipeLoc(2, 1) = New Point(12, 38)
        PipeLoc(2, 2) = New Point(3, 37)
        PipeLoc(2, 3) = New Point(5, 37)
        PipeLoc(2, 4) = New Point(4, 37)
        PipeLoc(2, 5) = New Point(10, 38)

        PipeLoc(3, 0) = New Point(8, 37)
        PipeLoc(3, 1) = New Point(14, 37)
        PipeLoc(3, 2) = New Point(0, 37)
        PipeLoc(3, 3) = New Point(2, 37)
        PipeLoc(3, 4) = New Point(1, 37)
        PipeLoc(3, 5) = New Point(8, 38)
    End Sub
    Private Function GetColor(x As Int16, y As Int16) As Color
        G2.CopyFromScreen(New Point(x, y), New Point(0, 0), New Size(1, 1))
        GetColor = B2.GetPixel(0, 0)
    End Function
    Private Function OCR(P() As Byte) As String
        'OCR识别
        Try
            Dim engine As TesseractEngine = New TesseractEngine("./tessdata", "eng", EngineMode.Default)
            Dim img As Pix = Pix.LoadFromMemory(P)
            Dim page As Page = engine.Process(img)
            Dim text As String = page.GetText()
            'Label1.Text = "置信度:" & page.GetMeanConfidence() & vbCrLf
            OCR = Replace(text.ToUpper, "O", "0")
            OCR = Replace(OCR, "-", "")
            OCR = Replace(OCR, "_", "")
            OCR = Replace(OCR, "+", "")
            OCR = Replace(OCR, " ", "")
            OCR = Replace(OCR, "*", "")
            OCR = Replace(OCR, "/", "")
            OCR = Replace(OCR, "\", "")
            OCR = Strings.Left(OCR, 9)
        Catch R As Exception
            OCR = ""
        End Try
    End Function

    Private Sub PicMap_DragEnter(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
    Private Sub PicMap_DragDrop(sender As Object, e As DragEventArgs)
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In files
            TextBox1.Text = path
        Next
        RefPic()
    End Sub
    Private Sub PicMap2_DragEnter(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
    Private Sub PicMap2_DragDrop(sender As Object, e As DragEventArgs)
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In files
            TextBox1.Text = path
        Next
        RefPic()
    End Sub
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        If File.Exists(files(0)) Then
            Dim F = New FileInfo(files(0))
            If UCase(F.Extension) = ".BCD" Then
                Label6.Text = F.FullName
                Label6.Text += vbCrLf & PT & "\download_data\" & F.Name
                File.Copy(F.FullName, PT & "\download_data\" & F.Name)
                DeMap2(F.Name, UCase(F.Name).Replace(".BCD", ".DEC"))
                TextBox1.Text = UCase(F.Name).Replace(".BCD", ".DEC")
                RefPic()
            ElseIf UCase(F.Extension) = ".DEC" Then
                Label6.Text = F.FullName
                Label6.Text += vbCrLf & PT & "\decrypt_data\" & F.Name
                File.Copy(F.FullName, PT & "\decrypt_data\" & F.Name)
                TextBox1.Text = F.Name
                RefPic()
            End If
        Else
            Exit Sub
        End If


    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Private Function GetD(P As String) As String
        Dim S() As String = P.Split("|")
        GetD = S(0).Replace(Chr(10), "")
    End Function


    Function GetToken(JSON As JObject, a As String) As String

        GetToken = vbCrLf
        GetToken += "昵称:" & JSON.SelectToken(a).SelectToken("name").ToString & vbCrLf
        GetToken += "ID:" & JSON.SelectToken(a).SelectToken("code").ToString & vbCrLf
        GetToken += "===关卡游玩===" & vbCrLf
        GetToken += "游玩数:" & JSON.SelectToken(a).SelectToken("courses_played").ToString & vbCrLf
        GetToken += "通过数:" & JSON.SelectToken(a).SelectToken("courses_cleared").ToString & vbCrLf
        GetToken += "尝试数:" & JSON.SelectToken(a).SelectToken("courses_attempted").ToString & vbCrLf
        GetToken += "死亡数:" & JSON.SelectToken(a).SelectToken("courses_deaths").ToString & vbCrLf
        GetToken += "首插数:" & JSON.SelectToken(a).SelectToken("first_clears").ToString & vbCrLf
        GetToken += "纪录数:" & JSON.SelectToken(a).SelectToken("world_records").ToString & vbCrLf
        GetToken += "===工匠点数===" & vbCrLf
        GetToken += "工匠点数:" & JSON.SelectToken(a).SelectToken("maker_points").ToString & vbCrLf
        GetToken += "赞:" & JSON.SelectToken(a).SelectToken("likes").ToString & vbCrLf
        GetToken += "上传关卡:" & JSON.SelectToken(a).SelectToken("uploaded_levels").ToString & vbCrLf
        GetToken += "===耐力挑战===" & vbCrLf
        GetToken += "简单:" & JSON.SelectToken(a).SelectToken("easy_highscore").ToString & vbCrLf
        GetToken += "普通:" & JSON.SelectToken(a).SelectToken("normal_highscore").ToString & vbCrLf
        GetToken += "困难:" & JSON.SelectToken(a).SelectToken("expert_highscore").ToString & vbCrLf
        GetToken += "极难:" & JSON.SelectToken(a).SelectToken("super_expert_highscore").ToString & vbCrLf
        GetToken += "===多人对战===" & vbCrLf
        GetToken += "对战等级:" & JSON.SelectToken(a).SelectToken("versus_rank_name").ToString & "(" & JSON.SelectToken(a).SelectToken("versus_rating").ToString & ")" & vbCrLf
        GetToken += "对战游玩:" & JSON.SelectToken(a).SelectToken("versus_plays").ToString & vbCrLf
        GetToken += "对战胜场:" & JSON.SelectToken(a).SelectToken("versus_won").ToString & vbCrLf
        GetToken += "对战负场:" & JSON.SelectToken(a).SelectToken("versus_lost").ToString & vbCrLf
        GetToken += "近期表现:" & JSON.SelectToken(a).SelectToken("recent_performance").ToString & vbCrLf
        GetToken += "当前连胜:" & JSON.SelectToken(a).SelectToken("versus_win_streak").ToString & vbCrLf
        GetToken += "当前连负:" & JSON.SelectToken(a).SelectToken("versus_lose_streak").ToString & vbCrLf
        GetToken += "对战击杀:" & JSON.SelectToken(a).SelectToken("versus_kills").ToString & vbCrLf
        GetToken += "对战被杀:" & JSON.SelectToken(a).SelectToken("versus_killed_by_others").ToString & vbCrLf
        GetToken += "对战断连:" & JSON.SelectToken(a).SelectToken("versus_disconnected").ToString & vbCrLf
        GetToken += "===多人合作===" & vbCrLf
        GetToken += "合作通关:" & JSON.SelectToken(a).SelectToken("coop_plays").ToString & vbCrLf
        GetToken += "合作游玩:" & JSON.SelectToken(a).SelectToken("coop_clears").ToString & vbCrLf
        GetToken += "===拥有奖牌===" & vbCrLf
        If JSON.SelectToken(a).SelectToken("badges").Count > 0 Then

            Dim K As JObject
            For Each K In JSON.SelectToken(a).SelectToken("badges")
                GetToken += BadgesType(Val(K.SelectToken("type").ToString)) & ":" & Badges(Val(K.SelectToken("rank").ToString)) & vbCrLf
                'GetToken += K.SelectToken("type").ToString & ":" & K.SelectToken("rank").ToString & vbCrLf
            Next
        End If
        GetToken += "=====服装=====" & vbCrLf
        GetToken += "帽子:" & JSON.SelectToken(a).SelectToken("hat").ToString & vbCrLf
        GetToken += "衣服:" & JSON.SelectToken(a).SelectToken("shirt").ToString & vbCrLf
        GetToken += "裤子:" & JSON.SelectToken(a).SelectToken("pants").ToString & vbCrLf
    End Function
    Sub LoadLvlInfo()
        'On Error Resume Next
        Dim S As String
        Dim JSON As JObject
        Dim B As New Bitmap(260, 760)
        Dim G As Graphics = Graphics.FromImage(B)
        Timer2.Enabled = False
        Label2.Text = ""
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        TextBox9.Text = TextBox9.Text.Replace("-", "")
        If TextBox9.Text.Length = 9 Then
            S = GetPage("http://tgrcode.com/mm2/level_info/" & TextBox9.Text)
            TextBox2.Text = S
            If S = "" Or InStr(S, "Code corresponds to a maker") > 0 Then Me.Text = "ID错误，请输入关卡ID" : Exit Sub
            JSON = JObject.Parse(S)
            Label2.Text += "标题:" & JSON.SelectToken("name").ToString & vbCrLf
            Label2.Text += "描述:" & JSON.SelectToken("description").ToString & vbCrLf
            Label2.Text += "上传日期:" & JSON.SelectToken("uploaded_pretty").ToString & vbCrLf
            Label2.Text += "风格:" & JSON.SelectToken("game_style_name").ToString & vbCrLf
            Label2.Text += "主题:" & JSON.SelectToken("theme_name").ToString & vbCrLf
            Label2.Text += "难度:" & JSON.SelectToken("difficulty_name").ToString & vbCrLf
            Label2.Text += "标签:" & JSON.SelectToken("tags_name").ToString.Replace(vbCrLf, "") & vbCrLf
            Label2.Text += "最短时间:" & JSON.SelectToken("world_record_pretty").ToString & vbCrLf
            Label2.Text += "上传时间:" & JSON.SelectToken("upload_time_pretty").ToString & vbCrLf
            'LABEL2.Text += "过关条件:" & JSON.SelectToken("clear_condition_name").ToString & vbCrLf
            Label2.Text += "过关:" & JSON.SelectToken("clears").ToString & vbCrLf
            Label2.Text += "尝试:" & JSON.SelectToken("attempts").ToString & vbCrLf
            Label2.Text += "过关率：" & JSON.SelectToken("clear_rate").ToString & vbCrLf
            Label2.Text += "游玩次数:" & JSON.SelectToken("plays").ToString & vbCrLf
            Label2.Text += "游玩人数:" & JSON.SelectToken("unique_players_and_versus").ToString & vbCrLf
            Label2.Text += "赞:" & JSON.SelectToken("likes").ToString & vbCrLf
            Label2.Text += "孬:" & JSON.SelectToken("boos").ToString & vbCrLf
            Label2.Text += "对战游玩:" & JSON.SelectToken("versus_matches").ToString & vbCrLf
            Label2.Text += "合作游玩:" & JSON.SelectToken("coop_matches").ToString & vbCrLf
            Label2.Text += "本周游玩:" & JSON.SelectToken("weekly_plays").ToString & vbCrLf
            Label2.Text += "本周点赞:" & JSON.SelectToken("weekly_likes").ToString & vbCrLf


            Label3.Text = "关卡作者" & GetToken(JSON, "uploader")
            Label4.Text = "最先通过" & GetToken(JSON, "first_completer")
            Label5.Text = "最短时间" & GetToken(JSON, "record_holder")
            If IO.File.Exists(PT & "\MII\" & JSON.SelectToken("uploader").SelectToken("code").ToString) Then
                PicM0.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("uploader").SelectToken("code").ToString)
            Else
                Call URLDownloadToFile(0, JSON.SelectToken("uploader").SelectToken("mii_image").ToString.Replace("width=512&instanceCount=1", "width=128&instanceCount=16"),
                                       PT & "\MII\" & JSON.SelectToken("uploader").SelectToken("code").ToString, 0, 0)
                'PicM0.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("uploader").SelectToken("code").ToString)
            End If

            If IO.File.Exists(PT & "\MII\" & JSON.SelectToken("first_completer").SelectToken("code").ToString) Then
                ' PicM1.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("first_completer").SelectToken("code").ToString)
            Else
                Call URLDownloadToFile(0, JSON.SelectToken("first_completer").SelectToken("mii_image").ToString.Replace("width=512&instanceCount=1", "width=128&instanceCount=16"),
                                       PT & "\MII\" & JSON.SelectToken("first_completer").SelectToken("code").ToString, 0, 0)
                'PicM1.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("first_completer").SelectToken("code").ToString)
            End If

            If IO.File.Exists(PT & "\MII\" & JSON.SelectToken("record_holder").SelectToken("code").ToString) Then
                'PicM2.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("record_holder").SelectToken("code").ToString)
            Else
                Call URLDownloadToFile(0, JSON.SelectToken("record_holder").SelectToken("mii_image").ToString.Replace("width=512&instanceCount=1", "width=128&instanceCount=16"),
                                       PT & "\MII\" & JSON.SelectToken("record_holder").SelectToken("code").ToString, 0, 0)
                'PicM2.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("record_holder").SelectToken("code").ToString)
            End If
            MiiCache(0) = Image.FromFile(PT & "\MII\" & JSON.SelectToken("uploader").SelectToken("code").ToString)
            MiiCache(1) = Image.FromFile(PT & "\MII\" & JSON.SelectToken("first_completer").SelectToken("code").ToString)
            MiiCache(2) = Image.FromFile(PT & "\MII\" & JSON.SelectToken("record_holder").SelectToken("code").ToString)
            Timer2.Enabled = True
        End If
    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs)
        LoadLvlInfo()
    End Sub
    Function GetStrW(s As String) As Single
        Dim B As New Bitmap(300, 100)
        Dim G As Graphics = Graphics.FromImage(B), SZ As SizeF
        SZ = G.MeasureString(s, Button1.Font)
        GetStrW = SZ.Width
    End Function

    Private Sub Button14_Click(sender As Object, e As EventArgs)
        LoadLvlInfo()
    End Sub

    Dim MiiF As Integer, MiiCache(2) As Image
    Dim ScX() As String, ScY(3) As Integer
    Private Sub Button3_Click_1(sender As Object, e As EventArgs)
        Timer1.Enabled = Not Timer1.Enabled
        Me.Text = If(Timer1.Enabled, "ON", "OFF")
    End Sub
    Function GetC(x As Integer, y As Integer) As Color
        Dim B As New Bitmap(1, 1)
        Dim G As Graphics = Graphics.FromImage(B)
        G.CopyFromScreen(New Point(x, y), New Point(0, 0), New Drawing.Size(1, 1))
        Return B.GetPixel(0, 0)
    End Function

    Dim MiiB(2) As Bitmap, MiiG(2) As Graphics

    Private Sub Button6_Click_1(sender As Object, e As EventArgs) 
        Dim bcdFileBytes = File.ReadAllBytes(TextBox1.Text)
        Dim DeData = Decrypt(bcdFileBytes)
        File.WriteAllBytes(TextBox1.Text & ".DAT", DeData)
    End Sub

    Private Sub TextBox9_Click(sender As Object, e As EventArgs) Handles TextBox9.Click
        TextBox9.SelectAll()
    End Sub


    Dim BBB As New Bitmap(300, 50)
    Dim GGG As Graphics = Graphics.FromImage(BBB)

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        '255,206,1
        '0,0,0
        If GetC(1920 + 225, 57) = Color.FromArgb(255, 0, 0, 0) AndAlso
            GetC(1920 + 43, 209) = Color.FromArgb(255, 0, 0, 0) AndAlso
            GetC(1920 + 102, 105) = Color.FromArgb(255, 255, 206, 0) Then
            '225 57
            '43 209
            '102 105
            GGG.CopyFromScreen(New Point(1920 + 110, 0 + 260), New Point(0, 0), New Drawing.Size(300, 50))
            PS.Image = BBB
            TextBox9.Text = OCRbyImg(BBB)
            Threading.Thread.Sleep(5000)
        End If
    End Sub
    Public Function OCR(testImagePath As String) As String
        'OCR识别
        Try
            Dim engine As TesseractEngine = New TesseractEngine("./tessdata", "eng", EngineMode.Default)
            Dim img As Pix = Pix.LoadFromFile(testImagePath)
            Dim page As Page = engine.Process(img)
            Dim text As String = page.GetText()
            OCR = text
            'Dim iter As ResultIterator = page.GetIterator()
            'iter.Begin()
            'Do
            '    Do
            '        Do
            '            Do

            '                If iter.IsAtBeginningOf(PageIteratorLevel.Block) Then
            '                    Label1.Text += "<BLOCK>" & vbCrLf
            '                End If
            '                Label1.Text += iter.GetText(PageIteratorLevel.Word) & vbCrLf
            '                Label1.Text += vbCrLf
            '                If iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word) Then
            '                    Label1.Text += vbCrLf
            '                End If
            '            Loop While iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word)

            '            If iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine) Then
            '                Label1.Text += vbCrLf
            '            End If
            '        Loop While iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine)
            '    Loop While iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para)
            'Loop While iter.Next(PageIteratorLevel.Block)
        Catch R As Exception
            OCR = "文本识别错误：" & R.Message & vbCrLf & R.ToString
        End Try
    End Function
    Public Function OCRbyImg(B As Image) As String
        'OCR识别
        Dim RE As String
        Try
            Dim engine As New TesseractEngine("./tessdata", "eng", EngineMode.Default)
            Dim MS As New MemoryStream
            B.Save(MS, Imaging.ImageFormat.Png)
            Dim Buffer() As Byte
            ReDim Buffer(MS.Length)
            MS.Seek(0, SeekOrigin.Begin)
            MS.Read(Buffer, 0, MS.Length)
            Dim img As Pix = Pix.LoadFromMemory(Buffer)
            Dim page As Page = engine.Process(img)
            Dim text As String = page.GetText()
            RE = text
            'Dim iter As ResultIterator = page.GetIterator()
            'iter.Begin()
            'Do
            '    Do
            '        Do
            '            Do

            '                If iter.IsAtBeginningOf(PageIteratorLevel.Block) Then
            '                    Label1.Text += "<BLOCK>" & vbCrLf
            '                End If
            '                Label1.Text += iter.GetText(PageIteratorLevel.Word) & vbCrLf
            '                Label1.Text += vbCrLf
            '                If iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word) Then
            '                    Label1.Text += vbCrLf
            '                End If
            '            Loop While iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word)

            '            If iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine) Then
            '                Label1.Text += vbCrLf
            '            End If
            '        Loop While iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine)
            '    Loop While iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para)
            'Loop While iter.Next(PageIteratorLevel.Block)
        Catch R As Exception
            'OCRbyImg = "文本识别错误：" & R.Message & vbCrLf & R.ToString
            RE = ""
        End Try
        RE = UCase(RE)
        RE = RE.Replace("O", "0")
        RE = RE.Replace("I", "1")
        RE = RE.Replace("Z", "2")
        Return RE
    End Function

    Private Sub Button15_Click(sender As Object, e As EventArgs)
        Timer2.Enabled = True
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        MiiG(0).Clear(Color.Transparent)
        MiiG(0).DrawImage(MiiCache(0), New Rectangle(0, 0, 128, 128), New Rectangle(MiiF * 128, 0, 128, 128), GraphicsUnit.Pixel)
        PicM0.Image = MiiB(0)
        MiiG(1).Clear(Color.Transparent)
        MiiG(1).DrawImage(MiiCache(1), New Rectangle(0, 0, 128, 128), New Rectangle(MiiF * 128, 0, 128, 128), GraphicsUnit.Pixel)
        PicM1.Image = MiiB(1)
        MiiG(2).Clear(Color.Transparent)
        MiiG(2).DrawImage(MiiCache(2), New Rectangle(0, 0, 128, 128), New Rectangle(MiiF * 128, 0, 128, 128), GraphicsUnit.Pixel)
        PicM2.Image = MiiB(2)
        MiiF += 1
        MiiF = MiiF Mod 16
    End Sub


    Dim GG As Graphics
    Dim GB As Bitmap
    Dim Tile As Image, TileW As Integer
    Public Function GetTile(x As Integer, y As Integer, w As Integer, h As Integer) As Image
        GB = New Bitmap(TileW * w, TileW * h)
        GG = Graphics.FromImage(GB)
        GG.DrawImage(Tile, New Rectangle(0, 0, TileW * w, TileW * h), New Rectangle(TileW * x, TileW * y, TileW * w, TileW * h), GraphicsUnit.Pixel)
        GetTile = GB
    End Function
End Class
