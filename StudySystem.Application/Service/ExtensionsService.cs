using StudySystem.Application.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietnamNumber;

namespace StudySystem.Application.Service
{
    public class ExtensionsService : IExtensionsService
    {
        public ExtensionsService() { }
        public async Task<string> ConvertPriceToWords(long price)
        {
            string output = Number.ToVietnameseWords(price);
            output = price.ToVietnameseWords();
            return output + " đồng";
        }
    }
}
