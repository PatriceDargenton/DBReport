﻿
' File modDBReport.vb
' -------------------

Imports System.Text ' StringBuilder

Public Module modDBReport

Public Const sMsgError$ = "Error !"
Public Const sMsgDone$ = "Done."
Public Const sMsgDBOff$ = "Could not connect to database !" & vbCrLf & _
    "Possible cause : the database server has not been started," & vbCrLf & _
    " or wrong database name or account used."
Public Const sMsgCompoMySQLNotInst$ = _
    "Possible cause : mysql-connector-net-6.9.8.msi is not installed"

Private m_dTimeStart As Date
Private m_delegMsg As clsDelegMsg
Private WithEvents m_dbReader As DatabaseSchemaReader.DatabaseReader
Private Sub ShowMessageDeleg(sender As Object, e As DatabaseSchemaReader.ReaderEventArgs) _
    Handles m_dbReader.ReaderProgress

    If bDebug Then
        Debug.WriteLine("")
        Debug.WriteLine("")
        Debug.WriteLine("Reading database schema : " & e.ProgressType.ToString & ", " & _
            e.SchemaObjectType.ToString) ' & ", " & e.Name & ", " & e.Index & ", " & e.Count)
        Dim dTimeEnd = Now
        Dim ts = dTimeEnd - m_dTimeStart
        Const sDateTimeFormat = "dd\/MM\/yyyy HH:mm:ss"
        Dim sTime$ = m_dTimeStart.ToString(sDateTimeFormat) & " -> " & _
            dTimeEnd.ToString(sDateTimeFormat) & " : " & sDisplayTime(ts.TotalSeconds)
        Debug.WriteLine(sTime)
    End If
    m_delegMsg.ShowMsg("Reading database schema : " & e.ProgressType.ToString & ", " & _
        e.SchemaObjectType.ToString) ' & ", " & e.Name & ", " & e.Index & ", " & e.Count)

End Sub

Public Class clsPrm

    Public sConnection$, sDBProvider$, sUserLogin$, sServer$, sDBName$

    Public bDisplayTableAndFieldDescription As Boolean
    Public bDisplayFieldType As Boolean
    Public bDisplayFieldDefaultValue As Boolean
    Public bDisplayLinkName As Boolean
    Public bSortColumns As Boolean
    Public bSortIndexes As Boolean
    Public bSortLinks As Boolean
    Public bAlertNotNullable As Boolean

End Class

