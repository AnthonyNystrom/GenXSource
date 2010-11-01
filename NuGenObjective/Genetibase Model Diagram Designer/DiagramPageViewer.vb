Option Strict Off
Option Explicit On

Imports Genetibase.NugenObjective
Imports Genetibase.NugenObjective.Windows.DiagramClient

Public Class DiagramPageViewer

	Private m_diagram As Diagram
	Private WithEvents m_currentSystem As ModelSystem
	Private WithEvents m_currentPages As ElementCollection(Of Page)
	Private m_selectedElement As Element
	Private m_explorerVisible As Boolean = True
	Private m_propertiesVisible As Boolean = True

	Private Sub ManageToolsContainerVisibility()
		If Not (m_explorerVisible Or m_propertiesVisible) Then
			MainContainer.Panel2Collapsed = True
		Else
			MainContainer.Panel2Collapsed = False
			With ToolsContainer
				.Panel1Collapsed = Not m_explorerVisible
				.Panel2Collapsed = Not m_propertiesVisible
			End With
		End If
	End Sub

	Private Sub SetUpDiagram()
		' Clear properties window
		Properties.SelectedObject = Nothing
		' Initialize the explorers
		InitializeDiagramExplorer()
		InitializePageExplorer()
		' Initially populate the diagram explorer
		PopulateDiagramExplorer()
		' Set up events
		m_currentSystem = m_diagram.System
		' Set up explorers
		Explorers.SelectedTab = PageExplorer
		ManageExplorers()
		' Set up page management
		PopulatePageSelector()
		CurrentPageViewer.SetScale(1.0!)
		SelectPage(m_diagram.Pages(0))
	End Sub

