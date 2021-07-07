using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspNetForums;
using AspNetForums.Components;
using System.ComponentModel;
using System.IO;

namespace AspNetForums.Controls {


    public class EditUserProfile : SkinnedForumWebControl {

        string skinFilename = "Skin-EditUserInfo.ascx";
        bool requirePasswordForUpdate =  false;

        // *********************************************************************
        //  EditUserInfo
        //
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        // ********************************************************************/
        public EditUserProfile() {

            // Set the default skin
            if (SkinFilename == null)
                SkinFilename = skinFilename;

        }
	
        
        // *********************************************************************
        //  Initializeskin
        //
        /// <summary>
        /// This method populates the user control used to edit a user's information
        /// </summary>
        /// <param name="control">Instance of the user control to populate</param>
        /// 
        // ***********************************************************************/
        override protected void InitializeSkin(Control skin) {

            Label label;
            TextBox textbox;
            DropDownList dropdownlist;
            CheckBox checkbox;
            Button submit;
            System.Web.UI.WebControls.Image image;

            // Set the name
            label = (Label) skin.FindControl("Username");
            label.Text = ForumUser.Username;

            // Set the email address
            textbox = (TextBox) skin.FindControl("Email");
            textbox.Text = ForumUser.Email;

            // Set the time zone
            dropdownlist = (DropDownList) skin.FindControl("Timezone");
            dropdownlist.Items.FindByValue(ForumUser.Timezone.ToString()).Selected = true;

            // Set the date format
            dropdownlist = (DropDownList) skin.FindControl("DateFormat");
            try {
                dropdownlist.Items.FindByValue(ForumUser.DateFormat).Selected = true;
            } catch (Exception ex) {
                ; // unknown date format, don't set
            }

            // Email tracking option
            checkbox = (CheckBox) skin.FindControl("EmailTracking");
            if (checkbox != null)
                checkbox.Checked = ForumUser.TrackPosts;

            // Hide read threads
            checkbox = (CheckBox) skin.FindControl("UnreadThreadsOnly");
            checkbox.Checked = ForumUser.HideReadThreads;

            // Occupation
            textbox = (TextBox) skin.FindControl("Occupation");
            if (textbox != null)
                textbox.Text = ForumUser.Occupation;

            // Location
            textbox = (TextBox) skin.FindControl("Location");
            if (textbox != null)
                textbox.Text = ForumUser.Location;

            // Interests
            textbox = (TextBox) skin.FindControl("Interests");
            if (textbox != null)
                textbox.Text = ForumUser.Interests;

            // MsnIm
            textbox = (TextBox) skin.FindControl("MsnIm");
            if (textbox != null)
                textbox.Text = ForumUser.MsnIM;

            // YahooIm
            textbox = (TextBox) skin.FindControl("YahooIm");
            if (textbox != null)
                textbox.Text = ForumUser.YahooIM;

            // AolIm
            textbox = (TextBox) skin.FindControl("AolIm");
            if (textbox != null)
                textbox.Text = ForumUser.AolIM;

            // ICQ
            textbox = (TextBox) skin.FindControl("ICQ");
            if (textbox != null)
                textbox.Text = ForumUser.IcqIM;

            // FakeEmail
            textbox = (TextBox) skin.FindControl("FakeEmail");
            textbox.Text = ForumUser.PublicEmail;

            // WebSite
            textbox = (TextBox) skin.FindControl("Website");
            if (textbox != null)
                textbox.Text = ForumUser.Url;

            // Signature
            textbox = (TextBox) skin.FindControl("Signature");
            if (textbox != null)
                textbox.Text = ForumUser.Signature;

            // Style
            dropdownlist = (DropDownList) skin.FindControl("SiteStyle");
            if (dropdownlist != null) {
                foreach (String styleOption in Globals.SiteStyles) {
                    dropdownlist.Items.Add(styleOption);
                }
            }


            // Attempt to apply the user's style ... but it might not exist
            try {
                dropdownlist.Items.FindByText(ForumUser.SiteStyle).Selected = true;
            } catch (Exception exception) {
                ; // Type not found
            }

            // Post view order
            dropdownlist = (DropDownList) skin.FindControl("PostViewOrder");
            dropdownlist.Items.Add(new ListItem("Oldest first", "0"));
            dropdownlist.Items.Add(new ListItem("Newest first", "1"));
            dropdownlist.Items.FindByValue(Convert.ToInt32(ForumUser.ShowPostsAscending).ToString()).Selected = true;

            // Has Icon
            Control hasIcon = skin.FindControl("HasIcon");

            if ((ForumUser.HasIcon) && (hasIcon != null)) {
                image = (System.Web.UI.WebControls.Image) skin.FindControl("CurrentIcon");
                image.ImageUrl = ForumUser.ImageUrl;
                hasIcon.Visible = true;

                ((CheckBox) skin.FindControl("ShowIcon")).Checked = ForumUser.ShowIcon;
            }

/*
            // Are we in administration mode?
            if (Mode == UserInfoEditMode.Admin) {
                ((Control) skin.FindControl("Administration")).Visible = true;

                // Is the user's profile approved?
                checkbox = (CheckBox) skin.FindControl("ProfileApproved");
                checkbox.Checked = ForumUser.IsProfileApproved;

                // Is the user banned
                checkbox = (CheckBox) skin.FindControl("Banned");
                checkbox.Checked = !ForumUser.IsApproved;

                // Is the user trusted
                checkbox = (CheckBox) skin.FindControl("Trusted");
                checkbox.Checked = ForumUser.IsTrusted;



            }
*/

            // Do we require a password for doing an update?
            if (!RequirePasswordForUpdate) {
                // Don't ask for a password when we update
                ((Control) skin.FindControl("PasswordRequired")).Visible = false;

                // Disable the validator
                ((RequiredFieldValidator) skin.FindControl("ValidatePassword")).Enabled = false;
            }

            // Wire-up the button
            submit = (Button) skin.FindControl("Submit");
            submit.Click += new System.EventHandler(UpdateUserInfo_ButtonClick);
        }

