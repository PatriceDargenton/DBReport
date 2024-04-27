
' File modConst.vb
' ----------------

Module _modConst

    Public Const sAppDate$ = "27/04/2024" '1.10:"10/04/2024" 1.09:"15/04/2023" '1.08:"01/10/2022" '1.07:"04/03/2022" '1.06:"28/02/2022" 1.05:"05/03/2017" '1.04:"23/10/2016" '"18/09/2016"

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