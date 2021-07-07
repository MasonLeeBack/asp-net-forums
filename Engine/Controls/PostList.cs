using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Controls.Moderation;
using AspNetForums.Components;
using System.ComponentModel;

namespace AspNetForums.Controls {

    /// <summary>
    /// This Web control displays a thread in a flat display.  The developer must pass in
    /// either a PostID.  If a PostID is passed in, the thread that that Post belongs
    /// to is constructed.
    /// </summary>
    [
        ParseChildren(true)
    ]
    public class PostList : DataList {

        User user;
        String siteStyle;

        // *********************************************************************
        //  CreateChildControls
        //
        /// <summary>
        /// This event handler adds the children controls and is resonsible
        /// for determining the template type used for the control.
        /// </summary>
        /// 
        // ********************************************************************/ 
        protected override void CreateChildControls() {

            // Do we have a user?
            if (HttpContext.Current.Request.IsAuthenticated) {
                user = Users.GetUserInfo(HttpContext.Current.User.Identity.Name, true);
            }

            // Set the siteStyle for the page
            if (user != null)
                siteStyle = user.SiteStyle;
            else
                siteStyle = Globals.SiteStyle;

            // Apply Template
            ApplyTemplates();

            // Viewstate is disabled
            EnableViewState = false;

        }

        // *********************************************************************
        //  HandleDataBindingForPostCell
        //
        /// <summary>
        /// Databinds the post cell
        /// </summary>
        /// <remarks>
        /// Used only if a user defined template is not provided.
        /// </remarks>
        /// 
        // ********************************************************************/
        private void HandleDataBindingForPostCell(Object sender, EventArgs e) {
            Table table;
            TableRow tr;
            TableCell td;
            Label label;
            string dateFormat; 
            DateTime postDateTime;
            User postUser;

            // Get the sender
            TableCell postInfo = (TableCell) sender;
            DataListItem container = (DataListItem) postInfo.NamingContainer;
            Post post = (Post) container.DataItem;

            // Get the user that created the post
            postUser = Users.GetUserInfo(post.Username, false);

            // Create the table
            table = new Table();
            table.CellPadding = 3;
            table.CellSpacing = 0;
            table.Width = Unit.Percentage(100);
            
            // Row 1
            tr = new TableRow();
            td = new TableCell();
            td.CssClass = "forumRowHighlight";

            // Add in Subject
            label = new Label();
            label.CssClass = "normalTextSmallBold";
            label.Text = post.Subject + "<a name=\"" + post.PostID + "\"/>";
            td.Controls.Add(label);

            td.Controls.Add(new LiteralControl("<br>"));

            // Add in 'Posted: '
            label = new Label();
            label.CssClass = "normalTextSmaller";
            label.Text = " Posted: ";
            td.Controls.Add(label);

            // Get the postDateTime
            postDateTime = post.PostDate;

            // Personalize
            if (user != null) {
                dateFormat = user.DateFormat;
                postDateTime = Users.AdjustForTimezone(postDateTime, user);
            } else {
                dateFormat = Globals.DateFormat;
            }

            // Add in PostDateTime
            label = new Label();
            label.CssClass = "normalTextSmaller";
            label.Text = postDateTime.ToString(dateFormat + " " + Globals.TimeFormat);
            td.Controls.Add(label);

            // Add row 1
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Row 2 (body)
            tr = new TableRow();
            td = new TableCell();

            // Add Body
            label = new Label();
            label.CssClass = "normalTextSmall";
            label.Text = Globals.FormatPostBody(post.Body);
            td.Controls.Add(label);

            // Add row 2
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Row 3 (Signature)
            tr = new TableRow();
            td = new TableCell();
            label = new Label();
            label.CssClass = "normalTextSmaller";

            if (postUser.Signature != "") {
                label.Text = Globals.FormatSignature(postUser.Signature);
            }
            td.Controls.Add(label);
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Add whitespace
            tr = new TableRow();
            td = new TableCell();
            td.Height = 2;
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Add buttons for user options
            tr = new TableRow();
            td = new TableCell();
            
            // Add the reply button
            if (!post.IsLocked) {
                // Reply button
                HyperLink replyButton = new HyperLink();
                replyButton.Text = "<img border=0 src=" + Globals.ApplicationVRoot + "/skins/" + Globals.Skin + "/images/newpost.gif" + ">";
                replyButton.NavigateUrl = Globals.UrlReplyToPost + post.PostID + "&mode=flat";
                td.Controls.Add(replyButton);
            }

            if ((user != null) && (user.Username.ToLower() == post.Username.ToLower()) && (user.IsTrusted)) {
                // Edit button
                HyperLink editButton = new HyperLink();
                editButton.Text = "<img border=0 src=" + Globals.ApplicationVRoot + "/skins/" + Globals.Skin + "/images/editpost.gif" + ">";
                editButton.NavigateUrl = Globals.UrlUserEditPost + post.PostID;
                td.Controls.Add(editButton);
            }

            // Anything to add to the table control?
            if (td.Controls.Count > 0) {
                tr.Controls.Add(td);
                table.Controls.Add(tr);
            }

            // Is the current user a moderator?
            if ((user != null) && (user.IsModerator)) {
                tr = new TableRow();
                td = new TableCell();

                // Find the moderation menu
                ModerationMenu moderationMenu = new ModerationMenu();
                moderationMenu.PostID = post.PostID;
                moderationMenu.ThreadID = post.ThreadID;
                moderationMenu.UsernamePostedBy = post.Username;
                moderationMenu.SkinFilename = "Moderation/Skin-ModeratePost.ascx";

                td.Controls.Add(moderationMenu);
                tr.Controls.Add(td);
                table.Controls.Add(tr);

            }

            postInfo.Controls.Add(table);
        }

