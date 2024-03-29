﻿
' DBReport : A DataBase structure Reporting tool for database administrators
' --------------------------------------------------------------------------
' Documentation : DBReport.html
' http://patrice.dargenton.free.fr/CodesSources/DBReport.html
' http://patrice.dargenton.free.fr/CodesSources/DBReport.vbproj.html
' Version 1.09 - 15/04/2023
' By Patrice Dargenton : mailto:patrice.dargenton@free.fr
' http://patrice.dargenton.free.fr/index.html
' http://patrice.dargenton.free.fr/CodesSources/index.html
' --------------------------------------------------------------------------

' Naming convention :
' -----------------
' b for Boolean (True or False)
' i for Integer : %
' l for Long : &
' r for Real number (Single!, Double# or Decimal : D)
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
' -----------------

' File frmDBReport.vb : Main form
' -------------------

Imports System.Text ' for StringBuilder

Public Class frmDBReport

#Region "Settings"

    ' In release mode, do not save password in config file, it may not be secure in all situations
    Private Const bSavePassWord As Boolean = Not bRelease

    Private Const bSortColumnsDef As Boolean = False
    Private Const bSortIndexesDef As Boolean = True ' To make the comparison easier
    Private Const bDisplayFieldDescriptionDef As Boolean = True
    Private Const bDisplayFieldTypeDef As Boolean = True
    Private Const bDisplayDefaultValueDef As Boolean = True
    Private Const bDisplayLinkNameDef As Boolean = False
    Private Const bSortLinksDef As Boolean = True ' To make the comparison easier
    Private Const bAlertNotNullableDef As Boolean = True
    Private Const bDisplayTableEngineDef As Boolean = True
    Private Const bDisplayCollationDef As Boolean = True

