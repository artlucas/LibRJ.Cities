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
using System.Collections.Generic;

namespace LibRJ.Cities.Models
{
    public class ContinentFactory
    {
        private static Continent[] m_Continents = null;

        private static void EnsureContinentsAreSetup()
        {
            if (m_Continents == null)
            {
                m_Continents = new Continent[] {
                    new Continent() { Code = "OC", Name = "Oceania" },
                    new Continent() { Code = "EU", Name = "Europe" },
                    new Continent() { Code = "AF", Name = "Africa" },
                    new Continent() { Code = "NA", Name = "North America" },
                    new Continent() { Code = "AN", Name = "Antartica" },
                    new Continent() { Code = "SA", Name = "South America" },
                    new Continent() { Code = "AS", Name = "Asia" }
                };
            }
        }

        public static Continent GetByCode(string code, bool throwExceptionOnNotFound=false)
        {
            EnsureContinentsAreSetup();

            foreach (var continent in m_Continents)
            {
                if (continent.Code == code)
                    return continent;
            }

            if (throwExceptionOnNotFound == true)
                throw new ArgumentException("Continent code was not found.", "code");

            return null;
        }
    }

    public class Continent
    {
        [StringLength(2)]
        public string Code { get; set; }

        [StringLength(15)]
        public string Name { get; set; }
    }
}

