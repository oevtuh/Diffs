using System;
using DiffingAPITask.BusinessLogic.Services.Interfaces;

namespace DiffingAPITask.BusinessLogic.Services
{
    public class DataValidationService : IDataValidationService
    {
        public bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer , out int bytesParsed);
        }
    }
}