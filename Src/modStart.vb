
Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup

            Dim sArg0$ = Microsoft.VisualBasic.Interaction.Command
            Dim bArg = False
            Dim configPath = ""
            If Not String.IsNullOrEmpty(sArg0) Then
                Dim GetDirectory = Path.GetDirectoryName(
                    Reflection.Assembly.GetExecutingAssembly().Location)
                Dim appName = Reflection.Assembly.GetEntryAssembly().GetName().Name
                Dim configFileName = appName & "_" & sArg0 & ".exe.config"
                configPath = GetDirectory & "\" & configFileName
                If File.Exists(configPath) Then bArg = True
            End If

            ' If user configuration exists, delete it, if an argument is chosen
            ' (otherwise the user configuration will be used instead)
            Dim userPath = Configuration.ConfigurationManager.OpenExeConfiguration(
                Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath
            If File.Exists(userPath) AndAlso bArg Then File.Delete(userPath)
            If bArg Then AppConfig.Change(configPath)

        End Sub

    End Class

End Namespace