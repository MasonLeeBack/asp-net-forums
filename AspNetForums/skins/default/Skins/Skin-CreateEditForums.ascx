<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Control Language="C#" %>
<table cellspacing="1" cellpadding="0" width="100%" Class="tableBorder">
  <tr>
    <th height="25" class="tableHeaderText" align="left">
      &nbsp;
      <asp:Label ID="Title" Runat="server" />
    </th>
  </tr>
  <tr>
    <td class="forumRow" align="left">
      <table border="0" cellpadding="3" cellspacing="0">
        <!-- Forum Name -->
        <tr>
          <td class="forumRow" nowrap>
            &nbsp; &nbsp;
          </td>
          <td align="right" nowrap>
            <span class="normalTextSmallBold">
              Forum Name:
            </span>
          </td>
          <td align="left"><asp:TextBox id="ForumName" runat="server" Columns="45"></asp:TextBox>
          </td>
          <td align="left" width="100%">
            <span class="normalTextSmall"></span>
          </td>
        </tr>
        <!-- Forum Group -->
        <tr>
          <td class="forumRow" nowrap>
            &nbsp; &nbsp;
          </td>
          <td align="right" nowrap>
            <span class="normalTextSmallBold">
              Forum
              Group:
            </span>
          </td>
          <td align="left">
            <asp:DropDownList id="ForumGroups" runat="server"></asp:DropDownList>
          </td>
          <td align="left" width="100%">
            <span class="normalTextSmall"></span>
          </td>
        </tr>
        <!-- Forum Description -->
        <tr>
          <td class="forumRow" nowrap>
            &nbsp; &nbsp;
          </td>
          <td align="right" valign="top">
            <span class="normalTextSmallBold"> Description:
            </span>
          </td>
          <td align="left">
            <asp:textbox rows="10" columns="60" TextMode="MultiLine" id="Description" runat="server" MaxLength="3500" />
          </td>
          <td align="left">
          </td>
        </tr>
        <!-- Moderated -->
        <tr>
          <td class="forumRow" nowrap>
            &nbsp; &nbsp;
          </td>
          <td align="right" nowrap>
            <span class="normalTextSmallBold"> Moderated:
            </span>
          </td>
          <td align="left">
            <asp:checkbox id="Moderated" runat="server" Checked="True" />
          </td>
          <td align="left" width="100%">
          </td>
        </tr>
        <!-- Active -->
        <tr>
          <td class="forumRow" nowrap>
            &nbsp; &nbsp;
          </td>
          <td align="right" nowrap>
            <span class="normalTextSmallBold"> Active:
            </span>
          </td>
          <td align="left">
            <asp:checkbox id="Active" runat="server" Checked="True" />
          </td>
          <td align="left" width="100%">
          </td>
        </tr>
        <!-- Button -->
        <tr>
          <td class="forumRow" nowrap>
            &nbsp; &nbsp;
          </td>
          <td align="right" nowrap colspan="2">
            <asp:button type="submit" id="CreateUpdate" runat="server" />
          </td>
          <td align="left" width="100%">
            <span class="normalTextSmall"></span>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>
