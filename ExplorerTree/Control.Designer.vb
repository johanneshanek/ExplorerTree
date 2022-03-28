<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExplorerTree
    Inherits System.Windows.Forms.UserControl

    'UserControl1 overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Tv_Explorer = New System.Windows.Forms.TreeView()
        Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.SuspendLayout()
        '
        'Tv_Explorer
        '
        Me.Tv_Explorer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Tv_Explorer.Location = New System.Drawing.Point(0, 0)
        Me.Tv_Explorer.Name = "Tv_Explorer"
        Me.Tv_Explorer.Size = New System.Drawing.Size(276, 382)
        Me.Tv_Explorer.TabIndex = 0
        '
        'ImageList
        '
        Me.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'ExplorerTree
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Tv_Explorer)
        Me.Name = "ExplorerTree"
        Me.Size = New System.Drawing.Size(276, 382)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Tv_Explorer As TreeView
    Friend WithEvents ImageList As ImageList
End Class
