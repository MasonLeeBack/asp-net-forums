using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;
using System.ComponentModel;
using System.IO;

namespace AspNetForums.Controls {

    /// <summary>
    /// This server control is used to display all the members of the current forum.
    /// </summary>
    [
        ParseChildren(true)
    ]
    public class ShowAllUsers : WebControl, INamingContainer {

        UserList userList;
        Control controlTemplate;
        Paging pager;
        DropDownList sortDirection;
        DropDownList sortBy;
        AlphaPicker letterPicker;
        TextBox searchText;
        Button searchButton;
        bool isSearchMode = false;

        // *********************************************************************
        //  CreateChildControls
        //
        /// <summary>
        /// Loads a user control used as the 'template' for the control. A file
        /// not found exception is thrown if the user control is not found.
        /// </summary>
        /// 
        // ********************************************************************/ 
        protected override void CreateChildControls() {

            // Attempt to load the control. If this fails, we're done
            try {
                controlTemplate = Page.LoadControl(Globals.ApplicationVRoot + "/skins/" + Globals.Skin + "/Skins/Skin-ShowAllUsers.ascx");
            }
            catch (FileNotFoundException e) {
                throw new Exception("The user control skins/Skins/Skin-ShowAllUsers.ascx was not found. Please ensure this file exists in your skins directory");
            }

            // Initialize the control template
            InitializeControlTemplate();

            // Get the total records used in the pager
            pager.PageSize = 50;
            pager.TotalRecords = Users.TotalNumberOfUserAccounts();

            // If we're not a post back, go ahead and set the data
            if (!Page.IsPostBack)
                SetDataSource();

            this.Controls.Add(controlTemplate);
        }

        // *********************************************************************
        //  InitializeControlTemplate
        //
        /// <summary>
        /// Initializes the user control loaded in CreateChildControls. Initialization
        /// consists of finding well known control names and wiring up any necessary events.
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void InitializeControlTemplate() {

            // Find the UserList server control in our template
            userList = (UserList) controlTemplate.FindControl("UserList");
            userList.EnableViewState = false;

            // Find the pager control
            pager = (Paging) controlTemplate.FindControl("Pager");
            pager.PageIndex_Changed += new System.EventHandler(PageIndex_Changed);

            // Find the sort by dropdownlist
            sortBy = (DropDownList) controlTemplate.FindControl("SortBy");
            sortBy.Items.Add(new ListItem("Date Joined", ((int) Users.SortUsersBy.JoinedDate).ToString()));
            sortBy.Items.Add(new ListItem("Date Last Active", ((int) Users.SortUsersBy.LastActiveDate).ToString()));
            sortBy.Items.Add(new ListItem("Posts", ((int) Users.SortUsersBy.Posts).ToString()));
            sortBy.Items.Add(new ListItem("Username", ((int) Users.SortUsersBy.Username).ToString()));
            sortBy.Items.Add(new ListItem("Website", ((int) Users.SortUsersBy.Website).ToString()));
            sortBy.AutoPostBack = true;
            sortBy.SelectedIndexChanged += new System.EventHandler(SortByList_Changed);

            // Find the sorty direction dropdownlist
            sortDirection = (DropDownList) controlTemplate.FindControl("SortDirection");
            sortDirection.AutoPostBack = true;
            sortDirection.Items.Add(new ListItem("Ascending", "0"));
            sortDirection.Items.Add(new ListItem("Descending", "1"));
            sortDirection.SelectedIndexChanged += new System.EventHandler(SortDirection_Changed);

            // Find the alpha picker
            letterPicker = (AlphaPicker) controlTemplate.FindControl("AlphaPicker");
            letterPicker.Letter_Changed += new System.EventHandler(Letter_Changed);

            // Find the search text box and button
            searchText = (TextBox) controlTemplate.FindControl("SeachForUser");
            searchButton = (Button) controlTemplate.FindControl("SearchButton");
            searchButton.Click += new System.EventHandler(Search_Click);

        }

