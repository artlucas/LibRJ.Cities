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
using System.Linq;

namespace LibRJ.Cities.GeoNames
{
    public class GeoNamesImportBase
    {
        public event GeoNamesImportingEventHandler Importing;

        protected virtual bool OnImporting(bool isNewRecord, GeoNamesRecordType recordType, object incomingRecord)
        {
            if (this.Importing != null)
                this.Importing(this, new GeoNamesImportingEventArgs() { 
                    IsNewRecord = isNewRecord,
                    RecordType = recordType,
                    IncomingRecord = incomingRecord
                });

            return true;
        }

        protected string SanitizeRawData(string rawData)
        {
            var sanitizedData = String.Join(Environment.NewLine, (
                from line in rawData.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                where !line.StartsWith("#")
                select line
            )); // Can't .Trim() as it will break the FileHelpers parsing...

            return sanitizedData;
        }
    }
}