#End Region

    Private Const sMsgDBProvider$ =
        "Name of the database provider installed in the DotNet Framework" &
        " (e.g. 'MySql.Data.MySqlClient' if mysql-connector-net-6.9.x.msi is used)"
    Private Const sMsgDBServer$ = "Name of the server (e.g. 'localhost' or '127.0.0.1')"
    Private Const sMsgDBName$ = "Name of the database for which you want to export the structure"
    Private Const sMsgUserName$ =
        "Login name (e.g. 'root', a registered user that can view the database structure)"
    Private Const sMsgUserPassword$ =
        "Login password for the selected user (leave blank if no password is set for this user)"
    Private Const sMsgDBReport$ = "Click 'DB report' to create the database report"
    Private Const sMsgResetSettings$ = "Click to restore default display settings of the database report"
    Private Const sMsgSortColumns$ = "Sort columns of each table"
    Private Const sMsgSortIndexes$ =
        "Sort indexes of each table (to make the database structure comparison easier)"
    Private Const sMsgSortLinks$ =
        "Sort links between tables (to make the database structure comparison easier)"
    Private Const sMsgDisplayFieldDefaultValue$ = "Display default value of each field"
    Private Const sMsgDisplayFieldType$ = "Display field type of each field"
    Private Const sMsgDisplayLinkName$ = "Display the name of links between two tables"
    Private Const sMsgDisplayDescription$ = "Display the description of tables and fields, if available"
    Private Const sMsgAlertNotNullable$ = "Alert about non-nullable field risks"
    Private Const sMsgDisplayTableEngine$ = "Display the table engine for a MySql database (MyISAM, InnoDB, ...)"
    Private Const sMsgDisplayCollation$ = "Display the collation for a MySql database (utf8_general_ci, ...)"

    Private WithEvents m_delegMsg As New clsDelegMsg

    Private Sub frmRapportBD_Load(sender As Object, e As EventArgs) Handles Me.Load

        SetMsgTitle(sMsgTitle)

        Dim sTxt$ = sMsgTitle & " " & sAppVersion & " (" & sAppDate & ")"
        If bDebug Then sTxt &= " - Debug"
        'If is64BitProcess() Then sTxt &= " - 64 bits" Else sTxt &= " - 32 bits"
        If Not is64BitProcess() Then sTxt &= " - 32 bits"
        Me.Text = sTxt

        ResetDisplaySettings()
        SaveAndRestoreSettings(bSave:=False)

        Me.cmdDBReport.Select()

        Me.ToolTip1.SetToolTip(Me.tbDBProvider, sMsgDBProvider)
        Me.ToolTip1.SetToolTip(Me.lblDBProvider, sMsgDBProvider)
        Me.ToolTip1.SetToolTip(Me.tbDBServer, sMsgDBServer)
        Me.ToolTip1.SetToolTip(Me.lblDBServer, sMsgDBServer)
        Me.ToolTip1.SetToolTip(Me.tbDBName, sMsgDBName)
        Me.ToolTip1.SetToolTip(Me.lblDBName, sMsgDBName)
        Me.ToolTip1.SetToolTip(Me.tbUserName, sMsgUserName)
        Me.ToolTip1.SetToolTip(Me.lblUserName, sMsgUserName)
        Me.ToolTip1.SetToolTip(Me.tbUserPassword, sMsgUserPassword)
        Me.ToolTip1.SetToolTip(Me.lblUserPassword, sMsgUserPassword)
        Me.ToolTip1.SetToolTip(Me.cmdDBReport, sMsgDBReport)
        Me.ToolTip1.SetToolTip(Me.cmdResetSettings, sMsgResetSettings)
        Me.ToolTip1.SetToolTip(Me.chkAlertNotNullable, sMsgAlertNotNullable)
        Me.ToolTip1.SetToolTip(Me.chkDisplayDescription, sMsgDisplayDescription)
        Me.ToolTip1.SetToolTip(Me.chkDisplayFieldDefaultValue, sMsgDisplayFieldDefaultValue)
        Me.ToolTip1.SetToolTip(Me.chkDisplayFieldType, sMsgDisplayFieldType)
        Me.ToolTip1.SetToolTip(Me.chkDisplayLinkName, sMsgDisplayLinkName)
        Me.ToolTip1.SetToolTip(Me.chkSortColumns, sMsgSortColumns)
        Me.ToolTip1.SetToolTip(Me.chkSortIndexes, sMsgSortIndexes)
        Me.ToolTip1.SetToolTip(Me.chkSortLinks, sMsgSortLinks)
        Me.ToolTip1.SetToolTip(Me.chkDisplayTableEngine, sMsgDisplayTableEngine)
        Me.ToolTip1.SetToolTip(Me.chkDisplayCollation, sMsgDisplayCollation)

        If bDebug Then
            Me.tbDBProvider.Text = sMySqlClient
            Me.tbDBServer.Text = "localhost"
            Me.tbDBName.Text = "northwind"
            Me.chkAlertNotNullable.Checked = False ' northwind
            Me.tbUserName.Text = "root"
            Me.tbUserPassword.Text = ""
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

            My.Settings.DisplayTableAndFieldDescription = Me.chkDisplayDescription.Checked
            My.Settings.DisplayFieldType = Me.chkDisplayFieldType.Checked
            My.Settings.DisplayFieldDefaultValue = Me.chkDisplayFieldDefaultValue.Checked
            My.Settings.DisplayLinkName = Me.chkDisplayLinkName.Checked
            My.Settings.SortColumns = Me.chkSortColumns.Checked
            My.Settings.SortIndexes = Me.chkSortIndexes.Checked
            My.Settings.SortLinks = Me.chkSortLinks.Checked
            My.Settings.AlertNotNullable = Me.chkAlertNotNullable.Checked
            My.Settings.MySqlDisplayTableEngine = Me.chkDisplayTableEngine.Checked
            My.Settings.MySqlDisplayCollation = Me.chkDisplayCollation.Checked

        Else

            Me.tbDBProvider.Text = My.Settings.DBProvider
            Me.tbDBServer.Text = My.Settings.DBServer
            Me.tbDBName.Text = My.Settings.DBName
            Me.tbUserName.Text = My.Settings.UserName
            Me.tbUserPassword.Text = My.Settings.UserPassword

            Me.chkDisplayDescription.Checked = My.Settings.DisplayTableAndFieldDescription
            Me.chkDisplayFieldType.Checked = My.Settings.DisplayFieldType
            Me.chkDisplayFieldDefaultValue.Checked = My.Settings.DisplayFieldDefaultValue
            Me.chkDisplayLinkName.Checked = My.Settings.DisplayLinkName
            Me.chkSortColumns.Checked = My.Settings.SortColumns
            Me.chkSortIndexes.Checked = My.Settings.SortIndexes
            Me.chkSortLinks.Checked = My.Settings.SortLinks
            Me.chkAlertNotNullable.Checked = My.Settings.AlertNotNullable
            Me.chkDisplayTableEngine.Checked = My.Settings.MySqlDisplayTableEngine
            Me.chkDisplayCollation.Checked = My.Settings.MySqlDisplayCollation

        End If

    End Sub

    Private Sub cmdResetConfig_Click(sender As Object, e As EventArgs) Handles cmdResetSettings.Click
        ResetDisplaySettings()
    End Sub

    Private Sub ResetDisplaySettings()
        Me.chkDisplayDescription.Checked = bDisplayFieldDescriptionDef
        Me.chkDisplayFieldType.Checked = bDisplayFieldTypeDef
        Me.chkDisplayFieldDefaultValue.Checked = bDisplayDefaultValueDef
        Me.chkDisplayLinkName.Checked = bDisplayLinkNameDef ' False
        Me.chkSortColumns.Checked = bSortColumnsDef ' False
        Me.chkSortIndexes.Checked = bSortIndexesDef
        Me.chkSortLinks.Checked = bSortLinksDef
        Me.chkAlertNotNullable.Checked = bAlertNotNullableDef
        Me.chkDisplayTableEngine.Checked = bDisplayTableEngineDef
        Me.chkDisplayCollation.Checked = bDisplayCollationDef
    End Sub

    Private Sub Activation(bActivate As Boolean)

        Me.cmdCancel.Enabled = Not bActivate

        Me.cmdDBReport.Enabled = bActivate
        Me.cmdResetSettings.Enabled = bActivate
        Me.tbDBProvider.Enabled = bActivate
        Me.tbDBServer.Enabled = bActivate
        Me.tbDBName.Enabled = bActivate
        Me.tbUserName.Enabled = bActivate
        Me.tbUserPassword.Enabled = bActivate

        Me.chkDisplayDescription.Enabled = bActivate
        Me.chkDisplayFieldType.Enabled = bActivate
        Me.chkDisplayFieldDefaultValue.Enabled = bActivate
        Me.chkDisplayLinkName.Enabled = bActivate
        Me.chkSortColumns.Enabled = bActivate
        Me.chkSortIndexes.Enabled = bActivate
        Me.chkSortLinks.Enabled = bActivate
        Me.chkAlertNotNullable.Enabled = bActivate
        Me.chkDisplayTableEngine.Enabled = bActivate
        Me.chkDisplayCollation.Enabled = bActivate

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
        Dim sConnection$ =
            "server=" & sServer & "; " &
            "userid=" & sUserLogin & "; " &
            "password=" & sPW & "; " &
            "database=" & sDBName & ";"

        Activation(bActivate:=False)
        m_delegMsg.m_bCancel = False
        If Not bFileExists(Application.StartupPath & "\DatabaseSchemaReader.dll", bPrompt:=True) Then GoTo Fin

        ' 15/04/2023 No more needed, using NuGet MySqlConnector 2.2.5 instead of mysql-connector-net-6.9.x.msi
        'If sDBProvider = sMySqlClient Then
        '    If Not bFileExists(Application.StartupPath & "\MySql.Data.dll", bPrompt:=True) Then GoTo Fin
        'End If

        Dim sb = New StringBuilder()

        Dim prm As New clsPrmDBR
        prm.sConnection = sConnection
        prm.sDBProvider = sDBProvider
        prm.sDBName = sDBName
        prm.sServer = sServer
        prm.sUserLogin = sUserLogin
        prm.sDBReportVersion = sAppVersion ' 23/10/2016

        prm.bDisplayTableAndFieldDescription = Me.chkDisplayDescription.Checked
        prm.bDisplayFieldDefaultValue = Me.chkDisplayFieldDefaultValue.Checked
        prm.bDisplayFieldType = Me.chkDisplayFieldType.Checked
        prm.bDisplayLinkName = Me.chkDisplayLinkName.Checked
        prm.bSortColumns = Me.chkSortColumns.Checked
        prm.bSortIndexes = Me.chkSortIndexes.Checked
        prm.bSortLinks = Me.chkSortLinks.Checked
        prm.bAlertNotNullable = Me.chkAlertNotNullable.Checked

        prm.sForeignKeyDeleteRuleDef = My.Settings.ForeignKeyDeleteRule ' 05/03/2017 RESTRICT
        prm.sForeignKeyUpdateRuleDef = My.Settings.ForeignKeyUpdateRule ' 05/03/2017 RESTRICT

        ' 05/03/2017

        'prm.mySqlprm.sSQLModeDef = "STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION"
        'prm.mySqlprm.sInnodbStrictModeDef = "ON"
        'prm.mySqlprm.iTimeOutMaxDef = 99999
        'prm.mySqlprm.iNetReadTimeoutSecDef = iNoDefaultTimeOut
        'prm.mySqlprm.iNetWriteTimeoutSecDef = iNoDefaultTimeOut
        'prm.mySqlprm.sTableEngineDef = "InnoDB"
        'prm.mySqlprm.sServerCollationDef = "utf8_general_ci"
        'prm.mySqlprm.sDatabaseCollationDef = "utf8_general_ci"
        'prm.mySqlprm.sTableCollationDef = "utf8_general_ci"
        'prm.mySqlprm.sColumnCollationDef = "utf8_general_ci"

        prm.mySqlprm.sSQLModeDef = My.Settings.MySqlSQLMode
        prm.mySqlprm.sInnodbStrictModeDef = My.Settings.MySqlInnodbStrictMode
        prm.mySqlprm.iTimeOutMaxDef = My.Settings.MySqlTimeOutMaxSec
        prm.mySqlprm.iNetReadTimeoutSecDef = My.Settings.MySqlNetReadTimeoutSec
        prm.mySqlprm.iNetWriteTimeoutSecDef = My.Settings.MySqlNetWriteTimeoutSec
        prm.mySqlprm.sTableEngineDef = My.Settings.MySqlTableEngine
        prm.mySqlprm.sServerCollationDef = My.Settings.MySqlServerCollation
        prm.mySqlprm.sDatabaseCollationDef = My.Settings.MySqlDatabaseCollation
        prm.mySqlprm.sTableCollationDef = My.Settings.MySqlTableCollation
        prm.mySqlprm.sColumnCollationDef = My.Settings.MySqlColumnCollation

        prm.mySqlprm.bDisplayTableEngine = Me.chkDisplayTableEngine.Checked
        prm.mySqlprm.bDisplayCollation = Me.chkDisplayCollation.Checked

        prm.mySqlprm.sforeign_key_checksDef = "ON" ' 04/03/2022

        If bDebug Then
            ' For Norhwind :
            prm.mySqlprm.sColumnCollationDef = "utf8_unicode_ci"
            'prm.mySqlprm.sTableCollationDef = "utf8_general_ci"
            'prm.mySqlprm.sDatabaseCollationDef = "utf8_general_ci"
            'prm.mySqlprm.sServerCollationDef = "utf8_general_ci"
            'prm.mySqlprm.sTableEngineDef = "InnoDB"
        End If

        If bCreateDBReport(prm, m_delegMsg, sMsgDBOff, sMsgCompoMySQLNotInst, sb) Then
            Dim sPath$ = Application.StartupPath & "\DBReport_" & sDBName & ".txt"
            If Not bWriteFile(sPath, sb) Then GoTo Fin
            LetOpenFile(sPath)
        Else
            If Me.m_delegMsg.m_bCancel Then
                ShowLongMessage("")
                ShowMessage("Cancelled by user.")
            End If
        End If

