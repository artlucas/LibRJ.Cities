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
	public class TestCountryRetriever
	{
		CountryRetriever import;

		[SetUp]
		public void Setup()
		{
			var mockWebClient = new Mock<IWebClient>();
			mockWebClient.Setup(x => x.DownloadStringTaskAsync(It.IsAny<Uri> ())).ReturnsAsync(
				"#ISO\tISO3\tISO-Numeric\tfips\tCountry\tCapital\tArea(in sq km)\tPopulation\tContinent\ttld\tCurrencyCode\tCurrencyName\tPhone\tPostal Code Format\tPostal Code Regex\tLanguages\tgeonameid\tneighbours\tEquivalentFipsCode\n" +
				"AD\tAND\t020\tAN\tAndorra\tAndorra la Vella\t468\t84000\tEU\t.ad\tEUR\tEuro\t376\tAD###\t^(?:AD)*(\\d{3})$\tca\t3041565\tES,FR\t\n" +
				"AE\tARE\t784\tAE\tUnited Arab Emirates\tAbu Dhabi\t82880\t4975593\tAS\t.ae\tAED\tDirham\t971\t\t\tar-AE,fa,en,hi,ur\t290557\tSA,OM\t\n" +
				"CA\tCAN\t124\tCA\tCanada\tOttawa\t9984670\t33679000\tNA\t.ca\tCAD\tDollar\t1\t@#@ #@#\t^([ABCEGHJKLMNPRSTVXY]\\d[ABCEGHJKLMNPRSTVWXYZ]) ?(\\d[ABCEGHJKLMNPRSTVWXYZ]\\d)$ \ten-CA,fr-CA,iu\t6251999\tUS\t"
			);

			var mockWebClientFactory = new Mock<IWebClientFactory>();
			mockWebClientFactory.Setup(x => x.Create ()).Returns(mockWebClient.Object);

			this.import = new CountryRetriever (mockWebClientFactory.Object);
		}

		[TearDown]
		public void TearDown()
		{
			this.import = null;
		}

		[Test]
		public void ShouldGetSourceData()
		{
			var dataTask = this.import.GetSourceData ();
			dataTask.Wait ();

			StringAssert.Contains ("Andorra", dataTask.Result);
			StringAssert.Contains ("United Arab Emirates", dataTask.Result);
			StringAssert.Contains ("Canada", dataTask.Result);
		}

		[Test]
		public void ShouldParseCountries()
		{
			var dataTask = this.import.GetSourceData ();
			dataTask.Wait ();
			var features = this.import.GetCountries (dataTask.Result).GetEnumerator();

			features.MoveNext ();
			Assert.AreEqual ("Andorra", features.Current.CountryName);

			features.MoveNext ();
			Assert.AreEqual ("United Arab Emirates", features.Current.CountryName);

			features.MoveNext ();
			Assert.AreEqual ("Canada", features.Current.CountryName);
		}

	}
}

