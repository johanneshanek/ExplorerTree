Imports System.IO

Public Class ExplorerTree

    Public Event SelectedPathChanged(Path As String)
    Public Event DoubleClicked(FileName As String)

    Public ShowFilesInTree As Boolean
    Public ShowContextMenuStrip As Boolean


    Private Function AddImageToImgList(FullPath As String, Optional SpecialImageKeyName As String = "") As String
        Dim ImgKey As String = If(SpecialImageKeyName = "", FullPath, SpecialImageKeyName)
        Dim LoadFromExt As Boolean = False
        If ImgKey = FullPath AndAlso File.Exists(FullPath) Then
            Dim ext As String = Path.GetExtension(FullPath).ToLower
            If ext <> "" AndAlso ext <> ".exe" AndAlso ext <> ".lnk" AndAlso ext <> ".url" Then
                ImgKey = Path.GetExtension(FullPath).ToLower
                LoadFromExt = True
            End If
        End If
        If Not ImageList.Images.Keys.Contains(ImgKey) Then ImageList.Images.Add(ImgKey, Iconhelper.GetIconImage(If(LoadFromExt, ImgKey, FullPath), IconSizes.Large32x32))
        Return ImgKey
    End Function

    Private Sub AddSpecialAndStandardFolderImages()
        AddImageToImgList(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Folder")
        Dim SpecialFolders As New List(Of Environment.SpecialFolder)
        With SpecialFolders
            .Add(Environment.SpecialFolder.Desktop)
            .Add(Environment.SpecialFolder.MyDocuments)
            .Add(Environment.SpecialFolder.Favorites)
            .Add(Environment.SpecialFolder.Recent)
            .Add(Environment.SpecialFolder.MyMusic)
            .Add(Environment.SpecialFolder.MyVideos)
            .Add(Environment.SpecialFolder.Fonts)
            .Add(Environment.SpecialFolder.History)
            .Add(Environment.SpecialFolder.MyPictures)
            .Add(Environment.SpecialFolder.UserProfile)
        End With
        For Each sf As Environment.SpecialFolder In SpecialFolders
            AddImageToImgList(Environment.GetFolderPath(sf))
        Next
    End Sub

    Private Sub AddDriveRootNodes()
        TreeView.Nodes.Clear()
        For Each drv As DriveInfo In DriveInfo.GetDrives
            Dim DriveNode As TreeNode
            If drv.IsReady Then
                AddImageToImgList(drv.Name)
                If drv.VolumeLabel = "" Then
                    DriveNode = New TreeNode(drv.Name)
                Else
                    DriveNode = New TreeNode(drv.VolumeLabel & " (" & drv.Name & ")")
                End If
                With DriveNode
                    .Tag = drv.Name
                    .ImageKey = drv.Name
                    .SelectedImageKey = drv.Name
                    .Nodes.Add("Empty")
                End With
                TreeView.Nodes.Add(DriveNode)
            End If
        Next
    End Sub

    Private Sub AddSpecialFolderRootNode(SpecialFolder As SpecialNodeFolders)
        Dim SpecialFolderPath As String = Environment.GetFolderPath(CType(SpecialFolder, Environment.SpecialFolder))
        Dim SpecialFolderName As String = Path.GetFileName(SpecialFolderPath)
        AddImageToImgList(SpecialFolderPath, SpecialFolderName)
        If SpecialFolderName = "" Then SpecialFolderName = [Enum].GetName(GetType(SpecialNodeFolders), SpecialFolder)
        Dim DesktopNode As New TreeNode(SpecialFolderName)
        With DesktopNode
            .Tag = SpecialFolderPath
            .ImageKey = SpecialFolderName
            .SelectedImageKey = SpecialFolderName
            .Nodes.Add("Empty")
        End With
        TreeView.Nodes.Add(DesktopNode)
    End Sub

    Private Sub AddCustomFolderRootNode(folderpath As String)
        If Directory.Exists(folderpath) Then
            Dim FolderName As String = New DirectoryInfo(folderpath).Name
            AddImageToImgList(folderpath)
            Dim rootNode As New TreeNode(FolderName)
            With rootNode
                .Tag = folderpath
                .ImageKey = folderpath
                .SelectedImageKey = folderpath
                If Directory.GetDirectories(folderpath).Count > 0 OrElse Directory.GetFiles(folderpath).Count > 0 Then .Nodes.Add("Empty")
            End With
            TreeView.Nodes.Add(rootNode)
        End If
    End Sub

    Private Sub AddChildNodes(tn As TreeNode, DirPath As String)
        Dim DirInfo As New DirectoryInfo(DirPath)
        Try
            Dim Directories As New List(Of DirectoryInfo)
            Directories.AddRange(DirInfo.GetDirectories)
            Directories.Sort(New DirectoryInfoComparer)
            For Each di As DirectoryInfo In Directories
                If Not (di.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                    tn.Nodes.Add(CreateTreeNode(di))
                End If
            Next
            For Each fi As FileInfo In DirInfo.GetFiles
                If Not (fi.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                    Dim ImgKey As String = AddImageToImgList(fi.FullName)
                    Dim FileNode As New TreeNode(fi.Name)
                    With FileNode
                        .Tag = fi.FullName
                        .ImageKey = ImgKey
                        .SelectedImageKey = ImgKey
                    End With
                    tn.Nodes.Add(FileNode)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function CreateTreeNode(newFolder As DirectoryInfo) As TreeNode
        Dim FolderNode As New TreeNode(newFolder.Name)
        With FolderNode
            .Tag = newFolder.FullName
            If ImageList.Images.Keys.Contains(newFolder.FullName) Then
                .ImageKey = newFolder.FullName
                .SelectedImageKey = newFolder.FullName
            Else
                .ImageKey = "Folder"
                .SelectedImageKey = "Folder"
            End If
            .Nodes.Add("*Empty*")
        End With
        Return FolderNode
    End Function

    Private Function CreateTreeNode(fi As FileInfo) As TreeNode
        Dim FolderNode As New TreeNode(fi.Name)
        If Not (fi.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
            Dim ImgKey As String = AddImageToImgList(fi.FullName)
            Dim FileNode As New TreeNode(fi.Name)
            With FileNode
                .Tag = fi.FullName
                .ImageKey = ImgKey
                .SelectedImageKey = ImgKey
            End With
        End If
        Return FolderNode
    End Function

    Private Class DirectoryInfoComparer
        Implements IComparer(Of DirectoryInfo)
        Public Function Compare(ByVal x As DirectoryInfo, ByVal y As DirectoryInfo) As Integer _
            Implements System.Collections.Generic.IComparer(Of DirectoryInfo).Compare
            Return String.Compare(x.Name, y.Name)
        End Function
    End Class

    Private Enum SpecialNodeFolders As Integer
        Desktop = Environment.SpecialFolder.Desktop
        Favorites = Environment.SpecialFolder.Favorites
        History = Environment.SpecialFolder.History
        MyDocuments = Environment.SpecialFolder.MyDocuments
        MyMusic = Environment.SpecialFolder.MyMusic
        MyPictures = Environment.SpecialFolder.MyPictures
        MyVideos = Environment.SpecialFolder.MyVideos
        Recent = Environment.SpecialFolder.Recent
        UserProfile = Environment.SpecialFolder.UserProfile
    End Enum

    Private Sub TreeView_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles TreeView.BeforeExpand
        Dim DrvIsReady As Boolean = (From d As DriveInfo In DriveInfo.GetDrives Where
           d.Name = e.Node.ImageKey Select d.IsReady).FirstOrDefault
        If (e.Node.ImageKey <> "Desktop" AndAlso Not e.Node.ImageKey.Contains(":\")) OrElse DrvIsReady OrElse Directory.Exists(e.Node.ImageKey) Then
            e.Node.Nodes.Clear()
            AddChildNodes(e.Node, e.Node.Tag.ToString)
        ElseIf e.Node.ImageKey = "Desktop" Then
            e.Node.Nodes.Clear()
            Dim PublicDesktopFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)
            Dim CurrentUserDesktopFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            AddChildNodes(e.Node, CurrentUserDesktopFolder)
            AddChildNodes(e.Node, PublicDesktopFolder)
        Else
            e.Cancel = True
            MessageBox.Show("The CD or DVD drive is empty.", "Drive Info...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub TreeView_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles TreeView.AfterCollapse
        e.Node.Nodes.Clear()
        e.Node.Nodes.Add("Empty")
    End Sub

    Private Sub TreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView.AfterSelect
        RaiseEvent SelectedPathChanged(e.Node.Tag.ToString)
    End Sub

    Private Sub TV_Explorer_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView.NodeMouseDoubleClick
        If e.Button = MouseButtons.Left Then RaiseEvent DoubleClicked(e.Node.Tag.ToString) 'AndAlso File.Exists(e.Node.Tag.ToString)
    End Sub

    Private Sub ExplorerTree_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim pixels As Integer = 16
        ImageList.ImageSize = New Size(pixels, pixels)
        TreeView.ImageList = ImageList
        TreeView.ItemHeight = pixels + 1

        AddSpecialAndStandardFolderImages()
        AddDriveRootNodes()
        AddSpecialFolderRootNode(SpecialNodeFolders.Desktop)
        AddSpecialFolderRootNode(SpecialNodeFolders.MyDocuments)
    End Sub

    Public Sub ExpandNode(Path As String)
        Dim opendPath As String = ""
        Dim PathItems() As String = Path.Split("\")
        Dim tvn As TreeNodeCollection = TreeView.Nodes
        For Each item As String In PathItems
            If item <> "" Then
                For Each n As TreeNode In tvn
                    If n.Text <> "" Then
                        If CheckName(n.Text, item) Then
                            opendPath &= item & "\"
                            n.Nodes.Clear()
                            AddChildNodes(n, opendPath)
                            n.Expand()
                            tvn = n.Nodes
                            Exit For
                        End If
                    End If
                Next
            End If
        Next
    End Sub

    Private Function CheckName(name1 As String, name2 As String) As Boolean
        name1 = name1.Replace("\", "")
        name2 = name2.Replace("\", "")
        Return name1 = name2
    End Function

    Public Sub MakeNewFolder()
        If TreeView.SelectedNode IsNot Nothing Then
            Dim fi As New IO.FileInfo(TreeView.SelectedNode.Tag.ToString)
            If fi.Exists Then
                Dim tn As TreeNode = CreateTreeNode(Directory.CreateDirectory(fi.Directory.FullName & "\New Folder"))
                TreeView.SelectedNode.Parent.Nodes.Add(tn)
                TreeView.SelectedNode = tn
            Else
                Dim tn As TreeNode = CreateTreeNode(Directory.CreateDirectory(fi.FullName & "\New Folder"))
                Dim index As Integer = TreeView.SelectedNode.Nodes.Add(tn)
                TreeView.SelectedNode = TreeView.SelectedNode.Nodes(index)
            End If
            TreeView.LabelEdit = True
            TreeView.SelectedNode.BeginEdit()
        End If
    End Sub

    Private Sub TreeView_AfterLabelEdit(sender As Object, e As NodeLabelEditEventArgs) Handles TreeView.AfterLabelEdit
        Dim DirInfo As New DirectoryInfo(e.Node.Tag)
        Dim newFolder As String = DirInfo.Parent.FullName & "\" & e.Label

        Rename(e.Node.Tag, newFolder)

        e.Node.Text = e.Label
        e.Node.Tag = newFolder
        TreeView.LabelEdit = False
    End Sub

    Public Sub OpenSelected()
        If TreeView.SelectedNode IsNot Nothing Then Process.Start(TreeView.SelectedNode.Tag.ToString)
    End Sub

    Public Sub RenameSelected()
        If TreeView.SelectedNode IsNot Nothing Then
            TreeView.LabelEdit = True
            TreeView.SelectedNode.BeginEdit()
        End If
    End Sub

    Public Sub CopySelectionToClipBoard()
        If TreeView.SelectedNode IsNot Nothing Then
            Dim selectedPath As String = TreeView.SelectedNode.Tag.ToString
            'Nur für Dateien!!!
            If IO.File.Exists(selectedPath) Then
                Clipboard.SetFileDropList(New Specialized.StringCollection From {selectedPath})
            End If
        End If
    End Sub

    Public Sub PasteSelectionFromClipBoard()
        If TreeView.SelectedNode IsNot Nothing Then
            Dim fi As New IO.FileInfo(TreeView.SelectedNode.Tag.ToString)
            Dim DestinationFolder As String
            If fi.Exists Then
                'Es ist eine Datei markiert worden
                DestinationFolder = fi.Directory.FullName
                Dim sc As Specialized.StringCollection
                sc = Clipboard.GetFileDropList()
                For Each s As String In sc
                    Dim destfilename As String = DestinationFolder & "\Kopie " & IO.Path.GetFileName(s)
                    FileSystem.FileCopy(s, destfilename)
                    TreeView.SelectedNode.Parent.Nodes.Add(CreateTreeNode(New FileInfo(destfilename)))
                Next
            Else
                DestinationFolder = fi.FullName
                Dim sc As Specialized.StringCollection
                sc = Clipboard.GetFileDropList()
                For Each s As String In sc
                    Dim destfilename As String = DestinationFolder & "\Kopie " & IO.Path.GetFileName(s)
                    FileSystem.FileCopy(s, destfilename)
                    TreeView.SelectedNode.Nodes.Add(CreateTreeNode(New FileInfo(destfilename)))
                Next
            End If
        End If
    End Sub

    Public Sub RemoveSelection()
        If TreeView.SelectedNode IsNot Nothing Then
            Dim selectedPath As String = TreeView.SelectedNode.Tag.ToString
            Dim dinfo As New DirectoryInfo(TreeView.SelectedNode.Tag.ToString)
            If IO.File.Exists(selectedPath) Then
                If MsgBox("Soll " & dinfo.Name & " gelöscht werden?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    IO.File.Delete(selectedPath)
                    TreeView.Nodes.Remove(TreeView.SelectedNode)
                End If
            ElseIf IO.Directory.Exists(selectedPath) Then
                If MsgBox("Soll " & dinfo.FullName & " mit allen Dateien und Unterordnern gelöscht werden?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    IO.Directory.Delete(selectedPath, True)
                    TreeView.Nodes.Remove(TreeView.SelectedNode)
                End If
            End If
        End If
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        CopySelectionToClipBoard()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        PasteSelectionFromClipBoard()
    End Sub

    Private Sub RenameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem.Click
        RenameSelected()
    End Sub

    Private Sub NewFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewFolderToolStripMenuItem.Click
        MakeNewFolder()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        OpenSelected()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        RemoveSelection()
    End Sub

End Class
