
' File modUtilFile.vb : Utility module for files and folders
' ----------------

Imports System.Text

Module modUtilFile

Private Const sPossibleErrCause$ = _
    "The file may be write protected or locked by another software"

#Region "Reading files"

Public Function bFileExists(sFilePath$, Optional bPrompt As Boolean = False) As Boolean

    ' Return True if the specified file is founad
    ' Don't work with filter, for ex. C:\*.txt
    Dim bExists As Boolean = IO.File.Exists(sFilePath)

    If Not bExists AndAlso bPrompt Then _
        MsgBox("Can't find file :" & vbLf & sFilePath, _
            MsgBoxStyle.Critical, m_sMsgTitle & " - File not found")

    Return bExists

End Function

Public Sub LetOpenFile(sFilePath$, Optional sInfo$ = "")

    If Not bFileExists(sFilePath, bPrompt:=True) Then Exit Sub
    Dim lFileSize& = (New IO.FileInfo(sFilePath)).Length
    Dim sFileSize$ = sDisplaySizeInBytes(lFileSize)
    Dim sMsg$ = "File created successfully : " & IO.Path.GetFileName(sFilePath) & _
        vbLf & sFilePath
    If sInfo.Length > 0 Then sMsg &= vbLf & sInfo
    sMsg &= vbLf & "Would you like to open it ? (" & sFileSize & ")"
    If MsgBoxResult.Cancel = MsgBox(sMsg, _
        MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, m_sMsgTitle) Then Exit Sub
    StartAssociateApp(sFilePath)

End Sub

Public Sub StartAssociateApp(sFilePath$, _
    Optional bMaximized As Boolean = False, _
    Optional bCheckFile As Boolean = True, _
    Optional sArguments$ = "")

    If bCheckFile Then ' Don't check file if it is an URL to browse
        If Not bFileExists(sFilePath, bPrompt:=True) Then Exit Sub
    End If
    Dim p As New Process
    p.StartInfo = New ProcessStartInfo(sFilePath)
    p.StartInfo.Arguments = sArguments
    If bMaximized Then p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
    p.Start()

End Sub

Public Function sDisplaySizeInBytes$(lSizeInBytes&, _
    Optional bShowDetails As Boolean = False, _
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

Public Function sDisplayNumeric$(rVal!, _
    Optional bRemoveDot0 As Boolean = True, _
    Optional iNbDecimals% = 1)

    ' Show a numeric using 1 decimal precision

    ' The standard numeric format is correct, 
    '  we just have to remove useless decimal separator if zero

    Dim nfi As Globalization.NumberFormatInfo = _
        New Globalization.NumberFormatInfo
    nfi.NumberGroupSeparator = " "   ' Thousand and million separator...
    nfi.NumberDecimalSeparator = "." ' Decimal separator
    ' 3 groups for billion, million and Thousand
    nfi.NumberGroupSizes = New Integer() {3, 3, 3}
    nfi.NumberDecimalDigits = iNbDecimals ' 1 decimal for precision
    Dim sResult$ = rVal.ToString("n", nfi) ' n : general numeric
    If bRemoveDot0 Then
        If iNbDecimals = 1 Then
            sResult = sResult.Replace(".0", "")
        ElseIf iNbDecimals > 1 Then
            Dim i%
            Dim sb As New StringBuilder(".")
            For i = 1 To iNbDecimals : sb.Append("0") : Next
            sResult = sResult.Replace(sb.ToString, "")
        End If
    End If
    Return sResult

End Function

#End Region

#Region "Writing files"

' Attribute to prevent the IDE to stop in this function if an exception is thrown
<System.Diagnostics.DebuggerStepThrough()> _
Public Function bFileLocked(sFilePath$, _
    Optional bReadOnly As Boolean = False, _
    Optional bNotExistsOk As Boolean = False, _
    Optional bPrompt As Boolean = False, _
    Optional bPromptClose As Boolean = False, _
    Optional bPromptRetry As Boolean = False) As Boolean

    ' Check if a file is locked, for writting or just reading 
    ' (for example if a file is not locked by Excel)

    ' bReadOnly : check if a file is locked just for reading
    ' bNotExistsOk : if the file doesn't exist yet then there is no problem
    ' bPrompt : alert the user, otherwise continue
    ' bPromptClose : alert the user to close the file (if bPrompt set to false)
    ' bPromptRetry : alert the user to close the file again and again, 
    '  while the file is locked (if bPrompt set to false)

    Dim bLocked As Boolean = True

    If bNotExistsOk Then
        If Not bFileExists(sFilePath) Then Return False ' Not exists so not locked
    Else
        ' Not exists so can't be read nor write
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
            ShowErrorMsg(ex, "bFileLocked", "Can't access to the file :" & vbLf & _
                sFilePath, sPossibleErrCause)
        ElseIf bPromptClose OrElse bPromptRetry Then
            Dim sQuestion$ = ""
            If bPromptRetry Then
                msgbs = msgbs Or MsgBoxStyle.RetryCancel
                sQuestion = vbLf & "Retry ?"
            End If
            If bReadOnly Then
                userResponse = MsgBox("Please close the file :" & vbLf & sFilePath & _
                    sQuestion, msgbs, m_sMsgTitle)
            Else
                userResponse = MsgBox("The file can't be written :" & vbLf & sFilePath & vbLf & _
                    "Please close it as the case may be, or change it's attributes," & vbLf & _
                    "or its permissions." & sQuestion, msgbs, m_sMsgTitle)
            End If
        End If
    End Try

    If bLocked And userResponse = MsgBoxResult.Retry Then GoTo Retry
    Return bLocked

End Function

Public Function bDeleteFile(sFilePath$, Optional bPromptErr As Boolean = False) As Boolean

    If Not bFileExists(sFilePath) Then Return True

    If bFileLocked(sFilePath, bPromptClose:=bPromptErr, bPromptRetry:=bPromptErr) Then _
        Return False

    Try
        IO.File.Delete(sFilePath)
        Return True

    Catch ex As Exception
        If bPromptErr Then _
            ShowErrorMsg(ex, "Can't delete file :" & vbLf & sFilePath, sPossibleErrCause)
        Return False
    End Try

End Function

Public Function bWriteFile(sFilePath$, sbContenu As StringBuilder, _
    Optional bDefautEncoding As Boolean = True, _
    Optional encode As Encoding = Nothing, _
    Optional bPromptErr As Boolean = True, _
    Optional ByRef sMsgErr$ = "") As Boolean

    If Not bDeleteFile(sFilePath, bPromptErr:=True) Then Return False

    Dim sw As IO.StreamWriter = Nothing
    Try
        If bDefautEncoding Then encode = Encoding.Default
        sw = New IO.StreamWriter(sFilePath, append:=False, Encoding:=encode)
        sw.Write(sbContenu.ToString())
        sw.Close()
        Return True
    Catch ex As Exception
        Dim sMsg$ = "Can't write file :" & vbCrLf & _
            sFilePath & vbCrLf & sPossibleErrCause
        sMsgErr = sMsg & vbCrLf & ex.Message
        If bPromptErr Then ShowErrorMsg(ex, "bWriteFile", sMsg)
        Return False
    Finally
        If Not IsNothing(sw) Then sw.Close()
    End Try

End Function

#End Region

End Module
