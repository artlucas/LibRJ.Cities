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
using FileHelpers;
using DestModels = LibRJ.Cities.Models;

namespace LibRJ.Cities.GeoNames.SourceModels
{
    [DelimitedRecord("\t")]
    public class Country
    {
        public string ISO_A2 { get; set; }

        public string ISO_A3 { get; set; }

        public string ISO_Numeric { get; set; }

        public string Fips { get; set; }

        public string CountryName { get; set; }

        public string CapitalCityName { get; set; }

        public string AreaSqKm { get; set; }

        public string Population { get; set; }

        public string ContinentCode { get; set; }

        public string CCTLD { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencyName { get; set; }

        public string DialingCode { get; set; }

        public string PostalCodeFormat { get; set; }

        public string PostalCodeRegex { get; set; }

        public string Locales { get; set; }

        public int? GeoNameID { get; set; }
        // Some entries do not have one -- ie: Serbia
        public string Neighbours_A2 { get; set; }

        public string EquivalentFipsCode { get; set; }
    }
}

