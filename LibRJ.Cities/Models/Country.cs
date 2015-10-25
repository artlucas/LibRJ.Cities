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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibRJ.Cities.Models
{
    public class Country : GeoNameResource
    {
        public int ID { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; }

        [StringLength(2)]
        [Required]
        public string ISO_A2 { get; set; }

        [StringLength(3)]
        [Required]
        public string ISO_A3 { get; set; }

        [StringLength(3)]
        [Required]
        public string ISO_Numeric { get; set; }

        [StringLength(3)]
        public string CurrencyCode { get; set; }

//        [StringLength(10)] -- Needs to be child table, some countries have > 1
//        public string DialingCode { get; set; }

        [StringLength(20)]
        public string PostalCodeFormat { get; set; }

        [StringLength(200)]
        public string PostalCodeRegex { get; set; }

        [StringLength(2)]
        [Required]
        public string Continent { get; set; }

        public ICollection<Region> Regions { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}

