using FileApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileApi.Service
{
    public interface IFileService
    {
        public Task<List<HotelDTO>> ConvertHotelInformationsFromCsvToHotelList(string path);
        public Task<string> SaveCsvFile(IFormFile file);

    }
}
