using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.MetaWeblog
{
    [XmlRpcUrl("https://storage.msn.com/storageservice/MetaWeblog.rpc")]
    class MetaWeblogGateway : XmlRpcClientProtocol
    {
        /// <summary> 
        /// Returns the most recent draft and non-draft blog posts sorted in descending order by publish date. 
        /// </summary> 
        /// <param name="blogid"> This should be the String MyBlog, which indicates that the post is being created in the user’s blog. </param> 
        /// <param name="username"> The name of the user’s space. </param> 
        /// <param name="password"> The user’s secret word. </param> 
        /// <param name="numberOfPosts"> The number of posts to return. The maximum value is 20. </param> 
        /// <returns></returns>
        [XmlRpcMethod("metaWeblog.getRecentPosts")]
        public Post[] getRecentPosts(String blogid, String username, String password, Int32 numberOfPosts)
        {
            return (Post[])Invoke("getRecentPosts", new Object[] { blogid, username, password, numberOfPosts });
        }

        [XmlRpcMethod("wp.newCategory")]
        public Int32 newCategory(Int32 blogid, String username, String password, NewCategory oNewCat)
        {
            return Convert.ToInt32(Invoke("newCategory", new Object[] { Convert.ToInt32(blogid), username, password, oNewCat }));
        }

        public Int32 newCategory(String blogid, String username, String password, String category)
        {
            return newCategory(
                Convert.ToInt32(blogid),
                username,
                password,
                new NewCategory()
                {
                    name = category,
                    slug = "",
                    parent_id = 0,
                    description = category
                });
        }

        /// <summary> 
        /// Posts a new entry to a blog. 
        /// </summary> 
        /// <param name="blogid"> This should be the String MyBlog, which indicates that the post is being created in the user’s blog. </param> 
        /// <param name="username"> The name of the user’s space. </param> 
        /// <param name="password"> The user’s secret word. </param> 
        /// <param name="post"> A struct representing the content to update. </param> 
        /// <param name="publish"> If false, this is a draft post. </param> 
        /// <returns> The postid of the newly-created post. </returns> 
        [XmlRpcMethod("metaWeblog.newPost")]
        public String newPost(String blogid, String username, String password, Post content, Boolean publish)
        {
            return (String)Invoke("newPost", new Object[] { blogid, username, password, content, publish });
        }

        [XmlRpcMethod("metaWeblog.newComment")]
        public String newComment(String blogid, String username, String password, Comment comment)
        {
            return (String)Invoke("newComment", new Object[] { blogid, username, password, comment });
        }

        /// <summary> 
        /// Edits an existing entry on a blog. 
        /// </summary> 
        /// <param name="postid"> The ID of the post to update. </param> 
        /// <param name="username"> The name of the user’s space. </param> 
        /// <param name="password"> The user’s secret word. </param> 
        /// <param name="post"> A struct representing the content to update. </param> 
        /// <param name="publish"> If false, this is a draft post. </param> 
        /// <returns> Always returns true. </returns> 
        [XmlRpcMethod("metaWeblog.editPost")]
        public Boolean editPost(String postid, String username, String password, Post content, Boolean publish)
        {
            return (Boolean)Invoke("editPost", new Object[] { postid, username, password, content, publish });
        }

        /// <summary> 
        /// Deletes a post from the blog. 
        /// </summary> 
        /// <param name="appKey"> This value is ignored. </param> 
        /// <param name="postid"> The ID of the post to update. </param> 
        /// <param name="username"> The name of the user’s space. </param> 
        /// <param name="password"> The user’s secret word. </param> 
        /// <param name="post"> A struct representing the content to update. </param> 
        /// <param name="publish"> This value is ignored. </param> 
        /// <returns> Always returns true. </returns> 
        [XmlRpcMethod("blogger.deletePost")]
        public Boolean deletePost(String appKey, String postid, String username, String password, Boolean publish)
        {
            return (Boolean)Invoke("deletePost", new Object[] { appKey, postid, username, password, publish });
        }

        /// <summary> 
        /// Returns information about the user’s space. An empty array is returned if the user does not have a space. 
        /// </summary> 
        /// <param name="appKey"> This value is ignored. </param> 
        /// <param name="postid"> The ID of the post to update. </param> 
        /// <param name="username"> The name of the user’s space. </param> 
        /// <returns> An array of structs that represents each of the user’s blogs. The array will contain a maximum of one struct, since a user can only have a single space with a single blog. </returns> 
        [XmlRpcMethod("blogger.getUsersBlogs")]
        public UserBlog[] getUsersBlogs(String appKey, String username, String password)
        {
            return (UserBlog[])Invoke("getUsersBlogs", new Object[] { appKey, username, password });
        }

        /// <summary> 
        /// Returns basic user info (name, e-mail, userid, and so on). 
        /// </summary> 
        /// <param name="appKey"> This value is ignored. </param> 
        /// <param name="postid"> The ID of the post to update. </param> 
        /// <param name="username"> The name of the user’s space. </param> 
        /// <returns> A struct containing profile information about the user. 
        /// Each struct will contain the following fields: nickname, userid, url, e-mail, 
        /// lastname, and firstname. </returns> 
        [XmlRpcMethod("blogger.getUserInfo")]
        public UserInfo getUserInfo(String appKey, String username, String password)
        {
            return (UserInfo)Invoke("getUserInfo", new Object[] { appKey, username, password });
        }

        /// <summary> 
        /// Returns a specific entry from a blog. 
        /// </summary> 
        /// <param name="postid"> The ID of the post to update. </param> 
        /// <param name="username"> The name of the user’s space. </param> 
        /// <param name="password"> The user’s secret word. </param> 
        /// <returns> Always returns true. </returns> 
        [XmlRpcMethod("metaWeblog.getPost")]
        public Post getPost(String postid, String username, String password)
        {
            return (Post)Invoke("getPost", new Object[] { postid, username, password });
        }

        /// <summary> 
        /// Returns the list of categories that have been used in the blog. 
        /// </summary> 
        /// <param name="blogid"> This should be the String MyBlog, which indicates that the post is being created in the user’s blog. </param> 
        /// <param name="username"> The name of the user’s space. </param> 
        /// <param name="password"> The user’s secret word. </param> 
        /// <returns> An array of structs that contains one struct for each category. Each category struct will contain a description field that contains the name of the category. </returns> 
        [XmlRpcMethod("metaWeblog.getCategories")]
        public Category[] getCategories(String blogid, String username, String password)
        {
            return (Category[])Invoke("getCategories", new Object[] { blogid, username, password });
        }

        [XmlRpcMethod("wpLinkMentor.getLinks")]
        public Link[] getLinks(String blogid, String username, String password, String catid)
        {
            return (Link[])Invoke("getLinks", new Object[] { blogid, username, password, catid });
        }

        [XmlRpcMethod("wpLinkMentor.deleteLink")]
        public Boolean deleteLink(String blogid, String username, String password, String linkid)
        {
            return (Boolean)Invoke("deleteLink", new Object[] { blogid, username, password, linkid });
        }

        [XmlRpcMethod("wpLinkMentor.updateLink")]
        public Int32 updateLink(
            String blogid,
            String username,
            String password,
            String catid,
            String linkid,
            String link_url,
            String link_title,
            String link_description,
            String link_visible,
            String link_rel)
        {

            return (Int32)Invoke("updateLink", new Object[] { blogid, username, password, catid, linkid, link_url, link_title, link_description, link_visible, link_rel });
        }
    }
}
