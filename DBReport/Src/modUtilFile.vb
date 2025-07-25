﻿
' File modUtilFile.vb : Utility module for files and folders
' -------------------

Imports System.Text ' For Encoding
Imports System.Runtime.CompilerServices ' For MethodImpl(MethodImplOptions.AggressiveInlining)

Module modUtilFile

    Public Const sPossibleErrCause$ =
        "The file may be write-protected or locked by another software"

#Region "Reading files"

    Public Function bFileExists(sFilePath$, Optional bPrompt As Boolean = False) As Boolean

        ' Return True if the specified file is found
        ' Note : It doesn't work with filter, for ex. C:\*.txt
        Dim bExists As Boolean = IO.File.Exists(sFilePath)

        If Not bExists AndAlso bPrompt Then _
            MsgBox("Can't find file : " & IO.Path.GetFileName(sFilePath) & vbLf & sFilePath,
                MsgBoxStyle.Critical, m_sMsgTitle & " - File not found")

        Return bExists

    End Function

    Public Function bFileIsWritable(sFilePath$,
        Optional bPrompt As Boolean = False,
        Optional bPromptClose As Boolean = False,
        Optional bNonExistentOk As Boolean = False,
        Optional bPromptRetry As Boolean = False) As Boolean

        ' Check first if the file is locked by a software, in order to prompt user to close it
        If Not bFileIsAvailable(sFilePath, bPrompt, bPromptClose, bNonExistentOk, bPromptRetry,
            bCheckForSlowRead:=True) Then Return False
        ' And then check if the file is not write-protected
        If Not bFileIsAvailable(sFilePath, bPrompt, bPromptClose, bNonExistentOk, bPromptRetry) Then _
            Return False

        Return True

    End Function

    ' Attribute to prevent the IDE to stop in this function if an exception is thrown
    <System.Diagnostics.DebuggerStepThrough()>
    Public Function bFileIsAvailable(sFilePath$,
        Optional bPrompt As Boolean = False,
        Optional bPromptClose As Boolean = False,
        Optional bNonExistentOk As Boolean = False,
        Optional bPromptRetry As Boolean = False,
        Optional bCheckForReadOnly As Boolean = False,
        Optional bCheckForSlowRead As Boolean = False) As Boolean

        ' Check if a file is available for read/write (for example if a file is not locked by Excel)

        ' bNonExistentOk : If the file does not exist, it is writable
        ' bCheckForReadOnly : Check if the file can be opened at least for read only
        ' bCheckForSlowRead : Throw an exception if the file is locked for example in Excel : 
        '  reading the file may be very slow in this case

