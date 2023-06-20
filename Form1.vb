Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports System
Imports System.Diagnostics
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports Tesseract
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports MaterialSkin.Controls
Imports System.Runtime.CompilerServices
Imports System.Reflection.Emit
Imports Cyotek.Windows.Forms

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        isMapIO = True
        RefPic()
        'Button2.Enabled = True
    End Sub

    Public Sub LoadEFile()
        '关卡文件头H00长度200
        LoadLvlData(TextBox1.Text)
        'Label2.Text = "图名：" & LH.Name & vbCrLf
        'Label2.Text += "描述：" & LH.Desc & vbCrLf
        'Label2.Text += "时间：" & LH.Timer & vbCrLf
        'Label2.Text += "风格：" & LH.GameStyle & vbCrLf
        'Label2.Text += "版本：" & LH.GameVer & vbCrLf
        'Label2.Text += "起点：" & LH.StartY & vbCrLf
        'Label2.Text += "终点：" & LH.GoalX & "," & LH.GoalY & vbCrLf
        'Label2.Text += "======表世界======" & vbCrLf
        'Label2.Text += "主题：" & MH(0).Theme & vbCrLf
        'Label2.Text += "宽度：" & MH(0).BorR & vbCrLf
        'Label2.Text += "高度：" & MH(0).BorT & vbCrLf
        'Label2.Text += "砖块：" & MH(0).GroundCount & vbCrLf
        'Label2.Text += "单位：" & MH(0).ObjCount & vbCrLf
        'Label2.Text += "轨道：" & MH(0).TrackCount & vbCrLf
        'Label2.Text += "卷轴：" & MH(0).AutoscrollType & vbCrLf
        'Label2.Text += "水面：" & MH(0).LiqSHeight & "-" & MH(0).LiqEHeight & vbCrLf
        'Label2.Text += "======里世界======" & vbCrLf
        'Label2.Text += "主题：" & MH(1).Theme & vbCrLf
        'Label2.Text += "宽度：" & MH(1).BorR & vbCrLf
        'Label2.Text += "高度：" & MH(1).BorT & vbCrLf
        'Label2.Text += "砖块：" & MH(1).GroundCount & vbCrLf
        'Label2.Text += "单位：" & MH(1).ObjCount & vbCrLf
        'Label2.Text += "轨道：" & MH(1).TrackCount & vbCrLf
        'Label2.Text += "卷轴：" & MH(1).AutoscrollType & vbCrLf
        'Label2.Text += "水面：" & MH(1).LiqSHeight & "-" & MH(1).LiqEHeight & vbCrLf
        TTitle.Text = LH.Name
        'TMaker.Text = LH.UploadID.ToString
        TTime.Text = LH.Timer.ToString
        'TCR.Text = LH.ClearCA.ToString
        TMAttempt.Text = LH.ClearAttempts.ToString
        TGameVer.Text = GameVerStr(CInt(LH.ClearVer))
        TDesc.Text = LH.Desc
        'TMTime.Text = LH.ClearTime.ToString
        'TCR.Text = LH.ClearCC.ToString & "," & LH.ClearCA.ToString
        TDate.Text = LH.DateYY.ToString & "-" & LH.DateMM.ToString & "-" & LH.DateDD.ToString '& " " & LH.DateH.ToString & ":" & LH.DateM.ToString
        PGameStyle.Image = Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\logo.png")
        PGameTheme1.Image = Image.FromFile(PT & "\img\icon\" & MH(0).Theme.ToString & ".png")
        PGameTheme2.Image = Image.FromFile(PT & "\img\icon\" & MH(1).Theme.ToString & ".png")
        '.StartY = R(1, 0)
        '.GoalY = R(1, 1)
        '.GoalX = R(2, 2)
        '.Timer = R(2, 4)
        '.ClearCA = R(2, 6)
        '.DateYY = R(2, 8)
        '.DateMM = R(1, 10)
        '.DateDD = R(1, 11)
        '.DateH = R(1, 12)
        '.DateM = R(1, 13)
        '.AutoscrollSpd = R(1, 14)
        '.ClearCC = R(1, 15)
        '.ClearCRC = R(4, 16)
        '.GameVer = R(4, 20)
        '.MFlag = R(4, 24)
        '.ClearAttempts = R(4, 28)
        '.ClearTime = R(4, 32)
        '.CreationID = R(4, 36)
        '.UploadID = R(8, 40)
        '.ClearVer = R(4, 48)
        '.GameStyle = R(2, 241)
        '.Name = R(66, 244)
        '.Desc = R(202, 310)

        Dim LInfo() As FieldInfo
        Dim I As FieldInfo
        LInfo = LH.GetType.GetFields()
        TextBox2.Text = ""
        For Each I In LInfo
            TextBox2.Text += I.Name & ":" & I.GetValue(LH) & vbCrLf
        Next
        TextBox3.Text = "===M0===" & vbCrLf
        '表世界H200长度2DEE0
        LInfo = MH(0).GetType.GetFields()
        For Each I In LInfo
            TextBox3.Text += I.Name & ":" & I.GetValue(MH(0)).ToString & vbCrLf
        Next
        TextBox4.Text = "===M1===" & vbCrLf
        '表世界H200长度2DEE0
        LInfo = MH(1).GetType.GetFields()
        For Each I In LInfo
            TextBox4.Text += I.Name & ":" & I.GetValue(MH(1)).ToString & vbCrLf
        Next
    End Sub

    Dim BMAP(1, 1), BMAPR(1) As Bitmap
    Dim BSIZE(1) As Size
    Dim G(1) As Graphics
    'Const G_BG = 0
    'Const G_LIQ = 1
    'Const G_GRID = 2
    'Const G_SEMI = 3
    'Const G_BITEM = 4
    'Const G_GROUND = 5
    'Const G_DEATH = 6
    'Const G_BLOCK = 7
    'Const G_ITEM = 8
    'Const G_HIDE = 9
    'Const G_FITEM = 10
    'Const G_TRACK = 11
    'Const G_LINK = 12
    'Const G_SND = 13

    Const G_BG = 0
    Const G_LIQ = 0
    Const G_GRID = 0
    Const G_SEMI = 0
    Const G_BITEM = 0
    Const G_GROUND = 0
    Const G_DEATH = 0
    Const G_BLOCK = 0
    Const G_ITEM = 0
    Const G_HIDE = 1
    Const G_FITEM = 0
    Const G_TRACK = 0
    Const G_LINK = 0
    Const G_SND = 0

    Public Sub InitPng()
        Dim i As Integer
        Dim H As Integer, W As Integer
        H = MH(0).BorT \ 16
        W = MH(0).BorR \ 16
        PZoom = 32 '固定缩放倍率 2 ^ TrackBar1.Value
        BSIZE(0).Width = W * PZoom
        BSIZE(0).Height = H * PZoom
        For i = 0 To BMAP.GetLength(1) - 1
            BMAP(0, i) = New Bitmap(BSIZE(0).Width, BSIZE(0).Height)
            G(i) = Graphics.FromImage(BMAP(0, i))
            G(i).InterpolationMode = InterpolationMode.NearestNeighbor
            G(i).SmoothingMode = SmoothingMode.HighQuality
        Next

        For i = 0 To H
            G(G_GRID).DrawLine(Pens.LightGray, 0, CSng(i * PZoom), CSng(W * PZoom), CSng(i * PZoom))
            If i Mod 13 = 0 Then
                G(G_GRID).DrawLine(Pens.LightGray, 0, CSng((H - i) * PZoom + 1), CSng(W * PZoom), CSng((H - i) * PZoom + 1))
            End If
            If (H - i) Mod 10 = 0 Then
                G(G_GRID).DrawString((H - i).ToString, Button1.Font, Brushes.Black, 0, CSng(i * PZoom))
            End If
        Next
        For i = 0 To W
            G(G_GRID).DrawLine(Pens.LightGray, CSng(i * PZoom), 0, CSng(i * PZoom), CSng(H * PZoom))
            If i Mod 24 = 0 Then
                G(G_GRID).DrawLine(Pens.LightGray, CSng(i * PZoom + 1), 0, CSng(i * PZoom + 1), CSng(H * PZoom))
            End If
            If i Mod 10 = 9 Then
                G(G_GRID).DrawString((i + 1).ToString, Button1.Font, Brushes.Black, CSng(i * PZoom), 0)
            End If
        Next
        '
        Dim BC1, BC2 As Color
        If MH(0).Theme = 2 Then
            BC1 = Color.FromArgb(64, 255, 0, 0)
            BC2 = Color.FromArgb(64, 255, 0, 0)
            G(G_LIQ).FillRectangle(New SolidBrush(BC2), 0, CSng((H - MH(0).LiqEHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
            G(G_LIQ).FillRectangle(New SolidBrush(BC1), 0, CSng((H - MH(0).LiqSHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
        ElseIf MH(0).Theme = 9 Then
            BC1 = Color.FromArgb(64, 0, 0, 255)
            BC2 = Color.FromArgb(64, 0, 0, 255)
            G(G_LIQ).FillRectangle(New SolidBrush(BC2), 0, CSng((H - MH(0).LiqEHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
            G(G_LIQ).FillRectangle(New SolidBrush(BC1), 0, CSng((H - MH(0).LiqSHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
        End If

    End Sub
    Public Sub InitPng2()
        Dim i As Integer
        Dim H As Integer, W As Integer
        H = MH(1).BorT \ 16
        W = MH(1).BorR \ 16
        PZoom = 32 '固定缩放倍率 2 ^ TrackBar1.Value
        BSIZE(1).Width = W * PZoom
        BSIZE(1).Height = H * PZoom
        For i = 0 To BMAP.GetLength(1) - 1
            BMAP(1, i) = New Bitmap(W * PZoom, H * PZoom)
            G(i) = Graphics.FromImage(BMAP(1, i))
            G(i).InterpolationMode = InterpolationMode.NearestNeighbor
            G(i).SmoothingMode = SmoothingMode.HighQuality
        Next

        For i = 0 To H
            G(G_GRID).DrawLine(Pens.WhiteSmoke, 0, CSng(i * PZoom), CSng(W * PZoom), CSng(i * PZoom))
            If i Mod 13 = 0 Then
                G(G_GRID).DrawLine(Pens.WhiteSmoke, 0, CSng((H - i) * PZoom + 1), CSng(W * PZoom), CSng((H - i) * PZoom + 1))
            End If
            If (H - i) Mod 10 = 0 Then
                G(G_GRID).DrawString((H - i).ToString, Button1.Font, Brushes.Black, 0, CSng(i * PZoom))
            End If
        Next
        For i = 0 To W
            G(G_GRID).DrawLine(Pens.WhiteSmoke, CSng(i * PZoom), 0, CSng(i * PZoom), CSng(H * PZoom))
            If i Mod 24 = 0 Then
                G(G_GRID).DrawLine(Pens.WhiteSmoke, CSng(i * PZoom + 1), 0, CSng(i * PZoom + 1), CSng(H * PZoom))
            End If
            If i Mod 10 = 9 Then
                G(G_GRID).DrawString((i + 1).ToString, Button1.Font, Brushes.Black, CSng(i * PZoom), 0)
            End If
        Next

        Dim BC1, BC2 As Color
        If MH(1).Theme = 2 Then
            BC1 = Color.FromArgb(64, 255, 0, 0)
            BC2 = Color.FromArgb(64, 255, 0, 0)
            G(G_LIQ).FillRectangle(New SolidBrush(BC2), 0, CSng((H - MH(1).LiqEHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
            G(G_LIQ).FillRectangle(New SolidBrush(BC1), 0, CSng((H - MH(1).LiqSHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
        ElseIf MH(1).Theme = 9 Then
            BC1 = Color.FromArgb(64, 0, 0, 255)
            BC2 = Color.FromArgb(64, 0, 0, 255)
            G(G_LIQ).FillRectangle(New SolidBrush(BC2), 0, CSng((H - MH(1).LiqEHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
            G(G_LIQ).FillRectangle(New SolidBrush(BC1), 0, CSng((H - MH(1).LiqSHeight - 0.5) * PZoom), CSng(W * PZoom), CSng(H * PZoom))
        End If

    End Sub
    Public Sub DrawMoveBlock(DR As Integer, ID As Byte, EX As Byte, X As Integer, Y As Integer)
        Dim H, W, XX, YY As Integer
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16
        XX = X / 160 + 1
        YY = (Y + 80) / 160 + 1
        Dim i As Integer

        Select Case ID
            Case 85
                Select Case MapTrackBlk(DR, EX - 1).Node(0).p1
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
                For i = 0 To MapTrackBlk(DR, EX - 1).NodeCount - 1
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SS.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    'G.DrawString(MapTrackBlk(DR,EX - 1).Node(i).p1, Me.Font, Brushes.Black, (XX) * PZOOM, (H - YY) * PZOOM)
                    Select Case MapTrackBlk(DR, EX - 1).Node(i).p1
                        Case 1 'L
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 2 'R
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 3 'D
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 4 'U
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 5 'LD
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SRD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 6 'DL
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SUL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 7 'LU
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SRU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 8 'UL
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SDL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 9 'RD
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SLD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 10 'DR
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SUR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 11 'RU
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SLU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 12 'UR
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SDR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 13 'RE
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 14 'LE
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 15 'UE
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 16 'DE
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    End Select
                Next
            Case 119
                Select Case MapMoveBlk(DR, EX - 1).Node(0).p1
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
                For i = 0 To MapMoveBlk(DR, EX - 1).NodeCount - 1
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SS.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    'G(G_ITEM).DrawString(MapMoveBlk(DR,EX - 1).Node(i).p1, Me.Font, Brushes.Black, (XX) * PZOOM, (H - YY) * PZOOM)
                    Select Case MapMoveBlk(DR, EX - 1).Node(i).p1
                        Case 1 'L
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 2 'R
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 3 'D
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 4 'U
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 5 'LD
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SRD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 6 'DL
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SUL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 7 'LU
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SRU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 8 'UL
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SDL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX -= 2
                        Case 9 'RD
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SLD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY -= 2
                        Case 10 'DR
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SUR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 11 'RU
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SLU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            YY += 2
                        Case 12 'UR
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SDR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                            XX += 2
                        Case 13 'RE
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 14 'LE
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 15 'UE
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                        Case 16 'DE
                            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    End Select
                Next
        End Select

    End Sub
    Public Sub DrawCrp(DR As Integer, EX As Byte, X As Integer, Y As Integer)
        Dim H, W, XX, YY As Integer
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16
        XX = X / 160 + 1
        YY = (Y + 80) / 160 + 1
        Dim i As Integer

        Select Case MapCrp(DR, EX - 1).Node(0)
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

        For i = 0 To MapCrp(DR, EX - 1).NodeCount - 1
            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SS.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
            'G(G_ITEM).DrawString(MapCrp(DR,EX - 1).Node(i), Me.Font, Brushes.Black, (XX) * PZOOM, (H - YY) * PZOOM)
            Select Case MapCrp(DR, EX - 1).Node(i)
                Case 1 'L
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX -= 2
                Case 2 'R
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX += 2
                Case 3 'D
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY -= 2
                Case 4 'U
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY += 2
                Case 5 'LD
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SRD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY -= 2
                Case 6 'DL
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SUL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX -= 2
                Case 7 'LU
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SRU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY += 2
                Case 8 'UL
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SDL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX -= 2
                Case 9 'RD
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SLD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY -= 2
                Case 10 'DR
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SUR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX += 2
                Case 11 'RU
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SLU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    YY += 2
                Case 12 'UR
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SDR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                    XX += 2
                Case 13 'RE
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                Case 14 'LE
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                Case 15 'UE
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
                Case 16 'DE
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom * 2, PZoom * 2)
            End Select
        Next
    End Sub
    Public Sub DrawSnake(DR As Integer, EX As Byte, X As Integer, Y As Integer, SW As Integer, SH As Integer)
        '蛇砖块
        On Error GoTo Err
        Dim H, W, XX, YY As Integer
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16

        YY = (Y + SH * 80) / 160
        If EX < &H10 Then
            XX = (X + SW * 80) / 160
            EX = EX Mod &H10
            Select Case MapSnk(DR, EX - 1).Node(0).Dir
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
            Select Case MapSnk(DR, EX - 1).Node(0).Dir
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


        For i = 0 To MapSnk(DR, EX - 1).NodeCount - 1
            G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SS.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
            'G(G_ITEM).DrawString(MapSnk(DR,EX - 1).Node(i).Dir, Me.Font, Brushes.Black, (XX + 0.5) * PZOOM, (H - YY - 0.5) * PZOOM)
            Select Case MapSnk(DR, EX - 1).Node(i).Dir
                Case 1 'L
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX -= 1
                Case 2 'R
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX += 1
                Case 3 'D
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY -= 1
                Case 4 'U
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY += 1
                Case 5 'LD
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SRD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY -= 1
                Case 6 'DL
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SUL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX -= 1
                Case 7 'LU
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SRU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY += 1
                Case 8 'UL
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SDL.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX -= 1
                Case 9 'RD
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SLD.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY -= 1
                Case 10 'DR
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SUR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX += 1
                Case 11 'RU
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SLU.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    YY += 1
                Case 12 'UR
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SDR.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                    XX += 1
                Case 13 'RE
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                Case 14 'LE
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                Case 15 'UE
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
                Case 16 'DE
                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\img\CMN\SE.PNG"), XX * PZoom, (H - YY) * PZoom, PZoom, PZoom)
            End Select

        Next

Err:
    End Sub
    Public Sub DrawIce(DR As Integer)
        '冰块
        Dim i As Integer, H As Integer
        For i = 0 To MH(DR).IceCount - 1
            If MapIce(DR, i).ID = 0 Then
                G(G_ITEM).DrawImage(GetTile(15, 41, 1, 2), MapIce(DR, i).X * PZoom, (MH(DR).BorT \ 16 - 2) * PZoom - MapIce(DR, i).Y * PZoom, PZoom, PZoom * 2)
                For H = 1 To 2
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).Obj += "118,"
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).Flag += ","
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).SubObj += ","
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).SubFlag += ","
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).State += ","
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).SubState += ","
                Next
            Else
                G(G_ITEM).DrawImage(GetTile(15, 39, 1, 2), MapIce(DR, i).X * PZoom, (MH(DR).BorT \ 16 - 2) * PZoom - MapIce(DR, i).Y * PZoom, PZoom, PZoom * 2)
                For H = 1 To 2
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).Obj += "118A,"
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).Flag += ","
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).SubObj += ","
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).SubFlag += ","
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).State += ","
                    ObjLocData(DR, MapIce(DR, i).X + 1, Int(H + MapIce(DR, i).Y)).SubState += ","
                Next
            End If
        Next
    End Sub
    Public Sub DrawTrack(DR As Integer)
        '轨道
        Dim H As Integer, W As Integer
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16
        Dim i As Integer
        For i = 0 To MH(DR).TrackCount - 1
            'LID+1?
            ObjLinkType(DR, MapTrk(DR, i).LID) = 59
            'Debug.Print(MapTrk(DR, i).Type & "," & (MapTrk(DR, i).X - 1) & "," & (H - 2 - MapTrk(DR, i).Y))
            If MapTrk(DR, i).Type < 8 Then
                'G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T" & MapTrk(DR, i).Type.ToString & ".PNG"), MapTrk(DR, i).X * PZoom - PZoom, (H - 2) * PZoom - MapTrk(DR, i).Y * PZoom, PZoom * 3, PZoom * 3)
                G(G_TRACK).DrawImage(TrackImg(MapTrk(DR, i).Type), (MapTrk(DR, i).X - 1) * PZoom, (H - 2 - MapTrk(DR, i).Y) * PZoom, PZoom * 3, PZoom * 3)
                Select Case MapTrk(DR, i).Type
                    Case 0
                        If TrackNode(DR, MapTrk(DR, i).X + 1 + 1, MapTrk(DR, i).Y + 1) = 1 AndAlso MapTrk(DR, i).F0 = 0 Then
                            G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(DR, i).X * PZoom + PZoom, (H - 1) * PZoom - MapTrk(DR, i).Y * PZoom, PZoom, PZoom)
                        End If
                        If TrackNode(DR, MapTrk(DR, i).X + 1 - 1, MapTrk(DR, i).Y + 1) = 1 AndAlso MapTrk(DR, i).F1 = 0 Then
                            G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(DR, i).X * PZoom - PZoom, (H - 1) * PZoom - MapTrk(DR, i).Y * PZoom, PZoom, PZoom)
                        End If
                    Case 1
                        If TrackNode(DR, MapTrk(DR, i).X + 1, MapTrk(DR, i).Y + 1 + 1) = 1 AndAlso MapTrk(DR, i).F0 = 0 Then
                            G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(DR, i).X * PZoom, (H - 2) * PZoom - MapTrk(DR, i).Y * PZoom, PZoom, PZoom)
                        End If
                        If TrackNode(DR, MapTrk(DR, i).X + 1, MapTrk(DR, i).Y + 1 - 1) = 1 AndAlso MapTrk(DR, i).F1 = 0 Then
                            G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(DR, i).X * PZoom, H * PZoom - MapTrk(DR, i).Y * PZoom, PZoom, PZoom)
                        End If
                    Case 2, 4, 5
                        If TrackNode(DR, MapTrk(DR, i).X + 1 + 1, MapTrk(DR, i).Y + 1 - 1) = 1 AndAlso MapTrk(DR, i).F0 = 0 Then
                            G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(DR, i).X * PZoom + PZoom, H * PZoom - MapTrk(DR, i).Y * PZoom, PZoom, PZoom)
                        End If
                        If TrackNode(DR, MapTrk(DR, i).X + 1 - 1, MapTrk(DR, i).Y + 1 + 1) = 1 AndAlso MapTrk(DR, i).F1 = 0 Then
                            G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(DR, i).X * PZoom - PZoom, (H - 2) * PZoom - MapTrk(DR, i).Y * PZoom, PZoom, PZoom)
                        End If
                    Case 3, 6, 7
                        If TrackNode(DR, MapTrk(DR, i).X + 1 + 1, MapTrk(DR, i).Y + 1 + 1) = 1 AndAlso MapTrk(DR, i).F0 = 0 Then
                            G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(DR, i).X * PZoom + PZoom, (H - 2) * PZoom - MapTrk(DR, i).Y * PZoom, PZoom, PZoom)
                        End If
                        If TrackNode(DR, MapTrk(DR, i).X + 1 - 1, MapTrk(DR, i).Y + 1 - 1) = 1 AndAlso MapTrk(DR, i).F1 = 0 Then
                            G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), MapTrk(DR, i).X * PZoom - PZoom, H * PZoom - MapTrk(DR, i).Y * PZoom, PZoom, PZoom)
                        End If
                End Select
            Else 'Y轨道
                'G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T" & MapTrk(DR, i).Type.ToString & ".PNG"), MapTrk(DR, i).X * PZoom - PZoom, (H - 4) * PZoom - MapTrk(DR, i).Y * PZoom, PZoom * 5, PZoom * 5)
                G(G_TRACK).DrawImage(TrackImg(MapTrk(DR, i).Type), (MapTrk(DR, i).X - 1) * PZoom, (H - 4 - MapTrk(DR, i).Y) * PZoom, PZoom * 5, PZoom * 5)

                'G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(DR,i).X - 1 + TrackYPt(MapTrk(DR,i).Type, 0).X) * PZOOM, H * PZOOM - (MapTrk(DR,i).Y + TrackYPt(MapTrk(DR,i).Type, 0).Y) * PZOOM, PZOOM, PZOOM)
                'G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(DR,i).X - 1 + TrackYPt(MapTrk(DR,i).Type, 1).X) * PZOOM, H * PZOOM - (MapTrk(DR,i).Y + TrackYPt(MapTrk(DR,i).Type, 1).Y) * PZOOM, PZOOM, PZOOM)
                'G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(DR,i).X - 1 + TrackYPt(MapTrk(DR,i).Type, 2).X) * PZOOM, H * PZOOM - (MapTrk(DR,i).Y + TrackYPt(MapTrk(DR,i).Type, 2).Y) * PZOOM, PZOOM, PZOOM)

                If TrackNode(DR, MapTrk(DR, i).X + TrackYPt(MapTrk(DR, i).Type, 0).X, MapTrk(DR, i).Y + TrackYPt(MapTrk(DR, i).Type, 0).Y) = 1 AndAlso MapTrk(DR, i).F0 = 0 Then
                    G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(DR, i).X - 1 + TrackYPt(MapTrk(DR, i).Type, 0).X) * PZoom, (H - 4) * PZoom - (MapTrk(DR, i).Y - TrackYPt(MapTrk(DR, i).Type, 0).Y) * PZoom, PZoom, PZoom)
                End If
                If TrackNode(DR, MapTrk(DR, i).X + TrackYPt(MapTrk(DR, i).Type, 1).X, MapTrk(DR, i).Y + TrackYPt(MapTrk(DR, i).Type, 1).Y) = 1 AndAlso MapTrk(DR, i).F1 = 0 Then
                    G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(DR, i).X - 1 + TrackYPt(MapTrk(DR, i).Type, 1).X) * PZoom, (H - 4) * PZoom - (MapTrk(DR, i).Y - TrackYPt(MapTrk(DR, i).Type, 1).Y) * PZoom, PZoom, PZoom)
                End If
                If TrackNode(DR, MapTrk(DR, i).X + TrackYPt(MapTrk(DR, i).Type, 2).X, MapTrk(DR, i).Y + TrackYPt(MapTrk(DR, i).Type, 2).Y) = 1 AndAlso MapTrk(DR, i).F2 = 0 Then
                    G(G_TRACK).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(DR, i).X - 1 + TrackYPt(MapTrk(DR, i).Type, 2).X) * PZoom, (H - 4) * PZoom - (MapTrk(DR, i).Y - TrackYPt(MapTrk(DR, i).Type, 2).Y) * PZoom, PZoom, PZoom)
                End If
            End If
        Next
    End Sub

    Function GetGrdCodePlus(DR As Integer, X As Integer, Y As Integer) As Integer
        Dim code As Integer
        Dim E(), C() As Integer
        E = {0, 0, 0, 0}
        C = {0, 0, 0, 0}
        '0无 1方 2左上 3右上 4左下 5右下
        '6小左上 7小右上 8小左下 9小右上
        '10大左上 11大右上 12大左下 13大右下

        '角代码
        Select Case Hex(GroundNode(DR, X - 1, Y)) &
                    Hex(GroundNode(DR, X - 1, Y + 1)) &
                    Hex(GroundNode(DR, X, Y + 1))'左上
            Case "101", "501"
                C(0) = 1
            Case "105", "505"
                C(0) = 2
            Case "109", "D09", "D05", "509"
                C(0) = 3
            Case Else
                C(0) = 0
        End Select
        Select Case Hex(GroundNode(DR, X + 1, Y)) &
                    Hex(GroundNode(DR, X + 1, Y + 1)) &
                    Hex(GroundNode(DR, X, Y + 1))'右上
            Case "101", "401"
                C(1) = 1
            Case "104", "404"
                C(1) = 2
            Case "108", "C08", "C04", "408"
                C(1) = 3
            Case Else
                C(1) = 0
        End Select
        Select Case Hex(GroundNode(DR, X - 1, Y)) &
                    Hex(GroundNode(DR, X - 1, Y - 1)) &
                    Hex(GroundNode(DR, X, Y - 1))'左下
            Case "101", "301"
                C(2) = 1
            Case "103", "303"
                C(2) = 2
            Case "107", "B07", "B03", "307"
                C(2) = 3
            Case Else
                C(2) = 0
        End Select
        Select Case Hex(GroundNode(DR, X + 1, Y)) &
                    Hex(GroundNode(DR, X + 1, Y - 1)) &
                    Hex(GroundNode(DR, X, Y - 1))'右下
            Case "101", "201"
                C(3) = 1
            Case "102", "202"
                C(3) = 2
            Case "106", "A06", "A02", "206"
                C(3) = 3
            Case Else
                C(3) = 0
        End Select
        '边代码
        Select Case GroundNode(DR, X - 1, Y)'左
            Case 0
                E(0) = 1 : C(0) = 0 : C(2) = 0
            Case Else
                E(0) = 0
        End Select
        Select Case GroundNode(DR, X, Y + 1)'上
            Case 0
                E(1) = 1 : C(0) = 0 : C(1) = 0
            Case 13
                E(1) = 2 : C(0) = 0 : C(1) = 0
            Case 12
                E(1) = 3 : C(0) = 0 : C(1) = 0
            Case Else
                E(1) = 0
        End Select
        Select Case GroundNode(DR, X + 1, Y)'右
            Case 0
                E(2) = 1 : C(1) = 0 : C(3) = 0
            Case Else
                E(2) = 0
        End Select
        Select Case GroundNode(DR, X, Y - 1)'下
            Case 0
                E(3) = 1 : C(2) = 0 : C(3) = 0
            Case 11
                E(3) = 2 : C(2) = 0 : C(3) = 0
            Case 10
                E(3) = 3 : C(2) = 0 : C(3) = 0
            Case Else
                E(3) = 0
        End Select
        code = E(0) * 4 ^ 7 + E(1) * 4 ^ 6 + E(2) * 4 ^ 5 + E(3) * 4 ^ 4 +
                C(0) * 4 ^ 3 + C(1) * 4 ^ 2 + C(2) * 4 + C(3)
        For i As Integer = 0 To GrdTbl_Code.Length - 1
            If code = GrdTbl_Code(i) Then
                Return GrdTbl_Tile(i)
            End If
        Next
        Return 0
    End Function
    Public Sub DrawGrdCode(DR As Integer)
        '绘制地形
        Dim i, j As Integer
        Dim H, W As Integer
        Dim Code As Integer
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16
        'NewGrdType
        '0无 1方 2左上 3右上 4左下 5右下
        '6小左上 7小右上 8小左下 9小右上
        '10大左上 11大右上 12大左下 13大右下
        For i = 1 To W + 1
            For j = 1 To H + 1
                Select Case GroundNode(DR, i, j)
                    Case 0
                        Code = 0
                    Case 1
                        Code = GetGrdCodePlus(DR, i, j)
                    Case 2
                        If GroundNode(DR, i, j + 1) = 5 Then
                            Code = 402
                        ElseIf GroundNode(DR, i - 1, j + 1) = 1 Then
                            Code = 433
                        Else
                            Code = 435
                        End If
                    Case 3
                        If GroundNode(DR, i, j + 1) = 4 Then
                            Code = 403
                        ElseIf GroundNode(DR, i + 1, j + 1) = 1 Then
                            Code = 432
                        Else
                            Code = 434
                        End If
                    Case 4
                        If GroundNode(DR, i, j - 1) = 3 Then
                            Code = 387
                        ElseIf GroundNode(DR, i - 1, j - 1) = 1 Then
                            Code = 385
                        Else
                            Code = 419
                        End If
                    Case 5
                        If GroundNode(DR, i, j - 1) = 2 Then
                            Code = 386
                        ElseIf GroundNode(DR, i + 1, j - 1) = 1 Then
                            Code = 384
                        Else
                            Code = 418
                        End If
                    Case 6
                        If GroundNode(DR, i, j + 1) = 13 Then
                            Code = 529
                        Else
                            Code = 499
                        End If
                    Case 7
                        If GroundNode(DR, i, j + 1) = 12 Then
                            Code = 530
                        Else
                            Code = 496
                        End If
                    Case 8
                        If GroundNode(DR, i, j - 1) = 11 Then
                            Code = 515
                        Else
                            Code = 451
                        End If
                    Case 9
                        If GroundNode(DR, i, j - 1) = 10 Then
                            Code = 512
                        Else
                            Code = 448
                        End If
                    Case 10
                        If GroundNode(DR, i, j + 1) = 9 Then
                            Code = 528
                        ElseIf GroundNode(DR, i - 1, j + 1) = 1 Then
                            Code = 498
                        Else
                            Code = 501
                        End If
                    Case 11
                        If GroundNode(DR, i, j + 1) = 8 Then
                            Code = 531
                        ElseIf GroundNode(DR, i + 1, j + 1) = 1 Then
                            Code = 497
                        Else
                            Code = 500
                        End If
                    Case 12
                        If GroundNode(DR, i, j - 1) = 7 Then
                            Code = 514
                        ElseIf GroundNode(DR, i - 1, j - 1) = 1 Then
                            Code = 450
                        Else
                            Code = 453
                        End If
                    Case 13
                        If GroundNode(DR, i, j - 1) = 6 Then
                            Code = 513
                        ElseIf GroundNode(DR, i + 1, j - 1) = 1 Then
                            Code = 449
                        Else
                            Code = 452
                        End If
                    Case Else
                        Code = -1
                End Select

                '两格宽矩形
                Select Case Code
                    Case 242
                        If GroundNode(DR, i + 2, j) = 0 Then Code = 184
                    Case 244
                        If GroundNode(DR, i - 2, j) = 0 Then Code = 185
                    Case 245
                        If GroundNode(DR, i + 2, j) = 0 Then Code = 186
                    Case 247
                        If GroundNode(DR, i - 2, j) = 0 Then Code = 187
                    Case 248
                        If GroundNode(DR, i + 2, j) = 0 Then Code = 188
                    Case 250
                        If GroundNode(DR, i - 2, j) = 0 Then Code = 189
                End Select

                '起点终点 未优化

                If Code > 0 Then
                    G(G_GROUND).DrawImage(GetTile(Code Mod 16, Code \ 16, 1, 1, i - 1, H - j), (i - 1) * PZoom, (H - j) * PZoom, PZoom, PZoom)
                End If

                'If GroundNode(DR, i, j) > 0 Then
                '    For m = -1 To 1
                '        For n = -1 To 1
                '            G(G_GROUND).DrawString(Code.ToString, Me.Font, Brushes.White, (i - 1) * PZoom + m + 2, (H - j) * PZoom + n + 2)
                '        Next
                '    Next
                '    G(G_GROUND).DrawString(Code.ToString, Me.Font, Brushes.Black, (i - 1) * PZoom + 2, (H - j) * PZoom + 2)
                'End If


            Next
        Next

    End Sub
    Public Sub SetSlopeGrdCode(DR As Integer)
        Dim i, j As Integer
        Dim H, W As Integer
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16
        Dim CX, CY As Integer
        'NewGrdType
        '0无 1方 2左上 3右上 4左下 5右下
        '6小左上 7小右上 8小左下 9小右下
        '10大左上 11大右上 12大左下 13大右下

        '设置斜坡代码
        For i = 0 To MH(DR).ObjCount - 1
            Select Case MapObj(DR, i).ID
                Case 87
                    '缓坡
                    CX = (-0.5 + MapObj(DR, i).X / 160)
                    CY = (-0.5 + MapObj(DR, i).Y / 160)
                    If (MapObj(DR, i).Flag \ &H100000) Mod &H2 = 0 Then
                        '左斜
                        If GroundNode(DR, CX + 1, CY + 1) = 0 Then GroundNode(DR, CX + 1, CY + 1) = 1
                        If GroundNode(DR, CX + MapObj(DR, i).W, CY + MapObj(DR, i).H) = 0 Then GroundNode(DR, CX + MapObj(DR, i).W, CY + MapObj(DR, i).H) = 1
                        For j = 1 To MapObj(DR, i).W - 2 Step 2
                            GroundNode(DR, CX + j + 1, CY + (j \ 2) + 1) = 10
                            GroundNode(DR, CX + j + 1, CY + (j \ 2) + 2) = 9
                            GroundNode(DR, CX + j + 2, CY + (j \ 2) + 1) = 6
                            GroundNode(DR, CX + j + 2, CY + (j \ 2) + 2) = 13
                        Next
                    Else
                        '右斜
                        If GroundNode(DR, CX + 1, CY + MapObj(DR, i).H) = 0 Then GroundNode(DR, CX + 1, CY + MapObj(DR, i).H) = 1
                        If GroundNode(DR, CX + MapObj(DR, i).W, CY + 1) = 0 Then GroundNode(DR, CX + MapObj(DR, i).W, CY + 1) = 1
                        For j = 1 To MapObj(DR, i).W - 2 Step 2
                            GroundNode(DR, CX + j + 1, CY + MapObj(DR, i).H - (j \ 2) - 1) = 7
                            GroundNode(DR, CX + j + 1, CY + MapObj(DR, i).H - (j \ 2)) = 12
                            GroundNode(DR, CX + j + 2, CY + MapObj(DR, i).H - (j \ 2) - 1) = 11
                            GroundNode(DR, CX + j + 2, CY + MapObj(DR, i).H - (j \ 2)) = 8
                        Next
                    End If
                Case 88
                    '陡坡
                    CX = (-0.5 + MapObj(DR, i).X / 160)
                    CY = (-0.5 + MapObj(DR, i).Y / 160)
                    If (MapObj(DR, i).Flag \ &H100000) Mod &H2 = 0 Then
                        '左斜
                        If GroundNode(DR, CX + 1, CY + 1) = 0 Then GroundNode(DR, CX + 1, CY + 1) = 1
                        If GroundNode(DR, CX + MapObj(DR, i).W, CY + MapObj(DR, i).H) = 0 Then GroundNode(DR, CX + MapObj(DR, i).W, CY + MapObj(DR, i).H) = 1
                        For j = 1 To MapObj(DR, i).W - 2
                            GroundNode(DR, CX + j + 1, CY + j + 1) = 5
                            GroundNode(DR, CX + j + 1, CY + j) = 2
                        Next
                    Else
                        '右斜
                        If GroundNode(DR, CX + 1, CY + MapObj(DR, i).H) = 0 Then GroundNode(DR, CX + 1, CY + MapObj(DR, i).H) = 1
                        If GroundNode(DR, CX + MapObj(DR, i).W, CY + 1) = 0 Then GroundNode(DR, CX + MapObj(DR, i).W, CY + 1) = 1
                        For j = 1 To MapObj(DR, i).W - 2
                            GroundNode(DR, CX + j + 1, CY + MapObj(DR, i).W - j) = 4
                            GroundNode(DR, CX + j + 1, CY + MapObj(DR, i).W - j - 1) = 3
                        Next
                    End If
            End Select
        Next

        '计算斜坡填充
        For M = 0 To 2
            For i = 0 To W
                For j = 0 To H
                    Select Case GroundNode(DR, i + 1, j + 1)
                        Case 2 'UL
                            If GroundNode(DR, i + 2, j + 1) > 0 OrElse GroundNode(DR, i + 1, j) > 0 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 3 'UR
                            If GroundNode(DR, i, j + 1) > 0 OrElse GroundNode(DR, i + 1, j) > 0 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 4 'DL
                            If GroundNode(DR, i + 2, j + 1) > 0 OrElse GroundNode(DR, i + 1, j + 2) > 0 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 5 'DR
                            If GroundNode(DR, i, j + 1) > 0 OrElse GroundNode(DR, i + 1, j + 2) > 0 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 10 '下 右 右下 右2
                            If GroundNode(DR, i + 1, j) > 0 OrElse
                                GroundNode(DR, i + 2, j + 1) = 1 OrElse
                                GroundNode(DR, i + 2, j) = 1 OrElse
                                GroundNode(DR, i + 3, j + 1) = 1 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 11 '下 左 左下 左2
                            If GroundNode(DR, i + 1, j) > 0 OrElse
                                GroundNode(DR, i, j + 1) = 1 OrElse
                                GroundNode(DR, i, j) = 1 OrElse
                                GroundNode(DR, i - 1, j + 1) = 1 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 12 '上 右 右上 右2
                            If GroundNode(DR, i + 1, j + 2) > 0 OrElse
                                GroundNode(DR, i + 2, j + 1) = 1 OrElse
                                GroundNode(DR, i + 2, j + 2) = 1 OrElse
                                GroundNode(DR, i + 3, j + 1) = 1 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 13 '上 左 左上 左2
                            If GroundNode(DR, i + 1, j + 2) > 0 OrElse
                                GroundNode(DR, i, j + 1) = 1 OrElse
                                GroundNode(DR, i, j + 2) = 1 OrElse
                                GroundNode(DR, i - 1, j + 1) = 1 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 6
                            If GroundNode(DR, i, j + 1) = 1 OrElse
                                    GroundNode(DR, i + 2, j + 1) > 0 OrElse
                                    GroundNode(DR, i + 1, j) > 0 OrElse
                                    GroundNode(DR, i, j) > 0 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 7
                            If GroundNode(DR, i, j + 1) > 0 OrElse
                                    GroundNode(DR, i + 2, j + 1) = 1 OrElse
                                    GroundNode(DR, i + 1, j) > 0 OrElse
                                    GroundNode(DR, i + 2, j) > 0 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 8
                            If GroundNode(DR, i, j + 1) = 1 OrElse
                                    GroundNode(DR, i + 2, j + 1) > 0 OrElse
                                    GroundNode(DR, i + 1, j + 2) > 0 OrElse
                                    GroundNode(DR, i, j + 2) > 0 Then GroundNode(DR, i + 1, j + 1) = 1
                        Case 9
                            If GroundNode(DR, i, j + 1) > 0 OrElse
                                    GroundNode(DR, i + 2, j + 1) = 1 OrElse
                                    GroundNode(DR, i + 1, j + 2) > 0 OrElse
                                    GroundNode(DR, i + 2, j + 2) > 0 Then GroundNode(DR, i + 1, j + 1) = 1
                    End Select
                Next
            Next
        Next
    End Sub
    Public Sub DrawGrd(DR As Integer)
        Dim i As Integer
        Dim K As Image

        K = GetTile(0, 12, 1, 1) 'Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG")
        If DR = 0 Then
            '终点
            Select Case LH.GameStyle
                Case 12621 '1
                    If MH(DR).Theme = 2 Then
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27A.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY - 4) * PZoom, PZoom * 2, PZoom * 4)
                        For i = 0 To 13
                            G(G_GROUND).DrawImage(GetTile(15, 15, 1, 1), CSng((LH.GoalX / 10 - 14.5 + i) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY) * PZoom, PZoom, PZoom)
                        Next
                    Else
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY - 11) * PZoom, PZoom, PZoom * 11)
                    End If
                Case 13133 '3
                    If MH(DR).Theme = 2 Then
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27A.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY - 4) * PZoom, PZoom * 2, PZoom * 4)
                        For i = 0 To 13
                            G(G_GROUND).DrawImage(GetTile(15, 15, 1, 1), CSng((LH.GoalX / 10 - 14.5 + i) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY) * PZoom, PZoom, PZoom)
                        Next
                    Else
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY - 5) * PZoom, PZoom * 2, PZoom * 2)
                    End If
                Case 22349 'W
                    If MH(DR).Theme = 2 Then
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27A.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY - 4) * PZoom, PZoom * 2, PZoom * 4)
                        For i = 0 To 13
                            G(G_GROUND).DrawImage(GetTile(15, 15, 1, 1), CSng((LH.GoalX / 10 - 14.5 + i) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY) * PZoom, PZoom, PZoom)
                        Next
                    Else
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27F.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), CSng(MH(DR).BorT \ 16 - LH.GoalY - 8.5) * PZoom, PZoom, PZoom * 9)
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10) * PZoom), CSng(MH(DR).BorT \ 16 - LH.GoalY - 8) * PZoom, PZoom * 2, PZoom)
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27G.PNG"), CSng((LH.GoalX / 10 + 1.5) * PZoom), CSng(MH(DR).BorT \ 16 - LH.GoalY - 8.5) * PZoom, PZoom, PZoom * 9)
                    End If
                Case 21847 'U
                    If MH(DR).Theme = 2 Then
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27A.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY - 4) * PZoom, PZoom * 2, PZoom * 4)
                        For i = 0 To 13
                            G(G_GROUND).DrawImage(GetTile(15, 15, 1, 1, Int((LH.GoalX / 10 - 14.5 + i)), (MH(DR).BorT \ 16 - LH.GoalY)), CSng((LH.GoalX / 10 - 14.5 + i) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY) * PZoom, PZoom, PZoom)
                        Next
                    Else
                        G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY - 11) * PZoom, PZoom, PZoom * 11)
                    End If
                Case 22323 '3D
                    G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27.PNG"), CSng((LH.GoalX / 10 - 0.5) * PZoom), (MH(DR).BorT \ 16 - LH.GoalY - 11) * PZoom, PZoom, PZoom * 11)
            End Select

            G(G_GROUND).DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\38.PNG"), 1 * PZoom, (MH(DR).BorT \ 16 - LH.StartY - 3) * PZoom, PZoom * 3, PZoom * 3)
        End If

    End Sub
    Dim DrawIO As Integer = 0
    Public Sub DrawItem(DR As Integer, K As String, L As Boolean)
        'On Error Resume Next
        Dim i As Integer, j As Integer, j2 As Integer
        Dim H As Integer, W As Integer, PR As String
        Dim LX, LY, KX, KY As Integer
        Dim PP As Integer
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16
        Dim P As String = PT
        Dim GLayer = G_ITEM
        For i = 0 To MH(DR).ObjCount - 1
            PR = ""
            If InStr(K, "/" & MapObj(DR, i).ID.ToString & "/") > 0 Then

                If MapObj(DR, i).ID = 105 Then
                    If (MapObj(DR, i).Flag \ &H400) Mod 2 = 1 Then
                        KY = 0
                    Else
                        KY = -3 * PZoom
                    End If
                    ObjLinkType(DR, MapObj(DR, i).LID + 1) = 105

                    If (MapObj(DR, i).Flag \ &H80) Mod 2 = 1 Then
                        G(G_LINK).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\105A.PNG"),
                        CSng(-1.5 + MapObj(DR, i).X / 160) * PZoom, H * PZoom - CSng(0.5 + MapObj(DR, i).Y / 160) * PZoom + KY, PZoom * 3, PZoom * 5)
                    Else
                        G(G_LINK).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\105.PNG"),
                        CSng(-1.5 + MapObj(DR, i).X / 160) * PZoom, H * PZoom - CSng(0.5 + MapObj(DR, i).Y / 160) * PZoom + KY, PZoom * 3, PZoom * 5)
                    End If
                    'CID
                    If MapObj(DR, i).CID <> -1 Then
                        If (MapObj(DR, i).CFlag \ &H4) Mod 2 = 1 Then
                            G(G_LINK).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & "A.PNG"), 0.5), LX, LY + KY, PZoom, PZoom)
                        Else
                            G(G_LINK).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & ".PNG"), 0.5), LX, LY + KY, PZoom, PZoom)
                        End If
                    End If
                Else

                    Select Case ObjLinkType(DR, MapObj(DR, i).LID + 1)
                        Case 9 '管道L
                            KX = 0
                            KY = ((Math.Min(MapObj(DR, i).W, MapObj(DR, i).H) - 1) / 2) * PZoom
                        Case 105 '夹子L
                            KX = 0
                            KY = -PZoom / 4
                        Case 59 '轨道
                            KX = 0
                            KY = ((Math.Min(MapObj(DR, i).W, MapObj(DR, i).H) - 1) / 2) * PZoom
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

                    If MapObj(DR, i).LID + 1 = 0 AndAlso Not L OrElse MapObj(DR, i).LID + 1 > 0 AndAlso L OrElse MapObj(DR, i).ID = 9 Then
                        Select Case MapObj(DR, i).ID
                            Case 89 '卷轴相机
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\CMR\1.PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 14
                                '蘑菇平台
                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    j2 = 3
                                ElseIf (MapObj(DR, i).Flag \ &H80000) Mod 2 = 1 Then
                                    j2 = 4
                                Else
                                    j2 = 2
                                End If
                                For j = 0 To MapObj(DR, i).W - 1
                                    If j = 0 Then
                                        G(G_SEMI).DrawImage(GetTile(3, j2, 1, 1),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(DR, i).W - 1 Then
                                        G(G_SEMI).DrawImage(GetTile(5, j2, 1, 1),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G(G_SEMI).DrawImage(GetTile(4, j2, 1, 1),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                            Case 16
                                '半碰撞地形
                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    j2 = 10
                                ElseIf (MapObj(DR, i).Flag \ &H80000) Mod 2 = 1 Then
                                    j2 = 13
                                Else
                                    j2 = 7
                                End If
                                For j = 0 To MapObj(DR, i).W - 1
                                    If j = 0 Then
                                        G(G_SEMI).DrawImage(GetTile(j2, 3, 1, 1),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(DR, i).W - 1 Then
                                        G(G_SEMI).DrawImage(GetTile(j2 + 2, 3, 1, 1),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G(G_SEMI).DrawImage(GetTile(j2 + 1, 3, 1, 1),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                            Case 71
                                '3D半碰撞地形
                                Dim TL, TM, TR As String
                                For j2 = 0 To MapObj(DR, i).H - 1
                                    Select Case j2
                                        Case 0
                                            TL = "71"
                                            TM = "71A"
                                            TR = "71B"
                                        Case MapObj(DR, i).H - 1
                                            TL = "71F"
                                            TM = "71G"
                                            TR = "71H"
                                        Case Else
                                            TL = "71C"
                                            TM = "71D"
                                            TR = "71E"
                                    End Select
                                    For j = 0 To MapObj(DR, i).W - 1
                                        If j = 0 Then
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & TL & ".PNG"),
                                        CSng((j + MapObj(DR, i).X \ 160) * PZoom), (H + j2) * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                        ElseIf j = MapObj(DR, i).W - 1 Then
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & TR & ".PNG"),
                                        CSng((j + MapObj(DR, i).X \ 160) * PZoom), (H + j2) * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                        Else
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & TM & ".PNG"),
                                        CSng((j + MapObj(DR, i).X \ 160) * PZoom), (H + j2) * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                        End If
                                    Next
                                Next
                            Case 17
                                '桥
                                For j = 0 To MapObj(DR, i).W - 1
                                    If j = 0 Then
                                        G(G_SEMI).DrawImage(GetTile(0, 2, 1, 2),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    ElseIf j = MapObj(DR, i).W - 1 Then
                                        G(G_SEMI).DrawImage(GetTile(2, 2, 1, 2),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    Else
                                        G(G_SEMI).DrawImage(GetTile(1, 2, 1, 2),
                                            CSng((j + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    End If
                                Next
                            Case 113, 132
                                '蘑菇跳台 开关跳台
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    For j = 0 To MapObj(DR, i).W - 1
                                        If j = 0 Then
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & "D.PNG"),
                                                CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                        ElseIf j = MapObj(DR, i).W - 1 Then
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & "E.PNG"),
                                                CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                        Else
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & "C.PNG"),
                                                CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                        End If
                                    Next
                                Else
                                    For j = 0 To MapObj(DR, i).W - 1
                                        If j = 0 Then
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & "A.PNG"),
                                                CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                        ElseIf j = MapObj(DR, i).W - 1 Then
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & "B.PNG"),
                                                CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                        Else
                                            G(G_SEMI).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & ".PNG"),
                                                CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                        End If
                                    Next
                                End If

                            Case 66, 67, 90
                                '箭头 单向板 中间旗 
                                Select Case MapObj(DR, i).Flag
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
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H / 2 + MapObj(DR, i).Y / 160) * PZoom)
                                G(G_BITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                                CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 83 '狼牙棒
                                Select Case MapObj(DR, i).Flag
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
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H / 2 + MapObj(DR, i).Y / 160) * PZoom)
                                G(G_FITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"), 0.7),
                                                CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 64
                                '藤蔓
                                For j = 1 To MapObj(DR, i).H
                                    If j = 1 Then
                                        G(G_BITEM).DrawImage(GetTile(13, 7, 1, 1),
                                                CSng(-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom, H * PZoom - CSng((j + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(DR, i).H Then
                                        G(G_BITEM).DrawImage(GetTile(15, 7, 1, 1),
                                                CSng(-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom, H * PZoom - CSng((j + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G(G_BITEM).DrawImage(GetTile(14, 7, 1, 1),
                                                CSng(-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom, H * PZoom - CSng((j + MapObj(DR, i).Y \ 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                            Case 4, 5, 6, 21, 22, 23, 29, 63, 79, 99, 100, 43, 8
                                '4,4A 5 6 8 8A 21 22 23 23A 29 43 49 63 79 79A 92 99 100 100A
                                '砖 问号 硬砖 地面 竹轮 云砖 刺 金币
                                '音符  隐藏 
                                '冰砖  P砖 开关 开关砖

                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PP = 1
                                Else
                                    PP = 0
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_BLOCK).DrawImage(GetTile(TileLoc(MapObj(DR, i).ID, PP).X, TileLoc(MapObj(DR, i).ID, PP).Y, 1, 1),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                GLayer = G_BLOCK
                            Case 108
                                '闪烁砖
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_BLOCK).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 106 '树
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H + 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_BITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\106.PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * 4, PZoom * 4)
                                For j = 4 To MapObj(DR, i).H - 1
                                    G(G_BITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\106A.PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + 1.5 + MapObj(DR, i).X / 160) * PZoom),
                                    (H + j) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom, PZoom)
                                Next
                                G(G_BITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\106B.PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + 1 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((-0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * 2, PZoom)
                            Case 85, 119
                                '机动砖 轨道砖
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                DrawMoveBlock(DR, MapObj(DR, i).ID, MapObj(DR, i).Ex, MapObj(DR, i).X, MapObj(DR, i).Y)
                            Case 94
                                '斜传送带
                                Dim C1, C2 As Point
                                If (MapObj(DR, i).Flag \ &H400000) Mod 2 = 0 Then
                                    C1 = New Point(8, 0)
                                    C2 = New Point(4, 16)
                                Else
                                    C1 = New Point(13, 24)
                                    C2 = New Point(10, 22)
                                End If
                                If (MapObj(DR, i).Flag \ &H200000) Mod &H2 = 0 Then
                                    '左斜
                                    LX = CSng((-1 + MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                    LY = (H - 0.5 - MapObj(DR, i).H / 2) * PZoom - CSng((-0.5 + MapObj(DR, i).Y / 160) * PZoom)
                                    G(G_ITEM).DrawImage(GetTile(C1.X, C1.Y, 1, 1),
                                            CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom), (H - 1) * PZoom - CSng((-0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    G(G_ITEM).DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1),
                                            CSng((MapObj(DR, i).W - 1.5 + MapObj(DR, i).X / 160) * PZoom), (H - 1) * PZoom - CSng((MapObj(DR, i).H - 1.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    For j = 1 To MapObj(DR, i).W - 2
                                        G(G_ITEM).DrawImage(GetTile(C2.X + 1, C2.Y, 1, 2),
                                                CSng((j - 0.5 + MapObj(DR, i).X / 160) * PZoom), (H - 1) * PZoom - CSng((j - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    Next

                                Else
                                    '右斜
                                    LX = CSng((-1 + MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                    LY = (H - 0.5 - MapObj(DR, i).H / 2) * PZoom - CSng((-0.5 + MapObj(DR, i).Y / 160) * PZoom)
                                    G(G_ITEM).DrawImage(GetTile(C1.X, C1.Y, 1, 1),
                                            CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom), (H - 1) * PZoom - CSng((MapObj(DR, i).H - 1.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    G(G_ITEM).DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1),
                                            CSng((MapObj(DR, i).W - 1.5 + MapObj(DR, i).X / 160) * PZoom), (H - 1) * PZoom - CSng((-0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    For j = 1 To MapObj(DR, i).W - 2
                                        G(G_ITEM).DrawImage(GetTile(C2.X + 4, C2.Y, 1, 2),
                                                CSng((j - 0.5 + MapObj(DR, i).X / 160) * PZoom), (H - 1) * PZoom - CSng((-0.5 - j + MapObj(DR, i).H + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom * 2)
                                    Next
                                End If


                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 0 Then
                                    If (MapObj(DR, i).Flag \ &H8) Mod 2 = 1 Then
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\A1.PNG"), LX, LY, PZoom, PZoom)
                                    Else
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\A0.PNG"), LX, LY, PZoom, PZoom)
                                    End If
                                Else
                                    If (MapObj(DR, i).Flag \ &H8) Mod 2 = 1 Then
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\A3.PNG"), LX, LY, PZoom, PZoom)
                                    Else
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\A2.PNG"), LX, LY, PZoom, PZoom)
                                    End If
                                End If


                            Case 53
                                '传送带
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom)
                                Dim C1 As Point
                                If (MapObj(DR, i).Flag \ &H400000) Mod 2 = 0 Then
                                    C1 = New Point(8, 0)
                                Else
                                    C1 = New Point(13, 24)
                                End If

                                For j = 0 To MapObj(DR, i).W - 1
                                    If j = 0 Then
                                        G(G_ITEM).DrawImage(GetTile(C1.X, C1.Y, 1, 1),
                                        CSng((j - 0.5 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(DR, i).W - 1 Then
                                        G(G_ITEM).DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1),
                                        CSng((j - 0.5 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G(G_ITEM).DrawImage(GetTile(C1.X + 1, C1.Y, 1, 1),
                                        CSng((j - 0.5 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    End If

                                    If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 0 Then
                                        If (MapObj(DR, i).Flag \ &H8) Mod 2 = 1 Then
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\A1.PNG"), LX + CInt((-0.5 + MapObj(DR, i).W / 2) * PZoom), LY, PZoom, PZoom)
                                        Else
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\A0.PNG"), LX + CInt((-0.5 + MapObj(DR, i).W / 2) * PZoom), LY, PZoom, PZoom)
                                        End If
                                    Else
                                        If (MapObj(DR, i).Flag \ &H8) Mod 2 = 1 Then
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\A3.PNG"), LX + CInt((-0.5 + MapObj(DR, i).W / 2) * PZoom), LY, PZoom, PZoom)
                                        Else
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\A2.PNG"), LX + CInt((-0.5 + MapObj(DR, i).W / 2) * PZoom), LY, PZoom, PZoom)
                                        End If
                                    End If
                                Next
                            Case 9
                                '管道
                                ObjLinkType(DR, MapObj(DR, i).LID + 1) = 9
                                '0绿 4红 8蓝 C橙
                                PP = ((MapObj(DR, i).Flag \ &H10000) Mod &H10) \ 4
                                '00右 20左 40上 60下
                                '以相对左下角为准
                                Select Case MapObj(DR, i).Flag Mod &H80
                                    Case &H0 'R
                                        LX = CSng((MapObj(DR, i).H - 1 - 1 - 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                        LY = H * PZoom - CSng((MapObj(DR, i).Y / 160) * PZoom)
                                        For j = 0 To MapObj(DR, i).H - 2
                                            G(G_ITEM).DrawImage(GetTile(PipeLoc(PP, 4).X, PipeLoc(PP, 4).Y, 1, 2),
                                                            CSng((j - 0.5 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, 2 * PZoom)
                                        Next
                                        G(G_ITEM).DrawImage(GetTile(PipeLoc(PP, 3).X, PipeLoc(PP, 3).Y, 1, 2),
                                                        CSng((j - 0.5 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, 2 * PZoom)
                                    Case &H20 'L
                                        LX = CSng((-MapObj(DR, i).H + 1 + 1 - 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                        LY = H * PZoom - CSng((1 + MapObj(DR, i).Y / 160) * PZoom)
                                        For j = 0 To MapObj(DR, i).H - 2
                                            G(G_ITEM).DrawImage(GetTile(PipeLoc(PP, 4).X, PipeLoc(PP, 4).Y, 1, 2),
                                                            CSng((-j - 0.5 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, 2 * PZoom)
                                        Next
                                        G(G_ITEM).DrawImage(GetTile(PipeLoc(PP, 2).X, PipeLoc(PP, 2).Y, 1, 2),
                                                        CSng((-j - 0.5 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((1.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, 2 * PZoom)
                                    Case &H40 'U
                                        LX = CSng((+MapObj(DR, i).X / 160) * PZoom)
                                        LY = (H - MapObj(DR, i).H + 1 + 1) * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom)
                                        For j = 0 To MapObj(DR, i).H - 2
                                            G(G_ITEM).DrawImage(GetTile(PipeLoc(PP, 5).X, PipeLoc(PP, 5).Y, 2, 1),
                                                            CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom), (H - j) * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), 2 * PZoom, PZoom)
                                        Next
                                        G(G_ITEM).DrawImage(GetTile(PipeLoc(PP, 0).X, PipeLoc(PP, 0).Y, 2, 1),
                                                        CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom), (H - j) * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), 2 * PZoom, PZoom)
                                    Case &H60 'D
                                        LX = CSng((-1 + MapObj(DR, i).X / 160) * PZoom)
                                        LY = (H + MapObj(DR, i).H - 1 - 1) * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom)
                                        For j = 0 To MapObj(DR, i).H - 2
                                            G(G_ITEM).DrawImage(GetTile(PipeLoc(PP, 5).X, PipeLoc(PP, 5).Y, 2, 1),
                                                            CSng((-1.5 + MapObj(DR, i).X / 160) * PZoom), (H + j) * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), 2 * PZoom, PZoom)
                                        Next
                                        G(G_ITEM).DrawImage(GetTile(PipeLoc(PP, 1).X, PipeLoc(PP, 1).Y, 2, 1),
                                                        CSng((-1.5 + MapObj(DR, i).X / 160) * PZoom), (H + j) * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom), 2 * PZoom, PZoom)
                                End Select
                                PR = ((MapObj(DR, i).Flag Mod &H1000000) \ &H100000 - 1).ToString
                                If PR <> "-1" Then
                                    G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\C" & PR & ".PNG"), LX, LY, PZoom, PZoom)
                                End If

                            Case 55
                                '门
                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                ElseIf (MapObj(DR, i).Flag \ &H80000) Mod 2 = 1 Then
                                    PR = "B"
                                Else
                                    PR = ""
                                End If
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\55" & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                PR = ((MapObj(DR, i).Flag Mod &H800000) \ &H200000).ToString
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\C" & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    (H + 1) * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                            Case 97
                                '传送箱
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\97" & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                PR = ((MapObj(DR, i).Flag Mod &H800000) \ &H200000).ToString
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\CMN\C" & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                            Case 84
                                '蛇
                                For j = 0 To MapObj(DR, i).W - 1
                                    If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\84A.PNG"),
                                                CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\84.PNG"),
                                                CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                                '&H10方向
                                DrawSnake(DR, MapObj(DR, i).Ex, MapObj(DR, i).X, MapObj(DR, i).Y, MapObj(DR, i).W, MapObj(DR, i).H)
                            Case 68, 82
                                '齿轮 甜甜圈
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H - 1.5) * PZoom - CSng((MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_FITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & ".PNG"), 0.7),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)

                            Case 0, 10, 15, 19, 20, 35,
                                 48, 56, 57, 60, 76, 92, 95, 102, 72, 50, 51, 65, 80, 114, 119,
                                  77, 104, 120, 121, 122, 123, 124, 125, 126, 112, 127, 128, 129, 130, 131,
                                    96, 117, 86
                                '板栗  金币 弹簧 炸弹 P POW 蘑菇 
                                ' 无敌星 鱿鱼 鱼
                                '黑花 火球  风  红币 钥匙  地鼠 慢慢龟汽车 跳跳怪 跳跳鼠 蜜蜂 冲刺砖块 尖刺鱼 !方块
                                '奔奔  太阳 库巴七人 木箱 纸糊道具
                                '蚂蚁 斗斗 乌卡
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 33
                                ' 1UP 
                                If MH(DR).Theme = 0 And MH(DR).Flag = 2 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)

                            Case 74
                                '加邦 
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    If MH(DR).Theme = 6 Then
                                        PR = "B"
                                    Else
                                        PR = "A"
                                    End If
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 42
                                '飞机
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 OrElse (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + MapObj(DR, i).H - 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    (H + MapObj(DR, i).H - 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * 2, PZoom * 2)
                            Case 34
                                '火花 
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                        PR = "C"
                                    Else
                                        PR = "A"
                                    End If
                                Else
                                    If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                        PR = "B"
                                    Else
                                        PR = ""
                                    End If
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 81, 116
                                'USA  锤子
                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 44
                                '大蘑菇
                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 12
                                '咚咚
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + MapObj(DR, i).H / 2 - 0.5) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_FITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\12.PNG"), 0.7),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                If MapObj(DR, i).LID = -1 Then
                                    Select Case MapObj(DR, i).Flag Mod &H100
                                        Case &H40, &H42, &H44
                                            G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\E1.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H48, &H4A, &H4C
                                            G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\E2.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H50, &H52, &H54
                                            G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\E0.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H58, &H5A, &H5C
                                            G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\E3.PNG"), LX, LY, PZoom, PZoom)
                                    End Select
                                End If
                                GLayer = G_FITEM
                            Case 41
                                '幽灵
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                Select Case LH.GameStyle
                                    Case 22323
                                        If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41D.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Else
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        End If
                                    Case Else
                                        If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41A.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        ElseIf (MapObj(DR, i).Flag \ &H1000000) Mod &H8 = &H4 Then
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41C.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        ElseIf (MapObj(DR, i).Flag \ &H100) Mod 2 = 1 Then
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41B.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Else
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\41.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        End If
                                End Select
                            Case 28, 25, 18
                                '钢盔 刺龟 P
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & "A.PNG"),
                                        CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                ElseIf (MapObj(DR, i).Flag \ &H1000000) Mod 8 = &H6 Then
                                    G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & ".PNG"),
                                        CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                Else
                                    G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & "B.PNG"),
                                        CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                End If
                            Case 40
                                '小刺龟
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + MapObj(DR, i).W) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    Select Case (MapObj(DR, i).Flag \ &H1000000) Mod 8
                                    '方向6上 4下 0左 2右
                                        Case &H0 'L
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40B0.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Case &H2 'R
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40B2.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Case &H4 'D
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40B4.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Case &H6 'U
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40B6.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    End Select
                                Else
                                    Select Case (MapObj(DR, i).Flag \ &H1000000) Mod 8
                                    '方向6上 4下 0左 2右
                                        Case &H0 'L
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40A0.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Case &H2 'R
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40A2.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Case &H4 'D
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40A4.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Case &H6 'U
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\40A6.PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    End Select
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                            Case 2
                                '绿花
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "2B"
                                Else
                                    PR = "2A"
                                End If
                                Select Case (MapObj(DR, i).Flag \ &H1000000) Mod &H8
                            '方向6上 4下 0左 2右
                                    Case &H0 'L
                                        LX = CSng((MapObj(DR, i).H / 2 - 1 + MapObj(DR, i).X / 160) * PZoom)
                                        LY = (H + MapObj(DR, i).W + (MapObj(DR, i).W \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & PR & "0.PNG"),
                                        CSng((-MapObj(DR, i).W * 3 / 2 + MapObj(DR, i).X / 160) * PZoom),
                                        (H + MapObj(DR, i).W) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(DR, i).W * 2, PZoom * MapObj(DR, i).H)
                                    Case &H2 'R
                                        LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                        LY = (H + MapObj(DR, i).W + (MapObj(DR, i).W \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & PR & "2.PNG"),
                                        CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                        (H + MapObj(DR, i).W) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(DR, i).W * 2, PZoom * MapObj(DR, i).H)
                                    Case &H4 'D
                                        LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                        LY = (H + MapObj(DR, i).W) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & PR & "4.PNG"),
                                        CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                        (H + MapObj(DR, i).W) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H * 2)
                                    Case &H6 'U
                                        LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                        LY = (H + MapObj(DR, i).H + (MapObj(DR, i).W \ 2)) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & PR & "6.PNG"),
                                        CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                        H * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H * 2)
                                End Select
                            Case 107
                                '长长吞食花
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "E"
                                Else
                                    PR = ""
                                End If
                                Select Case MapObj(DR, i).Flag \ &H1000000
                                    Case &H0
                                        PR += "C"
                                    Case &H2
                                        PR += "A"
                                    Case &H4
                                        PR += "B"
                                    Case &H6
                                        PR += ""
                                End Select
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\107" & PR & ".PNG"), LX, LY, PZoom * 2, PZoom * 2)
                                DrawCrp(DR, MapObj(DR, i).Ex, MapObj(DR, i).X, MapObj(DR, i).Y)
                            Case 32
                                '大炮弹
                                Select Case MapObj(DR, i).Flag
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
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\32" & PR & ".PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 1, 46, 52, 58
                                '慢慢龟，碎碎龟，花花，扳手
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H * 2)
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                            Case 30
                                '裁判
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 1 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\30.PNG"), LX, LY, PZoom, PZoom * 2)
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\31.PNG"), LX - PZoom \ 2, LY + PZoom \ 2, PZoom * 2, PZoom)
                            Case 31
                                '裁判云
                                ObjLinkType(DR, MapObj(DR, i).LID + 1) = 31
                                LX = CSng((-MapObj(DR, i).W / 2 - 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom)
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\31.PNG"), LX, LY, PZoom * 2, PZoom)
                            Case 45 '鞋 耀西
                                Select Case LH.GameStyle
                                    Case 21847, 22349 'U W
                                        If MapObj(DR, i).W = 2 Then
                                            PR = "A"
                                        Else
                                            PR = ""
                                        End If
                                    Case Else
                                        If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                            PR = "A"
                                        Else
                                            PR = ""
                                        End If
                                End Select
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"), LX, LY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H * 2)
                                LX = CSng((-MapObj(DR, i).W / 2 + (MapObj(DR, i).W \ 2) / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + (MapObj(DR, i).H \ 2) / 2) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                            Case 62
                                '库巴
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                Select Case LH.GameStyle
                                    Case 22323
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & "A.PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    Case Else
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & ".PNG"),
                                    CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                End Select
                            Case 3
                                '德莱文
                                Select Case LH.GameStyle
                                    Case 22323
                                        If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                            LX = CSng((-MapObj(DR, i).W / 2 + 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                            LY = H * PZoom - CSng((1 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\3B.PNG"),
                                            CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                            (H) * PZoom - CSng((1.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Else
                                            LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                            LY = H * PZoom - CSng((MapObj(DR, i).H * 2 - 1.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\3.PNG"),
                                            CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                            (H + 2) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        End If
                                    Case Else
                                        If (MapObj(DR, i).Flag \ &H4000) Mod 2 = 1 Then
                                            LX = CSng((-MapObj(DR, i).W / 2 + 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                            LY = H * PZoom - CSng((1 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\3A.PNG"),
                                            CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                            H * PZoom - CSng((1.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                        Else
                                            LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                            LY = H * PZoom - CSng((MapObj(DR, i).H * 2 - 1.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                            G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\3.PNG"),
                                            CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                            H * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(DR, i).W, 2 * PZoom * MapObj(DR, i).H)
                                        End If
                                End Select
                            Case 13
                                '炮台
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "B"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_FITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\13" & PR & ".PNG"), 0.7), LX, LY, PZoom * MapObj(DR, i).W, PZoom * 2)
                                For j = 2 To MapObj(DR, i).H - 1
                                    If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                        G(G_FITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\13C.PNG"), 0.7), LX, LY + j * PZoom, PZoom, PZoom)
                                    Else
                                        G(G_FITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\13A.PNG"), 0.7), LX, LY + j * PZoom, PZoom, PZoom)
                                    End If
                                Next
                                GLayer = G_FITEM
                            Case 39
                                '魔法师
                                LX = CSng((2 - MapObj(DR, i).W / 2 - MapObj(DR, i).W + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + 1) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\39.PNG"), LX - PZoom - PZoom, LY - PZoom + KY, 2 * PZoom * MapObj(DR, i).W + KY, 2 * PZoom * MapObj(DR, i).H)
                            Case 47
                                '小炮
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "E"
                                Else
                                    PR = ""
                                End If
                                Dim ANG As Single, D As String
                                Select Case MapObj(DR, i).Flag \ &H100000
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
                                Select Case MapObj(DR, i).Flag \ &H100000
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
                                LX = CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom)
                                G(G_ITEM).TranslateTransform(LX + MapObj(DR, i).W * PZoom \ 2, LY + MapObj(DR, i).H * PZoom \ 2)
                                G(G_ITEM).RotateTransform(ANG)
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\47" & PR & ".PNG"),
                                    -MapObj(DR, i).W * PZoom \ 2,
                                    -MapObj(DR, i).H * PZoom \ 2, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                G(G_ITEM).ResetTransform()
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\47" & PR & D & ".PNG"),
                                CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 61
                                '汪汪
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 0 Then
                                    G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\61A.PNG"),
                                            CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom),
                                            H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                            PZoom, PZoom)
                                End If
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\61.PNG"),
                                        CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                        H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                        PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 78
                                '仙人掌
                                LX = CSng(-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom
                                LY = (H + 1) * PZoom - CSng((MapObj(DR, i).H + MapObj(DR, i).Y \ 160) * PZoom) + KY
                                For j = 0 To MapObj(DR, i).H - 1
                                    If j = MapObj(DR, i).H - 1 Then
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\78.PNG"),
                                                CSng(-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom,
                                                (H - 1) * PZoom - CSng((j + MapObj(DR, i).Y \ 160) * PZoom) + KY,
                                                PZoom, PZoom)
                                    Else
                                        G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\78A.PNG"),
                                                CSng(-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom,
                                                (H - 1) * PZoom - CSng((j + MapObj(DR, i).Y \ 160) * PZoom) + KY,
                                                PZoom, PZoom)
                                    End If
                                Next
                            Case 111
                                '机械库巴
                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "B"
                                ElseIf (MapObj(DR, i).Flag \ &H80000) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W + 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + 1) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\111" & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    2 * PZoom * MapObj(DR, i).W, 2 * PZoom * MapObj(DR, i).H)
                            Case 70
                                '大金币 
                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                ElseIf (MapObj(DR, i).Flag \ &H80000) Mod 2 = 1 Then
                                    PR = "B"
                                Else
                                    PR = ""
                                End If
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                            CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom),
                                            H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                LX = CSng((-MapObj(DR, i).W / 2 + 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 1 + MapObj(DR, i).Y / 160) * PZoom) + KY
                            Case 110
                                '刺方块
                                If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                                    PR = "A"
                                ElseIf (MapObj(DR, i).Flag \ &H80000) Mod 2 = 1 Then
                                    PR = "B"
                                Else
                                    PR = ""
                                End If
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\" & MapObj(DR, i).ID.ToString & PR & ".PNG"),
                                            CSng((-MapObj(DR, i).W / 2 + 0.5 + MapObj(DR, i).X / 160) * PZoom),
                                            H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                            PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                            Case 98
                                '小库巴
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                LX = CSng((-MapObj(DR, i).W + 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = (H + 1) * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\98" & PR & ".PNG"),
                                    CSng((-MapObj(DR, i).W + MapObj(DR, i).X / 160) * PZoom),
                                    H * PZoom - CSng((MapObj(DR, i).H * 2 - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY,
                                    2 * PZoom * MapObj(DR, i).W, 2 * PZoom * MapObj(DR, i).H)
                            Case 103
                                '骨鱼
                                LX = CSng((-MapObj(DR, i).W + 0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                G(G_ITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\103.PNG"), CSng((-MapObj(DR, i).W + MapObj(DR, i).X / 160) * PZoom) + KY, LY, 2 * PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)

                            Case 91
                                '跷跷板
                                For j = 0 To MapObj(DR, i).W - 1
                                    If j = 0 Then
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\91A.PNG"),
                                                CSng((j - MapObj(DR, i).W \ 2 + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    ElseIf j = MapObj(DR, i).W - 1 Then
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\91B.PNG"),
                                                CSng((j - MapObj(DR, i).W \ 2 + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    Else
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\91.PNG"),
                                                CSng((j - MapObj(DR, i).W \ 2 + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                    End If
                                Next
                                G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\91C.PNG"),
                                        CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom, PZoom)
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom)
                            Case 36
                                '熔岩台
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                                    PR = "A"
                                Else
                                    PR = ""
                                End If
                                If MapObj(DR, i).LID <> -1 Then
                                    MapObj(DR, i).W = 1
                                End If
                                For j = 0 To MapObj(DR, i).W - 1
                                    G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\36" & PR & ".PNG"),
                                        CSng((j - MapObj(DR, i).W \ 2 + MapObj(DR, i).X \ 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom, PZoom)
                                Next
                                LX = CSng((j - 1 - MapObj(DR, i).W \ 2 + MapObj(DR, i).X \ 160) * PZoom)
                                LY = H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                            Case 11
                                '升降台
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY

                                For j = 0 To MapObj(DR, i).W - 1
                                    If (MapObj(DR, i).Flag \ &H4) Mod 2 = 0 Then
                                        If j = 0 Then
                                            PR = "A"
                                        ElseIf j = MapObj(DR, i).W - 1 Then
                                            PR = "B"
                                        Else
                                            PR = ""
                                        End If
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\11" & PR & ".PNG"),
                                            CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom, PZoom)
                                    Else
                                        If j = 0 Then
                                            PR = "D"
                                        ElseIf j = MapObj(DR, i).W - 1 Then
                                            PR = "E"
                                        Else
                                            PR = "C"
                                        End If
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\11" & PR & ".PNG"),
                                            CSng((j - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom, PZoom)
                                    End If
                                Next
                                If (MapObj(DR, i).Flag \ &H4) Mod 2 = 0 Then
                                    Select Case MapObj(DR, i).Flag Mod &H100
                                        Case &H40
                                            G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\D1.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H48
                                            G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\D2.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H50
                                            G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\D0.PNG"), LX, LY, PZoom, PZoom)
                                        Case &H58
                                            G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\D3.PNG"), LX, LY, PZoom, PZoom)
                                    End Select
                                End If
                                GLayer = G_FITEM
                            Case 54
                                '喷枪
                                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                                LY = H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY
                                Select Case MapObj(DR, i).Flag Mod &H100
                                    Case &H40
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54.PNG"), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    Case &H48
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A.PNG"), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    Case &H50
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54B.PNG"), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    Case &H58
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54C.PNG"), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    Case &H44
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54.PNG"), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    Case &H4C
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A.PNG"), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    Case &H54
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54B.PNG"), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                    Case &H5C
                                        G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54C.PNG"), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom) + KY, PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                                End Select
                            Case 24
                                '火棍
                                LX = CSng(-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom
                                LY = H * PZoom - CSng(MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom
                                G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\24.PNG"),
                                LX, LY, PZoom, PZoom)
                            Case 105
                                '夹子
                                If MapObj(DR, i).Flag Mod &H400 >= &H100 Then
                                    KY = PZoom * 3
                                    ObjLinkType(DR, MapObj(DR, i).LID + 1) = 105
                                Else
                                    KY = 0
                                    ObjLinkType(DR, MapObj(DR, i).LID + 1) = 105
                                End If
                                LX = CSng(-1.5 + MapObj(DR, i).X / 160) * PZoom
                                LY = H * PZoom - CSng(3.5 + MapObj(DR, i).Y / 160) * PZoom + KY

                                If (MapObj(DR, i).Flag \ &H80) Mod 2 = 1 Then
                                    G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\105A.PNG"), LX, LY, PZoom * 3, PZoom * 5)
                                Else
                                    G(G_FITEM).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\105.PNG"), LX, LY, PZoom * 3, PZoom * 5)
                                End If
                                GLayer = G_FITEM
                        End Select

                        PR = ""
                        'PR += IIf((MapObj(DR,i).Flag \ &H4000) Mod 2 = 1, "M", "")
                        PR += IIf((MapObj(DR, i).Flag \ &H8000) Mod 2 = 1, "P", "")
                        PR += IIf((MapObj(DR, i).Flag \ 2) Mod 2 = 1, "W", "")
                        If PR = "PW" Then PR = "B"
                        If PR.Length > 0 Then
                            G(GLayer).DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & PR & ".PNG"), LX, LY, PZoom \ 2, PZoom \ 2)
                        End If

                        'Obj音效
                        'Dim dx, dy As Integer
                        'If MapObj(DR, i).SID > 0 Then
                        '    Select Case MapObj(DR, i).SID
                        '        Case Is < 10
                        '            dy = 5
                        '            dx = MapObj(DR, i).SID
                        '        Case Is < 20
                        '            dy = 6
                        '            dx = MapObj(DR, i).SID - 10
                        '        Case Is < 27
                        '            dy = 7
                        '            dx = MapObj(DR, i).SID - 20
                        '        Case Is < 36
                        '            dy = 8
                        '            dx = MapObj(DR, i).SID - 27
                        '        Case Else
                        '            dy = 9
                        '            dx = MapObj(DR, i).SID - 36
                        '    End Select
                        '    G(G_SND).DrawImage(SndTile, New Rectangle(LX, LY, PZoom, PZoom),
                        '                New Rectangle(dx * 128, dy * 128, 128, 128), GraphicsUnit.Pixel)
                        '    Debug.Print(LX.ToString & "," & LY.ToString)
                        'End If


                        If L And ObjLinkType(DR, MapObj(DR, i).LID + 1) = 59 Then
                            PR = ((MapObj(DR, i).Flag Mod &H400000) \ &H100000).ToString
                            G(G_LINK).DrawImage(Image.FromFile(P & "\img\CMN\D" & PR & ".PNG"), LX, LY, PZoom, PZoom)
                        End If

                    End If
                End If
            End If
        Next


    End Sub
    Public Sub DrawCID(DR As Integer)
        Dim i As Integer
        Dim H As Integer, W As Integer, PR As String
        Dim LX, LY As Integer

        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16
        Dim P As String = PT
        Dim OP As Double = 0.75
        For i = 0 To MH(DR).ObjCount - 1
            LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
            LY = H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom)
            Select Case MapObj(DR, i).CID
                Case -1'无

                Case 44, 81, 116 '状态
                    If (MapObj(DR, i).CFlag \ &H40000) Mod 2 = 1 Then
                        G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & "A.PNG"), OP), LX, LY, PZoom, PZoom)
                    Else
                        G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & ".PNG"), OP), LX, LY, PZoom, PZoom)
                    End If
                    G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\CMN\F1.PNG"), OP), LX, LY, PZoom, PZoom)
                Case 34 '状态火花
                    If (MapObj(DR, i).CFlag \ &H4) Mod 2 = 1 Then
                        If (MapObj(DR, i).CFlag \ &H40000) Mod 2 = 1 Then
                            G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & "C.PNG"), OP), LX, LY, PZoom, PZoom)
                        Else
                            G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & "A.PNG"), OP), LX, LY, PZoom, PZoom)
                        End If
                    Else
                        If (MapObj(DR, i).CFlag \ &H40000) Mod 2 = 1 Then
                            G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & "B.PNG"), OP), LX, LY, PZoom, PZoom)
                        Else
                            G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & ".PNG"), OP), LX, LY, PZoom, PZoom)
                        End If
                    End If
                    G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\CMN\F1.PNG"), OP), LX, LY, PZoom, PZoom)
                Case 111 '机械库巴
                    If (MapObj(DR, i).CFlag \ &H40000) Mod 2 = 1 Then
                        PR = "B"
                    ElseIf (MapObj(DR, i).CFlag \ &H80000) Mod 2 = 1 Then
                        PR = "A"
                    Else
                        PR = ""
                    End If
                    G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & PR & ".PNG"), OP), LX, LY, PZoom, PZoom)
                    G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\CMN\F1.PNG"), OP), LX, LY, PZoom, PZoom)
                Case 76 '加邦
                    If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                        If MH(DR).Theme = 6 Then
                            PR = "B"
                        Else
                            PR = "A"
                        End If
                    Else
                        PR = ""
                    End If
                    G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & PR & ".PNG"), OP), LX, LY, PZoom, PZoom)
                    G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\CMN\F1.PNG"), OP), LX, LY, PZoom, PZoom)
                Case 33 '1UP
                    If MH(DR).Theme = 1 And MH(DR).Flag = 2 Then
                        G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & "A.PNG"), OP), LX, LY, PZoom, PZoom)
                    Else
                        G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & ".PNG"), OP), LX, LY, PZoom, PZoom)
                    End If
                    G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\CMN\F1.PNG"), OP), LX, LY, PZoom, PZoom)

                Case Else
                    If (MapObj(DR, i).CFlag \ &H4) Mod 2 = 1 Then
                        G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & "A.PNG"), OP), LX, LY, PZoom, PZoom)
                    Else
                        G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\CID\" & MapObj(DR, i).CID.ToString & ".PNG"), OP), LX, LY, PZoom, PZoom)
                    End If
                    G(G_HIDE).DrawImage(SetOpacity(Image.FromFile(P & "\img\CMN\F1.PNG"), OP), LX, LY, PZoom, PZoom)
            End Select



        Next
    End Sub
    Public Sub DrawFireBar(DR As Integer)
        Dim i, j, LX, LY As Integer
        Dim P As String = PT
        Dim H As Integer, W As Integer
        Dim FR As Single
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16
        ''火棍
        ''长度&H40 0000，角度EX/&H38E 38E0
        For i = 0 To MH(DR).ObjCount - 1
            If MapObj(DR, i).ID = 24 Then
                LX = CSng(-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom
                LY = H * PZoom - CSng(MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom
                FR = MapObj(DR, i).Ex / &H38E38E0
                G(G_FITEM).TranslateTransform(LX + PZoom \ 2, LY + PZoom \ 2)
                G(G_FITEM).RotateTransform(-FR * 5)
                For j = 0 To (MapObj(DR, i).Flag - &H6000000) / &H400000 + 1
                    G(G_FITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\24A.PNG"), 0.5), -PZoom \ 4 + j * PZoom, -PZoom \ 4, PZoom, PZoom \ 2)
                Next
                G(G_FITEM).ResetTransform()
                If (MapObj(DR, i).Flag \ &H8) Mod 2 = 1 Then
                    G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\B0.PNG"), LX, LY, PZoom, PZoom)
                Else
                    G(G_FITEM).DrawImage(Image.FromFile(P & "\img\CMN\B1.PNG"), LX, LY, PZoom, PZoom)
                End If
            End If
        Next

    End Sub
    Public Sub DrawFire(DR As Integer)
        Dim i, LX, LY As Integer
        Dim P As String = PT
        Dim H As Integer, W As Integer
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16

        For i = 0 To MH(DR).ObjCount - 1
            If MapObj(DR, i).ID = 54 Then
                LX = CSng((-0.5 + MapObj(DR, i).X / 160) * PZoom)
                LY = H * PZoom - CSng((0.5 + MapObj(DR, i).Y / 160) * PZoom)
                Select Case MapObj(DR, i).Flag Mod &H100
                    Case &H40
                        G(G_ITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A1.PNG"), 0.5), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), (H - 3) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, 3 * PZoom * MapObj(DR, i).H)
                    Case &H48
                        G(G_ITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A3.PNG"), 0.5), CSng((-MapObj(DR, i).W / 2 + 1 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), 3 * PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                    Case &H50
                        G(G_ITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A5.PNG"), 0.5), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), (H + 1) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, 3 * PZoom * MapObj(DR, i).H)
                    Case &H58
                        G(G_ITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A7.PNG"), 0.5), CSng((-MapObj(DR, i).W / 2 - 3 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), 3 * PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                    Case &H44
                        G(G_ITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A2.PNG"), 0.5), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), (H - 3) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, 3 * PZoom * MapObj(DR, i).H)
                    Case &H4C
                        G(G_ITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A4.PNG"), 0.5), CSng((-MapObj(DR, i).W / 2 + 1 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), 3 * PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                    Case &H54
                        G(G_ITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A6.PNG"), 0.5), CSng((-MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160) * PZoom), (H + 1) * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), PZoom * MapObj(DR, i).W, 3 * PZoom * MapObj(DR, i).H)
                    Case &H5C
                        G(G_ITEM).DrawImage(SetOpacity(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A8.PNG"), 0.5), CSng((-MapObj(DR, i).W / 2 - 3 + MapObj(DR, i).X / 160) * PZoom), H * PZoom - CSng((MapObj(DR, i).H - 0.5 + MapObj(DR, i).Y / 160) * PZoom), 3 * PZoom * MapObj(DR, i).W, PZoom * MapObj(DR, i).H)
                End Select
            End If
        Next

    End Sub
    Dim SndTile As Bitmap
    Public Sub DrawSnd(Dr As Integer)
        Dim i As Integer
        Dim dx, dy As Integer
        For i = 0 To MH(Dr).SndCount - 1
            Select Case MapSnd(Dr, i).ID
                Case Is < 10
                    dy = 5
                    dx = MapSnd(Dr, i).ID
                Case Is < 20
                    dy = 6
                    dx = MapSnd(Dr, i).ID - 10
                Case Is < 27
                    dy = 7
                    dx = MapSnd(Dr, i).ID - 20
                Case Is < 36
                    dy = 8
                    dx = MapSnd(Dr, i).ID - 27
                Case Else
                    dy = 9
                    dx = MapSnd(Dr, i).ID - 36
            End Select
            G(G_SND).DrawImage(SndTile, New Rectangle(MapSnd(Dr, i).X * PZoom, (MH(Dr).BorT \ 16 - MapSnd(Dr, i).Y - 1) * PZoom, PZoom, PZoom),
                                        New Rectangle(dx * 128, dy * 128, 128, 128), GraphicsUnit.Pixel)
            Debug.Print(MapSnd(Dr, i).X.ToString & "," & MapSnd(Dr, i).Y.ToString)
        Next
    End Sub
    Public Sub DrawObject(Dr As Integer)

        '3D平台
        '半碰撞
        '蘑菇平台
        '桥 
        '蘑菇跳台 
        '开关跳台
        DrawItem(Dr, "/132/", False)
        DrawItem(Dr, "/16/", False)
        DrawItem(Dr, "/14/", False)
        DrawItem(Dr, "/17/", False)
        DrawItem(Dr, "/113/", False)
        DrawItem(Dr, "/71/", False)

        '箭头 单向板 中间旗 藤蔓 
        DrawItem(Dr, "/66/67/106/", False)
        DrawItem(Dr, "/64/", False)
        DrawItem(Dr, "/90/", False)

        '树 长长吞食花
        DrawItem(Dr, "/106/107/", False)

        '地面 
        '计算斜坡填充
        SetSlopeGrdCode(Dr)
        '绘制地面
        DrawGrd(Dr)
        DrawGrdCode(Dr)

        '传送带 开关 开关砖 P砖 冰锥
        DrawItem(Dr, "/53/94/99/100/79/", False)
        DrawIce(Dr)

        '无LINKE
        '管道 门 蛇 传送箱
        DrawItem(Dr, "/9/55/84/97/", False)
        '机动砖 轨道砖
        DrawItem(Dr, "/85/119/", False)
        '夹子
        DrawItem(Dr, "/105/", False)
        '轨道
        DrawTrack(Dr)
        '软砖 问号 硬砖 竹轮 云 音符 隐藏 刺 冰块 闪烁砖 
        DrawItem(Dr, "/4/5/6/21/22/23/29/43/63/110/108/", False)

        '跷跷板 熔岩台 升降台 
        DrawItem(Dr, "/91/36/11/", False)

        '狼牙棒
        DrawItem(Dr, "/83/", False)

        '齿轮 甜甜圈
        DrawItem(Dr, "/68/82/", False)

        '道具
        DrawItem(Dr, "/0/1/2/3/8/10/12/13/15/18/19/20/25/28/30/31/32/33/34/35/39/", False)
        DrawItem(Dr, "/40/41/42/44/45/46/47/48/52/56/57/58/60/61/62/70/74/76/77/78/81/92/95/98/102/103/104/", False)
        DrawItem(Dr, "/111/120/121/122/123/124/125/126/112/127/128/129/130/131/72/50/51/65/80/114/116/", False)
        DrawItem(Dr, "/96/117/86/", False)
        '喷枪 火棍
        DrawItem(Dr, "/24/54/", False)
        '夹子
        DrawItem(Dr, "/105/", False)
        '轨道
        DrawTrack(Dr)
        '夹子
        DrawItem(Dr, "/105/", True)
        '卷轴相机
        'DrawItem("/89/", False)

        'LINK
        '软砖 问号 硬砖 竹轮 云 音符 隐藏 刺 冰块
        DrawItem(Dr, "/4/5/6/21/22/23/29/43/63/", True)


        '跷跷板 熔岩台 升降台
        DrawItem(Dr, "/91/36/11/", True)

        '齿轮 甜甜圈
        DrawItem(Dr, "/68/82/", True)

        '道具
        DrawItem(Dr, "/0/1/2/3/8/10/12/13/15/18/19/20/25/28/30/31/32/33/34/35/39/", True)
        DrawItem(Dr, "/40/41/42/44/45/46/47/48/52/56/57/58/60/61/62/70/74/76/77/78/81/92/95/98/102/103/104/", True)
        DrawItem(Dr, "/111/120/121/122/123/124/125/126/112/127/128/129/130/131/72/50/51/65/80/114/116/", True)
        DrawItem(Dr, "/96/117/86/", True)

        DrawCID(Dr)

        '喷枪 火棍
        DrawItem(Dr, "/24/54/", True)
        DrawFireBar(Dr)
        DrawFire(Dr)

        '透明管
        DrawCPipe(Dr)

        If isLoadDeathData Then
            DrawDeath(Dr)
        End If

        'DrawSnd(Dr)
    End Sub
    Public Sub DrawDeath(Dr As Integer)
        Dim i As Integer
        Dim D = SetOpacity(Image.FromFile(PT & "\img\cmn\k0.png"), 0.5)
        For i = 0 To MapDeath.Length - 1
            If Dr = IIf(MapDeath(i).Map, 1, 0) Then
                G(G_DEATH).DrawImage(D, CInt(MapDeath(i).x * PZoom / 16), CInt((MH(Dr).BorT - MapDeath(i).y - 16) * PZoom / 16), PZoom, PZoom)
            End If
        Next
    End Sub
    Public Sub DrawCPipe(DR As Integer)
        Dim i As Integer, J As Integer, K As Integer
        Dim H As Integer, W As Integer, CP As String
        H = MH(DR).BorT \ 16
        W = MH(DR).BorR \ 16

        For i = 0 To MH(DR).ClearPipCount - 1
            For J = 0 To MapCPipe(DR, i).NodeCount - 1
                Select Case J
                    Case 0
                        For K = 0 To MapCPipe(DR, i).Node(J).H - 1
                            Select Case MapCPipe(DR, i).Node(J).Dir
                                Case 0 'R
                                    If K = 0 Then
                                        CP = "C"
                                    ElseIf MapCPipe(DR, i).NodeCount = 1 And K = MapCPipe(DR, i).Node(J).H - 1 Then
                                        CP = "E"
                                    Else
                                        CP = "D"
                                    End If
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(DR, i).Node(J).X + K) * PZoom,
                                                    (H - 1 - MapCPipe(DR, i).Node(J).Y) * PZoom,
                                                    PZoom, 2 * PZoom)
                                Case 1 'L
                                    If K = 0 Then
                                        CP = "E"
                                    ElseIf MapCPipe(DR, i).NodeCount = 1 And K = MapCPipe(DR, i).Node(J).H - 1 Then
                                        CP = "C"
                                    Else
                                        CP = "D"
                                    End If
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(DR, i).Node(J).X - K) * PZoom,
                                                    (H - 1 - MapCPipe(DR, i).Node(J).Y - 1) * PZoom,
                                                     PZoom, 2 * PZoom)
                                Case 2 'U
                                    If K = 0 Then
                                        CP = ""
                                    ElseIf MapCPipe(DR, i).NodeCount = 1 And K = MapCPipe(DR, i).Node(J).H - 1 Then
                                        CP = "B"
                                    Else
                                        CP = "A"
                                    End If
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    MapCPipe(DR, i).Node(J).X * PZoom,
                                                    (H - 1 - MapCPipe(DR, i).Node(J).Y - K) * PZoom,
                                                    2 * PZoom, PZoom)
                                Case 3 'D
                                    If K = 0 Then
                                        CP = "B"
                                    ElseIf MapCPipe(DR, i).NodeCount = 1 And K = MapCPipe(DR, i).Node(J).H - 1 Then
                                        CP = ""
                                    Else
                                        CP = "A"
                                    End If
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(DR, i).Node(J).X - 1) * PZoom,
                                                    (H - 1 - MapCPipe(DR, i).Node(J).Y + K) * PZoom,
                                                    2 * PZoom, PZoom)
                            End Select
                        Next
                    Case MapCPipe(DR, i).NodeCount - 1
                        For K = 0 To MapCPipe(DR, i).Node(J).H - 1
                            Select Case MapCPipe(DR, i).Node(J).Dir
                                Case 0 'R
                                    CP = IIf(K = MapCPipe(DR, i).Node(J).H - 1, "E", "D")
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(DR, i).Node(J).X + K) * PZoom,
                                                    (H - 1 - MapCPipe(DR, i).Node(J).Y) * PZoom,
                                                    PZoom, 2 * PZoom)
                                Case 1 'L
                                    CP = IIf(K = MapCPipe(DR, i).Node(J).H - 1, "C", "D")
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(DR, i).Node(J).X - K) * PZoom,
                                                    (H - 1 - MapCPipe(DR, i).Node(J).Y - 1) * PZoom,
                                                     PZoom, 2 * PZoom)
                                Case 2 'U
                                    CP = IIf(K = MapCPipe(DR, i).Node(J).H - 1, "B", "A")
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    MapCPipe(DR, i).Node(J).X * PZoom,
                                                    (H - 1 - MapCPipe(DR, i).Node(J).Y - K) * PZoom,
                                                    2 * PZoom, PZoom)
                                Case 3 'D
                                    CP = IIf(K = MapCPipe(DR, i).Node(J).H - 1, "", "A")
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93" & CP & ".PNG"),
                                                    (MapCPipe(DR, i).Node(J).X - 1) * PZoom,
                                                    (H - 1 - MapCPipe(DR, i).Node(J).Y + K) * PZoom,
                                                    2 * PZoom, PZoom)
                            End Select
                        Next
                    Case Else
                        If MapCPipe(DR, i).Node(J).type >= 3 And MapCPipe(DR, i).Node(J).type <= 10 Then
                            Select Case MapCPipe(DR, i).Node(J).type
                                Case 3, 7 'RU DL
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93G.PNG"),
                                                        (MapCPipe(DR, i).Node(J).X) * PZoom,
                                                        (H - 2 - MapCPipe(DR, i).Node(J).Y) * PZoom,
                                                        2 * PZoom, 2 * PZoom)
                                Case 4, 9 'RD UL
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93H.PNG"),
                                                        (MapCPipe(DR, i).Node(J).X) * PZoom,
                                                        (H - 2 - MapCPipe(DR, i).Node(J).Y) * PZoom,
                                                        2 * PZoom, 2 * PZoom)
                                Case 6, 10 'UR LD
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93J.PNG"),
                                                        (MapCPipe(DR, i).Node(J).X) * PZoom,
                                                        (H - 2 - MapCPipe(DR, i).Node(J).Y) * PZoom,
                                                        2 * PZoom, 2 * PZoom)
                                Case 5, 8 'DR LU
                                    G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93F.PNG"),
                                                        (MapCPipe(DR, i).Node(J).X) * PZoom,
                                                        (H - 2 - MapCPipe(DR, i).Node(J).Y) * PZoom,
                                                        2 * PZoom, 2 * PZoom)
                            End Select
                            'G.DrawString(MapCPipe(DR,i).Node(J).type.ToString, Me.Font, Brushes.Black, (MapCPipe(DR,i).Node(J).X) * PZOOM, (H - 2 - MapCPipe(DR,i).Node(J).Y) * PZOOM)
                        Else
                            For K = 0 To MapCPipe(DR, i).Node(J).H - 1
                                Select Case MapCPipe(DR, i).Node(J).Dir
                                    Case 0 'R
                                        G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93D.PNG"),
                                                        (MapCPipe(DR, i).Node(J).X + K) * PZoom,
                                                        (H - 1 - MapCPipe(DR, i).Node(J).Y) * PZoom,
                                                        PZoom, 2 * PZoom)
                                    Case 1 'L
                                        G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93D.PNG"),
                                                        (MapCPipe(DR, i).Node(J).X - K) * PZoom,
                                                        (H - 1 - MapCPipe(DR, i).Node(J).Y - 1) * PZoom,
                                                         PZoom, 2 * PZoom)
                                    Case 2 'U
                                        G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93A.PNG"),
                                                        MapCPipe(DR, i).Node(J).X * PZoom,
                                                        (H - 1 - MapCPipe(DR, i).Node(J).Y - K) * PZoom,
                                                        2 * PZoom, PZoom)
                                    Case 3 'D
                                        G(G_ITEM).DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\OBJ\93A.PNG"),
                                                        (MapCPipe(DR, i).Node(J).X - 1) * PZoom,
                                                        (H - 1 - MapCPipe(DR, i).Node(J).Y + K) * PZoom,
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
        MapA.Image.Save(PT & "\" & TCode.Text & "-A.PNG", Imaging.ImageFormat.Png)
        MapB.Image.Save(PT & "\" & TCode.Text & "-B.PNG", Imaging.ImageFormat.Png)
        MsgBox("已保存地图至" & PT & "\" & TCode.Text)
    End Sub
    Private Sub SaveGif(n As String, delayMs As Integer)
        Application.DoEvents()
        Dim fileList() As String = System.IO.Directory.GetFiles(PT & "\img\temp")
        Dim c As Integer = fileList.Length
        Dim gif As AnimatedGif.AnimatedGifCreator = AnimatedGif.AnimatedGif.Create(PT & "\img\" & n & ".gif", delayMs)
        For i = 0 To 5
            gif.AddFrame(Image.FromFile(PT & "\img\temp\" & i.ToString & ".png"), -1, AnimatedGif.GifQuality.Bit8)
        Next
        For i = 4 To 1 Step -1
            gif.AddFrame(Image.FromFile(PT & "\img\temp\" & i.ToString & ".png"), -1, AnimatedGif.GifQuality.Bit8)
        Next
        gif.Dispose()
    End Sub

    Private Sub RefPic()
        DrawIO = 0
        LoadEFile()
        MapWidth(0) = MH(0).BorR \ 16
        MapHeight(0) = MH(0).BorT \ 16
        Dim DN As String = IIf(MH(0).Flag = 2, "A", "")
        Dim i, j As Integer
        If LH.GameStyle = 22323 Then
            For i = 0 To 3
                For j = 0 To 3
                    Tile3DW(i, j) = Image.FromFile(PT & "\img\TILE\22323-" & MH(0).Theme.ToString & i.ToString & j.ToString & ".png")
                Next
            Next
            TileW = Tile3DW(0, 0).Width \ 16
        Else
            Tile = Image.FromFile(PT & "\img\TILE\" & LH.GameStyle & "-" & MH(0).Theme.ToString & DN & ".png")
            TileW = Tile.Width \ 16
        End If
        InitPng()
        InitTrackImg()
        DrawObject(DrawIO)
        RefList(0, ListBox1)

        '显示GIF=========================================
        BMAPR(0) = New Bitmap(BSIZE(0).Width, BSIZE(0).Height)
        Dim GG = Graphics.FromImage(BMAPR(0))
        'GG.DrawImage(BMAP(0, 0), 0, 0)
        'BMAPR(0).Save(PT & "\img\temp\0.png", Imaging.ImageFormat.Png)
        'For i = 1 To 5
        '    GG.DrawImage(SetOpacity(BMAP(0, 1), i / 5), 0, 0)
        '    BMAPR(0).Save(PT & "\img\temp\" & i.ToString & ".png", Imaging.ImageFormat.Png)
        'Next
        'SaveGif("0", 50)
        'MapA.Image = Image.FromFile(PT & "\img\0.gif")
        '================================================
        For i = 0 To BMAP.GetLength(1) - 1
            If Draw_CK(i).Checked Then
                GG.DrawImage(BMAP(0, i), 0, 0)
            End If
            BMAP(0, i).Dispose()
        Next
        MapA.Image = BMAPR(0)

        'ObjInfo()
        DrawIO = 1
        MapWidth(1) = MH(1).BorR \ 16
        MapHeight(1) = MH(1).BorT \ 16
        DN = IIf(MH(1).Flag = 2, "A", "")
        If LH.GameStyle = 22323 Then
            For i = 0 To 3
                For j = 0 To 3
                    Tile3DW(i, j) = Image.FromFile(PT & "\img\TILE\22323-" & MH(1).Theme.ToString & i.ToString & j.ToString & ".png")
                Next
            Next
            TileW = Tile3DW(0, 0).Width \ 16
        Else
            Tile = Image.FromFile(PT & "\img\TILE\" & LH.GameStyle & "-" & MH(1).Theme.ToString & DN & ".png")
            TileW = Tile.Width \ 16
        End If
        InitPng2()
        InitTrackImg()
        DrawObject(DrawIO)
        RefList(1, ListBox2)

        BMAPR(1) = New Bitmap(BSIZE(1).Width, BSIZE(1).Height)
        GG = Graphics.FromImage(BMAPR(1))
        For i = 0 To BMAP.GetLength(1) - 1
            If Draw_CK(i).Checked Then
                GG.DrawImage(BMAP(1, i), 0, 0)
            End If
            G(i).Dispose()
            BMAP(1, i).Dispose()
        Next
        MapB.Image = BMAPR(1)

        'ObjInfo()
        'GetLvlInfo()
    End Sub
    Private Declare Function DoFileDownload Lib "shdocvw.dll" (ByVal lpszFile As String) As Integer
    Private Declare Function URLDownloadToFile Lib "urlmon" Alias "URLDownloadToFileA" _
        (ByVal pCaller As Integer, ByVal szURL As String, ByVal szFileName As String, ByVal dwReserved As Integer, ByVal lpfnCB As Integer) As Integer
    Dim isMapIO As Boolean = True
    Dim isLoadDeathData As Boolean = False
    Sub DownloadDeathInfo()
        isLoadDeathData = False
        Dim i As Integer
        TCode.Text = TCode.Text.Replace("-", "")
        TCode.Text = TCode.Text.Replace(" ", "")
        TCode.Text = UCase(Strings.Left(TCode.Text, 9))
        'Label6.Text = DateTime.Now.ToString & " 校验图号"
        '检查图号
        Dim LInfo As String = ""
        i = Code2Num(TCode.Text, LInfo)
        Select Case i
            Case 0 '关卡ID
                'Label6.Text += vbCrLf & LInfo
            Case 1 '工匠ID
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 此ID为工匠ID"
                'Label6.Text += vbCrLf & LInfo
                Exit Sub
            Case Else '错误ID
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 校验失败，图号输入有误"
                'Label6.Text += vbCrLf & LInfo
                Exit Sub
        End Select
        Dim json = GetPage("https://" & CApi.Text & "/mm2/level_deaths/" & TCode.Text)
        Dim J As JObject, K As JToken
        If json.Length > 0 Then
            'MapDeath
            J = JObject.Parse(json)
            K = J.SelectToken("deaths")
            ReDim MapDeath(K.Children.Count - 1)
            For i = 0 To K.Children.Count - 1
                With MapDeath(i)
                    .x = CInt(K.Children.ElementAt(i).SelectToken("x"))
                    .y = CInt(K.Children.ElementAt(i).SelectToken("y"))
                    .Map = LCase(K.Children.ElementAt(i).SelectToken("is_subworld").ToString) = "true"
                End With
                Debug.Print(MapDeath(i).x.ToString & "," & MapDeath(i).y.ToString & "," & MapDeath(i).Map.ToString & K.Children.ElementAt(i).SelectToken("is_subworld").ToString)
            Next
        End If
        isLoadDeathData = True
    End Sub
    Sub DownloadLvlInfo()
        Dim i As Integer
        TCode.Text = TCode.Text.Replace("-", "")
        TCode.Text = TCode.Text.Replace(" ", "")
        TCode.Text = UCase(Strings.Left(TCode.Text, 9))
        'Label6.Text = DateTime.Now.ToString & " 校验图号"
        '检查图号
        Dim LInfo As String = ""
        i = Code2Num(TCode.Text, LInfo)
        Select Case i
            Case 0 '关卡ID
                'Label6.Text += vbCrLf & LInfo
            Case 1 '工匠ID
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 此ID为工匠ID"
                'Label6.Text += vbCrLf & LInfo
                Exit Sub
            Case Else '错误ID
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 校验失败，图号输入有误"
                'Label6.Text += vbCrLf & LInfo
                Exit Sub
        End Select

        Dim json = GetPage("https://" & CApi.Text & "/mm2/level_info/" & TCode.Text)
        Dim J As JObject, K As JToken
        If json.Length > 0 Then
            J = JObject.Parse(json)
            TTag.Text = J.SelectToken("tags_name").ToString
            TMTime.Text = J.SelectToken("upload_time_pretty").ToString
            If J.SelectToken("world_record_pretty") IsNot Nothing Then
                TWTime.Text = J.SelectToken("world_record_pretty").ToString
                K = J.SelectToken("first_completer")
                TFMaker.Text = K.SelectToken("name").ToString
                TFMaker.Hint = "首通工匠 " & K.SelectToken("code").ToString
                K = J.SelectToken("record_holder")
                TWMaker.Text = K.SelectToken("name").ToString
                TWMaker.Hint = "最速工匠 " & K.SelectToken("code").ToString
            End If

            'TFTime.Text = J.SelectToken("world_record_pretty").ToString
            If J.SelectToken("clear_condition").ToString <> "0" Then
                TCR.Text = J.SelectToken("clear_condition_name").ToString.Replace(
                "(n)", "(" & J.SelectToken("clear_condition_magnitude").ToString & ")")
            Else
                TCR.Text = "无"
            End If
            TClear.Text = J.SelectToken("clears").ToString
            TAttempt.Text = J.SelectToken("attempts").ToString
            TCRate.Text = J.SelectToken("clear_rate_pretty").ToString
            TPlay1.Text = J.SelectToken("plays").ToString
            TPlay2.Text = J.SelectToken("versus_matches").ToString
            TPlay3.Text = J.SelectToken("coop_matches").ToString
            TLike.Text = J.SelectToken("likes").ToString
            TBoo.Text = J.SelectToken("boos").ToString
            TComm.Text = J.SelectToken("num_comments").ToString
            TDiff.Text = DiffStr(CInt(J.SelectToken("difficulty").ToString))

            K = J.SelectToken("uploader")
            TMaker.Text = K.SelectToken("name").ToString
            TMaker.Hint = "工匠 " & K.SelectToken("code").ToString

        End If
    End Sub
    Function GetJson(J As JObject, K As String) As String
        Return J.SelectToken(K).ToString
    End Function
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles BtnLoadLvl.Click
        BtnLoadLvl.Enabled = False
        Dim I As Integer
        Dim F As FileInfo
        TCode.Text = TCode.Text.Replace("-", "")
        TCode.Text = TCode.Text.Replace(" ", "")
        TCode.Text = UCase(Strings.Left(TCode.Text, 9))

        'Label6.Text = DateTime.Now.ToString & " 校验图号"
        '检查图号
        Dim LInfo As String = ""
        I = Code2Num(TCode.Text, LInfo)
        Select Case I
            Case 0 '关卡ID
                TTitle.Text = "下载地图"
            Case 1 '工匠ID
                TTitle.Text = "输入的ID为工匠ID"
                BtnLoadLvl.Enabled = True
                Exit Sub
            Case Else '错误ID
                TTitle.Text = "输入的ID有误"
                BtnLoadLvl.Enabled = True
                Exit Sub
        End Select

        '====================
        If SLvlInfo.Checked Then
            '加载地图信息
            DownloadLvlInfo()
        End If
        isLoadDeathData = False
        If SLvlDeath.Checked Then
            '加载死亡信息
            DownloadDeathInfo()
        End If
        '====================
        If IO.File.Exists(PT & "\decrypt_data\" & TCode.Text & ".DEC") Then
            F = New FileInfo(PT & "\decrypt_data\" & TCode.Text & ".DEC")
            If F.Length = 376768 Then
                I = 2
            Else
                I = 0
            End If
        ElseIf IO.File.Exists(PT & "\download_data\" & TCode.Text & ".BCD") Then
            F = New FileInfo(PT & "\download_data\" & TCode.Text & ".BCD")
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
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 从服务器下载地图"
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " /mm2/level_data/" & TCode.Text
                Application.DoEvents()
                '下载文件
                Try
                    Dim wc = New WebClient
                    wc.DownloadFile("https://" & MaterialComboBox1.Text & "/mm2/level_data/" & TCode.Text, PT & "\download_data\" & TCode.Text & ".BCD")
                    Do
                        Application.DoEvents()
                        If IO.File.Exists(PT & "\download_data\" & TCode.Text & ".BCD") Then
                            F = New FileInfo(PT & "\download_data\" & TCode.Text & ".BCD")
                            If F.Length = 376832 Then
                                DeMap2(TCode.Text & ".BCD", TCode.Text & ".DEC")
                                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 解析地图"
                                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 已加载地图"
                                TextBox1.Text = TCode.Text & ".DEC"
                                isMapIO = True
                                RefPic()
                                BtnLoadLvl.Enabled = True
                                Exit Sub
                            End If
                        End If
                        I += 1
                        Threading.Thread.Sleep(1000)
                    Loop Until I > 30
                    'Label6.Text += vbCrLf & DateTime.Now.ToString & " 下载地图超时"
                Catch ex As Exception
                    'Label6.Text += vbCrLf & DateTime.Now.ToString & " " & ex.Message
                End Try

            Case 1 '存在BCD地图文件
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 解析地图"
                DeMap2(TCode.Text & ".BCD", TCode.Text & ".DEC")
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 已从BCD文件加载地图"
                TextBox1.Text = TCode.Text & ".DEC"
                isMapIO = True
                RefPic()
                BtnLoadLvl.Enabled = True
                Exit Sub
            Case 2 '存在DEC文档，直接读取
                'Label6.Text += vbCrLf & DateTime.Now.ToString & " 已从DEC文件加载地图"
                TextBox1.Text = TCode.Text & ".DEC"
                isMapIO = True
                BtnLoadLvl.Enabled = True
                RefPic()
                Exit Sub
        End Select

        BtnLoadLvl.Enabled = True
    End Sub
    Sub DeMap2(InF As String, OutF As String) '解密BCD地图文件
        Dim k() = File.ReadAllBytes(PT & "\download_data\" & InF)
        Dim r() = Decrypt(k)
        File.WriteAllBytes(PT & "\decrypt_data\" & OutF, r)
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        PZoom = 2 ^ TrackBar1.Value
        TxtZoom.Text = "×" & PZoom.ToString
    End Sub

    Private Function SetObjInfo(DR As Integer, idx As Integer, flag As Integer) As ObjStr
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
                    If MH(DR).Theme = 6 Then
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
                If MH(DR).Theme = 6 Then
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
                If MH(DR).Theme = 0 And MH(DR).Flag = 2 Then
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

    Private Sub RefList(DR As Integer, L As ListBox)
        Dim i As Integer, P As Integer
        Dim PB As String
        Dim J1, J2 As ObjStr
        Dim Offset, ADDR As Integer
        Offset = IIf(DR = 0, &H200, &H2E0E0)
        For i = 0 To MH(DR).ObjCount - 1
            If MapObj(DR, i).CID = -1 Then
                'Offset +&H48 + &H0 + M * &H20
                ADDR = Offset + &H48 + &H0 + i * &H20
                L.Items.Add(Hex(ADDR) & " " & GetItemName(MapObj(DR, i).ID, LH.GameStyle).ToString & " " & (0.5 + MapObj(DR, i).X / 160).ToString & "," & (0.5 + MapObj(DR, i).Y / 160).ToString)
            Else
                'Offset +&H48 + &H0 + M * &H20
                ADDR = Offset + &H48 + &H0 + i * &H20
                L.Items.Add(Hex(ADDR) & " " & GetItemName(MapObj(DR, i).ID, LH.GameStyle).ToString & "(" & GetItemName(MapObj(DR, i).CID, LH.GameStyle) & ") " & (0.5 + MapObj(DR, i).X / 160).ToString & "," & (0.5 + MapObj(DR, i).Y / 160).ToString)
            End If

            Select Case MapObj(DR, i).ID
                Case 9 '管道PIPE
                Case 93 '透明管道CLEAR PIPE
                Case 14, 16, 71 '平台
                    For W = 1 To MapObj(DR, i).W
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H + MapObj(DR, i).Y / 160)).Obj += MapObj(DR, i).ID.ToString & ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H + MapObj(DR, i).Y / 160)).Flag += ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H + MapObj(DR, i).Y / 160)).SubObj += ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H + MapObj(DR, i).Y / 160)).SubFlag += ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H + MapObj(DR, i).Y / 160)).State += ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H + MapObj(DR, i).Y / 160)).SubState += ","
                    Next
                Case 17 '桥
                    For W = 1 To MapObj(DR, i).W
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H - 1 + MapObj(DR, i).Y / 160)).Obj += MapObj(DR, i).ID.ToString & ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H - 1 + MapObj(DR, i).Y / 160)).Flag += ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H - 1 + MapObj(DR, i).Y / 160)).SubObj += ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H - 1 + MapObj(DR, i).Y / 160)).SubFlag += ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H - 1 + MapObj(DR, i).Y / 160)).State += ","
                        ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(MapObj(DR, i).H - 1 + MapObj(DR, i).Y / 160)).SubState += ","
                    Next
                Case 87, 88 '斜坡 SLOPE
                Case 53, 94 '传送带
                Case 105 '夹子
                Case 97 '传送箱
                    P = ((MapObj(DR, i).Flag Mod &H800000) \ &H200000)
                    If (MapObj(DR, i).Flag \ &H4) Mod 2 = 1 Then
                        PB = "A"
                    Else
                        PB = ""
                    End If
                    For W = 1 To MapObj(DR, i).W
                        For H = 1 To MapObj(DR, i).H
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Obj += MapObj(DR, i).ID.ToString & PB & ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Flag += Strings.Mid("ABCDEFGHJ", P + 1, 1) & ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubObj += ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubFlag += ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).State += ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubState += ","
                        Next
                    Next
                Case 55 '门 
                    P = ((MapObj(DR, i).Flag Mod &H800000) \ &H200000)
                    If (MapObj(DR, i).Flag \ &H40000) Mod 2 = 1 Then
                        PB = "A"
                    ElseIf (MapObj(DR, i).Flag \ &H80000) Mod 2 = 1 Then
                        PB = "B"
                    Else
                        PB = ""
                    End If
                    For W = 1 To MapObj(DR, i).W
                        For H = 1 To MapObj(DR, i).H
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Obj += MapObj(DR, i).ID.ToString & PB & ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Flag += Strings.Mid("ABCDEFGHJ", P + 1, 1) & ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubObj += ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubFlag += ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).State += ","
                            ObjLocData(DR, Int(W - 0.5 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubState += ","
                        Next
                    Next
                Case 34, 44, 81 '花 弹力球 USA 大蘑菇
                    J1 = SetObjInfo(DR, MapObj(DR, i).ID, MapObj(DR, i).Flag)
                    For W = 1 To MapObj(DR, i).W
                        For H = 1 To MapObj(DR, i).H
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Obj += J1.Obj & ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Flag += J1.Flag & ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).State += J1.State & ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubObj += ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubFlag += ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubState += ","
                        Next
                    Next
                Case 110
                    J1 = SetObjInfo(DR, MapObj(DR, i).ID, MapObj(DR, i).Flag)
                    J2 = SetObjInfo(DR, MapObj(DR, i).CID, MapObj(DR, i).CFlag)
                    For W = 1 To MapObj(DR, i).W
                        For H = 1 To MapObj(DR, i).H
                            ObjLocData(DR, Int(W + 1 - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Obj += J1.Obj & ","
                            ObjLocData(DR, Int(W + 1 - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Flag += J1.Flag & ","
                            ObjLocData(DR, Int(W + 1 - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).State += J1.State & ","
                            ObjLocData(DR, Int(W + 1 - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubObj += J2.Obj & ","
                            ObjLocData(DR, Int(W + 1 - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubFlag += J2.Flag & ","
                            ObjLocData(DR, Int(W + 1 - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubState += J2.State & ","
                        Next
                    Next
                Case Else
                    J1 = SetObjInfo(DR, MapObj(DR, i).ID, MapObj(DR, i).Flag)
                    J2 = SetObjInfo(DR, MapObj(DR, i).CID, MapObj(DR, i).CFlag)
                    For W = 1 To MapObj(DR, i).W
                        For H = 1 To MapObj(DR, i).H
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Obj += J1.Obj & ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).Flag += J1.Flag & ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).State += J1.State & ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubObj += J2.Obj & ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubFlag += J2.Flag & ","
                            ObjLocData(DR, Int(W - MapObj(DR, i).W / 2 + MapObj(DR, i).X / 160), Int(H + MapObj(DR, i).Y / 160)).SubState += J2.State & ","
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
    Public Sub ObjInfo(DR As Integer)

        Dim i As Integer, M As Integer
        Dim s As String = "==OBJ==" & vbCrLf
        'OBJ参数详情
        s += "ID" & vbTab & "ID" & vbTab & "X" & vbTab & "Y" & vbTab & "FLAG" & vbTab & "CID" & vbTab & "CFLAG" & vbTab & "HEX" & vbTab &
            "LID" & vbTab & "SID" & vbTab & "W" & vbTab & "H" & vbCrLf

        For i = 0 To MH(DR).ObjCount - 1
            s += ObjEng(MapObj(DR, i).ID) & vbTab & MapObj(DR, i).ID & vbTab & MapObj(DR, i).X & vbTab & MapObj(DR, i).Y & vbTab
            s += Hex(MapObj(DR, i).Flag) & vbTab & MapObj(DR, i).CID & vbTab & Hex(MapObj(DR, i).CFlag) & vbTab & Hex(MapObj(DR, i).Ex) & vbTab
            s += MapObj(DR, i).LID & vbTab & MapObj(DR, i).SID & vbTab & MapObj(DR, i).W & vbTab & MapObj(DR, i).H & vbCrLf

        Next

        s += "==TRACK==" & vbCrLf
        s += "UN" & vbTab & "X" & vbTab & "Y" & vbTab & "HFLAG" & vbTab & "TYPE" & vbTab & "LID" & vbTab & "HK0,HK1" & vbCrLf
        For i = 0 To MH(DR).TrackCount - 1
            s += MapTrk(DR, i).UN & vbTab & MapTrk(DR, i).X & vbTab & MapTrk(DR, i).Y & vbTab & Hex(MapTrk(DR, i).Flag) & vbTab &
            MapTrk(DR, i).Type & vbTab & MapTrk(DR, i).LID & vbTab & Hex(MapTrk(DR, i).K0) & vbTab & Hex(MapTrk(DR, i).K1) & vbCrLf
        Next


        s += "==CPIPE==" & vbCrLf
        For M = 0 To MH(DR).ClearPipCount - 1
            s += "INDEX" & vbTab & "NC" & vbTab & "N" & vbTab & "HFLAG" & vbTab & "TYPE" & vbTab & "HLID" & vbTab & "HK0,HK1" & vbCrLf
            s += MapCPipe(DR, M).Index & vbTab & MapCPipe(DR, M).NodeCount & vbCrLf
            s += "TYPE" & vbTab & "INDEX" & vbTab & "X" & vbTab & "Y" & vbTab & "W" & vbTab & "H" & vbTab & "DIR" & vbCrLf
            For i = 0 To MapCPipe(DR, M).NodeCount - 1
                s += MapCPipe(DR, M).Node(i).type & vbTab & MapCPipe(DR, M).Node(i).index & vbTab & MapCPipe(DR, M).Node(i).X & vbTab & MapCPipe(DR, M).Node(i).Y & vbTab &
                MapCPipe(DR, M).Node(i).W & vbTab & MapCPipe(DR, M).Node(i).H & vbTab & MapCPipe(DR, M).Node(i).Dir & vbCrLf
            Next
        Next
        TextBox4.Text += s


    End Sub
    Private Sub PicMap2_MouseUp(sender As Object, e As MouseEventArgs)
        MapMove2 = False
    End Sub

    Public Function GetPage(ByVal url As String) As String
        On Error GoTo Err
        Application.DoEvents()
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
    Dim Draw_CK() As MaterialCheckbox
    Dim Draw_CKStr() As String = {"背景", "水面", "网格", "平台", "背景道具",
        "地面砖块", "标记", "前景砖块", "道具", "隐藏道具",
        "前景道具", "轨道", "链接道具", "音效"}
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PT = Application.StartupPath
        Dim i, j, k As Integer
        UR2 = ""
        For i = 1 To UR.Length Step 6
            j = Int(Mid(UR, i, 3))
            k = Int(Mid(UR, i + 3, 3))
            UR2 += Chr(j Xor k)
        Next
        ReDim Draw_CK(BMAP.GetLength(1) - 1)
        For i = 0 To BMAP.GetLength(1) - 1
            Draw_CK(i) = New MaterialCheckbox
            Draw_CK(i).Text = Draw_CKStr(i)
            Draw_CK(i).Checked = True
            CList.Items.Add(Draw_CK(i))
        Next
        G2 = Graphics.FromImage(B2)
        G0 = Graphics.FromImage(B0)
        Me.AllowDrop = True
        InitLocData()

        MiiB(0) = New Bitmap(128, 128)
        MiiB(1) = New Bitmap(128, 128)
        MiiB(2) = New Bitmap(128, 128)
        MiiG(0) = Graphics.FromImage(MiiB(0))
        MiiG(1) = Graphics.FromImage(MiiB(1))
        MiiG(2) = Graphics.FromImage(MiiB(2))
        SndTile = Bitmap.FromFile(PT & "\img\snd\SndTile.png")

    End Sub
    Public Sub InitLocData()
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
        'Dim f As String
        'Dim R, B As Bitmap
        'For Each f In files
        '    R = Magnifier(Bitmap.FromFile(f), 4)
        '    R.Save(f.Replace("img-copy", "img"))
        'Next
        If File.Exists(files(0)) Then
            Dim F = New FileInfo(files(0))
            If UCase(F.Extension) = ".BCD" Then
                'Label6.Text = F.FullName
                'Label6.Text += vbCrLf & PT & "\download_data\" & F.Name
                File.Copy(F.FullName, PT & "\download_data\" & F.Name)
                DeMap2(F.Name, UCase(F.Name).Replace(".BCD", ".DEC"))
                TextBox1.Text = UCase(F.Name).Replace(".BCD", ".DEC")
                RefPic()
            ElseIf UCase(F.Extension) = ".DEC" Then
                'Label6.Text = F.FullName
                'Label6.Text += vbCrLf & PT & "\decrypt_data\" & F.Name
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
        'Label2.Text = ""
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        TCode.Text = TCode.Text.Replace("-", "")
        If TCode.Text.Length = 9 Then
            S = GetPage("http://" & CApi.Text & "/mm2/level_info/" & TCode.Text)
            TextBox2.Text = S
            If S = "" OrElse InStr(S, "Code corresponds to a maker") > 0 Then Exit Sub
            JSON = JObject.Parse(S)
            'Label2.Text += "标题:" & JSON.SelectToken("name").ToString & vbCrLf
            'Label2.Text += "描述:" & JSON.SelectToken("description").ToString & vbCrLf
            'Label2.Text += "上传日期:" & JSON.SelectToken("uploaded_pretty").ToString & vbCrLf
            'Label2.Text += "风格:" & JSON.SelectToken("game_style_name").ToString & vbCrLf
            'Label2.Text += "主题:" & JSON.SelectToken("theme_name").ToString & vbCrLf
            'Label2.Text += "难度:" & JSON.SelectToken("difficulty_name").ToString & vbCrLf
            'Label2.Text += "标签:" & JSON.SelectToken("tags_name").ToString.Replace(vbCrLf, "") & vbCrLf
            'Label2.Text += "最短时间:" & JSON.SelectToken("world_record_pretty").ToString & vbCrLf
            'Label2.Text += "上传时间:" & JSON.SelectToken("upload_time_pretty").ToString & vbCrLf
            ''LABEL2.Text += "过关条件:" & JSON.SelectToken("clear_condition_name").ToString & vbCrLf
            'Label2.Text += "过关:" & JSON.SelectToken("clears").ToString & vbCrLf
            'Label2.Text += "尝试:" & JSON.SelectToken("attempts").ToString & vbCrLf
            'Label2.Text += "过关率：" & JSON.SelectToken("clear_rate").ToString & vbCrLf
            'Label2.Text += "游玩次数:" & JSON.SelectToken("plays").ToString & vbCrLf
            'Label2.Text += "游玩人数:" & JSON.SelectToken("unique_players_and_versus").ToString & vbCrLf
            'Label2.Text += "赞:" & JSON.SelectToken("likes").ToString & vbCrLf
            'Label2.Text += "孬:" & JSON.SelectToken("boos").ToString & vbCrLf
            'Label2.Text += "对战游玩:" & JSON.SelectToken("versus_matches").ToString & vbCrLf
            'Label2.Text += "合作游玩:" & JSON.SelectToken("coop_matches").ToString & vbCrLf
            'Label2.Text += "本周游玩:" & JSON.SelectToken("weekly_plays").ToString & vbCrLf
            'Label2.Text += "本周点赞:" & JSON.SelectToken("weekly_likes").ToString & vbCrLf


            'Label3.Text = "关卡作者" & GetToken(JSON, "uploader")
            'Label4.Text = "最先通过" & GetToken(JSON, "first_completer")
            'Label5.Text = "最短时间" & GetToken(JSON, "record_holder")
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
    Function GetStrW(s As String) As Single
        Dim B As New Bitmap(300, 100)
        Dim G As Graphics = Graphics.FromImage(B), SZ As SizeF
        SZ = G.MeasureString(s, Button1.Font)
        GetStrW = SZ.Width
    End Function
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
    Dim CX0 As Integer = 0, CY0 As Integer = 0
    Dim CX1 As Integer = 0, CY1 As Integer = 0

    Private Sub MapB_MouseMove(sender As Object, e As MouseEventArgs) Handles MapB.MouseMove
        If MapA.IsPointInImage(e.X, e.Y) Then
            Dim R = MapB.PointToImage(e.X, e.Y)
            Dim CX = 1 + R.X \ PZoom
            Dim CY = 1 + R.Y \ PZoom
            If CX < 0 OrElse CX > 300 OrElse CY < 0 OrElse CY > 300 Then
                PicBot.Visible = False
                Exit Sub
            End If
            If CX <> CX1 OrElse CY <> CY1 Then
                Dim ObjC = ObjLocData(1, CX, MapHeight(1) + 1 - CY)
                If ObjC.Obj = Nothing Then
                    PicBot.Visible = False
                    Exit Sub
                End If
                PicBot.Image = GetItemImg(ObjC, 16, 16)
                PicBot.Visible = True
                CX1 = CX
                CY1 = CY
            End If
            PicBot.Left = MapB.Left + e.X + 16
            PicBot.Top = MapB.Top + e.Y + 16
        Else
            PicBot.Visible = False
        End If
    End Sub

    Dim CX, CY As Integer
    Private Sub MapA_MouseMove(sender As Object, e As MouseEventArgs) Handles MapA.MouseMove
        If MapA.IsPointInImage(e.X, e.Y) Then
            Dim R = MapA.PointToImage(e.X, e.Y)
            Dim CX = 1 + R.X \ PZoom
            Dim CY = 1 + R.Y \ PZoom
            If CX < 0 OrElse CX > 300 OrElse CY < 0 OrElse CY > 300 Then
                PicBot.Visible = False
                Exit Sub
            End If
            If CX <> CX0 OrElse CY <> CY0 Then
                Dim ObjC = ObjLocData(0, CX, MapHeight(0) + 1 - CY)
                If ObjC.Obj = Nothing Then
                    PicBot.Visible = False
                    Exit Sub
                End If
                PicBot.Image = GetItemImg(ObjC, 16, 16)
                PicBot.Visible = True
                CX0 = CX
                CY0 = CY
            End If
            PicBot.Left = MapA.Left + e.X + 16
            PicBot.Top = MapA.Top + e.Y + 16
        Else
            PicBot.Visible = False
        End If
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        MapA.Width = Me.ClientSize.Width - 408
        MapA.Height = Me.ClientSize.Height \ 2
        MapB.Top = MapA.Height
        MapB.Width = MapA.Width
        MapB.Height = MapA.Height
    End Sub


    Private Sub Button6_Click_1(sender As Object, e As EventArgs)
        Dim bcdFileBytes = File.ReadAllBytes(TextBox1.Text)
        Dim DeData = Decrypt(bcdFileBytes)
        File.WriteAllBytes(TextBox1.Text & ".DAT", DeData)
    End Sub

    Private Sub TextBox9_Click(sender As Object, e As EventArgs) Handles TCode.Click
        TCode.SelectAll()
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
            TCode.Text = OCRbyImg(BBB)
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



End Class
