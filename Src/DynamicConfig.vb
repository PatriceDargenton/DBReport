
' From https://www.c-sharpcorner.com/UploadFile/4876ca/making-our-component-to-be-dynamically-loading-the-config-fi/
' Making Our Component to be Dynamically Loading the Config File, 2012

Imports System.Configuration
Imports System.Reflection

Public MustInherit Class AppConfig : Implements IDisposable

    Public Shared Function Change(path As String) As AppConfig
        Return New ChangeAppConfig(path)
    End Function

    Public MustOverride Sub Dispose() Implements IDisposable.Dispose

    Private Class ChangeAppConfig : Inherits AppConfig

        Private ReadOnly oldConfig As String =
            AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString()
        Private disposedValue As Boolean

        Public Sub New(path As String)
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path)
            ResetConfigMechanism()
        End Sub

        Public Overrides Sub Dispose()

            If Not disposedValue Then
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", oldConfig)
                ResetConfigMechanism()
                disposedValue = True
            End If
            GC.SuppressFinalize(Me)

        End Sub

        Private Shared Sub ResetConfigMechanism()

            GetType(ConfigurationManager).GetField("s_initState",
                BindingFlags.NonPublic Or BindingFlags.Static).SetValue(Nothing, 0)

            GetType(ConfigurationManager).GetField("s_configSystem",
                BindingFlags.NonPublic Or BindingFlags.Static).SetValue(Nothing, Nothing)

            GetType(ConfigurationManager).Assembly.GetTypes().Where(
                Function(x) x.FullName =
                    "System.Configuration.ClientConfigPaths").First().GetField("s_current",
                        BindingFlags.NonPublic Or BindingFlags.Static).SetValue(Nothing, Nothing)

        End Sub

    End Class

End Class