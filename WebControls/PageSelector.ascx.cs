using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuantumConcepts.Common.Web.Utils;

namespace QuantumConcepts.Common.Web.WebControls
{
    public delegate void PageSelectorSelectionChangedEventHandler(object sender, PageSelectorEventArgs e);

    public partial class PageSelector : UserControl
    {
        private const string VS_PAGE_INDEX = "PageIndex";
        private const string VS_PAGE_SIZE = "PageSize";
        private static readonly int[] DEFAULT_PAGE_SIZES = new int[] { 25, 50, 100 };

        public event PageSelectorSelectionChangedEventHandler SelectionChanged;

        private int _currentPageIndex = 0;
        private int? _pageSize = 25;
        private int _pageCount = 1;
        private List<int> _pageSizes = new List<int>(DEFAULT_PAGE_SIZES);

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set
            {
                _currentPageIndex = value;
                UpdateDisplay();
            }
        }

        public int? PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                UpdateDisplay();
            }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                UpdateDisplay();
            }
        }

        public List<int> PageSizes
        {
            get { return _pageSizes; }
            set
            {
                _pageSizes = value;
                UpdateDisplay();
            }
        }

        public bool ShowPageSizeSelector { get { return pageSizePanel.Visible; } set { pageSizePanel.Visible = value; } }

        public int MinimumPageSize { get { return _pageSizes.Min(); } }
        public int MaximumPageSize { get { return _pageSizes.Max(); } }
        
        protected void pageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentPageIndex = 0;

            if ("ALL".Equals(pageSizeDropDownList.SelectedValue))
                _pageSize = null;
            else
            {
                int pageSizeTemp;

                if (int.TryParse(pageSizeDropDownList.SelectedValue, out pageSizeTemp))
                    _pageSize = pageSizeTemp;
            }

            OnSelectionChanged(); 
        }

        protected void pageButton_Click(object sender, EventArgs e)
        {
            if (sender == firstPageButton)
                _currentPageIndex = 0;
            else if (sender == previousPageButton)
                _currentPageIndex--;
            else if (sender == nextPageButton)
                _currentPageIndex++;
            else if (sender == lastPageButton)
                _currentPageIndex = (viewPageDropDownList.Items.Count - 1);

            OnSelectionChanged(); 
        }

        protected void viewPageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentPageIndex = viewPageDropDownList.SelectedIndex;

            OnSelectionChanged();
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            object currentPageIndex = ViewState[VS_PAGE_INDEX];
            object pageSize = ViewState[VS_PAGE_SIZE];

            if (currentPageIndex != null)
                _currentPageIndex = (int)currentPageIndex;

            if (pageSize != null)
                _pageSize = (int)pageSize;
        }

        protected override object SaveViewState()
        {
            ViewState[VS_PAGE_INDEX] = _currentPageIndex;
            ViewState[VS_PAGE_SIZE] = _pageSize;

            return base.SaveViewState();
        }

        private void UpdateDisplay()
        {
            if (_pageCount > 1)
            {
                pageSizeDropDownList.Items.Clear();

                if (_pageSize.HasValue && !_pageSizes.Contains(_pageSize.Value))
                    _pageSizes.Add(_pageSize.Value);
                
                _pageSizes.Sort();

                foreach (int pageSize in _pageSizes)
                    pageSizeDropDownList.Items.Add(new ListItem(pageSize.ToString(), pageSize.ToString()));

                pageSizeDropDownList.Items.Add(new ListItem("All", "ALL"));
                pageSizeDropDownList.SelectedIndex = 0;

                pagePanel.Visible = true;
                firstPageButton.Enabled = (_currentPageIndex > 0);
                previousPageButton.Enabled = (_currentPageIndex > 0);
                viewPageDropDownList.Enabled = true;
                nextPageButton.Enabled = (_currentPageIndex < (_pageCount - 1));
                lastPageButton.Enabled = (_currentPageIndex < (_pageCount - 1));

                viewPageDropDownList.Items.Clear();

                for (int i = 1; i <= _pageCount; i++)
                    viewPageDropDownList.Items.Add(i.ToString());

                if (_currentPageIndex > (viewPageDropDownList.Items.Count - 1))
                    _currentPageIndex = 0;

                viewPageDropDownList.SelectedIndex = _currentPageIndex;
            }
            else
                pagePanel.Visible = false;
        }

        private void OnSelectionChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged(this, new PageSelectorEventArgs(_currentPageIndex, _pageSize));
        }
    }

    public class PageSelectorEventArgs : EventArgs
    {
        private int _currentPageIndex;
        private int? _pageSize;

        public int CurrentPageIndex { get { return _currentPageIndex; } }
        public int? PageSize { get { return _pageSize; } }

        public PageSelectorEventArgs(int currentPageIndex, int? pageSize)
        {
            _currentPageIndex = currentPageIndex;
            _pageSize = pageSize;
        }
    }
}