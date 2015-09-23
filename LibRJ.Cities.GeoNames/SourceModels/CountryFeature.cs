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

        //        population        : bigint (8 byte int) 
        //        elevation         : in meters, integer
        //        dem               : digital elevation model, srtm3 or gtopo30, average elevation of 3''x3'' (ca 90mx90m) or 30''x30'' (ca 900mx900m) area in meters, integer. srtm processed by cgiar/ciat.
        //        timezone          : the timezone id (see file timeZone.txt) varchar(40)
        //        modification date : 

        public long? Population { get; set; }
        public int? Elevation { get; set; }     // meters
        public int? ElevationDEM { get; set; }  // digital elevation model, meters

        [StringLength(40)]
        public string TimeZone { get; set; }

        [StringLength(10)]
        public string ModificationDate { get; set; }    // date of last modification in yyyy-MM-dd format
    }
}

