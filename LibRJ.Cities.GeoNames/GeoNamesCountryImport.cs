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

        public async Task<int> SyncCountries(IDbSet<Country> countrySet)
        {
            var webClient = this.webClientFactory.Create();
            int newRecordsCount = 0;

            var response = await webClient.DownloadStringTaskAsync(this.importURL);
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
                    var translatedCountry = sourceCountry.ToCountry();
                    countrySet.Add(translatedCountry);
                    Console.WriteLine("Added: " + translatedCountry.Name);
                    newRecordsCount += 1;
                }
            }

            return newRecordsCount;
        }
    }
}

