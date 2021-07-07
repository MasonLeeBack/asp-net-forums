using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Security.Principal;
using AspNetForums.Components;

namespace AspNetForums {

    // *********************************************************************
    //  UserRoles
    //
    /// <summary>
    /// The user roles class is used to manage user to role mappings.
    /// </summary>
    // ***********************************************************************/
    public class UserRoles {
        const string rolesCookie = "AspNetForumsRoles";
        string[] roles;

        // *********************************************************************
        //  GetUserRoles
        //
        /// <summary>
        /// Connects to the user role's datasource, retrieves all the roles a given
        /// user belongs to, and add them to the curret IPrincipal. The roles are retrieved
        /// from the datasource or from an encrypted cookie.
        /// </summary>
        // ***********************************************************************/
        public void GetUserRoles() {
            HttpContext Context = HttpContext.Current;
            string[] userRoles = null;
            string formattedUserRoles;

            // Is the request authenticated?
            if (!Context.Request.IsAuthenticated)
                return;

            // Get the roles this user is in
            if ((Context.Request.Cookies[rolesCookie] == null) || (Context.Request.Cookies[rolesCookie].Value == "")) {

                userRoles = UserRoles.GetUserRoles(Context.User.Identity.Name);

                // Format string array
                formattedUserRoles = "";
                foreach (string role in userRoles) {
                    formattedUserRoles += role;
                    formattedUserRoles += ";";
                }

                // Create authentication ticket
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                              // version
                    Context.User.Identity.Name,     // user name
                    DateTime.Now,                   // issue time
                    DateTime.Now.AddHours(1),       // expires every hour
                    false,                          // don't persist cookie
                    formattedUserRoles              // roles
                    );

                // Encrypt the ticket
                String cookieStr = FormsAuthentication.Encrypt(ticket);

                // Send the cookie to the client
                Context.Response.Cookies[rolesCookie].Value = cookieStr;
                Context.Response.Cookies[rolesCookie].Path = Globals.ApplicationVRoot;
                Context.Response.Cookies[rolesCookie].Expires = DateTime.Now.AddMinutes(5);
            } else {
                // Get roles from roles cookie
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Context.Request.Cookies[rolesCookie].Value);

                //convert the string representation of the role data into a string array
                ArrayList rolesArrayList = new ArrayList();

                foreach (String role in ticket.UserData.Split( new char[] {';'} )) {

                    if (role != "")
                        rolesArrayList.Add(role);
                }

                userRoles = (String[]) rolesArrayList.ToArray(typeof(String));
            }

            // Add our own custom principal to the request containing the roles in the auth ticket
            Context.User = new GenericPrincipal(Context.User.Identity, userRoles);
        }

        // *********************************************************************
        //  GetUserRoles
        //
        /// <summary>
        /// All the roles that the named user belongs to
        /// </summary>
        /// <param name="username">Name of user to retrieve roles for</param>
        /// <returns>String array of roles</returns>
        // ***********************************************************************/
        public static String[] GetUserRoles(string username) {
            // Create Instance of the IWebForumsDataProviderBase
            IWebForumsDataProviderBase dp = DataProvider.Instance();

            return dp.GetUserRoles(username);
        }

        // *********************************************************************
        //  SignOut
        //
        /// <summary>
        /// Cleans up cookies used for role management when the user signs out.
        /// </summary>
        // ***********************************************************************/
        public static void SignOut() {
            HttpContext Context = HttpContext.Current;

            // Invalidate roles token
            Context.Response.Cookies[rolesCookie].Value = null;
            Context.Response.Cookies[rolesCookie].Expires = new System.DateTime(1999, 10, 12);
            Context.Response.Cookies[rolesCookie].Path = Globals.ApplicationVRoot;
        }

    }
}
