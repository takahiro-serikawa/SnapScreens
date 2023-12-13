Public Class Form2
    Public Sub New(bitmap As Bitmap)
        Me.Text = ""
        Me.ControlBox = False
        Me.WindowState = FormWindowState.Maximized
        Me.TopMost = True
        Me.Show()
        AddHandler Me.KeyPress, Sub(sender, e) Me.Close()

        Dim isSelecting = False
        Dim p1 As Point
        Dim selRect As Rectangle
        Dim picturebox1 As New PictureBox With {
            .Parent = Me,
            .Size = bitmap.Size,
            .Image = bitmap,
            .Cursor = Cursors.Cross
        }

        AddHandler picturebox1.MouseDown,
            Sub(sender, e)
                If e.Button = MouseButtons.Left Then
                    isSelecting = True
                    p1 = e.Location
                End If
            End Sub

        AddHandler picturebox1.MouseMove,
            Sub(sender, e)
                If isSelecting Then
                    Dim p2 = e.Location
                    selRect = New Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y),
                                            Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y))
                    picturebox1.Invalidate()
                End If
            End Sub

        AddHandler picturebox1.MouseUp,
            Sub(sender, e)
                If e.Button = MouseButtons.Left Then
                    isSelecting = False

                    Dim cropped As New Bitmap(selRect.Width, selRect.Height)
                    Using g = Graphics.FromImage(cropped)
                        g.DrawImage(picturebox1.Image, New Rectangle(0, 0, cropped.Width, cropped.Height), selRect, GraphicsUnit.Pixel)
                    End Using
                    Clipboard.SetImage(cropped)
                End If
            End Sub

        AddHandler picturebox1.Paint,
            Sub(sender, e)
                'Dim ia As New ImageAttributes()
                'ia.SetColorMatrix(New ColorMatrix With {
                '                    .Matrix00 = 0.25F, .Matrix11 = 0.25F, .Matrix22 = 0.25F,
                '                    .Matrix33 = 1, .Matrix44 = 1})

                'e.Graphics.DrawImage(Image, New Rectangle(0, 0, Image.Width, Image.Height),
                '                    0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia)

                'e.Graphics.DrawImage(Image, selRect.X, selRect.Y, selRect, GraphicsUnit.Pixel)

                Using pen1 As New Pen(Color.Red, 1)
                    e.Graphics.DrawRectangle(pen1, selRect)
                End Using
            End Sub

    End Sub
End Class