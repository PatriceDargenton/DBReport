
Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace DBReportVB.Tests

    <TestClass>
    Public Class UnitTestDBReport

        <TestMethod>
        Sub TestNorthwindEFSample()

            Dim path As String = AppContext.BaseDirectory ' Application.StartupPath() equivalent in .Net8
            'Dim dbPath As String = Path.Combine(path, "..\..\..\..", "DBReport\bin\northwindEF.db")
            Dim dbPath As String = IO.Path.Combine(path, "..\..\..\..",
                "DBReport\bin\Debug\net8.0-windows\northwindEF.db")
            Dim fullPath As String = IO.Path.GetFullPath(dbPath)
            Dim exists As Boolean = File.Exists(dbPath)
            ' The file northwindEF.db is in: northwind-sql.zip
            Assert.AreEqual(exists, True)

            Dim prm As New DBReport.clsPrmDBR()
            prm.sServer = fullPath
            prm.sDBName = "northwindEF.db"
            prm.sDBReportVersion = "1.24" ' _modConst.sAppVersion
            prm.sDBProvider = "System.Data.SQLite" 'DBReport.DBReport.enumDBProvider.SQLiteClient.ToDescription()
            prm.DBProvider = DBReport.enumDBProvider.SQLiteClient
            prm.bDisplayFieldType = True
            prm.bDisplayFieldDefaultValue = True
            prm.bSortTables = True
            prm.bSortColumns = True
            prm.bSortIndexes = True
            prm.bSortLinks = True
            prm.bDisplayMultipleIndexName = True
            prm.bRenameSQLiteMultipleIndex = True
            prm.sForeignKeyDeleteRuleDef = "NO ACTION"
            prm.sForeignKeyUpdateRuleDef = "NO ACTION"
            prm.bDisplayDateTime = False

            Dim delegMsg As New clsDelegMsg()
            Dim msgErr As String = ""
            Dim msgErrPossibleCause As String = ""
            Dim sb As New System.Text.StringBuilder()

            Dim result As Boolean = DBReport.modDBReport.bCreateDBReport(
                prm, delegMsg, msgErr, msgErrPossibleCause, sb)
            Assert.AreEqual(result, True)

            Dim content As String = sb.ToString()

            Dim expectedReportPath As String = IO.Path.Combine(path, "..\..\..\..",
                "DBReport\bin\DBReport_northwindEF_db_ok.txt")
            Dim expectedReportFullPath As String = IO.Path.GetFullPath(expectedReportPath)
            Dim exists2 As Boolean = File.Exists(expectedReportFullPath)
            Assert.AreEqual(exists2, True)

            Dim expected As String = File.ReadAllText(expectedReportFullPath)
            Assert.AreEqual(content, expected)

        End Sub

    End Class

End Namespace