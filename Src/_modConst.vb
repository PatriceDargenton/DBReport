
' File modConst.vb
' ----------------

Module _modConst

    Public Const sAppDate$ = "17/05/2024"

#If DEBUG Then
    Public Const bDebug As Boolean = True
    Public Const bRelease As Boolean = False
#Else
    Public Const bDebug As Boolean = False
    Public Const bRelease As Boolean = True
#End If

    Public ReadOnly sAppVersion$ =
        My.Application.Info.Version.Major & "." &
        My.Application.Info.Version.Minor & My.Application.Info.Version.Build

    Public ReadOnly sAppName$ = My.Application.Info.Title
    Public ReadOnly sMsgTitle$ = sAppName

End Module