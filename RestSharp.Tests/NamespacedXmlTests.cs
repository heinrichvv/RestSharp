﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Xml.Linq;
using RestSharp.Deserializers;

namespace RestSharp.Tests
{
	public class NamespacedXmlTests
	{
		[Fact]
		public void Can_Deserialize_Elements_With_Namespace() {
			var doc = CreateElementsXml();

			var d = new XmlDeserializer();
			d.Namespace = "http://restsharp.org";
			var p = d.Deserialize<Person>(doc);

			Assert.Equal("John Sheehan", p.Name);
			Assert.Equal(new DateTime(2009, 9, 25, 0, 6, 1), p.StartDate);
			Assert.Equal(28, p.Age);
			Assert.Equal(long.MaxValue, p.BigNumber);
			Assert.Equal(99.9999m, p.Percent);
			Assert.Equal(false, p.IsCool);

			Assert.NotNull(p.Friends);
			Assert.Equal(10, p.Friends.Count);

			Assert.NotNull(p.BestFriend);
			Assert.Equal("The Fonz", p.BestFriend.Name);
			Assert.Equal(1952, p.BestFriend.Since);
		}

		[Fact]
		public void Can_Deserialize_Attributes_With_Namespace() {
			var doc = CreateAttributesXml();

			var d = new XmlDeserializer();
			d.Namespace = "http://restsharp.org";
			var p = d.Deserialize<Person>(doc);

			Assert.Equal("John Sheehan", p.Name);
			Assert.Equal(new DateTime(2009, 9, 25, 0, 6, 1), p.StartDate);
			Assert.Equal(28, p.Age);
			Assert.Equal(long.MaxValue, p.BigNumber);
			Assert.Equal(99.9999m, p.Percent);
			Assert.Equal(false, p.IsCool);

			Assert.NotNull(p.BestFriend);
			Assert.Equal("The Fonz", p.BestFriend.Name);
			Assert.Equal(1952, p.BestFriend.Since);
		}

		[Fact]
		public void Ignore_Protected_Property_That_Exists_In_Data() {
			var doc = CreateElementsXml();

			var d = new XmlDeserializer();
			d.Namespace = "http://restsharp.org";
			var p = d.Deserialize<Person>(doc);

			Assert.Null(p.IgnoreProxy);
		}

		[Fact]
		public void Ignore_ReadOnly_Property_That_Exists_In_Data() {
			var doc = CreateElementsXml();

			var d = new XmlDeserializer();
			d.Namespace = "http://restsharp.org";
			var p = d.Deserialize<Person>(doc);

			Assert.Null(p.ReadOnlyProxy);
		}

		[Fact]
		public void Can_Deserialize_Names_With_Underscores_With_Namespace() {
			var doc = CreateUnderscoresXml();

			var d = new XmlDeserializer();
			d.Namespace = "http://restsharp.org";
			var p = d.Deserialize<Person>(doc);

			Assert.Equal("John Sheehan", p.Name);
			Assert.Equal(new DateTime(2009, 9, 25, 0, 6, 1), p.StartDate);
			Assert.Equal(28, p.Age);
			Assert.Equal(long.MaxValue, p.BigNumber);
			Assert.Equal(99.9999m, p.Percent);
			Assert.Equal(false, p.IsCool);

			Assert.NotNull(p.Friends);
			Assert.Equal(10, p.Friends.Count);

			Assert.NotNull(p.BestFriend);
			Assert.Equal("The Fonz", p.BestFriend.Name);
			Assert.Equal(1952, p.BestFriend.Since);

			Assert.NotNull(p.Foes);
			Assert.Equal(5, p.Foes.Count);
			Assert.Equal("Yankees", p.Foes.Team);
		}

		private static string CreateUnderscoresXml() {
			var doc = new XDocument();
			var ns = XNamespace.Get("http://restsharp.org");
			var root = new XElement(ns + "Person");
			root.Add(new XElement(ns + "Name", "John Sheehan"));
			root.Add(new XElement(ns + "Start_Date", new DateTime(2009, 9, 25, 0, 6, 1)));
			root.Add(new XAttribute(ns + "Age", 28));
			root.Add(new XElement(ns + "Percent", 99.9999m));
			root.Add(new XElement(ns + "Big_Number", long.MaxValue));
			root.Add(new XAttribute(ns + "Is_Cool", false));
			root.Add(new XElement(ns + "Ignore", "dummy"));
			root.Add(new XAttribute(ns + "Read_Only", "dummy"));

			root.Add(new XElement(ns + "Best_Friend",
						new XElement(ns + "Name", "The Fonz"),
						new XAttribute(ns + "Since", 1952)
					));

			var friends = new XElement(ns + "Friends");
			for (int i = 0; i < 10; i++) {
				friends.Add(new XElement(ns + "Friend",
								new XElement(ns + "Name", "Friend" + i),
								new XAttribute(ns + "Since", DateTime.Now.Year - i)
							));
			}
			root.Add(friends);

			var foes = new XElement(ns + "Foes");
			foes.Add(new XAttribute(ns + "Team", "Yankees"));
			for (int i = 0; i < 5; i++) {
				foes.Add(new XElement(ns + "Foe", new XElement(ns + "Nickname", "Foe" + i)));
			}
			root.Add(foes);

			doc.Add(root);
			return doc.ToString();
		}

		private static string CreateElementsXml() {
			var doc = new XDocument();
			var ns = XNamespace.Get("http://restsharp.org");
			var root = new XElement(ns + "Person");
			root.Add(new XElement(ns + "Name", "John Sheehan"));
			root.Add(new XElement(ns + "StartDate", new DateTime(2009, 9, 25, 0, 6, 1)));
			root.Add(new XElement(ns + "Age", 28));
			root.Add(new XElement(ns + "Percent", 99.9999m));
			root.Add(new XElement(ns + "BigNumber", long.MaxValue));
			root.Add(new XElement(ns + "IsCool", false));
			root.Add(new XElement(ns + "Ignore", "dummy"));
			root.Add(new XElement(ns + "ReadOnly", "dummy"));

			root.Add(new XElement(ns + "BestFriend",
						new XElement(ns + "Name", "The Fonz"),
						new XElement(ns + "Since", 1952)
					));

			var friends = new XElement(ns + "Friends");
			for (int i = 0; i < 10; i++) {
				friends.Add(new XElement(ns + "Friend",
								new XElement(ns + "Name", "Friend" + i),
								new XElement(ns + "Since", DateTime.Now.Year - i)
							));
			}
			root.Add(friends);

			doc.Add(root);
			return doc.ToString();
		}

		private static string CreateAttributesXml() {
			var doc = new XDocument();
			var ns = XNamespace.Get("http://restsharp.org");
			var root = new XElement(ns + "Person");
			root.Add(new XAttribute(ns + "Name", "John Sheehan"));
			root.Add(new XAttribute(ns + "StartDate", new DateTime(2009, 9, 25, 0, 6, 1)));
			root.Add(new XAttribute(ns + "Age", 28));
			root.Add(new XAttribute(ns + "Percent", 99.9999m));
			root.Add(new XAttribute(ns + "BigNumber", long.MaxValue));
			root.Add(new XAttribute(ns + "IsCool", false));
			root.Add(new XAttribute(ns + "Ignore", "dummy"));
			root.Add(new XAttribute(ns + "ReadOnly", "dummy"));

			root.Add(new XElement(ns + "BestFriend",
						new XAttribute(ns + "Name", "The Fonz"),
						new XAttribute(ns + "Since", 1952)
					));

			doc.Add(root);
			return doc.ToString();
		}
	}
}