        // *********************************************************************
        //  Letter_Changed
        //
        /// <summary>
        /// Event raised by the alpha-picker to indicate a letter changed
        /// </summary>
        /// 
        // ********************************************************************/
        private void Letter_Changed(Object sender, EventArgs e) {
            pager.PageIndex = 0;
            IsSearchMode = false;
            sortBy.Enabled = true;
            sortDirection.Enabled = true;
            SetDataSource();
        }

        // *********************************************************************
        //  Search_Click
        //
        /// <summary>
        /// Event raised when the user opt's to perform a search
        /// </summary>
        /// 
        // ********************************************************************/
        private void Search_Click(Object sender, EventArgs e) {
            pager.PageIndex = 0;
            IsSearchMode = true;
            sortBy.Enabled = false;
            sortDirection.Enabled = false;
            SetDataSource();
        }

        // *********************************************************************
        //  SetDataSource
        //
        /// <summary>
        /// Private event used to set the datasource based on options selected by the user.
        /// </summary>
        /// 
        // ********************************************************************/
        public void SetDataSource() {
            if (IsSearchMode) {
                userList.DataSource = Users.FindUsersByName(pager.PageIndex, pager.PageSize, searchText.Text);

                // Get the total records used in the pager
                pager.TotalRecords = Users.TotalNumberOfUserAccounts(null, searchText.Text);
            } else {
                Users.SortUsersBy enumSortBy = (Users.SortUsersBy) Convert.ToInt32(sortBy.SelectedItem.Value);
                userList.DataSource = Users.GetAllUsers(pager.PageIndex, pager.PageSize, enumSortBy, Convert.ToInt32(sortDirection.SelectedItem.Value), letterPicker.SelectedLetter);

                // Get the total records used in the pager
                pager.TotalRecords = Users.TotalNumberOfUserAccounts(letterPicker.SelectedLetter, null);
            }


            if (Convert.ToInt32(sortDirection.SelectedItem.Value) == 0) {
                userList.UserCountIsAscending = true;
                userList.UserCount = (pager.PageIndex * pager.PageSize) + 1;
            } else {
                userList.UserCountIsAscending = false;
                userList.UserCount = pager.TotalRecords - (pager.PageIndex * pager.PageSize);
            }

        }

        // *********************************************************************
        //  SortByList_Changed
        //
        /// <summary>
        /// Event raised when the sort by dropdownlist value changes
        /// </summary>
        /// 
        // ********************************************************************/
        public void SortByList_Changed(Object sender, EventArgs e) {
            pager.PageIndex = 0;
            SetDataSource();
        }

        // *********************************************************************
        //  SortDirection_Changed
        //
        /// <summary>
        /// Event raised when the sort direction dropdownlist value changes
        /// </summary>
        /// 
        // ********************************************************************/
        public void SortDirection_Changed(Object sender, EventArgs e) {
            pager.PageIndex = 0;
            SetDataSource();
        }

        // *********************************************************************
        //  PageIndex_Changed
        //
        /// <summary>
        /// Event raised when the selected index of the page has changed.
        /// </summary>
        /// 
        // ********************************************************************/
        private void PageIndex_Changed(Object sender, EventArgs e) {
            SetDataSource();
        }

        // *********************************************************************
        //  OnPreRender
        //
        /// <summary>
        /// Override OnPreRender and databind
        /// </summary>
        /// 
        // ********************************************************************/ 
        protected override void OnPreRender(EventArgs e) {

                DataBind();

        }

        // *********************************************************************
        //  IsSearchMode
        //
        /// <summary>
        /// Private property to determine if we're in search mode or doing a linear
        /// walkthrough of users
        /// </summary>
        /// 
        // ********************************************************************/ 
        private bool IsSearchMode {
            get {
                if (ViewState["IsSearchMode"] == null)
                    return isSearchMode;

                return (bool) ViewState["IsSearchMode"];
            }
            set {
                ViewState["IsSearchMode"] = value;
            }
        }
    }
}