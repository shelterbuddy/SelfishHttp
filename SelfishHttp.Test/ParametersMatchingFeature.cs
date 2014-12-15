﻿using System;
using System.Text.RegularExpressions;
using System.Web;
using NUnit.Framework;
using SelfishHttp.Params.Matching;

namespace SelfishHttp.Test
{
    [TestFixture]
    public class ParametersMatchingFeature
    {
        [Test]
        public void ShouldMatch()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1");
            Assert.That(new EqualityMatch("1", StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.True);
        }

        [Test]
        public void ShouldNotMatch()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1");
            Assert.That(new EqualityMatch("2", StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);
        }

        [Test]
        public void ShouldNotMatchWithMany()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1&id=2&id=3");
            Assert.That(new EqualityMatch("4", StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);
        }

        [Test]
        public void ShouldMatchAll()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1");
            Assert.That(new AllOfMatch(new[] { "1" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.True);
        }

        [Test]
        public void ShouldMatchAllWithMany()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1&id=2&id=3");
            Assert.That(new AllOfMatch(new []{ "1", "2", "3" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.True);
        }

        [Test]
        public void ShouldNotMatchAll()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1&id=2");
            Assert.That(new AllOfMatch(new[] { "1" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);

            parameters = HttpUtility.ParseQueryString("?id=1");
            Assert.That(new AllOfMatch(new[] { "1", "2" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);
        }

        [Test]
        public void ShouldNotMatchAllWithMany()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1&id=2&id=3&id=4");
            Assert.That(new AllOfMatch(new[] { "1", "2", "3" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);

            parameters = HttpUtility.ParseQueryString("?id=1&id=2");
            Assert.That(new AllOfMatch(new[] { "1", "2", "3" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);
        }

        [Test]
        public void ShouldMatchAny()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1");
            Assert.That(new AnyOfMatch(new[] { "1", "2", "3" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.True);
        }

        [Test]
        public void ShouldMatchAnyWithMany()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1&id=2");
            Assert.That(new AnyOfMatch(new[] { "1", "2", "3", "4" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.True);
        }

        [Test]
        public void ShouldNotMatchAny()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1");
            Assert.That(new AnyOfMatch(new[] { "2" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);

            parameters = HttpUtility.ParseQueryString("?id=1&id=2");
            Assert.That(new AnyOfMatch(new[] { "3" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);
        }

        [Test]
        public void ShouldNotMatchAnyWithMany()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1&id=2");
            Assert.That(new AnyOfMatch(new[] { "3", "4" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);

            parameters = HttpUtility.ParseQueryString("?id=1&id=2&id=3");
            Assert.That(new AnyOfMatch(new[] { "3", "4" }, StringComparison.CurrentCulture).IsMatch(parameters.GetValues("id")), Is.False);
        }

        [Test]
        public void CanMatch()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1");
            Assert.That(new RegexMatch(new Regex("[0-9]")).IsMatch(parameters.GetValues("id")), Is.True);
        }

        [Test]
        public void CanMatchWithMany()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1&id=2&id=3");
            Assert.That(new RegexMatch(new Regex("[0-9]")).IsMatch(parameters.GetValues("id")), Is.True);
        }

        [Test]
        public void CannotMatch()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1");
            Assert.That(new RegexMatch(new Regex("[^0-9]")).IsMatch(parameters.GetValues("id")), Is.False);

        }

        [Test]
        public void CannotMatchWithMany()
        {
            var parameters = HttpUtility.ParseQueryString("?id=1&id=2&id=3");
            Assert.That(new RegexMatch(new Regex("[^0-9]")).IsMatch(parameters.GetValues("id")), Is.False);
        }        
    }
}