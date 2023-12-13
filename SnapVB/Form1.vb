Imports System.Runtime.InteropServices

Public Class Form1
    Public Sub New()
        InitializeComponent()

        AddHandler Application.ThreadException, Function(sender, e) _
            MessageBox.Show(e.Exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)

        AddHandler Me.FormClosed, Function(sender, e) UnregisterHotKey(Me.Handle, hotkey_id)

        ' ホットキーを変えるときはここ
        RegisterHotKey(Me.Handle, hotkey_id, MODKEY.CONTROL + MODKEY.ALT, Keys.F2)
    End Sub

    Enum MODKEY
        ALT = &H1
        CONTROL = &H2
        SHIFT = &H4
    End Enum

    Dim hotkey_id = 1

    Const WM_HOTKEY As Integer = &H312

    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)

        If m.Msg = WM_HOTKEY AndAlso m.WParam.ToInt32() = hotkey_id Then
            Dim bounds = Screen.GetBounds(Cursor.Position)
            Dim captured As New Bitmap(bounds.Width, bounds.Height)
            Using g = Graphics.FromImage(captured)
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size)
            End Using
            Dim f As New Form2(captured)
        End If
    End Sub

    <DllImport("user32.dll")>
    Shared Function RegisterHotKey(hWnd As IntPtr, id As Integer, fsModifiers As MODKEY, vk As Keys) As Boolean
    End Function

    <DllImport("user32.dll")>
    Shared Function UnregisterHotKey(hWnd As IntPtr, id As Integer) As Boolean
    End Function
End Class
