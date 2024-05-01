using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.enums
{
    public enum FileExtensionType
    {
        PNG = 1,
        JPEG = 2,
        JPG = 3,
        DOCX = 4,
        DOC = 5,
        PDF = 6,
        PPT = 7,
        PPTX = 8,
        XLSX = 9,
        XLS = 10,
        CSV = 11,
        ODS = 12,
        TXT = 13,
        SVG = 14
    }

    public static class FileExtensionTypeExtensions
    {
        public static string GetValue(this FileExtensionType fileExtensionType)
        {
            switch (fileExtensionType)
            {
                case FileExtensionType.PNG: return ".png";
                case FileExtensionType.JPEG: return ".jpeg";
                case FileExtensionType.JPG: return ".jpg";
                case FileExtensionType.DOCX: return ".docx";
                case FileExtensionType.DOC: return ".doc";
                case FileExtensionType.PDF: return ".pdf";
                case FileExtensionType.PPT: return ".ppt";
                case FileExtensionType.PPTX: return ".pptX";
                case FileExtensionType.XLSX: return ".xlsx";
                case FileExtensionType.XLS: return ".xls";
                case FileExtensionType.CSV: return ".csv";
                case FileExtensionType.ODS: return ".ods";
                case FileExtensionType.TXT: return ".txt";
                case FileExtensionType.SVG: return ".SVG";
                default: throw new ArgumentOutOfRangeException(nameof(fileExtensionType), fileExtensionType, null);
            }
        }
    }
}
