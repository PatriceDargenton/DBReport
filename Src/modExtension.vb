
Imports System.ComponentModel ' DescriptionAttribute
Imports System.Reflection
Imports System.Runtime.CompilerServices ' Extension

Public Module modExtension

    <Extension>
    Public Function ToDescription(Of TEnum As Structure)(enumValue As TEnum) As String

        Dim memberInfo = GetType(TEnum).GetMember(enumValue.ToString())(0)
        Dim descriptionAttributes = DirectCast(
            memberInfo.GetCustomAttributes(
                GetType(DescriptionAttribute), False), DescriptionAttribute())

        If descriptionAttributes.Length > 0 Then
            Return descriptionAttributes(0).Description
        Else
            Return Nothing
        End If

    End Function

    <Extension>
    Public Function GetValueFromDescription(Of TEnum As Structure)(enumType As Type, description As String) As TEnum

        Dim values = [Enum].GetValues(enumType).Cast(Of TEnum)()

        For Each value As TEnum In values
            Dim memberInfo = enumType.GetMember(value.ToString())(0)
            Dim descriptionAttribute = memberInfo.GetCustomAttribute(Of DescriptionAttribute)()
            If descriptionAttribute?.Description = description Then Return value
        Next

        Throw New ArgumentException($"'{description}' is not a valid description for {enumType.Name}")

    End Function

End Module