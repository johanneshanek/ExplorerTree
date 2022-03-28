Imports System.IO

Public Class ExplorerTree

    Public Event SelectedPathChanged(Path As String)
    Public Event DoubleClicked(FileName As String)

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
        Tv_Explorer.Nodes.Clear()
        For Each drv As DriveInfo In DriveInfo.GetDrives
            AddImageToImgList(drv.Name)
            Dim DriveNode As New TreeNode(drv.Name) 'drv. & "(" & drv.Name & ")")
            With DriveNode
                .Tag = drv.Name
                .ImageKey = drv.Name
                .SelectedImageKey = drv.Name
                .Nodes.Add("Empty")
            End With
            Tv_Explorer.Nodes.Add(DriveNode)
        Next
    End Sub

    Private Sub AddSpecialFolderRootNode(SpecialFolder As SpecialNodeFolders)
        Dim SpecialFolderPath As String = Environment.GetFolderPath(CType(SpecialFolder, Environment.SpecialFolder))
        Dim SpecialFolderName As String = Path.GetFileName(SpecialFolderPath)
        AddImageToImgList(SpecialFolderPath, SpecialFolderName)
        Dim DesktopNode As New TreeNode(SpecialFolderName)
        With DesktopNode
            .Tag = SpecialFolderPath
            .ImageKey = SpecialFolderName
            .SelectedImageKey = SpecialFolderName
            .Nodes.Add("Empty")
        End With
        Tv_Explorer.Nodes.Add(DesktopNode)
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
            Tv_Explorer.Nodes.Add(rootNode)
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
                    Dim FolderNode As New TreeNode(di.Name)
                    With FolderNode
                        .Tag = di.FullName
                        If ImageList.Images.Keys.Contains(di.FullName) Then
                            .ImageKey = di.FullName
                            .SelectedImageKey = di.FullName
                        Else
                            .ImageKey = "Folder"
                            .SelectedImageKey = "Folder"
                        End If
                        .Nodes.Add("*Empty*")
                    End With
                    tn.Nodes.Add(FolderNode)
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

    Public Class DirectoryInfoComparer
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

    Private Sub TV_Explorer_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles Tv_Explorer.BeforeExpand
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

    Private Sub TV_Explorer_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles Tv_Explorer.AfterCollapse
        e.Node.Nodes.Clear()
        e.Node.Nodes.Add("Empty")
    End Sub

    Private Sub TV_Explorer_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles Tv_Explorer.AfterSelect
        RaiseEvent SelectedPathChanged(e.Node.Tag.ToString)
    End Sub

    Private Sub TV_Explorer_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles Tv_Explorer.NodeMouseDoubleClick
        If e.Button = MouseButtons.Left AndAlso File.Exists(e.Node.Tag.ToString) Then
            OpenSelection(e.Node.Tag.ToString)
        End If
    End Sub

    Private Sub OpenSelection(filename As String)
        RaiseEvent DoubleClicked(filename)
    End Sub

    Public Sub Open()
        OpenSelection(Tv_Explorer.SelectedNode.Tag.ToString)
    End Sub

    Private Sub ExplorerTree_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim pixels As Integer = 16
        ImageList.ImageSize = New Size(pixels, pixels)
        Tv_Explorer.ImageList = ImageList
        Tv_Explorer.ItemHeight = pixels + 1

        AddSpecialAndStandardFolderImages()
        AddSpecialFolderRootNode(SpecialNodeFolders.Desktop)
        'AddSpecialFolderRootNode(SpecialNodeFolders.MyDocuments)
        AddDriveRootNodes()
    End Sub

    Public Sub ExpandNode(Path As String)
        Dim opendPath As String = ""
        Dim PathItems() As String = Path.Split("\")
        Dim tvn As TreeNodeCollection = Tv_Explorer.Nodes
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
        Dim selectedPath As String = Tv_Explorer.SelectedNode.Tag.ToString
        If IO.File.Exists(selectedPath) Then
            MsgBox("Bitte Verzeichnis auswählen")
        ElseIf IO.Directory.Exists(selectedPath) Then
            Dim dirname As String = InputBox("Bitte geben Sie einen Namen für den Ordner an",
                                             "Ziel:" & selectedPath, "Neuer Ordner")
            Dim dir As String = selectedPath & "\" & dirname
            If Not IO.Directory.Exists(dir) Then
                IO.Directory.CreateDirectory(dir)
                AddChildNodes(Tv_Explorer.SelectedNode, selectedPath)
            Else
                MsgBox("Der Ordner existiert bereits")
            End If
        End If
    End Sub

    Public Sub RenameFolder()
        Dim selectedPath As String = Tv_Explorer.SelectedNode.Tag.ToString
        If IO.File.Exists(selectedPath) Then
            MsgBox("Bitte Verzeichnis auswählen")
        ElseIf IO.Directory.Exists(selectedPath) Then
            Dim dirname As String = InputBox("Bitte geben Sie neuen Namen für den Ordner an", "Ordner umbenennen",
                                             IO.Path.GetFileName(selectedPath))
            Dim dir As String = Tv_Explorer.SelectedNode.Parent.Tag.ToString & "\" & dirname
            FileSystem.Rename(selectedPath, dir)
            AddChildNodes(Tv_Explorer.SelectedNode, selectedPath)
        End If
    End Sub

    Private Cut As Boolean

    Public Sub CopySelectionToClipBoard()
        Dim selectedPath As String = Tv_Explorer.SelectedNode.Tag.ToString
        If IO.File.Exists(selectedPath) Then
            Dim dir As String = selectedPath
            Dim sc As New Specialized.StringCollection
            sc.Add(dir)
            Clipboard.SetFileDropList(sc)
            Cut = False
        End If
    End Sub

    Public Sub CutSelectionToClipBoard()
        Dim selectedPath As String = Tv_Explorer.SelectedNode.Tag.ToString
        If IO.File.Exists(selectedPath) Then
            Dim sc As New Specialized.StringCollection
            sc.Add(selectedPath)
            Clipboard.SetFileDropList(sc)
            Cut = True
        End If
    End Sub

    Public Sub PasteSelectionFromClipBoard()
        Dim selectedPath As String = Tv_Explorer.SelectedNode.Tag.ToString
        If IO.File.Exists(selectedPath) Then
            MsgBox("Bitte Verzeichnis auswählen")
        ElseIf IO.Directory.Exists(selectedPath) Then
            Dim sc As Specialized.StringCollection
            sc = Clipboard.GetFileDropList()
            Dim dir As String = selectedPath
            For Each s As String In sc
                FileSystem.FileCopy(s, dir & "\" & IO.Path.GetFileName(s))
                If Cut Then IO.File.Delete(s)
            Next
            AddChildNodes(Tv_Explorer.SelectedNode, selectedPath)
        End If
    End Sub

    Public Sub RemoveSelection()
        Dim selectedPath As String = Tv_Explorer.SelectedNode.Tag.ToString
        If IO.File.Exists(selectedPath) Then
            IO.File.Delete(selectedPath)
            AddChildNodes(Tv_Explorer.SelectedNode, selectedPath)
        ElseIf IO.Directory.Exists(selectedPath) Then
            IO.Directory.Delete(selectedPath)
            AddChildNodes(Tv_Explorer.SelectedNode, selectedPath)
        End If
    End Sub

End Class
