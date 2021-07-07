using System;
using System.Data;
using System.Data.OleDb;
using AspNetForums.Components;
using System.Web;
using System.Web.Mail;
using System.IO;
using System.Text.RegularExpressions;

namespace AspNetForums.Data {

    /// <summary>
    /// Summary description for WebForumsDataProvider.
    /// </summary>
    public class OleDbDataProvider {
        /*********************************************************************************/
		
        /****************** PRIVATE HELPER FUNCTIONS ***************************
            * These functions are private helper functions to the WebForumsSqlDataProvider
            * class.  These include functions that help populate the various WebForums
            * objects from a OleDbDataReader.
            * *********************************************************************/

        /****************************************************************
        // AddForumGroup
        //
        /// <summary>
        /// Creates a new forum group, and exception is raised if the
        /// forum group already exists.
        /// </summary>
        /// <param name="forumGroupName">Name of the forum group to create</param>
        //
        ****************************************************************/
        public void AddForumGroup(string forumGroupName) {

        }


        /****************************************************************
        // UpdateForumGroup
        //
        /// <summary>
        /// Updates the name of an existing forum group
        /// </summary>
        /// <param name="forumGroupName">New name for the forum group</param>
        /// <param name="forumGroupId">Unique identifier for the forum group to update</param>
        //
        *****************************************************************/
        public void UpdateForumGroup(string forumGroupName, int forumGroupId) {

        }

        /// <summary>
        /// Builds and returns an instance of the Post class based on the current row of an
        /// aptly populated OleDbDataReader object.
        /// </summary>
        /// <param name="dr">The OleDbDataReader object that contains, at minimum, the following
        /// columns: PostID, ParentID, Body, ForumID, PostDate, PostLevel, SortOrder, Subject,
        /// ThreadDate, ThreadID, Replies, Username, and Approved.</param>
        /// <returns>An instance of the Post class that represents the current row of the passed 
        /// in OleDbDataReader, dr.</returns>
        private Post PopulatePostFromOleDbDataReader(OleDbDataReader dr) {
            Post post = new Post();
            post.PostID = Convert.ToInt32(dr["PostID"]);
            post.ParentID = Convert.ToInt32(dr["ParentID"]);
            post.Body = Convert.ToString(dr["Body"]);
            post.ForumID = Convert.ToInt32(dr["ForumID"]);
            post.PostDate = Convert.ToDateTime(dr["PostDate"]);
            post.PostLevel = Convert.ToInt32(dr["PostLevel"]);
            post.SortOrder = Convert.ToInt32(dr["SortOrder"]);
            post.Subject = Convert.ToString(dr["Subject"]);
            post.ThreadDate = Convert.ToDateTime(dr["ThreadDate"]);
            post.ThreadID = Convert.ToInt32(dr["ThreadID"]);
            post.Replies = Convert.ToInt32(dr["Replies"]);
            post.Username = Convert.ToString(dr["Username"]);
            post.Approved = Convert.ToBoolean(dr["Approved"]);
			
            return post;
        }


        /// <summary>
        /// Builds and returns an instance of the Forum class based on the current row of an
        /// aptly populated OleDbDataReader object.
        /// </summary>
        /// <param name="dr">The OleDbDataReader object that contains, at minimum, the following
        /// columns: ForumID, DateCreated, Description, Name, Moderated, and DaysToView.</param>
        /// <returns>An instance of the Forum class that represents the current row of the passed 
        /// in OleDbDataReader, dr.</returns>
        private  Forum PopulateForumFromOleDbDataReader(OleDbDataReader dr) {
            Forum forum = new Forum();
            forum.ForumID = Convert.ToInt32(dr["ForumID"]);
            forum.ForumGroupId = Convert.ToInt32(dr["ForumGroupId"]);
            forum.DateCreated = Convert.ToDateTime(dr["DateCreated"]);
            forum.Description = Convert.ToString(dr["Description"]);
            forum.Name = Convert.ToString(dr["Name"]);
            forum.Moderated = Convert.ToBoolean(dr["Moderated"]);
            forum.DaysToView = Convert.ToInt32(dr["DaysToView"]);
            forum.Active = Convert.ToBoolean(dr["Active"]);			

            return forum;
        }
	
        private ForumGroup PopulateForumGroupFromOleDbDataReader(OleDbDataReader dr) {

            ForumGroup forumGroup = new ForumGroup();
            forumGroup.ForumGroupID = (int) dr["ForumGroupId"];
            forumGroup.Name = (string) dr["Name"];

            return forumGroup;
        }

        /// <summary>
        /// Builds and returns an instance of the User class based on the current row of an
        /// aptly populated OleDbDataReader object.
        /// </summary>
        /// <param name="dr">The OleDbDataReader object that contains, at minimum, the following
        /// columns: Signature, Email, FakeEmail, Url, Password, Username, Administrator, Approved,
        /// Trusted, Timezone, DateCreated, LastLogin, and ForumView.</param>
        /// <returns>An instance of the User class that represents the current row of the passed 
        /// in OleDbDataReader, dr.</returns>
        private  User PopulateUserFromOleDbDataReader(OleDbDataReader dr) {
            User user = new User();
            user.Signature = Convert.ToString(dr["Signature"]);
            user.Email = Convert.ToString(dr["Email"]);
            user.FakeEmail = Convert.ToString(dr["FakeEmail"]);
            user.Url = Convert.ToString(dr["URL"]);
            user.Password = Convert.ToString(dr["Password"]);
            user.Username = Convert.ToString(dr["Username"]);
            user.IsAdministrator = Convert.ToBoolean(dr["Administrator"]);
            user.Approved = Convert.ToBoolean(dr["Approved"]);
            user.Trusted = Convert.ToBoolean(dr["Trusted"]);
            user.Timezone = Convert.ToInt32(dr["Timezone"]);
            user.DateCreated = Convert.ToDateTime(dr["DateCreated"]);
            user.LastLogin = Convert.ToDateTime(dr["LastLogin"]);
            user.TrackPosts = Convert.ToBoolean(dr["TrackYourPosts"]);
			
            switch (Convert.ToInt32(dr["ForumView"])) {
                case 0:
                    user.ForumView = ViewOptions.Flat;
                    break;

                case 1:
                    user.ForumView = ViewOptions.Mixed;
                    break;

                case 2:
                    user.ForumView = ViewOptions.Threaded;
                    break;

                default:
                    user.ForumView = ViewOptions.NotSet;
                    break;
            }
			
            return user;
        }


		
        /// <summary>
        /// Builds and returns an instance of the EmailTemplate class based on the current row of an
        /// aptly populated OleDbDataReader object.
        /// </summary>
        /// <param name="dr">The OleDbDataReader object that contains, at minimum, the following
        /// columns: EmailID, Subject, Message, FromAddress, Importance, and Description.</param>
        /// <returns>An instance of the EmailTemplate class that represents the current row of the passed 
        /// in OleDbDataReader, dr.</returns>
        private  EmailTemplate PopulateEmailTemplateFromOleDbDataReader(OleDbDataReader dr) {
            EmailTemplate email = new EmailTemplate();
			
            email.EmailTemplateID = Convert.ToInt32(dr["EmailID"]);
            email.Subject = Convert.ToString(dr["Subject"]);
            email.Body = Convert.ToString(dr["Message"]);
            email.From = Convert.ToString(dr["FromAddress"]);
            email.Description = Convert.ToString(dr["Description"]);

            switch (Convert.ToInt32(dr["Importance"])) {
                case 0:
                    email.Priority = MailPriority.Low;
                    break;

                case 2:
                    email.Priority = MailPriority.High;
                    break;

                default:		// the default
                    email.Priority = MailPriority.Normal;
                    break;
            }

            return email;
        }
        /*********************************************************************************/


        /*********************************************************************************/

        /************************ POST FUNCTIONS ***********************
                 * These functions return information about a post or posts.  They
                 * are called from the WebForums.Posts class.
                 * *************************************************************/

        
        public PostCollection GetAllTopics(int forumID, int pageSize, int pageIndex, DateTime startDate, DateTime endDate, string username) {

            return null; // TODO
        }

        public         void MarkAllTopicsRead(int forumID, string username) {
        }

        /// <summary>
        /// Returns all of the messages for a particular page of posts for a paticular forum in a
        /// particular ForumView mode.
        /// </summary>
        /// <param name="ForumID">The ID of the Forum whose posts you wish to display.</param>
        /// <param name="ForumView">How to display the Forum posts.  The ViewOptions enumeration
        /// supports one of three values: Flat, Mixed, and Threaded.</param>
        /// <param name="PagesBack">How many pages back of data to display.  A value of 0 displays
        /// the posts from the current time to a time that is the Forum's DaysToView days prior to the
        /// current day.</param>
        /// <returns>A PostCollection object containing all of the posts.</returns>
        public  PostCollection GetAllMessages(int ForumID, ViewOptions ForumView, int PagesBack) {
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();
            OleDbCommand myCommandForDaysToView = new OleDbCommand("SELECT DaysToView FROM [Forums] WHERE [ForumID] = " + ForumID.ToString(), myConnection);
            int DaysToView = Convert.ToInt32(myCommandForDaysToView.ExecuteScalar());


            // Create Instance of Connection and Command Object
            String strSQL = "";
            DateTime startDate, stopDate;
            startDate = stopDate = DateTime.Now.AddMinutes(5.0);
			

            if (ForumView == ViewOptions.NotSet) ForumView = (ViewOptions) Globals.DefaultForumView;

            startDate = startDate.AddDays(-PagesBack * DaysToView);
            stopDate = startDate.AddDays(-DaysToView);

            switch (ForumView) {
                case ViewOptions.Flat:
                    strSQL = "SELECT [Subject], [PostID], [ForumID], [ThreadID], [ParentID], [PostLevel], [SortOrder], [Approved], [PostDate], [ThreadDate], [UserName], (SELECT COUNT(*) FROM [Posts] P2 WHERE P2.[ParentID] = P.[PostID] AND P2.[PostLevel[ <> 1) AS [Replies], [Body] " +
                        "FROM [Posts] AS P WHERE [Approved] = Yes AND [ForumID] = @ForumID and [PostDate] >= #" + stopDate.ToString("d-MMM-yyy HH:m") + "# AND [PostDate] <= #" + startDate.ToString("d-MMM-yyy HH:m") + "# ORDER BY [PostDate] DESC";
                    break;

                case ViewOptions.Mixed:
                    strSQL = "SELECT [Subject], [PostID], [ForumID], [ThreadID], [ParentID], [PostLevel], [SortOrder], [Approved], [PostDate], [ThreadDate], [UserName], (SELECT COUNT(*) FROM Posts P2 WHERE P2.ParentID = P.PostID AND P2.PostLevel <> 1) AS Replies, [Body] " +
                        "FROM [Posts] AS P WHERE [PostLevel] = 1 AND [Approved] = Yes AND [ForumID] = @ForumID and [PostDate] >= #" + stopDate.ToString("d-MMM-yyy HH:m") + "# AND [PostDate] <= #" + startDate.ToString("d-MMM-yyy HH:m") + "# ORDER BY [PostDate] DESC";
                    break;

                case ViewOptions.Threaded:
                    strSQL = "SELECT [Subject], [PostID], [ForumID], [ThreadID], [ParentID], [PostLevel], [SortOrder], [Approved], [PostDate], [ThreadDate], [UserName], 0 AS [Replies], [Body] " +
                        "FROM [Posts] AS P WHERE [Approved] = Yes AND [ForumID] = @ForumID and [ThreadDate] >= #" + stopDate.ToString("d-MMM-yyy HH:m") + "# AND [ThreadDate] <= #" + startDate.ToString("d-MMM-yyy HH:m") + "# ORDER BY [ThreadID] DESC, [SortOrder]";
                    //HttpContext.Current.Response.Write(strSQL + " - Pages Back: " + PagesBack.ToString());
                    break;
            }

			
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterForumId = new OleDbParameter("@ForumId", OleDbType.Integer, 4);
            parameterForumId.Value = ForumID;
            myCommand.Parameters.Add(parameterForumId);

            // Execute the command
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            PostCollection posts = new PostCollection();
            while (dr.Read()) {
                posts.Add(PopulatePostFromOleDbDataReader(dr));
            }
            dr.Close();
            myConnection.Close();

            return posts;
        }

        /// <summary>
        /// Given a particular ThreadID, this method returns the ID of the next thread in the specified forum.
        /// </summary>
        /// <param name="ForumID">The ForumID of the post we are viewing.</param>
        /// <param name="ThreadID">The ThreadID of the post we are viewing.</param>
        /// <param name="myConnection">An open OleDbConnection object.</param>
        /// <returns>The PostID of the first post in the next thread.</returns>
        /// <remarks>If there is no next thread, 0 is returned.</remarks>
        private int GetNextThreadID(int ForumID, int ThreadID, OleDbConnection myConnection) {
            String strSQL = "SELECT TOP 1 [ThreadID] FROM [Posts] WHERE [ThreadID] > @ThreadID AND [ForumID] = @ForumID AND [Approved] = YES ORDER BY [ThreadID] DESC";

            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);
			
            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = ForumID;
            myCommand.Parameters.Add(parameterForumID);

