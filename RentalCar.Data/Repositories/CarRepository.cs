using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentalCar.Model.Models;

namespace RentalCar.Data.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly DataContext _context;

        public CarRepository(DataContext context)
        {
            _context = context;
        }
        public Car? GetCarByCarname(string carname)
        {
            return _context.Cars.FirstOrDefault(u => u.Name == carname);
        }

        public Car? GetCarById(int id)
        {         
            return _context.Cars.Include(c => c.CarModel).ThenInclude(m => m.CarBrand)
                    .Include(c => c.Status)
                    .Include(c => c.User)
                    .Include(c => c.CarImages)
                    .Include(c => c.CarReviews).ThenInclude(s => s.User)
                    .FirstOrDefault(u => u.Id == id);
        }

        public List<Car>? GetCars()
        {
            return _context.Cars.ToList();
        }

        public string? GetImageAvtByCarId(int CarId)
        {
            var image = _context.CarImages.FirstOrDefault(u => u.CarId == CarId);
            if(image == null) return null;
            return image.Path;
        }
        public List<CarBrand>? GetCarBrands()
        {
            return _context.CarBrands.ToList();
        }
        public List<CarModel>? GetCarModels()
        {
            return _context.CarModels.ToList();
        }

        public List<District>? GetDistricts()
        {
            return _context.Districts.ToList();
        }

        public List<Ward>? GetWards()
        {
            return _context.Wards.ToList();
        }

        public List<string>? GetImageByCarId(int CarId)
        {
            return _context.CarImages.Where(p=> p.CarId == CarId).Select(p => p.Path).ToList();
        }

        public void CreateCar(Car car)
        {
            _context.Cars.Add(car);
        }

        public void InsertImage(int carid, List<string> CarImage)
        {
            foreach(var carimage in CarImage)
            {
                _context.CarImages.Add(new CarImage{
                    CarId=carid,
                    Path = carimage
                });
            }   
        }

        public Car? GetCarByPateNumber(string Platenumber)
        {
            return _context.Cars.FirstOrDefault(u => u.Plate_number == Platenumber);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }

        public void DeleteCar(Car car)
        {
            _context.Cars.Remove(car);
        }

        public List<CarReview>? GetCarReviewsByCarId(int id)
        {
            return _context.CarReviews.Where(r => r.CarId == id).ToList();
        }

        public List<Car>? GetCarsByUser(int idUser)
        {
            
            return _context.Cars.Include(c => c.User).Where(c => c.UserId == idUser).ToList();
        }
    }
}