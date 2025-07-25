﻿
' File modDBReport.vb
' -------------------

'Imports System.Data.Common ' NuGet MySqlConnector 2.2.5: DbProviderFactories
Imports System.ComponentModel
Imports System.Text ' StringBuilder
Imports MySqlConnector
Imports Oracle.ManagedDataAccess.Client ' OracleConnection

Namespace DBReport

    Public Module modDBReport

        Public Const sMsgError$ = "Error !"
        Public Const sMsgDone$ = "Done."
        Public Const sMsgDBOff$ = "Could not connect to database !" & vbCrLf &
            "Possible cause : the database server has not been started," & vbCrLf &
            " or wrong database name or account used."
        Public Const sMsgCompoMySQLNotInst$ =
            "Possible cause : mysql-connector-net-6.9.x.msi is not installed"

        Private Const iNoDefaultTimeOut% = -1

        Private Const sMsgEmpty$ = "[Empty]"

        Private m_dTimeStart As Date
        Private m_delegMsg As clsDelegMsg
        Private WithEvents m_dbReader As DatabaseSchemaReader.DatabaseReader
        Private Sub ShowMsg(sMsg$)
            m_delegMsg.ShowMsg(sMsg)
        End Sub
        Private Sub ShowLongMsg(sMsg$)
            m_delegMsg.ShowLongMsg(sMsg)
        End Sub
        Private Sub ShowMessageDeleg(sender As Object, e As DatabaseSchemaReader.ReaderEventArgs) _
                Handles m_dbReader.ReaderProgress

            If bDebug Then
                Debug.WriteLine("")
                Debug.WriteLine("")
                Debug.WriteLine("Reading database schema : " & e.ProgressType.ToString & ", " &
                e.SchemaObjectType.ToString) ' & ", " & e.Name & ", " & e.Index & ", " & e.Count)
                Dim dTimeEnd = Now()
                Dim ts = dTimeEnd - m_dTimeStart
                Const sDateTimeFormat = "dd\/MM\/yyyy HH:mm:ss"
                Dim sTime$ = m_dTimeStart.ToString(sDateTimeFormat) & " -> " &
                dTimeEnd.ToString(sDateTimeFormat) & " : " & sDisplayTime(ts.TotalSeconds)
                Debug.WriteLine(sTime)
            End If
            ShowMsg("Reading database schema : " & e.ProgressType.ToString & ", " &
                e.SchemaObjectType.ToString) ' & ", " & e.Name & ", " & e.Index & ", " & e.Count)

        End Sub

