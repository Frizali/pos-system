using System.Text;

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
    }
}
