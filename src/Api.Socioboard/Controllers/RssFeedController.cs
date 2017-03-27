﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Api.Socioboard.Model;
using Microsoft.AspNetCore.Cors;
using System.Xml;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Socioboard.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class RssFeedController : Controller
    {
        public RssFeedController(ILogger<RssFeedController> logger, Microsoft.Extensions.Options.IOptions<Helper.AppSettings> settings, IHostingEnvironment env)
        {
            _logger = logger;
            _appSettings = settings.Value;
            _redisCache = new Helper.Cache(_appSettings.RedisConfiguration);
            _env = env;
        }
        private readonly ILogger _logger;
        private Helper.AppSettings _appSettings;
        private Helper.Cache _redisCache;
        private readonly IHostingEnvironment _env;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="groupId">Id of the group to which account is to be added. </param>
        /// <param name="rssUrl"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpPost("AddRssUrl")]
        public IActionResult AddRssUrl(long userId,long groupId,string rssUrl, string profileId)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
                xmlDoc.Load(rssUrl);
                var abc = xmlDoc.DocumentElement.GetElementsByTagName("item");
                if(abc.Count==0)
                {
                    return Ok("This Url Does't  Conatin Rss Feed");
                }
            }
            catch (Exception ex)
            {
                return Ok("This Url Does't  Conatin Rss Feed");
            }

            DatabaseRepository dbr = new DatabaseRepository(_logger, _env);
            Domain.Socioboard.Models.RssFeedUrl _RssFeedUrl = Repositories.RssFeedRepository.AddRssUrl(rssUrl, dbr);
            if(_RssFeedUrl!=null)
            {
                string[] lstProfileIds = null;
                if (profileId != null)
                {
                    lstProfileIds = profileId.Split(',');
                    profileId = lstProfileIds[0];
                }
                else
                {
                    return Ok("profileId required");
                }

                foreach (var item in lstProfileIds)
                {
                    if (item.StartsWith("fb"))
                    {
                        string prId = item.Substring(3, item.Length - 3);
                        Domain.Socioboard.Models.Facebookaccounts objFacebookAccount = Api.Socioboard.Repositories.FacebookRepository.getFacebookAccount(prId, _redisCache, dbr);
                        string ret = Repositories.RssFeedRepository.AddRssFeed(rssUrl, userId, _RssFeedUrl,prId,Domain.Socioboard.Enum.SocialProfileType.Facebook, "http://graph.facebook.com/"+objFacebookAccount.FbUserId+"/picture?type=small", objFacebookAccount.FbUserName,dbr,_appSettings);
                        
                    }

                    if (item.StartsWith("page"))
                    {
                        string prId = item.Substring(5, item.Length - 5);
                        Domain.Socioboard.Models.Facebookaccounts objFacebookAccount = Api.Socioboard.Repositories.FacebookRepository.getFacebookAccount(prId, _redisCache, dbr);
                        string ret = Repositories.RssFeedRepository.AddRssFeed(rssUrl, userId, _RssFeedUrl, prId, Domain.Socioboard.Enum.SocialProfileType.FacebookFanPage, "http://graph.facebook.com/" + objFacebookAccount.FbUserId + "/picture?type=small", objFacebookAccount.FbUserName, dbr, _appSettings);
                        
                    }
                    if (item.StartsWith("tw"))
                    {
                        string prId = item.Substring(3, item.Length - 3);
                        Domain.Socioboard.Models.TwitterAccount objTwitterAccount = Api.Socioboard.Repositories.TwitterRepository.getTwitterAccount(prId, _redisCache, dbr);
                        string ret = Repositories.RssFeedRepository.AddRssFeed(rssUrl, userId, _RssFeedUrl, prId, Domain.Socioboard.Enum.SocialProfileType.Twitter, objTwitterAccount.profileImageUrl, objTwitterAccount.twitterName, dbr, _appSettings);
                        
                    }
                }
               

            }
            return Ok("Added Successfully");
        }

        [HttpGet("GetPostedRssDataByUser")]
        public IActionResult GetPostedRssDataByUser(long userId, long groupId)
        {
            DatabaseRepository dbr = new DatabaseRepository(_logger, _env);
            List<Domain.Socioboard.Models.Mongo.RssFeed> lstRss = Repositories.RssFeedRepository.GetPostedRssDataByUser(userId, groupId, _appSettings, dbr);
            lstRss = lstRss.Where(t => !string.IsNullOrEmpty(t.Message)).ToList();
            return Ok(lstRss);
        }

        [HttpGet("GetRssDataByUser")]
        public IActionResult GetRssDataByUser(long userId, long groupId)
        {
            DatabaseRepository dbr = new DatabaseRepository(_logger, _env);
            List<Domain.Socioboard.Models.Mongo.Rss> lstRss = Repositories.RssFeedRepository.GetRssDataByUser(userId, groupId, _appSettings, dbr);
            return Ok(lstRss);
        }


        [HttpPost("EditFeedUrl")]
        public IActionResult EditFeedUrl(string NewFeedUrl, string OldFeedUrl, string RssId)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
                xmlDoc.Load(NewFeedUrl);
                var abc = xmlDoc.DocumentElement.GetElementsByTagName("item");
                if (abc.Count == 0)
                {
                    return Ok("This Url Does't  Conatin Rss Feed");
                }
            }
            catch (Exception ex)
            {
                return Ok("This Url Does't  Conatin Rss Feed");
            }
            DatabaseRepository dbr = new DatabaseRepository(_logger, _env);
            string editdata = Repositories.RssFeedRepository.EditFeedUrl(NewFeedUrl, OldFeedUrl, RssId, _appSettings, dbr);
            return Ok(editdata);
        }

        [HttpPost("DeleteFeedUrl")]
        public IActionResult DeleteFeedUrl(string RssId)
        {
            DatabaseRepository dbr = new DatabaseRepository(_logger, _env);
            string deletedata = Repositories.RssFeedRepository.DeleteFeedUrl(RssId, dbr, _appSettings);
            return Ok(deletedata);
        }


        [HttpGet("RssNewsFeedsUrl")]
        public IActionResult RssNewsFeedsUrl(long userId, string keyword )
        {
            DatabaseRepository dbr = new DatabaseRepository(_logger, _env);
            string res = Repositories.RssNewsContentsRepository.AddRssContentsUrl(keyword, userId, _appSettings, dbr);
            return Ok(res);    
        }


        [HttpGet("getRssNewsFeedsContents")]
        public IActionResult getRssNewsFeedsContents(string userId, string keyword)
        {

            DatabaseRepository dbr = new DatabaseRepository(_logger, _env);
            List<Domain.Socioboard.Models.Mongo.RssNewsContentsFeeds> lstRss = Repositories.RssNewsContentsRepository.GetRssNewsFeeds(userId,keyword, _appSettings);
          //  lstRss = lstRss.Where(t => !string.IsNullOrEmpty(t.Message)).ToList();
            return Ok(lstRss);


        }



        [HttpGet("getRssNewsFeedsPost")]
        public IActionResult getRssNewsFeedsPost(string userId)
        {

            DatabaseRepository dbr = new DatabaseRepository(_logger, _env);
            List<Domain.Socioboard.Models.Mongo.RssNewsContentsFeeds> lstRss = Repositories.RssNewsContentsRepository.GetRssNewsPostedFeeds(userId, _appSettings);
            //  lstRss = lstRss.Where(t => !string.IsNullOrEmpty(t.Message)).ToList();
            return Ok(lstRss);


        }

    }
}
