using BigMill.Models;
using SimpleMvcSitemap;
using SimpleMvcSitemap.Images;
using SimpleMvcSitemap.Mobile;
using SimpleMvcSitemap.News;
using SimpleMvcSitemap.StyleSheets;
using SimpleMvcSitemap.Translations;
using SimpleMvcSitemap.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigMill.Controllers
{
  
    public class SitemapController : Controller
    {
        private readonly ISitemapProvider sitemapProvider;

        private SiteMapDataBuilder dataBuilder;



        public SitemapController() : this(new SitemapProvider()) { }


        public SitemapController(ISitemapProvider sitemapProvider)
        {
            this.sitemapProvider = sitemapProvider;
            dataBuilder = new SiteMapDataBuilder();
        }

 
        public ActionResult SitemapXml()
        {
            return sitemapProvider.CreateSitemapIndex(new SitemapIndexModel(new List<SitemapIndexNode>
            {
                new SitemapIndexNode(Url.Action("Default")),
                new SitemapIndexNode(Url.Action("News")),
                new SitemapIndexNode(Url.Action("Image")),
            }));
        }

        public ActionResult Default()
        {
            return sitemapProvider.CreateSitemap(new SitemapModel(
                dataBuilder.CreateSitemapNodeWithAllProperties()
            ));
        }

        public ActionResult Image()
        {
            return sitemapProvider.CreateSitemap(new SitemapModel(
                dataBuilder.CreateSitemapNodeWithImageAllProperties()));
        }

        public ActionResult News()
        {
            return sitemapProvider.CreateSitemap(new SitemapModel(
                dataBuilder.CreateSitemapNodeWithNewsAllProperties()
            ));
        }


    }


}