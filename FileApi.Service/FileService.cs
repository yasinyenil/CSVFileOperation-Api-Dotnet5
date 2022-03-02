using CsvHelper;
using FileApi.Models;
using FileApi.Service.Config;
using FileApi.Service.Validation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileApi.Service
{
    public class FileService : AutoMapperService, IFileService
    {

        public async Task<List<HotelDTO>> ConvertHotelInformationsFromCsvToHotelList(string path)
        {
            List<Hotel> validHotels = new();
            HotelValidator hotelValidator = new HotelValidator();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var hotel = new Hotel
                    {
                        Name = csv.GetField("name"),
                        Address = csv.GetField("address"),
                        Stars = csv.GetField<int>("stars"),
                        Phone = csv.GetField("phone"),
                        Contact = csv.GetField("contact"),
                        URI = csv.GetField("uri"),

                    };

                    ValidationResult result = hotelValidator.Validate(hotel);
                    if (result.IsValid) 
                        validHotels.Add(hotel);

                }
            }

            List<HotelDTO> hotelDTO = Mapper.Map<List<HotelDTO>>(validHotels);

            //The parameter can set in front end 
            await WriteFile(hotelDTO, "json");
            //await WriteFile(hotelDTO, "xml");

            return hotelDTO;
        }

        public async Task<string> SaveCsvFile(IFormFile file)
        {
            return CheckCsvFile(file) ? await WriteFile(file) : "";
        }

        private bool CheckCsvFile(IFormFile file)
        {

            var supportedTypes = new[] { "csv"};
            var fileExt = Path.GetExtension(file.FileName).Substring(1);

            if (supportedTypes.Contains(fileExt)) return true;
            return false;
        }

        //This method can be static in another helper file 
        protected async Task<string> WriteFile(IFormFile file)
        {
            bool isSaveSuccess = false;
            string fileName;
            string path = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[^1];
                fileName = "hotel" + extension;

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                   fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                isSaveSuccess = true;
            }
            catch (Exception e)
            {
                //log error
            }

            return isSaveSuccess ? path : "";
        }

        //This method can be static in another helper file 
        protected async Task WriteFile(List<HotelDTO> hotels, string fileType)
        {
            //In here it can change the state with fileType
            //

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                   "ValidHotel." + fileType);

            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }

            if (fileType == "json")
            {
                await using FileStream createStream = File.Create(path);
                await JsonSerializer.SerializeAsync(createStream, hotels);
                
            }
            else if (fileType == "xml")
            {
                await using FileStream createStream = File.Create(path);
                XmlSerializer serializer = new XmlSerializer(typeof(List<HotelDTO>));
                //TextWriter writer = new StreamWriter(filename);

                serializer.Serialize(createStream, hotels);
            }
        }
    }
}
