using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace QuantumConcepts.Common.Web.WebControls
{
    [ParseChildren(true)]
    public partial class PagedRepeater : UserControl
    {
        private IEnumerable _dataSource = null;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Literal Title { get { return titleLiteral; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel Filter { get { return filterPanel; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel Sort { get { return sortPanel; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PageSelector PageSelector { get { return topPageSelector; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PageSelector BottomPageSelector { get { return bottomPageSelector; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Repeater Repeater { get { return repeater; } }

        public IEnumerable DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            header.Visible = !string.IsNullOrEmpty(titleLiteral.Text);
            filterPanel.Visible = (filterPanel.Controls.Count > 0);
            sortPanel.Visible = (sortPanel.Controls.Count > 0);
        }

        protected void pageSelector_SelectionChanged(object sender, PageSelectorEventArgs e)
        {
            bottomPageSelector.PageSize = e.PageSize;
            bottomPageSelector.CurrentPageIndex = e.CurrentPageIndex;
            topPageSelector.PageSize = e.PageSize;
            topPageSelector.CurrentPageIndex = e.CurrentPageIndex;
        }

        public new void DataBind()
        {
            if (_dataSource == null)
            {
                optionsPanel.Visible = false;
                repeater.Visible = false;
                noResultsLabel.Visible = true;
                return;
            }

            PagedDataSource pagedDataSource = new PagedDataSource();

            pagedDataSource.AllowPaging = true;
            pagedDataSource.DataSource = _dataSource;
            pagedDataSource.PageSize = (topPageSelector.PageSize.HasValue ? topPageSelector.PageSize.Value : 0);

            if (topPageSelector.CurrentPageIndex > pagedDataSource.PageCount - 1)
            {
                topPageSelector.CurrentPageIndex = 0;
                bottomPageSelector.CurrentPageIndex = 0;
            }

            pagedDataSource.CurrentPageIndex = topPageSelector.CurrentPageIndex;

            topPageSelector.Visible = (pagedDataSource.DataSourceCount > topPageSelector.MinimumPageSize);
            topPageSelector.PageCount = pagedDataSource.PageCount;

            bottomPageSelectorPanel.Visible = topPageSelector.Visible;
            bottomPageSelector.PageCount = topPageSelector.PageCount;

            repeater.DataSource = pagedDataSource;

            if (pagedDataSource.Count > 0)
                repeater.DataBind();

            if (topPageSelector.Visible)
                topResultsLiteral.Text = (pagedDataSource.FirstIndexInPage + 1) + " to " + (pagedDataSource.FirstIndexInPage + pagedDataSource.Count) + " of " + pagedDataSource.DataSourceCount + " results (page " + (pagedDataSource.CurrentPageIndex + 1) + " of " + pagedDataSource.PageCount + ").";
            else
                topResultsLiteral.Text = (pagedDataSource.DataSourceCount == 1 ? "Only one result" : "All " + pagedDataSource.DataSourceCount + " results") + ".";

            bottomResultsLiteral.Text = topResultsLiteral.Text;

            optionsPanel.Visible = (pagedDataSource.Count > 0);
            repeater.Visible = (pagedDataSource.Count > 0);
            noResultsLabel.Visible = (pagedDataSource.Count == 0);
        }
    }
}