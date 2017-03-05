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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDBReport))
        Me.cmdDBReport = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tbDBName = New System.Windows.Forms.TextBox()
        Me.lblDBName = New System.Windows.Forms.Label()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.tbUserName = New System.Windows.Forms.TextBox()
        Me.lblUserPassword = New System.Windows.Forms.Label()
        Me.tbUserPassword = New System.Windows.Forms.TextBox()
        Me.lblDBServer = New System.Windows.Forms.Label()
        Me.tbDBServer = New System.Windows.Forms.TextBox()
        Me.lblDBProvider = New System.Windows.Forms.Label()
        Me.tbDBProvider = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdResetSettings = New System.Windows.Forms.Button()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.chkDisplayFieldType = New System.Windows.Forms.CheckBox()
        Me.chkDisplayFieldDefaultValue = New System.Windows.Forms.CheckBox()
        Me.chkDisplayLinkName = New System.Windows.Forms.CheckBox()
        Me.chkSortColumns = New System.Windows.Forms.CheckBox()
        Me.chkSortIndexes = New System.Windows.Forms.CheckBox()
        Me.chkSortLinks = New System.Windows.Forms.CheckBox()
        Me.chkAlertNotNullable = New System.Windows.Forms.CheckBox()
        Me.chkDisplayDescription = New System.Windows.Forms.CheckBox()
        Me.chkDisplayTableEngine = New System.Windows.Forms.CheckBox()
        Me.chkDisplayCollation = New System.Windows.Forms.CheckBox()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdDBReport
        '
        Me.cmdDBReport.Location = New System.Drawing.Point(246, 12)
        Me.cmdDBReport.Name = "cmdDBReport"
        Me.cmdDBReport.Size = New System.Drawing.Size(70, 38)
        Me.cmdDBReport.TabIndex = 20
        Me.cmdDBReport.Text = "DB report"
        Me.cmdDBReport.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 313)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(515, 22)
        Me.StatusStrip1.TabIndex = 24
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'tbDBName
        '
        Me.tbDBName.Location = New System.Drawing.Point(86, 64)
        Me.tbDBName.Name = "tbDBName"
        Me.tbDBName.Size = New System.Drawing.Size(129, 20)
        Me.tbDBName.TabIndex = 5
        '
        'lblDBName
        '
        Me.lblDBName.AutoSize = True
        Me.lblDBName.Location = New System.Drawing.Point(12, 67)
        Me.lblDBName.Name = "lblDBName"
        Me.lblDBName.Size = New System.Drawing.Size(51, 13)
        Me.lblDBName.TabIndex = 4
        Me.lblDBName.Text = "DB name"
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Location = New System.Drawing.Point(12, 93)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(58, 13)
        Me.lblUserName.TabIndex = 6
        Me.lblUserName.Text = "User name"
        '
        'tbUserName
        '
        Me.tbUserName.Location = New System.Drawing.Point(86, 90)
        Me.tbUserName.Name = "tbUserName"
        Me.tbUserName.Size = New System.Drawing.Size(129, 20)
        Me.tbUserName.TabIndex = 7
        '
        'lblUserPassword
        '
        Me.lblUserPassword.AutoSize = True
        Me.lblUserPassword.Location = New System.Drawing.Point(13, 119)
        Me.lblUserPassword.Name = "lblUserPassword"
        Me.lblUserPassword.Size = New System.Drawing.Size(53, 13)
        Me.lblUserPassword.TabIndex = 8
        Me.lblUserPassword.Text = "Password"
        '
        'tbUserPassword
        '
        Me.tbUserPassword.Location = New System.Drawing.Point(86, 116)
        Me.tbUserPassword.Name = "tbUserPassword"
        Me.tbUserPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tbUserPassword.Size = New System.Drawing.Size(129, 20)
        Me.tbUserPassword.TabIndex = 9
        '
        'lblDBServer
        '
        Me.lblDBServer.AutoSize = True
        Me.lblDBServer.Location = New System.Drawing.Point(12, 41)
        Me.lblDBServer.Name = "lblDBServer"
        Me.lblDBServer.Size = New System.Drawing.Size(54, 13)
        Me.lblDBServer.TabIndex = 2
        Me.lblDBServer.Text = "DB server"
        '
        'tbDBServer
        '
        Me.tbDBServer.Location = New System.Drawing.Point(86, 38)
        Me.tbDBServer.Name = "tbDBServer"
        Me.tbDBServer.Size = New System.Drawing.Size(129, 20)
        Me.tbDBServer.TabIndex = 3
        '
        'lblDBProvider
        '
        Me.lblDBProvider.AutoSize = True
        Me.lblDBProvider.Location = New System.Drawing.Point(12, 15)
        Me.lblDBProvider.Name = "lblDBProvider"
        Me.lblDBProvider.Size = New System.Drawing.Size(63, 13)
        Me.lblDBProvider.TabIndex = 0
        Me.lblDBProvider.Text = "DB provider"
        '
        'tbDBProvider
        '
        Me.tbDBProvider.Location = New System.Drawing.Point(86, 12)
        Me.tbDBProvider.Name = "tbDBProvider"
        Me.tbDBProvider.Size = New System.Drawing.Size(129, 20)
        Me.tbDBProvider.TabIndex = 1
        '
        'cmdResetSettings
        '
        Me.cmdResetSettings.Location = New System.Drawing.Point(415, 12)
        Me.cmdResetSettings.Name = "cmdResetSettings"
        Me.cmdResetSettings.Size = New System.Drawing.Size(70, 38)
        Me.cmdResetSettings.TabIndex = 22
        Me.cmdResetSettings.Text = "Default"
        Me.ToolTip1.SetToolTip(Me.cmdResetSettings, "Click to reset settings to their default value")
        Me.cmdResetSettings.UseVisualStyleBackColor = True
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.Location = New System.Drawing.Point(12, 204)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(490, 99)
        Me.lblInfo.TabIndex = 23
        Me.lblInfo.Text = "Messages"
        '
        'cmdCancel
        '
        Me.cmdCancel.Enabled = False
        Me.cmdCancel.Location = New System.Drawing.Point(331, 12)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(70, 38)
        Me.cmdCancel.TabIndex = 21
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'chkDisplayFieldType
        '
        Me.chkDisplayFieldType.AutoSize = True
        Me.chkDisplayFieldType.Location = New System.Drawing.Point(246, 66)
        Me.chkDisplayFieldType.Name = "chkDisplayFieldType"
        Me.chkDisplayFieldType.Size = New System.Drawing.Size(105, 17)
        Me.chkDisplayFieldType.TabIndex = 10
        Me.chkDisplayFieldType.Text = "Display field type"
        Me.chkDisplayFieldType.UseVisualStyleBackColor = True
        '
        'chkDisplayFieldDefaultValue
        '
        Me.chkDisplayFieldDefaultValue.AutoSize = True
        Me.chkDisplayFieldDefaultValue.Location = New System.Drawing.Point(246, 93)
        Me.chkDisplayFieldDefaultValue.Name = "chkDisplayFieldDefaultValue"
        Me.chkDisplayFieldDefaultValue.Size = New System.Drawing.Size(146, 17)
        Me.chkDisplayFieldDefaultValue.TabIndex = 11
        Me.chkDisplayFieldDefaultValue.Text = "Display field default value"
        Me.chkDisplayFieldDefaultValue.UseVisualStyleBackColor = True
        '
        'chkDisplayLinkName
        '
        Me.chkDisplayLinkName.AutoSize = True
        Me.chkDisplayLinkName.Location = New System.Drawing.Point(246, 119)
        Me.chkDisplayLinkName.Name = "chkDisplayLinkName"
        Me.chkDisplayLinkName.Size = New System.Drawing.Size(108, 17)
        Me.chkDisplayLinkName.TabIndex = 12
        Me.chkDisplayLinkName.Text = "Display link name"
        Me.chkDisplayLinkName.UseVisualStyleBackColor = True
        '
        'chkSortColumns
        '
        Me.chkSortColumns.AutoSize = True
        Me.chkSortColumns.Location = New System.Drawing.Point(398, 66)
        Me.chkSortColumns.Name = "chkSortColumns"
        Me.chkSortColumns.Size = New System.Drawing.Size(87, 17)
        Me.chkSortColumns.TabIndex = 15
        Me.chkSortColumns.Text = "Sort columns"
        Me.chkSortColumns.UseVisualStyleBackColor = True
        '
        'chkSortIndexes
        '
        Me.chkSortIndexes.AutoSize = True
        Me.chkSortIndexes.Location = New System.Drawing.Point(398, 93)
        Me.chkSortIndexes.Name = "chkSortIndexes"
        Me.chkSortIndexes.Size = New System.Drawing.Size(84, 17)
        Me.chkSortIndexes.TabIndex = 16
        Me.chkSortIndexes.Text = "Sort indexes"
        Me.chkSortIndexes.UseVisualStyleBackColor = True
        '
        'chkSortLinks
        '
        Me.chkSortLinks.AutoSize = True
        Me.chkSortLinks.Location = New System.Drawing.Point(398, 119)
        Me.chkSortLinks.Name = "chkSortLinks"
        Me.chkSortLinks.Size = New System.Drawing.Size(69, 17)
        Me.chkSortLinks.TabIndex = 17
        Me.chkSortLinks.Text = "Sort links"
        Me.chkSortLinks.UseVisualStyleBackColor = True
        '
        'chkAlertNotNullable
        '
        Me.chkAlertNotNullable.AutoSize = True
        Me.chkAlertNotNullable.Location = New System.Drawing.Point(398, 146)
        Me.chkAlertNotNullable.Name = "chkAlertNotNullable"
        Me.chkAlertNotNullable.Size = New System.Drawing.Size(104, 17)
        Me.chkAlertNotNullable.TabIndex = 18
        Me.chkAlertNotNullable.Text = "Alert not nullable"
        Me.chkAlertNotNullable.UseVisualStyleBackColor = True
        '
        'chkDisplayDescription
        '
        Me.chkDisplayDescription.AutoSize = True
        Me.chkDisplayDescription.Location = New System.Drawing.Point(246, 146)
        Me.chkDisplayDescription.Name = "chkDisplayDescription"
        Me.chkDisplayDescription.Size = New System.Drawing.Size(114, 17)
        Me.chkDisplayDescription.TabIndex = 13
        Me.chkDisplayDescription.Text = "Display description"
        Me.chkDisplayDescription.UseVisualStyleBackColor = True
        '
        'chkDisplayTableEngine
        '
        Me.chkDisplayTableEngine.AutoSize = True
        Me.chkDisplayTableEngine.Location = New System.Drawing.Point(246, 173)
        Me.chkDisplayTableEngine.Name = "chkDisplayTableEngine"
        Me.chkDisplayTableEngine.Size = New System.Drawing.Size(121, 17)
        Me.chkDisplayTableEngine.TabIndex = 14
        Me.chkDisplayTableEngine.Text = "Display table engine"
        Me.chkDisplayTableEngine.UseVisualStyleBackColor = True
        '
        'chkDisplayCollation
        '
        Me.chkDisplayCollation.AutoSize = True
        Me.chkDisplayCollation.Location = New System.Drawing.Point(398, 173)
        Me.chkDisplayCollation.Name = "chkDisplayCollation"
        Me.chkDisplayCollation.Size = New System.Drawing.Size(102, 17)
        Me.chkDisplayCollation.TabIndex = 19
        Me.chkDisplayCollation.Text = "Display collation"
        Me.chkDisplayCollation.UseVisualStyleBackColor = True
        '
        'frmDBReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(515, 335)
        Me.Controls.Add(Me.chkDisplayCollation)
        Me.Controls.Add(Me.chkDisplayTableEngine)
        Me.Controls.Add(Me.chkDisplayDescription)
        Me.Controls.Add(Me.chkAlertNotNullable)
        Me.Controls.Add(Me.chkSortLinks)
        Me.Controls.Add(Me.chkSortIndexes)
        Me.Controls.Add(Me.chkSortColumns)
        Me.Controls.Add(Me.chkDisplayLinkName)
        Me.Controls.Add(Me.chkDisplayFieldDefaultValue)
        Me.Controls.Add(Me.chkDisplayFieldType)
        Me.Controls.Add(Me.cmdResetSettings)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.lblDBProvider)
        Me.Controls.Add(Me.tbDBProvider)
        Me.Controls.Add(Me.lblDBServer)
        Me.Controls.Add(Me.tbDBServer)
        Me.Controls.Add(Me.lblUserPassword)
        Me.Controls.Add(Me.tbUserPassword)
        Me.Controls.Add(Me.lblUserName)
        Me.Controls.Add(Me.tbUserName)
        Me.Controls.Add(Me.lblDBName)
        Me.Controls.Add(Me.tbDBName)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.cmdDBReport)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDBReport"
        Me.Text = "DB Report"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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

End Class
