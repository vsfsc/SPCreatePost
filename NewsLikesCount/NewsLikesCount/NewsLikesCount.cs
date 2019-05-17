using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.ReputationModel;
using System.Text;
using System.Collections.Generic;
namespace NewsLikesCount.NewsLikesCount
{
    [ToolboxItemAttribute(false)]
    public class NewsLikesCount : WebPart
    {
        #region 方法和事件
        private void AddStyle()
        {
            StringBuilder txt = new StringBuilder();
            txt.AppendLine("<style type=\"text/css\" >");
            txt.AppendLine(".likecount {");
            txt.AppendLine("margin-left:6px;margin-right:10px;");
            txt.AppendLine("}");

            txt.AppendLine(".LikeSection {");
            txt.AppendLine("margin-left:15px;margin-bottom:15px;");
            txt.AppendLine("}");
            txt.AppendLine("</style>");
            Page.Header.Controls.Add(new LiteralControl(txt.ToString()));
        }
        private void AddJs()
        {
            StringBuilder txt = new StringBuilder();

            txt.AppendLine("<SharePoint:ScriptLink ID=\"ScriptLink6\" name=\"SP.js\" runat=\"server\" ondemand=\"false\" localizable=\"false\" loadafterui=\"true\" />");
            txt.AppendLine("<SharePoint:ScriptLink ID=\"ScriptLink8\" name=\"SP.Core.js\" runat=\"server\" ondemand=\"false\" localizable=\"false\" loadafterui=\"true\" />");
            txt.AppendLine("<SharePoint:ScriptLink ID=\"ScriptLink9\" name=\"Reputation.js\" runat=\"server\" ondemand=\"false\" localizable=\"false\" loadafterui=\"true\" />");
            txt.AppendLine("<script src=\"/_layouts/15/NewsLikesCount/Like.js\" type=\"text/javascript\"></script>");
            Page.Header.Controls.Add(new LiteralControl(txt.ToString()));
        }
        protected override void CreateChildControls()
        {
            SPUser loginUser = SPContext.Current.Web.CurrentUser;
            if (loginUser != null)
            {
                AddStyle();
                GetLikeCount();
            }
        }
        #endregion
        #region like/unlike
        private void GetLikeCount()
        {
            if (Page.Request.QueryString["ID"] == null) return;
            int listID = int.Parse(Page.Request.QueryString["ID"]);
            try
            {
                string listUrl = Page.Request.FilePath; ;
                SPList myNotice = SPContext.Current.Web.GetList(listUrl);
                ViewState["ListName"] = myNotice.Title;
                SPListItem myItem = myNotice.GetItemById(listID);
                SPFieldUserValueCollection users = myItem["LikedBy"] as SPFieldUserValueCollection;
                Panel divCount = new Panel();
                divCount.ID = "divShow";
                Table tbl = new Table();
                tbl.ID = "tblShow";
                TableRow tRow = new TableRow();
                TableCell tCell = new TableCell();

                LinkButton btn = new LinkButton();
                btn.ID = "btnLike";
                btn.Text = "赞";
                btn.CssClass = "LikeSection";
                btn.CommandArgument = listID.ToString();
                btn.Command += btn_Command;
                tCell.Controls.Add(divCount);
                tRow.Cells.Add(tCell);
                tCell = new TableCell();
                tRow.Cells.Add(tCell);
                tCell.Controls.Add(btn);
                tbl.Rows.Add(tRow);
                UpdatePanel updatePanel;
                updatePanel = new UpdatePanel();

                updatePanel.ID = "upUpdatePanel";
                updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
                updatePanel.ChildrenAsTriggers = false;
                AsyncPostBackTrigger t = new AsyncPostBackTrigger();
                t.EventName = "Command";
                t.ControlID = btn.ID;
                updatePanel.Triggers.Add(t);

                updatePanel.ContentTemplateContainer.Controls.Add(tbl);
                Controls.Add(updatePanel);
                if (users != null)
                {
                    SPUser loginUser = SPContext.Current.Web.CurrentUser;
                    StringBuilder txtLikers = new StringBuilder();
                    bool userIsLike = false;
                    foreach (SPFieldUserValue user in users)
                    {
                        if (user.LookupId == loginUser.ID)
                            userIsLike = true;
                        txtLikers.AppendLine(user.User.Name);
                    }
                    Label lbl = new Label();
                    lbl.ID = "lblCount";
                    lbl.Text = myItem["LikesCount"].ToString();
                    lbl.ToolTip = txtLikers.ToString().Trim();
                    string txt = "<img alt='' src='/_layouts/15/images/LikeFull.11x11x32.png' /><span class=\"likecount\">";
                    divCount.Controls.Add(new LiteralControl(txt));
                    divCount.Controls.Add(lbl);
                    txt = "</span>";
                    divCount.Controls.Add(new LiteralControl(txt));

                    if (userIsLike)
                    {
                        btn.Text = "取消赞";
                    }
                }
            }
            catch (Exception ex)
            {
                this.Controls.Add(new LiteralControl(ex.ToString()));
            }
        }

