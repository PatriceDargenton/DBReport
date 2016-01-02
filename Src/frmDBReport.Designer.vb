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
        Me.lblUserPW = New System.Windows.Forms.Label()
        Me.tbUserPassword = New System.Windows.Forms.TextBox()
        Me.lblDBServer = New System.Windows.Forms.Label()
        Me.tbDBServer = New System.Windows.Forms.TextBox()
        Me.lblDBProvider = New System.Windows.Forms.Label()
        Me.tbDBProvider = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdDBReport
        '
        Me.cmdDBReport.Location = New System.Drawing.Point(288, 54)
        Me.cmdDBReport.Name = "cmdDBReport"
        Me.cmdDBReport.Size = New System.Drawing.Size(70, 38)
        Me.cmdDBReport.TabIndex = 10
        Me.cmdDBReport.Text = "DB report"
        Me.ToolTip1.SetToolTip(Me.cmdDBReport, "Click to create the database report")
        Me.cmdDBReport.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 192)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(422, 22)
        Me.StatusStrip1.TabIndex = 11
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
        'lblUserPW
        '
        Me.lblUserPW.AutoSize = True
        Me.lblUserPW.Location = New System.Drawing.Point(13, 119)
        Me.lblUserPW.Name = "lblUserPW"
        Me.lblUserPW.Size = New System.Drawing.Size(53, 13)
        Me.lblUserPW.TabIndex = 8
        Me.lblUserPW.Text = "Password"
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
        'lblInfo
        '
        Me.lblInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.Location = New System.Drawing.Point(13, 149)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(397, 31)
        Me.lblInfo.TabIndex = 12
        Me.lblInfo.Text = "Messages"
        '
        'frmDBReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 214)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.lblDBProvider)
        Me.Controls.Add(Me.tbDBProvider)
        Me.Controls.Add(Me.lblDBServer)
        Me.Controls.Add(Me.tbDBServer)
        Me.Controls.Add(Me.lblUserPW)
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
    Friend WithEvents lblUserPW As System.Windows.Forms.Label
    Friend WithEvents tbUserPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblDBServer As System.Windows.Forms.Label
    Friend WithEvents tbDBServer As System.Windows.Forms.TextBox
    Friend WithEvents lblDBProvider As System.Windows.Forms.Label
    Friend WithEvents tbDBProvider As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblInfo As System.Windows.Forms.Label

End Class