Public Function bCreateDBReport(prm As clsPrm, delegMsg As clsDelegMsg, _
    sMsgErr$, sMsgErrPossibleCause$, ByRef sb As StringBuilder) As Boolean

    ' Library used : https://dbschemareader.codeplex.com

    Try
        delegMsg.ShowMsg("Connecting to database...")
        If delegMsg.m_bCancel Then Return False
        'Dim dbReader As New DatabaseSchemaReader.DatabaseReader(prm.sConnection, prm.sDBProvider)
        m_dbReader = New DatabaseSchemaReader.DatabaseReader(prm.sConnection, prm.sDBProvider)
        m_dbReader.Owner = prm.sDBName ' 22/08/2016
        m_delegMsg = delegMsg

        delegMsg.ShowMsg("Reading database schema...")
        If delegMsg.m_bCancel Then Return False

        m_dTimeStart = Now
        Dim schema = m_dbReader.ReadAll
        Const sDateTimeFormat = "dd\/MM\/yyyy HH:mm:ss"
        If bDebug Then
            Dim dTimeEnd = Now
            Dim ts = dTimeEnd - m_dTimeStart
            Dim sTime$ = m_dTimeStart.ToString(sDateTimeFormat) & " -> " & _
                dTimeEnd.ToString(sDateTimeFormat) & " : " & sDisplayTime(ts.TotalSeconds)
            Debug.WriteLine(sTime & " : ReadAll")
        End If

        delegMsg.ShowMsg("Building database report...")
        If delegMsg.m_bCancel Then Return False

        sb = New StringBuilder()

        sb.AppendLine()
        sb.AppendLine("Database report")
        sb.AppendLine("---------------")
        sb.AppendLine()
        sb.AppendLine("Login    : " & prm.sUserLogin)
        sb.AppendLine("Server   : " & prm.sServer)
        sb.AppendLine("Database : " & prm.sDBName)
        If prm.bSortColumns Then
            sb.AppendLine("Columns  : Sorted")
        Else
            'sb.AppendLine("Columns  : Not sorted")
        End If
        If prm.bSortIndexes Then
            sb.AppendLine("Indexes  : Sorted")
        Else
            'sb.AppendLine("Indexes  : Not sorted")
        End If
        sb.AppendLine()
        'GoTo Links

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

        For Each table In schema.Tables
            Dim sTableTitle$ = table.Name
            If prm.bDisplayTableAndFieldDescription AndAlso table.Description.Length > 0 Then _
                sTableTitle &= " : " & table.Description
            sb.AppendLine(sTableTitle)

            ' Noter tous les champs nullables pour alerter sur les clés uniques
            ' Hashset of nullable fields of the table, to warn uniqueness
            Dim hsNullablesCol As New HashSet(Of String) ' key : column name
            Dim lstCol As New List(Of String)
            For Each col In table.Columns
                Dim sTitle$ = col.Name
                If col.Nullable Then hsNullablesCol.Add(sTitle)
                Dim bDefVal As Boolean = False
                If Not String.IsNullOrEmpty(col.DefaultValue) Then bDefVal = True
                If prm.bDisplayFieldType Then sTitle &= " (" & col.DbDataType & ")"
                If prm.bDisplayFieldDefaultValue AndAlso bDefVal Then _
                    sTitle &= " (" & col.DefaultValue & ")"
                If prm.bDisplayTableAndFieldDescription AndAlso col.Description.Length > 0 Then _
                    sTitle &= " : " & col.Description
                If col.IsAutoNumber Then
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
                lstCol.Add(sTitle)
            Next
            If prm.bSortColumns Then lstCol.Sort()
            For Each sCol In lstCol
                sb.AppendLine("  " & sCol)
            Next

            Dim dico As New SortDic(Of String, DatabaseSchemaReader.DataSchema.DatabaseIndex)
            For Each ind In table.Indexes
                dico.Add(ind.Name, ind)
            Next

            Dim sSorting$ = ""
            If prm.bSortIndexes Then sSorting = "Name"

            For Each ind In dico.Sort(sSorting)
                Dim sUnique$ = ""
                Dim sPrimary$ = ""
                If ind.IsUnique Then sUnique = ", Unique"

                If Not IsNothing(table.PrimaryKey) AndAlso _
                   table.PrimaryKey.Name = ind.Name Then
                   sPrimary = ", Primary"
                End If

                ' MySQL (5.6) ne peut garantir l'unicité si un des champs d'une clé unique peut être nul
                '  on peut alors avoir des doublons dans la table
                ' MySQL (5.6) can't guarantee uniqueness (unicity) if one field of a unique key 
                '  is nullable, you can have duplicates records in the table
                Dim sWarnNF$ = ""
                Const sWarnNFTxt = " (nullable field for a unique index)"
                If ind.Columns.Count = 1 Then
                    If ind.IsUnique AndAlso hsNullablesCol.Contains(ind.Columns(0).Name) Then _
                        sWarnNF = sWarnNFTxt
                    sb.AppendLine("    Index   : " & ind.Columns(0).Name & sPrimary & sUnique & sWarnNF)
                Else
                    sb.AppendLine("    Index   : " & ind.Name & sPrimary & sUnique & ", " & _
                        ind.Columns.Count & " fields" & " :")
                    For Each chp In ind.Columns
                        sWarnNF = ""
                        If ind.IsUnique AndAlso hsNullablesCol.Contains(chp.Name) Then _
                            sWarnNF = sWarnNFTxt
                        sb.AppendLine("      field : " & chp.Name & sWarnNF)
                    Next
                End If
            Next

            sb.AppendLine()
        Next

        sb.AppendLine()

