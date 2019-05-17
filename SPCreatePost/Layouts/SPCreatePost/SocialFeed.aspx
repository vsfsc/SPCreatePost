<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SocialFeed.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.SocialFeed" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table width="100%" id="tblPosts"></table><br/>
    <span id="spanMessage" style="color: #FF0000;"></span>
    <SharePoint:ScriptLink ID="ScriptLink1" name="SP.js" runat="server" ondemand="false" localizable="false" loadafterui="true" />
    <SharePoint:ScriptLink ID="ScriptLink2" name="SP.UserProfiles.js" runat="server" ondemand="false" localizable="false" loadafterui="true" />
    <SharePoint:FormDigest id="FormDigest" runat="server"/>
<script type="text/javascript">

    //var targetUser = 'ccc\\xueqingxia';

    // Ensure that the SP.UserProfiles.js file is loaded before the custom code runs.
    SP.SOD.executeOrDelayUntilScriptLoaded(GetFeeds, 'SP.UserProfiles.js');

    // Declare global variables.
    var clientContext;
    var feedManager;
    var personalFeed;
    var newsFeed;
    var timelineFeed;
    var targetUserFeed;

    function GetFeeds() {
        // Initialize the current client context and the SocialFeedManager instance.
        clientContext = SP.ClientContext.get_current();
        feedManager = new SP.Social.SocialFeedManager(clientContext);

        // Set parameters for the feed content that you want to retrieve.
        var feedOptions = new SP.Social.SocialFeedOptions();
        feedOptions.set_maxThreadCount(1000); // default is 20

        // Get all feed types for current user and get the Personal feed
        // for the target user.
        personalFeed = feedManager.getFeed(SP.Social.SocialFeedType.personal, feedOptions);
        //newsFeed = feedManager.getFeed(SP.Social.SocialFeedType.news, feedOptions);
        //targetUserFeed = feedManager.getFeedFor(targetUser, feedOptions);

        // Change the sort order to optimize the Timeline feed results.
        //feedOptions.set_sortOrder(SP.Social.SocialFeedSortOrder.byCreatedTime);
        //timelineFeed = feedManager.getFeed(SP.Social.SocialFeedType.timeline, feedOptions);

        clientContext.load(feedManager);
        clientContext.executeQueryAsync(CallIterateFunctionForFeeds, RequestFailed);
    }
    function CallIterateFunctionForFeeds() {
        IterateThroughFeed(personalFeed, "Personal", true);
        //IterateThroughFeed(newsFeed, "News", true);
        //IterateThroughFeed(timelineFeed, "Timeline", true);
        //IterateThroughFeed(targetUserFeed, "Personal", false);
    }
    function IterateThroughFeed(feed, feedType, isCurrentUser) {
        //tblPosts.insertRow().insertCell();
        var feedHeaderRow = tblPosts.insertRow();
        var feedOwner = feedManager.get_owner().get_name();

        // Iterate through the array of threads in the feed.
        var threads = feed.get_threads();
        for (var i = 0; i < threads.length ; i++) {
            var thread = threads[i];
            var actors = thread.get_actors();
            //if (i == 0) {

            //    // Get the name of the target user for the feed header row. Users are 
            //    // owners of all threads in their Personal feed.
            //    if (!isCurrentUser) {
            //        feedOwner = actors[thread.get_ownerIndex()].get_name();
            //    }
            //    feedHeaderRow.insertCell().innerText = feedType.toUpperCase() + ' FEED FOR '
            //        + feedOwner.toUpperCase();
            //}

            // Use only Normal-type threads and ignore reference-type threads. (SocialThreadType.Normal = 0).get_threadType() == 0
            if (thread.get_attributes() == 6) {//.get_attributes() == 0) {

                // Get the root post's author, content, and number of replies.
                var post = thread.get_rootPost();
                var authorName = actors[post.get_authorIndex()].get_name();
                var postContent = post.get_text();
                var totalReplies = thread.get_totalReplyCount();
               
                var postRow = tblPosts.insertRow();
                postRow.insertCell().innerText = postContent + '\" (' + totalReplies + ' replies)';
                   

                // If there are any replies, iterate through the array and
                // get the author and content. 
                // If a thread contains more than two replies, the server
                // returns a thread digest that contains only the two most
                // recent replies. To get all replies, call the 
                // SocialFeedManager.getFullThread method.
                if (totalReplies > 0) {
                    var replies = thread.get_replies();

                    for (var j = 0; j < replies.length; j++) {
                        var replyRow = tblPosts.insertRow();

                        var reply = replies[j];
                        replyRow.insertCell().innerText = '  - ' + actors[reply.get_authorIndex()].get_name()
                            + ' replied \"' + reply.get_text() + '\"';
                    }
                }
            }
        }
    }
    function RequestFailed(sender, args) {
        $get("spanMessage").innerText = 'Request failed: ' + args.get_message();
    }

</script></asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
新闻源
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的新闻源
</asp:Content>
