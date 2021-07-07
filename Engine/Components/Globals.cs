using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Web.Caching;
using System.Text.RegularExpressions;


namespace AspNetForums.Components {
    /************* DECLARE ENUMERATIONS ****************/
    /// <summary>
    /// The NextPrevMessagesPosition enumeration is used with the ForumView Web control to indicate the position of the
    /// Next/Prev Messages links.  The available options are Top, Bottom, and Both.
    /// </summary>
    public enum NextPrevMessagesPosition { 
        /// <summary>
        /// Places the Next/Prev Messages bar just at the top of the forum post listing.
        /// </summary>
        Top, 
		
        /// <summary>
        /// Places the Next/Prev Messages bar just at the bottom of the forum post listing.
        /// </summary>
        Bottom, 
		
        /// <summary>
        /// Places the Next/Prev Messages bar at the top and the bottom of the forum post listing.
        /// </summary>
        Both,
	
        /// <summary>
        /// Does not display the Next/Prev Messages bar.
        /// </summary>
        None
    }

    /// <summary>
    /// The ViewOptions enumeration determines how the posts for a particular forum are displayed.
    /// The options are NotSet, meaning the default is used; Flat; Mixed; and Threaded.
    /// </summary>
    public enum ViewOptions { 
        /// <summary>
        /// When the forum is visited by an anonymous user, their ViewOptions are NotSet.
        /// Pass this value in to have the default forum view setting used.
        /// </summary>
        NotSet = -1, 
		
        /// <summary>
        /// Specifies to display the forum in a Flat mode.
        /// </summary>
        Flat = 0, 
		
        /// <summary>
        /// Specifies to display the forum in the Mixed mode.
        /// </summary>
        Mixed = 1, 
		
        /// <summary>
        /// Specifies to display the forum in Threaded mode.
        /// </summary>
        Threaded = 2 }

    /// <summary>
    /// The CreateEditPostMode enumeration determines what mode the PostDisplay Web control assumes.
    /// The options are NewPost, ReplyToPost, and EditPost.
    /// </summary>
    public enum CreateEditPostMode { 
        /// <summary>
        /// Specifies that the user is creating a new post.
        /// </summary>
        NewPost, 
		
        /// <summary>
        /// Specifies that the user is replying to an existing post.
        /// </summary>
        ReplyToPost, 
		
        /// <summary>
        /// Specifies that a  moderator or administrator is editing an existing post.
        /// </summary>
        EditPost }

    /// <summary>
    /// The CreateEditForumMode enumeration determines what mode the CreateEditForum Web control assumes.
    /// The options are CreateForum and EditForum.
    /// </summary>
    public enum CreateEditForumMode { 
        /// <summary>
        /// Specifies that a new forum is being created.
        /// </summary>
        CreateForum, 
		
        /// <summary>
        /// Specifies that an existing forum is being edited.
        /// </summary>
        EditForum }

    /// <summary>
    /// The DateTimeFormatEnum enumeration determines the date/time format returned by the AccountForTimezone
    /// functions.  The available options are: ShortTimeString; ShortDateString; LongTimeString;
    /// LongDateString, and CompleteDate.
    /// </summary>
    public enum DateTimeFormatEnum { ShortTimeString, ShortDateString, LongTimeString, LongDateString, CompleteDate }

    /// <summary>
    /// The ModeratedForumMode enumeration determines how the ModeratedForums Web control works.
    /// A value of ViewForForum shows all of the moderators for a particular forum; a value of ViewForUser
    /// shows all of the forums a particular user moderates.
    /// </summary>
    public enum ModeratedForumMode { 
        /// <summary>
        /// Specifies to view the list of moderators for a particular forum.
        /// </summary>
        ViewForForum,
 
        /// <summary>
        /// Specifies to view a list of moderated forums for a particular user.
        /// </summary>
        ViewForUser }

    /// <summary>
    /// The UserInfoEditMode enumeration determines the role the UserInfo Web control assumes.  The
    /// available options are Edit and View.
    /// </summary>
    public enum UserInfoEditMode { 
        /// <summary>
        /// Indicates that the user is editing his or her personal user information.
        /// </summary>
        Edit, 
		
