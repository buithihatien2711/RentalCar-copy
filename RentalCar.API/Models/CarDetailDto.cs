using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalCar.API.Models
{
    public class CarDetailDto
    {
       public int Id { get; set; }

        public string? Description { get; set; }
        
        public int Capacity { get; set; }
        
        public decimal Cost { get; set; }
        
        public string? CarModel { get; set; }
        
        public string? CarBrand { get; set; }

        //Truyền động
        public string? Transmission { get; set; }

        //Loại nhiên liệu
        public string? FuelType { get; set; }

        //Mức tiêu thụ nhiên liệu
        public int FuelConsumption { get; set; }

        //AddressCar
        public string? AddressCar { get; set; }

        //Điều khoản
        public string? Rule { get; set; }

        public decimal NumberStar { get; set; }

        public List<string>? CarImageDtos { get; set; }

        public List<CarReviewDto>?  CarReviewDtos { get; set; }
    }
}