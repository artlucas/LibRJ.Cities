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
// LICENSE.txt file in the root of the repository.  You can also access the
// file online at: https://github.com/RemitJet/LibRJ.Cities/blob/master/LICENSE.txt
//
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using DestModels = LibRJ.Cities.Models;

namespace LibRJ.Cities.GeoNames
{
    public class RegionImport
    {
        private IDbSet<DestModels.Region> entitySet = null;

        public RegionImport(IDbSet<DestModels.Region> entitySet)
        {
            this.entitySet = entitySet;
        }

        protected async Task<bool> IsNewAsync(SourceModels.CountryFeature feature)
        {
            bool isNew = (
                await this.entitySet.Where(x =>
                    (feature.GeoNameID != null && x.GeoNameID == feature.GeoNameID)
                ).CountAsync() > 0
            );
            return isNew;
        }

        protected bool IsNew(SourceModels.CountryFeature feature)
        {
            var task = this.IsNewAsync(feature);
            task.Wait();
            return task.Result;
        }

        public DestModels.Region Translate(SourceModels.CountryFeature feature, DestModels.Country parent)
        {
            if (!feature.IsRegion)
                return null;

            var record = new DestModels.Region();

            record.Name = feature.Name;
            record.GeoNameID = feature.GeoNameID;
            record.CountryID = parent.ID;

            return record;
        }
    }
}

