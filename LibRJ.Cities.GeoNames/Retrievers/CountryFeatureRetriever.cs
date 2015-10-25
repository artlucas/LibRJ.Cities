//  _    _ _    ___    _  ___ _ _   _        
// | |  (_) |__| _ \_ | |/ __(_) |_(_)___ ___
// | |__| | '_ \   / || | (__| |  _| / -_|_-<
// |____|_|_.__/_|_\\__(_)___|_|\__|_\___/__/
//
// Author(s):
//   Arthur Lucas <arthur@remitjet.com>
//
// Copyright (c) 2015 Remit Jet, Ltd.
//
// By using this software you agree to our software license as detailed in the
// LICENSE.txt file in the root of the repository.  You can also view this file
// online at: https://github.com/RemitJet/LibRJ.Cities/blob/master/LICENSE.txt
//
using System;
using System.Configuration;
using System.IO.Compression;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using FileHelpers;

namespace LibRJ.Cities.GeoNames.Retrievers
{
    public class CountryFeatureRetriever
    {
        public const string DefaultImportURL = "http://download.geonames.org/export/dump/$$.zip";

        private FileHelperEngine<SourceModels.CountryFeature> engine;
        private IWebClientFactory webClientFactory;
        private Uri importURL;

        public CountryFeatureRetriever(IWebClientFactory webClientFactory=null, Uri importURL=null)
        {
            this.engine = new FileHelperEngine<SourceModels.CountryFeature>();

            this.importURL = importURL ?? new Uri(
                ConfigurationManager.AppSettings["GeoNamesCountryFeatureImport:ImportURI"]
                ?? DefaultImportURL
            );

            this.webClientFactory = webClientFactory ?? (IWebClientFactory)new WebClientFactory();
        }

        /// <summary>
        /// Gets raw data from the data provider.
        /// </summary>
        /// <returns>The data as a big string.</returns>
        /// <param name="countryIsoA2">ISO A2 of the country to get regions for.</param>
        public async Task<string> GetSourceData(string countryIsoA2)
        {
            var countryImportUrl = new Uri(this.importURL.ToString().Replace("$$", countryIsoA2));
            var webClient = this.webClientFactory.Create();
            var rawResponse = await webClient.DownloadDataTaskAsync(countryImportUrl);
            var zipArchive = new ZipArchive(new MemoryStream(rawResponse), ZipArchiveMode.Read);
            var dataEntry = zipArchive.GetEntry(countryIsoA2 + ".txt");
            var responseData = await new StreamReader(dataEntry.Open()).ReadToEndAsync();

            return responseData;
        }

        /// <summary>
        /// Generator which will parse a single line (record) from the sourceData on each iteration.
        /// </summary>
        /// <returns>Enumerable of CountryFeature objects.</returns>
        /// <param name="sourceData">Source data from the provider.</param>
        public IEnumerable<SourceModels.CountryFeature> GetCountryFeatures(string sourceData)
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

                    var countryFeatures = this.engine.ReadString(newLine);

                    if (countryFeatures != null && countryFeatures.Length > 0)
                        foreach (var countryFeature in countryFeatures)
                            yield return countryFeature;
                }
            }
        }

    }
}

