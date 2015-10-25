//  _    _ _    ___    _  ___ _ _   _        
// | |  (_) |__| _ \_ | |/ __(_) |_(_)___ ___
// | |__| | '_ \   / || | (__| |  _| / -_|_-<
// |____|_|_.__/_|_\\__(_)___|_|\__|_\___/__/
//
// Author(s):
//  Arthur Lucas <arthur@remitjet.com>
//
// Copyright (c) 2015 Remit Jet, Ltd.
//
// By using this software you agree to our software license as detailed in the
// LICENSE.txt file in the root of the repository.  You can also view this file
// online at: https://github.com/RemitJet/LibRJ.Cities/blob/master/LICENSE.txt
//
using System;
using System.Collections;
using System.IO;
using NUnit.Framework;
using Moq;
using LibRJ.Cities.GeoNames.Retrievers;

namespace LibRJ.Cities.Tests
{
	[TestFixture]
	public class TestCountryFeatureRetriever
	{
		CountryFeatureRetriever import;

		[SetUp]
		public void Setup()
		{
			var mockWebClient = new Mock<IWebClient>();
			mockWebClient.Setup(x => x.DownloadDataTaskAsync(It.IsAny<Uri> ())).ReturnsAsync(
				File.ReadAllBytes("Assets/CA.zip")
			);

			var mockWebClientFactory = new Mock<IWebClientFactory>();
			mockWebClientFactory.Setup(x => x.Create ()).Returns(mockWebClient.Object);

			this.import = new CountryFeatureRetriever (mockWebClientFactory.Object);
		}

		[TearDown]
		public void TearDown()
		{
			this.import = null;
		}

		[Test]
		public void ShouldGetSourceData()
		{
			var dataTask = this.import.GetSourceData ("CA");
			dataTask.Wait ();

			StringAssert.Contains ("British Columbia", dataTask.Result);
			StringAssert.Contains ("Alberta", dataTask.Result);
			StringAssert.Contains ("Quesnel", dataTask.Result);
			StringAssert.Contains ("Red Deer", dataTask.Result);
			StringAssert.Contains ("Abbotsford", dataTask.Result);
		}

		[Test]
		public void ShouldDetermineIfRegionOrCity()
		{
			var dataTask = this.import.GetSourceData ("CA");
			dataTask.Wait ();
			var features = this.import.GetCountryFeatures (dataTask.Result).GetEnumerator();
			features.MoveNext ();

			if (features.Current.Name != "Alberta")		// Alberta is a province (Region)
				Assert.Inconclusive ();
			Assert.IsTrue (features.Current.IsRegion);
			Assert.IsFalse (features.Current.IsCity);

			features.MoveNext ();
			features.MoveNext ();
			features.MoveNext ();

			if (features.Current.Name != "Abbotsford")	// Abbotsford is a city
				Assert.Inconclusive ();
			Assert.IsFalse (features.Current.IsRegion);
			Assert.IsTrue (features.Current.IsCity);
		}

		[Test]
		public void ShouldParseCountryFeatures()
		{
			var dataTask = this.import.GetSourceData ("CA");
			dataTask.Wait ();
			var features = this.import.GetCountryFeatures (dataTask.Result).GetEnumerator();

			features.MoveNext ();
			Assert.AreEqual ("Alberta", features.Current.Name);
			Assert.IsTrue (features.Current.IsRegion);
			Assert.IsFalse (features.Current.IsCity);

			features.MoveNext ();
			Assert.AreEqual ("British Columbia", features.Current.Name);
			Assert.IsTrue (features.Current.IsRegion);
			Assert.IsFalse (features.Current.IsCity);

			features.MoveNext ();
			Assert.AreEqual ("Manitoba", features.Current.Name);
			Assert.IsTrue (features.Current.IsRegion);
			Assert.IsFalse (features.Current.IsCity);

			features.MoveNext ();
			Assert.AreEqual ("Abbotsford", features.Current.Name);
			Assert.IsFalse (features.Current.IsRegion);
			Assert.IsTrue (features.Current.IsCity);
		}

	}
}

