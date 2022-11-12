using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCar.API.Mapping;
using RentalCar.API.Models;
using RentalCar.Model.Models;
using RentalCar.Service;

namespace RentalCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IMapper _mapper;
        public CarController(ICarService carService, IMapper mapper)
        {
            _mapper = mapper;
            _carService = carService;
        }

        [HttpGet("Homepage")]
        public ActionResult<List<CarViewDto>> Homepage()
        {
            List<Car> ListCar = _carService.GetCars();
            List<CarViewDto> ListCarView = new List<CarViewDto>();

            foreach(var car in ListCar){
                ListCarView.Add(new CarViewDto{
                    Image = _carService.GetImageByCarId(car.Id),
                    Name = car.Name,
                    Cost = car.Cost,
                    numberStar = car.NumberStar,
                    AddressCar = car.AddressCar
                });
            }
            return Ok(ListCarView);
            // ImageAvt = _CarService.GetImageAvtByCarId()
        }

        [HttpGet("{id}")]
        public ActionResult<CarDetailDto> Get(int id)
        {
            var car = _carService.GetCarById(id);
            if(car == null) return NotFound();

            var carImages = _carService.GetImageByCarId(id);
            var carReviews = _carService.GetCarReviewsByCarId(id);
            var carReviewDtos = new List<CarReviewDto>();

            if(carReviews != null)
            {
                foreach (var carReview in carReviews)
                {
                    carReviewDtos.Add(new CarReviewDto()
                    {
                        Id = carReview.Id,
                        Content = carReview.Content,
                        Rating = carReview.Rating,
                        CreatedAt = carReview.CreatedAt,
                        UpdateAt = carReview.UpdateAt,
                        AccountDto = new AccountDto()
                        {
                            ProfileImage = carReview.User.ProfileImage,
                            Fullname = carReview.User.Fullname
                        }
                    });
                }
            } 

            return Ok(new CarDetailDto()
            {
                Id = car.Id,
                Description = car.Description,
                Capacity = car.Capacity,
                Cost = car.Cost,
                CarModel = car.CarModel.Name,
                CarBrand = car.CarModel.CarBrand.Name,
                Transmission = car.Transmission,
                FuelConsumption = car.FuelConsumption,
                AddressCar = car.AddressCar,
                Rule = car.Rule,
                NumberStar = car.NumberStar,
                CarImageDtos = carImages,
                CarReviewDtos = carReviewDtos
            });
        }
    
        // [HttpDelete("{id}")]
        // public ActionResult<CarDetailDto> Delete(int id)
        // {
        //     var car = _carService.GetCarById(id);
        //     if(car == null) return NotFound();

        //     _carService.DeleteCar(car);
        //     if(_carService.SaveChanges()) return NoContent();
        //     return BadRequest();
        // }
    }
}