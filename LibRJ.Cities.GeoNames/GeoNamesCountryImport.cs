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
using System.Configuration;
using System.Data.Entity;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using FileHelpers;
using LibRJ.Cities.Models;
using SourceModels = LibRJ.Cities.GeoNames.SourceModels;

namespace LibRJ.Cities.GeoNames
{
    public class GeoNamesCountryImport : GeoNamesImportBase
    {
        public const string DefaultImportURL = "http://download.geonames.org/export/dump/countryInfo.txt";

        private FileHelperEngine<SourceModels.Country> engine;
        private IWebClientFactory webClientFactory;
        private Uri importURL;

        public GeoNamesCountryImport(IWebClientFactory webClientFactory=null, Uri importURL=null)
        {
            this.engine = new FileHelperEngine<SourceModels.Country>();

            this.importURL = importURL ?? new Uri(
                ConfigurationManager.AppSettings["GeoNamesCountryImport:ImportURL"]
                ?? DefaultImportURL
            );
            
            this.webClientFactory = webClientFactory ?? (IWebClientFactory)new WebClientFactory();
        }

        public Country TranslateCountry(SourceModels.Country sourceCountry)
        {
            var newCountry = new Country();

            newCountry.ISO_A2 = sourceCountry.ISO_A2;
            newCountry.ISO_A3 = sourceCountry.ISO_A3;
            newCountry.ISO_Numeric = sourceCountry.ISO_Numeric;
            newCountry.Name = sourceCountry.CountryName;
            newCountry.Continent = sourceCountry.ContinentCode;
            newCountry.GeoNameID = sourceCountry.GeoNameID;
            newCountry.CurrencyCode = sourceCountry.CurrencyCode;   // TODO: add event on each country read?
            //newCountry.DialingCode = sourceCountry.DialingCode;
            newCountry.PostalCodeFormat = sourceCountry.PostalCodeFormat;
            newCountry.PostalCodeRegex = sourceCountry.PostalCodeRegex;

            return newCountry;
        }

        public async Task<int> SyncCountries(IDbSet<Country> countrySet)
        {
            var webClient = this.webClientFactory.Create();
            int newRecordsCount = 0;

            var response = await webClient.DownloadStringTaskAsync(this.countryInfo);
            response = this.SanitizeRawData(response);

            SourceModels.Country[] sourceCountries = null;

            try
            {
                sourceCountries = this.engine.ReadString(response);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            foreach (var sourceCountry in sourceCountries)
            {
                bool isNew = (
                    await countrySet.Where(x => 
                        (sourceCountry.GeoNameID != null && x.GeoNameID == sourceCountry.GeoNameID) ||
                        (sourceCountry.ISO_A2 != null && x.ISO_A2 == sourceCountry.ISO_A2) ||
                        (sourceCountry.ISO_A3 != null && x.ISO_A3 == sourceCountry.ISO_A3) ||
                        (sourceCountry.ISO_Numeric != null && x.ISO_Numeric == sourceCountry.ISO_Numeric) ||
                        (sourceCountry.CountryName == x.Name)).CountAsync() == 0
                );

                bool shouldImport = this.OnImporting(isNew, GeoNames.GeoNamesRecordType.Country, sourceCountry);

                if (isNew && shouldImport)
                {
                    var translatedCountry = this.TranslateCountry(sourceCountry);
                    countrySet.Add(translatedCountry);
                    Console.WriteLine("Added: " + translatedCountry.Name);
                    newRecordsCount += 1;
                }
            }

            return newRecordsCount;
        }
    }
}