#Region " Explorer Management "
	Private Sub ManageExplorers()
		If m_selectedElement IsNot Nothing Then
			Dim parentNode, currentNode As TreeNode
			Dim currentExplorer As TreeView
			If Explorers.SelectedTab Is PageExplorer Then
				currentExplorer = PageExplorerTreeView
			ElseIf Explorers.SelectedTab Is DiagramExplorer Then
				currentExplorer = DiagramExplorerTreeView
			Else
				currentExplorer = Nothing
			End If

			If currentExplorer IsNot Nothing Then
				If currentExplorer Is DiagramExplorerTreeView _
						AndAlso _
					TypeOf m_selectedElement Is Interaction Then
					Dim currentInteraction As Interaction = DirectCast(m_selectedElement, Interaction)
					parentNode = currentExplorer.Nodes("ModelObject").Nodes(currentInteraction.Object.Key)
				Else
					parentNode = currentExplorer.Nodes(TypeName(m_selectedElement))
				End If

				If parentNode IsNot Nothing Then
					currentNode = parentNode.Nodes(m_selectedElement.Key)

					If currentNode IsNot Nothing Then
						currentNode.EnsureVisible()
						currentExplorer.SelectedNode = currentNode
					End If
				End If
			End If
		End If
	End Sub

	Private Sub InitializeDiagramExplorer()
		With DiagramExplorerTreeView
			With .Nodes
				.Clear()
				.Add("ModelObject", "Objects")
				.Add("State", "States")
				.Add("Role", "Roles")
				.Add("Action", "Actions")
			End With
		End With
	End Sub

	Private Sub PopulateDiagramExplorer()
		Dim objectsNode, statesNode, rolesNode, actionsNode As TreeNode


		InitializeDiagramExplorer()

		With DiagramExplorerTreeView
			objectsNode = .Nodes("ModelObject")
			statesNode = .Nodes("State")
			rolesNode = .Nodes("Role")
			actionsNode = .Nodes("Action")
		End With

		With m_diagram.System
			For Each modelObject As ModelObject In .Objects
				Dim currentObjectNode As TreeNode = _
				objectsNode.Nodes.Add(modelObject.Key, modelObject.Name)
				currentObjectNode.Tag = modelObject
				For Each interaction As Interaction In modelObject.Interactions
					currentObjectNode.Nodes.Add(interaction.Key, interaction.Name).Tag = interaction
				Next
			Next

			For Each state As State In .States
				statesNode.Nodes.Add(state.Key, state.Name).Tag = state
			Next

			For Each role As Role In .Roles
				rolesNode.Nodes.Add(role.Key, role.Name).Tag = role
			Next

			For Each action As Action In .Actions
				actionsNode.Nodes.Add(action.Key, action.Name).Tag = action
			Next
		End With
	End Sub

	Private Sub AddToPageExplorer(Of T As Element)(ByVal element As T)
		Dim parentNode As TreeNode = PageExplorerTreeView.Nodes(TypeName(element))
		parentNode.Nodes.Add(element.Key, element.Name).Tag = element
	End Sub

	Private Sub RemoveFromPageExplorer(Of T As Element)(ByVal element As T)
		Dim parentNode As TreeNode = PageExplorerTreeView.Nodes(TypeName(element))
		parentNode.Nodes.RemoveByKey(element.Key)
	End Sub

	Private Sub InitializePageExplorer()
		With PageExplorerTreeView.Nodes
			.Clear()
			.Add("ModelObject", "Objects")
			.Add("State", "States")
			.Add("Role", "Roles")
			.Add("Action", "Actions")
			.Add("Interaction", "Interactions")
		End With
	End Sub

	Private Sub PopulatePageExplorer()
		PageExplorerTreeView.SuspendLayout()
		Dim currentPage As Page = CurrentPageViewer.Page
		InitializePageExplorer()
		For Each role As String In currentPage.Roles
			AddToPageExplorer(DirectCast(m_diagram.System.Roles(role), Element))
		Next
		For Each action As String In currentPage.Actions
			AddToPageExplorer(DirectCast(m_diagram.System.Actions(action), Element))
		Next
		For Each state As String In currentPage.States
			AddToPageExplorer(DirectCast(m_diagram.System.States(state), Element))
		Next
		For Each modelObject As String In currentPage.Objects
			AddToPageExplorer(DirectCast(m_diagram.System.Objects(modelObject), Element))
		Next
		For Each interaction As String In currentPage.Interactions
			AddToPageExplorer(DirectCast(m_diagram.System.GetInteraction(interaction), Element))
		Next
		PageExplorerTreeView.SuspendLayout()
	End Sub

	Private Overloads Sub UpdateExplorerOnRename(ByVal explorer As TreeView, ByVal nodeName As String, ByVal renamedElement As Element, ByVal oldKey As String)
		With explorer
			Dim parentNode As TreeNode = .Nodes(nodeName)
			If parentNode IsNot Nothing Then
				Dim indexOfRenamedNode As Integer = parentNode.Nodes.IndexOfKey(oldKey)
				parentNode.Nodes.RemoveAt(indexOfRenamedNode)
				parentNode.Nodes.Insert(indexOfRenamedNode, renamedElement.Key, renamedElement.Name).Tag = renamedElement
			End If
		End With
	End Sub

	Private Overloads Sub UpdateExplorerOnRename(ByVal explorer As TreeView, ByVal parentNode As TreeNode, ByVal renamedElement As Element, ByVal oldKey As String)
		With explorer
			If parentNode IsNot Nothing Then
				Dim indexOfRenamedNode As Integer = parentNode.Nodes.IndexOfKey(oldKey)
				parentNode.Nodes.RemoveAt(indexOfRenamedNode)
				parentNode.Nodes.Insert(indexOfRenamedNode, renamedElement.Key, renamedElement.Name).Tag = renamedElement
			End If
		End With

	End Sub
#End Region

#Region " Page management "
	Private Sub DeletePage(ByVal page As Page)
		Dim pageIndex As Integer = m_diagram.Pages.IndexOf(page)
		With m_diagram.Pages
			.RemoveAt(pageIndex)
			If .Count = 0 Then
				page = m_diagram.AddNewPage()
			Else
				If pageIndex >= .Count Then
					page = .Item(.Count - 1)
				Else
					page = .Item(pageIndex)
				End If
			End If
		End With
		PopulatePageSelector()
		SelectPage(page)
	End Sub

	Private Sub SelectPage(ByVal page As Page)
		CurrentPageViewer.Page = page
		PopulatePageExplorer()

		For Each pageTab As ToolStripButton In PageSelector.Items
			If pageTab.Tag Is page Then
				pageTab.Checked = True
			Else
				pageTab.Checked = False
			End If
		Next

		Properties.SelectedObject = page
	End Sub

	Private Sub PageTabMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		Dim selectedPage As Page = TryCast(TryCast(sender, ToolStripItem).Tag, Page)
		If selectedPage IsNot Nothing Then
			SelectPage(selectedPage)
		End If
		If e.Button = System.Windows.Forms.MouseButtons.Right Then
			PagesContextMenuStrip.Show(PageSelector, e.Location)
		End If
	End Sub

	Private Sub PopulatePageSelector()
		PageSelector.Items.Clear()
		For Each page As Page In m_diagram.Pages
			Dim pageTab As New ToolStripButton(page.Name)
			With pageTab
				.Tag = page
				.Visible = True
			End With
			AddHandler pageTab.MouseDown, AddressOf PageTabMouseDown
			PageSelector.Items.Add(pageTab)
		Next
	End Sub
