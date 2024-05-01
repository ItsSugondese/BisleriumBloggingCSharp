using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.Utils.GenericFile
{
    public enum FilePathMapping
    {
        BLOG_PICTURE,
        ADD_INS

        // Add more enum values as needed
    }

    public static class FilePathMappingExtensions
    {
        // Properties to get paths and locations
        public static string GetPath(this FilePathMapping mapping)
        {

            return $"{GetImage()}{Path.DirectorySeparatorChar}post-file{Path.DirectorySeparatorChar}{GetFile()}{Path.DirectorySeparatorChar}";
        }

        public static string GetLocation(this FilePathMapping mapping)
        {
            return $"{mapping.ToString().ToLower()}{Path.DirectorySeparatorChar}{mapping.ToString().ToLower()}-image{Path.DirectorySeparatorChar}";
        }

        // Static methods to get path segments
        private static string GetProfile()
        {
            return "profile";
        }

        private static string GetTemporary()
        {
            return "temporary";
        }

        private static string GetUser()
        {
            return "user";
        }

        private static string GetFile()
        {
            return "file";
        }

        private static string GetLogo()
        {
            return "logo";
        }

        private static string GetCompany()
        {
            return "company";
        }

        private static string GetImage()
        {
            return "image";
        }
    }
}