#Region "Classes"

        ' See https://github.com/TylerBrinkley/Enums.NET
        Public Enum enumDBProvider

            ''' <summary>
            ''' Unknown
            ''' </summary>
            <Description("Unknown")>
            Unknown

            ''' <summary>
            ''' MySql.Data.MySqlClient (This client for MySql comes from Oracle Corporation)
            ''' </summary>
            <Description("MySql.Data.MySqlClient")>
            MySqlClient

            ''' <summary>
            ''' MySqlConnector (MariaDb client)
            ''' </summary>
            <Description("MySqlConnector")>
            MariaDbClient

            ''' <summary>
            ''' System.Data.OracleClient
            ''' </summary>
            <Description("System.Data.OracleClient")>
            OracleClient

            ''' <summary>
            ''' System.Data.SQLite
            ''' </summary>
            <Description("System.Data.SQLite")>
            SQLiteClient

            ''' <summary>
            ''' Npgsql is the open source .NET data provider for PostgreSQL
            ''' </summary>
            <Description("Npgsql")>
            PostgreSQL

            ''' <summary>
            ''' Default: MySql.Data.MySqlClient
            ''' </summary>
            <Description("MySql.Data.MySqlClient")>
            [Default] = MySqlClient

        End Enum

        ' https://en.wikipedia.org/wiki/SQL#SQL_data_types
        Public Enum enumSqlStandardType

            ''' <summary>
            ''' BINARY
            ''' </summary>
            <Description("BINARY")>
            Binary

            ''' <summary>
            ''' Bit (Sqlite) -> BINARY
            ''' </summary>
            <Description("BINARY")>
            Bit

            'Binary varying (VARBINARY)

            ''' <summary>
            ''' SmallInt
            ''' </summary>
            <Description("SMALLINT")>
            SmallInt

            ''' <summary>
            ''' TinyInt (MySql) -> SmallInt
            ''' </summary>
            <Description("SMALLINT")>
            TinyInt

            ''' <summary>
            ''' Int2 (PostgreSql) -> SMALLINT
            ''' </summary>
            <Description("SMALLINT")>
            Int2

            ''' <summary>
            ''' Integer
            ''' </summary>
            <Description("INTEGER")>
            [Integer]

            ''' <summary>
            ''' Int, Int(10) (MySql) -> INTEGER
            ''' </summary>
            <Description("INTEGER")>
            Int

            ''' <summary>
            ''' Int4 (PostgreSql) -> INTEGER
            ''' </summary>
            <Description("INTEGER")>
            Int4

            ''' <summary>
            ''' BigInt
            ''' </summary>
            <Description("BIGINT")>
            BigInt

            ''' <summary>
            ''' Int8 (PostgreSql) -> BIGINT
            ''' </summary>
            <Description("BIGINT")>
            Int8

            ''' <summary>
            ''' FLOAT (Approximate numeric types)
            ''' </summary>
            <Description("FLOAT")>
            Float

            ''' <summary>
            ''' REAL (Approximate numeric types)
            ''' </summary>
            <Description("REAL")>
            Real

            ''' <summary>
            ''' DOUBLE PRECISION (Approximate numeric types)
            ''' </summary>
            <Description("DOUBLE PRECISION")>
            DoublePrecision

            ''' <summary>
            ''' DOUBLE (Approximate numeric types)
            ''' </summary>
            <Description("DOUBLE PRECISION")>
            [Double]

            ''' <summary>
            ''' Float8 (PostgreSql) -> DOUBLE (Approximate numeric types)
            ''' </summary>
            <Description("DOUBLE PRECISION")>
            Float8

            'DECIMAL(p, s) = NUMERIC(p, s)

            ''' <summary>
            ''' Money (Sqlite) -> DECIMAL
            ''' </summary>
            <Description("DECIMAL(18, 4)")>
            Money

            ''' <summary>
            ''' Currency (Oracle) -> DECIMAL
            ''' </summary>
            <Description("DECIMAL(18, 4)")>
            Currency

            'Exact numeric types (NUMERIC, DECIMAL, SMALLINT, INTEGER, BIGINT)
            'Decimal floating-point type (DECFLOAT)

            ''' <summary>
            ''' Binary large object (BLOB)
            ''' </summary>
            <Description("BLOB")>
            Blob

            ''' <summary>
            ''' Image (Sqlite) -> BLOB
            ''' </summary>
            <Description("BLOB")>
            Image

            'Character (CHAR)

            ''' <summary>
            ''' Text (PostgreSql) -> Character varying VARCHAR
            ''' </summary>
            <Description("VARCHAR")>
            Text

            ''' <summary>
            ''' Character large object (CLOB)
            ''' </summary>
            <Description("CLOB")>
            Clob

            ''' <summary>
            ''' NText (SQLite) -> CLOB
            ''' </summary>
            <Description("CLOB")>
            NText

            ''' <summary>
            ''' LongText (MySQL) -> CLOB
            ''' </summary>
            <Description("CLOB")>
            LongText

            ''' <summary>
            ''' MediumText (MySQL) -> CLOB
            ''' </summary>
            <Description("CLOB")>
            MediumText

            ''' <summary>
            ''' National character (NCHAR)
            ''' </summary>
            <Description("NCHAR")>
            NChar

            ''' <summary>
            ''' National character varying (NCHAR VARYING)
            ''' </summary>
            <Description("NCHAR VARYING")>
            NCharVarying

            ''' <summary>
            ''' National character large object (NCLOB)
            ''' </summary>
            <Description("NCLOB")>
            NClob

            ''' <summary>
            ''' DATETIME (Sqlite) -> TIMESTAMP
            ''' </summary>
            <Description("TIMESTAMP")>
            DateTime

            ''' <summary>
            ''' DATE
            ''' </summary>
            <Description("DATE")>
            [Date]

            ''' <summary>
            ''' TIME (Hour)
            ''' </summary>
            <Description("TIME")>
            Time

        End Enum

        Public Class clsPrmDBR

            Public DBProvider As enumDBProvider ' 21/06/2024
            Public sConnection$, sDBProvider$, sUserLogin$, sServer$, sDBName$, sDBReportVersion$
            Public sUserPassword$, sInstanceName$, sPort$ ' 10/04/2024 Oracle

            Public bDisplayTableAndFieldDescription As Boolean
            Public bDisplayFieldType As Boolean
            Public bDisplayFieldDefaultValue As Boolean
            Public bDisplayLinkName As Boolean
            Public bSortTables As Boolean ' 04/05/2024
            Public bSortColumns As Boolean
            Public bSortIndexes As Boolean
            Public bSortLinks As Boolean
            Public bAlertNotNullable As Boolean
            Public sForeignKeyDeleteRuleDef$ ' 05/03/2017
            Public sForeignKeyUpdateRuleDef$ ' 05/03/2017

            ' 04/05/2024
            Public bDisplayAutonumberAsPrimaryKey As Boolean
            Public bDisplayMultipleIndexName As Boolean
            Public bRenameSQLiteMultipleIndex As Boolean
            Public bDisplaySQLiteSimpleIndexName As Boolean

            Public mySqlprm As New clsPrmMySql ' 05/03/2017
            Public bDisplayMySqlParameters As Boolean ' 04/05/2024
            Public bDisplayLinksBelowEachTable As Boolean ' 31/05/2024
            Public bUseUpperCaseIdentifiers As Boolean ' 10/05/2025
            Public bDisplayDateTime As Boolean ' 29/06/2025
            Public bDisplayStandardSqlType As Boolean ' 19/07/2025

            ''' <summary>
            ''' Ex.: VARCHAR(255) -> VARCHAR
            ''' </summary>
            Public bDisplayFieldLength As Boolean ' 25/07/2025

        End Class

        Public Class clsPrmMySql
            Public bDisplayTableEngine As Boolean
            Public bDisplayCollation As Boolean
            Public sSQLModeDef$
            Public sInnodbStrictModeDef$
            Public iTimeOutMaxDef%
            Public iNetReadTimeoutSecDef%
            Public iNetWriteTimeoutSecDef%
            Public sTableEngineDef$
            Public sServerCollationDef$
            Public sDatabaseCollationDef$
            Public sTableCollationDef$
            Public sColumnCollationDef$
            Public sforeign_key_checksDef$ ' 04/03/2022
        End Class

        ' http://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html
        Public Class enumMySqlPrm

            ' sql_mode controls what SQL syntax MySQL accepts, and determines whether it silently
            '  ignores errors, or validates input syntax and data values. For example, if sql_mode
            '  is empty, implicit conversions can be performed without error (but only with warnings)
            ' Default : STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION
            ' https://dev.mysql.com/doc/refman/5.7/en/sql-mode.html
            Public Const sql_mode$ = "sql_mode"

            ' This is analogous to sql_mode in MySQL, it enables additional error checks for InnoDB tables.
            ' The default value is ON since MySQL 5.7.7
            Public Const innodb_strict_mode$ = "innodb_strict_mode"

            Public Const collation_server$ = "collation_server" ' Default collation for new databases
            Public Const DEFAULT_COLLATION_NAME$ = "DEFAULT_COLLATION_NAME" ' Database default collation

            Public Const net_read_timeout_sec = "net_read_timeout"
            Public Const net_write_timeout_sec = "net_write_timeout"

            Public Const foreign_key_checks$ = "foreign_key_checks" ' 04/03/2022

        End Class

        Public Class clsLink ' Relationship between two tables
            Public Id$ = ""
            Public Name$ = ""
            Public Table$ = ""
            Public Count% = 0
            Public dc As DatabaseSchemaReader.DataSchema.DatabaseConstraint ' (for foreing key)
        End Class