#End Region

#Region " Explorer Management Events "
	Private Sub CurrentPageViewer_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CurrentPageViewer.SelectionChanged
		m_selectedElement = CurrentPageViewer.SelectedElement
		Properties.SelectedObject = m_selectedElement

		ManageExplorers()
	End Sub

	Private Sub Properties_PropertyValueChanged(ByVal s As System.Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles Properties.PropertyValueChanged
		If Not TypeOf Properties.SelectedObject Is Interaction AndAlso _
			Not TypeOf Properties.SelectedObject Is ModelSystem Then
			CurrentPageViewer.Invalidate()
		End If
	End Sub

	Private Sub PageExplorerTreeView_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles PageExplorerTreeView.AfterSelect, DiagramExplorerTreeView.AfterSelect
		If e.Node.Tag IsNot Nothing Then
			m_selectedElement = DirectCast(e.Node.Tag, Element)
			If CurrentPageViewer.SelectedElement IsNot m_selectedElement Then
				CurrentPageViewer.SelectedElement = m_selectedElement
			End If
		End If
	End Sub

	Private Sub CurrentPageViewer_ElementsChanged(ByVal sender As System.Object, ByVal e As PageElementsChangedEventArgs) Handles CurrentPageViewer.ElementsChanged
		For Each el As Element In e.GetChangedElements
			If e.ChangeType = PageElementsChangedEventArgs.TypeOfChange.Added Then
				AddToPageExplorer(el)
			ElseIf e.ChangeType = PageElementsChangedEventArgs.TypeOfChange.Removed Then
				RemoveFromPageExplorer(el)
			End If
		Next
	End Sub

	Private Sub DiagramExplorerTreeView_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagramExplorerTreeView.DoubleClick
		' If editing has been disabled, do nothing
		If Not EditingEnabled Then
			Exit Sub
		End If
		Dim thisNode As TreeNode = DiagramExplorerTreeView.SelectedNode
		If thisNode IsNot Nothing AndAlso thisNode.Tag IsNot Nothing Then
			CurrentPageViewer.Page.AddExistingElement(thisNode.Tag)
		End If
	End Sub

	Private Sub m_currentSystem_ActionAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Action)) Handles m_currentSystem.ActionAdded
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("Action")
		parentNode.Nodes.Add(e.Element.Key, e.Element.Name).Tag = e.Element
	End Sub

	Private Sub m_currentSystem_ActionRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Action)) Handles m_currentSystem.ActionRemoved
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("Action")
		Dim nodeToRemove As TreeNode = parentNode.Nodes(e.Element.Key)
		parentNode.Nodes.Remove(nodeToRemove)
	End Sub

	Private Sub Explorers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Explorers.SelectedIndexChanged
		ManageExplorers()
	End Sub

	Private Sub m_currentSystem_ActionRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_currentSystem.ActionRenamed
		UpdateExplorerOnRename(DiagramExplorerTreeView, "Action", DirectCast(sender, Element), e.OldKey)
		UpdateExplorerOnRename(PageExplorerTreeView, "Action", DirectCast(sender, Element), e.OldKey)
	End Sub

	Private Sub m_currentSystem_InteractionAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction)) Handles m_currentSystem.InteractionAdded
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("ModelObject")
		Dim parentObjectNode As TreeNode = parentNode.Nodes(e.Element.[Object].Key)
		parentObjectNode.Nodes.Add(e.Element.Key, e.Element.Name).Tag = e.Element
	End Sub

	Private Sub m_currentSystem_InteractionChanged(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction)) Handles m_currentSystem.InteractionReplaced
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("ModelObject")
		Dim parentObjectNode As TreeNode = parentNode.Nodes(e.Element.[Object].Key)

		Dim nodePosition As Integer = parentObjectNode.Nodes.IndexOfKey(e.Element.Key)
		parentObjectNode.Nodes.RemoveAt(nodePosition)
		parentObjectNode.Nodes.Insert(nodePosition, e.Element.Key, e.Element.Name)
	End Sub

	Private Sub m_currentSystem_InteractionRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction)) Handles m_currentSystem.InteractionRemoved
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("ModelObject")
		Dim parentObjectNode As TreeNode = parentNode.Nodes(e.Element.[Object].Key)
		Dim nodeToRemove As TreeNode = parentObjectNode.Nodes(e.Element.Key)
		parentObjectNode.Nodes.Remove(nodeToRemove)
	End Sub

	Private Sub m_currentSystem_InterActionRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_currentSystem.InteractionRenamed
		Dim renamedInteraction As Interaction = DirectCast(sender, Interaction)
		If CurrentPageViewer.Page.Interactions.Contains(renamedInteraction.Key) Then
			UpdateExplorerOnRename(PageExplorerTreeView, "Interaction", renamedInteraction, e.OldKey)
		End If
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("ModelObject")
		Dim parentObjectNode As TreeNode = parentNode.Nodes(renamedInteraction.[Object].Key)
		UpdateExplorerOnRename(DiagramExplorerTreeView, parentObjectNode, renamedInteraction, e.OldKey)
	End Sub

	Private Sub m_currentSystem_ObjectAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of ModelObject)) Handles m_currentSystem.ObjectAdded
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("ModelObject")
		parentNode.Nodes.Add(e.Element.Key, e.Element.Name).Tag = e.Element
	End Sub

	Private Sub m_currentSystem_ObjectRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of ModelObject)) Handles m_currentSystem.ObjectRemoved
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("ModelObject")
		Dim nodeToRemove As TreeNode = parentNode.Nodes(e.Element.Key)
		parentNode.Nodes.Remove(nodeToRemove)
	End Sub

	Private Sub m_currentSystem_ObjectRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_currentSystem.ObjectRenamed
		Dim renamedObject As ModelObject = DirectCast(sender, ModelObject)
		UpdateExplorerOnRename(DiagramExplorerTreeView, "ModelObject", renamedObject, e.OldKey)
		UpdateExplorerOnRename(PageExplorerTreeView, "ModelObject", renamedObject, e.OldKey)
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("ModelObject").Nodes(renamedObject.Key)
		For Each interaction As Interaction In renamedObject.Interactions
			parentNode.Nodes.Add(interaction.Key, interaction.Name).Tag = interaction
		Next
	End Sub

	Private Sub m_currentSystem_RoleAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Role)) Handles m_currentSystem.RoleAdded
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("Role")
		parentNode.Nodes.Add(e.Element.Key, e.Element.Name).Tag = e.Element
	End Sub

	Private Sub m_currentSystem_RoleRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Role)) Handles m_currentSystem.RoleRemoved
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("Role")
		Dim nodeToRemove As TreeNode = parentNode.Nodes(e.Element.Key)
		parentNode.Nodes.Remove(nodeToRemove)
	End Sub

	Private Sub m_currentSystem_RoleRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_currentSystem.RoleRenamed
		UpdateExplorerOnRename(DiagramExplorerTreeView, "Role", DirectCast(sender, Element), e.OldKey)
		UpdateExplorerOnRename(PageExplorerTreeView, "Role", DirectCast(sender, Element), e.OldKey)
	End Sub

	Private Sub m_currentSystem_StateAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of State)) Handles m_currentSystem.StateAdded
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("State")
		parentNode.Nodes.Add(e.Element.Key, e.Element.Name).Tag = e.Element
	End Sub

	Private Sub m_currentSystem_StateRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of State)) Handles m_currentSystem.StateRemoved
		Dim parentNode As TreeNode = DiagramExplorerTreeView.Nodes("State")
		Dim nodeToRemove As TreeNode = parentNode.Nodes(e.Element.Key)
		parentNode.Nodes.Remove(nodeToRemove)
	End Sub

	Private Sub m_currentSystem_StateRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_currentSystem.StateRenamed
		UpdateExplorerOnRename(DiagramExplorerTreeView, "State", DirectCast(sender, Element), e.OldKey)
		UpdateExplorerOnRename(PageExplorerTreeView, "State", DirectCast(sender, Element), e.OldKey)
	End Sub
