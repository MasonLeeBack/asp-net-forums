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
    /// This Web control displays the posts for a particular forum.  The posts are displayed in
    /// a format either specifically indicated by the programmer utilizing this Web control or by
    /// the Web visitor's Forum Display settings.  The posts shown are the posts for the forum that
    /// fall within a certain date range, which can be specified via the Forum Administration Web page.
    /// </summary>
    /// <remarks>When using this control you must set the ForumID property to the forum's posts you
    /// wish to display.  Failure to set this property will result in an Exception.</remarks>
    [
    ParseChildren(true)
    ]
    public class WhereAmI : WebControl, INamingContainer {

        bool showHome = false;
        bool enableLinks = true;
        const string separator = " > ";

        // *********************************************************************
        //  WhereAmI
        //
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        // ********************************************************************/
        public WhereAmI() {

            // If we have an instance of context, let's attempt to
            // get the ForumID so we can save the user from writing
            // the code
            if (null != Context) {

                // Attempt to get the ForumID
                if (null != Context.Request.QueryString["ForumID"])
                    this.ForumID = Convert.ToInt32(Context.Request.QueryString["ForumID"]);
                else if (null != Context.Request.Form["ForumId"])
                    this.ForumID = Convert.ToInt32(Context.Request.Form["ForumId"]);

                // Attempt to get the ForumGroupID
                if (null != Context.Request.QueryString["ForumGroupID"])
                    this.ForumGroupID = Convert.ToInt32(Context.Request.QueryString["ForumGroupID"]);
                else if (null != Context.Request.Form["ForumGroupID"])
                    this.ForumGroupID = Convert.ToInt32(Context.Request.Form["ForumGroupID"]);

                // Attempt to get the PostID
                if (null != Context.Request.QueryString["PostID"]) {
                    string postID = Context.Request.QueryString["PostID"];

                    // Contains a #
                    if (postID.IndexOf("#") > 0)
                        postID = postID.Substring(0, postID.IndexOf("#"));

                    this.PostID = Convert.ToInt32(postID);
                } else if (null != Context.Request.Form["PostID"]) {
                    this.PostID = Convert.ToInt32(Context.Request.Form["PostID"]);
                }

            }

        }

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
            PlaceHolder navigation;
            HyperLink link;
            Forum forum = null;
            ForumGroup forumGroup = null;
            Post post = null;
            bool inPost = false;
            bool inForum = false;
            bool inForumGroup = false;

            // Are we in a post or forum or what?
            if (PostID > -1) {
                try {
                    post = Posts.GetPost(PostID, Context.User.Identity.Name);
                } catch (Components.PostNotFoundException postNotFound) {
                    HttpContext.Current.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.PostDoesNotExist));
                    HttpContext.Current.Response.End();
                }
                inPost = true;
            } else if (ForumID > -1) {
                forum = Forums.GetForumInfo(ForumID);
                inForum = true;
            } else if (ForumGroupID > -1) {
                try {
                    forumGroup = ForumGroups.GetForumGroup(ForumGroupID);
                } catch (Components.ForumGroupNotFoundException forumGroupNotFound) {
                    HttpContext.Current.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UnknownForum));
                    HttpContext.Current.Response.End();
                }
                inForumGroup = true;
            }

            // This is the label we'll use to contain our controls
            navigation = new PlaceHolder();

            if (ShowHome) {
                // Always add the site name as the base
                link = new HyperLink();
                link.CssClass = "linkSmallBold";
                link.Text = Globals.SiteName;
                link.NavigateUrl = Globals.UrlHome;
                navigation.Controls.Add(link);

                // Add separator
                navigation.Controls.Add(Separator());
            }

            // Are we in a forum group?
            if (inForumGroup) {
                RenderForumGroupName(navigation, forumGroup);
            }

            // Are we in a forum?
            if (inForum) {
                RenderForumName(navigation, forum);
            }

            // Are we in a post?
            if (inPost) {
                RenderPostName(navigation, post);
            }

            Controls.Add(navigation);
            
        }

        // *********************************************************************
        //  RenderSeparator
        //
        /// <summary>
        /// Creates a separator between menu item
        /// </summary>
        /// 
        // ********************************************************************/ 
        private Label Separator() {
            Label menuSeparator = new Label();

            // Add separator
            menuSeparator.CssClass = "normalTextSmallBold";
            menuSeparator.Text = separator;

            return menuSeparator;

        }


        // *********************************************************************
        //  RenderPostName
        //
        /// <summary>
        /// Creates a hyperlink for navigation to the current post
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void RenderPostName(Control control, Post post) {
            ForumGroup forumGroup;
            HyperLink link;

            // Get the forum group details
            forumGroup = ForumGroups.GetForumGroupByForumID(post.ForumID);

            // Add forum group name
            link = new HyperLink();
            link.CssClass = "linkSmallBold";
            link.Text = forumGroup.Name;
            if (EnableLinks)
                link.NavigateUrl = Globals.UrlShowForumGroup + forumGroup.ForumGroupID;
            control.Controls.Add(link);

            // Add separator
            control.Controls.Add(Separator());

            // Add forum name
            link = new HyperLink();
            link.CssClass = "linkSmallBold";
            link.Text = post.ForumName;
            if (EnableLinks)
                link.NavigateUrl = Globals.UrlShowForum + post.ForumID;
            control.Controls.Add(link);

            // Add separator
            control.Controls.Add(Separator());

            // Add post name
            link = new HyperLink();
            link.CssClass = "linkSmallBold";
            link.Text = post.Subject;
            if (post.PostID != post.ThreadID) {
                if (EnableLinks) {
                    link.NavigateUrl = Globals.UrlShowPost + post.ThreadID + "#" + post.PostID;
                }
            } else {
                if (EnableLinks) {
                    link.NavigateUrl = Globals.UrlShowPost + post.ThreadID;
                }
            }

            control.Controls.Add(link);
        }

        // *********************************************************************
        //  RenderForumName
        //
        /// <summary>
        /// Creates a hyperlink for navigation to the current forum
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void RenderForumName(Control control, Forum forum) {
            ForumGroup forumGroup;
            HyperLink link;

            // Get the forum group details
            forumGroup = ForumGroups.GetForumGroupByForumID(forum.ForumID);

            // Add forum group name
            link = new HyperLink();
            link.CssClass = "linkSmallBold";
            link.Text = forumGroup.Name;
            if (EnableLinks)
                link.NavigateUrl = Globals.UrlShowForumGroup + forumGroup.ForumGroupID;
            control.Controls.Add(link);

            // Add separator
            control.Controls.Add(Separator());

            link = new HyperLink();
            link.CssClass = "linkSmallBold";
            link.Text = forum.Name;
            if (EnableLinks)
                link.NavigateUrl = Globals.UrlShowForum + forum.ForumID;
            control.Controls.Add(link);
        }

        // *********************************************************************
        //  RenderForumGroupName
        //
        /// <summary>
        /// Creates a hyperlink for navigation to the current forum group
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void RenderForumGroupName(Control control, ForumGroup forumGroup) {
            HyperLink link;

            // Add forum group name
            link = new HyperLink();
            link.CssClass = "linkSmallBold";
            link.Text = forumGroup.Name;
            if (EnableLinks)
                link.NavigateUrl = Globals.UrlShowForumGroup + forumGroup.ForumGroupID;
            control.Controls.Add(link);
        }

        /// <summary>
        /// Specifies the Forum's posts you want to view.
        /// </summary>
        [
        Category("Required"),
        Description("Specifies the forum whose posts should be displayed.")
        ]
        public int ForumID {
            get {
                // the forumID is stuffed in the ViewState so that
                // it is persisted across postbacks.
                if (ViewState["forumID"] == null)
                    return -1;		// if it's not found in the ViewState, return the default value
					
                return Convert.ToInt32(ViewState["forumID"].ToString());
            }
            set {
                // set the viewstate
                ViewState["forumID"] = value;
            }
        }

        /// <summary>
        /// Specifies the Forum's posts you want to view.
        /// </summary>
        [
        Category("Required"),
        Description("Specifies the forum whose posts should be displayed.")
        ]
        public int ForumGroupID {
            get {
                // the forumID is stuffed in the ViewState so that
                // it is persisted across postbacks.
                if (ViewState["forumGroupID"] == null)
                    return -1;		// if it's not found in the ViewState, return the default value
					
                return Convert.ToInt32(ViewState["forumGroupID"].ToString());
            }
            set {
                // set the viewstate
                ViewState["forumGroupID"] = value;
            }
        }

        /// <summary>
        /// Specifies the Forum's posts you want to view.
        /// </summary>
        [
        Category("Required"),
        Description("Specifies the Post we're in.")
        ]
        public int PostID {
            get {
                // the forumID is stuffed in the ViewState so that
                // it is persisted across postbacks.
                if (ViewState["postID"] == null)
                    return -1;		// if it's not found in the ViewState, return the default value
					
                return Convert.ToInt32(ViewState["postID"].ToString());
            }
            set {
                // set the viewstate
                ViewState["postID"] = value;
            }
        }

        /****************************************************************
        // ShowHome
        //
        /// <summary>
        /// Controls whether or not the root element for the home is shown
        /// </summary>
        //
        ****************************************************************/
        public bool ShowHome {
            get {return showHome; }
            set {showHome = value; }
        }

        /****************************************************************
        // EnableLinks
        //
        /// <summary>
        /// Controls whether or not the root element for the home is shown
        /// </summary>
        //
        ****************************************************************/
        public bool EnableLinks {
            get {return enableLinks; }
            set {enableLinks = value; }
        }

    }
}