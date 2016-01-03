
' DBReport : DataBase Reporting tool for DataBase Administrator
' -------------------------------------------------------------
' Documentation : DBReport.html
' http://patrice.dargenton.free.fr/CodesSources/DBReport.html
' http://patrice.dargenton.free.fr/CodesSources/DBReport.vbproj.html
' Version 1.01 - 03/01/2016
' By Patrice Dargenton : mailto:patrice.dargenton@free.fr
' http://patrice.dargenton.free.fr/index.html
' http://patrice.dargenton.free.fr/CodesSources/index.html
' ------------------------------------------------------------------

' Naming convention :
' -----------------
' b for Boolean (True or False)
' i for Integer : %
' l for Long : &
' r for Real number (Single!, Double# ou Decimal : D)
' s for String : $
' c for Char or Byte
' d for Date
' u for Unsigned (positif integer)
' a for Array : ()
' o for Object
' m_ for member variable of a class or of a form (but not for constants)
' frm for Form
' cls for Class
' mod for Module
' ...
' ------------------------------------

' File frmDBReport.vb : Main form
' ----------------

Imports System.Text ' for StringBuilder

Public Class frmDBReport

#Region "Settings"

' In release mode, do not save password in config file, it may not be secure in all situations
Private Const bSavePassWord As Boolean = Not bRelease

Private Const bSortColumns As Boolean = False
Private Const bSortIndex As Boolean = True ' To make the comparison easier
Private Const bDisplayFieldType As Boolean = True
Private Const bDisplayDefaultValue As Boolean = True
Private Const bDisplayLinkName As Boolean = False
Private Const bSortLink As Boolean = True ' To make the comparison easier

#End Region

Private Const sMsgDBProvider$ = "Name of the database provider installed in the DotNet Framework (e.g. 'MySql.Data.MySqlClient' if mysql-connector-net-6.8.3.msi is installed)"
Private Const sMsgDBServer$ = "Name of the server (e.g. 'localhost' or '127.0.0.1')"
Private Const sMsgDBName$ = "Name of the database for which you want to export the structure"
Private Const sMsgUserName$ = "Login name (e.g. 'root', a registered user that can view the database structure)"
Private Const sMsgUserPassword$ = "Login password for the selected user (leave blank if no password is set for this user)"
Private Const sMsgDBReport$ = "Click 'DB report' to create the database report"

Private WithEvents m_delegMsg As New clsDelegMsg

Private Sub frmRapportBD_Load(sender As Object, e As EventArgs) Handles Me.Load

    SetMsgTitle(sMsgTitle)

    Dim sTxt$ = sMsgTitle & " " & sAppVersion & " (" & sAppDate & ")"
    If bDebug Then sTxt &= " - Debug"
    'If is64BitProcess() Then sTxt &= " - 64 bits" Else sTxt &= " - 32 bits"
    If Not is64BitProcess() Then sTxt &= " - 32 bits"
    Me.Text = sTxt

    SaveAndRestoreSettings(bSave:=False)

    Me.cmdDBReport.Select()

    Me.ToolTip1.SetToolTip(Me.tbDBProvider, sMsgDBProvider)
    Me.ToolTip1.SetToolTip(Me.tbDBServer, sMsgDBServer)
    Me.ToolTip1.SetToolTip(Me.tbDBName, sMsgDBName)
    Me.ToolTip1.SetToolTip(Me.tbUserName, sMsgUserName)
    Me.ToolTip1.SetToolTip(Me.tbUserPassword, sMsgUserPassword)

    If bDebug Then
        'Me.tbDBProvider.Text = "MySql.Data.MySqlClient"
        'Me.tbDBServer.Text = "localhost"
        Me.tbDBName.Text = "northwind"
        'Me.tbUserName.Text = "root"
        'Me.tbUserPassword.Text = ""
    End If

End Sub

Private Sub frmDBReport_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    SaveAndRestoreSettings(bSave:=True)
End Sub

Private Sub SaveAndRestoreSettings(bSave As Boolean)

    If bSave Then
        My.Settings.DBProvider = Me.tbDBProvider.Text
        My.Settings.DBServer = Me.tbDBServer.Text
        My.Settings.DBName = Me.tbDBName.Text
        My.Settings.UserName = Me.tbUserName.Text
        If bSavePassWord Then _
            My.Settings.UserPassword = Me.tbUserPassword.Text
    Else
        Me.tbDBProvider.Text = My.Settings.DBProvider
        Me.tbDBServer.Text = My.Settings.DBServer
        Me.tbDBName.Text = My.Settings.DBName
        Me.tbUserName.Text = My.Settings.UserName
        Me.tbUserPassword.Text = My.Settings.UserPassword
    End If

