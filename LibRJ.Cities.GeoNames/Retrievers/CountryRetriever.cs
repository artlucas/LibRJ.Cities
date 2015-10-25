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
using FileHelpers;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace LibRJ.Cities.GeoNames.Retrievers
{
    public class CountryRetriever
    {
        public const string DefaultImportURL = "http://download.geonames.org/export/dump/countryInfo.txt";

        private FileHelperEngine<SourceModels.Country> engine;
        private IWebClientFactory webClientFactory;
        private Uri importURL;

        public CountryRetriever(IWebClientFactory webClientFactory=null, Uri importURL=null)
        {
            this.engine = new FileHelperEngine<SourceModels.Country>();

            this.importURL = importURL ?? new Uri(
                ConfigurationManager.AppSettings["GeoNamesCountryImport:ImportURL"]
                ?? DefaultImportURL
            );

            this.webClientFactory = webClientFactory ?? (IWebClientFactory)new WebClientFactory();
        }

        /// <summary>
        /// Gets raw data from the data provider.
        /// </summary>
        /// <returns>The data as a big string.</returns>
        public async Task<string> GetSourceData()
        {
            var webClient = this.webClientFactory.Create();
            var responseData = await webClient.DownloadStringTaskAsync(this.importURL);
            return responseData;
        }

        /// <summary>
        /// Generator which will parse a single line (record) from the sourceData on each iteration.
        /// </summary>
        /// <returns>Enumerable of Country objects.</returns>
        /// <param name="sourceData">Source data from the provider.</param>
        public IEnumerable<SourceModels.Country> GetCountries(string sourceData)
        {
            using (var sourceDataReader = new StringReader(sourceData))
            {
                while (true)
                {
                    var newLine = sourceDataReader.ReadLine();

                    if (newLine == null)
                        yield break;

                    if (newLine.StartsWith("#"))
                        continue;

                    var countries = this.engine.ReadString(newLine);

                    if (countries != null && countries.Length > 0)
                        foreach (var country in countries)
                            yield return country;
                }
            }
        }

    }
}

