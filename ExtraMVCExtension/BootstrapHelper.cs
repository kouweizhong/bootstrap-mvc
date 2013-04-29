﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ExtraMVCExtension.Bootstrap.Enums;

namespace ExtraMVCExtension.Bootstrap
{
    /// <summary>
    /// Provides static methods to generate HTML code for Twitter's Bootstrap.
    /// </summary>
    public class BootstrapHelper
    {
        #region Fields
        private readonly HtmlHelper _helper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the Bootstrap helper.
        /// </summary>
        /// <param name="helper">The view HtmlHelper</param>
        public BootstrapHelper(HtmlHelper helper)
        {
            _helper = helper;
        }
        #endregion

        #region Typography
        /// <summary>
        /// Make a paragraph using the lead class to make it stand out.
        /// </summary>
        /// <param name="text">Content of the paragraph</param>
        public static MvcHtmlString LeadBody(string text)
        {
            var p = new TagBuilder("p");
            p.AddCssClass("lead");
            p.SetInnerText(text);

            return MvcHtmlString.Create(p.ToString());
        }

        /// <summary>
        /// Create an emphasized paragraph
        /// </summary>
        /// <param name="text">Content of the emphasized paragraph</param>
        /// <param name="emphasisType">Type of the emphasis</param>
        public static MvcHtmlString EmphasizedParagraph(string text, Emphasis emphasisType)
        {
            var p = new TagBuilder("p");

            switch (emphasisType)
            {
                case Emphasis.Muted:
                    p.AddCssClass("muted");
                    break;
                case Emphasis.Warning:
                    p.AddCssClass("text-warning");
                    break;
                case Emphasis.Error:
                    p.AddCssClass("text-error");
                    break;
                case Emphasis.Info:
                    p.AddCssClass("text-info");
                    break;
                case Emphasis.Success:
                    p.AddCssClass("text-success");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("emphasisType");
            }

            p.SetInnerText(text);
            return MvcHtmlString.Create(p.ToString());
        }

        /// <summary>
        /// Create an HTML abbreviation with it's corresponding definition.
        /// </summary>
        /// <param name="title">Definition of the abbreaviation.</param>
        /// <param name="value">The abbreviation.</param>
        public static MvcHtmlString Abbreviation(string title, string value)
        {
            return Abbreviation(title, value, false);
        }

        /// <summary>
        /// Create an HTML abbreviation with it's corresponding definition.
        /// </summary>
        /// <param name="title">Definition of the abbreaviation.</param>
        /// <param name="value">The abbreviation.</param>
        /// <param name="isReduced">Defines if the abbreviation uses the initialism class for a slightly smaller font-size.</param>
        public static MvcHtmlString Abbreviation(string title, string value, bool isReduced)
        {
            var abbr = new TagBuilder("abbr");
            if (isReduced)
                abbr.AddCssClass("initialism");
            abbr.MergeAttribute("title", title);
            abbr.SetInnerText(value);

            return MvcHtmlString.Create(abbr.ToString());
        }

        /// <summary>
        /// Creates an HTML blockquote.
        /// </summary>
        /// <param name="quote">The quote.</param>
        /// <param name="author">The author.</param>
        /// <param name="source">The source.</param>
        /// <param name="sourceTitle">The source title.</param>
        public static MvcHtmlString Blockquote(string quote, string author, string source, string sourceTitle)
        {
            return Blockquote(quote, author, source, sourceTitle, false);
        }