End Sub

Private Sub ShowLongMessage(sMsg$)
    Me.lblInfo.Text = sMsg
    'Me.ShowMessage(sMsg)
End Sub

Private Sub ShowMessage(sMsg$)

    Me.ToolStripStatusLabel1.Text = sMsg

    TruncateChildTextAccordingToControlWidth(Me.ToolStripStatusLabel1, Me, appendEllipsis:=True)

    Application.DoEvents()

End Sub

Private Sub ShowMessageDeleg(sender As Object, e As clsMsgEventArgs) _
    Handles m_delegMsg.EvShowMessage
    Me.ShowMessage(e.sMessage)
End Sub

Private Sub ShowLongMessageDeleg(sender As Object, e As clsMsgEventArgs) _
    Handles m_delegMsg.EvShowLongMessage
    Me.ShowLongMessage(e.sMessage)
End Sub

Private Sub cmdDBReport_Click(sender As Object, e As EventArgs) Handles cmdDBReport.Click
    DBReport()
End Sub

Private Sub DBReport()

    Dim sServer$ = Me.tbDBServer.Text
    Dim sDBName$ = Me.tbDBName.Text
    Dim sUserLogin$ = Me.tbUserName.Text
    Dim sPW$ = Me.tbUserPassword.Text

    Dim sDBProvider$ = Me.tbDBProvider.Text
    Dim sConnection$ = _
        "server=" & sServer & "; " & _
        "userid=" & sUserLogin & "; " & _
        "password=" & sPW & "; " & _
        "database=" & sDBName & ";"

    Me.cmdDBReport.Enabled = False
    m_delegMsg.m_bCancel = False
    If Not bFileExists(Application.StartupPath & "\DatabaseSchemaReader.dll", _
        bPrompt:=True) Then GoTo Fin

    Dim sb = New StringBuilder()
    If bCreateDBReport(m_delegMsg, sb, sConnection, sDBProvider, _
        bDisplayFieldType, bDisplayDefaultValue, bDisplayLinkName, _
        bSortColumns, bSortIndex, bSortLink, sUserLogin, sServer, sDBName, _
        sMsgDBOff, sMsgCompoMySQLNotInst) Then
        Dim sPath$ = Application.StartupPath & "\DBReport_" & sDBName & ".txt"
        If Not bWriteFile(sPath, sb) Then GoTo Fin
        LetOpenFile(sPath)
    End If

Fin:
    Me.cmdDBReport.Enabled = True

End Sub

Private Sub lblDBProvider_MouseHover(sender As Object, e As EventArgs) _
    Handles lblDBProvider.MouseHover, tbDBProvider.MouseHover, tbDBProvider.Enter
    ShowLongMessage(sMsgDBProvider)
End Sub
Private Sub lblDBServer_MouseHover(sender As Object, e As EventArgs) _
    Handles lblDBServer.MouseHover, tbDBServer.MouseHover, tbDBServer.Enter
    ShowLongMessage(sMsgDBServer)
End Sub
Private Sub lblDBName_MouseHover(sender As Object, e As EventArgs) _
    Handles lblDBName.MouseHover, tbDBName.MouseHover, tbDBName.Enter
    ShowLongMessage(sMsgDBName)
End Sub
Private Sub lblUserName_MouseHover(sender As Object, e As EventArgs) _
    Handles lblUserName.MouseHover, tbUserName.MouseHover, tbUserName.Enter
    ShowLongMessage(sMsgUserName)
End Sub
Private Sub lblUserPW_MouseHover(sender As Object, e As EventArgs) _
    Handles lblUserPW.MouseHover, tbUserPassword.MouseHover, tbUserPassword.Enter
    ShowLongMessage(sMsgUserPassword)
End Sub

Private Sub cmdDBReport_MouseHover(sender As Object, e As EventArgs) _
    Handles cmdDBReport.MouseHover, cmdDBReport.GotFocus
    ShowLongMessage(sMsgDBReport)
End Sub

End Class