#End Region

        Public Function bCreateDBReport(prm As clsPrmDBR, delegMsg As clsDelegMsg,
                sMsgErr$, sMsgErrPossibleCause$, ByRef sb As StringBuilder) As Boolean

            ' Library used : https://github.com/martinjw/dbschemareader

            Try
                m_delegMsg = delegMsg
                ShowMsg("Connecting to database...")
                If delegMsg.m_bCancel Then Return False

                m_dTimeStart = Now()

                Dim lstMySqlPrm As New List(Of String)
                Dim dicSqlPrm As New Dictionary(Of String, String)
                Dim dicSqlTE As New Dictionary(Of String, String)
                Dim dicSqlTC As New Dictionary(Of String, String)
                Dim dicSqlCC As New Dictionary(Of String, String)
                Dim sMySqlConnectorVersion$ = "" ' 04/05/2024
                Dim bMySql As Boolean = False
                If prm.DBProvider = enumDBProvider.MySqlClient Then bMySql = True
                If prm.DBProvider = enumDBProvider.MariaDbClient Then bMySql = True
                If bMySql AndAlso prm.bDisplayMySqlParameters Then
                    If Not bGetMySqlParameters(prm.sConnection, prm.sDBName, dicSqlPrm, lstMySqlPrm,
                        sMsgErr, sMsgErrPossibleCause) Then Return False
                    GetMySqlTablesCollationAndEngine(prm.sConnection, prm.sDBName, dicSqlTE, dicSqlTC)
                    GetMySqlColumnsCollation(prm.sConnection, prm.sDBName, dicSqlCC)

                    ' 04/05/2024
                    Const sDllMySql$ = "MySqlConnector.dll" '"MySQL.Data.dll"
                    Dim sDllFullPath = Application.StartupPath & "\" & sDllMySql
                    Dim sDllPath = ""
                    If bFileExists(sDllFullPath) Then sDllPath = sDllMySql
                    Dim sVersion$
                    If sDllPath.Length > 0 Then
                        ' For MySQL.Data.dll:
                        'sVersion = System.Reflection.AssemblyName.GetAssemblyName(sDllPath).Version.ToString
                        ' For MySqlConnector.dll:
                        Dim fvi = FileVersionInfo.GetVersionInfo(sDllPath)
                        sVersion = fvi.FileVersion.ToString
                        sMySqlConnectorVersion = sVersion
                    End If
                End If

                If bMySql Then
                    Dim dbConnection As New MySql.Data.MySqlClient.MySqlConnection(prm.sConnection)
                    m_dbReader = New DatabaseSchemaReader.DatabaseReader(dbConnection)
                End If

                ' 10/04/2024 Oracle
                If prm.DBProvider = enumDBProvider.OracleClient Then

                    Dim sLogin$ = prm.sUserLogin
                    Dim sPW$ = prm.sUserPassword
                    Dim sServer$ = prm.sServer
                    Dim sSID = prm.sInstanceName
                    Dim sPort$ = prm.sPort

                    If String.IsNullOrEmpty(sSID) Then
                        ' 31/05/2024 TNS connection mode (Transparent Network Substrate)
                        prm.sConnection =
                            "USER ID=" + sLogin + ";PASSWORD=" + sPW +
                            ";DATA SOURCE=" + sServer
                    Else
                        prm.sConnection =
                            "USER ID=" + sLogin + ";PASSWORD=" + sPW +
                            ";DATA SOURCE=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + sServer +
                            " )(PORT = " + sPort +
                            " )))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = " + sSID + " )))"
                    End If

                    ' This package Oracle.ManagedDataAccess is not available in .Net Core, but in .Net 4
                    Dim dbConnection As New OracleConnection(prm.sConnection)
                    m_dbReader = New DatabaseSchemaReader.DatabaseReader(dbConnection)
                End If

                ' System.Data.SQLite
                If prm.DBProvider = enumDBProvider.SQLiteClient Then
                    Dim sServer$ = prm.sServer
                    prm.sConnection = "Data Source=" & prm.sServer & ";"
                    'Dim sDBProvider$ = prm.sDBProvider
                    'Dim sDBProvider$ = prm.DBProvider.ToDescription()
                    'Dim fact = System.Data.Common.DbProviderFactories.GetFactory(sDBProvider)
                    'Using cnn As System.Data.Common.DbConnection = fact.CreateConnection()
                    '    cnn.ConnectionString = prm.sConnection
                    '    cnn.Open()
                    'End Using
                    Dim dbConnection As New Microsoft.Data.Sqlite.SqliteConnection(prm.sConnection)
                    m_dbReader = New DatabaseSchemaReader.DatabaseReader(dbConnection)
                End If

                ' Npgsql : PostgreSQL
                If prm.DBProvider = enumDBProvider.PostgreSQL Then
                    prm.sConnection = "Host=" & prm.sServer & ";Port=" & prm.sPort &
                        ";Database=" & prm.sDBName &
                        ";Username=" & prm.sUserLogin & ";Password=" & prm.sUserPassword & ";"
                    'prm.sConnection = "Server=" & prm.sServer &
                    '    ";User id=" & prm.sUserLogin & ";Pwd=" & prm.sUserPassword & ";" &
                    '    ";database=" & prm.sDBName & ";Port=" & prm.sPort
                    Dim dbConnection As New Npgsql.NpgsqlConnection(prm.sConnection)
                    m_dbReader = New DatabaseSchemaReader.DatabaseReader(dbConnection,
                    DatabaseSchemaReader.DataSchema.SqlType.PostgreSql)
                End If

                Select Case prm.DBProvider
                    Case enumDBProvider.OracleClient
                    Case enumDBProvider.SQLiteClient
                    Case enumDBProvider.MySqlClient
                    Case enumDBProvider.MariaDbClient
                    Case enumDBProvider.PostgreSQL
                    Case Else
                        MsgBox(".Net core: The database connection must be explicit now",
                            MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, m_sMsgTitle)
                        Return False
                End Select

                'm_dbReader = New DatabaseSchemaReader.DatabaseReader(prm.sConnection, prm.sDBProvider)
                If prm.DBProvider = enumDBProvider.OracleClient Then m_dbReader.Owner = prm.sDBName ' 22/08/2016
                If prm.DBProvider = enumDBProvider.MySqlClient Then m_dbReader.Owner = prm.sDBName ' 19/07/2025

                ShowMsg("Reading database schema...")
                If delegMsg.m_bCancel Then Return False

                Dim schema = m_dbReader.ReadAll
                Const sDateTimeFormat = "dd\/MM\/yyyy HH:mm:ss"
                If bDebug Then
                    Dim dTimeEnd = Now()
                    Dim ts = dTimeEnd - m_dTimeStart
                    Dim sTime$ = m_dTimeStart.ToString(sDateTimeFormat) & " -> " &
                    dTimeEnd.ToString(sDateTimeFormat) & " : " & sDisplayTime(ts.TotalSeconds)
                    Debug.WriteLine(sTime & " : ReadAll")
                End If

                ShowMsg("Building database report...")
                If delegMsg.m_bCancel Then Return False

                sb = New StringBuilder()

                CreateHeader(sb, prm)

                If bMySql AndAlso prm.bDisplayMySqlParameters Then ShowMySqlInfos(sb, prm, lstMySqlPrm, dicSqlPrm, sMySqlConnectorVersion)

                Dim dicTableLinks As New SortDic(Of String, List(Of String))
                CreateLinkReport(sb, prm, schema, dicTableLinks)

                CreateTableReport(sb, prm, schema, bMySql, dicSqlTE, dicSqlTC, dicSqlCC, dicTableLinks)

                If Not prm.bDisplayLinksBelowEachTable Then
                    sb.AppendLine("Links")
                    For Each kvp In dicTableLinks
                        sb.AppendLine()
                        Dim sTableTitle$ = kvp.Key
                        sb.AppendLine(sTableTitle)
                        For Each sLine In kvp.Value
                            sb.AppendLine(sLine)
                        Next
                    Next
                End If

                ShowMsg(sMsgDone)
                ShowLongMsg("")

                If prm.bDisplayDateTime Then
                    Dim dTimeEnd2 = Now()
                    Dim ts2 = dTimeEnd2 - m_dTimeStart
                    Dim sTime2$ = m_dTimeStart.ToString(sDateTimeFormat) & " -> " &
                    dTimeEnd2.ToString(sDateTimeFormat) & " : " & sDisplayTime(ts2.TotalSeconds)
                    If Not prm.bDisplayLinksBelowEachTable Then sb.AppendLine()
                    sb.AppendLine("Report created : " & sTime2)
                End If

                Return True

            Catch ex As Exception
                ShowMsg(sMsgError)
                Dim sFinalErrMsg$ = ""
                ShowErrorMsg(ex, "bCreateDBReport", sMsgErr, sMsgErrPossibleCause, sFinalErrMsg:=sFinalErrMsg)
                ShowLongMsg(sFinalErrMsg)
                Return False

            End Try

        End Function

        Private Sub CreateHeader(sb As StringBuilder, prm As clsPrmDBR)

            sb.AppendLine()
            sb.AppendLine("Database report " & prm.sDBReportVersion) ' 23/10/2016
            sb.AppendLine("--------------------")
            sb.AppendLine()

            Dim bOracleClient = prm.DBProvider = enumDBProvider.OracleClient
            If prm.DBProvider = enumDBProvider.SQLiteClient Then ' 27/04/2024
                If Not String.IsNullOrEmpty(prm.sUserLogin) Then sb.AppendLine("Login    : " & prm.sUserLogin)
                sb.AppendLine("Database : " & prm.sDBName)
            Else
                If Not bOracleClient OrElse
                  (bOracleClient AndAlso prm.sUserLogin <> prm.sDBName) Then ' 31/05/2024
                    sb.AppendLine("Login    : " & prm.sUserLogin)
                End If
                sb.AppendLine("Server   : " & prm.sServer)
                sb.AppendLine("Database : " & prm.sDBName)
            End If

            If bOracleClient AndAlso Not String.IsNullOrEmpty(prm.sInstanceName) Then ' 28/05/2024
                sb.AppendLine("Instance : " & prm.sInstanceName)
                sb.AppendLine("Port     : " & prm.sPort)
            End If

            If prm.bDisplayLinksBelowEachTable <> frmDBReport.bDisplayLinksBelowEachTableDef Then
                If prm.bDisplayLinksBelowEachTable Then
                    sb.AppendLine("Links    : Below each table")
                Else
                    sb.AppendLine("Links    : Second part of the report")
                End If
            End If

            If prm.bSortTables <> frmDBReport.bSortTablesDef Then
                If prm.bSortTables Then
                    sb.AppendLine("Tables   : Sorted")
                Else
                    sb.AppendLine("Tables   : Not sorted")
                End If
            End If

            If prm.bSortColumns <> frmDBReport.bSortColumnsDef Then
                If prm.bSortColumns Then
                    sb.AppendLine("Columns  : Sorted")
                Else
                    sb.AppendLine("Columns  : Not sorted")
                End If
            End If

            If prm.bSortIndexes <> frmDBReport.bSortIndexesDef Then
                If prm.bSortIndexes Then
                    sb.AppendLine("Indexes  : Sorted")
                Else
                    sb.AppendLine("Indexes  : Not sorted")
                End If
            End If

            sb.AppendLine()

        End Sub

        Private Sub ShowMySqlInfos(sb As StringBuilder, prm As clsPrmDBR,
                lstMySqlPrm As List(Of String), dicSqlPrm As Dictionary(Of String, String),
                sMySqlConnectorVersion$)

            sb.AppendLine("MySql parameters :")
            For Each sPrm In lstMySqlPrm
                Dim sVal = dicSqlPrm(sPrm)
                Dim sDisplayedPrm$ = sPrm

                If sPrm = enumMySqlPrm.sql_mode Then
                    If sVal <> prm.mySqlprm.sSQLModeDef Then
                        If sVal.Length = 0 Then sVal = sMsgEmpty
                        sVal &= " (Default : " & prm.mySqlprm.sSQLModeDef & ")"
                    End If
                End If
                If sPrm = enumMySqlPrm.innodb_strict_mode Then
                    If sVal <> prm.mySqlprm.sInnodbStrictModeDef Then
                        If sVal.Length = 0 Then sVal = sMsgEmpty
                        sVal &= " (Default : " & prm.mySqlprm.sInnodbStrictModeDef & ")"
                    End If
                End If
                ' 04/03/2022
                If sPrm = enumMySqlPrm.foreign_key_checks Then
                    If sVal <> prm.mySqlprm.sforeign_key_checksDef Then
                        If sVal.Length = 0 Then sVal = sMsgEmpty
                        sVal &= " (Default : " & prm.mySqlprm.sforeign_key_checksDef & ")"
                    End If
                End If

                If sPrm = enumMySqlPrm.collation_server Then
                    If Not prm.mySqlprm.bDisplayCollation Then Continue For
                    If sVal <> prm.mySqlprm.sServerCollationDef Then
                        sDisplayedPrm = "Default collation for new databases (collation_server)"
                        If sVal.Length = 0 Then sVal = sMsgEmpty
                        sVal &= " (Default : " & prm.mySqlprm.sServerCollationDef & ")"
                    Else
                        Continue For
                    End If
                End If

                If sPrm = enumMySqlPrm.DEFAULT_COLLATION_NAME Then
                    If Not prm.mySqlprm.bDisplayCollation Then Continue For
                    If sVal <> prm.mySqlprm.sDatabaseCollationDef Then
                        sDisplayedPrm = "Database default collation (DEFAULT_COLLATION_NAME)"
                        If sVal.Length = 0 Then sVal = sMsgEmpty
                        sVal &= " (Default : " & prm.mySqlprm.sDatabaseCollationDef & ")"
                    Else
                        Continue For
                    End If
                End If

                If sPrm = enumMySqlPrm.net_read_timeout_sec OrElse
                    sPrm = enumMySqlPrm.net_write_timeout_sec Then

                    Dim iValDef% = 0
                    If sPrm = enumMySqlPrm.net_read_timeout_sec Then _
                        iValDef = prm.mySqlprm.iNetReadTimeoutSecDef
                    If sPrm = enumMySqlPrm.net_write_timeout_sec Then _
                        iValDef = prm.mySqlprm.iNetWriteTimeoutSecDef

                    If sVal = prm.mySqlprm.iTimeOutMaxDef.ToString Then
                        sVal = sVal & " (no limit)"
                    Else
                        Dim iVal = Convert.ToInt32(sVal)
                        sVal = sVal & " (" & sDisplayTime(iVal) & ")"
                    End If

                    If iValDef > iNoDefaultTimeOut Then
                        Dim sValDef$ = iValDef.ToString
                        If sValDef = prm.mySqlprm.iTimeOutMaxDef.ToString Then
                            sValDef = sValDef & " (no limit)"
                        Else
                            sValDef = sValDef & " (" & sDisplayTime(iValDef) & ")"
                        End If
                        sVal &= " (Default : " & sValDef & ")"
                    End If

                End If

                If dicSqlPrm.ContainsKey(sPrm) Then sb.AppendLine(sDisplayedPrm & " : " & sVal)
            Next
            If sMySqlConnectorVersion.Length > 0 Then sb.AppendLine("MySQL Connector/NET driver version : " & sMySqlConnectorVersion)
            sb.AppendLine()

        End Sub

        Private Sub CreateTableReport(sb As StringBuilder, prm As clsPrmDBR,
                schema As DatabaseSchemaReader.DataSchema.DatabaseSchema, bMySql As Boolean,
                dicSqlTE As Dictionary(Of String, String),
                dicSqlTC As Dictionary(Of String, String),
                dicSqlCC As Dictionary(Of String, String),
                dicTableLinks As SortDic(Of String, List(Of String)))

            ' Lister les clés étrangères pour préciser :
            ' Build foreign key list to specify :
            ' "not nullable without default value" -> "not nullable foreign key"
            Dim hsForeignKeys As New HashSet(Of String)
            If prm.bAlertNotNullable Then
                For Each table In schema.Tables
                    For Each fk In table.ForeignKeys
                        Dim sId$ = fk.Columns(0)
                        Dim sCleFK2$ = table.Name & ":" & sId
                        hsForeignKeys.Add(sCleFK2)
                    Next
                Next
            End If

            Dim dicTables As New SortDic(Of String, DatabaseSchemaReader.DataSchema.DatabaseTable)
            For Each table In schema.Tables
                ' 18/06/2025 Ignore PostgreSQL system tables
                If table.Name.ToUpper.StartsWith("PG_") Then Continue For
                If table.Name.ToUpper.StartsWith("SQL_") Then Continue For
                ' 19/07/2025 Check if not already in dictionary
                If Not dicTables.ContainsKey(table.Name) Then dicTables.Add(table.Name, table)
            Next

            Dim sTableSorting$ = ""
            If prm.bSortTables Then sTableSorting = "Name"

            Dim sqlTypesDictionary As Dictionary(Of String, String) =
                [Enum].GetValues(GetType(enumSqlStandardType)).
                Cast(Of enumSqlStandardType)().
                ToDictionary(Function(t) t.ToString().ToUpper(), Function(t) t.ToDescription())

            Dim toUpper = prm.bUseUpperCaseIdentifiers
            For Each table In dicTables.Sort(sTableSorting)
                Dim sTableTitle$ = table.Name
                If toUpper Then sTableTitle = sTableTitle.ToUpper
                If prm.bDisplayTableAndFieldDescription AndAlso Not IsNothing(table.Description) AndAlso table.Description.Length > 0 Then _
                    sTableTitle &= " : " & table.Description
                If bMySql Then
                    If prm.mySqlprm.bDisplayTableEngine AndAlso dicSqlTE.ContainsKey(table.Name) Then
                        Dim sTE$ = dicSqlTE(table.Name)
                        If sTE <> prm.mySqlprm.sTableEngineDef Then sTableTitle &= " (engine : " & sTE & ")"
                    End If
                    If prm.mySqlprm.bDisplayCollation AndAlso dicSqlTC.ContainsKey(table.Name) Then
                        Dim sTC$ = dicSqlTC(table.Name)
                        If sTC <> prm.mySqlprm.sTableCollationDef Then sTableTitle &= " (collation : " & sTC & ")"
                    End If
                End If
                sb.AppendLine(sTableTitle)

                Dim bSQLite = False
                If prm.DBProvider = enumDBProvider.SQLiteClient Then bSQLite = True

                ' Noter tous les champs nullables pour alerter sur les clés uniques
                ' Hashset of nullable fields of the table, to warn uniqueness
                Dim hsNullablesCol As New HashSet(Of String) ' key : column name
                Dim lstCol As New List(Of String)
                Dim sAutonumberColName$ = ""
                For Each col In table.Columns
                    Dim sTitle$ = col.Name
                    If toUpper Then sTitle = sTitle.ToUpper ' 10/05/2025
                    Dim sColumnName$ = sTitle ' 25/07/2025
                    If col.Nullable Then hsNullablesCol.Add(sTitle)
                    Dim bDefVal As Boolean = False

                    ' 23/10/2016 Distinguish empty string to null
                    'If Not String.IsNullOrEmpty(col.DefaultValue) Then bDefVal = True
                    If Not IsNothing(col.DefaultValue) Then bDefVal = True

                    If prm.bDisplayFieldType Then
                        Dim sType$ = col.DbDataType.TrimEnd
                        If prm.bDisplayStandardSqlType Then ' 19/07/2025
                            sType = sType.Replace(" ", "") ' Remove spaces
                            sType = sType.ToUpper
                            sType = sType.Replace("UNSIGNED", "") ' Not in SQL standard
                            sType = sType.Replace("INT(10)", "INT") ' (10) is used only to display, it is not in SQL standard
                            sType = sType.Replace("SMALLINT(5)", "SMALLINT") ' (5) is used only to display, it is not in SQL standard
                            sType = sType.Replace("TINYINT(5)", "TINYINT") ' (5) is used only to display, it is not in SQL standard
                            If sqlTypesDictionary.ContainsKey(sType) Then sType = sqlTypesDictionary(sType)
                        End If
                        If Not prm.bDisplayFieldLength Then ' Ex.: VARCHAR(255) -> VARCHAR
                            If sType.Contains("(") Then
                                Dim iIndex = sType.IndexOf("(")
                                sType = sType.Substring(0, iIndex) ' Remove field length
                            End If
                        End If
                        sTitle &= " (" & sType & ")" ' 04/05/2024
                    End If

                    If prm.bDisplayFieldDefaultValue AndAlso bDefVal Then
                        Dim sDisplay$ = col.DefaultValue
                        If sDisplay.Length = 0 Then sDisplay = "''" ' ' 23/10/2016
                        sTitle &= " (" & sDisplay & ")"
                        End If
                        If prm.bDisplayTableAndFieldDescription AndAlso Not IsNothing(col.Description) AndAlso col.Description.Length > 0 Then _
                        sTitle &= " : " & col.Description

                    'If col.IsAutoNumber Then sAutonumberColName = sTitle ' 04/05/2024
                    If col.IsAutoNumber AndAlso bSQLite AndAlso
                        String.IsNullOrEmpty(col.ForeignKeyTableName) Then
                        'sAutonumberColName = sTitle ' 24/05/2024
                        sAutonumberColName = sColumnName ' 25/07/2025
                    End If

                    If col.IsAutoNumber AndAlso Not prm.bDisplayAutonumberAsPrimaryKey Then
                        sTitle &= " (autonumber)"
                    ElseIf Not col.Nullable Then
                        If prm.bAlertNotNullable Then
                            If Not bDefVal Then
                                Dim sCleFK$ = table.Name & ":" & col.Name
                                If hsForeignKeys.Contains(sCleFK) Then
                                    sTitle &= " (not nullable foreign key)"
                                Else
                                    sTitle &= " (not nullable without default value)"
                                End If
                            Else
                                sTitle &= " (not nullable)"
                            End If
                        End If
                    End If

                    If bMySql AndAlso prm.mySqlprm.bDisplayCollation Then
                        Dim sKey$ = table.Name & ":" & col.Name
                        If dicSqlCC.ContainsKey(sKey) Then
                            Dim sCC$ = dicSqlCC(sKey)
                            If sCC <> prm.mySqlprm.sColumnCollationDef Then sTitle &= " (collation : " & sCC & ")"
                        End If
                    End If

                    lstCol.Add(sTitle)
                Next
                If prm.bSortColumns Then lstCol.Sort()
                For Each sCol In lstCol
                    sb.AppendLine("  " & sCol)
                Next

                Dim dicIndexes As New SortDic(Of String, DatabaseSchemaReader.DataSchema.DatabaseIndex)
                For Each ind In table.Indexes
                    dicIndexes.Add(ind.Name, ind)
                Next

                Dim sSorting$ = ""
                If prm.bSortIndexes Then sSorting = "Name"
                Dim bIndexAutonumberDisplayed As Boolean = False
                Dim sPreviousIndex$ = ""

                For Each ind In dicIndexes.Sort(sSorting)

                    ' 18/06/2025 For PostgreSQL system tables
                    Dim sIndexNameWithout_pkey = ind.Name.Replace("_pkey", "")
                    Dim bPrimaryKeyEndsWithIndexName = table.PrimaryKey?.Name?.EndsWith(sIndexNameWithout_pkey)
                    Dim bTableNameIsIndexName = (table.Name = sIndexNameWithout_pkey)

                    Dim sUnique$ = ""
                    Dim sPrimary$ = ""
                    Dim bMultipleIndex As Boolean = False
                    If ind.Columns.Count > 1 Then bMultipleIndex = True
                    Dim bPK = False
                    Const sSQLiteAutoIndex$ = "sqlite_autoindex_"
                    Dim bSQLiteAutoIndex = False
                    If ind.Name.StartsWith(sSQLiteAutoIndex) Then bSQLiteAutoIndex = True

                    ' 24/05/2024
                    Dim bIndexNameContainsPK = False
                    If bSQLite AndAlso bSQLiteAutoIndex AndAlso
                        ind.Name = sSQLiteAutoIndex & table.PrimaryKey.TableName & "_1" Then bIndexNameContainsPK = True

                    If (Not IsNothing(table.PrimaryKey) AndAlso
                        (table.PrimaryKey.Name = ind.Name OrElse bIndexNameContainsPK OrElse
                        bPrimaryKeyEndsWithIndexName OrElse bTableNameIsIndexName)) OrElse
                        (bSQLite AndAlso bSQLiteAutoIndex AndAlso
                        Not IsNothing(table.PrimaryKeyColumn) AndAlso
                        table.PrimaryKeyColumn.Name = ind.Columns(0).Name) Then ' 04/05/2024
                        If sAutonumberColName.Length = 0 Then ' 24/05/2024 PrimaryKey yet found
                            sPrimary = ", Primary"
                            bPK = True
                            If bMultipleIndex AndAlso Not prm.bDisplayMultipleIndexName Then sPrimary = "Primary" ' 04/05/2024
                            ind.IsUnique = True ' 25/04/2024 Necessarily
                        End If
                    End If

                    If bSQLiteAutoIndex Then ind.IsUnique = True ' 04/05/2024

                    If ind.IsUnique Then
                        sUnique = ", Unique"
                        If bMultipleIndex AndAlso Not prm.bDisplayMultipleIndexName AndAlso Not bPK Then sUnique = "Unique" ' 04/05/2024
                    End If

                    ' MySQL (5.6) ne peut garantir l'unicité si un des champs d'une clé unique peut être nul
                    '  on peut alors avoir des doublons dans la table
                    ' MySQL (5.6) can't guarantee uniqueness (unicity) if one field of a unique key 
                    '  is nullable, you can have duplicates records in the table
                    Dim sWarnNF$ = ""
                    Const sWarnNFTxt = " (nullable field for a unique index)"
                    Dim sIndexName$ = "" ' 04/05/2024
                    If Not bMultipleIndex Then
                        sIndexName = ind.Columns(0).Name
                        If toUpper Then sIndexName = sIndexName.ToUpper ' 10/05/2025
                        If sIndexName = sAutonumberColName Then bIndexAutonumberDisplayed = True ' 04/05/2024
                        If bSQLite AndAlso prm.bDisplaySQLiteSimpleIndexName Then sIndexName &= " (" & ind.Name & ")" ' 04/05/2024
                        If ind.IsUnique AndAlso hsNullablesCol.Contains(sIndexName) Then sWarnNF = sWarnNFTxt
                        If sIndexName <> sPreviousIndex OrElse Not bSQLite Then ' 04/05/2024
                            sb.AppendLine("    Index   : " & sIndexName & sPrimary & sUnique & sWarnNF)
                        End If
                    Else
                        If prm.bDisplayMultipleIndexName Then sIndexName = ind.Name ' 04/05/2024
                        Dim sComma$ = ", "
                        If Not prm.bDisplayMultipleIndexName AndAlso Not bPK AndAlso Not ind.IsUnique Then sComma = "" ' 04/05/2024

                        ' 04/05/2024 sqlite_autoindex_X_1 -> PK_X
                        If prm.bRenameSQLiteMultipleIndex Then ' 04/05/2024
                            sIndexName = sIndexName.Replace("sqlite_autoindex_", "PK_")
                            sIndexName = sIndexName.Replace("_1", "")
                        End If
                        If toUpper Then sIndexName = sIndexName.ToUpper ' 10/05/2025

                        sb.AppendLine("    Index   : " & sIndexName & sPrimary & sUnique & sComma &
                            ind.Columns.Count & " fields" & " :")
                        For Each fld In ind.Columns
                            sWarnNF = ""
                            If ind.IsUnique AndAlso hsNullablesCol.Contains(fld.Name) Then _
                                sWarnNF = sWarnNFTxt
                            Dim sFieldName$ = fld.Name
                            If toUpper Then sFieldName = sFieldName.ToUpper ' 10/05/2025
                            sb.AppendLine("      field : " & sFieldName & sWarnNF)
                        Next
                    End If

                    sPreviousIndex = sIndexName

                Next

                ' This one is not sorted:
                ' prm.bDisplayIndexName: Except sAutonumberColName
                If prm.bDisplayAutonumberAsPrimaryKey AndAlso
                    Not bIndexAutonumberDisplayed AndAlso
                    Not String.IsNullOrEmpty(sAutonumberColName) Then ' 04/05/2024
                    sb.AppendLine("    Index   : " & sAutonumberColName & ", Primary, Unique")
                End If

                If prm.bDisplayLinksBelowEachTable Then ' 31/05/2024
                    If dicTableLinks.ContainsKey(table.Name) Then
                        Dim lst = dicTableLinks(table.Name)
                        For Each sLine In lst
                            sb.AppendLine(sLine)
                        Next
                    End If
                End If

                sb.AppendLine()
            Next

            If Not prm.bDisplayLinksBelowEachTable Then sb.AppendLine()

        End Sub

        Private Sub CreateLinkReport(sb As StringBuilder, prm As clsPrmDBR,
                schema As DatabaseSchemaReader.DataSchema.DatabaseSchema,
                dicTableLinks As SortDic(Of String, List(Of String)))

            ' Relationships between tables

            ' 24/05/2024
            Dim dicTables As New SortDic(Of String, DatabaseSchemaReader.DataSchema.DatabaseTable)
            For Each table In schema.Tables
                ' 18/06/2025 Ignore PostgreSQL system tables
                If table.Name.ToUpper.StartsWith("PG_") Then Continue For
                If table.Name.ToUpper.StartsWith("SQL_") Then Continue For
                ' 19/07/2025 Check if not already in dictionary
                If Not dicTables.ContainsKey(table.Name) Then dicTables.Add(table.Name, table)
            Next

            Dim sTableSorting$ = ""
            If prm.bSortTables Then sTableSorting = "Name"

            Dim toUpper = prm.bUseUpperCaseIdentifiers
            For Each table In dicTables.Sort(sTableSorting)

                Dim sTableTitle$ = table.Name
                If toUpper Then sTableTitle = sTableTitle.ToUpper ' 10/05/2025
                If prm.bDisplayTableAndFieldDescription AndAlso Not IsNothing(table.Description) AndAlso table.Description.Length > 0 Then _
                    sTableTitle &= " : " & table.Description

                Dim lst As New List(Of String)

                Dim dicLinks As New SortDic(Of String, clsLink)
                For Each fk In table.ForeignKeys
                    Dim sId$ = fk.Columns(0)
                    Dim sLinkTable$ = fk.RefersToTable
                    Dim iCount% = 1
                    Dim sKey$ = sLinkTable & " : " & sId