        /// <summary>
        /// Creates an HTML blockquote.
        /// </summary>
        /// <param name="quote">The quote.</param>
        /// <param name="author">The author.</param>
        /// <param name="source">The source.</param>
        /// <param name="sourceTitle">The source title.</param>
        /// <param name="isPulledRight">Set to true for a floated, right-aligned blockquote.</param>
        public static MvcHtmlString Blockquote(string quote, string author, string source, string sourceTitle, bool isPulledRight)
        {
            var cite = new TagBuilder("cite");
            cite.MergeAttribute("title", sourceTitle);
            cite.SetInnerText(source);

            var small = new TagBuilder("small") {InnerHtml = String.Concat(author, " ", cite.ToString())};

            var p = new TagBuilder("p");
            p.SetInnerText(quote);

            var blockquote = new TagBuilder("blockquote");
            if (isPulledRight)
                blockquote.AddCssClass("pull-right");

            blockquote.InnerHtml = String.Concat(p.ToString(), small.ToString());

            return MvcHtmlString.Create(blockquote.ToString());
        }

        /// <summary>
        /// Begin a new list tag
        /// </summary>
        /// <param name="listType">Type of the desired list.</param>
        /// <returns></returns>
        public BootstrapMVCList BeginList(ListType listType)
        {
            var list = new BootstrapMVCList(_helper.ViewContext);
            list.BeginList(listType);

            return list;
        }

        /// <summary>
        /// A list of terms with their associated descriptions.
        /// </summary>
        /// <param name="isHorizontal">Make terms and descriptions in dl line up side-by-side.</param>
        public BootstrapMVCList BeginDescriptionList(bool isHorizontal)
        {
            var list = new BootstrapMVCList(_helper.ViewContext);
            list.BeginDescriptionList(isHorizontal);

            return list;
        }

        /// <summary>
        /// Creates a list with the associated elements.
        /// </summary>
        /// <param name="listType">The type of the list.</param>
        /// <param name="elements">The elements of the list.</param>
        public static MvcHtmlString List(ListType listType, IEnumerable<string> elements)
        {
            var root = BootstrapMVCList.GetRootTagBuilder(listType);

            if (elements == null)
                return MvcHtmlString.Create(root.ToString());

            foreach (var element in elements)
            {
                root.AddChildTag(new TagBuilderExt("li", element));
            }

            return MvcHtmlString.Create(root.ToString());
        }

        /// <summary>
        /// Creates a description list with the associated descriptions
        /// </summary>
        /// <param name="isHorizontal">Make terms and descriptions in line up side-by-side.</param>
        /// <param name="elements">The dictionary of descriptions by title (key) and description (value).</param>
        public static MvcHtmlString DescriptionList(bool isHorizontal, IDictionary<string, string> elements)
        {
            if (elements == null)
                return null;

            var root = new TagBuilderExt("dl");
            if (isHorizontal)
                root.AddCssClass("dl-horizontal");

            foreach (var element in elements)
            {
                root.AddChildTag(new TagBuilderExt("dt", element.Key));
                root.AddChildTag(new TagBuilderExt("dd", element.Value));
            }

            return MvcHtmlString.Create(root.ToString());
        }
        #endregion

        #region Images
        /// <summary>
        /// Define an image
        /// </summary>
        /// <param name="source">the url for the image</param>
        /// <param name="alt">alternate text for the image</param>
        /// <param name="imageType">type of image</param>
        public static MvcHtmlString Image(string source, string alt, ImageType imageType)
        {
            var img = new TagBuilder("img");
            img.MergeAttribute("alt", alt);
            img.MergeAttribute("src", source);

            switch (imageType)
            {
                case ImageType.Rounded:
                    img.AddCssClass("img-rounded");
                    break;
                case ImageType.Circle:
                    img.AddCssClass("img-circle");
                    break;
                case ImageType.Polaroid:
                    img.AddCssClass("img-polaroid");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("imageType");
            }

            return MvcHtmlString.Create(img.ToString());
        }

        /// <summary>
        /// Define an Image(source as the alternate text). 
        /// </summary>
        /// <param name="source">url and alternate text</param>
        /// <param name="imageType">type of image</param>
        public static MvcHtmlString Image(string source, ImageType imageType)
        {
            return Image(source, source, imageType);
        }
        #endregion
    }
}