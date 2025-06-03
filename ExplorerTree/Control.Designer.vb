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
        Me.TreeView = New System.Windows.Forms.TreeView()
        Me.TreeContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TreeContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeView
        '
        Me.TreeView.ContextMenuStrip = Me.TreeContextMenuStrip
        Me.TreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeView.Location = New System.Drawing.Point(0, 0)
        Me.TreeView.Name = "TreeView"
        Me.TreeView.Size = New System.Drawing.Size(276, 382)
        Me.TreeView.TabIndex = 0
        '
        'TreeContextMenuStrip
        '
        Me.TreeContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.ToolStripSeparator2, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.DeleteToolStripMenuItem, Me.ToolStripSeparator1, Me.NewFolderToolStripMenuItem, Me.RenameToolStripMenuItem})
        Me.TreeContextMenuStrip.Name = "TreeContextMenuStrip"
        Me.TreeContextMenuStrip.Size = New System.Drawing.Size(190, 148)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(186, 6)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(186, 6)
        '
        'ImageList
        '
        Me.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Image = Global.ExplorerTree.My.Resources.Resources._3643772_archive_archives_document_folder_open_113445__1_
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.OpenToolStripMenuItem.Text = "Öffnen"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = Global.ExplorerTree.My.Resources.Resources.copy_icon_128895
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CopyToolStripMenuItem.Text = "Kopieren"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Image = Global.ExplorerTree.My.Resources.Resources.plus_insert_add_rectangle_icon_176436
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.PasteToolStripMenuItem.Text = "Einfügen"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Image = Global.ExplorerTree.My.Resources.Resources._4115230_cancel_close_cross_delete_114048
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.DeleteToolStripMenuItem.Text = "Löschen"
        '
        'NewFolderToolStripMenuItem
        '
        Me.NewFolderToolStripMenuItem.Image = Global.ExplorerTree.My.Resources.Resources._create_new_folder_90671
        Me.NewFolderToolStripMenuItem.Name = "NewFolderToolStripMenuItem"
        Me.NewFolderToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewFolderToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.NewFolderToolStripMenuItem.Text = "Neuer Ordner"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.Image = Global.ExplorerTree.My.Resources.Resources.rename_icon_215315
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.RenameToolStripMenuItem.Text = "Umbenennen"
        '
        'ExplorerTree
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TreeView)
        Me.Name = "ExplorerTree"
        Me.Size = New System.Drawing.Size(276, 382)
        Me.TreeContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TreeView As TreeView
    Friend WithEvents ImageList As ImageList
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents RenameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Public WithEvents TreeContextMenuStrip As ContextMenuStrip
End Class
