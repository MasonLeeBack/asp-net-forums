/***************************** ModeratedForums Web control **********************************
 * 
 * SUMMARY:
 *		This Web control has two purposes: it can display the forums that a particular user can
 *		moderate, and provide a means for the end user to add and remove from this list of forums
 *		that can be moderated by a particular user; also, it can list the users that can moderate
 *		a particular forum.  However, this control does not allow for users to be added and removed
 *		as moderators for a particular forum.
 *
 * GENERAL COMMENTS:
 *		Chances are, the end developer will never need to use this control from a page itself.
 *		An instance of this control is created in both the UserAdmin and EditForum user
 *		controls.
 *
 ******************************************************************************************/


using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;
using System.ComponentModel;

namespace AspNetForums.Controls.Moderation {
    [ ToolboxItemAttribute(false) ]

    /// <summary>
    /// This Web control has two purposes: it can display the forums that a particular user can
    /// moderate, and provide a means for the end user to add and remove from this list of forums
    /// that can be moderated by a particular user; also, it can list the users that can moderate
    /// a particular forum.  However, this control does not allow for users to be added and removed
    /// as moderators for a particular forum.
    /// </summary>
    /// <remarks>Chances are, the end developer will never need to use this control from a page itself.
    /// An instance of this control is created in both the UserAdmin and EditForum user
    /// controls.</remarks>
    public class ModeratedForums : WebControl, INamingContainer {

        /********** DECLARE PRIVATE CONSTANTS **************/
        // the default ModeratedForumMode setting
        const ModeratedForumMode _defaultMode = ModeratedForumMode.ViewForUser;

        const int _defaultCellPadding = 3;		// the default cell padding for the datagrid
        const int _defaultCellSpacing = 0;		// the default cell spacing for the datagrid

        readonly String _defaultForUserDataGridTitle = "Currently Moderator of the Following Forums:" + Globals.HtmlNewLine;
        readonly String _defaultForForumDataGridTitle = "This Forum Currently is Moderated by the Following Users:" + Globals.HtmlNewLine;
        /***************************************************/


        /********** DECLARE PRIVATE VARIABLES **************/
        // Create the styles
        TableItemStyle _headerStyle = new TableItemStyle();		// the style for the datagrid's header
        TableItemStyle _itemStyle = new TableItemStyle();			// the style for each item in the datagrid

        // Create the datagrid
        DataGrid dgModeratedForums;
        /***************************************************/



        /***********************************************************************
        CreateChildControls Event Handler
        ---------------------------------
            This event handler adds the children controls.
        ************************************************************************/
        protected override void CreateChildControls() {

            if (this.CheckUserPermissions && !((User) Users.GetUserInfo(Context.User.Identity.Name, true)).IsAdministrator)
                // this user isn't an administrator
                Context.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UnableToAdminister));

			
            // make sure we have a username/forumID
            if (Mode == ModeratedForumMode.ViewForUser && Username.Length == 0)
                throw new Exception("When specifying Mode as ViewForUser you must pass in the Username of the user whose forum moderation informaion you wish to view.");

            if (Mode == ModeratedForumMode.ViewForForum && ForumID == -1)
                throw new Exception("When specifying Mode as ViewForForum you must pass in the ForumID of the forum whose moderators you wish to view.");

            // add the title
            Label lblTmp = new Label();
            lblTmp.CssClass = "head";
            lblTmp.Text = DataGridTitle + Globals.HtmlNewLine;
            Controls.Add(lblTmp);


            // add the datagrid
            dgModeratedForums = new DataGrid();

            dgModeratedForums.AutoGenerateColumns = false;
            dgModeratedForums.CellPadding = CellPadding;
            dgModeratedForums.CellSpacing = CellSpacing;
            dgModeratedForums.ItemCommand += new DataGridCommandEventHandler(dgModeratedForums_RemoveForum);

            // build up the DataGrid's columns
            BuildDataGridColumns();
            Controls.Add(dgModeratedForums);

