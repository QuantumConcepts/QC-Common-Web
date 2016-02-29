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
    [Themeable(true)]
    public partial class PagedDataGrid : UserControl
    {
        public delegate void NeedsDataBindingEventHandler(object sender, EventArgs e);
        public delegate bool ItemAlteredEventHandler(object sender, DataGridItemEventArgs e);
        public delegate bool ItemCommandEventHandler(object sender, DataGridCommandEventArgs e);

        public event NeedsDataBindingEventHandler NeedsDataBinding;
        public event ItemAlteredEventHandler ItemAdded;
        public event ItemAlteredEventHandler ItemSaved;
        public event ItemAlteredEventHandler ItemDeleted;
        public event ItemCommandEventHandler ItemCommand;
        
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
        public DataGrid DataGrid { get { return dataGrid; } }

        public IEnumerable DataSource { get; set; }

        public bool EnableInlineEdit { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                header.Visible = !string.IsNullOrEmpty(titleLiteral.Text);
                filterPanel.Visible = (filterPanel.Controls.Count > 0);
                sortPanel.Visible = (sortPanel.Controls.Count > 0);

                if (this.DataSource == null)
                    OnNeedsDataBinding();
            }
        }

        protected void pageSelector_SelectionChanged(object sender, PageSelectorEventArgs e)
        {
            topPageSelector.PageSize = e.PageSize;
            topPageSelector.CurrentPageIndex = e.CurrentPageIndex;
            bottomPageSelector.PageSize = e.PageSize;
            bottomPageSelector.CurrentPageIndex = e.CurrentPageIndex;

            OnNeedsDataBinding();
        }

        protected void dataGrid_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            if (this.EnableInlineEdit)
            {
                if ("Add".Equals(e.CommandName))
                {
                    if (OnItemAdded(new DataGridItemEventArgs(e.Item)))
                        OnNeedsDataBinding();
                }
                else if ("Edit".Equals(e.CommandName))
                {
                    dataGrid.EditItemIndex = e.Item.ItemIndex;
                    OnNeedsDataBinding();
                }
                else if ("Delete".Equals(e.CommandName))
                {
                    if (OnItemDeleted(new DataGridItemEventArgs(e.Item)))
                        OnNeedsDataBinding();
                }
                else if ("Save".Equals(e.CommandName))
                {
                    if (OnItemSaved(new DataGridItemEventArgs(e.Item)))
                    {
                        dataGrid.EditItemIndex = -1;
                        OnNeedsDataBinding();
                    }
                }
                else if ("Cancel".Equals(e.CommandName))
                {
                    dataGrid.EditItemIndex = -1;
                    OnNeedsDataBinding();
                }
                else if (OnItemCommand(e))
                    OnNeedsDataBinding();
            }
            else
                OnItemCommand(e);
        }

        public void Bind(IEnumerable dataSource)
        {
            this.DataSource = dataSource;
            this.DataBind();
        }

        public new void DataBind()
        {
            if (this.DataSource == null)
            {
                dataGrid.Visible = false;
                noResultsLabel.Visible = true;
                return;
            }

            PagedDataSource pagedDataSource = new PagedDataSource();

            pagedDataSource.AllowPaging = true;
            pagedDataSource.DataSource = this.DataSource;
            pagedDataSource.PageSize = (topPageSelector.PageSize.HasValue ? topPageSelector.PageSize.Value : 0);

            if (topPageSelector.CurrentPageIndex > pagedDataSource.PageCount - 1)
            {
                topPageSelector.CurrentPageIndex = 0;
                bottomPageSelector.CurrentPageIndex = 0;
            }

            pagedDataSource.CurrentPageIndex = topPageSelector.CurrentPageIndex;

            topPageSelector.PageCount = pagedDataSource.PageCount;
            topPageSelector.Visible = (pagedDataSource.DataSourceCount > topPageSelector.MinimumPageSize);

            bottomPageSelector.CurrentPageIndex = topPageSelector.CurrentPageIndex;
            bottomPageSelector.PageSize = topPageSelector.PageSize;
            bottomPageSelector.PageSizes = topPageSelector.PageSizes;
            bottomPageSelector.PageCount = pagedDataSource.PageCount;
            bottomPageSelectorPanel.Visible = topPageSelector.Visible;

            dataGrid.DataSource = pagedDataSource;

            if (pagedDataSource.Count > 0)
                dataGrid.DataBind();

            if (topPageSelector.Visible)
                topResultsLiteral.Text = "Showing " + (pagedDataSource.FirstIndexInPage + 1) + " to " + (pagedDataSource.FirstIndexInPage + pagedDataSource.Count) + " of " + pagedDataSource.DataSourceCount + " results (page " + (pagedDataSource.CurrentPageIndex + 1) + " of " + pagedDataSource.PageCount + ").";
            else
                topResultsLiteral.Text = (pagedDataSource.DataSourceCount == 1 ? "Showing one result" : "Showing all " + pagedDataSource.DataSourceCount + " results") + ".";

            bottomResultsLiteral.Text = topResultsLiteral.Text;

            dataGrid.Visible = (pagedDataSource.Count > 0);
            noResultsLabel.Visible = (pagedDataSource.Count == 0);
        }

        public int GetKeyAsInt(int itemIndex)
        {
            return (int)dataGrid.DataKeys[itemIndex];
        }

        public string GetEditFieldAsString(DataGridItem item, int cellIndex)
        {
            TextBox control = item.Cells[cellIndex].Controls.OfType<TextBox>().FirstOrDefault();

            if (control == null)
                throw new ApplicationException("Could not locate edit control.");

            return control.Text;
        }

        public bool GetEditFieldAsBool(DataGridItem item, int cellIndex)
        {
            CheckBox control = item.Cells[cellIndex].Controls.OfType<CheckBox>().FirstOrDefault();

            if (control == null)
                throw new ApplicationException("Could not locate edit control.");

            return control.Checked;
        }

        public T GetEditFieldAsEnumeration<T>(DataGridItem item, int cellIndex)
        {
            DropDownList control = item.Cells[cellIndex].Controls.OfType<DropDownList>().FirstOrDefault();

            if (control == null)
                throw new ApplicationException("Could not locate edit control.");

            return (T)Enum.Parse(typeof(T), control.SelectedValue);
        }

        public int GetEditFieldAsInt(DataGridItem item, int cellIndex)
        {
            TextBox textBoxControl = item.Cells[cellIndex].Controls.OfType<TextBox>().FirstOrDefault();
            DropDownList dropDownListControl = item.Cells[cellIndex].Controls.OfType<DropDownList>().FirstOrDefault();

            if (textBoxControl == null && dropDownListControl == null)
                throw new ApplicationException("Could not locate edit control.");

            if (textBoxControl != null)
                return int.Parse(textBoxControl.Text);

            return int.Parse(dropDownListControl.SelectedValue);
        }

        protected void OnNeedsDataBinding()
        {
            if (NeedsDataBinding != null)
                NeedsDataBinding(this, EventArgs.Empty);
        }

        protected bool OnItemAdded(DataGridItemEventArgs e)
        {
            if (ItemAdded != null)
                return ItemAdded(this, e);

            return false;
        }

        protected bool OnItemSaved(DataGridItemEventArgs e)
        {
            if (ItemSaved != null)
                return ItemSaved(this, e);

            return false;
        }

        protected bool OnItemDeleted(DataGridItemEventArgs e)
        {
            if (ItemDeleted != null)
                return ItemDeleted(this, e);

            return false;
        }

        private bool OnItemCommand(DataGridCommandEventArgs e)
        {
            if (ItemCommand != null)
                return ItemCommand(this, e);

            return false;
        }
    }
}