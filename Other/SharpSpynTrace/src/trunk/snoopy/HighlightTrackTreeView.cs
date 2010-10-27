using System;
using System.Windows.Forms;

namespace Genetibase.Debug {

	internal class HighlightTrackTreeView : TreeView {

		TreeNode highlightedNode;
		TreeNode lastRightClickNode;

		protected override void OnMouseDown(MouseEventArgs e) {
			highlightedNode = GetNodeAt(e.X, e.Y);
			if (e.Button == MouseButtons.Right) {
				lastRightClickNode = highlightedNode;
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			highlightedNode = null;
		}

		public TreeNode HiglightedNode {
			get {
				if (highlightedNode != null) {
					return highlightedNode;
				} else {
					return SelectedNode;
				}
			}
		}

		public TreeNode LastRightClickNode {
			get {
				return lastRightClickNode;
			}
		}
	}
}
