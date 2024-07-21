<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDBReport
    Inherits System.Windows.Forms.Form

    Public Sub New()
        MyBase.New()

        'Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        'Ajoutez une initialisation quelconque après l'appel InitializeComponent()
        If bDebug Then Me.StartPosition = FormStartPosition.CenterScreen

    End Sub

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDBReport))
        cmdDBReport = New Button()
        StatusStrip1 = New StatusStrip()
        ToolStripStatusLabel1 = New ToolStripStatusLabel()
        tbDBName = New TextBox()
        lblDBName = New Label()
        lblUserName = New Label()
        tbUserName = New TextBox()
        lblUserPassword = New Label()
        tbUserPassword = New TextBox()
        lblDBServer = New Label()
        tbDBServer = New TextBox()
        lblDBProvider = New Label()
        tbDBProvider = New TextBox()
        ToolTip1 = New ToolTip(components)
        cmdResetSettings = New Button()
        cbDataProviders = New ComboBox()
        lblInfo = New Label()
        cmdCancel = New Button()
        chkDisplayFieldType = New CheckBox()
        chkDisplayFieldDefaultValue = New CheckBox()
        chkDisplayLinkName = New CheckBox()
        chkSortColumns = New CheckBox()
        chkSortIndexes = New CheckBox()
        chkSortLinks = New CheckBox()
        chkAlertNotNullable = New CheckBox()
        chkDisplayDescription = New CheckBox()
        chkDisplayTableEngine = New CheckBox()
        chkDisplayCollation = New CheckBox()
        chkDisplayLinksBelowEachTable = New CheckBox()
        chkSortTables = New CheckBox()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' cmdDBReport
        ' 
        cmdDBReport.Location = New Point(324, 37)
        cmdDBReport.Margin = New Padding(4, 3, 4, 3)
        cmdDBReport.Name = "cmdDBReport"
        cmdDBReport.Size = New Size(82, 44)
        cmdDBReport.TabIndex = 23
        cmdDBReport.Text = "DB report"
        cmdDBReport.UseVisualStyleBackColor = True
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {ToolStripStatusLabel1})
        StatusStrip1.Location = New Point(0, 406)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Padding = New Padding(1, 0, 16, 0)
        StatusStrip1.Size = New Size(674, 22)
        StatusStrip1.TabIndex = 27
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' ToolStripStatusLabel1
        ' 
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New Size(0, 17)
        ' 
        ' tbDBName
        ' 
        tbDBName.Location = New Point(100, 97)
        tbDBName.Margin = New Padding(4, 3, 4, 3)
        tbDBName.Name = "tbDBName"
        tbDBName.Size = New Size(204, 23)
        tbDBName.TabIndex = 6
        ' 
        ' lblDBName
        ' 
        lblDBName.AutoSize = True
        lblDBName.Location = New Point(14, 100)
        lblDBName.Margin = New Padding(4, 0, 4, 0)
        lblDBName.Name = "lblDBName"
        lblDBName.Size = New Size(55, 15)
        lblDBName.TabIndex = 5
        lblDBName.Text = "DB name"
        ' 
        ' lblUserName
        ' 
        lblUserName.AutoSize = True
        lblUserName.Location = New Point(14, 130)
        lblUserName.Margin = New Padding(4, 0, 4, 0)
        lblUserName.Name = "lblUserName"
        lblUserName.Size = New Size(63, 15)
        lblUserName.TabIndex = 7
        lblUserName.Text = "User name"
        ' 
        ' tbUserName
        ' 
        tbUserName.Location = New Point(100, 127)
        tbUserName.Margin = New Padding(4, 3, 4, 3)
        tbUserName.Name = "tbUserName"
        tbUserName.Size = New Size(204, 23)
        tbUserName.TabIndex = 8
        ' 
        ' lblUserPassword
        ' 
        lblUserPassword.AutoSize = True
        lblUserPassword.Location = New Point(15, 160)
        lblUserPassword.Margin = New Padding(4, 0, 4, 0)
        lblUserPassword.Name = "lblUserPassword"
        lblUserPassword.Size = New Size(57, 15)
        lblUserPassword.TabIndex = 9
        lblUserPassword.Text = "Password"
        ' 
        ' tbUserPassword
        ' 
        tbUserPassword.Location = New Point(100, 157)
        tbUserPassword.Margin = New Padding(4, 3, 4, 3)
        tbUserPassword.Name = "tbUserPassword"
        tbUserPassword.PasswordChar = "*"c
        tbUserPassword.Size = New Size(204, 23)
        tbUserPassword.TabIndex = 10
        ' 
        ' lblDBServer
        ' 
        lblDBServer.AutoSize = True
        lblDBServer.Location = New Point(14, 70)
        lblDBServer.Margin = New Padding(4, 0, 4, 0)
        lblDBServer.Name = "lblDBServer"
        lblDBServer.Size = New Size(56, 15)
        lblDBServer.TabIndex = 3
        lblDBServer.Text = "DB server"
        ' 
        ' tbDBServer
        ' 
        tbDBServer.Location = New Point(100, 67)
        tbDBServer.Margin = New Padding(4, 3, 4, 3)
        tbDBServer.Name = "tbDBServer"
        tbDBServer.Size = New Size(204, 23)
        tbDBServer.TabIndex = 4
        ' 
        ' lblDBProvider
        ' 
        lblDBProvider.AutoSize = True
        lblDBProvider.Location = New Point(14, 40)
        lblDBProvider.Margin = New Padding(4, 0, 4, 0)
        lblDBProvider.Name = "lblDBProvider"
        lblDBProvider.Size = New Size(69, 15)
        lblDBProvider.TabIndex = 1
        lblDBProvider.Text = "DB provider"
        ' 
        ' tbDBProvider
        ' 
        tbDBProvider.Location = New Point(100, 37)
        tbDBProvider.Margin = New Padding(4, 3, 4, 3)
        tbDBProvider.Name = "tbDBProvider"
        tbDBProvider.Size = New Size(204, 23)
        tbDBProvider.TabIndex = 2
        ' 
        ' cmdResetSettings
        ' 
        cmdResetSettings.Location = New Point(522, 37)
        cmdResetSettings.Margin = New Padding(4, 3, 4, 3)
        cmdResetSettings.Name = "cmdResetSettings"
        cmdResetSettings.Size = New Size(82, 44)
        cmdResetSettings.TabIndex = 25
        cmdResetSettings.Text = "Default"
        ToolTip1.SetToolTip(cmdResetSettings, "Click to reset settings to their default value")
        cmdResetSettings.UseVisualStyleBackColor = True
        ' 
        ' cbDataProviders
        ' 
        cbDataProviders.Enabled = False
        cbDataProviders.FormattingEnabled = True
        cbDataProviders.Location = New Point(100, 6)
        cbDataProviders.Margin = New Padding(4, 3, 4, 3)
        cbDataProviders.Name = "cbDataProviders"
        cbDataProviders.Size = New Size(204, 23)
        cbDataProviders.TabIndex = 0
        ToolTip1.SetToolTip(cbDataProviders, "List of installed system database providers")
        ' 
        ' lblInfo
        ' 
        lblInfo.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        lblInfo.Location = New Point(15, 286)
        lblInfo.Margin = New Padding(4, 0, 4, 0)
        lblInfo.Name = "lblInfo"
        lblInfo.Size = New Size(645, 117)
        lblInfo.TabIndex = 26
        lblInfo.Text = "Messages"
        ' 
        ' cmdCancel
        ' 
        cmdCancel.Enabled = False
        cmdCancel.Location = New Point(424, 37)
        cmdCancel.Margin = New Padding(4, 3, 4, 3)
        cmdCancel.Name = "cmdCancel"
        cmdCancel.Size = New Size(82, 44)
        cmdCancel.TabIndex = 24
        cmdCancel.Text = "Cancel"
        cmdCancel.UseVisualStyleBackColor = True
        ' 
        ' chkDisplayFieldType
        ' 
        chkDisplayFieldType.AutoSize = True
        chkDisplayFieldType.Location = New Point(324, 127)
        chkDisplayFieldType.Margin = New Padding(4, 3, 4, 3)
        chkDisplayFieldType.Name = "chkDisplayFieldType"
        chkDisplayFieldType.Size = New Size(116, 19)
        chkDisplayFieldType.TabIndex = 12
        chkDisplayFieldType.Text = "Display field type"
        chkDisplayFieldType.UseVisualStyleBackColor = True
        ' 
        ' chkDisplayFieldDefaultValue
        ' 
        chkDisplayFieldDefaultValue.AutoSize = True
        chkDisplayFieldDefaultValue.Location = New Point(324, 158)
        chkDisplayFieldDefaultValue.Margin = New Padding(4, 3, 4, 3)
        chkDisplayFieldDefaultValue.Name = "chkDisplayFieldDefaultValue"
        chkDisplayFieldDefaultValue.Size = New Size(161, 19)
        chkDisplayFieldDefaultValue.TabIndex = 13
        chkDisplayFieldDefaultValue.Text = "Display field default value"
        chkDisplayFieldDefaultValue.UseVisualStyleBackColor = True
        ' 
        ' chkDisplayLinkName
        ' 
        chkDisplayLinkName.AutoSize = True
        chkDisplayLinkName.Location = New Point(324, 188)
        chkDisplayLinkName.Margin = New Padding(4, 3, 4, 3)
        chkDisplayLinkName.Name = "chkDisplayLinkName"
        chkDisplayLinkName.Size = New Size(119, 19)
        chkDisplayLinkName.TabIndex = 14
        chkDisplayLinkName.Text = "Display link name"
        chkDisplayLinkName.UseVisualStyleBackColor = True
        ' 
        ' chkSortColumns
        ' 
        chkSortColumns.AutoSize = True
        chkSortColumns.Location = New Point(527, 127)
        chkSortColumns.Margin = New Padding(4, 3, 4, 3)
        chkSortColumns.Name = "chkSortColumns"
        chkSortColumns.Size = New Size(96, 19)
        chkSortColumns.TabIndex = 18
        chkSortColumns.Text = "Sort columns"
        chkSortColumns.UseVisualStyleBackColor = True
        ' 
        ' chkSortIndexes
        ' 
        chkSortIndexes.AutoSize = True
        chkSortIndexes.Location = New Point(527, 158)
        chkSortIndexes.Margin = New Padding(4, 3, 4, 3)
        chkSortIndexes.Name = "chkSortIndexes"
        chkSortIndexes.Size = New Size(90, 19)
        chkSortIndexes.TabIndex = 19
        chkSortIndexes.Text = "Sort indexes"
        chkSortIndexes.UseVisualStyleBackColor = True
        ' 
        ' chkSortLinks
        ' 
        chkSortLinks.AutoSize = True
        chkSortLinks.Location = New Point(527, 188)
        chkSortLinks.Margin = New Padding(4, 3, 4, 3)
        chkSortLinks.Name = "chkSortLinks"
        chkSortLinks.Size = New Size(74, 19)
        chkSortLinks.TabIndex = 20
        chkSortLinks.Text = "Sort links"
        chkSortLinks.UseVisualStyleBackColor = True
        ' 
        ' chkAlertNotNullable
        ' 
        chkAlertNotNullable.AutoSize = True
        chkAlertNotNullable.Location = New Point(527, 219)
        chkAlertNotNullable.Margin = New Padding(4, 3, 4, 3)
        chkAlertNotNullable.Name = "chkAlertNotNullable"
        chkAlertNotNullable.Size = New Size(117, 19)
        chkAlertNotNullable.TabIndex = 21
        chkAlertNotNullable.Text = "Alert not nullable"
        chkAlertNotNullable.UseVisualStyleBackColor = True
        ' 
        ' chkDisplayDescription
        ' 
        chkDisplayDescription.AutoSize = True
        chkDisplayDescription.Location = New Point(324, 219)
        chkDisplayDescription.Margin = New Padding(4, 3, 4, 3)
        chkDisplayDescription.Name = "chkDisplayDescription"
        chkDisplayDescription.Size = New Size(126, 19)
        chkDisplayDescription.TabIndex = 15
        chkDisplayDescription.Text = "Display description"
        chkDisplayDescription.UseVisualStyleBackColor = True
        ' 
        ' chkDisplayTableEngine
        ' 
        chkDisplayTableEngine.AutoSize = True
        chkDisplayTableEngine.Location = New Point(324, 250)
        chkDisplayTableEngine.Margin = New Padding(4, 3, 4, 3)
        chkDisplayTableEngine.Name = "chkDisplayTableEngine"
        chkDisplayTableEngine.Size = New Size(132, 19)
        chkDisplayTableEngine.TabIndex = 16
        chkDisplayTableEngine.Text = "Display table engine"
        chkDisplayTableEngine.UseVisualStyleBackColor = True
        ' 
        ' chkDisplayCollation
        ' 
        chkDisplayCollation.AutoSize = True
        chkDisplayCollation.Location = New Point(527, 250)
        chkDisplayCollation.Margin = New Padding(4, 3, 4, 3)
        chkDisplayCollation.Name = "chkDisplayCollation"
        chkDisplayCollation.Size = New Size(113, 19)
        chkDisplayCollation.TabIndex = 22
        chkDisplayCollation.Text = "Display collation"
        chkDisplayCollation.UseVisualStyleBackColor = True
        ' 
        ' chkDisplayLinksBelowEachTable
        ' 
        chkDisplayLinksBelowEachTable.AutoSize = True
        chkDisplayLinksBelowEachTable.Location = New Point(324, 100)
        chkDisplayLinksBelowEachTable.Margin = New Padding(4, 3, 4, 3)
        chkDisplayLinksBelowEachTable.Name = "chkDisplayLinksBelowEachTable"
        chkDisplayLinksBelowEachTable.Size = New Size(183, 19)
        chkDisplayLinksBelowEachTable.TabIndex = 11
        chkDisplayLinksBelowEachTable.Text = "Display links below each table"
        chkDisplayLinksBelowEachTable.UseVisualStyleBackColor = True
        ' 
        ' chkSortTables
        ' 
        chkSortTables.AutoSize = True
        chkSortTables.Location = New Point(527, 100)
        chkSortTables.Margin = New Padding(4, 3, 4, 3)
        chkSortTables.Name = "chkSortTables"
        chkSortTables.Size = New Size(81, 19)
        chkSortTables.TabIndex = 17
        chkSortTables.Text = "Sort tables"
        chkSortTables.UseVisualStyleBackColor = True
        ' 
        ' frmDBReport
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(674, 428)
        Controls.Add(chkSortTables)
        Controls.Add(chkDisplayLinksBelowEachTable)
        Controls.Add(cbDataProviders)
        Controls.Add(chkDisplayCollation)
        Controls.Add(chkDisplayTableEngine)
        Controls.Add(chkDisplayDescription)
        Controls.Add(chkAlertNotNullable)
        Controls.Add(chkSortLinks)
        Controls.Add(chkSortIndexes)
        Controls.Add(chkSortColumns)
        Controls.Add(chkDisplayLinkName)
        Controls.Add(chkDisplayFieldDefaultValue)
        Controls.Add(chkDisplayFieldType)
        Controls.Add(cmdResetSettings)
        Controls.Add(cmdCancel)
        Controls.Add(lblInfo)
        Controls.Add(lblDBProvider)
        Controls.Add(tbDBProvider)
        Controls.Add(lblDBServer)
        Controls.Add(tbDBServer)
        Controls.Add(lblUserPassword)
        Controls.Add(tbUserPassword)
        Controls.Add(lblUserName)
        Controls.Add(tbUserName)
        Controls.Add(lblDBName)
        Controls.Add(tbDBName)
        Controls.Add(StatusStrip1)
        Controls.Add(cmdDBReport)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4, 3, 4, 3)
        Name = "frmDBReport"
        Text = "DB Report"
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub
    Friend WithEvents cmdDBReport As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tbDBName As System.Windows.Forms.TextBox
    Friend WithEvents lblDBName As System.Windows.Forms.Label
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents tbUserName As System.Windows.Forms.TextBox
    Friend WithEvents lblUserPassword As System.Windows.Forms.Label
    Friend WithEvents tbUserPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblDBServer As System.Windows.Forms.Label
    Friend WithEvents tbDBServer As System.Windows.Forms.TextBox
    Friend WithEvents lblDBProvider As System.Windows.Forms.Label
    Friend WithEvents tbDBProvider As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdResetSettings As System.Windows.Forms.Button
    Friend WithEvents chkDisplayFieldType As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayFieldDefaultValue As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayLinkName As System.Windows.Forms.CheckBox
    Friend WithEvents chkSortColumns As System.Windows.Forms.CheckBox
    Friend WithEvents chkSortIndexes As System.Windows.Forms.CheckBox
    Friend WithEvents chkSortLinks As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertNotNullable As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayDescription As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayTableEngine As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayCollation As System.Windows.Forms.CheckBox
    Friend WithEvents cbDataProviders As ComboBox
    Friend WithEvents chkDisplayLinksBelowEachTable As CheckBox
    Friend WithEvents chkSortTables As CheckBox
End Class