'Links:
        sb.AppendLine("Links") ' Relationships between tables
        For Each table In schema.Tables
            sb.AppendLine()
            Dim sTableTitle$ = table.Name
            If prm.bDisplayTableAndFieldDescription AndAlso table.Description.Length > 0 Then _
                sTableTitle &= " : " & table.Description
            sb.AppendLine(sTableTitle)

            Dim dico As New SortDic(Of String, clsLink)
            For Each fk In table.ForeignKeys
                Dim sId$ = fk.Columns(0)
                Dim sLinkTable$ = fk.RefersToTable
                Dim iCount% = 1
                Dim sKey$ = sLinkTable & " : " & sId
Retry:          ' 04/09/2016 A constraint may be duplicated
                If iCount > 1 Then
                    sKey = sLinkTable & " : " & sId & ":" & iCount
                End If
                Dim l0 As New clsLink
                l0.Id = sId
                l0.Name = fk.Name
                l0.Table = sLinkTable
                l0.dc = fk
                l0.Count = iCount
                If dico.ContainsKey(sKey) Then
                    iCount += 1
                    GoTo Retry
                End If
                dico.Add(sKey, l0)
            Next
            Dim sSorting$ = ""
            If prm.bSortLinks Then
                If prm.bDisplayLinkName Then
                    sSorting = "Name"
                Else
                    sSorting = "Table, Id"
                End If
            End If
            For Each fk0 In dico.Sort(sSorting)
                Dim fk = fk0.dc
                Dim sId$ = fk.Columns(0)
                Dim sLinkTable$ = fk.RefersToTable
                Dim sLinkName$ = ""
                Dim sCount$ = ""
                If prm.bDisplayLinkName Then
                    sLinkName = " : " & fk.Name
                ElseIf fk0.Count > 1 Then
                    sCount = " (" & fk0.Count & ")" ' 04/09/2016
                End If
                Dim sDelRule$ = ""
                Dim sUpdRule$ = ""
                Const sRestrict$ = "RESTRICT"
                If fk.DeleteRule <> sRestrict Then sDelRule = " (Delete rule : " & _
                    CStr(IIf(String.IsNullOrEmpty(fk.DeleteRule), "undefined", fk.DeleteRule)) & ")"
                If fk.UpdateRule <> sRestrict Then sUpdRule = " (Update rule : " & _
                    CStr(IIf(String.IsNullOrEmpty(fk.UpdateRule), "undefined", fk.UpdateRule)) & ")"
                sb.AppendLine("  " & sLinkTable & " : " & _
                    sId & sCount & sLinkName & sDelRule & sUpdRule)
            Next
        Next

        delegMsg.ShowMsg(sMsgDone)
        delegMsg.ShowLongMsg("")

        Dim dTimeEnd2 = Now()
        Dim ts2 = dTimeEnd2 - m_dTimeStart
        Dim sTime2$ = m_dTimeStart.ToString(sDateTimeFormat) & " -> " & _
            dTimeEnd2.ToString(sDateTimeFormat) & " : " & sDisplayTime(ts2.TotalSeconds)
        sb.AppendLine()
        sb.AppendLine("Report created : " & sTime2)

        Return True

    Catch ex As Exception
        delegMsg.ShowMsg(sMsgError)
        Dim sFinalErrMsg$ = ""
        ShowErrorMsg(ex, "bCreateDBReport", sMsgErr, sMsgErrPossibleCause, sFinalErrMsg:=sFinalErrMsg)
        delegMsg.ShowLongMsg(sFinalErrMsg)
        Return False

    End Try

End Function

Public Class clsLink ' Relationship between two tables
    Public Id$ = ""
    Public Name$ = ""
    Public Table$ = ""
    Public Count% = 0
    Public dc As DatabaseSchemaReader.DataSchema.DatabaseConstraint
End Class

End Module