        // *********************************************************************
        //  UpdateUserInfo_ButtonClick
        //
        /// <summary>
        /// This event is raised when the user clicks the submit button in the user
        /// control loaded in the DisplayEditMode function. This event is responsible
        /// for processing the form values and writing them back to the database if
        /// necessary.
        /// </summary>
        /// 
        // ********************************************************************/
        private void UpdateUserInfo_ButtonClick(Object sender, EventArgs e) {

            // Ensure the page is valid
            if (!Page.IsValid) 
                return;

            String password = null;
            Control skin;
            TextBox textbox;
            CheckBox checkbox;
            DropDownList dropdown;
            Control control;

            // Find the EditUserInformation user control
            skin = ((Control)sender).Parent;

/*
            // First get the user's password
            if (Mode == UserInfoEditMode.Admin) {
                password = user.Password;
            } else {
                password = ((TextBox) skin.FindControl("Password")).Text;
            }
*/
            // Get the values from the form
            ForumUser.Email = ((TextBox) skin.FindControl("Email")).Text;
            ForumUser.PublicEmail = ((TextBox) skin.FindControl("FakeEmail")).Text;
            
            textbox = (TextBox) skin.FindControl("WebSite");
            if (textbox != null)
                ForumUser.Url = textbox.Text;

            textbox = (TextBox) skin.FindControl("Signature");
            if (textbox != null)
                ForumUser.Signature = textbox.Text;

            checkbox = (CheckBox) skin.FindControl("EmailTracking");
            if (checkbox != null)
                ForumUser.TrackPosts = checkbox.Checked;
            
            ForumUser.Timezone = Convert.ToInt32(((DropDownList) skin.FindControl("Timezone")).SelectedItem.Value);

            // Do we require a password to perform the update?
            if (RequirePasswordForUpdate) {
                textbox = (TextBox) skin.FindControl("Password");
                if (textbox != null)
                    ForumUser.Password = textbox.Text;
            }
            
            textbox = (TextBox) skin.FindControl("Occupation");
            if (textbox != null)
                ForumUser.Occupation = textbox.Text;
            
            textbox = (TextBox) skin.FindControl("Location");
            if (textbox != null)
                ForumUser.Location = textbox.Text;
            
            textbox = (TextBox) skin.FindControl("Interests");
            if (textbox != null)
                ForumUser.Interests = textbox.Text;
            
            textbox = (TextBox) skin.FindControl("MsnIm");
            if (textbox != null)
                ForumUser.MsnIM = textbox.Text;
            
            textbox = (TextBox) skin.FindControl("YahooIm");
            if (textbox != null)
                ForumUser.YahooIM = textbox.Text;
            
            textbox = (TextBox) skin.FindControl("AolIm");
            if (textbox != null)
                ForumUser.AolIM = textbox.Text;
            
            textbox = (TextBox) skin.FindControl("ICQ");
            if (textbox != null)
                ForumUser.IcqIM = textbox.Text;
            
            checkbox = (CheckBox) skin.FindControl("UnreadThreadsOnly");
            if (checkbox != null)
                ForumUser.HideReadThreads = checkbox.Checked;
            
            checkbox = (CheckBox) skin.FindControl("ShowIcon");
            if (checkbox != null)
                ForumUser.ShowIcon = checkbox.Checked;

            dropdown = (DropDownList) skin.FindControl("SiteStyle");
            if (dropdown != null)
                ForumUser.SiteStyle = dropdown.SelectedItem.Value;

            dropdown = (DropDownList) skin.FindControl("DateFormat");
            if (dropdown != null)
                ForumUser.DateFormat = dropdown.SelectedItem.Value;

            dropdown = (DropDownList) skin.FindControl("PostViewOrder");
            if (dropdown != null)
                ForumUser.ShowPostsAscending = Convert.ToBoolean(Convert.ToInt32(dropdown.SelectedItem.Value));

            control = skin.FindControl("HasIcon");
            if (null != control) {
                if (control.Visible == true) {
                    ForumUser.HasIcon = true;
                }
            }
/*
            // If we're in admin mode we have a couple other options to handle
            if (Mode == UserInfoEditMode.Admin) {
                checkbox = (CheckBox) skin.FindControl("ProfileApproved");
                user.IsProfileApproved = checkbox.Checked;
                
                checkbox = (CheckBox) skin.FindControl("Banned");
                user.IsApproved = !checkbox.Checked;

                checkbox = (CheckBox) skin.FindControl("Trusted");
                user.IsTrusted = !checkbox.Checked;

            }
*/

            // Did the user send an icon?
            HtmlInputFile postedFile = (HtmlInputFile) skin.FindControl("Icon");
            if (postedFile != null) {

                if (postedFile.PostedFile.ContentLength != 0) {

                    if (postedFile.PostedFile.ContentType == "image/gif") {
                        postedFile.PostedFile.SaveAs(Page.Server.MapPath(Globals.ApplicationVRoot + "/UserIcons/" + ForumUser.Username + ".gif"));
                        ForumUser.IconExtension = "gif";
                        ForumUser.HasIcon = true;
                    } else if ((postedFile.PostedFile.ContentType == "image/jpg") || (postedFile.PostedFile.ContentType == "image/jpeg")) {
                        postedFile.PostedFile.SaveAs(Page.Server.MapPath(Globals.ApplicationVRoot + "/UserIcons/" + ForumUser.Username + ".jpg"));
                        ForumUser.IconExtension = "jpg";
                        ForumUser.HasIcon = true;
                    } else {
                        Label validatePostedFile = (Label) skin.FindControl("validatePostedFile");
                        validatePostedFile.Text = "File type must be .gif or .jpg";
                        return;
                    }
                }
            }

            // Do update
            try {
                bool updateResult;

                // Perform the actual update
                updateResult = Users.UpdateUserProfile(ForumUser);

                /*
                // If we're admin do another update
                if (Mode == UserInfoEditMode.Admin) {
                    Users.UpdateUserInfoFromAdminPage(user);
                }
                */

                if (updateResult) {
                    // the user was updated successfully, send an email if the password was changed
                    Context.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UserProfileUpdated));
                    Context.Response.End();
                }
                else {
                    RequiredFieldValidator validatePassword;
                    validatePassword = (RequiredFieldValidator) skin.FindControl("ValidatePassword");

                    validatePassword.Text = "Password is invalid";
                    validatePassword.IsValid = false;
                }

            } catch (Exception exception) {
                RequiredFieldValidator validateEmail;
                validateEmail = (RequiredFieldValidator) skin.FindControl("ValidateEmail");

                validateEmail.Text = "Email address exists";
                validateEmail.IsValid = false;

            }

        }

        public bool RequirePasswordForUpdate {
            get { return requirePasswordForUpdate; }
            set { requirePasswordForUpdate = value; }
        }
    }
}