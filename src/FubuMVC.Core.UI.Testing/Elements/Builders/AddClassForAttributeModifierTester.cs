using FubuHtml.Elements;
using FubuHtml.Elements.Builders;
using HtmlTags;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuHtml.Testing.Elements.Builders
{
    [TestFixture]
    public class AddClassForAttributeModifierTester
    {
        AddClassForAttributeModifier<FakeRequiredAttribute> theModifier = new AddClassForAttributeModifier<FakeRequiredAttribute>("fizz");

        [Test]
        public void matches_positive()
        {
            theModifier.Matches(ElementRequest.For<AttributeTarget>(x => x.Decorated)).ShouldBeTrue();
        }

        [Test]
        public void matches_negative()
        {
            theModifier.Matches(ElementRequest.For<AttributeTarget>(x => x.NotDecorated)).ShouldBeFalse();
        }

        [Test]
        public void modify_adds_the_class_to_the_current_tag()
        {
            var request = ElementRequest.For<AttributeTarget>(x => x.Decorated);
            request.ReplaceTag(new HtmlTag("div"));

            theModifier.Modify(request);

            request.CurrentTag.HasClass("fizz").ShouldBeTrue();
        }
    }

    public class AttributeTarget
    {
        [FakeRequired]
        public string Decorated { get; set; }
        public string NotDecorated { get; set; }
    }
}