        void btn_Command(object sender, CommandEventArgs e)
        {
            Page.Title = ViewState["ListName"].ToString();
            LinkButton lnkButton = sender as LinkButton;
            Control ctl = sender as Control;
            UpdatePanel up = ctl.NamingContainer.FindControl("upUpdatePanel") as UpdatePanel;
            Table  tbl = up.ContentTemplateContainer.FindControl("tblShow") as Table;
            Panel divCount = tbl.Rows[0].Cells[0].Controls[0]  as Panel ;  
            SwitchButton(lnkButton, divCount);
        }

        private void SwitchButton(LinkButton lnkButton, Panel divCount)
        {
            SPUser loginUser = SPContext.Current.Web.CurrentUser;

            if (lnkButton.Text == "赞")
            {
                SaveLikeUnLike(int.Parse(lnkButton.CommandArgument), 1);
                lnkButton.Text = "取消赞";
                if (divCount.Controls.Count == 0)
                {
                    Label lbl = new Label();
                    lbl.ID = "lblCount";
                    lbl.Text = "1";
                    lbl.ToolTip = loginUser.Name;
                    string txt = "<img alt='' src='/_layouts/15/images/LikeFull.11x11x32.png' /><span class=\"likecount\">";
                    divCount.Controls.AddAt(0, new LiteralControl(txt));
                    divCount.Controls.AddAt(1, lbl);
                    txt = "</span>";
                    divCount.Controls.AddAt(2, new LiteralControl(txt));
                }
                else
                {
                    Label lbl = (Label)divCount.Controls[1];
                    lbl.Text = (int.Parse(lbl.Text) + 1).ToString();
                    lbl.ToolTip = lbl.ToolTip + "\r\n" + loginUser.Name;
                }
            }
            else
            {
                SaveLikeUnLike(int.Parse(lnkButton.CommandArgument), -1);
                lnkButton.Text = "赞";
                Label lbl = (Label)divCount.Controls[1];
                int lCount = int.Parse(lbl.Text) - 1;
                if (lCount > 0)
                {
                    lbl.Text = lCount.ToString();
                    lbl.ToolTip = lbl.ToolTip.Replace(loginUser.Name + "\r\n", "");
                    lbl.ToolTip = lbl.ToolTip.Replace(loginUser.Name, "");
                }
                else
                {
                    divCount.Controls.Clear();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="likeCount">1/-1点赞和取消赞</param>
        private void SaveLikeUnLike(int itemID, int likeCount)
        {
            SPUser loginUser = SPContext.Current.Web.CurrentUser;
            Guid webID = SPContext.Current.Web.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite mySite = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb thisWeb = mySite.AllWebs[webID])
                    {
                        thisWeb.AllowUnsafeUpdates = true;
                        try
                        {
                            string listUrl = Page.Request.FilePath; ;
                            SPList list = thisWeb.GetList(listUrl);
                            SPListItem lstItem = list.GetItemById(itemID);
                            SPFieldUserValueCollection users = lstItem["LikedBy"] as SPFieldUserValueCollection;
                            SPFieldUserValue userValue = new SPFieldUserValue(thisWeb,loginUser.ID,loginUser.Name);
                            if (users!= null)
                            {
                                lstItem["LikesCount"] = (double)lstItem["LikesCount"] + likeCount;
                                if (likeCount > 0)//点赞
                                {
                                    users.Add(userValue);
                                    lstItem["LikedBy"] = users;
                                }
                                else //取消赞
                                {
                                    for(int i=0;i<users.Count;i++)
                                    {
                                        if (users[i].LookupId ==loginUser.ID )
                                        {
                                            users.RemoveAt(i);
                                            break;
                                        }
                                    }
                                    lstItem["LikedBy"] = users;
                                }
                            }
                            else
                            {
                                lstItem["LikedBy"] = loginUser ;
                                lstItem["LikesCount"] = likeCount;
                            }
                            lstItem.Update();
                        }
                        catch (Exception ex)
                        {
                            this.Controls.Add(new LiteralControl(ex.ToString()));
                        }
                        thisWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion
    }
}