            // if we are viewing this from the User Admin page (that is, we are viewing a list of the
            // forums administrated by a particular user), then we need to show the panel that allows
            // the end user to add/remove forums that this particular user can administer.
            if (Mode == ModeratedForumMode.ViewForUser) {
                Panel tmpPanel = new Panel();
                tmpPanel.ID = "panelAddForum";

                // add the listbox that lists the forums that the user can add
                tmpPanel.Controls.Add(new LiteralControl(Globals.HtmlNewLine + "<b>Add a Forum for this User to Moderate:</b>" + Globals.HtmlNewLine));
				
                ListBox lstTmp = new ListBox();
                lstTmp.ID = "lstForums";
                lstTmp.Rows = 1;
                lstTmp.CssClass = "normalListbox";
                lstTmp.DataValueField = "ForumID";
                lstTmp.DataTextField = "Name";
                tmpPanel.Controls.Add(lstTmp);

                CheckBox chkTmp = new CheckBox();
                chkTmp.ID = "chkEmailNotification";
                chkTmp.Checked = true;
                tmpPanel.Controls.Add(chkTmp);

                tmpPanel.Controls.Add(new LiteralControl("Receive Email Notification when a New Post Needs to be Moderated..." + Globals.HtmlNewLine));

                // add a submit button
                Button btnTmp = new Button();
                btnTmp.Text = "Add Forum";
                btnTmp.CssClass = "normalButton";
                btnTmp.ToolTip = "Click to add the selected forum to this user's list of moderateable forums.";
                btnTmp.Click += new EventHandler(btnAddForum_Click);
                btnTmp.ID = "btnAddForum";
                tmpPanel.Controls.Add(btnTmp);
			
                Controls.Add(tmpPanel);
            }
            else {
                // add a label that explains to moderate the users of a forum, you
                // must visit the user's admin page
                lblTmp = new Label();
                lblTmp.Text = Globals.HtmlNewLine + "In order to alter the moderators for this forum, you must visit the administration page for the specific user.";
                lblTmp.CssClass = "warning";
                Controls.Add(lblTmp);
            }
        }





        /***********************************************************************
        btnAddForum_Click Event Handler
        -------------------------------
            This event handler is fired when the user clicks on the Add Forum button.
            It needs to add a new forum to this users list of moderatable forums.
            Fortunately, the AddModeratedForumForUser statis method of the Users
            class does all of the nitty gritty work for us.
        ************************************************************************/
        private void btnAddForum_Click(Object sender, EventArgs e) {
            // add to the users list of acceptable forums the selected forum
            int iForumID = Convert.ToInt32(((ListBox) FindControl("lstForums")).SelectedItem.Value);
            bool bolEmailNotification = ((CheckBox) FindControl("chkEmailNotification")).Checked;

            ModeratedForum forum = new ModeratedForum();
            forum.Username = Username;
            forum.ForumID = iForumID;
            forum.EmailNotification = bolEmailNotification;

            Users.AddModeratedForumForUser(forum);

            // rebind the datagrid
            RebindData();
        }





        /***********************************************************************
        void BuildDataGridColumns function
        ----------------------------------
            This helper function builds up the columns for the datagrid.  The columns
            that are built-up depend on whether the page is being viewed for a particular
            user or for a particular forum.  If for a user, four columns are constructed:
                1.) The Remove Button
                2.) A hidden column that stores the Forum's ID
                3.) The name of the forum that is moderated
                4.) The date the user was assigned to moderate this forum
			
            If we are viewing the users who moderate a particular forum, only two
            columns are created - the Username of each of the forum's moderators, and
            the Date/Time they were given moderation privledges.
        ************************************************************************/
        private void BuildDataGridColumns() {
            ButtonColumn btcolTmp;
            BoundColumn bndcolTmp;

            if (Mode == ModeratedForumMode.ViewForUser) {
                // add the pushbutton to remove existing forums, but only if we are viewing
                // the list of forums moderated by a particular user
                btcolTmp = new ButtonColumn();
                btcolTmp.ButtonType = ButtonColumnType.PushButton;
                btcolTmp.Text = "Remove";
                btcolTmp.HeaderStyle.CopyFrom(this._headerStyle);
                btcolTmp.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                btcolTmp.ItemStyle.CssClass = "normalButton";
                btcolTmp.HeaderText = "Remove";            
                dgModeratedForums.Columns.Add(btcolTmp);

                // add a hidden bound column to perserve the forum id to remove
                bndcolTmp = new BoundColumn();
                bndcolTmp.Visible = false;
                bndcolTmp.DataField = "ForumID";
                dgModeratedForums.Columns.Add(bndcolTmp);

                // display the forumname
                bndcolTmp = new BoundColumn();
                bndcolTmp.HeaderStyle.CopyFrom(this._headerStyle);
                bndcolTmp.ItemStyle.CopyFrom(this._itemStyle);
                bndcolTmp.DataField = "Name";
                bndcolTmp.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                bndcolTmp.HeaderText = "Forum";
                dgModeratedForums.Columns.Add(bndcolTmp);
            }
            else {	// we are viewing the users who can moderate a particular forum
                // display the user's name
                bndcolTmp = new BoundColumn();
                bndcolTmp.HeaderStyle.CopyFrom(this._headerStyle);
                bndcolTmp.ItemStyle.CopyFrom(this._itemStyle);
                bndcolTmp.DataField = "UserName";
                bndcolTmp.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                bndcolTmp.HeaderText = "User";
                dgModeratedForums.Columns.Add(bndcolTmp);
            }
				
            // display whether or not the user receives email notification
            bndcolTmp = new BoundColumn();			
            bndcolTmp.DataField = "EmailNotification";
            bndcolTmp.HeaderStyle.CopyFrom(this._headerStyle);
            bndcolTmp.ItemStyle.CopyFrom(this._itemStyle);
            bndcolTmp.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            bndcolTmp.HeaderText = "Email Notification";
            bndcolTmp.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            bndcolTmp.DataFormatString = "{0:g}";
            dgModeratedForums.Columns.Add(bndcolTmp);

            // display the date they were given administration of this forum
            bndcolTmp = new BoundColumn();			
            bndcolTmp.DataField = "DateCreated";
            bndcolTmp.HeaderStyle.CopyFrom(this._headerStyle);
            bndcolTmp.ItemStyle.CopyFrom(this._itemStyle);
            bndcolTmp.HeaderText = "Date Assigned";
            bndcolTmp.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            bndcolTmp.DataFormatString = "{0:g}";
            dgModeratedForums.Columns.Add(bndcolTmp);
        }



	

        /***********************************************************************
        dgModeratedForums_RemoveForum Event Handler
        -------------------------------------------
            This event handler is fired when the user clicks on the Remove Forum button.
            This can only happen if we are viewing the forums a particular user can
            moderate.  The RemoveModeratedForumForUser method of the Users class
            does all the leg work for us.
        ************************************************************************/
        private void dgModeratedForums_RemoveForum(Object sender, DataGridCommandEventArgs e) {
            // remove the selected moderated forum for the particular user
            int iForumID = Convert.ToInt32(e.Item.Cells[1].Text);
			
            ModeratedForum forum = new ModeratedForum();
            forum.Username = Username;
            forum.ForumID = iForumID;
			
            Users.RemoveModeratedForumForUser(forum);

            // rebind the datagrid
            RebindData();
        }



        /***********************************************************************
        void RebindData function
        ------------------------
            This function is called from the UserAdmin and EditForum Web controls
            to bind the necessary Web controls within the ModeratedFroums Web control.			
        ************************************************************************/
        /// <summary>
        /// This method is called from the UserAdmin and EditForum Web controls
        /// to bind the necessary Web controls within the ModeratedFroums Web control.
        /// </summary>
        /// <remarks>This method is synonymous to other Web control's DataBind() method.
        /// This Web control, however, does not have a DataSource property.  Rather, this
        /// property is hard-coded in the control.</remarks>
        public void RebindData() {
            // make sure CreateChildControls has been called.
            this.EnsureChildControls();

            // determine if we are viewing a list of forums that a user can moderate...
            if (Mode == ModeratedForumMode.ViewForUser) {
                // databind the moderated forums datagrid
                dgModeratedForums.DataSource = Users.GetForumsModeratedByUser(Username);
                dgModeratedForums.DataBind();
				

                // databind the listbox of forum names
                ModeratedForumCollection forumsNotModerated = Users.GetForumsNotModeratedByUser(Username);

                // did we get any forums back?
                if (forumsNotModerated.Count > 0) {
                    ((Panel) FindControl("panelAddForum")).Visible = true;
                    ((ListBox) FindControl("lstForums")).DataSource = forumsNotModerated;
                    ((ListBox) FindControl("lstForums")).DataBind();
                }
                else {
                    ((Panel) FindControl("panelAddForum")).Visible = false;
                }
            }
            else {	// or if we are viewing a list of users that can moderate a forum
                // databind the list of moderators datagrid
                dgModeratedForums.DataSource = Moderate.GetForumModerators(ForumID);
                dgModeratedForums.DataBind();

            }
        }

		


        /************ PROPERTY SET/GET STATEMENTS **************/
        /// <summary>
        /// Specifies the style for the table headers in the DataGrid.
        /// </summary>
        public TableItemStyle DataGridHeaderStyle {
            get  {  return _headerStyle;  }
        }

        /// <summary>
        /// Specifies the style for each item in the DataGrid.
        /// </summary>
        public TableItemStyle DataGridItemStyle {
            get  {  return _itemStyle;  }
        }

        /// <summary>
        /// When Mode is set to ViewForUser, you must specify the Username of the
        /// user whose list of moderated forums you wish to view.
        /// <seealso cref="Mode"/>
        /// </summary>
        /// <remarks>If Mode is set to ViewForUser and Username is not specified, an
        /// Exception will be thrown.</remarks>
        public String Username {
            get {
                if (ViewState["username"] == null) return "";
                return (String) ViewState["username"];
            }
            set { ViewState["username"] = value; }
        }
		
        /// <summary>
        /// When Mode is set to ViewForForum, you must specify the ForumID of the
        /// forum whose list of moderators you wish to view.
        /// <seealso cref="Mode"/>
        /// </summary>
        /// <remarks>If Mode is set to ViewForForum and ForumID is not specified, an
        /// Exception will be thrown.</remarks>
        public int ForumID {
            get {
                if (ViewState["forumID"] == null) return -1;
                return (int) ViewState["forumID"];
            }
            set { ViewState["forumID"] = value; }
        }

        /// <summary>
        /// Specifies the CellPadding for the DataGrid.
        /// </summary>
        public int CellPadding {
            get {
                if (ViewState["cellPadding"] == null) return _defaultCellPadding;
                return (int) ViewState["cellPadding"];
            }
            set { ViewState["cellPadding"] = value; }
        }

        /// <summary>
        /// Specifies a textual title to display immediately before the DataGrid.
        /// </summary>
        public String DataGridTitle {
            get {
                if (ViewState["dgTitle"] == null) {
                    if (Mode == ModeratedForumMode.ViewForUser)
                        return _defaultForUserDataGridTitle;
                    else
                        return this._defaultForForumDataGridTitle;
                }
                return (String) ViewState["dgTitle"];
            }
            set { ViewState["dgTitle"] = value; }
        }

        /// <summary>
        /// Specifies the CellSpacing to use for the DataGrid.
        /// </summary>
        public int CellSpacing {
            get {
                if (ViewState["cellSpacing"] == null) return _defaultCellSpacing;
                return (int) ViewState["cellSpacing"];
            }
            set { ViewState["cellSpacing"] = value; }
        }

        /// <summary>
        /// Specifies the Mode for the Web control.  This property can have one of two values:
        /// ViewForUser or ViewForForum.  If ViewForUser is selected, a Username must be passed in
        /// and the list of forums that the particular user moderates is displayed, along with the
        /// option to add and remove forums from this list.  If Mode is set to ViewForForum, a
        /// ForumID must be passed in and the list of users that moderate the particular forum are
        /// shown.  The default is ViewForUser.
        /// <seealso cref="Username"/>
        /// <seealso cref="ForumID"/>
        /// </summary>
        /// <remarks>If Mode is set to ViewForUser and the Username property is not set, an
        /// Exception will be thrown.  Likewise, if Mode is set to ViewForForum and the ForumID
        /// property is not set, an Exception will be thrown.</remarks>
        public ModeratedForumMode Mode {
            get {
                if (ViewState["mode"] == null) return _defaultMode;
                return (ModeratedForumMode) ViewState["mode"];
            }
            set { ViewState["mode"] = value; }
        }

        /// <summary>
        /// Indicates if the Web control should check to verify that the user visiting the page
        /// is, indeed, a moderator.
        /// </summary>
        public bool CheckUserPermissions {
            get {
                if (ViewState["checkUserPerm"] == null) return true;
                return (bool) ViewState["checkUserPerm"];
            }
            set { ViewState["checkUserPerm"] = value; }
        }
        /*******************************************************/
    }
}
