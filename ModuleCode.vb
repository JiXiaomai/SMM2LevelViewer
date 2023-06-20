Module ModuleCode
    Const chr1 As String = "0123456789BCDFGHJKLMNPQRSTVWXY"
    Const chr2 As String = "00010110100000001110000001111100"
    Const chr3 As String = "00000000000000000000000000000000"
    Function Num2Code(N As Long, IsLvl As Boolean) As String
        Dim R As String = ""
        Dim num As Long, code As String
        Dim A, B, C, D, E, F As String, H As Long
        Dim FC As String
        A = "1000"
        D = IIf(IsLvl, "0", "1")
        E = "1"

        R += ("编号：" & N & vbCrLf & vbCrLf)
        H = N Xor Convert.ToInt64(chr2, 2)
        R += ("H=" & H & vbCrLf)

        B = Strings.Right(chr3 & Convert.ToString((N - 31) Mod 64, 2), 6)
        FC = Strings.Right(chr3 & Convert.ToString(H, 2), 32)
        F = Strings.Left(FC, 12)
        C = Strings.Right(FC, 20)

        R += ("A=" & A & vbCrLf)
        R += ("B=" & B & vbCrLf)
        R += ("C=" & C & vbCrLf)
        R += ("D=" & D & IIf(D = "0", "(L)", "(M)") & vbCrLf)
        R += ("E=" & E & vbCrLf)
        R += ("F=" & F & vbCrLf)
        num = Convert.ToInt64(A & B & C & D & E & F, 2)
        R += ("N2=" & A & B & C & D & E & F & vbCrLf)
        R += ("N1=" & num & vbCrLf)

        code = ""
        Do Until num = 0
            code &= Mid(chr1, (num Mod 30) + 1, 1)
            num \= 30
        Loop
        'R += (vbCrLf & IIf(D = "0", "关卡ID：", "工匠ID：") & code)
        R = code
        Return R
    End Function
    Function Code2Num(code As String, ByRef INFO As String) As Integer
        INFO = ""
        Dim num As Long, i As Long, N As String
        num = 0
        For i = 9 To 1 Step -1
            num = num * 30 + InStr(chr1, Mid(code, i, 1)) - 1
        Next

        N = Convert.ToString(num, 2)

        Dim A, B, C, D, E, F As String
        A = Mid(N, 1, 4) 'A=1000
        B = Mid(N, 5, 6)
        C = Mid(N, 11, 20)
        D = Mid(N, 31, 1) 'D=0 Level  D=1 Maker
        E = Mid(N, 32, 1) 'E=1
        F = Strings.Right(N, 12)
        Dim H, M As Long
        H = Convert.ToInt64(F & C, 2) Xor Convert.ToInt64(chr2, 2)
        M = (H - 31) Mod 64

        INFO += IIf(D = "0", "    关卡ID：", "    工匠ID：") & code & vbCrLf
        INFO += "    N1=" & num.ToString & vbCrLf
        INFO += "    N2=" & N & vbCrLf
        INFO += "    A =" & A & vbCrLf
        INFO += "    B =" & B & " (" & Convert.ToInt64(B, 2).ToString & ")" & vbCrLf
        INFO += "    C =" & C & vbCrLf
        INFO += "    D =" & D & IIf(D = "0", " (Level)", " (Maker)") & vbCrLf
        INFO += "    E =" & E & vbCrLf
        INFO += "    F =" & F & vbCrLf
        INFO += "    G =" & Convert.ToInt64(F & C, 2) & vbCrLf
        INFO += "    H =" & H.ToString & vbCrLf
        INFO += "    M =" & M.ToString & vbCrLf
        INFO += "    K =" & Convert.ToString(H, 2) & vbCrLf
        INFO += IIf(D = "0", "    关卡序号：", "    工匠序号：") & H.ToString

        If M.ToString = Convert.ToInt64(B, 2).ToString Then
            Return Int(D)
        Else
            Return -1
        End If

    End Function
End Module
