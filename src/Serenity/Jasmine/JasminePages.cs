using System;
using System.Collections.Generic;
using System.Reflection;
using FubuMVC.Core.Assets.Files;
using FubuMVC.Core.UI;
using HtmlTags;
using FubuCore;

namespace Serenity.Jasmine
{
    public class JasminePages
    {
        private const string Title = "Serenity Jasmine Runner";
        private readonly SpecificationGraph _specifications;
        private readonly SpecHierarchyBuilder _builder;
        private readonly SpecAssetRequirements _requirements;
        private readonly FubuHtmlDocument _document;
        private static string _header;

        static JasminePages()
        {
            _header =
                Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof (JasminePages), "Header.htm").
                    ReadAllText();
        }

        public JasminePages(SpecificationGraph specifications, SpecHierarchyBuilder builder, SpecAssetRequirements requirements, FubuHtmlDocument document)
        {
            _specifications = specifications;
            _builder = builder;
            _requirements = requirements;
            _document = document;

            _document.Body.Append(new HtmlTag("div").Text(_header).Encoded(false));

        }

        public HtmlDocument Home()
        {
            _document.Title = Title;

            var tag = _builder.CompleteHierarchy();
            _document.Add(tag);

            return _document;
        }

        public HtmlDocument AllSpecs()
        {
            _document.Title = Title + ":  All Specifications";

            writeNode(_specifications);

            return _document;
        }

        public HtmlDocument Spec(SpecPath path)
        {
            _document.Title = Title + ":  " + path.Parts.Join("/");

            var node = _specifications.FindSpecNode(path);

            writeNode(node);

            return _document;
        }

        private void writeNode(ISpecNode node)
        {
            var tag = _builder.BuildInPlaceHierarchyFor(node);



            _document.Add(tag);
            _document.Add("hr");

            _requirements.WriteAssetsInto(_document, node.AllSpecifications);
        }
    }
}