Retry:
        If bNonExistentOk Then
            ' If the file does not exist, it is writable : return True without any alert
            If Not bFileExists(sFilePath) Then Return True
        Else
            If Not bFileExists(sFilePath, bPrompt) Then Return False
        End If

        'Retry:
        Dim answer As MsgBoxResult = MsgBoxResult.Cancel
        Try
            ' If Excel locked the file, the file can still be open for reading
            '  if the sharing mode is also set to IO.FileShare.ReadWrite
            Dim mode As IO.FileMode = IO.FileMode.Open
            Dim access As IO.FileAccess = IO.FileAccess.ReadWrite
            If bCheckForReadOnly Then access = IO.FileAccess.Read
            Dim share = IO.FileShare.ReadWrite
            ' bCheckForSlowRead : Throw an exception to check for slowness risk
            If bCheckForSlowRead Then share = IO.FileShare.Read : access = IO.FileAccess.Read
            Using fs As New IO.FileStream(sFilePath, mode, access, share)
                fs.Close()
            End Using
            Return True
        Catch ex As Exception
            Dim msgbs As MsgBoxStyle = MsgBoxStyle.Exclamation
            Dim sQuestion$ = ""
            If bPromptRetry Then
                msgbs = msgbs Or MsgBoxStyle.RetryCancel
                sQuestion = vbLf & "Retry ?"
            End If
            If bCheckForSlowRead AndAlso (bPrompt OrElse bPromptClose OrElse bPromptRetry) Then
                answer = MsgBox(
                    "Please close the file : " & IO.Path.GetFileName(sFilePath) & vbLf &
                    sFilePath & sQuestion, msgbs, m_sMsgTitle)
            ElseIf bPromptClose OrElse bPromptRetry Then
                If bCheckForReadOnly Then
                    ' The file cannot be read for various causes (insufficient read privileges, 
                    '  file locked by another application, ...)
                    answer = MsgBox("The file cannot be read : " & IO.Path.GetFileName(sFilePath) & vbLf &
                        sFilePath & vbLf &
                        "Please close the file, if it is opened, or change" & vbLf &
                        "his read attributes, if it is appropriate." &
                        sQuestion, msgbs, m_sMsgTitle)
                Else
                    answer = MsgBox("The file is write-protected : " & IO.Path.GetFileName(sFilePath) & vbLf &
                        sFilePath & vbLf &
                        "Please close the file, if it is opened, or change" & vbLf &
                        "his write attributes, if it is appropriate." &
                        sQuestion, msgbs, m_sMsgTitle)
                End If
            ElseIf bPrompt Then
                ShowErrorMsg(ex, "bFileIsAvailable", "The file cannot be accessed : " &
                    IO.Path.GetFileName(sFilePath) & vbCrLf & sFilePath, sPossibleErrCause)
            End If
        End Try

        If answer = MsgBoxResult.Retry Then GoTo Retry
        Return False

    End Function

    ''' <summary>
    ''' Determines a text file's encoding by analyzing its Byte Order Mark (BOM).
    ''' Defaults to ASCII when detection of the text file's endianness fails.
    ''' </summary>
    ''' <param name="filename">The text file to analyze.</param>
    ''' <returns>The detected encoding.</returns>
    Public Function GetEncoding(filename As String) As Encoding

        ' Read the BOM
        Dim bom = New Byte(3) {}
        Using file = New IO.FileStream(filename, IO.FileMode.Open, IO.FileAccess.Read)
            file.Read(bom, 0, 4)
        End Using

        ' Analyze the BOM
        'If bom(0) = &H2B AndAlso bom(1) = &H2F AndAlso bom(2) = &H76 Then
        '    Return Encoding.UTF7
        'End If
        If bom(0) = &HEF AndAlso bom(1) = &HBB AndAlso bom(2) = &HBF Then
            Return Encoding.UTF8
        End If

        ' 28/02/2016
        If bom(0) = &H22 AndAlso bom(1) = &H43 AndAlso bom(2) = &H6F AndAlso bom(3) = &H75 Then
            Return Encoding.UTF8
        End If

        If bom(0) = &HFF AndAlso bom(1) = &HFE Then
            Return Encoding.Unicode
        End If
        'UTF-16LE
        If bom(0) = &HFE AndAlso bom(1) = &HFF Then
            Return Encoding.BigEndianUnicode
        End If
        'UTF-16BE
        If bom(0) = 0 AndAlso bom(1) = 0 AndAlso bom(2) = &HFE AndAlso bom(3) = &HFF Then
            Return Encoding.UTF32
        End If
        Return Encoding.ASCII

    End Function

    Public Function asReadFile(sFilePath$,
        Optional bReadOnly As Boolean = False,
        Optional bCheckCrCrLf As Boolean = False,
        Optional bUnicodeUTF8 As Boolean = False,
        Optional bWindows1252 As Boolean = False,
        Optional encod As Encoding = Nothing) As String()

        ' Read and return the file content as an array of String

        If Not bFileExists(sFilePath, bPrompt:=True) Then Return Nothing

        Try

            Dim encod0 As Encoding
            If bUnicodeUTF8 Then
                encod0 = Encoding.UTF8
            ElseIf bWindows1252 Then
                ' Latin alphabet for English and for some other Western languages
                encod0 = Encoding.GetEncoding(1252)
            ElseIf Not IsNothing(encod) Then
                encod0 = encod
            Else
                encod0 = Encoding.Default
            End If

            If bReadOnly Then
                ' If Excel locked the file, the file can still be open for reading
                '  if the sharing mode is also set to IO.FileShare.ReadWrite
                Using fs As New IO.FileStream(sFilePath,
                    IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                    Using sr As New IO.StreamReader(fs, encod0)
                        ' Do exactly as sr.ReadLine()
                        Dim sStream As New clsStringStream(sr.ReadToEnd)
                        Return sStream.asLines(bCheckCrCrLf)
                    End Using : End Using
            Else
                Return IO.File.ReadAllLines(sFilePath, encod0)
            End If

        Catch ex As Exception
            ShowErrorMsg(ex, "asReadFile")
            Return Nothing
        End Try

    End Function

    Public Function bLetOpenFile(sFilePath$, Optional bCheckFile As Boolean = True,
        Optional sInfo$ = "") As Boolean

        ' Don't check file if it is a URL, for example
        If bCheckFile AndAlso Not bFileExists(sFilePath, bPrompt:=True) Then Return False
        Dim lFileSize& = (New IO.FileInfo(sFilePath)).Length
        Dim sFileSize$ = sDisplaySizeInBytes(lFileSize)
        Dim sMsg$ = "File created successfully : " & IO.Path.GetFileName(sFilePath) &
            vbLf & sFilePath
        If sInfo.Length > 0 Then sMsg &= vbLf & sInfo
        sMsg &= vbLf & "Would you like to open it ? (" & sFileSize & ")"
        If MsgBoxResult.Cancel = MsgBox(sMsg,
            MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, m_sMsgTitle) Then Return False
        StartAssociateApp(sFilePath)
        Return True

    End Function

    Public Sub LetOpenFile(sFilePath$, Optional sInfo$ = "")

        If Not bFileExists(sFilePath, bPrompt:=True) Then Exit Sub
        Dim lFileSize& = (New IO.FileInfo(sFilePath)).Length
        Dim sFileSize$ = sDisplaySizeInBytes(lFileSize)
        Dim sMsg$ = "File created successfully : " & IO.Path.GetFileName(sFilePath) &
            vbLf & sFilePath
        If sInfo.Length > 0 Then sMsg &= vbLf & sInfo
        sMsg &= vbLf & "Would you like to open it ? (" & sFileSize & ")"
        If MsgBoxResult.Cancel = MsgBox(sMsg,
            MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, m_sMsgTitle) Then Exit Sub
        StartAssociateApp(sFilePath)

    End Sub

    Public Sub StartAssociateApp(sFilePath$,
        Optional bMaximized As Boolean = False,
        Optional bCheckFile As Boolean = True,
        Optional sArguments$ = "")

        If bCheckFile Then ' Don't check file if it is a URL to browse
            If Not bFileExists(sFilePath, bPrompt:=True) Then Exit Sub
        End If
        Dim p As New Process
        p.StartInfo = New ProcessStartInfo(sFilePath)
        p.StartInfo.Arguments = sArguments
        p.StartInfo.UseShellExecute = True
        If bMaximized Then p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
        p.Start()

    End Sub

    Public Function sDisplaySizeInBytes$(lSizeInBytes&,
        Optional bShowDetails As Boolean = False,
        Optional bRemoveDot0 As Boolean = False)

        ' Return a file size in a correct string format
        ' (see also StrFormatByteSizeA API in shlwapi.dll)

        Dim rNbKb! = CSng(Math.Round(lSizeInBytes / 1024, 1))
        Dim rNbMb! = CSng(Math.Round(lSizeInBytes / (1024 * 1024), 1))
        Dim rNbGb! = CSng(Math.Round(lSizeInBytes / (1024 * 1024 * 1024), 1))
        Dim sResult$ = ""

        If bShowDetails Then
            sResult = sDisplayNumeric(lSizeInBytes) & " bytes"
            If rNbKb >= 1 Then sResult &= " (" & sDisplayNumeric(rNbKb) & " Kb"
            If rNbMb >= 1 Then sResult &= " = " & sDisplayNumeric(rNbMb) & " Mb"
            If rNbGb >= 1 Then sResult &= " = " & sDisplayNumeric(rNbGb) & " Gb"
            If rNbKb >= 1 Or rNbMb >= 1 Or rNbGb >= 1 Then sResult &= ")"
        Else
            If rNbGb >= 1 Then
                sResult = sDisplayNumeric(rNbGb, bRemoveDot0) & " Gb"
            ElseIf rNbMb >= 1 Then
                sResult = sDisplayNumeric(rNbMb, bRemoveDot0) & " Mb"
            ElseIf rNbKb >= 1 Then
                sResult = sDisplayNumeric(rNbKb, bRemoveDot0) & " Kb"
            Else
                sResult = sDisplayNumeric(lSizeInBytes, bRemoveDot0:=True) & " bytes"
            End If
        End If

        sDisplaySizeInBytes = sResult

    End Function

    Public Function sDisplayTime$(rNbSeconds#)

        ' Return a during time in a String

        Dim sDisplay$ = ""
        Dim sSep$ = ""

        Dim rNbMn# = rNbSeconds / 60
        Dim rNbHours# = rNbMn / 60
        Dim rNbDays# = rNbHours / 24

        Dim iNbDays% = CInt(Fix(rNbDays))
        If iNbDays >= 1 Then
            sDisplay &= sSep & iNbDays & " d." : sSep = " "
            rNbHours -= iNbDays * 24
            rNbMn -= iNbDays * 24 * 60
            rNbSeconds -= iNbDays * 24 * 3600
        End If
        Dim iNbHours% = CInt(Fix(rNbHours))
        If iNbHours >= 1 Then
            sDisplay &= sSep & iNbHours & " h." : sSep = " "
            rNbMn -= iNbHours * 60
            rNbSeconds -= iNbHours * 3600
        End If
        Dim iNbMn% = CInt(Fix(rNbMn))
        If iNbMn >= 1 Then
            sDisplay &= sSep & iNbMn & " mn" : sSep = " "
            rNbSeconds -= iNbMn * 60
        End If

        Dim rNbSecondsSng! = CSng(rNbSeconds)
        ' 14/12/2016 Hide 0 sec. if there are mn, h or days
        If rNbSecondsSng = 0 AndAlso (iNbMn > 0 OrElse iNbHours > 0 OrElse iNbDays > 0) Then
            ' Display nothing else
        ElseIf rNbSeconds >= 0 Then
            sDisplay &= sSep & sDisplayNumeric(rNbSecondsSng, bRemoveDot0:=True) & " sec."
        End If

        Return sDisplay

    End Function

    Public Function sDisplayNumeric$(rValue!, Optional bRemoveDot0 As Boolean = True, Optional iNbDigits% = 1)

        ' Show a numeric with 1 digit accuracy by default

        ' The standard numeric format is correct, 
        '  we just have to remove useless ending dot if the value is zero

        Dim nfi As Globalization.NumberFormatInfo =
            New Globalization.NumberFormatInfo
        nfi.NumberGroupSeparator = " "   ' Thousand and million separator...
        Const sDot$ = "."
        nfi.NumberDecimalSeparator = sDot ' Decimal separator
        ' 3 groups for billion, million and thousand
        nfi.NumberGroupSizes = New Integer() {3, 3, 3}
        nfi.NumberDecimalDigits = iNbDigits ' 1 digit accuracy
        Dim sResult$ = rValue.ToString("n", nfi) ' n : general numeric
        If bRemoveDot0 Then
            If iNbDigits = 1 Then
                sResult = sResult.Replace(".0", "")
            ElseIf iNbDigits > 1 Then
                Dim i%
                Dim sb As New StringBuilder(sDot)
                For i = 1 To iNbDigits : sb.Append("0") : Next
                sResult = sResult.Replace(sb.ToString, "")
            End If
        End If
        Return sResult

    End Function

#End Region

#Region "Writing files"

    ' Attribute to prevent the IDE to stop in this function if an exception is thrown
    <System.Diagnostics.DebuggerStepThrough()>
    Public Function bFileLocked(sFilePath$,
        Optional bReadOnly As Boolean = False,
        Optional bNonExistentOk As Boolean = False,
        Optional bPrompt As Boolean = False,
        Optional bPromptClose As Boolean = False,
        Optional bPromptRetry As Boolean = False) As Boolean

        ' Check if a file is locked, for writing or just reading 
        ' (for example if a file is not locked by Excel)

        ' bReadOnly : Check if a file is locked just for reading
        ' bNonExistentOk : If the file doesn't exist yet then there is no problem
        ' bPrompt : Alert the user, otherwise continue
        ' bPromptClose : Alert the user to close the file (if bPrompt is set to false)
        ' bPromptRetry : Alert the user to close the file again and again, 
        '  while the file is locked (if bPrompt is set to false)

        Dim bLocked As Boolean = True

        If bNonExistentOk Then
            If Not bFileExists(sFilePath) Then Return False ' It does not exist so it is not locked
        Else
            ' It does not exists so it can't be read nor written
            If Not bFileExists(sFilePath, bPrompt) Then Return True
        End If

Retry:
        Dim userResponse As MsgBoxResult = MsgBoxResult.Cancel
        Try
            ' If Excel locked the file, the file can still be open for reading
            '  if the sharing mode is also set to IO.FileShare.ReadWrite
            Dim mode As IO.FileMode = IO.FileMode.Open
            Dim access As IO.FileAccess = IO.FileAccess.ReadWrite
            If bReadOnly Then access = IO.FileAccess.Read
            Using fs As New IO.FileStream(sFilePath, mode, access, IO.FileShare.ReadWrite)
            End Using ' fs.Close()
            bLocked = False

        Catch ex As Exception
            Dim msgbs As MsgBoxStyle = MsgBoxStyle.Exclamation
            If bPrompt Then
                ShowErrorMsg(ex, "bFileLocked", "Can't access to the file : " &
                    IO.Path.GetFileName(sFilePath) & vbCrLf & sFilePath, sPossibleErrCause)
            ElseIf bPromptClose OrElse bPromptRetry Then
                Dim sQuestion$ = ""
                If bPromptRetry Then
                    msgbs = msgbs Or MsgBoxStyle.RetryCancel
                    sQuestion = vbLf & "Retry ?"
                End If
                If bReadOnly Then
                    userResponse = MsgBox("Please close the file : " &
                        IO.Path.GetFileName(sFilePath) & vbLf & sFilePath &
                        sQuestion, msgbs, m_sMsgTitle)
                Else
                    userResponse = MsgBox("The file can't be written : " &
                        IO.Path.GetFileName(sFilePath) & vbLf & sFilePath & vbLf &
                        "Please close it as the case may be, or change it's attributes," & vbLf &
                        "or its permissions." & sQuestion, msgbs, m_sMsgTitle)
                End If
            End If
        End Try

        If bLocked And userResponse = MsgBoxResult.Retry Then GoTo Retry
        Return bLocked

    End Function

    Public Function bDeleteFile(sFilePath$, Optional bPromptIfErr As Boolean = False) As Boolean

        If Not bFileExists(sFilePath) Then Return True

        If bFileLocked(sFilePath, bPromptClose:=bPromptIfErr, bPromptRetry:=bPromptIfErr) Then _
            Return False

        Try
            IO.File.Delete(sFilePath)
            Return True

        Catch ex As Exception
            If bPromptIfErr Then _
                ShowErrorMsg(ex, "Can't delete file : " & IO.Path.GetFileName(sFilePath) & vbCrLf &
                    sFilePath, sPossibleErrCause)
            Return False
        End Try

    End Function

    Public Function bWriteFile(sFilePath$, sbContenu As StringBuilder,
        Optional bDefautEncoding As Boolean = True,
        Optional encode As Encoding = Nothing,
        Optional bPromptIfErr As Boolean = True,
        Optional ByRef sMsgErr$ = "") As Boolean

        If Not bDeleteFile(sFilePath, bPromptIfErr:=True) Then Return False

        Dim sw As IO.StreamWriter = Nothing
        Try
            If bDefautEncoding Then encode = Encoding.Default
            sw = New IO.StreamWriter(sFilePath, append:=False, encoding:=encode)
            sw.Write(sbContenu.ToString())
            sw.Close()
            Return True
        Catch ex As Exception
            Dim sMsg$ = "Can't write file : " & IO.Path.GetFileName(sFilePath) & vbCrLf &
                sFilePath & vbCrLf & sPossibleErrCause
            sMsgErr = sMsg & vbCrLf & ex.Message
            If bPromptIfErr Then ShowErrorMsg(ex, "bWriteFile", sMsg)
            Return False
        Finally
            If Not IsNothing(sw) Then sw.Close()
        End Try

    End Function

#End Region

#Region "String stream class"

    ' Equivalent of mscorlib.dll: System.IO.StreamReader.ReadLine() As String
    '  but for a string

    Public Class clsStringStream

        Private m_iNumLine% = 0 ' Debug
        Private m_sString$
        Private m_iPos% = 0
        Private c13 As Char = ChrW(13) ' vbCr
        Private c10 As Char = ChrW(10) ' vbLf

        Public Sub New(sString$)
            m_sString = sString
        End Sub

        Public Function asLines(Optional bCheckCrCrLf As Boolean = False) As String()

            Dim lst As New Collections.Generic.List(Of String)
            Dim iNumLine2% = 0
            Do
                Dim sLine$ = StringReadLine(bCheckCrCrLf)
                If IsNothing(sLine) Then sLine = ""
                lst.Add(sLine)
                iNumLine2 += 1
            Loop While m_iPos < m_sString.Length
            Return lst.ToArray

        End Function

        ' Attribute for inline to avoid function overhead
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function StringReadLine$(Optional bCheckCrCrLf As Boolean = False)

            If String.IsNullOrEmpty(m_sString) Then Return Nothing

            Dim iLong% = m_sString.Length

            Dim iNum% = m_iPos
            Do While iNum < iLong
                Dim ch As Char = m_sString.Chars(iNum)
                Select Case ch
                    Case c13, c10

                        Dim str$ = m_sString.Substring(m_iPos, iNum - m_iPos)

                        m_iPos = iNum + 1

                        If Not bCheckCrCrLf Then
                            If ch = c13 AndAlso m_iPos < iLong AndAlso
                            m_sString.Chars(m_iPos) = c10 Then m_iPos += 1
                            Return str
                        End If

                        Dim chNext As Char
                        If m_iPos < iLong Then chNext = m_sString.Chars(m_iPos)

                        Dim chNext2 As Char
                        If m_iPos < iLong - 1 Then chNext2 = m_sString.Chars(m_iPos + 1)
                        If ch = c13 AndAlso m_iPos < iLong - 1 AndAlso
                        chNext = c13 AndAlso chNext2 = c10 Then
                            m_iPos += 2
                        ElseIf ch = c13 AndAlso m_iPos < iLong AndAlso chNext = c10 Then
                            m_iPos += 1
                        End If
                        m_iNumLine += 1
                        Return str
                End Select
                iNum += 1
            Loop
            If iNum > m_iPos Then
                Dim str2$ = m_sString.Substring(m_iPos, iNum - m_iPos)
                m_iPos = iNum
                m_iNumLine += 1
                Return str2
            End If

            Return Nothing

        End Function

    End Class

#End Region

    Public Function asCmdLineArg(sCmdLine$, Optional bRemoveSpaces As Boolean = True) As String()

        ' Return arguments of the command line

        ' "Filenames with spaces are quoted", FilenamesWhihoutSpaceAreNotQuoted
        ' Example : "Filename with spaces 1" UnspacedFilename "Filename with spaces 2"

        Dim asArgs$() = Nothing
        If String.IsNullOrEmpty(sCmdLine) Then
            ReDim asArgs(0)
            asArgs(0) = ""
            asCmdLineArg = asArgs
            Exit Function
        End If

        Dim lstArgs As New List(Of String) ' 16/10/2016
        Const iASCQuotes% = 34
        Const sDbleQuote$ = """" ' Only one " in fact : Chr$(34)
        Dim sFile$, sDelimiter$
        Dim sCmd$, iCmdLen%, iEnd%, iStart%, iStart2%
        Dim bEnd As Boolean, bQuotedFile As Boolean
        Dim iNextCar% = 1
        sCmd = sCmdLine
        iCmdLen = Len(sCmd)
        iStart = 1
        Do

            bQuotedFile = False : sDelimiter = " "

            If Mid(sCmd, iStart, 2) = sDbleQuote & sDbleQuote Then
                bQuotedFile = True : sDelimiter = sDbleQuote
                iEnd = iStart + 1
                GoTo NextItem
            End If

            If Mid(sCmd, iStart, 1) = sDbleQuote Then bQuotedFile = True : sDelimiter = sDbleQuote

            iStart2 = iStart
            If bQuotedFile AndAlso iStart2 < iCmdLen Then iStart2 += 1
            iEnd = InStr(iStart2 + 1, sCmd, sDelimiter)

            ' 16/10/2016 DblQuote " can replace space
            iNextCar = 1
            Dim iQuotedEnd% = InStr(iStart2 + 1, sCmd, sDbleQuote)
            If iQuotedEnd > 0 AndAlso iEnd > 0 AndAlso iQuotedEnd < iEnd Then
                iEnd = iQuotedEnd : bQuotedFile = True : sDelimiter = sDbleQuote : iNextCar = 0
            End If

            If iEnd = 0 Then bEnd = True : iEnd = iCmdLen + 1
            sFile = Mid(sCmd, iStart2, iEnd - iStart2)
            If bRemoveSpaces Then sFile = Trim$(sFile)

            If sFile.Length > 0 Then lstArgs.Add(sFile)

            If bEnd Or iEnd = iCmdLen Then Exit Do

NextItem:
            iStart = iEnd + iNextCar ' 1
            ' 16/10/2016 DblQuote " can replace space
            'If bQuotedFile Then iStart = iEnd + 2
            If iStart > iCmdLen Then Exit Do

        Loop

        asArgs = lstArgs.ToArray()
        For iNumArg As Integer = 0 To UBound(asArgs)
            Dim sArg$ = asArgs(iNumArg)
            Dim iLen% = Len(sArg)
            If iLen = 1 AndAlso Asc(sArg.Chars(0)) = iASCQuotes Then asArgs(iNumArg) = ""
        Next iNumArg

        Return asArgs

    End Function

End Module