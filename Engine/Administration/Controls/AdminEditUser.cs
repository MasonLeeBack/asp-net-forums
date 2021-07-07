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
using System.IO;

namespace AspNetForums.Controls.Admin {
    [
    ParseChildren(true)
    ]

    /// <summary>
    /// This server control allows the administrator to edit a user's profile
    /// </summary>
    public class AdminEditUser : UserProfile {

        User user;
        string siteStyle;
        Control controlTemplate;
        UserList userList;
        AlphaPicker picker;

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

            // set the mode to edit
            base.Mode = UserInfoEditMode.Edit;

            // Initialize the base edit template
            base.InitializeEditTemplate();

            // Attempt to get the user name
            if (Page.Request.IsAuthenticated)
                user = Users.GetUserInfo(Page.User.Identity.Name, true);

            // Set the siteStyle for the page
            if (user != null)
                siteStyle = user.SiteStyle;
            else
                siteStyle = Globals.SiteStyle;

            // Attempt to load the control. If this fails, we're done
            try {
                controlTemplate = Page.LoadControl(Globals.ApplicationVRoot + "/skins/" + Globals.Skin + "/Controls/Template-AdminPickUser.ascx");
            }
            catch (FileNotFoundException e) {
                throw new Exception("The user control skins/Controls/Template-AdminPickUser.ascx was not found. Please ensure this file exists in your skins directory");
            }

            // Initialize the control template
            InitializeControlTemplate();

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

            // Find the picker control
            picker = (AlphaPicker) controlTemplate.FindControl("AlphaPicker");


            // Find the user list control
            userList = (UserList) controlTemplate.FindControl("UserList");

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
            UserCollection users;
            
            users = Users.GetUsersByFirstCharacter(picker.SelectedLetter);
            userList.DataSource = users;
            DataBind();
        }


    }
}
