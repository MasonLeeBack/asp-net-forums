using System;
using System.Web;
using AspNetForums.Components;
using System.Web.UI.WebControls;

namespace AspNetForums {

    // *********************************************************************
    //  Forums
    //
    /// <summary>
    /// This class contains methods for working with the Forums.
    /// </summary>
    /// 
    // ********************************************************************/ 
    public class Forums {

        // *********************************************************************
        //  GetAllButOneForum
        //
        /// <summary>
        /// This method returns a list of all of the forums EXCEPT for the forum
        /// the passed in PostID is from.  This is useful when listing the forums
        /// to move a post awaiting moderation to.
        /// </summary>
        /// <param name="PostID">The Post that belongs to the Forum that we DO NOT want to return.</param>
        /// <returns>A ForumCollection with all of the active forums except for the one specified.</returns>
        /// <remarks>This method is called from the Moderation page, where a post can be moved from
        /// one forum to another.  This method populates the listbox of forums to move the post to: in
        /// essence we want to let the user move the post from its current forum to forum BUT the
        /// forum it exists in.</remarks>
        /// 
        // ********************************************************************/ 
        // TODO: REMOVE?
        /*
        public static ForumCollection GetAllButOneForum(int PostID) {
            ForumCollection forums;

            IWebForumsDataProviderBase dp = DataProvider.Instance();

            forums = dp.GetAllButOneForum(PostID);

            forums.Sort();

            return forums;
        }
        */


        // *********************************************************************
        //  MarkAllThreadsRead
        //
        /// <summary>
        /// Marks all threads in the current forum as read
        /// </summary>
        /// <param name="forumID">Forum to mark threads read for</param>
        /// <param name="username">Username to mark threads read for</param>
        /// 
        // ********************************************************************/ 
        public static void MarkAllThreadsRead(int forumID, string username) {
            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            dp.MarkAllThreadsRead(forumID, username);
        }

        // *********************************************************************
        //  GetAllForums
        //
        /// <summary>
        /// Returns all of the active forums in the database.
        /// </summary>
        /// 
        // ********************************************************************/ 
        public static ForumCollection GetAllForums() {
            return GetAllForums(false, null);
        }

        // *********************************************************************
        //  PopulateForumList
        //
        /// <summary>
        /// A listing of forums and forum groups.
        /// </summary>
        /// <param name="username">Username making the request</param>
        /// <param name="listStyle">How the list is to be formatted</param>
        /// 
        // ***********************************************************************/
        public ListItemCollection ForumListItemCollection(string username, ForumListStyle listStyle) {
            return ForumListItemCollection(username, listStyle, null);
        }

        // *********************************************************************
        //  PopulateForumList
        //
        /// <summary>
        /// A listing of forums and forum groups.
        /// </summary>
        /// <param name="username">Username making the request</param>
        /// <param name="listStyle">How the list is to be formatted</param>
        /// 
        // ***********************************************************************/
        public ListItemCollection ForumListItemCollection(string username, ForumListStyle listStyle, ListItemCollection listItems) {

            // Only do this once per request
            if (HttpContext.Current.Items["Moderation-ForumList"] == null) {
                if (listItems == null)
                    listItems = new ListItemCollection();

                ForumGroupCollection forumGroups;
                ForumCollection forumCollection;
                Forums forums = new Forums();

                // Get all forum groups
                forumGroups = ForumGroups.GetAllForumGroups(false, true);
                forumGroups.Sort();

                // Walk through forum groups
                foreach (ForumGroup group in forumGroups) {
                    listItems.Add(new ListItem(group.Name, "g-" + group.ForumGroupID));

                    // Now walk though each forum in the current group
                    forumCollection = forums.GetForumsByForumGroupId(group.ForumGroupID, username);

                    forumCollection.Sort();

                    foreach (Forum forum in forumCollection) {
                        listItems.Add(new ListItem("---" + forum.Name, "f-" + forum.ForumID.ToString()));
                    }
                }

                HttpContext.Current.Items["Moderation-ForumList"] = listItems;

                return listItems;
            } else {
                return (ListItemCollection) HttpContext.Current.Items["Moderation-ForumList"];
            }
        }

