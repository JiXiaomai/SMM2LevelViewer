Public Class Form2
    Dim MX, MY As Integer, isMove As Boolean
    Dim CX, CY As Integer
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
        CX = 1 + e.X \ 16
        CY = 1 + e.Y \ 16
        On Error Resume Next
        Me.Text = CX.ToString & "," & CY.ToString & "=" & ObjLocData(0, CX, MapHeight(1) + 1 - CY)
    End Sub
End Class