Retry:              ' 04/09/2016 A constraint may be duplicated
                    If iCount > 1 Then
                        sKey = sLinkTable & " : " & sId & ":" & iCount
                    End If
                    Dim l0 As New clsLink
                    l0.Id = sId
                    l0.Name = fk.Name
                    l0.Table = sLinkTable
                    l0.dc = fk
                    l0.Count = iCount
                    If dicLinks.ContainsKey(sKey) Then
                        iCount += 1
                        GoTo Retry
                    End If
                    dicLinks.Add(sKey, l0)
                Next
                Dim sSorting$ = ""
                If prm.bSortLinks Then
                    If prm.bDisplayLinkName Then
                        sSorting = "Name"
                    Else
                        sSorting = "Table, Id"
                    End If
                End If
                For Each fk0 In dicLinks.Sort(sSorting)
                    Dim fk = fk0.dc
                    Dim sId$ = fk.Columns(0)
                    Dim sLinkTable$ = fk.RefersToTable
                    If String.IsNullOrEmpty(sLinkTable) Then Continue For ' 18/06/2025
                    If toUpper Then sLinkTable = sLinkTable.ToUpper : sId = sId.ToUpper ' 10/05/2025
                    Dim sLinkName$ = ""
                    Dim sCount$ = ""
                    If prm.bDisplayLinkName Then
                        sLinkName = " : " & fk.Name
                    ElseIf fk0.Count > 1 Then
                        sCount = " (" & fk0.Count & ")" ' 04/09/2016
                    End If
                    Dim sDelRule$ = ""
                    Dim sUpdRule$ = ""
                    'Const sRestrict$ = "RESTRICT"
                    Dim sRestrictDef$ = prm.sForeignKeyDeleteRuleDef
                    If fk.DeleteRule <> sRestrictDef Then sDelRule = " (Delete rule : " &
                        CStr(IIf(String.IsNullOrEmpty(fk.DeleteRule), "undefined", fk.DeleteRule)) & ")"
                    sRestrictDef = prm.sForeignKeyUpdateRuleDef
                    If Not IsNothing(fk.UpdateRule) AndAlso fk.UpdateRule <> sRestrictDef Then sUpdRule = " (Update rule : " &
                        CStr(IIf(String.IsNullOrEmpty(fk.UpdateRule), "undefined", fk.UpdateRule)) & ")"

                    Dim sLine$ = sLinkTable & " : " &
                    sId & sCount & sLinkName & sDelRule & sUpdRule
                    If prm.bDisplayLinksBelowEachTable Then ' 31/05/2024
                        sLine = "    Link    : " & sLine
                    Else
                        sLine = "  " & sLine
                    End If
                    lst.Add(sLine)

                Next

                dicTableLinks.Add(sTableTitle, lst)

            Next

        End Sub

