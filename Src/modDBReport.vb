
Imports System.Text ' StringBuilder

Public Module modDBReport

Public Const sMsgError$ = "Error !"
Public Const sMsgDone$ = "Done."
Public Const sMsgDBOff$ = "Could not connect to database !" & vbCrLf & _
    "Possible cause : the database server has not been started," & vbCrLf & _
    " or wrong database name or account used."
Public Const sMsgCompoMySQLNotInst$ = _
    "Possible cause : mysql-connector-net-6.8.3.msi is not installed"

Public Function bCreateDBReport(delegMsg As clsDelegMsg, ByRef sb As StringBuilder, _
    sConnection$, sDBProvider$, _
    bDisplayFieldType As Boolean, bDisplayDefaultValue As Boolean, bDisplayLinkName As Boolean, _
    bSortColumns As Boolean, bSortIndex As Boolean, bSortLink As Boolean, _
    sUserLogin$, sServer$, sDBName$, sMsgErr$, sMsgErrPossibleCause$) As Boolean

    ' Library used : https://dbschemareader.codeplex.com

    Try
        delegMsg.ShowMsg("Connecting to database...")
        If delegMsg.m_bCancel Then Return False
        Dim dbReader As New DatabaseSchemaReader.DatabaseReader(sConnection, sDBProvider)

        delegMsg.ShowMsg("Reading database schema...")
        If delegMsg.m_bCancel Then Return False
        Dim schema = dbReader.ReadAll()
        delegMsg.ShowMsg("Building database report...")
        If delegMsg.m_bCancel Then Return False

        sb = New StringBuilder()

        sb.AppendLine()
        sb.AppendLine("Database report")
        sb.AppendLine("---------------")
        sb.AppendLine()
        sb.AppendLine("Login    : " & sUserLogin)
        sb.AppendLine("Server   : " & sServer)
        sb.AppendLine("Database : " & sDBName)
        If bSortColumns Then
            sb.AppendLine("Columns  : Sorted")
        Else
            'sb.AppendLine("Columns  : Not sorted")
        End If
        If bSortIndex Then
            sb.AppendLine("Index    : Sorted")
        Else
            'sb.AppendLine("Index    : Not sorted")
        End If
        sb.AppendLine()
        'GoTo Links

        ' Lister les clés étrangères pour préciser :
        ' Build foreign key list to specify :
        ' "not nullable without default value" -> "not nullable foreign key"
        Dim hsForeignKeys As New HashSet(Of String)
        For Each table In schema.Tables
            For Each fk In table.ForeignKeys
                Dim sId$ = fk.Columns(0)
                Dim sCleFK2$ = table.Name & ":" & sId
                hsForeignKeys.Add(sCleFK2)
            Next
        Next

        For Each table In schema.Tables
            Dim sTableTitle$ = table.Name
            If table.Description.Length > 0 Then sTableTitle &= " : " & table.Description
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
                If bDisplayFieldType Then sTitle &= " (" & col.DbDataType & ")"
                If bDisplayDefaultValue AndAlso bDefVal Then _
                    sTitle &= " (" & col.DefaultValue & ")"
                If col.Description.Length > 0 Then sTitle &= " : " & col.Description
                If col.IsAutoNumber Then
                    sTitle &= " (autonumber)"
                ElseIf Not col.Nullable Then
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
                lstCol.Add(sTitle)
            Next
            If bSortColumns Then lstCol.Sort()
            For Each sCol In lstCol
                sb.AppendLine("  " & sCol)
            Next

            Dim dico As New SortDic(Of String, DatabaseSchemaReader.DataSchema.DatabaseIndex)
            For Each ind In table.Indexes
                dico.Add(ind.Name, ind)
            Next

            Dim sSorting$ = ""
            If bSortIndex Then sSorting = "Name"

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
                Const sWarnNFTxt = " (nullable field for an unique index)"
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
        sb.AppendLine("Links")
        For Each table In schema.Tables
            sb.AppendLine()
            Dim sTableTitle$ = table.Name
            If table.Description.Length > 0 Then sTableTitle &= " : " & table.Description
            sb.AppendLine(sTableTitle)

            Dim dico As New SortDic(Of String, clsLink)
            For Each fk In table.ForeignKeys
                Dim sId$ = fk.Columns(0)
                Dim sLinkTable$ = fk.RefersToTable
                Dim sKey$ = sLinkTable & " : " & sId
                Dim l0 As New clsLink
                l0.Id = sId
                l0.Name = fk.Name
                l0.Table = sLinkTable
                l0.dc = fk
                dico.Add(sKey, l0)
            Next
            Dim sSorting$ = ""
            If bSortLink Then
                If bDisplayLinkName Then
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
                If bDisplayLinkName Then sLinkName = " : " & fk.Name
                Dim sDelRule$ = ""
                Dim sUpdRule$ = ""
                Const sRestrict$ = "RESTRICT"
                If fk.DeleteRule <> sRestrict Then sDelRule = " (Delete rule : " & _
                    CStr(IIf(String.IsNullOrEmpty(fk.DeleteRule), "undefined", fk.DeleteRule)) & ")"
                If fk.UpdateRule <> sRestrict Then sUpdRule = " (Update rule : " & _
                    CStr(IIf(String.IsNullOrEmpty(fk.UpdateRule), "undefined", fk.UpdateRule)) & ")"
                sb.AppendLine("  " & sLinkTable & " : " & _
                    sId & sLinkName & sDelRule & sUpdRule)
            Next
        Next

        delegMsg.ShowMsg(sMsgDone)
        delegMsg.ShowLongMsg("")
        Return True

    Catch ex As Exception
        delegMsg.ShowMsg(sMsgError)
        Dim sFinalErrMsg$ = ""
        ShowErrorMsg(ex, "bCreateDBReport", sMsgErr, sMsgErrPossibleCause, sFinalErrMsg:=sFinalErrMsg)
        delegMsg.ShowLongMsg(sFinalErrMsg)
        Return False

    End Try

End Function

Public Class clsLink
    Public Id$ = ""
    Public Name$ = ""
    Public Table$ = ""
    Public dc As DatabaseSchemaReader.DataSchema.DatabaseConstraint
End Class

End Module