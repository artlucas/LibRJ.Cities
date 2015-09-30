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
using System.IO;
using System.Threading.Tasks;
using BaseWebClient = System.Net.WebClient;

namespace LibRJ.Cities.GeoNames
{
    public interface IWebClient : IDisposable
    {
        // List all members from `System.Net.WebClient` that we need.
        Task<string> DownloadStringTaskAsync(Uri address);
        Task<Stream> OpenReadTaskAsync(Uri address);
    }

    public interface IWebClientFactory
    {
        IWebClient Create();
    }

    public class WebClient : BaseWebClient, IWebClient
    { }

    public class WebClientFactory : IWebClientFactory
    {
        public IWebClient Create()
        {
            return new WebClient();
        }
    }
}