        /// <summary>
        /// Indicates that a user is viewing a user's information (not necessarily his or her own).
        /// </summary>
        View,

        /// <summary>
        /// Indicates that the user is being edited by the admin or moderator.
        /// </summary>
        Admin

    }


    /// <summary>
    /// Indicates how to apply the search query terms.
    /// </summary>
    public enum ToSearchEnum { 
        /// <summary>
        /// Specifies that the PerformSearch method should apply the search query to the post body.
        /// </summary>
        PostsSearch, 
		
        /// <summary>
        /// Specifies that the PerformSearch method should apply the search query to the post's author's 
        /// Username.
        /// </summary>
        PostsBySearch }

    /// <summary>
    /// Indicates the return status for creating a new user.
    /// </summary>
    public enum CreateUserStatus { 
        /// <summary>
        /// The user was not created for some unknown reason.
        /// </summary>
        UnknownFailure, 
		
        /// <summary>
        /// The user's account was successfully created.
        /// </summary>
        Created, 
		
        /// <summary>
        /// The user's account was not created because the user's desired username is already being used.
        /// </summary>
        DuplicateUsername, 
		
        /// <summary>
        /// The user's account was not created because the user's email address is already being used.
        /// </summary>
        DuplicateEmailAddress, 
		
        /// <summary>
        /// The user's account was not created because the user's desired username did not being with an
        /// alphabetic character.
        /// </summary>
        InvalidFirstCharacter }

    /// <summary>
    /// Indicates how to interpret the search terms.
    /// </summary>
    public enum SearchWhatEnum { 
        /// <summary>
        /// Searches for all words entered into the search terms.
        /// </summary>
        SearchAllWords, 
		
        /// <summary>
        /// Searches for any word entered as search terms.
        /// </summary>
        SearchAnyWord, 
		
        /// <summary>
        /// Searches for the EXACT search phrase entered in the search terms.
        /// </summary>
        SearchExactPhrase }

    /// <summary>
    /// Returns the status of a moved post operation.
    /// </summary>
    public enum MovedPostStatus { 
        /// <summary>
        /// The post was not moved; this could happen due to the post having been already deleted or
        /// already approved by another moderator.
        /// </summary>
        NotMoved, 
		
        /// <summary>
        /// The post was moved successfully to the specified forum, but is still waiting approval, since
        /// the moderator who moved the post lacked moderation rights to the forum the post was moved to.
        /// </summary>
        MovedButNotApproved, 
		
        /// <summary>
        /// The post was moved successfully to the specified forum and approved.
        /// </summary>
        MovedAndApproved}


    /// <summary>
    /// The EmailTypeEnum enumeration determines what type of message is to be displayed
    /// </summary>
    public enum Messages {
        UnableToAdminister = 1,
        UnableToEditPost = 2,
        UnableToModerate = 3,
        DuplicatePost = 4,
        FileNotFound = 5,
        UnknownForum = 6,
        NewAccountCreated = 7,
        PostPendingModeration = 8,
        PostDoesNotExist = 9,
        PostIdParameterNotSpecified = 10,
        ProblemPosting = 11,
        UnableToViewMessage = 12,
        UserProfileUpdated = 13,
        UserDoesNotExist = 14,
        UserPasswordChangeSuccess = 15,
        UserPasswordChangeFailed = 16
    }

    /// <summary>
    /// The EmailTypeEnum enumeration determines what type of email template is used to send an email.
    /// The available options are: ForgottenPassword, ChangedPassword, NewMessagePostedToThread,
    /// NewUserAccountCreated, MessageApproved, MessageMovedAndApproved, MessageMovedAndNotApproved,
    /// MessageDeleted, and ModeratorEmailNotification.
    /// </summary>
    public enum EmailTypeEnum {
        /// <summary>
        /// Sends a user their username and password to the email address on file.
        /// </summary>
        ForgottenPassword = 1,

        /// <summary>
        /// Sends an email to the user when he changes his password.
        /// </summary>
        ChangedPassword = 2,

