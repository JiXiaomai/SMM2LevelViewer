
Public Class Form2

    Dim MX, MY As Integer, isMove As Boolean
    Dim CX, CY, CX0, CY0 As Integer

    Public Property ZoomValue As Integer = 64

    Private Sub P_MouseDown(sender As Object, e As MouseEventArgs) Handles P.MouseDown
        MX = e.X
        MY = e.Y
        isMove = True
    End Sub


    Private Sub P_MouseUp(sender As Object, e As MouseEventArgs) Handles P.MouseUp
        isMove = False
    End Sub


    Private Sub P_MouseMove(sender As Object, e As MouseEventArgs) Handles P.MouseMove
        If isMove Then
            P.Left += e.X - MX
            P.Top += e.Y - MY
        End If
        CX = 1 + e.X \ ZoomValue
        CY = 1 + e.Y \ ZoomValue
        If CX <> CX0 OrElse CY <> CY0 Then
            ToolTip1.Hide(P)
            ObjC = ObjLocData(0, CX, MapHeight(0) + 1 - CY)
            TTipImg = GetItemImg(ObjC, TTW, TTH)
            ToolTip1.Show(" ", P)
            CX0 = CX
            CY0 = CY
        End If
    End Sub
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        ToolTip1.OwnerDraw = True
        ToolTip1.AutomaticDelay = 0
        ToolTip1.AutoPopDelay = 0
        'ToolTip1.SetToolTip(P, "")
    End Sub
    Private Sub ToolTip1_Popup(sender As Object, e As PopupEventArgs) Handles ToolTip1.Popup
        e.ToolTipSize = New Size(TTW, TTH)
    End Sub
    Private Sub ToolTip1_Draw(sender As Object, e As DrawToolTipEventArgs) Handles ToolTip1.Draw
        e.Graphics.Clear(SystemColors.Info)
        e.Graphics.DrawImage(TTipImg, New Point(0, 0))
        TextRenderer.DrawText(e.Graphics, e.ToolTipText, e.Font,
                              New Rectangle(64, 8, e.Bounds.Width - 72, e.Bounds.Height - 16),
                              SystemColors.InfoText, Color.Empty,
                              TextFormatFlags.WordBreak Or TextFormatFlags.VerticalCenter)
    End Sub
    Dim TTipImg As Bitmap, TTW As Integer = 16, TTH As Integer = 16
    Dim ObjC As ObjStr

End Class