#End Region

#Region "Page Management Events"
	Private Sub PageRenameContextMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageRenameContextMenu.Click
		Dim newName As String = InputBox("Enter new name:", , CurrentPageViewer.Page.Name)
		If newName <> "" Then
			CurrentPageViewer.Page.Name = newName
			PopulatePageSelector()
			SelectPage(CurrentPageViewer.Page)
		End If
	End Sub

	Private Sub PageRemoveContextMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageRemoveContextMenu.Click
		If MsgBox("Are you sure you want to delete the current page?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
			DeletePage(CurrentPageViewer.Page)
		End If
	End Sub

	Private Sub m_currentPages_Added(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Page)) Handles m_currentPages.Added
		PopulatePageSelector()
		SelectPage(e.Element)
	End Sub
#End Region

#Region " Text Editing Events "
	Private Sub CurrentPageViewer_TextEditStarting(ByVal sender As System.Object, ByVal e As TextEditEventArgs) Handles CurrentPageViewer.TextEditStarting
		If TypeOf CurrentPageViewer.SelectedElement Is Interaction Then
			e.Cancel = True
		Else
			e.TextBox.Text = CurrentPageViewer.SelectedElement.Name
		End If
	End Sub

	Private Sub CurrentPageViewer_TextEditCancelled(ByVal sender As System.Object, ByVal e As TextEditEventArgs) Handles CurrentPageViewer.TextEditCancelled

	End Sub

	Private Sub CurrentPageViewer_TextEditComplete(ByVal sender As System.Object, ByVal e As TextEditEventArgs) Handles CurrentPageViewer.TextEditComplete
		CurrentPageViewer.SelectedElement.Name = e.TextBox.Text
	End Sub
