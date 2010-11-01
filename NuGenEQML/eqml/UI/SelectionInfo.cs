namespace UI
{
    using Nodes;
    using System;

    internal class SelectionInfo
    {
        public SelectionInfo(Node SelectedNode, int selectedNodeMark, Node MultiSelectNode, int multiSelectMark, bool MultiSelect)
        {
            this.selected_ = null;
            this.multi_ = null;
            this.selectedMark_ = 0;
            this.multiMark_ = 0;
            this.hasMulti = false;
            this.selected_ = SelectedNode;
            this.selectedMark_ = selectedNodeMark;
            this.hasMulti = MultiSelect;
            if (this.hasMulti)
            {
                this.multi_ = null;
                this.multiMark_ = 0;
            }
            else
            {
                this.multi_ = MultiSelectNode;
                this.multiMark_ = multiSelectMark;
            }
        }

        public bool Equals(SelectionInfo SelectionInfo)
        {
            if (this.selected_ != SelectionInfo.selected_)
            {
                return false;
            }
            if (this.selectedMark_ != SelectionInfo.selectedMark_)
            {
                return false;
            }
            if (this.hasMulti != SelectionInfo.hasMulti)
            {
                return false;
            }
            if (this.hasMulti)
            {
                if (this.multi_ != SelectionInfo.multi_)
                {
                    return false;
                }
                if (this.multiMark_ != SelectionInfo.multiMark_)
                {
                    return false;
                }
            }
            return true;
        }


        private Node selected_;
        private Node multi_;
        private int selectedMark_;
        private int multiMark_;
        private bool hasMulti;
    }
}