        // *********************************************************************
        //  GetAllForums
        //
        /// <summary>
        /// Returns all of the forums in the database.
        /// </summary>
        /// <param name="ShowAllForums">If ShowAllForums is true, ALL forums, active and nonactive,
        /// are returned.  If ShowAllForums is false, just the active forums are returned.</param>
        /// <returns>A ForumCollection with all of the active forums, or all of the active and nonactive
        /// forums, depending on the value of the ShowAllForums property.</returns>
        /// 
        // ***********************************************************************/
        public static ForumCollection GetAllForums(bool showAllForums, string username) {
            ForumCollection forums = null;

            // If the user is anonymous we'll take some load off the database
            if (username == null) {
                if (HttpContext.Current.Cache["ForumCollection-AllForums-Anonymous"] != null)
                    return (ForumCollection) HttpContext.Current.Cache["ForumCollection-AllForums-Anonymous"];
            }

            // Optimize this method to ensure we only ask for the forums once per request
            if (HttpContext.Current.Items["ForumCollection" + showAllForums + username] ==  null) {
                // Create Instance of the IWebForumsDataProviderBase
                IWebForumsDataProviderBase dp = DataProvider.Instance();

                forums = dp.GetAllForums(showAllForums, username);

                // If we have a user add the results to the items collection else add to cache
                if (username == null)
                    HttpContext.Current.Cache.Insert("ForumCollection-AllForums-Anonymous", forums, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero);
                else
                    HttpContext.Current.Items.Add("ForumCollection" + showAllForums + username, forums);

                return forums;
            } else {
                forums = (ForumCollection) HttpContext.Current.Items["ForumCollection" + showAllForums + username];
            }

            return forums;
        }

        
        // *********************************************************************
        //  GetForumsByForumGroupId
        //
        /// <summary>
        /// Used to return a narrow collection of forums that belong to a given forum id.
        /// The username is provied for personalization, e.g. if the user has new
        /// posts in the forum
        /// </summary>
        /// <param name="forumGroupId">Forum Group ID to retrieve forums for</param>
        /// <param name="username">Username making the request</param>
        /// <param name="showAll">Show forums marked as inactive?</param>
        /// 
        // ***********************************************************************/
        public ForumCollection GetForumsByForumGroupId(int forumGroupId, string username, bool showAll) {
            ForumCollection allForums;
            ForumCollection forumsBelongingToGroup = new ForumCollection();

            // First get all the forums
            allForums = GetAllForums(showAll, username);

            // Sort the forums
            allForums.Sort();

            // Find all the forums that belong to the requested forumGroupId
            foreach (Forum f in allForums) {

                if (f.ForumGroupId == forumGroupId)
                    forumsBelongingToGroup.Add(f);

            }

            forumsBelongingToGroup.Sort();

            return forumsBelongingToGroup;
        }


        // *********************************************************************
        //  GetForumsByForumGroupId
        //
        /// <summary>
        /// Used to return a narrow collection of forums that belong to a given forum id.
        /// The username is provied for personalization, e.g. if the user has new
        /// posts in the forum
        /// </summary>
        /// <param name="forumGroupId">Forum Group ID to retrieve forums for</param>
        /// <param name="username">Username making the request</param>
        /// 
        // ***********************************************************************/
        public ForumCollection GetForumsByForumGroupId(int forumGroupId, string username) {
            return GetForumsByForumGroupId(forumGroupId, username, false);
        }

        // *********************************************************************
        //  GetForumInfo
        //
        /// <summary>
        /// Returns information on a particular forum.
        /// </summary>
        /// <param name="ForumID">The ID of the Forum that you are interested in.</param>
        /// <returns>A Forum object with information about the specified forum.</returns>
        /// <remarks>If the passed in ForumID is not found, a ForumNotFoundException exception is
        /// thrown.</remarks>
        /// 
        // ***********************************************************************/
        public static Forum GetForumInfo(int ForumID) {

            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            //return dp.GetForumInfoByPostID(PostID);
            return dp.GetForumInfo(ForumID, HttpContext.Current.User.Identity.Name);
        }

        // *********************************************************************
        //  GetTotalThreadsInForum
        //
        /// <summary>
        /// Used in paging to return a count of the forums based on the query
        /// </summary>
        /// <param name="forumID">id of the forum</param>
        /// <param name="maxDateTime">Datetime filter</param>
        /// <param name="minDateTime">Datetime filter</param>
        /// <param name="unreadThreadsOnly">Display threads that the user has not read</param>
        /// <param name="username">User making the request</param>
        /// 
        // ***********************************************************************/
        public static int GetTotalThreadsInForum(int forumID, DateTime maxDateTime, DateTime minDateTime, string username, bool unreadThreadsOnly) {
            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            return dp.GetTotalThreadsInForum(forumID, maxDateTime, minDateTime, username, unreadThreadsOnly);

        }
        
