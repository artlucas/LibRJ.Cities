//  _    _ _    ___    _  ___ _ _   _        
// | |  (_) |__| _ \_ | |/ __(_) |_(_)___ ___
// | |__| | '_ \   / || | (__| |  _| / -_|_-<
// |____|_|_.__/_|_\\__(_)___|_|\__|_\___/__/
//
// Author:
//   Arthur Lucas <arthur@remitjet.com>
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

using System;
using FileHelpers;

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
        public int GeoNameID { get; set; }
        public string Neighbours_A2 { get; set; }
        public string EquivalentFipsCode { get; set; }
    }
}