        // *********************************************************************
        //  HandleDataBindingForAuthorCell
        //
        /// <summary>
        /// Databinds the name of the author.
        /// </summary>
        /// <remarks>
        /// Used only if a user defined template is not provided.
        /// </remarks>
        /// 
        // ********************************************************************/
        private void HandleDataBindingForAuthorCell(Object sender, EventArgs e) {	

            TableCell userInfo = (TableCell) sender;
            DataListItem container = (DataListItem) userInfo.NamingContainer;
            Post post = (Post) container.DataItem;
            User user;
            HyperLink link;
            Label label;
            Image image;
            Uri url;

            // Get the user object - note, we are using
            // the cache under the covers, so this doesn't
            // result in a db lookup for each request
            user = Users.GetUserInfo(post.Username, false);

            // Build user info table
            Table table = new Table();
            TableRow tr;
            TableCell td;

            // Author
            tr = new TableRow();
            td = new TableCell();
            label = new Label();
            label.CssClass = "normalTextSmallBold";
            label.Text = user.Username;
            td.Controls.Add(label);
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // About
            tr = new TableRow();
            td = new TableCell();
            link = new HyperLink();
            link.CssClass = "normalTextSmaller";
            link.NavigateUrl = Globals.UrlUserProfile + user.Username;
            link.Text = "Profile";
            td.Controls.Add(link);

            // whitespace
            td.Controls.Add(new LiteralControl("<br>"));

            // Web Site
            if (user.Url != "") {
                url = new Uri(user.Url);
                link = new HyperLink();
                link.CssClass = "normalTextSmaller";
                link.NavigateUrl = user.Url;
                link.Text = url.Host;
                link.Target = "_new";
                td.Controls.Add(link);
            }
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Do we have any Icon for the user?
            if ((user.HasIcon) && (user.ShowIcon)) {
                tr = new TableRow();
                td = new TableCell();
                image = new Image();
                image.Width = 80;
                image.Height = 80;
                image.ImageUrl = user.ImageUrl;
                td.Controls.Add(image);
                tr.Controls.Add(td);
                table.Controls.Add(tr);
            }

            // Top 25 user?
            if (Users.IsTop25User(user.Username)) {
                tr = new TableRow();
                td = new TableCell();
                image = new Image();
                image.ImageUrl = Globals.ApplicationVRoot + "/skins/" + siteStyle + "/images/users_top25.gif";
                image.AlternateText = "Top 25 Poster";
                td.Controls.Add(image);
                tr.Controls.Add(td);
                table.Controls.Add(tr);

            // Top 50 user?
            } else if (Users.IsTop50User(user.Username)) {
                tr = new TableRow();
                td = new TableCell();
                image = new Image();
                image.ImageUrl = Globals.ApplicationVRoot + "/skins/" + siteStyle + "/images/users_top50.gif";
                image.AlternateText = "Top 50 Poster";
                td.Controls.Add(image);
                tr.Controls.Add(td);
                table.Controls.Add(tr);

            } else if (Users.IsTop100User(user.Username)) {
                tr = new TableRow();
                td = new TableCell();
                image = new Image();
                image.ImageUrl = Globals.ApplicationVRoot + "/skins/" + siteStyle + "/images/users_top100.gif";
                image.AlternateText = "Top 100 Poster";
                td.Controls.Add(image);
                tr.Controls.Add(td);
                table.Controls.Add(tr);
            }

            // Moderator?
            if (user.IsModerator) {
                tr = new TableRow();
                td = new TableCell();
                image = new Image();
                image.ImageUrl = Globals.ApplicationVRoot + "/skins/" + siteStyle + "/images/users_moderator.gif";
                image.AlternateText = "Forum Moderator";
                td.Controls.Add(image);
                tr.Controls.Add(td);
                table.Controls.Add(tr);
            }

            // Add a little whitespace
            tr = new TableRow();
            td = new TableCell();
            td.Text = "&nbsp;";
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Add table to placeholder
            userInfo.Controls.Add(table);
        }