        // *********************************************************************
        //  GetForumInfoByPostID
        //
        /// <summary>
        /// Returns information about the forum that a particular post exists in.
        /// </summary>
        /// <param name="PostID">The ID of the Post that exists in the forum you're interested in.</param>
        /// <returns>A Forum object containing information about the forum the Post exists in.</returns>
        /// <remarks>If the post is not found or does not contain a valid ForumID, a 
        /// ForumNotFoundException is thrown.</remarks>
        /// 
        // ***********************************************************************/
        public static Forum GetForumInfoByPostID(int PostID) {
            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            return dp.GetForumInfoByPostID(PostID);			
        }


        // *********************************************************************
        //  GetForumInfoByThreadID
        //
        /// <summary>
        /// Returns information about the forum that a particular thread exists in.
        /// </summary>
        /// <param name="ThreadID">The ID of the Thread that exists in the forum you're interested in.</param>
        /// <returns>A Forum object containing information about the forum the Thread exists in.</returns>
        /// <remarks>If the thread is not found or does not contain a valid ForumID, a 
        /// ForumNotFoundException is thrown.</remarks>
        /// 
        // ***********************************************************************/
        public static Forum GetForumInfoByThreadID(int ThreadID) {
            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            return dp.GetForumInfoByThreadID(ThreadID);
        }


        // *********************************************************************
        //  DeleteForum
        //
        /// <summary>
        /// Deletes a forum and all of the posts in the forum.
        /// </summary>
        /// <param name="ForumID">The ID of the forum to delete.</param>
        /// <remarks>Be very careful when using this method.  The specified forum and ALL of its posts
        /// will be deleted.</remarks>
        /// 
        // ***********************************************************************/
        public static void DeleteForum(int forumID) {
            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            dp.DeleteForum(forumID);
        }


        // *********************************************************************
        //  AddForum
        //
        /// <summary>
        /// Creates a new forum.
        /// </summary>
        /// <param name="forum">Specifies information about the forum to create.</param>
        /// 
        // ***********************************************************************/
        public static void AddForum(Forum forum) {
            // turn the description into a formatted version
            forum.Description = ForumDescriptionFormattedToRaw(forum.Description);

            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            dp.AddForum(forum);
        }


        // *********************************************************************
        //  UpdateForum
        //
        /// <summary>
        /// Updates a particular forum.
        /// </summary>
        /// <param name="forum">Specifies information about the forum to update.  The ForumID property
        /// indicates what forum it is that you wish to update.</param>
        /// 
        // ***********************************************************************/
        public static void UpdateForum(Forum forum) {
            // turn the description into a formatted version
            forum.Description = ForumDescriptionFormattedToRaw(forum.Description);

            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            dp.UpdateForum(forum);
        }

        // *********************************************************************
        //  ForumDescriptionFormattedToRaw
        //
        /// <summary>
        /// Converts the forum description from formatted text to raw text.
        /// </summary>
        /// <param name="Description">The formatted text forum description</param>
        /// <returns>A raw text version of the forum's description.</returns>
        /// <remarks>This function merely converts HTML newline tags to carraige returns.
        /// This method only needs to be called editing an existing forum.
        /// 
        // ***********************************************************************/
        public static String ForumDescriptionFormattedToRaw(String Description) {
            // replace new line characters with \n
            return Description.Replace(Globals.HtmlNewLine, "\n");
        }


        // *********************************************************************
        //  TotalNumberOfForums
        //
        /// <summary>
        /// Returns the total number of forums that exist.
        /// </summary>
        /// <returns>The total number of forums.</returns>
        /// 
        // ***********************************************************************/
        public static int TotalNumberOfForums() {
            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            return dp.TotalNumberOfForums();
        }

        // *********************************************************************
        //  ForumListStyle
        //
        /// <summary>
        /// Used for displaying the forum group/forum relationship in dropdownlists
        /// </summary>
        /// 
        // ***********************************************************************/
        public enum ForumListStyle {
            Flat,
            Nested
        }
    }
}
