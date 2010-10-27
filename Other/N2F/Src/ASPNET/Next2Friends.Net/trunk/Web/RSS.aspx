<?xml version="1.0" encoding="UTF-8"?><%@ Page Language="C#" AutoEventWireup="true" ContentType="text/xml" MaintainScrollPositionOnPostback="false" EnableTheming="false" CodeFile="RSS.aspx.cs" Inherits="RSS" %><head runat="server" visible="false"></head><asp:repeater id="rptRssFeed" runat="server"><HeaderTemplate>
<rss version="2.0" xmlns:media="http://search.yahoo.com/mrss/" xmlns:dc="http://purl.org/dc/elements/1.1/">
    <channel>
    <title><%= FeedTitle %></title>
		<link>http://www.next2friends.com</link>
		<image> 
			<url>http://www.next2friends.com/images/logo.gif</url>
			<link>http://www.next2friends.com</link> 
			<title><%= FeedTitle %></title> 
			<height>46</height>
			<width>234</width>
		</image>
		<description><%= FeedDescription %></description>
</HeaderTemplate>
<ItemTemplate>
    <item>
      <title><![CDATA[<%# Eval("Title") %>]]></title>
      <description><![CDATA[<%# Eval("Description")%>]]></description>
      <pubDate><![CDATA[<%# Eval("DtCreated")%>]]></pubDate>
      <link>http://www.next2friends.com<%# Eval("Link")%></link>
      <media:title><![CDATA[<%# Eval("Title") %>]]></media:title>
      
      <media:description type="plain"></media:description>
      <media:keywords></media:keywords>
      <media:thumbnail width="128" height="96" url="<%# Eval("ResourceFileThumb") %>"/>
    </item>

</ItemTemplate>
<FooterTemplate>
    </channel>
</rss>
</FooterTemplate>
</asp:repeater>