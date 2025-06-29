using Microsoft.AspNetCore.Identity;
using System.Text;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace pos_system.Helpers
{
    public static class Unique
    {
        public static string ID()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GenerateCode(string name, int codeLength)
        {
            var clearedName = name.Replace(" ", "");
            var nameLength = clearedName.Length;

            if (nameLength > codeLength)
            {
                var codeDevide = nameLength / codeLength;
                var rawCode = String.Empty;

                for (var i = 0; i < codeLength * codeDevide; i += codeDevide)
                {
                    rawCode += clearedName[i];
                }

                return rawCode.ToUpper();
            }
            else if (nameLength == codeLength)
            {
                return name.ToUpper();
            }
            else
            {
                return "N/A";
            }
        }

        public static IFormFile CreateFakeFormFile(string fileName, string contentType, string content)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User", "Cashier", "Manager" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static byte[] MergePdfs(byte[] pdf1, byte[] pdf2)
        {
            using (var outStream = new MemoryStream())
            {
                using (var outputDocument = new PdfDocument())
                {
                    using (var ms1 = new MemoryStream(pdf1))
                    {
                        var inputDoc1 = PdfReader.Open(ms1, PdfDocumentOpenMode.Import);
                        foreach (var page in inputDoc1.Pages)
                        {
                            outputDocument.AddPage(page);
                        }
                    }

                    using (var ms2 = new MemoryStream(pdf2))
                    {
                        var inputDoc2 = PdfReader.Open(ms2, PdfDocumentOpenMode.Import);
                        foreach (var page in inputDoc2.Pages)
                        {
                            outputDocument.AddPage(page);
                        }
                    }

                    outputDocument.Save(outStream);
                }

                return outStream.ToArray();
            }
        }
    }
}
