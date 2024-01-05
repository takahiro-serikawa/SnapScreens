Public Class Form2
    Public Sub New()
        InitializeComponent()

        ' カーソルのある方のスクリーンを画像としてキャプチャ
        Dim bounds = Screen.GetBounds(Cursor.Position)
        Dim captured As New Bitmap(bounds.Width, bounds.Height)
        Using g = Graphics.FromImage(captured)
            g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size)
        End Using

        ' 最前面で最大化表示
        Me.Text = ""
        Me.ControlBox = False
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = bounds.Location
        Me.WindowState = FormWindowState.Maximized
        Me.TopMost = True
        Me.Show()
        Me.TopMost = False

        ' なにかキーが押されるととりあえず閉じる
        AddHandler Me.KeyPress, Sub(sender, e) Me.Close()

        ' 画像表示、マウスによる矩形選択
        Dim isSelecting = False
        Dim p1 As Point
        Dim selRect As Rectangle
        Dim picturebox1 As New PictureBox With {
            .Parent = Me,
            .Size = captured.Size,
            .Image = captured,
            .Cursor = Cursors.Cross
        }

        AddHandler picturebox1.MouseDown,
            Sub(sender, e)
                ' 選択はじめ
                If e.Button = MouseButtons.Left Then
                    isSelecting = True
                    p1 = e.Location
                End If
            End Sub

        AddHandler picturebox1.MouseMove,
            Sub(sender, e)
                ' 選択中
                If isSelecting Then
                    selRect = New Rectangle(Math.Min(p1.X, e.X), Math.Min(p1.Y, e.Y),
                                            Math.Abs(p1.X - e.X), Math.Abs(p1.Y - e.Y))
                    picturebox1.Invalidate()
                End If
            End Sub

        AddHandler picturebox1.MouseUp,
            Sub(sender, e)
                ' 選択おわり
                If e.Button = MouseButtons.Left Then
                    isSelecting = False

                    If selRect.Width > 0 AndAlso selRect.Height > 0 Then
                        ' 画像を切り抜いてクリップボードへコピー
                        Dim cropped As New Bitmap(selRect.Width, selRect.Height)
                        Using g = Graphics.FromImage(cropped)
                            g.DrawImage(picturebox1.Image, New Rectangle(0, 0, cropped.Width, cropped.Height), selRect, GraphicsUnit.Pixel)
                        End Using
                        Clipboard.SetImage(cropped)
                    End If
                End If
            End Sub

        AddHandler picturebox1.Paint,
            Sub(sender, e)
                ' 選択中の矩形を描画
                Using pen1 As New Pen(Color.Red, 1)
                    e.Graphics.DrawRectangle(pen1, selRect)
                End Using
            End Sub

    End Sub
End Class