        /// <summary>
        /// Sends a mass emailing when a new post is added to a thread.  Those who receive the email are those
        /// who have email thread tracking turned on for the particular thread that the new post was added to.
        /// </summary>
        NewMessagePostedToThread = 3,

        /// <summary>
        /// When a user creates a new account, this email template sends their UrlShowPost information (username/password).
        /// </summary>
        NewUserAccountCreated = 4,

        /// <summary>
        /// When a user's post that was awaiting moderation is approved, they are sent this email.
        /// </summary>
        MessageApproved = 5,

        /// <summary>
        /// If a user's post is moved from one forum to another, this email indicates this fact.
        /// </summary>
        MessageMovedAndApproved = 6,

        /// <summary>
        /// If a user's post was moved to another forum but is still waiting moderator approval, this
        /// email template informs them of the situation.
        /// </summary>
        MessageMovedAndNotApproved = 7,

        /// <summary>
        /// If a user's post is deleted, this email explains why their post was deleted.
        /// </summary>
        MessageDeleted = 8,

        /// <summary>
        /// When a new post needs to be approved, those moderators of the posted-to forum who have email
        /// notification turned on are sent this email to instruct them that there is a post waiting moderation.
        /// </summary>
        ModeratorEmailNotification = 9
    }
    /***************************************************/

    public class Globals {
	
        // the HTML newline character
        public const String HtmlNewLine = "<br />";
		
        public const String _appSettingsPrefix = "AspNetForumsSettings.";

        // *********************************************************************
        //  LoadSkinnedTemplate
        //
        /// <summary>
        /// Attempts to load a template from the skin defined for the application.
        /// If no template is found, or if an error occurs, a maker is added to the
        /// cache to indicate that we won't try the code path again. Otherwise the
        /// template is added to the cache and loaded from memory.
        /// </summary>
        /// 
        // ********************************************************************/
        public static ITemplate LoadSkinnedTemplate(string virtualPathToTemplate, string templateKey, Page page) {

            ITemplate _template;
            CacheDependency fileDep;
            HttpContext Context = HttpContext.Current;

            // Get the instance of the Cache
            Cache Cache = Context.Cache;

            // Attempt to load header template from Cache
            if ((Cache[virtualPathToTemplate] == null) && (Cache[templateKey] != "TemplateNotFound")) {

                try {

                    // Create a file dependency
                    fileDep = new CacheDependency(page.Server.MapPath(virtualPathToTemplate));

                    // Load the template
                    _template = page.LoadTemplate(virtualPathToTemplate);

                    // Add to cache
                    Cache.Insert(templateKey, _template, fileDep);

                } catch (FileNotFoundException fileNotFound) {

                    // Add a marker we can check for to skip this in the future
                    Cache.Insert(templateKey, "FileNotFound");

                    return null;
                } catch (HttpException httpException) {

                    // Add a marker we can check for to skip this in the future
                    if (httpException.ErrorCode == -2147467259)
                        Cache.Insert(templateKey, "FileNotFound");
                    else
                        throw httpException;

                    return null;
                }


            } else {
                return null;
            }

            // return the template
            return (ITemplate) Cache[templateKey];

        }

        public static string FormatSignature(string userSignature) {
            if (userSignature != String.Empty)
                return "<hr size=\"1\" align=\"left\" width=\"15%\">" + Globals.FormatPostBody(userSignature);

            return null;
        }

        /// <summary>
        /// Converts the raw, database form of a post's body to an HTML-friendly formatted version.
        /// </summary>
        /// <param name="RawPostBody">The raw format of the post body text.</param>
        /// <returns>The formatted version of the raw text.</returns>
        /// <remarks>When a post is saved into a database, it is the raw text that is saved.  (The literal
        /// text entered by the user.)  However, due to the way a browser views certain text characters,
        /// we need to format this text before we display it to the user.  This method removes breaking
        /// HTML characters and applies the text transformations specified by the transformation file.</remarks>
        public static String FormatPostBody(String stringToTransform) {
            return Transforms.TransformString(stringToTransform);
        }


