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
// online at: https://github.com/RemitJet/LibRJ.Cities
//
using System;
using System.Configuration;
using System.IO.Compression;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.IO;
using FileHelpers;
using DestModels = LibRJ.Cities.Models;

namespace LibRJ.Cities.GeoNames
{
    public class GeoNamesCountryFeatureImport : GeoNamesImportBase
    {
        public const string DefaultImportURL = "http://download.geonames.org/export/dump/$$.zip";

        private FileHelperEngine<SourceModels.CountryFeature> engine;
        private IWebClientFactory webClientFactory;
        private Uri importURL;

        public GeoNamesCountryFeatureImport(IWebClientFactory webClientFactory=null, Uri importURL=null)
        {
            this.engine = new FileHelperEngine<SourceModels.CountryFeature>();

            this.importURL = importURL ?? new Uri(
                ConfigurationManager.AppSettings["GeoNamesCountryFeatureImport:ImportURI"]
                ?? DefaultImportURL
            );

            this.webClientFactory = webClientFactory ?? (IWebClientFactory)new WebClientFactory();
        }

        protected async Task<bool> IsNewRegion(IDbSet<DestModels.Region> regionSet, SourceModels.CountryFeature feature)
        {
            bool isNew = (
                await regionSet.Where(x =>
                    (feature.GeoNameID != null && x.GeoNameID == feature.GeoNameID)
                ).CountAsync() > 0
            );
            return isNew;
        }

        protected async Task<bool> IsNewCity(IDbSet<DestModels.City> citySet, SourceModels.CountryFeature feature)
        {
            bool isNew = (
                await citySet.Where(x => 
                    (feature.GeoNameID != null && x.GeoNameID == feature.GeoNameID)
                ).CountAsync() > 0
            );
            return isNew;
        }

        protected async Task<string> GetSourceData(string countryIsoA2)
        {
            var countryImportUrl = new Uri(this.importURL.ToString().Replace("$$", countryIsoA2));
            var webClient = this.webClientFactory.Create();
            var responseStream = await webClient.OpenReadTaskAsync(countryImportUrl);
            var zipArchive = new ZipArchive(responseStream, ZipArchiveMode.Read);
            var dataEntry = zipArchive.GetEntry(countryIsoA2 + ".txt");
            var responseData = await new StreamReader(dataEntry.Open()).ReadToEndAsync();
            responseData = this.SanitizeRawData(responseData);

            return responseData;
        }

        public async Task<int> SyncRegions(IDbSet<DestModels.Region> regionSet, DestModels.Country parent)
        {
            SourceModels.CountryFeature[] countryFeatures = null;

            try
            {
                var responseData = await this.GetSourceData(parent.ISO_A2);
                countryFeatures = this.engine.ReadString(responseData);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            int newRecordsCount = 0;

            foreach (var feature in countryFeatures)
            {
                if (!feature.IsRegion)
                    continue;
                
                bool isNew = await this.IsNewRegion(regionSet, feature);
                bool shouldImport = this.OnImporting(isNew, GeoNames.GeoNamesRecordType.Region, feature);

                if (isNew && shouldImport)
                {
                    var newRecord = feature.ToRegion(parent);
                    regionSet.Add(newRecord);
                    Console.WriteLine("Added: " + newRecord.Name + ", " + feature.CountryCode);
                    newRecordsCount += 1;
                }
            }

            return newRecordsCount;
        }
    }
}