#Region "MySql parameters"

        Public Function bGetMySqlParameters(sMySQLConnectionString$, sDbName$,
                dicMySqlPrm As Dictionary(Of String, String), lstMySqlPrm As List(Of String),
                sMsgErr$, sMsgErrPossibleCause$) As Boolean

            ShowMsg("Loading MySql parameters...")
            Try

                Using oConnMySQL As New MySqlConnection
                    oConnMySQL.ConnectionString = sMySQLConnectionString
                    oConnMySQL.Open()

                    ' Syntax ok in phpMyAdmin but not there !?
                    'sSQL = "SHOW VARIABLES WHERE (Variable_Name LIKE '%sql_mode' OR Variable_Name LIKE '%timeout');"

                    Dim lstSQL As New List(Of String)
                    ' http://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html

                    ' Ordered list (a dictionary is not ordered)
                    lstMySqlPrm.Add("version_comment")
                    lstMySqlPrm.Add("version")
                    lstMySqlPrm.Add("protocol_version")
                    lstMySqlPrm.Add("sql_mode")
                    lstMySqlPrm.Add("foreign_key_checks") ' 04/03/2022
                    lstMySqlPrm.Add("innodb_strict_mode")
                    lstMySqlPrm.Add("net_read_timeout")
                    lstMySqlPrm.Add("net_write_timeout")
                    lstMySqlPrm.Add("collation_server")
                    lstMySqlPrm.Add("DEFAULT_COLLATION_NAME")

                    lstSQL.Add("SHOW VARIABLES WHERE Variable_Name IN (" &
                        "'version', 'version_comment', 'protocol_version', 'sql_mode', " &
                        "'foreign_key_checks', 'innodb_strict_mode', 'net_read_timeout', 'net_write_timeout');")
                    lstSQL.Add("SELECT 'DEFAULT_COLLATION_NAME', DEFAULT_COLLATION_NAME FROM " &
                        "information_schema.SCHEMATA WHERE schema_name = '" & sDbName & "';")
                    lstSQL.Add("SHOW VARIABLES LIKE 'collation_server'")

                    Dim iNbRecords% = 0
                    For Each sSQL In lstSQL
                        Using cmd2 As New MySqlCommand(sSQL, oConnMySQL)
                            Using reader As MySqlDataReader = cmd2.ExecuteReader()
                                If reader.HasRows Then
                                    Do While reader.Read()
                                        iNbRecords += 1
                                        Dim sPrmName$ = reader.GetString(0)
                                        Dim sPrmValue$ = reader.GetString(1)
                                        dicMySqlPrm.Add(sPrmName, sPrmValue)
                                    Loop
                                Else
                                    Debug.WriteLine("bGetMySqlParameters : No rows found.")
                                End If
                            End Using
                        End Using
                    Next

                End Using
                Return True

            Catch ex As Exception
                ShowMsg(sMsgError)
                Dim sFinalErrMsg$ = ""
                ShowErrorMsg(ex, "bGetMySqlParameters", sMsgErr, sMsgErrPossibleCause, sFinalErrMsg:=sFinalErrMsg)
                ShowLongMsg(sFinalErrMsg)
                Return False

            Finally

            End Try

        End Function

        Public Sub GetMySqlTablesCollationAndEngine(sMySQLConnectionString$, sDbName$,
                dicMySqlTE As Dictionary(Of String, String),
                dicMySqlTC As Dictionary(Of String, String))

            ShowMsg("Loading MySql tables collation...")
            Try

                Using oConnMySQL As New MySqlConnection
                    oConnMySQL.ConnectionString = sMySQLConnectionString
                    oConnMySQL.Open()

                    Dim lstSQL As New List(Of String)
                    lstSQL.Add("SELECT TABLE_NAME, ENGINE, COLLATION_NAME FROM information_schema.`TABLES` T," &
                        " information_schema.`COLLATION_CHARACTER_SET_APPLICABILITY` CCSA" &
                        " WHERE CCSA.collation_name = T.table_collation AND T.table_schema = '" & sDbName & "';")

                    Dim iNbRecords% = 0
                    For Each sSQL In lstSQL
                        Using cmd2 As New MySqlCommand(sSQL, oConnMySQL)
                            Using reader As MySqlDataReader = cmd2.ExecuteReader()
                                If reader.HasRows Then
                                    Do While reader.Read()
                                        iNbRecords += 1
                                        Dim sPrmName$ = reader.GetString(0)
                                        Dim sPrmValue1$ = reader.GetString(1)
                                        Dim sPrmValue2$ = reader.GetString(2)
                                        dicMySqlTE.Add(sPrmName, sPrmValue1)
                                        dicMySqlTC.Add(sPrmName, sPrmValue2)
                                    Loop
                                Else
                                    Debug.WriteLine("GetMySqlTablesCollationAndEngine : No rows found.")
                                End If
                            End Using
                        End Using
                    Next

                End Using

            Catch ex As Exception
                ShowErrorMsg(ex, "GetMySqlTablesCollationAndEngine")
            Finally

            End Try

        End Sub

        Public Sub GetMySqlColumnsCollation(sMySQLConnectionString$, sDbName$,
                dicMySqlCC As Dictionary(Of String, String))

            ShowMsg("Loading MySql columns collation...")
            Try

                Using oConnMySQL As New MySqlConnection ' MySql.Data.MySqlClient.MySqlConnection
                    oConnMySQL.ConnectionString = sMySQLConnectionString
                    oConnMySQL.Open()

                    Dim lstSQL As New List(Of String)
                    lstSQL.Add("SELECT table_name, C.column_name, COLLATION_NAME FROM " &
                        "information_schema.`COLUMNS` C WHERE table_schema = '" & sDbName & "';")

                    Dim iNbRecords% = 0
                    For Each sSQL In lstSQL
                        Using cmd2 As New MySqlCommand(sSQL, oConnMySQL)
                            Using reader As MySqlDataReader = cmd2.ExecuteReader()
                                If reader.HasRows Then
                                    Do While reader.Read()
                                        iNbRecords += 1
                                        Dim sTableName$ = reader.GetString(0)
                                        Dim sColName$ = reader.GetString(1)
                                        If reader.IsDBNull(2) Then Continue Do
                                        Dim sPrmValue$ = reader.GetString(2)
                                        Dim sKey$ = sTableName & ":" & sColName
                                        dicMySqlCC.Add(sKey, sPrmValue)
                                    Loop
                                Else
                                    Debug.WriteLine("GetMySqlColumnsCollation : No rows found.")
                                End If
                            End Using
                        End Using
                    Next

                End Using

            Catch ex As Exception
                ShowErrorMsg(ex, "GetMySqlColumnsCollation")
            Finally

            End Try

        End Sub

#End Region

    End Module

End Namespace