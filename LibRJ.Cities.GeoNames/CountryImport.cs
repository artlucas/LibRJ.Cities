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
using System.Data.Entity;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using FileHelpers;
using LibRJ.Cities.Models;
using DestModels = LibRJ.Cities.Models;

namespace LibRJ.Cities.GeoNames
{
    public class CountryImport
    {
        private IDbSet<DestModels.Country> entitySet = null;

        public CountryImport(IDbSet<DestModels.Country> entitySet)
        {
            this.entitySet = entitySet;
        }

        protected async Task<bool> IsNewAsync(SourceModels.Country country)
        {
            bool isNew = (
                await this.entitySet.Where(x =>
                    (country.GeoNameID != null && x.GeoNameID == country.GeoNameID)
                ).CountAsync() > 0
            );
            return isNew;
        }

        protected bool IsNew(SourceModels.Country country)
        {
            var task = this.IsNewAsync(country);
            task.Wait();
            return task.Result;
        }

        public DestModels.Country Translate(SourceModels.Country source)
        {
            var newRecord = new DestModels.Country()
            {
                Name = source.CountryName,
                Continent = source.ContinentCode,
                CurrencyCode = source.CurrencyCode,
                GeoNameID = source.GeoNameID,
                ISO_A2 = source.ISO_A2,
                ISO_A3 = source.ISO_A3,
                ISO_Numeric = source.ISO_Numeric,
                PostalCodeFormat = source.PostalCodeFormat,
                PostalCodeRegex = source.PostalCodeRegex
            };
            return newRecord;
        }
    }
}

