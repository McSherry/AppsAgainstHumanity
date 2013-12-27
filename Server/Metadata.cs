using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server
{
    /// <summary>
    /// A class containing server metadata.
    /// </summary>
    public static class Metadata
    {
        /// <summary>
        /// The major version in a version string MAJOR.MINOR.PATCH.
        /// </summary>
        public static int MajorVersion
        {
            get { return 0; }
        }
        /// <summary>
        /// The minor version in a version string MAJOR.MINOR.PATCH.
        /// </summary>
        public static int MinorVersion
        {
            get { return 1; }
        }
        /// <summary>
        /// The patch version in a version string MAJOR.MINOR.PATCH.
        /// </summary>
        public static int PatchVersion
        {
            get { return 0; }
        }
        /// <summary>
        /// The AAH Version Identifier for this version.
        /// </summary>
        public static int VersionIdentifier
        {
            get { return (MajorVersion * 10000) + (MinorVersion * 100) + PatchVersion; }
        }

        /// <summary>
        /// Indicates the error which occurred when attempting to
        /// parse a META from the client.
        /// </summary>
        internal enum MetaStatus
        {
            Success,
            OutdatedServer,
            OutdatedClient,
            MalformedXml
        }
    }
}
