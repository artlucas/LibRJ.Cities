//  _    _ _    ___    _  ___ _ _   _        
// | |  (_) |__| _ \_ | |/ __(_) |_(_)___ ___
// | |__| | '_ \   / || | (__| |  _| / -_|_-<
// |____|_|_.__/_|_\\__(_)___|_|\__|_\___/__/
//
// Author:
//   arthur <>
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
using System.Linq;
using FileHelpers;
using System.Configuration;
using LibRJ.Cities.Models;
using System.Data.Entity;

namespace LibRJ.Cities.GeoNames
{
    public class GeoNamesCountryFeatureImport : GeoNamesImportBase
    {
        public const string DefaultImportURL = "http://download.geonames.org/export/dump/$$.zip";

        private FileHelperEngine<SourceModels.Country> engine;
        private IWebClientFactory webClientFactory;
        private Uri importURL;

        public GeoNamesCountryFeatureImport(IWebClientFactory webClientFactory=null, Uri importURL=null)
        {
            this.engine = new FileHelperEngine<SourceModels.Country>();

            this.importURL = importURL ?? new Uri(
                ConfigurationManager.AppSettings["GeoNamesCountryFeatureImport:ImportURI"]
                ?? DefaultImportURL
            );

            this.webClientFactory = webClientFactory ?? (IWebClientFactory)new WebClientFactory();
        }

        public Region TranslateToRegion(SourceModels.CountryFeature source, Country country)
        {
            if (source.FeatureCode != "ADM1")
                return null;
            
            var record = new Region();

            record.Name = source.Name;
            record.CountryID = country.ID;

            return record;
        }

        public Region TranslateToCity(SourceModels.CountryFeature source, Region region)
        {
            if (source.FeatureCode != "PPL")
                return null;

            var record = new City();

            record.Name = source.Name;
            record.CountryID = country.ID;

            return record;
        }
    }
}