#End Region

#Region "Tooltip Event"
	Private Sub CurrentPageViewer_ToolTip(ByVal sender As Object, ByVal e As DiagramClient.TooltipEventArgs) Handles CurrentPageViewer.ToolTip
		Dim th As New DefaultTooltipHelper(e)
		Dim el As Object = e.Element
		th.SetToolTip(el)
	End Sub
#End Region

	Public Property Diagram() As Diagram
		Get
			Return m_diagram
		End Get
		Set(ByVal value As Diagram)
			m_diagram = value
			If value IsNot Nothing Then
				m_currentSystem = value.System
				m_currentPages = value.Pages
				SetUpDiagram()
			End If
		End Set
	End Property

	Public ReadOnly Property CurrentPage() As Page
		Get
			Return CurrentPageViewer.Page
		End Get
	End Property

	Public ReadOnly Property SelectedElement() As Element
		Get
			Return CurrentPageViewer.SelectedElement
		End Get
	End Property

	Public Property EditingEnabled() As Boolean
		Get
			Return PagesContextMenuStrip.Enabled And Properties.Enabled
		End Get
		Set(ByVal value As Boolean)
			PagesContextMenuStrip.Enabled = value
			Properties.Enabled = value
		End Set
	End Property

	Public Property PropertiesWindowVisible() As Boolean
		Get
			Return m_propertiesVisible
		End Get
		Set(ByVal value As Boolean)
			m_propertiesVisible = value
			ManageToolsContainerVisibility()
		End Set
	End Property

	Public Property ExplorerWindowVisible() As Boolean
		Get
			Return m_explorerVisible
		End Get
		Set(ByVal value As Boolean)
			m_explorerVisible = value
			ManageToolsContainerVisibility()
		End Set
	End Property

	Public Sub SetScale(ByVal scaleFactor As Single)
		CurrentPageViewer.SetScale(scaleFactor)
	End Sub

	Public Sub DeleteSelectedElement(ByVal fromDiagram As Boolean)
		' The next line is needed for polymorphic dispatch
		Dim currentElement As Object = CurrentPageViewer.SelectedElement
		If currentElement IsNot Nothing Then
			CurrentPageViewer.Page.RemoveElement(currentElement)
			If fromDiagram Then
				If TypeOf currentElement Is Role Then
					m_currentSystem.Roles.Remove(currentElement)
				ElseIf TypeOf currentElement Is State Then
					m_currentSystem.States.Remove(currentElement)
				ElseIf TypeOf currentElement Is Action Then
					m_currentSystem.Actions.Remove(currentElement)
				ElseIf TypeOf currentElement Is ModelObject Then
					m_currentSystem.Objects.Remove(currentElement)
				ElseIf TypeOf currentElement Is Interaction Then
					Dim currentInteraction As Interaction = DirectCast(currentElement, Interaction)
					currentInteraction.Object.Interactions.Remove(currentInteraction)
				End If
			End If
			CurrentPageViewer.Invalidate()
		End If
	End Sub

	Public Sub LabelEdit()
		CurrentPageViewer.StartEdit()
	End Sub

	Public Sub Cut()
		CurrentPageViewer.Cut()
	End Sub

	Public Sub Copy()
		CurrentPageViewer.Copy()
	End Sub

	Public Sub Paste()
		CurrentPageViewer.Paste()
	End Sub

	Public Sub New()
		Me.InitializeComponent()
	End Sub
End Class
