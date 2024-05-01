using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.Constant
{
    public class FilePathConstants
    {
        public static readonly string FileSeparator = Path.DirectorySeparatorChar.ToString();
        public static readonly string ProjectPath = Directory.GetCurrentDirectory();
        public static readonly string ProjectName = new DirectoryInfo(ProjectPath).Name;
        public static readonly string PresentDir = ProjectPath.Substring(0, ProjectPath.LastIndexOf(FileSeparator, StringComparison.Ordinal));

        public static readonly string UploadDir = Path.Combine(PresentDir, "blog-document", "fyp");
        public static readonly string TempPath = Path.Combine(PresentDir, "blogtempdocument", "doc");
    }
}
