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
using System.ComponentModel.DataAnnotations;
using DestModels = LibRJ.Cities.Models;

namespace LibRJ.Cities.GeoNames.SourceModels
{
    public class CountryFeature
    {
        public int GeoNameID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string AsciiName { get; set; }

        [StringLength(10000)]
        public string AlternateNames { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        [StringLength(1)]
        public string FeatureClass { get; set; }

        [StringLength(10)]
        public string FeatureCode { get; set; }

        [StringLength(2)]
        public string CountryCode { get; set; }

        [StringLength(200)]
        public string CountryCodeAlt { get; set; }

        [StringLength(20)]
        public string Admin1Code { get; set; }

        [StringLength(80)]
        public string Admin2Code { get; set; }

        [StringLength(20)]
        public string Admin3Code { get; set; }

        [StringLength(20)]
        public string Admin4Code { get; set; }

        public long? Population { get; set; }
        public int? Elevation { get; set; }     // meters
        public int? ElevationDEM { get; set; }  // digital elevation model, meters

        [StringLength(40)]
        public string TimeZone { get; set; }

        [StringLength(10)]
        public string ModificationDate { get; set; }    // date of last modification in yyyy-MM-dd format

        public bool IsRegion
        {
            get {
                return this.FeatureCode == "ADM1";
            }
        }

        public bool IsCity
        {
            get {
                return this.FeatureCode == "PPL";
            }
        }

        public DestModels.Region ToRegion(DestModels.Country parent)
        {
            if (!this.IsRegion)
                return null;

            var record = new DestModels.Region();

            record.Name = this.Name;
            record.CountryID = parent.ID;

            return record;
        } 

        public DestModels.City ToCity(DestModels.Region parent)
        {
            if (!this.IsCity)
                return null;

            var record = new DestModels.City();

            record.Name = this.AsciiName;
            record.DisplayName = this.Name;
            record.RegionID = parent.ID;
            record.Latitude = this.Latitude;
            record.Longitude = this.Longitude;
            record.Population = (uint)this.Population;

            return record;
        }
    }
}

