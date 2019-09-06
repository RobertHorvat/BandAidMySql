using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BandAidMySql.Models;
using BandAidMySql.Models.PomocneKlase;

namespace BandAidMySql.Controllers
{
    public class IzvodacController : Controller
    {
        private bandaidContext _database = new bandaidContext();
        private readonly IHostingEnvironment he;

        public IzvodacController(IHostingEnvironment e)
        {
            he = e;
        }



        [HttpGet]
        public IActionResult Index()
        {

            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
				List<Events> userEvents = _database.Events.Where(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId).ToList();
				List<Events> isti = new List<Events>();
				foreach(Events e in _database.Events)
				{
					foreach(Events ev in userEvents)
					{
						if (e.Name == ev.Name)
						{
							isti.Add(e);
						}
					}
				}
                List<Events> _events = _database.Events.Where(it => it.StatusId == 5 ).ToList();
				foreach(Events e in isti)
				{
					_events.RemoveAll(it => it.Name == e.Name);
				}
				_events.RemoveAll(it => it.StatusId == 4);
                if (!_events.Any())
                {
                    ViewBag.SviEventi = _events;
                    ViewBag.BrojEvenata = 0.ToString();
                   
                }
                else
                {
                    ViewBag.SviEventi = _events;
                    ViewBag.BrojEvenata = _events.Count().ToString();
                   
                }

               
                return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
            }


        }
        [HttpGet]
        public IActionResult Settings()
        {

            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Events> _events = HttpContext.Session.GetObjectFromJson<Users>("user").Events.ToList();
				Users user = _database.Users.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);


                return View(user);
            }


        }
		[HttpPost]
		public IActionResult Settings(Users user, IFormFile Img)
		{
			Users newUser = _database.Users.FirstOrDefault(it=>it.UserId==HttpContext.Session.GetObjectFromJson<Users>("user").UserId);
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
			{
				return RedirectToAction("Index", "Home");
			}

			else if (newUser == user)
			{

				
				
				return View(user);
			}
			else if (user!=newUser)
			{


				try
				{
					string fileName = he.WebRootPath + "\\ProfilePics\\" + Path.GetFileName(Img.FileName);
					using (var fs = new FileStream(fileName, FileMode.Create))
					{
						Img.CopyTo(fs);
					}
					newUser.ProfileImg = "~/ProfilePics/" + Path.GetFileName(Img.FileName);
				}
				catch (Exception)
				{

					
				}
				
				
				newUser.City = user.City;
					newUser.Street = user.Street;
					newUser.PhoneNumber = user.PhoneNumber;
					newUser.Description = user.Description;
					newUser.Youtube = user.Youtube;
					newUser.Facebook = user.Facebook;
					newUser.Instagram = user.Instagram;
					newUser.PostCode = user.PostCode;
				
			}

				_database.Users.Update(newUser);
				_database.SaveChanges();


				return View(newUser);
			}
		


        

        [HttpGet]
        public IActionResult LoggedEvents()
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
				Users user = HttpContext.Session.GetObjectFromJson<Users>("user");
                List<Events> _events = _database.Events.Where(it => it.UserId == user.UserId && (it.StatusId== 2 || it.StatusId==6 || it.StatusId == 1 || it.StatusId == 3) ).ToList();

				if (_events.Count() == 0)
                {
                    ViewBag.NemaEvenata = "Niste pretplaćeni na nijedan event.";
                }
                else
                {
                    ViewBag.Eventi = _events;
                }
                return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
            }
        }

		[HttpGet]
		public IActionResult OverEvents()
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				Users user = HttpContext.Session.GetObjectFromJson<Users>("user");
				List<Events> _events = _database.Events.Where(it => it.UserId == user.UserId && it.StatusId == 4).ToList();

				if (_events.Count() == 0)
				{
					ViewBag.NemaEvenata = "Niste pretplaćeni na nijedan event.";
				}
				else
				{
					ViewBag.Eventi = _events;
				}
				return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
			}
		}
		
    
        [HttpGet]
        public IActionResult Event(string email)
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Users _user = _database.Users.FirstOrDefault(it => it.Email == email);
                List<Events> _events = _database.Events.Where(it => it.UserId == _user.UserId).ToList();

                if (!_events.Any())
                {
                    ViewBag.Eventi = null;
                }
                else
                {
                    ViewBag.Eventi = _events;
                    ViewBag.Korisnik = _user;


                }
                return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
            }
        }

		[HttpGet]
		public IActionResult SaveEvent(int id)
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				Users user = _database.Users.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);
				Events old = _database.Events.FirstOrDefault(it => it.EventId == id);
				Events newEvent = new Events();
				int lastIndex = _database.Events.Max(it => it.EventId);
				newEvent.EventId = lastIndex + 1;
				newEvent.Adress = old.Adress;
				newEvent.Date = old.Date;
				newEvent.Description = old.Description;
				newEvent.ImgUrl = old.ImgUrl;
				newEvent.Name = old.Name;
				newEvent.PhoneNumber = old.PhoneNumber;
				newEvent.StatusId = 2;
				//newEvent.Status = _database.Statuses.FirstOrDefault(it => it.StatusId == 2);
				//newEvent.User = HttpContext.Session.GetObjectFromJson<Users>("user");
				newEvent.UserId = HttpContext.Session.GetObjectFromJson<Users>("user").UserId;

				_database.Events.Add(newEvent);
				_database.SaveChanges();

				
				return RedirectToAction("LoggedEvents",user);
			}
		}

		[HttpGet]
		public IActionResult EventDetails(int id)
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				Users user = _database.Users.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);
				Events ev = _database.Events.FirstOrDefault(it => it.EventId == id);
				ViewBag.Event = ev;

				return View(user);
			}
		}

		[HttpGet]
		public IActionResult DeleteEvent(int id)
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				Users user = _database.Users.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);
				Events old = _database.Events.FirstOrDefault(it => it.EventId == id);


				_database.Events.Remove(old);
				_database.SaveChanges();


				return RedirectToAction("LoggedEvents", user);
			}
		}


	}
}