        /// <summary>
        /// Converts a prepared subject line back into a raw text subject line.
        /// </summary>
        /// <param name="FormattedMessageSubject">The prepared subject line.</param>
        /// <returns>A raw text subject line.</returns>
        /// <remarks>This function is only needed when editing an existing message or when replying to
        /// a message - it turns the HTML escaped characters back into their pre-escaped status.</remarks>
        public static String HtmlDecode(String FormattedMessageSubject) {		
            String strSubject = FormattedMessageSubject;
			
		
            // strip the HTML - i.e., turn < into &lt;, > into &gt;
            strSubject = HttpContext.Current.Server.HtmlDecode(strSubject);
			
            return strSubject;
        } 

        /// <summary>
        /// Converts a prepared subject line back into a raw text subject line.
        /// </summary>
        /// <param name="FormattedMessageSubject">The prepared subject line.</param>
        /// <returns>A raw text subject line.</returns>
        /// <remarks>This function is only needed when editing an existing message or when replying to
        /// a message - it turns the HTML escaped characters back into their pre-escaped status.</remarks>
        public static String HtmlEncode(String FormattedMessageSubject) {		
            String strSubject = FormattedMessageSubject;
		
            // strip the HTML - i.e., turn < into &lt;, > into &gt;
            strSubject = HttpContext.Current.Server.HtmlEncode(strSubject);
			
            return strSubject;
        } 

        /************ PROPERTY SET/GET STATEMENTS **************/
        /// <summary>
        /// Returns the default view to use for viewing the forum posts, as specified in the AspNetForumsSettings
        /// section of Web.config.
        /// </summary>
        static public int DefaultForumView {
            get {
                const int _defaultForumView = 2;
                const String _settingName = "defaultForumView";

                String _str = (String) HttpContext.Current.Cache.Get("webForums." + _settingName);
                int iValue = _defaultForumView;
                if (_str == null) {
                    // we need to get the string and place it in the cache
                    String prefix = "";
                    NameValueCollection context = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                    if (context == null) {
                        // get the appSettings context
                        prefix = Globals._appSettingsPrefix;;
                        context = (NameValueCollection)ConfigurationSettings.GetConfig("appSettings");
                    }

                    _str = context[prefix + _settingName];

                    // determine what forum view to show
                    if (_str == null)
                        // choose the default
                        iValue = _defaultForumView;
                    else
                        switch(_str.ToUpper()) {
                            case "THREADED":
                                iValue = 2;
                                break;

                            case "MIXED":
                                iValue = 1;
                                break;

                            case "FLAT":
                                iValue = 0;
                                break;

                            default:
                                iValue = _defaultForumView;
                                break;
                        }
					
                    _str = iValue.ToString();
                    HttpContext.Current.Cache.Insert("webForums." + _settingName, _str);
                }

                return Convert.ToInt32(_str);
            }
        }


        /// <summary>
        /// Returns a boolean value indicating whether or not duplicate posts are allowed on the forum.
        /// </summary>
        static public bool AllowDuplicatePosts {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return Convert.ToBoolean(configSettings["allowDuplicatePosts"]);
            }
        }