Fin:
        Activation(bActivate:=True)

    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Me.m_delegMsg.m_bCancel = True
    End Sub

    Private Sub tbDBProvider_Enter(sender As Object, e As EventArgs) Handles tbDBProvider.Enter
        ShowLongMessage(sMsgDBProvider)
    End Sub
    Private Sub tbDBServer_Enter(sender As Object, e As EventArgs) Handles tbDBServer.Enter
        ShowLongMessage(sMsgDBServer)
    End Sub
    Private Sub tbDBName_Enter(sender As Object, e As EventArgs) Handles tbDBName.Enter
        ShowLongMessage(sMsgDBName)
    End Sub
    Private Sub tbUserName_Enter(sender As Object, e As EventArgs) Handles tbUserName.Enter
        ShowLongMessage(sMsgUserName)
    End Sub
    Private Sub tbUserPassword_Enter(sender As Object, e As EventArgs) Handles tbUserPassword.Enter
        ShowLongMessage(sMsgUserPassword)
    End Sub
    Private Sub cmdDBReport_Enter(sender As Object, e As EventArgs) Handles cmdDBReport.Enter
        ShowLongMessage(sMsgDBReport)
    End Sub
    Private Sub cmdResetSettings_Enter(sender As Object, e As EventArgs) Handles cmdResetSettings.Enter
        ShowLongMessage(sMsgResetSettings)
    End Sub
    Private Sub chkSortColumns_Enter(sender As Object, e As EventArgs) Handles chkSortColumns.Enter
        ShowLongMessage(sMsgSortColumns)
    End Sub
    Private Sub chkSortIndexes_Enter(sender As Object, e As EventArgs) Handles chkSortIndexes.Enter
        ShowLongMessage(sMsgSortIndexes)
    End Sub
    Private Sub chkSortLinks_Enter(sender As Object, e As EventArgs) Handles chkSortLinks.Enter
        ShowLongMessage(sMsgSortLinks)
    End Sub
    Private Sub chkDisplayDefaultValue_Enter(sender As Object, e As EventArgs) Handles chkDisplayFieldDefaultValue.Enter
        ShowLongMessage(sMsgDisplayFieldDefaultValue)
    End Sub
    Private Sub chkDisplayFieldType_Enter(sender As Object, e As EventArgs) Handles chkDisplayFieldType.Enter
        ShowLongMessage(sMsgDisplayFieldType)
    End Sub
    Private Sub chkDisplayLinkName_Enter(sender As Object, e As EventArgs) Handles chkDisplayLinkName.Enter
        ShowLongMessage(sMsgDisplayLinkName)
    End Sub
    Private Sub chkAlertNotNullable_Enter(sender As Object, e As EventArgs) Handles chkAlertNotNullable.Enter
        ShowLongMessage(sMsgAlertNotNullable)
    End Sub
    Private Sub chkDisplayDescription_Enter(sender As Object, e As EventArgs) Handles chkDisplayDescription.Enter
        ShowLongMessage(sMsgDisplayDescription)
    End Sub
    Private Sub chkDisplayTableEngine_Enter(sender As Object, e As EventArgs) Handles chkDisplayTableEngine.Enter
        ShowLongMessage(sMsgDisplayTableEngine)
    End Sub
    Private Sub chkDisplayCollation_Enter(sender As Object, e As EventArgs) Handles chkDisplayCollation.Enter
        ShowLongMessage(sMsgDisplayCollation)
    End Sub

End Class