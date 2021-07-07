<%@ Page SmartNavigation="true" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Register TagPrefix="AspNetForumsAdmin" Namespace="AspNetForums.Controls.Admin" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<HTML>
  <HEAD>
    <AspNetForums:StyleSkin runat="server" ID="Styleskin1" />
  </HEAD>
  <body>
    <form runat="server" ID="Form1">
      <AspNetForums:NavigationMenu id="NavigationMenu2" title="ASP.NET Discussion Forum" runat="server" Description="A free discussion system for ASP.NET" />
      <p>
      <AspNetForumsAdmin:AdminPickUser runat="server" ID="Useradmin1" />
      <p />
      <a class="normalTextSmall" href="default.aspx">Return to the Administration page</a>
      <p>
      <AspNetForumsAdmin:OldUserAdmin runat="server" ID="Useradmin2" />
      </p>
    </form>
  </body>
</HTML>
