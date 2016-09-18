
' File modUtil.vb : Utility module
' ---------------

Module modUtil

Public m_sMsgTitle$ = sMsgTitle

Public Sub SetMsgTitle(sTitreMsg$)
    m_sMsgTitle = sTitreMsg
End Sub

Public Sub ShowErrorMsg(ByRef ex As Exception, _
    Optional sFctTitle$ = "", Optional sInfo$ = "", _
    Optional sDetailedErrMsg$ = "", _
    Optional bCopyErrMsgClipBoard As Boolean = True, _
    Optional ByRef sFinalErrMsg$ = "")

    If Not Cursor.Current.Equals(Cursors.Default) Then Cursor.Current = Cursors.Default

    Dim sMsg$ = ""
    If sFctTitle <> "" Then sMsg = "Function : " & sFctTitle
    If sInfo <> "" Then sMsg &= vbCrLf & sInfo
    If sDetailedErrMsg <> "" Then sMsg &= vbCrLf & sDetailedErrMsg
    If ex.Message <> "" Then
        sMsg &= vbCrLf & ex.Message.Trim
        If Not IsNothing(ex.InnerException) Then _
            sMsg &= vbCrLf & ex.InnerException.Message
    End If
    If bCopyErrMsgClipBoard Then
        CopyToClipBoard(sMsg)
        sMsg &= vbCrLf & "(this error message has been copied into the clipboard)"
    End If

    sFinalErrMsg = sMsg

    MsgBox(sMsg, MsgBoxStyle.Critical, m_sMsgTitle)

End Sub

Public Sub CopyToClipBoard(sInfo$)

    ' Copier des informations dans le presse-papier de Windows
    ' (elles resteront jusqu'à ce que l'application soit fermée)
    ' Copy text into Windows clipboard

    Try
        Dim dataObj As New DataObject
        dataObj.SetData(DataFormats.Text, sInfo)
        Clipboard.SetDataObject(dataObj)
    Catch ex As Exception
        ' Le presse-papier peut être indisponible
        ShowErrorMsg(ex, "CopyToClipBoard", bCopyErrMsgClipBoard:=False)
    End Try

End Sub

Public Function is64BitProcess() As Boolean
    Return (IntPtr.Size = 8)
End Function

''' <summary>
''' If a child ToolStripStatusLabel is wider than it's parent then this method will attempt to
'''  make the child's text fit inside of the parent's boundaries. An ellipsis can be appended
'''  at the end of the text to indicate that it has been truncated to fit.
''' </summary>
''' <param name="child">Child ToolStripStatusLabel</param>
''' <param name="parent">Parent control where the ToolStripStatusLabel resides</param>
''' <param name="appendEllipsis">Append an "..." to the end of the truncated text</param>
Public Sub TruncateChildTextAccordingToControlWidth(child As ToolStripStatusLabel, _
    parent As Control, appendEllipsis As Boolean)

    ' http://stackoverflow.com/questions/5708375/how-can-i-determine-how-much-of-a-string-will-fit-in-a-certain-width

    ' If the child's width is greater than that of the parent's
    Const rMarge! = 0.1
    'If child.Size.Width >= parent.Size.Width * 0.9 Then
    If child.Size.Width >= parent.Size.Width * (1 - rMarge) Then

        ' Get the number of times that the child is oversized [child/parent]
        Dim decOverSized As Decimal = CDec(child.Size.Width) / CDec(parent.Size.Width)

        ' Get the new Text length based on the number of times that the child's width is oversized.
        'Dim iNewLength% = CInt(child.Text.Length / (2D * decOverSized))
        Dim iNewLength% = CInt(child.Text.Length / ((1 + rMarge) * decOverSized))

        ' Doubling as a buffer (Magic Number).
        ' If the ellipsis is to be appended
        If appendEllipsis Then
            ' then 3 more characters need to be removed to make room for it.
            iNewLength = iNewLength - 3
        End If

        ' If the new length is negative for whatever reason
        If iNewLength < 0 Then iNewLength = 0 ' Then default it to zero

        ' Truncate the child's Text accordingly
        If child.Text.Length >= iNewLength Then _
            child.Text = child.Text.Substring(0, iNewLength)

        ' If the ellipsis is to be appended
        If appendEllipsis Then child.Text += "..." ' Then do this last

    End If

End Sub

End Module
