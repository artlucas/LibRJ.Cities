//  _    _ _    ___    _  ___ _ _   _        
// | |  (_) |__| _ \_ | |/ __(_) |_(_)___ ___
// | |__| | '_ \   / || | (__| |  _| / -_|_-<
// |____|_|_.__/_|_\\__(_)___|_|\__|_\___/__/
//
// Author:
//   Arthur Lucas <arthur@remitjet.com>
//
// Copyright (c) 2015, Remit Jet, Ltd. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the
//      following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
//      following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of Remit Jet, Ltd. nor the names of its contributors may be used to endorse or promote
//      products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;
using Moq;
using LibRJ.Cities.GeoNames;
using LibRJ.Cities.Tests.Fakes;
using LibRJ.Cities.Models;

namespace LibRJ.Cities.Tests.GeoNames
{
    [TestFixture]
    public class TestCountryImport
    {
        private Mock<IDbSet<Country>> mockCountryDBSet = null;
        private Mock<IWebClientFactory> mockWebClientFactory = null;
        private byte[] responseData = null;

        [SetUp]
        public void SetUp()
        {
            this.responseData = Encoding.UTF8.GetBytes(@"#
#ISO    ISO3    ISO-Numeric fips    Country Capital Area(in sq km)  Population  Continent   tld CurrencyCode    CurrencyName    Phone   Postal Code Format  Postal Code Regex   Languages   geonameid   neighbours  EquivalentFipsCode
AD  AND 020 AN  Andorra Andorra la Vella    468 84000   EU  .ad EUR Euro    376 AD###   ^(?:AD)*(\d{3})$    ca  3041565 ES,FR   
AE  ARE 784 AE  United Arab Emirates    Abu Dhabi   82880   4975593 AS  .ae AED Dirham  971         ar-AE,fa,en,hi,ur   290557  SA,OM   
AF  AFG 004 AF  Afghanistan Kabul   647500  29121286    AS  .af AFN Afghani 93          fa-AF,ps,uz-AF,tk   1149361 TM,CN,IR,TJ,PK,UZ");

            this.mockWebClientFactory = new Mock<IWebClientFactory>();
            var mockWebClient = new Mock<IWebClient>();
            this.mockCountryDBSet = new Mock<IDbSet<Country>>();

            mockWebClient.Setup(x => x.DownloadDataTaskAsync(It.IsAny<Uri>())).ReturnsAsync(this.responseData);
            this.mockWebClientFactory.Setup(x => x.Create()).Returns(mockWebClient.Object);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void ShouldCreateAllRowsOnEmptyDB()
        {
            var countryImport = new CountryImport(this.mockWebClientFactory.Object);
            countryImport.SyncCountries(this.mockCountryDBSet.Object);

            try
            {
                this.mockCountryDBSet.Verify(x => x.Add(It.IsAny<Country>()), Times.Exactly(3));
            }
            catch(MockException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}