        /// <summary>
        /// Specifies the SMTP Mail Server to use to send email information.  If no value is specified, or
        /// a value of "default" is specified, the default SMTP Mail Server is used.
        /// </summary>
        static public String SmtpServer {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                string smtpServer = configSettings["smtpServer"];

                if (smtpServer.Length == 0 || smtpServer.ToUpper() == "DEFAULT")
                    smtpServer = "";

                return smtpServer;
            }
        }

        /// <summary>
        /// Returns the Url to view a User's information
        /// </summary>
        static public String WebSiteUrl {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["webSiteUrl"];
            }
        }

        /// <summary>
        /// A read-only property that determines if WebForums.NET should send automated email messages
        /// or not.  Simply, this property returns false if the smtpServer property is set to NONE, true
        /// otherwise.
        /// </summary>
        static public bool SendEmail {
            get { return SmtpServer.ToUpper() != "NONE"; }
        }
		
        /// <summary>
        /// Url path to the page implementing search features
        /// </summary>
        static public String UrlSearch {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlSearch"];
            }
        }

        /// <summary>
        /// Url path to the page used for new user registration
        /// </summary>
        static public String UrlRegister {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlRegister"];
            }
        }
        
        /// <summary>
        /// Url path to the user profile page
        /// </summary>
        static public String UrlEditUserProfile {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlEditUserProfile"];
            }
        }

        /// <summary>
        /// Name of the skin to be applied
        /// </summary>
        static public String Skin {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["Skin"];
            }
        }

        /// <summary>
        /// Name of the default style to be applied
        /// </summary>
        static public String SiteStyle {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["Style"];
            }
        }

        /// <summary>
        /// Available styles that can be used
        /// </summary>
        static public String[] SiteStyles {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["AvailableStyles"].Split(';');
            }
        }

        /// <summary>
        /// TODO: Necessary?
        /// </summary>
        static public String ApplicationVRoot {
            get {
                NameValueCollection configSettings = (NameValueCollection)ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["ApplicationRoot"];
            }
        }
        
        /// <summary>
        /// Returns the database connection string
        /// </summary>
        static public String DatabaseConnectionString {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["connectionString"];
            }
        }

        /// <summary>
        /// Returns the default page size used in paging
        /// </summary>
        static public int PageSize {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return Convert.ToInt32(configSettings["pageSize"]);
            }
        }

        /// <summary>
        /// Default date format
        /// </summary>
        static public string DateFormat {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["defaultDateFormat"];
            }
        }

        /// <summary>
        /// Default time format
        /// </summary>
        static public string TimeFormat {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["defaultTimeFormat"];
            }
        }

        /// <summary>
        /// Returns the path to the images directory
        /// </summary>
        static public String ImagePath {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["imagesPath"];
            }
        }
		
        /// <summary>
        /// Returns the offset of the timezone of the database server
        /// </summary>
        static public int DBTimezone {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return Convert.ToInt32(configSettings["dbTimeZoneOffset"]);
            }
        }
		
        /// <summary>
        /// Returns the Url to view a User's information
        /// </summary>
        static public String UrlUserProfile {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlShowUserProfile"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to the home of the forums app
        /// </summary>
        static public String UrlHome {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlHome"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to my forums
        /// </summary>
        static public String UrlMyForums {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlMyForums"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to change the user's password
        /// </summary>
        static public String UrlChangePassword {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlChangePassword"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to change the user's password
        /// </summary>
        static public String UrlForgotPassword {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlForgotPassword"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to search for all posts by a given user
        /// </summary>
        static public String UrlSearchForPostsByUser {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlSearchForUser"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to view all users
        /// </summary>
        static public String UrlShowAllUsers {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlShowAllUsers"];
            }
        }

        
		
        /// <summary>
        /// Returns the Url to edit an existing post from the post moderation page
        /// </summary>
        static public String UrlEditPost {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlEditPost"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to edit an existing post from the post moderation page
        /// </summary>
        static public String UrlUserEditPost {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlUserEditPost"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to edit an existing post from the post moderation page
        /// </summary>
        static public String UrlDeletePost {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlDeletePost"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to edit an existing post from the post moderation page
        /// </summary>
        static public String UrlMovePost {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlMovePost"].Replace("^", "&");
            }
        }

        static public String UrlModerateForumPosts {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlModerateForumPosts"].Replace("^", "&");
            }
        }

        static public String UrlModerateThread {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlModerateThread"].Replace("^", "&");
            }
        }

        static public String UrlManageForumPosts {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlManageForumPosts"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to edit an existing post from the administration page
        /// </summary>
        static public String UrlEditPostFromAdmin {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlEditExistingPostFromAdmin"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to login to the forum site
        /// </summary>
        static public String UrlLogin {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlLogin"];
            }
        }

        /// <summary>
        /// Returns the Url to login to the forum site
        /// </summary>
        static public String UrlLogout {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlLogout"];
            }
        }

        /// <summary>
        /// Returns the Url to show a particular forum
        /// </summary>
        static public String UrlShowForum {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlShowForum"].Replace("^", "&");
            }
        }

        /// <summary>
        /// <summary>   
        /// Returns the Url to show a particular forum group
        /// </summary>
        static public String UrlShowForumGroup {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlShowForumGroup"].Replace("^", "&");
            }
        }

        /// Returns the Url to show a particular post
        /// </summary>
        static public String UrlShowPost {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlShowPost"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Returns the Url to the post moderation page
        /// </summary>
        static public String UrlModeration {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlModeration"].Replace("^", "&");
            }

        }

        /// <summary>
        /// Returns the path to the location of the various message Web pages.  The 
        /// message pages are pages that are automatically shown at certain events, such
        /// as when a user posts a message to a moderated forum, or when a user attempts 
        /// to view a post that doesn't exist.
        /// </summary>
        static public String UrlMessage {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlMessage"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Specifies the Url to reply to a post
        /// </summary>
        static public String UrlReplyToPost {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlReplyToPost"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Indicates the Url to add a new post
        /// </summary>
        static public String UrlAddNewPost {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlAddNewPost"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Indicates the Url to edit a forum
        /// </summary>
        static public String UrlEditForum {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlEditForum"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Indicates the Url to create a new forum
        /// </summary>
        static public String UrlCreateForum {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlCreateForum"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Indicates the Url to show a forum's posts for editing and deleting purposes
        /// </summary>
        static public String UrlShowForumPostsForAdmin {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlShowForumPostsForAdmin"].Replace("^", "&");
            }
        }

        /// <summary>
        /// The Url to use for Admin
        /// </summary>
        static public String UrlAdmin {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlAdmin"].Replace("^", "&");
            }
        }

        /// <summary>
        /// The Url to use for Admins to edit users
        /// </summary>
        static public String UrlAdminEditUser {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["urlAdminEditUser"].Replace("^", "&");
            }
        }

        /// <summary>
        /// Indicates the name of the Web site.
        /// </summary>
        static public String SiteName {
            get {
                NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                return configSettings["siteName"];
            }
        }

        /// <summary>
        /// Indicates the physical path to the transformation text file.
        /// </summary>
        static public String PhysicalPathToTransformationFile {
            get {

                if (HttpContext.Current.Cache["pathToTransformationFile"] == null) {
                    string path;
                    NameValueCollection configSettings = (NameValueCollection) ConfigurationSettings.GetConfig("AspNetForumsSettings");
                    path = configSettings["pathToTransformationFile"];

                    if (path.Substring(0,1) == "/")
                        // using virtual path, must convert to physical path
                        path = HttpContext.Current.Server.MapPath(path);

                    HttpContext.Current.Cache.Insert("pathToTransformationFile", path);

                    return path;
                } else {
                    return (string) HttpContext.Current.Cache["pathToTransformationFile"];
                }

            }
        }

        /// <summary>
        /// Creates a temporary password of a specified length.
        /// </summary>
        /// <param name="length">The maximum length of the temporary password to create.</param>
        /// <returns>A temporary password less than or equal to the length specified.</returns>
        public static String CreateTemporaryPassword(int length) {
            // begin by creating a random password
            // start off by getting a temp filename
            String strTempPassword = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + DateTime.Now.Millisecond.ToString();
			
            // jumble up the password
            Random rnd = new Random();
			
            const int RandomLoops = 3;
            int iRandNumber = 0;
            for (int iLoop=0; iLoop < RandomLoops; iLoop++) {
                iRandNumber = rnd.Next(strTempPassword.Length-3);				
                if (iLoop % 2 == 0)
                    strTempPassword += strTempPassword.Substring(iRandNumber, rnd.Next(iRandNumber, strTempPassword.Length-1) -iRandNumber);
                else
                    strTempPassword = strTempPassword.Substring(iRandNumber, rnd.Next(iRandNumber, strTempPassword.Length-1) - iRandNumber) + strTempPassword;
            }
			
            // make sure the password is only 10 characters long, at most
            if (strTempPassword.Length > length)
                strTempPassword = strTempPassword.Substring(0, length);

            return strTempPassword;
        }
    }

}