        // *********************************************************************
        //  RenderHeaderTemplate
        //
        /// <summary>
        /// This function renders the header template
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderHeaderTemplate() {
            PostCollection posts;
            Table table = new Table();
            TableRow tr;
            TableHeaderCell th;

            // Get details about this post and ensure
            // we get the root post
            posts = (PostCollection) DataSource;

            // Titles row
            tr = new TableRow();

            // Authors
            th = new TableHeaderCell();
            th.Height = Unit.Pixel(25);
            th.CssClass = "tableHeaderText";
            th.Width = 100;
            
            th.HorizontalAlign = HorizontalAlign.Left;
            th.Text = "&nbsp;Author";
            tr.Controls.Add(th);

            // Messages
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.Width = Unit.Percentage(85);
            th.HorizontalAlign = HorizontalAlign.Left;
            th.Text = "&nbsp;Thread: " + ((Post)posts[0]).Subject;
            
            tr.Controls.Add(th);


            // Add header row to table
            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  RenderItemTemplate
        //
        /// <summary>
        /// This function renders the item template for flat display
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderItemTemplate() {
            Table table = new Table();
            TableRow tr = new TableRow();
            TableCell authorCell = new TableCell();
            TableCell postCell = new TableCell();

            // Author Cell Details
            authorCell.VerticalAlign = VerticalAlign.Top;
            authorCell.CssClass = "forumRow";
            authorCell.Wrap = false;
            authorCell.DataBinding += new System.EventHandler(HandleDataBindingForAuthorCell);

            // Post Cell Details
            postCell.VerticalAlign = VerticalAlign.Top;
            postCell.CssClass = "forumRow";
            postCell.DataBinding += new System.EventHandler(HandleDataBindingForPostCell);

            // Add controls to control tree
            tr.Controls.Add(authorCell);
            tr.Controls.Add(postCell);
            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  RenderAlternatingItemTemplate
        //
        /// <summary>
        /// This function renders the item template for flat display
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderAlternatingItemTemplate() {
            Table table = new Table();
            TableRow tr = new TableRow();
            TableCell authorCell = new TableCell();
            TableCell postCell = new TableCell();

            // Author Cell Details
            authorCell.VerticalAlign = VerticalAlign.Top;
            authorCell.CssClass = "forumAlternate";
            authorCell.Wrap = false;
            authorCell.DataBinding += new System.EventHandler(HandleDataBindingForAuthorCell);

            // Post Cell Details
            postCell.VerticalAlign = VerticalAlign.Top;
            postCell.CssClass = "forumAlternate";
            postCell.DataBinding += new System.EventHandler(HandleDataBindingForPostCell);

            // Add controls to control tree
            tr.Controls.Add(authorCell);
            tr.Controls.Add(postCell);
            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  RenderFooterTemplate
        //
        /// <summary>
        /// This function renders the footer template
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderFooterTemplate() {
            Table table = new Table();
            TableRow tr;
            TableCell td;

            // Titles row
            tr = new TableRow();

            // Messages
            td = new TableCell();
            td.ColumnSpan = 2;
            td.CssClass = "forumHeaderBackgroundAlternate";
            td.Height = 20;
            
            tr.Controls.Add(td);

            // Add header row to table
            table.Controls.Add(tr);

            return table;
        }

        
        // *********************************************************************
        //  BuildHeaderTemplate
        //
        /// <summary>
        /// This function is called to create the template for the header
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildHeaderTemplate(Control _ctrl) {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(RenderHeaderTemplate());
            
        }

 
        // *********************************************************************
        //  BuildItemTemplate
        //
        /// <summary>
        /// This function is called to create the template for items
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildItemTemplate(Control _ctrl) {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));
            __parser.AddParsedSubObject(RenderItemTemplate());
            
        }

        // *********************************************************************
        //  BuildSeparatorItemTemplate
        //
        /// <summary>
        /// This function is called to create the template for items
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildSeparatorTemplate(Control _ctrl) {
            Table table = new Table();
            TableRow tr = new TableRow();
            TableCell td = new TableCell();

            td.ColumnSpan = 2;
            td.Height = 2;
            td.CssClass = "flatViewSpacing";

            tr.Controls.Add(td);
            table.Controls.Add(tr);

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));
            __parser.AddParsedSubObject(table);
        }