            Object returnResult = myCommand.ExecuteScalar();

            if (Convert.IsDBNull(returnResult) || returnResult == null)
                return 0;
            else
                return (int) returnResult;
        }


        /// <summary>
        /// Given a particular ThreadID, this method returns the ID of the previous thread in the specified forum.
        /// </summary>
        /// <param name="ForumID">The ForumID of the post we are viewing.</param>
        /// <param name="ThreadID">The ThreadID of the post we are viewing.</param>
        /// <param name="myConnection">An open OleDbConnection object.</param>
        /// <returns>The PostID of the first post in the previous thread.</returns>
        /// <remarks>If there is no previous thread, 0 is returned.</remarks>
        private int GetPrevThreadID(int ForumID, int ThreadID, OleDbConnection myConnection) {
            String strSQL = "SELECT TOP 1 [ThreadID] FROM [Posts] WHERE [ThreadID] < @ThreadID AND [ForumID] = @ForumID AND [Approved] = YES ORDER BY [ThreadID] DESC";

            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);
			
            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = ForumID;
            myCommand.Parameters.Add(parameterForumID);
			
            Object returnResult = myCommand.ExecuteScalar();

            if (Convert.IsDBNull(returnResult) || returnResult == null)
                return 0;
            else
                return (int) returnResult;
        }


		
		
        /// <summary>
        /// Given a particular ThreadID, this method returns the ID of the previous post in the thread.
        /// </summary>
        /// <param name="ForumID">The ForumID of the post we are viewing.</param>
        /// <param name="ThreadID">The ThreadID of the post we are viewing.</param>
        /// <param name="myConnection">An open OleDbConnection object.</param>
        /// <returns>The PostID of the previous post in the thread.</returns>
        /// <remarks>If there is no previous post in the thread, 0 is returned.</remarks>
        private int GetPrevPostID(int ForumID, int ThreadID, int SortOrder, OleDbConnection myConnection) {
            String strSQL = "SELECT TOP 1 [PostID] FROM [Posts] WHERE [ThreadID] = @ThreadID AND [ForumID] = @ForumID AND [SortOrder] = @SortOrder-1 AND [Approved] = YES";

            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);
			
            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = ForumID;
            myCommand.Parameters.Add(parameterForumID);

            OleDbParameter parameterSortOrder = new OleDbParameter("@SortOrder", OleDbType.Integer, 4);
            parameterSortOrder.Value = SortOrder;
            myCommand.Parameters.Add(parameterSortOrder);

            Object returnResult = myCommand.ExecuteScalar();

            if (Convert.IsDBNull(returnResult) || returnResult == null)
                return 0;
            else
                return (int) returnResult;
        }


		
        /// <summary>
        /// Given a particular ThreadID, this method returns the ID of the next post in the thread.
        /// </summary>
        /// <param name="ForumID">The ForumID of the post we are viewing.</param>
        /// <param name="ThreadID">The ThreadID of the post we are viewing.</param>
        /// <param name="myConnection">An open OleDbConnection object.</param>
        /// <returns>The PostID of the next post in the thread.</returns>
        /// <remarks>If there is no next post in the thread, 0 is returned.</remarks>
        private int GetNextPostID(int ForumID, int ThreadID, int SortOrder, OleDbConnection myConnection) {
            String strSQL = "SELECT TOP 1 [PostID] FROM [Posts] WHERE [ThreadID] = @ThreadID AND [ForumID] = @ForumID AND [SortOrder] = @SortOrder+1 AND [Approved] = YES";

            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);
			
            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = ForumID;
            myCommand.Parameters.Add(parameterForumID);

            OleDbParameter parameterSortOrder = new OleDbParameter("@SortOrder", OleDbType.Integer, 4);
            parameterSortOrder.Value = SortOrder;
            myCommand.Parameters.Add(parameterSortOrder);

            Object returnResult = myCommand.ExecuteScalar();

            if (Convert.IsDBNull(returnResult) || returnResult == null)
                return 0;
            else
                return (int) returnResult;
        }


		
        /// <summary>
        /// Given a particular ThreadID, this method returns a Boolean indicating whether or not the
        /// user viewing the post is signed up to receive email notification when new messages are
        /// posted to the thread.
        /// </summary>
        /// <param name="ThreadID">The ThreadID of the post we are viewing.</param>
        /// <param name="Username">The Username of the user viewing the post.</param>
        /// <param name="myConnection">An open OleDbConnection object.</param>
        /// <returns>A Boolean indicating whether or not the user is signed up to receive email notifications
        /// when new posts are made to this thread.</returns>
        private bool GetThreadTrackingForUser(int ThreadID, String Username, OleDbConnection myConnection) {
            String strSQL = "SELECT [ThreadID] FROM [ThreadTrackings] WHERE [ThreadID] = @ThreadID AND [UserName] = @UserName";

            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);
			
            OleDbParameter parameterUsername = new OleDbParameter("@ForumID", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);

            Object returnResult = myCommand.ExecuteScalar();

            if (Convert.IsDBNull(returnResult) || returnResult == null)
                return false;
            else
                return true;
        }


        /// <summary>
        /// Returns count of all posts in system
        /// </summary>
        /// <returns></returns>
        public int GetTotalPostCount() {
            String strSQL = "SELECT Count(*) FROM Posts";
            int totalPostCount;

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader();		// Don't use CommandBehavior.CloseConnection b/c we need to reuse this connection

            dr.Read();
            totalPostCount = (int) dr[0];

            dr.Close();
            myConnection.Close();

            return totalPostCount;
        }

        /// <summary>
        /// Gets the details for a particular post.  These details include the IDs of the next/previous
        /// post and the next/prev thread, along with information about the user who posted the post.
        /// </summary>
        /// <param name="PostID">The ID of the Post to get the information from.</param>
        /// <param name="Username">The Username of the person viewing the post.  Used to determine if
        /// the particular user has email tracking turned on for the thread that this message resides.</param>
        /// <returns>A PostDetails instance with rich information about the particular post.</returns>
        /// <remarks>If a PostID is passed in that is NOT found in the database, a PostNotFoundException
        /// exception is thrown.</remarks>
        public  PostDetails GetPostDetails(int PostID, String Username) {

            String strSQL = "SELECT [Subject], [ForumID], [ThreadID], [ParentID], [PostLevel], [SortOrder], [PostDate], [ThreadDate], P.[UserName], U.[FakeEmail], U.[URL], U.[Signature], P.[Approved], " +
                "(SELECT COUNT(*) FROM [Posts] AS P2 WHERE P2.[ParentID] = P.[PostID] AND P2.[PostLevel] <> 1) AS [Replies],	[Body] FROM [Posts] AS P INNER JOIN [Users] AS U ON	U.[UserName] = P.[UserName] WHERE [PostID] = @PostID";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterPostId = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostId.Value = PostID;
            myCommand.Parameters.Add(parameterPostId);
			
            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader();		// Don't use CommandBehavior.CloseConnection b/c we need to reuse this connection

            if (!dr.Read()) {
                // we did not get back a post
                dr.Close();
                myConnection.Close();

                throw new Components.PostNotFoundException("Did not get back a post for PostID " + PostID.ToString());
            }

            // we have a post to work with
            PostDetails post = new PostDetails();
            post.PostID = PostID;
            post.ParentID = Convert.ToInt32(dr["ParentID"]);
            post.Body = Convert.ToString(dr["Body"]);
            post.ForumID = Convert.ToInt32(dr["ForumID"]);
            post.PostDate = Convert.ToDateTime(dr["PostDate"]);
            post.PostLevel = Convert.ToInt32(dr["PostLevel"]);
            post.SortOrder = Convert.ToInt32(dr["SortOrder"]);
            post.Subject = Convert.ToString(dr["Subject"]);
            post.ThreadDate = Convert.ToDateTime(dr["ThreadDate"]);
            post.ThreadID = Convert.ToInt32(dr["ThreadID"]);
            post.Replies = Convert.ToInt32(dr["Replies"]);
            post.Username = Convert.ToString(dr["Username"]);
			
            // populate information about the User
            User user = new User();
            user.Username = post.Username;
            user.FakeEmail = Convert.ToString(dr["FakeEmail"]);
            user.Url = Convert.ToString(dr["URL"]);
            user.Signature = Convert.ToString(dr["Signature"]);
			
            post.UserInfo = user;

            dr.Close();

            // Now that we've closed the datareader, reuse the connection object to find the next/prev threads/posts
            post.NextPostID = GetNextPostID(post.ForumID, post.ThreadID, post.SortOrder, myConnection);
            post.PrevPostID = GetPrevPostID(post.ForumID, post.ThreadID, post.SortOrder, myConnection);
            post.NextThreadID = GetNextThreadID(post.ForumID, post.ThreadID, myConnection);
            post.PrevThreadID = GetPrevThreadID(post.ForumID, post.ThreadID, myConnection);
            post.ThreadTracking = this.GetThreadTrackingForUser(post.ThreadID, post.Username, myConnection);

            myConnection.Close();

            return post;
        }



        /// <summary>
        /// Get basic information about a single post.  This method returns an instance of the Post class,
        /// which contains less information than the PostDeails class, which is what is returned by the
        /// GetPostDetails method.
        /// </summary>
        /// <param name="PostID">The ID of the post whose information we are interested in.</param>
        /// <returns>An instance of the Post class.</returns>
        /// <remarks>If a PostID is passed in that is NOT found in the database, a PostNotFoundException
        /// exception is thrown.</remarks>
        public  Post GetPost(int PostID, string username, bool trackViews) {
            String strSQL = "SELECT [Subject],	[PostID], [UserName], P.[ForumID], (SELECT [Name] FROM [Forums] AS F WHERE F.[ForumID] = P.[ForumID]), " +
                "[ParentID], [ThreadID], [Approved], [PostDate], [PostLevel], [SortOrder], [ThreadDate], (SELECT COUNT(*) FROM [Posts] AS P2 WHERE P2.[ParentID] = P.[PostID] AND P2.[PostLevel] <> 1) AS [Replies], [Body] " +
                "FROM [Posts] AS P WHERE P.[PostID] = @PostID";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterPostId = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostId.Value = PostID;
            myCommand.Parameters.Add(parameterPostId);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            if (!dr.Read()) {
                // we did not get back a post
                dr.Close();
                myConnection.Close();

                throw new Components.PostNotFoundException("Did not get back a post for PostID " + PostID.ToString());
            }

            // we have a post to work with	
            Post post = PopulatePostFromOleDbDataReader(dr);

            dr.Close();
            myConnection.Close();

            return post;
        }


        /// <summary>
        /// Get basic information about a single post.  This method returns an instance of the Post class,
        /// which contains less information than the PostDeails class, which is what is returned by the
        /// GetPostDetails method.
        /// </summary>
        /// <param name="PostID">The ID of the post whose information we are interested in.</param>
        /// <param name="myConnection">An open OleDbConnection object.</param>
        /// <returns>An instance of the Post class.</returns>
        /// <remarks>If a PostID is passed in that is NOT found in the database, a PostNotFoundException
        /// exception is thrown.  This form of GetPost should be used when we already have an openned
        /// OleDbConnection object.  If this is not the case, use the other form of the GetPost method.</remarks>
        public  Post GetPost(int PostID, OleDbConnection myConnection) {
            String strSQL = "SELECT [Subject],	[PostID], [UserName], P.[ForumID], (SELECT [Name] FROM [Forums] AS F WHERE F.[ForumID] = P.[ForumID]), " +
                "[ParentID], [ThreadID], [Approved], [PostDate], [PostLevel], [SortOrder], [ThreadDate], (SELECT COUNT(*) FROM [Posts] AS P2 WHERE P2.[ParentID] = P.[PostID] AND P2.[PostLevel] <> 1) AS [Replies], [Body] " +
                "FROM [Posts] AS P WHERE P.[PostID] = @PostID";

            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterPostId = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostId.Value = PostID;
            myCommand.Parameters.Add(parameterPostId);

            // Execute the command
            OleDbDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read()) {
                // we did not get back a post
                dr.Close();

                throw new Components.PostNotFoundException("Did not get back a post for PostID " + PostID.ToString());
            }

            // we have a post to work with	
            Post post = PopulatePostFromOleDbDataReader(dr);

            dr.Close();
            return post;
        }


	
        /// <summary>
        /// Reverses a particular user's email thread tracking options for the thread that contains
        /// the post specified by PostID.  That is, if a User has email thread tracking turned on for
        /// a particular thread, a call to this method will turn off the email thread tracking; conversely,
        /// if a user has thread tracking turned off for a particular thread, a call to this method will
        /// turn it on.
        /// </summary>
        /// <param name="Username">The User whose email thread tracking options we wish to reverse.</param>
        /// <param name="PostID"></param>
        public  void ReverseThreadTracking(String Username, int PostID) {
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();

            Post post = this.GetPost(PostID, myConnection);			// get the threadID for the specific post
            int threadID = post.ThreadID;

            String strSQL = "SELECT COUNT(*) FROM [ThreadTrackings] WHERE [ThreadID] = @ThreadID AND [Username] = @Username";


            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = threadID;
            myCommand.Parameters.Add(parameterThreadId);

            OleDbParameter parameterUsername = new OleDbParameter("@Username", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);


            // Execute the command
            if ((int) myCommand.ExecuteScalar() == 0) {
                // we need to add the thread tracking to the table
                strSQL = "INSERT INTO [ThreadTrackings] ([ThreadID], [Username]) VALUES (@ThreadID, @Username)";

                myCommand.CommandText = strSQL;
                myCommand.Parameters.Clear();

                myCommand.Parameters.Add(parameterThreadId);
                myCommand.Parameters.Add(parameterUsername);

                myCommand.ExecuteNonQuery();
            }
            else {
                // we need to remove the thread tracking to the table
                strSQL = "DELETE FROM [ThreadTrackings] WHERE [ThreadID] = @ThreadID AND [Username] = @Username";

                myCommand.CommandText = strSQL;
                myCommand.Parameters.Clear();

                myCommand.Parameters.Add(parameterThreadId);
                myCommand.Parameters.Add(parameterUsername);

                myCommand.ExecuteNonQuery();
            }

            myConnection.Close();
        }



        /// <summary>
        /// Returns a collection of Posts that make up a particular thread.
        /// </summary>
        /// <param name="ThreadID">The ID of the Thread to retrieve the posts of.</param>
        /// <returns>A PostCollection object that contains the posts in the thread specified by
        /// ThreadID.</returns>
        public  PostCollection GetThread(int ThreadID) {
            String strSQL = "SELECT [PostID], [ForumID], [Subject], [ParentID], [ThreadID], [PostLevel], [SortOrder], [PostDate], [ThreadDate], [UserName], (SELECT COUNT(*) FROM [Posts] AS P2 WHERE P2.[ParentID] = P.[PostID] AND P2.[PostLevel] <> 1) AS [Replies], " +
                "[Body] FROM [Posts] AS P WHERE [Approved] = YES AND [ThreadID] = @ThreadID ORDER BY [SortOrder]";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // loop through the results
            PostCollection posts = new PostCollection();
            while (dr.Read()) {
                posts.Add(PopulatePostFromOleDbDataReader(dr));
            }
            dr.Close();
            myConnection.Close();

            return posts;
        }


        public PostCollection GetThreadByPostID(int postID, int currentPageIndex, int pageSize) {
            // TODO
            return null;
        }
		
        /// <summary>
        /// Returns a collection of Posts that make up a particular thread.
        /// </summary>
        /// <param name="PostID">The ID of a Post in the thread that you are interested in retrieving.</param>
        /// <returns>A PostCollection object that contains the posts in the thread.</returns>
        public  PostCollection GetThreadByPostID(int PostID) {
            String strSQL = "SELECT [PostID], [ThreadID], [ForumID], [Subject], [ParentID],	[PostLevel], [SortOrder], [PostDate], [ThreadDate], [UserName], [Approved], (SELECT COUNT(*) FROM [Posts] P2 WHERE P2.[ParentID] = P.[PostID] AND P2.[PostLevel] <> 1) AS Replies, [Body] " +
                "FROM [Posts] P WHERE [Approved]=YES AND [ThreadID] = (SELECT [ThreadID] FROM [Posts] WHERE [PostID] = @PostID) ORDER BY [SortOrder]";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterPostId = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostId.Value = PostID;
            myCommand.Parameters.Add(parameterPostId);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // loop through the results
            PostCollection posts = new PostCollection();
            while (dr.Read()) {
                posts.Add(PopulatePostFromOleDbDataReader(dr));
            }

            dr.Close();
            myConnection.Close();

            return posts;
        }




        /// <summary>
        /// Adds a new Post.  This method checks the allowDuplicatePosts settings to determine whether
        /// or not to allow for duplicate posts.  If allowDuplicatePosts is set to false and the user
        /// attempts to enter a duplicate post, a PostDuplicateException exception is thrown.
        /// </summary>
        /// <param name="PostToAdd">A Post object containing the information needed to add a new
        /// post.  The essential fields of the Post class that must be set are: the Subject, the
        /// Body, the Username, and a ForumID or a ParentID (depending on whether the post to add is
        /// a new post or a reply to an existing post, respectively).</param>
        /// <returns>A Post object with information on the newly inserted post.  This returned Post
        /// object includes the ID of the newly added Post (PostID) as well as if the Post is
        /// Approved or not.</returns>
        public Post AddPost(Post PostToAdd) {
            // Get information about the user and forum
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();

            // check for any duplicate messages
            String strSQL = "";

            OleDbParameter parameterUserName = new OleDbParameter("@Username", OleDbType.VarChar, 50);
            parameterUserName.Value = PostToAdd.Username;

            OleDbParameter parameterBody = new OleDbParameter("@Body", OleDbType.LongVarChar);
            parameterBody.Value = PostToAdd.Body;

            // see if we have dups, if needed
            if (!Globals.AllowDuplicatePosts) {
                strSQL = "SELECT COUNT(*) FROM [Posts] WHERE [Username] = @Username AND [Body] = @Body";
                OleDbCommand checkForDupsCommand = new OleDbCommand(strSQL, myConnection);

                checkForDupsCommand.CommandType = CommandType.Text;

                checkForDupsCommand.Parameters.Add(parameterUserName);
                checkForDupsCommand.Parameters.Add(parameterBody);


                if (((int) checkForDupsCommand.ExecuteScalar()) > 0) {
                    // we have a dup
                    myConnection.Close();

                    throw new PostDuplicateException("Attempting to insert a duplicate post.");
                }

                checkForDupsCommand.Parameters.Clear();			// clear the parameters
            }


            User user = this.GetUserInfo(PostToAdd.Username, myConnection);
            Forum forum;
			
			
            // how are we going to get the forum information?
            if (PostToAdd.ForumID == 0 && PostToAdd.ParentID != 0)
                // we need to get the forum ID from replyToPostID
                forum = this.GetForumInfoByPostID(PostToAdd.ParentID, myConnection);
            else
                // get the forum information by forumID
                forum = this.GetForumInfo(PostToAdd.ForumID, myConnection);


            bool ModeratedForum = forum.Moderated;
            int newID, threadID;

            bool ApprovedPost = true;
            if (ModeratedForum)
                // this is a moderated forum, so the post is not approved if the user is not trusted
                if (!user.Trusted)
                    ApprovedPost = false;

            OleDbParameter parameterForumId = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumId.Value = PostToAdd.ForumID;

            OleDbParameter parameterApproved = new OleDbParameter("@ApprovedPost", OleDbType.Boolean, 1);
            parameterApproved.Value = ApprovedPost;

            OleDbParameter parameterSubject = new OleDbParameter("@Subject", OleDbType.VarChar, 50);
            parameterSubject.Value = PostToAdd.Subject;
			
            if (PostToAdd.ParentID == 0) {
                // we've got a new post here
                strSQL = "INSERT INTO [Posts] ([ForumID], [ThreadID], [ParentID], [PostLevel], [SortOrder], [PostDate], [Subject], [Username],[Approved], [Body]) " +
                    "VALUES (@ForumID, 0, 0, 1, 1, Now(), @Subject, @Username, @ApprovedPost,  @Body)";
				
                OleDbCommand myCommandNewPost = new OleDbCommand(strSQL, myConnection);
                myCommandNewPost.CommandType = CommandType.Text;

                myCommandNewPost.Parameters.Add(parameterForumId);				
                myCommandNewPost.Parameters.Add(parameterSubject);
                myCommandNewPost.Parameters.Add(parameterUserName);
                myCommandNewPost.Parameters.Add(parameterApproved);
                myCommandNewPost.Parameters.Add(parameterBody);

                // insert the record
                myCommandNewPost.ExecuteNonQuery();

                // get the ID of the just inserted record
                strSQL = "SELECT @@IDENTITY AS NewPost FROM [Posts]";

                myCommandNewPost.Parameters.Clear();
                myCommandNewPost.CommandText = strSQL;
                threadID = newID = (int) myCommandNewPost.ExecuteScalar();

                strSQL = "UPDATE [Posts] SET [ThreadID] = @NewPostID, [ParentID] = @NewPostID WHERE [PostID] = @NewPostID";
                myCommandNewPost.CommandText = strSQL;
                myCommandNewPost.Parameters.Clear();

                OleDbParameter parameterNewID = new OleDbParameter("@NewPostID", OleDbType.Integer, 4);
                parameterNewID.Value = newID;
                myCommandNewPost.Parameters.Add(parameterNewID);

                myCommandNewPost.ExecuteNonQuery();				
            } 
            else {
                // we are adding a reply to an existing post

                // get information about the parent post
                Post parentPost = this.GetPost(PostToAdd.ParentID, myConnection);
                parameterForumId.Value = parentPost.ForumID;		// update the forumID parameter

                // find the next post at the same level or higher (if it exists)
                strSQL = "SELECT Min([SortOrder]) AS NextSortOrder FROM [Posts] WHERE [PostLevel] <= @ParentLevel AND [SortOrder] > @ParentSortOrder AND [ThreadID] = @ThreadID";

                OleDbCommand myCommandPostReply = new OleDbCommand(strSQL, myConnection);
                myCommandPostReply.CommandType = CommandType.Text;

                OleDbParameter parameterParentLevel = new OleDbParameter("@ParentLevel", OleDbType.Integer, 4);
                parameterParentLevel.Value = parentPost.PostLevel;
                myCommandPostReply.Parameters.Add(parameterParentLevel);

                OleDbParameter parameterParentSortOrder = new OleDbParameter("@ParentSortOrder", OleDbType.Integer, 4);
                parameterParentSortOrder.Value = parentPost.SortOrder;
                myCommandPostReply.Parameters.Add(parameterParentSortOrder);

                OleDbParameter parameterParentThreadID = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
                parameterParentThreadID.Value = parentPost.ThreadID;
                myCommandPostReply.Parameters.Add(parameterParentThreadID);

                Object objMinSortOrder = myCommandPostReply.ExecuteScalar();
                int MinSortOrder;
                if (Convert.IsDBNull(objMinSortOrder) || objMinSortOrder == null) {
                    strSQL = "SELECT Max(SortOrder) AS MaxSortOrder FROM [Posts] WHERE [ThreadID] = @ThreadID";

                    myCommandPostReply.CommandText = strSQL;
                    myCommandPostReply.Parameters.Clear();
                    myCommandPostReply.Parameters.Add(parameterParentThreadID);

                    MinSortOrder = (int) myCommandPostReply.ExecuteScalar() + 1;
                } 
                else {					
                    MinSortOrder = (int) objMinSortOrder;

                    // move the existing posts down
                    strSQL = "UPDATE [Posts] SET [SortOrder] = [SortOrder] + 1 WHERE [ThreadID] = @ThreadID AND [SortOrder] >= @NextSortOrder";
                    myCommandPostReply.CommandText = strSQL;
                    myCommandPostReply.Parameters.Clear();

                    myCommandPostReply.Parameters.Add(parameterParentThreadID);
					
                    OleDbParameter parameterNextSortOrder = new OleDbParameter("@NextSortOrder", OleDbType.Integer, 4);
                    parameterNextSortOrder.Value = MinSortOrder;
                    myCommandPostReply.Parameters.Add(parameterNextSortOrder);

                    myCommandPostReply.ExecuteNonQuery();					
                }

                // Now insert the post
                strSQL = "INSERT INTO [Posts] ([ForumID], [ThreadID], [ParentID], [PostLevel], [SortOrder], [Subject], [PostDate], [Username], [Approved], [Body]) " +
                    "VALUES (@ForumID, @ThreadID, @ReplyToPostID, @ParentLevel, @NextSortOrder, @Subject, Now(), @Username, @ApprovedPost, @Body)";

                myCommandPostReply.CommandText = strSQL;
                myCommandPostReply.CommandType = CommandType.Text;

                myCommandPostReply.Parameters.Clear();
                myCommandPostReply.Parameters.Add(parameterForumId);
                myCommandPostReply.Parameters.Add(parameterParentThreadID);

                OleDbParameter parameterReplyToPostID = new OleDbParameter("@ReplyToPostID", OleDbType.Integer, 4);
                parameterReplyToPostID.Value = PostToAdd.ParentID;
                myCommandPostReply.Parameters.Add(parameterReplyToPostID);

                parameterParentLevel.Value = (int) parameterParentLevel.Value + 1;
                myCommandPostReply.Parameters.Add(parameterParentLevel);

                OleDbParameter parameterProperSortOrder = new OleDbParameter("@NextSortOrder", OleDbType.Integer, 4);
                parameterProperSortOrder.Value = MinSortOrder;
                myCommandPostReply.Parameters.Add(parameterProperSortOrder);

                myCommandPostReply.Parameters.Add(parameterSubject);				
                myCommandPostReply.Parameters.Add(parameterUserName);
                myCommandPostReply.Parameters.Add(parameterApproved);
                myCommandPostReply.Parameters.Add(parameterBody);
				
                myCommandPostReply.ExecuteNonQuery();			// insert the record


                // get the ID of the just inserted record
                strSQL = "SELECT @@IDENTITY AS NewPost FROM [Posts]";

                myCommandPostReply.CommandText = strSQL;
                myCommandPostReply.Parameters.Clear();
                newID = (int) myCommandPostReply.ExecuteScalar();

                // if this message is approved, update the threaddate
                if (ApprovedPost) {
                    strSQL = "UPDATE [Posts] SET [ThreadDate] = Now() WHERE [ThreadID] = @ThreadID";

                    myCommandPostReply.CommandText = strSQL;
                    myCommandPostReply.Parameters.Clear();
                    myCommandPostReply.Parameters.Add(parameterParentThreadID);
                    myCommandPostReply.ExecuteNonQuery();
                }

                threadID = parentPost.ThreadID;
            }

            // if the user wants to track this thread, add a row to threadtrackings, if necessary
            if (user.TrackPosts) {
                strSQL = "SELECT COUNT(*) FROM [ThreadTrackings] WHERE [ThreadID] = @ThreadID AND [Username] = @Username";
                OleDbCommand myCommandTrackPosts = new OleDbCommand(strSQL, myConnection);
                myCommandTrackPosts.CommandType = CommandType.Text;

                OleDbParameter parameterGlobalThreadID = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
                parameterGlobalThreadID.Value = threadID;
                myCommandTrackPosts.Parameters.Add(parameterGlobalThreadID);

                myCommandTrackPosts.Parameters.Add(parameterUserName);

                int exists = (int) myCommandTrackPosts.ExecuteScalar();

                if (exists == 0) {
                    // need to add a new row
                    strSQL = "INSERT INTO [ThreadTrackings] ([ThreadID], [Username]) VALUES (@ThreadID, @Username)";

                    myCommandTrackPosts.CommandText = strSQL;
                    myCommandTrackPosts.Parameters.Clear();
                    myCommandTrackPosts.Parameters.Add(parameterGlobalThreadID);
                    myCommandTrackPosts.Parameters.Add(parameterUserName);

                    myCommandTrackPosts.ExecuteNonQuery();
                }
            }

            // Return a Post instance with info from the newly inserted post.
            Post post = GetPost(newID, myConnection);

            myConnection.Close();

            return post;
        }

		

        /// <summary>
        /// Updates a post.
        /// </summary>
        /// <param name="UpdatedPost">The Post data used to update the Post.  The ID of the UpdatedPost
        /// Post object corresponds to what post is to be updated.  The only other fields used to update
        /// the Post are the Subject and Body.</param>
        public void UpdatePost(Post UpdatedPost) {
            String strSQL = "UPDATE [Posts] SET [Subject] = @Subject, [Body] = @Body WHERE [PostID] = @PostID";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterSubject = new OleDbParameter("@Subject", OleDbType.VarChar, 50);
            parameterSubject.Value = UpdatedPost.Subject;
            myCommand.Parameters.Add(parameterSubject);

            OleDbParameter parameterBody = new OleDbParameter("@Body", OleDbType.LongVarWChar);
            parameterBody.Value = UpdatedPost.Body;
            myCommand.Parameters.Add(parameterBody);

            OleDbParameter parameterPostId = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostId.Value = UpdatedPost.PostID;
            myCommand.Parameters.Add(parameterPostId);

            // Execute the command
            myConnection.Open();
            try {
                myCommand.ExecuteNonQuery();
            } 
            catch (Exception e) {
                // oops, something went wrong
                throw new Exception(e.Message);
            }
            myConnection.Close();
        }


		
        /// <summary>
        /// This method deletes a particular post and all of its replies.
        /// </summary>
        /// <param name="postID">The PostID that you wish to delete.</param>
        public  void DeletePost(int PostID) {
            // Delete the post
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            Post post = GetPost(PostID, myConnection);

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = post.ThreadID;

            String strSQL = String.Empty;

            // determine if the post we wish to delete is the parent post
            if (post.ThreadID == PostID) {
                // delete all of the posts who have the same threadID
                // also delete all of the threadtrackings for this thread
                strSQL = "DELETE FROM [ThreadTrackings] WHERE [ThreadID] = @ThreadID";
                myCommand.Parameters.Add(parameterThreadId);

                myCommand.CommandText = strSQL;

                myCommand.ExecuteNonQuery();
            }


            // get the next sortorder
            strSQL = "SELECT IsNull(MIN([SortOrder], 10000) AS [NextSortOrder] FROM [Posts] WHERE " +
                "[ThreadID] = @ThreadID AND [SortOrder] > @SortOrder AND [PostLevel] = @PostLevel";
            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();
            myCommand.Parameters.Add(parameterThreadId);

            OleDbParameter parameterSortOrder = new OleDbParameter("@SortOrder", OleDbType.Integer, 4);
            parameterSortOrder.Value = post.SortOrder;
            myCommand.Parameters.Add(parameterSortOrder);

            OleDbParameter parameterPostLevel = new OleDbParameter("@PostLevel", OleDbType.Integer, 4);
            parameterPostLevel.Value = post.PostLevel;
            myCommand.Parameters.Add(parameterPostLevel);

            int nextSortOrder = Convert.ToInt32(myCommand.ExecuteScalar());


            // now delete all posts between post.SortOrder and nextSortOrder
            strSQL = "DELETE FROM [Posts] WHERE [ThreadID] = @ThreadID AND [SortOrder] >= @SortOrder " +
                "AND [SortOrder] < @NextSortOrder";
            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();
            myCommand.Parameters.Add(parameterThreadId);
            myCommand.Parameters.Add(parameterSortOrder);
            
            OleDbParameter parameterNextSortOrder = new OleDbParameter("@NextSortOrder", OleDbType.Integer, 4);
            parameterNextSortOrder.Value = nextSortOrder;
            myCommand.Parameters.Add(parameterNextSortOrder);


            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        /*********************************************************************************/




        /*********************************************************************************/

        /************************ FORUM FUNCTIONS ***********************
                 * These functions return information about a forum.
                 * are called from the WebForums.Forums class.
                 * **************************************************************/
	
        /// <summary>
        /// Returns information about a particular forum that contains a particular thread.
        /// </summary>
        /// <param name="ThreadID">The ID of the thread that is contained in the Forum you wish to
        /// retrieve information about.</param>
        /// <returns>A Forum object instance containing the information about the Forum that the
        /// specified thread exists in.</returns>
        /// <remarks>If a ThreadID is passed in that is NOT found in the database, a ForumNotFoundException
        /// exception is thrown.</remarks>
        public  Forum GetForumInfoByThreadID(int ThreadID) {
            String strSQL = "SELECT [ForumGroupId], [ForumID], [Name], [Description], [DateCreated], [Moderated], [DaysToView], [Active] " +
                "FROM [Forums] WHERE [ForumID] = (SELECT DISTINCT [ForumID] FROM [Posts] WHERE [ThreadID] = @ThreadID)";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            if (!dr.Read()) {
                // we didn't get a forum, handle it
                dr.Close();
                myConnection.Close();

                throw new Components.ForumNotFoundException("Did not get back a forum for ThreadID " + ThreadID.ToString());
            }
            else {
                Forum forum = PopulateForumFromOleDbDataReader(dr);

                dr.Close();
                myConnection.Close();

                return forum;
            }
        }



        /// <summary>
        /// Returns a Forum object with information on a particular forum.
        /// </summary>
        /// <param name="ForumID">The ID of the Forum you are interested in.</param>
        /// <returns>A Forum object.</returns>
        /// <remarks>If a ForumID is passed in that is NOT found in the database, a ForumNotFoundException
        /// exception is thrown.</remarks>
        public  Forum GetForumInfo(int ForumID) {
            String strSQL = "SELECT	[ForumID], [ForumGroupId], [Name], [Description], [Moderated], [DaysToView], [DateCreated],	[Active] FROM [Forums] WHERE [ForumID] = @ForumID";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterForumId = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumId.Value = ForumID;
            myCommand.Parameters.Add(parameterForumId);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            if (!dr.Read()) {
                // we didn't get a forum, handle it
                dr.Close();
                myConnection.Close();

                throw new Components.ForumNotFoundException("Did not get back a forum for ForumID " + ForumID.ToString());
            }
            else {
                Forum forum = PopulateForumFromOleDbDataReader(dr);

                dr.Close();
                myConnection.Close();

                return forum;
            }
        }



        public  Forum GetForumInfo(int ForumID, OleDbConnection myConnection) {
            String strSQL = "SELECT	[ForumID], [Name], [Description], [Moderated], [DaysToView], [DateCreated],	[Active] FROM [Forums] WHERE [ForumID] = @ForumID";

            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterForumId = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumId.Value = ForumID;
            myCommand.Parameters.Add(parameterForumId);

            // Execute the command
            OleDbDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read()) {
                // we didn't get a forum, handle it
                dr.Close();

                throw new Components.ForumNotFoundException("Did not get back a forum for ForumID " + ForumID.ToString());
            }
            else {
                Forum forum = PopulateForumFromOleDbDataReader(dr);

                dr.Close();
                return forum;
            }
        }




        /// <summary>
        /// Returns information about a particular forum that contains a particular post.
        /// </summary>
        /// <param name="PostID">The ID of the post that is contained in the Forum you wish to
        /// retrieve information about.</param>
        /// <returns>A Forum object instance containing the information about the Forum that the
        /// specified thread exists in.</returns>
        /// <remarks>If a Post is passed in that is NOT found in the database, a ForumNotFoundException
        /// exception is thrown.</remarks>
        public  Forum GetForumInfoByPostID(int PostID) {
            String strSQL = "SELECT [ForumID], [ForumGroupId] [Name], [Description], [DateCreated], [Moderated], [DaysToView], [Active] FROM [Forums] WHERE [ForumID] = (SELECT [ForumID] FROM [Posts] WHERE [PostID] = @PostID)";
            
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterPostId = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostId.Value = PostID;
            myCommand.Parameters.Add(parameterPostId);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            if (!dr.Read()) {
                // we didn't get a forum, handle it
                dr.Close();
                myConnection.Close();

                throw new Components.ForumNotFoundException("Did not get back a forum for PostID " + PostID.ToString());
            }
            else {
                Forum forum = PopulateForumFromOleDbDataReader(dr);

                dr.Close();
                myConnection.Close();

                return forum;
            }
        }


        public  Forum GetForumInfoByPostID(int PostID, OleDbConnection myConnection) {
            String strSQL = "SELECT [ForumID], [Name], [Description], [DateCreated], [Moderated], [DaysToView], [Active] FROM [Forums] WHERE [ForumID] = (SELECT [ForumID] FROM [Posts] WHERE [PostID] = @PostID)";
            
            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterPostId = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostId.Value = PostID;
            myCommand.Parameters.Add(parameterPostId);

            // Execute the command
            OleDbDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read()) {
                // we didn't get a forum, handle it
                dr.Close();
                throw new Components.ForumNotFoundException("Did not get back a forum for PostID " + PostID.ToString());
            }
            else {
                Forum forum = PopulateForumFromOleDbDataReader(dr);

                dr.Close();
                return forum;
            }
        }

        public ForumCollection GetForumsByForumGroupId(int forumGroupId) {
            return null;
        }

        public ForumGroupCollection GetAllForumGroups(bool displayAllForumGroups) {

            string sql = "SELECT ForumGroupId, Name FROM ForumGroups";

            // return all of the forums and their total and daily posts
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(sql, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // populate a ForumCollection object
            ForumGroupCollection forumGroups = new ForumGroupCollection();
            ForumGroup forumGroup;

            while (dr.Read()) {
                forumGroup = PopulateForumGroupFromOleDbDataReader(dr);

                forumGroups.Add(forumGroup);
            }
            dr.Close();

            return forumGroups;
        }


        /// <summary>
        /// Returns a list of all Forums.
        /// </summary>
        /// <returns>A ForumCollection object.</returns>
        public  ForumCollection GetAllForums(bool ShowAllForums) {
            String strSQL = "SELECT	[ForumID], [Name], [Description], [DateCreated], [DaysToView], [Moderated], (SELECT COUNT(*) FROM [Posts] AS P WHERE P.[ForumID] = F.[ForumID] AND P.[Approved]= YES) AS [TotalPosts], " +
                "(SELECT COUNT(*) FROM [Posts] AS P2 WHERE P2.[ForumID] = F.[ForumID] AND P2.[Approved]= YES AND P2.[PostLevel] = 1) AS [TotalTopics], " +
                "(SELECT TOP 1 [PostDate] FROM [Posts] P3 WHERE P3.[ForumID] = F.[ForumID] AND P3.[Approved]=YES ORDER BY [PostDate] DESC) AS [MostRecentPostDate], " +
                "(SELECT TOP 1 [Username] FROM [Posts] P3 WHERE P3.[ForumID] = F.[ForumID] AND P3.[Approved]=YES ORDER BY [PostDate] DESC) AS [MostRecentPostAuthor], " +
                "[Active] FROM [Forums] AS F ";
            if (!ShowAllForums) strSQL += "WHERE [Active] = True";

            // return all of the forums and their total and daily posts
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // populate a ForumCollection object
            ForumCollection forums = new ForumCollection();
            Forum forum;
            while (dr.Read()) {
                forum = PopulateForumFromOleDbDataReader(dr);
                forum.TotalPosts = Convert.ToInt32(dr["TotalPosts"]);
                forum.TotalTopics = Convert.ToInt32(dr["TotalTopics"]);

                forum.MostRecentPostAuthor = Convert.ToString(dr["MostRecentPostAuthor"]);
				
                if (Convert.IsDBNull(dr["MostRecentPostDate"]))
                    forum.MostRecentPostDate = DateTime.MinValue;
                else
                    forum.MostRecentPostDate = Convert.ToDateTime(dr["MostRecentPostDate"]);


                forums.Add(forum);
            }
            dr.Close();
            myConnection.Close();

            return forums;
        }



        /// <summary>
        /// Returns a list of all forums less one particular forum.
        /// </summary>
        /// <param name="PostID">The ID of a Post that is in the Forum you wish to exclude.</param>
        /// <returns>A ForumCollection that contains all of the Forums except for the forum that contains
        /// the PostID passed in.</returns>
        public  ForumCollection GetAllButOneForum(int PostID) {
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();

            // get forum information on the post
            Post post = GetPost(PostID, myConnection);
            int forumIDtoExclude = post.ForumID;

            String strSQL = "SELECT	*FROM [Forums] WHERE NOT ([ForumID] = @ExcludeForumID) AND [Active] = YES";

            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            OleDbParameter parameterExcludeForumID = new OleDbParameter("@ExcludeForumID", OleDbType.Integer, 4);
            parameterExcludeForumID.Value = forumIDtoExclude;
            myCommand.Parameters.Add(parameterExcludeForumID);

            // Execute the command
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // populate a ForumCollection object
            ForumCollection forums = new ForumCollection();
            while (dr.Read()) {
                forums.Add(PopulateForumFromOleDbDataReader(dr));
            }

            dr.Close();
            myConnection.Close();

            return forums;
        }

		

        /// <summary>
        /// Deletes a forum and all of its posts.
        /// </summary>
        /// <param name="ForumID">The ID of the forum to delete.</param>
        /// <remarks>Be very careful when using this method.  It will permanently delete ALL of the
        /// posts associated with the forum.</remarks>
        public  void DeleteForum(int ForumID) {
            // first delete all of the threadtrackings for the forum
            String strSQL = "DELETE FROM [ThreadTrackings] WHERE [ThreadID] IN (SELECT DISTINCT [ThreadID] FROM [Posts] WHERE [ForumID] = @ForumID)";
            
            // return all of the forums and their total and daily posts
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = ForumID;
            myCommand.Parameters.Add(parameterForumID);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();

            // now remove the moderators
            strSQL = "DELETE FROM [Moderators] WHERE [ForumID] = @ForumID";

            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();
            myCommand.Parameters.Add(parameterForumID);
            myCommand.ExecuteNonQuery();

            // now remove all of the posts
            strSQL = "DELETE FROM [Posts] WHERE [ForumID] = @ForumID";

            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();
            myCommand.Parameters.Add(parameterForumID);
            myCommand.ExecuteNonQuery();

            // finally, remove the forum
            strSQL = "	DELETE FROM [Forums] WHERE [ForumID] = @ForumID";

            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();
            myCommand.Parameters.Add(parameterForumID);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }


        /// <summary>
        /// Adds a new forum.
        /// </summary>
        /// <param name="forum">A Forum object instance that defines the variables for the new forum to
        /// be added.  The Forum object properties used to create the new forum are: Name, Description,
        /// Moderated, and DaysToView.</param>
        public  void AddForum(Forum forum) {
            String strSQL = "INSERT INTO [Forums] (ForumGroupId, [Name], [Description], [Moderated], [DaysToView], [Active]) " +
                "VALUES (@ForumGroupId, @Name, @Description, @Moderated, @DaysToView, @Active)";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterForumName = new OleDbParameter("@Name", OleDbType.VarChar, 100);
            parameterForumName.Value = forum.Name;
            myCommand.Parameters.Add(parameterForumName);

            OleDbParameter parameterForumDesc = new OleDbParameter("@Description", OleDbType.VarChar, 5000);
            parameterForumDesc.Value = forum.Description;
            myCommand.Parameters.Add(parameterForumDesc);

            OleDbParameter parameterForumGroupId = new OleDbParameter("@Description", OleDbType.Integer, 4);
            parameterForumGroupId.Value = forum.ForumGroupId;
            myCommand.Parameters.Add(parameterForumGroupId);
            
            OleDbParameter parameterModerated = new OleDbParameter("@Moderated", OleDbType.Boolean, 1);
            parameterModerated.Value = forum.Moderated;
            myCommand.Parameters.Add(parameterModerated);

            OleDbParameter parameterViewToDays = new OleDbParameter("@DaysToView", OleDbType.Integer, 4);
            parameterViewToDays.Value = forum.DaysToView;
            myCommand.Parameters.Add(parameterViewToDays);

            OleDbParameter parameterActive = new OleDbParameter("@Active", OleDbType.Boolean, 1);
            parameterActive.Value = forum.Active;
            myCommand.Parameters.Add(parameterActive);


            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }



        /// <summary>
        /// Updates an existing forum.
        /// </summary>
        /// <param name="forum">A Forum object with the new, updated properties.  The ForumID property
        /// specifies what forum to update, while hte Name, Description, Moderated, and DaysToView
        /// properties indicate the new values.</param>
        public  void UpdateForum(Forum forum) {
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();
            OleDbCommand myCommand = new OleDbCommand();

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            myCommand.Connection = myConnection;

            // Add Parameters to SPROC
            OleDbParameter parameterForumId = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumId.Value = forum.ForumID;


            // if we are making the forum unmoderated, remove all moderators
            String strSQL;
            if (!forum.Moderated) {
                strSQL = "DELETE FROM [Moderators] WHERE ForumID = @ForumID";

                myCommand.CommandText = strSQL;
                myCommand.Parameters.Add(parameterForumId);
                myCommand.ExecuteNonQuery();

                myCommand.Parameters.Clear();
            }

            strSQL = "UPDATE [Forums] SET [Name] = @Name, [Description] = @Description, [Moderated] = @Moderated, [DaysToView] = @DaysToView, [Active] = @Active " +
                "WHERE [ForumID] = @ForumID";

            myCommand.CommandText = strSQL;

            OleDbParameter parameterForumName = new OleDbParameter("@Name", OleDbType.VarChar, 100);
            parameterForumName.Value = forum.Name;
            myCommand.Parameters.Add(parameterForumName);

            OleDbParameter parameterForumDesc = new OleDbParameter("@Description", OleDbType.VarChar, 5000);
            parameterForumDesc.Value = forum.Description;
            myCommand.Parameters.Add(parameterForumDesc);

            OleDbParameter parameterModerated = new OleDbParameter("@Moderated", OleDbType.Boolean, 1);
            parameterModerated.Value = forum.Moderated;
            myCommand.Parameters.Add(parameterModerated);

            OleDbParameter parameterViewToDays = new OleDbParameter("@DaysToView", OleDbType.Integer, 4);
            parameterViewToDays.Value = forum.DaysToView;
            myCommand.Parameters.Add(parameterViewToDays);

            OleDbParameter parameterActive = new OleDbParameter("@Active", OleDbType.Boolean, 1);
            parameterActive.Value = forum.Active;
            myCommand.Parameters.Add(parameterActive);

            myCommand.Parameters.Add(parameterForumId);


            // Execute the command			
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }


        /// <summary>
        /// Returns the total number of forums.
        /// </summary>
        /// <returns>The total number of forums.</returns>
        public int TotalNumberOfForums() {
            String strSQL = "SELECT COUNT(*) FROM [Forums]";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a text
            myCommand.CommandType = CommandType.Text;

            myConnection.Open();
            int totalForums = Convert.ToInt32(myCommand.ExecuteScalar());
            myConnection.Close();

            return totalForums;
        }
        /*********************************************************************************/




        /*********************************************************************************/

        /************************ USER FUNCTIONS ***********************
                 * These functions return information about a user.
                 * are called from the WebForums.Users class.
                 * *************************************************************/
	
        /// <summary>
        /// Retrieves information about a particular user.
        /// </summary>
        /// <param name="Username">The name of the User whose information you are interested in.</param>
        /// <returns>A User object.</returns>
        /// <remarks>If a Username is passed in that is NOT found in the database, a UserNotFoundException
        /// exception is thrown.</remarks>
        public  User GetUserInfo(String Username) {
            String strSQL = "SELECT * FROM [Users] WHERE UserName = @Username";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            if (!dr.Read()) {
                // we didn't get a forum, handle it
                dr.Close();
                myConnection.Close();

                throw new Components.UserNotFoundException("User not found for Username " + Username);
            }
            else {
                User user = PopulateUserFromOleDbDataReader(dr);
                dr.Close();
                myConnection.Close();

                return user;
            }
        }


        /// <summary>
        /// Get basic information about a single post.  This method returns an instance of the Post class,
        /// which contains less information than the PostDeails class, which is what is returned by the
        /// GetPostDetails method.
        /// </summary>
        /// <param name="Username">The name of the User whose information you are interested in.</param>
        /// <param name="myConnection">An open OleDbConnection object.</param>
        /// <returns>A User object.</returns>
        /// <remarks>If a Username is passed in that is NOT found in the database, a UserNotFoundException
        /// exception is thrown.  This form of GetUserInfo should be used when we already have an openned
        /// OleDbConnection object.  If this is not the case, use the other form of the GetUserInfo method.</remarks>
        public  User GetUserInfo(String Username, OleDbConnection myConnection) {
            String strSQL = "SELECT * FROM [Users] WHERE UserName = @Username";

            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);

            // Execute the command
            OleDbDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read()) {
                // we didn't get a forum, handle it
                dr.Close();

                throw new Components.UserNotFoundException("User not found for Username " + Username);
            }
            else {
                User user = PopulateUserFromOleDbDataReader(dr);
                dr.Close();

                return user;
            }
        }


	
        /// <summary>
        /// Updates a user's information.
        /// </summary>
        /// <param name="user">A User object that contains information about the existing user.</param>
        /// <param name="NewPassword">The new password for the User.  (If the user has not changed their
        /// password, this property should be their existing password.)</param>
        /// <returns>A boolean - true if the user's password was correct, false otherwise.  In the case
        /// of an incorrect password being entered, the update is not performed.</returns>
        public  bool UpdateUserInfo(User user, String NewPassword) {
            // first determine if the password is correct
            String strSQL = "SELECT COUNT(*) FROM [Users] WHERE [Username] = @Username AND [Password] = @Password";
            
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();
			
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = user.Username;
            myCommand.Parameters.Add(parameterUsername);

            OleDbParameter parameterPassword = new OleDbParameter("@Password", OleDbType.VarChar, 20);
            parameterPassword.Value = user.Password;
            myCommand.Parameters.Add(parameterPassword);


            if ((int) myCommand.ExecuteScalar() == 0) {
                // the user's username/password doesn't match up
                myConnection.Close();
                return false;
            }
            else {
                // the username/password match up, update the user's information
                strSQL = "UPDATE [Users] SET [Email] = @Email, [FakeEmail] = @FakeEmail, [URL] = @URL, [Signature] = @Signature, [ForumView] = @ForumView, [TrackYourPosts] = @ThreadTracking, [Timezone] = @Timezone, [Password] = @NewPassword " +
                    "WHERE [UserName] = @UserName";
			
                myCommand.Parameters.Clear();
                myCommand.CommandText = strSQL;


                OleDbParameter parameterEmail = new OleDbParameter("@Email", OleDbType.VarChar, 75);
                parameterEmail.Value = user.Email;
                myCommand.Parameters.Add(parameterEmail);

                OleDbParameter parameterFakeEmail = new OleDbParameter("@FakeEmail", OleDbType.VarChar, 75);
                parameterFakeEmail.Value = user.FakeEmail;
                myCommand.Parameters.Add(parameterFakeEmail);

                OleDbParameter parameterURL = new OleDbParameter("@URL", OleDbType.VarChar, 100);
                parameterURL.Value = user.Url;
                myCommand.Parameters.Add(parameterURL);

                OleDbParameter parameterSig = new OleDbParameter("@Signature", OleDbType.VarChar, 255);
                parameterSig.Value = user.Signature;
                myCommand.Parameters.Add(parameterSig);

                OleDbParameter parameterForumView = new OleDbParameter("@ForumView", OleDbType.Integer, 4);
                parameterForumView.Value = (int) user.ForumView;
                myCommand.Parameters.Add(parameterForumView);

                OleDbParameter parameterTT = new OleDbParameter("@ThreadTracking", OleDbType.Boolean, 1);
                parameterTT.Value = user.TrackPosts;
                myCommand.Parameters.Add(parameterTT);

                OleDbParameter parameterTZ = new OleDbParameter("@Timezone", OleDbType.Integer, 4);
                parameterTZ.Value = user.Timezone;
                myCommand.Parameters.Add(parameterTZ);

                OleDbParameter parameterNewPassword = new OleDbParameter("@NewPassword", OleDbType.VarChar, 20);
                parameterNewPassword.Value = NewPassword;
                myCommand.Parameters.Add(parameterNewPassword);

                myCommand.Parameters.Add(parameterUsername);

                myCommand.ExecuteNonQuery();

                myConnection.Close();

                return true;
            }
        }



        /// <summary>
        /// Returns a collection of users whose Username begins with a specified character.
        /// </summary>
        /// <param name="FirstCharacter">The starting character.</param>
        /// <returns>A UserCollection object with Users whose Username begins with the specified
        /// FirstCharacter letter.</returns>
        public  UserCollection GetUsersByFirstCharacter(String FirstCharacter) {
            String strSQL = "SELECT [UserName], [Password], [Email], [FakeEmail], [Approved], [Administrator], [Trusted], [Signature], [URL], [Timezone], [DateCreated], [LastLogin], [ForumView], [TrackYourPosts] " +
                "FROM [Users] WHERE LEFT([UserName], 1) = @FirstLetter";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            OleDbParameter parameterUsername = new OleDbParameter("@FirstLetter", OleDbType.Char, 1);
            parameterUsername.Value = FirstCharacter;
            myCommand.Parameters.Add(parameterUsername);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            UserCollection users = new UserCollection();
            while (dr.Read()) {
                users.Add(PopulateUserFromOleDbDataReader(dr));
            }

            dr.Close();
            myConnection.Close();

            return users;
        }

		

        /// <summary>
        /// Updates a user's information via the administration page.
        /// </summary>
        /// <param name="user">The information about the User you wish to update.  Note that the
        /// Username of the User identifies the user you wish to update, while the Approved,
        /// Trusted, and Administrator properties specify the new settings for those fields.</param>
        public void UpdateUserInfoFromAdminPage(User user) {
            String strSQL = "UPDATE [Users] SET [Approved] = @Approved, [Trusted] = @Trusted, [Administrator] = @Admin WHERE [Username] = @Username";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;


            OleDbParameter parameterApproved = new OleDbParameter("@Approved", OleDbType.Boolean, 1);
            parameterApproved.Value = user.Approved;
            myCommand.Parameters.Add(parameterApproved);

            OleDbParameter parameterTrusted = new OleDbParameter("@Trusted", OleDbType.Boolean, 1);
            parameterTrusted.Value = user.Trusted;
            myCommand.Parameters.Add(parameterTrusted);

            OleDbParameter parameterAdmin = new OleDbParameter("@Admin", OleDbType.Boolean, 1);
            parameterAdmin.Value = user.IsAdministrator;
            myCommand.Parameters.Add(parameterAdmin);

            OleDbParameter parameterUsername = new OleDbParameter("@Username", OleDbType.VarChar, 50);
            parameterUsername.Value = user.Username;
            myCommand.Parameters.Add(parameterUsername);


            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

		
        /// <summary>
        /// This method creates a new user if possible.  If the username or
        /// email addresses already exist, an appropriate CreateUserStatus message is
        /// returned.
        /// </summary>
        /// <param name="user">The email for the new user account.</param>
        /// <returns>An CreateUserStatus enumeration value, indicating if the user was created successfully
        /// (CreateUserStatus.Created) or if the new user couldn't be created because of a duplicate
        /// Username (CreateUserStatus.DuplicateUsername) or duplicate email address (CreateUserStatus.DuplicateEmailAddress).</returns>
        /// <remarks>All User accounts created must consist of a unique Username and a unique
        /// Email address.</remarks>
        public CreateUserStatus CreateNewUser(User user) {
            // make sure the user's name begins with an alphabetic character
            if (!Regex.IsMatch(user.Username, "^[A-Za-z].*"))
                return CreateUserStatus.InvalidFirstCharacter;

            // first, determine if the username is a duplicate
            String strSQL = "SELECT COUNT(*) FROM [Users] WHERE [Username] = @Username";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@Username", OleDbType.VarChar, 50);
            parameterUsername.Value = user.Username;
            myCommand.Parameters.Add(parameterUsername);

            if ((int) myCommand.ExecuteScalar() > 0)
                // duplicate username
                return CreateUserStatus.DuplicateUsername;

            // see if the email is a duplicate
            strSQL = "SELECT COUNT(*) FROM [Users] WHERE [Email] = @Email";

            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();

            OleDbParameter parameterEmail = new OleDbParameter("@Email", OleDbType.VarChar, 75);
            parameterEmail.Value = user.Email;
            myCommand.Parameters.Add(parameterEmail);

            if ((int) myCommand.ExecuteScalar() > 0)
                // duplicate email address
                return CreateUserStatus.DuplicateEmailAddress;

            // ok, if we reach here, everything is groovy, insert the user
            strSQL = "INSERT INTO [Users] ([Username], [Email], [Password]) " +
                "VALUES (@Username, @Email, @RandomPassword)";

            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();
            myCommand.Parameters.Add(parameterUsername);
            myCommand.Parameters.Add(parameterEmail);

            OleDbParameter parameterPassword = new OleDbParameter("@RandomPassword", OleDbType.VarChar, 20);
            parameterPassword.Value = user.Password;
            myCommand.Parameters.Add(parameterPassword);
            

            // Execute the command
            try {
                myCommand.ExecuteNonQuery();
            } 
            catch (Exception e) {
                return CreateUserStatus.UnknownFailure;
            }

            myConnection.Close();
			
            return CreateUserStatus.Created;		// if we reach here, the user was successfully created.
        }

		
        /// <summary>
        /// This method determines whether or not a particular username/password combo
        /// is valid.
        /// </summary>
        /// <param name="user">The User to determine if he/she is valid.</param>
        /// <returns>A boolean value, indicating if the username and password are valid.</returns>
        public bool ValidUser(User user) {
            String strSQL = "SELECT [Username] FROM [Users] WHERE [Username] = @Username AND [Password] = @Password";


            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@Username", OleDbType.VarChar, 50);
            parameterUsername.Value = user.Username;
            myCommand.Parameters.Add(parameterUsername);

            OleDbParameter parameterPassword = new OleDbParameter("@Password", OleDbType.VarChar, 20);
            parameterPassword.Value = user.Password;
            myCommand.Parameters.Add(parameterPassword);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader();
            bool validUser = dr.Read();
            dr.Close();

            if (validUser) {
                // update last login time				
                strSQL = "UPDATE [Users] SET [LastLogin] = Now() WHERE [Username] = @Username";

                OleDbCommand mySecondCommand = new OleDbCommand(strSQL, myConnection);

                OleDbParameter parameterUsernameUpdateLoginTime = new OleDbParameter("@Username", OleDbType.VarChar, 50);
                parameterUsernameUpdateLoginTime.Value = user.Username;
                mySecondCommand.Parameters.Add(parameterUsernameUpdateLoginTime);

                mySecondCommand.ExecuteNonQuery();
            }

            myConnection.Close();

            return validUser;
        }


        /// <summary>
        /// Calculates and returns the total number of user accounts.
        /// </summary>
        /// <returns>The total number of user accounts created.</returns>
        public int TotalNumberOfUserAccounts() {
            String strSQL = "SELECT COUNT(*) FROM [Users]";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            myConnection.Open();
            int userAccountCount = Convert.ToInt32(myCommand.ExecuteScalar());
            myConnection.Close();

            return userAccountCount;
        }
        /*********************************************************************************/




        /*********************************************************************************/

        /************************ SEARCH FUNCTIONS ***********************
                 * These functions are used to perform searching.
                 * ***************************************************************/
	
        /// <summary>
        /// Performs a search, returning a PostCollection object with appropriate posts.
        /// </summary>
        /// <param name="ToSearch">Specifies what to search, specifically.  Must be set to a valid
        /// ToSearchEnum value, which supports two possible values: PostsSearch and PostsBySearch.</param>
        /// <param name="SearchWhat">A SearchWhatEnum value, this parameter specifies what to search. 
        /// Acceptable values are: SearchAllWords, SearchAnyWord, and SearchExactPhrase.</param>
        /// <param name="ForumToSearch">Specifies what forum to search.  To search all forums, pass in a
        /// value of 0.</param>
        /// <param name="SearchTerms">Specifies the terms to search on.</param>
        /// <param name="Page">Specifies what page of the search results to display.</param>
        /// <param name="RecsPerPage">Specifies how many records per page to show on the search
        /// results.</param>
        /// <returns>A PostCollection object, containing the posts to display for the particular page
        /// of the search results.</returns>
        public  PostCollection PerformSearch(ToSearchEnum ToSearch, SearchWhatEnum SearchWhat,
            int ForumToSearch, String SearchTerms, int Page, int RecsPerPage) {
            // return all of the forums and their total and daily posts
            // first, though, we've got to put our search phrase in the right order
            String strColumnName = "";
            if (ToSearch == ToSearchEnum.PostsSearch) strColumnName = "Body";
            else if (ToSearch == ToSearchEnum.PostsBySearch) strColumnName = "UserName";
			
            String strWhereClause = " WHERE (";
            String [] aTerms = null;
			
            // depending on the search style, our WHERE clause will differ
            switch(SearchWhat) {
                case SearchWhatEnum.SearchExactPhrase:
                    // easy, we want to search for the exact search term
                    strWhereClause += "[" + strColumnName + "] LIKE '%" + SearchTerms + "%' ";
                    break;
					
                case SearchWhatEnum.SearchAllWords:
                    // allrighty, we want to find rows where each word is found
                    // split up the search term string into an array
                    aTerms = SearchTerms.Split(new char[]{' '});
					
                    // now, loop through the aTerms array
                    strWhereClause += "[" + strColumnName + "] LIKE '%" + String.Join("%' AND [" + strColumnName + "] LIKE '%", aTerms) + "%'";
                    break;

                case SearchWhatEnum.SearchAnyWord:
                    // allrighty, we want to find rows where each word is found
                    // split up the search term string into an array
                    aTerms = SearchTerms.Split(new char[]{' '});
					
                    // now, loop through the aTerms array
                    strWhereClause += "[" + strColumnName + "] LIKE '%" + String.Join("%' OR [" + strColumnName + "] LIKE '%", aTerms) + "%'";
                    break;
            }
			
            strWhereClause += ") AND [Approved]=YES ";


            // see if we need to add a restriction on the ForumID
            if (ForumToSearch > 0)
                strWhereClause += " AND P.[ForumID] = " + ForumToSearch.ToString() + " ";
			
            String strSQL = "SELECT P.[PostID], P.[ParentID], P.[ThreadID], P.[PostLevel], P.[SortOrder], P.[UserName], P.[Subject], 0 AS [Replies], " +
                "P.[PostDate], P.[ThreadDate], P.[Approved], P.[ForumID], F.[Name] As [ForumName], P.[Body] FROM [Posts] AS P " +
                "INNER JOIN [Forums] AS F ON F.[ForumID] = P.[ForumID] " + strWhereClause + " ORDER BY P.[PostDate] DESC";
            //HttpContext.Current.Response.Write(strSQL); HttpContext.Current.Response.End();
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;
            

            // Execute the command
            myConnection.Open();

            myCommand.CommandText = "SELECT COUNT(*) FROM [Posts] " + strWhereClause;
            int totalRecords = (int) myCommand.ExecuteScalar();

            myCommand.CommandText = strSQL;
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // populate the Posts collection
            PostCollection posts = new PostCollection();
            if (!dr.Read()) {
                // we have an empty result, return the empty post collection
                dr.Close();
                myConnection.Close();

                return posts;
            }
            else {
                int count = 0;
                int startindex = RecsPerPage * (Page - 1);

                // move to the proper start index
                while (count < startindex) {
                    dr.Read();				
                    count++;
                }

                // we have to populate our postcollection
                count = 0;
                do {
                    posts.Add(PopulatePostFromOleDbDataReader(dr));
                    ((Post) posts[posts.Count - 1]).ForumName = Convert.ToString(dr["ForumName"]);					

                    count++;
                } while (dr.Read() && count < RecsPerPage);


                posts.TotalRecordCount = totalRecords;

                dr.Close();
                myConnection.Close();

                return posts;
            }
        }
        /*********************************************************************************/




        /*********************************************************************************/

        /********************* MODERATION FUNCTIONS *********************
                 * These functions are used to perform moderation.  They are called
                 * from the WebForums.Moderate class.
                 * **************************************************************/

        /// <summary>
        /// Gets a list of posts that are awaiting moderation that the current user has rights to moderate.
        /// </summary>
        /// <param name="Username">The User who is interested in viewing a list of posts awaiting
        /// moderation.</param>
        /// <returns>A PostCollection containing the posts the user can view that are awaiting moderation.</returns>
        public  PostCollection GetPostsAwaitingModeration(String Username) {
            // Find out if this user can moderate all forums
            String strSQL = "SELECT COUNT(*) FROM [Moderators] WHERE [Username] = @Username AND [ForumID] = 0";
            
            // return all of the forums and their total and daily posts
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);
            myConnection.Open();

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);

            strSQL = "SELECT [PostID], [ThreadID], [ThreadDate], [PostLevel], [SortOrder], [ParentID], [Subject], [Approved], P.[ForumID], F.[Name] AS [ForumName], [PostDate], P.[UserName], 0 AS [Replies], [Body] " +
                "FROM [Posts] AS P INNER JOIN [Forums] AS F ON F.[ForumID] = P.[ForumID] WHERE [Approved] = NO ";

            if ((int) myCommand.ExecuteScalar() == 0) {
                // this user can moderate only a subset of forums
                strSQL += "AND P.[ForumID] IN (SELECT [ForumID] FROM [Moderators] WHERE [Username] = @Username) ";
            }

            strSQL += "ORDER BY P.[ForumID], [PostDate]";


            myCommand.Parameters.Clear();
            myCommand.CommandText = strSQL;
            myCommand.Parameters.Add(parameterUsername);

            // Execute the command			
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            PostCollection posts = new PostCollection();
            Post post = null;

            while (dr.Read()) {
                post = PopulatePostFromOleDbDataReader(dr);
                post.ForumName = Convert.ToString(dr["ForumName"]);

                posts.Add(post);
            }
            dr.Close();
            myConnection.Close();

            return posts;
        }



        /// <summary>
        /// Approves a particular post that is waiting to be moderated.
        /// </summary>
        /// <param name="PostID">The ID of the post to approve.</param>
        /// <returns>A boolean indicating if the post has already been approved.</returns>
        /// <remarks>Keep in mind that multiple moderators may be working on approving/moving/editing/deleting
        /// posts at the same time.  Hence, these moderation functions may not perform the desired task.
        /// For example, if one opts to delete a post that has already been approved, the deletion will
        /// fail.</remarks>
        public bool ApprovePost(int PostID) {
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();
            OleDbCommand myCommand = new OleDbCommand();

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // set the connection
            myCommand.Connection = myConnection;

            // determine if the post has already been approved
            Post post = GetPost(PostID, myConnection);

            if (post.Approved)
                return false;

            // ok, post has not yet been approved, so approve it!
            String strSQL = "UPDATE [Posts] SET [Approved] = YES WHERE [PostID] = @PostID";

            myCommand.CommandText = strSQL;

            // Add Parameters to SPROC
            OleDbParameter parameterPostID = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostID.Value = PostID;
            myCommand.Parameters.Add(parameterPostID);

            // Execute the command
            myCommand.ExecuteNonQuery();

            // now update the threaddate for all of the messages in the thread
            strSQL = "UPDATE [Posts] SET [ThreadDate] = Now() WHERE [ThreadID] = (SELECT [ThreadID] FROM [Posts] WHERE [PostID] = @PostID)";

            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();
            myCommand.Parameters.Add(parameterPostID);

            myCommand.ExecuteNonQuery();
            myConnection.Close();

            return true;
        }


        /// <summary>
        /// Deletes a post that is currently waiting to be moderated.
        /// </summary>
        /// <param name="PostID">The ID of the post to delete.</param>
        /// <returns>A boolean, true if the post has been deleted, false otherwise.  The post might not
        /// be deleted if someone else has already approved the post.</returns>
        /// <remarks>Keep in mind that multiple moderators may be working on approving/moving/editing/deleting
        /// posts at the same time.  Hence, these moderation functions may not perform the desired task.
        /// For example, if one opts to delete a post that has already been approved, the deletion will
        /// fail.</remarks>
        public  bool DeleteModeratedPost(int PostID) {
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();

            // determine if the post is approved - if it is, return false
            if (((Post) GetPost(PostID, myConnection)).Approved)
                return false;

            // otherwise we need to delete this post
            String strSQL = "DELETE FROM [Posts] WHERE [PostID] = @PostID AND Approved = NO";

            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterPostID = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostID.Value = PostID;
            myCommand.Parameters.Add(parameterPostID);

            // Execute the command
            myConnection.Open();
            int iRowsAffectedCount = myCommand.ExecuteNonQuery();
            myConnection.Close();
            
            return iRowsAffectedCount != 0;		
        }


        /// <summary>
        /// Indicates if a particular user can moderate posts.
        /// </summary>
        /// <param name="Username">The User to check.</param>
        /// <returns>True if the user can moderate, False otherwise.</returns>
        public  bool CanModerate(String Username) {
            String strSQL = "SELECT COUNT(*) FROM [Moderators] WHERE [Username] = @Username";

            // return all of the forums and their total and daily posts
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);

            // Execute the command
            myConnection.Open();
            bool bolCanModerate = (int) myCommand.ExecuteScalar() > 0;
            myConnection.Close();
            
            return bolCanModerate;
        }



        /// <summary>
        /// Determines if a user can edit a particular post.
        /// </summary>
        /// <param name="Username">The name of the User.</param>
        /// <param name="PostID">The Post the User wants to edit.</param>
        /// <returns>A boolean value - True if the user can edit the Post, False otherwise.</returns>
        /// <remarks>An Administrator can edit any post.  Moderators may edit posts from forums that they
        /// have moderation rights to and that are awaiting approval.</remarks>
        public  bool CanEditPost(String Username, int PostID) {
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();

            // find out if the user in an administrator - if they are, they can edit the post
            if (((User) GetUserInfo(Username, myConnection)).IsAdministrator)
                return true;

            // ok, the user is not an administrator.  If the Post is NOT approved AND the user 
            // has rights to moderate the post's forum, then let them edit, otherwise, do not
            Post post = GetPost(PostID, myConnection);		// get information about the post

            // if it is approved, return false
            if (post.Approved)
                return false;

            // otherwise, if the user is a moderator for the forum, return true
            String strSQL = "SELECT COUNT(*) FROM [Moderators] WHERE ([ForumID] = 0 OR [ForumID] = @ForumID) AND [Username] = @Username)";

            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = post.ForumID;
            myCommand.Parameters.Add(parameterForumID);

            OleDbParameter parameterUsername = new OleDbParameter("@Username", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);

            // Execute the command
            int iResponse = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
            myConnection.Close();
            
            return iResponse > 0;
        }

        public UserGroupCollection GetUserGroups() {
            return null; //TODO
        }

        /// <summary>
        /// Moves a post awaiting moderation from one Forum to another.
        /// </summary>
        /// <param name="PostID">The ID of the Post to move.</param>
        /// <param name="MoveToForumID">The ID of the forum to move the post to.</param>
        /// <param name="Username">The name of the User who is attempting to move the post.</param>
        /// <returns>A MovedPostStatus enumeration value that indicates the status of the attempted move.
        /// This enumeration has three values: NotMoved, MovedButNotApproved, and MovedAndApproved.</returns>
        /// <remarks>A value of NotMoved means the post was not moved (either it has been approved already
        /// or deleted); a value of MovedButNotApproved indicates that the post has been moved to a new
        /// forum, but the user moving the post was NOT a moderator for the forum it was moved to, hence
        /// the moved post is still waiting to be approved; a value of MovedAndApproved indicates that the
        /// moderator moved to post to a forum he moderates, hence the post is automatically approved.</remarks>
        public  MovedPostStatus MovePost(int PostID, int MoveToForumID, String Username) {
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();

            // Get information about the Post, make sure it hasn't already been approved			
            try {
                Post post = GetPost(PostID, myConnection);

                if (post.Approved || post.ParentID != PostID)
                    // eep, the post has already been approved, cannot move it, or the post is not the top-level post
                    return MovedPostStatus.NotMoved;
            }
            catch (PostNotFoundException e) {
                // the post has been deleted
                return MovedPostStatus.NotMoved;
            }

            // Get information about the forum we are moving the post to
            Forum forum = this.GetForumInfo(MoveToForumID, myConnection);			

            // moves a post to a specified forum
            // Create Instance of Connection and Command Object
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterForumID = new OleDbParameter("@MoveToForumID", OleDbType.Integer, 4);
            parameterForumID.Value = MoveToForumID;

            OleDbParameter parameterUsername = new OleDbParameter("@Username", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;

            bool messageApproved = false;
            String strSQL;

            if (forum.Moderated) {
                // determine if this user has rights to approve the post where it's being moved to
                strSQL = "SELECT COUNT(*) FROM [Moderators] WHERE ([ForumID] = @MoveToForumID OR [ForumID] = 0) AND [Username] = @Username)";

                myCommand.CommandText = strSQL;
                myCommand.Parameters.Add(parameterForumID);
                myCommand.Parameters.Add(parameterUsername);

                // Execute the command
                int iStatus = Convert.ToInt32(myCommand.ExecuteScalar().ToString());

                if (iStatus > 0)
                    messageApproved = true;
            }
            else
                messageApproved = true;


            // update the post information - that is, move it!
            strSQL = "UPDATE [Posts] SET [ForumID] = @MoveToForumID, [Approved] = @ApproveSetting WHERE [PostID] = @PostID";

            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();
            myCommand.Parameters.Add(parameterForumID);

            OleDbParameter parameterApproveSetting = new OleDbParameter("@ApproveSetting", OleDbType.Boolean, 1);
            parameterApproveSetting.Value = messageApproved;
            myCommand.Parameters.Add(parameterApproveSetting);
			
            OleDbParameter parameterPostID = new OleDbParameter("@PostID", OleDbType.Integer, 4);
            parameterPostID.Value = PostID;
            myCommand.Parameters.Add(parameterPostID);

            myCommand.ExecuteNonQuery();
            myConnection.Close();

            // Determine the status of the moved post
            if (!messageApproved)
                return MovedPostStatus.MovedButNotApproved;
            else
                return MovedPostStatus.MovedAndApproved;
        }
        /*********************************************************************************/




        /*********************************************************************************/

        /********************* EMAIL FUNCTIONS *********************
                 * These functions are used to perform email functionality.
                 * They are called from the WebForums.Email class
                 * *********************************************************/

        /// <summary>
        /// Returns a list of Users that have email thread tracking turned on for a particular post
        /// in a particular thread.
        /// </summary>
        /// <param name="PostID">The ID of the Post of the thread to send a notification to.</param>
        /// <returns>A UserCollection listing the users who have email thread tracking turned on for
        /// the specified thread.</returns>
        public  UserCollection GetEmailList(int PostID) {
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();

            Post post = GetPost(PostID, myConnection);
            String strSQL = "SELECT [Email] FROM [Users] AS U INNER JOIN [ThreadTrackings] AS TT ON TT.[Username] = U.[Username] WHERE TT.[ThreadID] = @ThreadID AND TT.[Username] = @Username";

            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterThreadId = new OleDbParameter("@ThreadID", OleDbType.Integer, 4);
            parameterThreadId.Value = post.ThreadID;
            myCommand.Parameters.Add(parameterThreadId);

            OleDbParameter parameterUsername = new OleDbParameter("@Username", OleDbType.VarChar, 50);
            parameterUsername.Value = post.Username;
            myCommand.Parameters.Add(parameterUsername);

	
            // Execute the command
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            UserCollection users = new UserCollection();
            User u;
            while (dr.Read()) {
                u = new User();
                u.Email = dr["Email"].ToString();
                users.Add(u);
            }

            dr.Close();
            myConnection.Close();

            return users;
        }


        /// <summary>
        /// Returns information about a particular Email Template.
        /// </summary>
        /// <param name="EmailTemplateID">The ID of the Email Template.</param>
        /// <returns>An EmailTemplate class instance.</returns>
        /// <remarks>If the passed in EmailTemplateID does not match to a database entry, a
        /// EmailTemplateNotFoundException exception is thrown.</remarks>
        public EmailTemplate GetEmailTemplateInfo(int EmailTemplateID) {
            String strSQL = "SELECT	[Subject], [FromAddress], [Importance], [EmailID], [Description], [Message] FROM [Emails] WHERE [EmailID] = @EmailID";
			
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterEmailId = new OleDbParameter("@EmailId", OleDbType.Integer, 4);
            parameterEmailId.Value = EmailTemplateID;
            myCommand.Parameters.Add(parameterEmailId);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (!dr.Read()) {
                dr.Close();
                myConnection.Close();

                throw new Components.EmailTemplateNotFoundException("Email template not found for EmailTemplateID " + EmailTemplateID.ToString());
            }

            EmailTemplate email = PopulateEmailTemplateFromOleDbDataReader(dr);

            dr.Close();
            myConnection.Close();
			
            return  email;
        }



        /// <summary>
        /// Returns a list of all of the Email Templates.
        /// </summary>
        /// <returns>An EmailTemplateCollection instance, that contains a listing of all of the available
        /// Email Templates.</returns>
        public  EmailTemplateCollection GetEmailTemplateList() {
            String strSQL = "SELECT [EmailID], [Description], [Subject], [Importance], [FromAddress], [Message] FROM [Emails] ORDER BY [Description]";
			
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            EmailTemplateCollection emails = new EmailTemplateCollection();
            while (dr.Read()) {
                emails.Add(PopulateEmailTemplateFromOleDbDataReader(dr));
            }
            dr.Close();
            myConnection.Close();

            return emails;
        }


        /// <summary>
        /// Updates an existing Email Template.
        /// </summary>
        /// <param name="email">An EmailTemplate object instance that contains information on the
        /// Email Template to update.  The EmailID property of the passed in EmailTemplate instance
        /// specifies the Email Template to update, while the Subject and Message properties indicate
        /// the updated Subject and Message values for the Email Template.</param>
        public void UpdateEmailTemplate(EmailTemplate email) {
            String strSQL = "UPDATE [Emails] SET [Subject] = @Subject, [Message] = @Message WHERE [EmailID] = @EmailID";

            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterSubject = new OleDbParameter("@Subject", OleDbType.VarChar, 50);
            parameterSubject.Value = email.Subject;
            myCommand.Parameters.Add(parameterSubject);

            OleDbParameter parameterMessage = new OleDbParameter("@Message", OleDbType.LongVarWChar);
            parameterMessage.Value = email.Body;
            myCommand.Parameters.Add(parameterMessage);

            OleDbParameter parameterEmailId = new OleDbParameter("@EmailId", OleDbType.Integer, 4);
            parameterEmailId.Value = email.EmailTemplateID;
            myCommand.Parameters.Add(parameterEmailId);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        /*********************************************************************************/





        /*********************************************************************************/

        /**************** MODERATOR LISTING FUNCTIONS **************
                 * These functions are used to edit/update/work with the list
                 * of forums a user can moderate.
                 * *********************************************************/

        /// <summary>
        /// Retrieves a list of the Forums moderated by a particular user.
        /// </summary>
        /// <param name="user">The User whose list of moderated forums we are interested in.</param>
        /// <returns>A ModeratedForumCollection.</returns>
        public  ModeratedForumCollection GetForumsModeratedByUser(String Username) {
            // first determine if this user can moderate all forums
            String strSQL = "SELECT COUNT(*) FROM [Moderators] WHERE [Username] = @Username AND [ForumID] = 0";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);
            OleDbDataReader dr;
            ModeratedForumCollection forums = new ModeratedForumCollection();
            ModeratedForum forum;


            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);

            // Execute the command
            myConnection.Open();
            if ((int) myCommand.ExecuteScalar() > 0) {
                strSQL = "SELECT [EmailNotification], [DateCreated] FROM [Moderators] WHERE [Username] = @Username AND [ForumID] = 0";

                myCommand.CommandText = strSQL;
                myCommand.Parameters.Clear();
                myCommand.Parameters.Add(parameterUsername);

                dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
                dr.Read();

                // user can edit all forums
                forum = new ModeratedForum();

                forum.ForumID = 0;
                forum.ForumName = "All Forums";
                forum.DateCreated = Convert.ToDateTime(dr["DateCreated"]);
                forum.EmailNotification = Convert.ToBoolean(dr["EmailNotification"]);

                forums.Add(forum);

                dr.Close();
            }
            else {
                strSQL = "SELECT M.[ForumID], [EmailNotification], F.[Name] AS [ForumName], M.[DateCreated] FROM [Moderators] AS M " +
                    "INNER JOIN [Forums] AS F ON F.[ForumID] = M.[ForumID] WHERE [Username] = @Username";

                myCommand.CommandText = strSQL;
                myCommand.Parameters.Clear();
                myCommand.Parameters.Add(parameterUsername);

                dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read()) {
                    forum = new ModeratedForum();
                    forum.ForumID = Convert.ToInt32(dr["ForumID"]);
                    forum.ForumName = Convert.ToString(dr["ForumName"]);
                    forum.DateCreated = Convert.ToDateTime(dr["DateCreated"]);
                    forum.EmailNotification = Convert.ToBoolean(dr["EmailNotification"]);

                    forums.Add(forum);
                }
                dr.Close();
            }

            myConnection.Close();
            return forums;
        }


        /// <summary>
        /// Returns a list of forms NOT moderated by the user.
        /// </summary>
        /// <param name="user">The User whose list of non-moderated forums we are interested in
        /// viewing.</param>
        /// <returns>A ModeratedForumCollection containing the list of forums NOT moderated by
        /// the particular user.</returns>
        public ModeratedForumCollection GetForumsNotModeratedByUser(String Username) {
            // see if the user can moderate all forums - if so, then return an empty ModeratedForumCollection
            String strSQL = "SELECT COUNT(*) FROM [Moderators] WHERE [Username] = @Username AND [ForumID] = 0";


            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = Username;
            myCommand.Parameters.Add(parameterUsername);

            // Execute the command
            myConnection.Open();

            if ((int) myCommand.ExecuteScalar() > 0) {
                // can moderate all forums
                return new ModeratedForumCollection();
            }
            else {
                // get the forums the user cannot moderate
                strSQL = "SELECT [ForumID], F.[Name] AS [ForumName] FROM [Forums] AS F " +
                    "WHERE [ForumID] NOT IN (SELECT [ForumID] FROM [Moderators] WHERE [Username] = @Username)";

                myCommand.CommandText = strSQL;
                myCommand.Parameters.Clear();
                myCommand.Parameters.Add(parameterUsername);
                OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                ModeratedForumCollection forums = new ModeratedForumCollection();
                ModeratedForum forum;

                // Add the "All Forums" option
                forum = new ModeratedForum();
                forum.ForumID = 0;
                forum.ForumName = "All Forums";
                forums.Add(forum);


                while (dr.Read()) {
                    forum = new ModeratedForum();
                    forum.ForumID = Convert.ToInt32(dr["ForumID"]);
                    forum.ForumName = Convert.ToString(dr["ForumName"]);

                    forums.Add(forum);
                }
                dr.Close();
                myConnection.Close();

                return forums;
            }
        }


        /// <summary>
        /// Adds a forum to the list of moderatable forums for a particular user.
        /// </summary>
        /// <param name="forum">A ModeratedForum instance that contains the settings for the new user's
        /// moderatable forum.  The Username property indicates the user who can moderate the forum,
        /// the ForumID property indicates what forum the user can moderate, and the
        /// EmailNotification property indicates whether or not the moderator receives email
        /// notification when a new post has been made to the forum.</param>
        public void AddModeratedForumForUser(ModeratedForum forum) {
            String strSQL;

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;
            myConnection.Open();
            
            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = forum.Username;
			
            // if the forum to add is "All Forums," delete all moderated forums first
            if (forum.ForumID == 0) {
                strSQL = "DELETE FROM [Moderators] WHERE [Username] = @Username";

                myCommand.CommandText = strSQL;
                myCommand.Parameters.Add(parameterUsername);

                myCommand.ExecuteNonQuery();
            }

            // now, add the selected forum
            strSQL = "INSERT INTO [Moderators] ([Username], [ForumID], [EmailNotification]) VALUES (@Username, @ForumID, @EmailNotification)";

            myCommand.CommandText = strSQL;
            myCommand.Parameters.Clear();

            myCommand.Parameters.Add(parameterUsername);

            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = forum.ForumID;
            myCommand.Parameters.Add(parameterForumID);

            OleDbParameter parameterEmailNotification = new OleDbParameter("@EmailNotification", OleDbType.Boolean, 1);
            parameterEmailNotification.Value = forum.EmailNotification;
            myCommand.Parameters.Add(parameterEmailNotification);

            // Execute the command			
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }


        /// <summary>
        /// Removes a moderated forum for a particular user.  
        /// </summary>
        /// <param name="forum">A ModeratedForum instance.  The Username and ForumID properties specify
        /// what Forum to remove from what User's list of moderatable forums.</param>
        public  void RemoveModeratedForumForUser(ModeratedForum forum) {
            String strSQL = "DELETE FROM [Moderators] WHERE [Username] = @Username and [ForumID] = @ForumID";
            
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterUsername = new OleDbParameter("@UserName", OleDbType.VarChar, 50);
            parameterUsername.Value = forum.Username;
            myCommand.Parameters.Add(parameterUsername);

            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = forum.ForumID;
            myCommand.Parameters.Add(parameterForumID);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }


        /// <summary>
        /// Returns a list of users who are interested in receiving email notification for a newly
        /// added post.
        /// </summary>
        /// <param name="PostID">The ID of the newly added post.</param>
        /// <returns>A UserCollection instance containing the users who want to be emailed when a new
        /// post is added to the moderation system.</returns>
        public  UserCollection GetModeratorsInterestedInPost(int PostID) {
            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            myConnection.Open();

            // Get the forumID from the Post
            int forumID = ((Post) GetPost(PostID, myConnection)).ForumID;

            String strSQL = "SELECT	U.[Username], [Email] FROM [Users] AS U " +
                "INNER JOIN [Moderators] AS M ON M.[UserName] = U.[UserName] " +
                "WHERE (M.[ForumID] = @ForumID OR M.[ForumID] = 0) AND M.[EmailNotification] = YES";


            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterForumID = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumID.Value = forumID;
            myCommand.Parameters.Add(parameterForumID);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            UserCollection users = new UserCollection();
            User user;
            while (dr.Read()) {
                user = new User();
                user.Username = Convert.ToString(dr["Username"]);
                user.Email = Convert.ToString(dr["Email"]);
                users.Add(user);
            }

            dr.Close();
            myConnection.Close();

            return users;
        }

	

        /// <summary>
        /// Returns a list of the moderators of a particular forum.
        /// </summary>
        /// <param name="ForumID">The ID of the Forum whose moderators we are interested in listing.</param>
        /// <returns>A ModeratedForumCollection containing the moderators of a particular Forum.</returns>
        public  ModeratedForumCollection GetForumModerators(int ForumID) {
            String strSQL = "SELECT [UserName], [EmailNotification], [DateCreated] FROM [Moderators] WHERE [ForumID] = @ForumID OR [ForumID] = 0";

            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterForumId = new OleDbParameter("@ForumID", OleDbType.Integer, 4);
            parameterForumId.Value = ForumID;
            myCommand.Parameters.Add(parameterForumId);

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ModeratedForumCollection forums = new ModeratedForumCollection();
            ModeratedForum forum;
            while (dr.Read()) {
                forum = new ModeratedForum();
                forum.Username = Convert.ToString(dr["Username"]);
                forum.ForumID = ForumID;
                forum.EmailNotification = Convert.ToBoolean(dr["EmailNotification"]);
                forum.DateCreated = Convert.ToDateTime(dr["DateCreated"]);

                forums.Add(forum);
            }

            dr.Close();
            myConnection.Close();

            return forums;
        }
        /*********************************************************************************/




        /*********************************************************************************/

        /**************** SUMMARY FUNCTIONS **************
                 * This function is used to get Summary information about WebForums.NET
                 * *********************************************************/

	
        /// <summary>
        /// Returns a SummaryObject object with summary information about the message board.
        /// </summary>
        /// <returns>A SummaryObject object populated with the summary information.</returns>
        public Statistics GetSiteStatistics() {
            String strSQL = "SELECT (SELECT COUNT(*) FROM [Users]) AS [TotalUsers], " +
                "(SELECT COUNT(*) FROM [Posts] WHERE [PostID] = [ParentID]) AS [TotalTopics]," +
                "(SELECT COUNT(*) FROM [Posts]) AS [TotalPosts]," +
                "(SELECT COUNT(*) FROM [Posts] WHERE [PostID] = [ParentID] AND [PostDate] > @Yesterday) AS [DaysTopics]," +
                "(SELECT COUNT(*) FROM [Posts] WHERE [PostDate] > @Yesterday) AS [DaysPosts] " +
                "FROM Emails";


            // Create Instance of Connection and Command Object
            OleDbConnection myConnection = new OleDbConnection(Globals.DatabaseConnectionString);
            OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // Add Parameters to SPROC
            OleDbParameter parameterYesterday = new OleDbParameter("@Yesterday", OleDbType.Date);
            parameterYesterday.Value = DateTime.Now.AddDays(-1);
            myCommand.Parameters.Add(parameterYesterday);


            Statistics summary = new SummaryObject();

            // Execute the command
            myConnection.Open();
            OleDbDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            dr.Read();

            summary.TotalUsers = Convert.ToInt32(dr["TotalUsers"]);
            summary.TotalPosts = Convert.ToInt32(dr["TotalPosts"]);
            summary.TotalTopics = Convert.ToInt32(dr["TotalTopics"]);
            summary.DaysPosts = Convert.ToInt32(dr["DaysPosts"]);
            summary.DaysTopics = Convert.ToInt32(dr["DaysTopics"]);

            myConnection.Close();

            return summary;
        }
        /*********************************************************************************/
    }
}
