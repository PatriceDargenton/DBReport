
' File clsSortDic.vb : Sortable dictionary class
' ------------------

Public Class SortDic(Of TKey, TValue) : Inherits Dictionary(Of TKey, TValue)

    Public Function Sort(Optional sSorting$ = "") As TValue()

        ' Sort the dictionary and return sorted elements

        Dim iNbLignes% = Me.Count
        Dim arrayTvalue(iNbLignes - 1) As TValue
        Dim iNumLigne% = 0
        For Each kvp As KeyValuePair(Of TKey, TValue) In Me
            arrayTvalue(iNumLigne) = kvp.Value
            iNumLigne += 1
        Next

        ' If no sorting is specified, simply return the array
        If sSorting.Length = 0 Then Return arrayTvalue

        ' Sort the dictionary
        Dim comp As New UniversalComparer(Of TValue)(sSorting)
        Array.Sort(Of TValue)(arrayTvalue, comp)
        Return arrayTvalue

    End Function

End Class