        // *********************************************************************
        //  BuildAlternatingItemTemplate
        //
        /// <summary>
        /// This function is called to create the template for items
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildAlternatingItemTemplate(Control _ctrl) {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));
            __parser.AddParsedSubObject(RenderAlternatingItemTemplate());
            
        }
        
        // *********************************************************************
        //  BuildFooterTemplate
        //
        /// <summary>
        /// This function is called to create the template for the header
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildFooterTemplate(Control _ctrl) {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(RenderFooterTemplate());
            
        }

        // *********************************************************************
        //  ApplyTemplates
        //
        /// <summary>
        /// Applies templates to control the ui generated by the control. If no
        /// template is specified a custom template is used. If a template is found
        /// in the skins directory, that template is loaded and used. If a user defined
        /// template is found, that template takes priority.
        /// </summary>
        /// 
        // ********************************************************************/
        private void ApplyTemplates() {
            string pathToHeaderTemplate;
            string pathToItemTemplate;
            string pathToAlternatingItemTemplate;
            string pathToFooterTemplate;
            string keyForHeaderTemplate;
            string keyForItemTemplate;
            string keyForAlternatingItemTemplate;
            string keyForFooterTemplate;

            // Are we using skinned template?
            if (Page != null) {

                // Set the file paths to where the templates should be found
                keyForHeaderTemplate = Globals.Skin + "/Templates/PostList-Header.ascx";
                keyForItemTemplate = Globals.Skin + "/Templates/PostList-Item.ascx";
                keyForAlternatingItemTemplate = Globals.Skin + "/Templates/PostList-AlternatingItem.ascx";
                keyForFooterTemplate = Globals.Skin + "/Templates/PostList-Footer.ascx";

                // Set the file paths to where the templates should be found
                pathToHeaderTemplate = Globals.ApplicationVRoot + "/Skins/" + keyForHeaderTemplate;
                pathToItemTemplate = Globals.ApplicationVRoot + "/skins/" + keyForItemTemplate;
                pathToAlternatingItemTemplate = Globals.ApplicationVRoot + "/skins/" + keyForAlternatingItemTemplate;
                pathToFooterTemplate = Globals.ApplicationVRoot + "/skins/" + keyForFooterTemplate;

                // Attempt to get the skinned header template
                if (HeaderTemplate == null)
                    HeaderTemplate = Globals.LoadSkinnedTemplate(pathToHeaderTemplate, keyForHeaderTemplate, Page);

                // Attempt to get the skinned item template
                if (ItemTemplate == null)
                    ItemTemplate = Globals.LoadSkinnedTemplate(pathToItemTemplate,keyForItemTemplate, Page);

                // Attempt to get the skinned alternating item template
                if (AlternatingItemTemplate == null)
                    AlternatingItemTemplate = Globals.LoadSkinnedTemplate(pathToAlternatingItemTemplate,keyForAlternatingItemTemplate, Page);

                // Attempt to get the footer template
                if (FooterTemplate == null)
                    FooterTemplate = Globals.LoadSkinnedTemplate(pathToFooterTemplate, keyForFooterTemplate, Page);
            }

            // Are any templates specified yet?
            if (ItemTemplate == null) {
                // Looks like we're using custom templates
                ExtractTemplateRows = true;

                HeaderTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildHeaderTemplate));
                ItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildItemTemplate));
                //TODO: BUGBUG with DATALIST? SeparatorTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildSeparatorTemplate));
                AlternatingItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildAlternatingItemTemplate));
                FooterTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildFooterTemplate));
            }

        }

    }
}