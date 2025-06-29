
' File clsShowMsg.vb : Managing class for messages displayed by the delegate
' ------------------

Public Class clsMsgEventArgs : Inherits EventArgs
    Private m_sMsg$ = ""
    Public Sub New(sMsg$)
        If sMsg Is Nothing Then sMsg = ""
        Me.m_sMsg = sMsg
    End Sub
    Public ReadOnly Property sMessage$()
        Get
            Return Me.m_sMsg
        End Get
    End Property
End Class

Public Class clsDelegMsg

    ' Managing class for messages displayed by the delegate

    Public Delegate Sub ShowMessageDelegate(sender As Object, e As clsMsgEventArgs)
    Public Event EvShowMessage As ShowMessageDelegate
    Public Event EvShowLongMessage As ShowMessageDelegate

    Public m_bCancel As Boolean

    Public Sub New()
    End Sub

    Public Sub ShowMsg(sMsg$)
        Dim e As New clsMsgEventArgs(sMsg)
        RaiseEvent EvShowMessage(Me, e)
    End Sub
    Public Sub ShowLongMsg(sMsg$)
        Dim e As New clsMsgEventArgs(sMsg)
        RaiseEvent EvShowLongMessage(Me, e)
    End Sub

End Class