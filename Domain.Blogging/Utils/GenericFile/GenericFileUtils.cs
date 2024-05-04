using Domain.Blogging.Constant;
using Domain.Blogging.enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.Utils.GenericFile
{
    public class GenericFileUtils
    {
        public Dictionary<string, object> SaveTempFile(IFormFile doc, List<FileType> type)
        {
            try
            {
                if (doc.Length == 0)
                {
                    throw new Exception($"{string.Join(", ", type)} Not Found!!");
                }

                
                // Validate Extension here
                Dictionary<string, object> extension = ValidateExtension(doc, type);

                string imagePath = FilePathConstants.TempPath;
                Directory.CreateDirectory(imagePath);

                string fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}-{Guid.NewGuid()}.{extension["extension"]}";
                string tempFileName = Path.Combine(imagePath, fileName);

                using (var stream = new FileStream(tempFileName, FileMode.Create))
                {
                    doc.CopyTo(stream);
                }

                Dictionary<string, object> map = new Dictionary<string, object>
            {
                { "location", Path.Combine(imagePath, fileName) },
                { "fileType", extension["fileType"] }
            };

                return map;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Dictionary<string, object> ValidateExtension(IFormFile file, List<FileType> fileType)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                string extension = Path.GetExtension(file.FileName);
                Console.WriteLine($"Checking extension --- {extension}");
                ExtensionValidator(null, file.Name, extension);

                if (string.IsNullOrEmpty(extension))
                {
                    throw new Exception("File has no extension");
                }

                string message = $"Invalid Extension {extension} for file type {string.Join(", ", fileType)}";
                bool temp = false;

                foreach (var type in fileType)
                {
                    switch (type)
                    {
                        case FileType.IMAGE:
                            if (new List<string> { ".jpeg", ".jpg", ".png", ".svg" }.Contains(extension.ToLower()))
                            {
                                temp = true;
                                map["fileType"] = FileType.IMAGE;
                            }
                            break;
                        case FileType.DOC:
                            if (new List<string> { ".doc", ".docx" }.Contains(extension.ToLower()))
                            {
                                temp = true;
                                map["fileType"] = FileType.DOC;
                            }
                            break;
                        case FileType.PDF:
                            if (new List<string> { ".pdf" }.Contains(extension.ToLower()))
                            {
                                temp = true;
                                map["fileType"] = FileType.PDF;
                            }
                            break;
                        case FileType.TXT:
                            if (new List<string> { ".txt" }.Contains(extension.ToLower()))
                            {
                                temp = true;
                                map["fileType"] = FileType.TXT;
                            }
                            break;
                        case FileType.EXCEL:
                            if (new List<string> { ".xls", ".xlsx", ".csv", ".ods" }.Contains(extension.ToLower()))
                            {
                                temp = true;
                                map["fileType"] = FileType.EXCEL;
                            }
                            break;
                        default:
                            if (!temp)
                            {
                                throw new Exception(message);
                            }
                            break;
                    }
                }

                map["extension"] = extension;
                return map;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw e;
            }
        }

        private void ExtensionValidator(HttpResponse response, string fileName, string ext)
        {
            if (ext.Equals(FileExtensionType.PNG.GetValue(), StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(FileExtensionType.JPG.GetValue(), StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(FileExtensionType.JPEG.GetValue(), StringComparison.OrdinalIgnoreCase))
            {
                if (response == null) return;

                response.ContentType = $"image/{ext}";
                response.Headers["Content-Disposition"] = $"inline;filename={fileName}";
            }
            else if (ext.Equals(FileExtensionType.PDF.GetValue(), StringComparison.OrdinalIgnoreCase))
            {
                if (response == null) return;

                response.ContentType = $"application/{ext}";
                response.Headers["Content-Disposition"] = $"inline;filename={fileName}";
            }
            else if (ext.Equals(FileExtensionType.XLSX.GetValue(), StringComparison.OrdinalIgnoreCase) ||
                     ext.Equals(FileExtensionType.CSV.GetValue(), StringComparison.OrdinalIgnoreCase) ||
                     ext.Equals(FileExtensionType.XLS.GetValue(), StringComparison.OrdinalIgnoreCase))
            {
                if (response == null) return;

                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";
            }
            else if (ext.Equals(FileExtensionType.DOC.GetValue(), StringComparison.OrdinalIgnoreCase) ||
                     ext.Equals(FileExtensionType.DOCX.GetValue(), StringComparison.OrdinalIgnoreCase))
            {
                if (response == null) return;

                response.ContentType = "application/msword";
                response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";
            }
            else if (ext.Equals(FileExtensionType.PDF.GetValue(), StringComparison.OrdinalIgnoreCase))
            {
                if (response == null) return;

                response.ContentType = "application/pdf";
                response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";
            }
            else
            {
                throw new Exception($"Invalid File Extension {ext}");
            }
        }

        public string CopyFileToServer(string filePath, FilePathMapping moduleName, string fileFromPath)
        {
            string fileName = Path.GetFileName(filePath);
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            string fileFrom = Path.Combine(fileFromPath, fileName);
            string fileTo = Path.Combine(FilePathConstants.UploadDir, moduleName.GetLocation(), date, fileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileTo));

                File.Copy(fileFrom, fileTo, true);

                return fileTo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Failed to copy the file");
            }
        }

        public void GetFileFromFilePath(string filePath, HttpResponse response)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);

                if (!string.IsNullOrEmpty(fileName))
                {
                    var file = new FileInfo(filePath);

                    if (file.Exists)
                    {
                        // Set the response headers
                        response.Headers.Add("Content-Disposition", $"inline;filename={WebUtility.UrlEncode(fileName)}");
                        response.Headers.Add("Content-Type", "application/octet-stream"); // You may need to adjust the content type
                        response.ContentType = "image/jpeg";


                        // Write the file content to the response
                        response.Body.WriteAsync(System.IO.File.ReadAllBytes(file.FullName));
                    }
                    else
                    {
                        throw new FileNotFoundException("Invalid File Path", filePath);
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid File Path");
                }
            }
            catch (Exception e)
            {
                // Handle the exception or log it
                throw new ApplicationException("Error while processing file request", e);
            }
        }
    }
}
