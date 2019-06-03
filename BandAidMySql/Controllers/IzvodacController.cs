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
                List<Events> _events = _database.Events.Where(it => it.StatusId == 5).ToList();
                
                if (!_events.Any())
                {
                    ViewBag.SviEventi = "Nema evenata u bazi!";
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

                if (!_events.Any())
                {
                    ViewBag.SviEventi = "Niste pretplaceni na nijedan event";
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
        [HttpPost]
        public IActionResult Settings(Users user,IFormFile Img)
        {

            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Users newUser = HttpContext.Session.GetObjectFromJson<Users>("user");
                if(newUser.PhoneNumber!=user.PhoneNumber)
                {
                    newUser.PhoneNumber = user.PhoneNumber;
                }
                else if(newUser.Street!=user.Street)
                {
                    newUser.Street = user.Street;
                }
                else if(newUser.PostCode!=user.PostCode)
                {
                    newUser.PostCode = user.PostCode;
                }
                else if(newUser.City!=user.City)
                {
                    newUser.City = user.City;
                }
                else if(newUser.Youtube!=user.Youtube)
                {
                    newUser.Youtube = user.Youtube;
                }
                else if(newUser.Instagram!=user.Instagram)
                {
                    newUser.Instagram = user.Instagram;
                }
                else if(newUser.Facebook!=user.Facebook)
                {
                    newUser.Facebook = user.Facebook;
                }
                else if(newUser.Description!=user.Description)
                {
                    newUser.Description = user.Description;
                }
                else if(Img!=null)
                {
                    var fileName = Path.Combine(he.WebRootPath, "\\ProfilePics\\", Path.GetFileName(Img.FileName));
                    using (var fs = new FileStream(fileName, FileMode.Create))
                    {
                        Img.CopyTo(fs);
                    }
                    newUser.ProfileImg = "~/ProfilePics/"+ Path.GetFileName(Img.FileName);
                }

                _database.Users.Update(newUser);
                _database.SaveChanges();


                return View(newUser);
            }


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
                List<Events> _events = HttpContext.Session.GetObjectFromJson<Users>("user").Events.ToList();
                
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
        [HttpPost]
        public IActionResult Events(string status, string searchString)
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {

                List<Events> _events = new List<Events>();
                List<Users> _users = _database.Users.ToList();

                if (status != null)
                {
                    switch (status)
                    {
                        case ("Zatvoreno"):
                            foreach (Events r in _database.Events.Where(it => it.StatusId == 4))
                            {
                                _events.Add(r);
                            }
                            break;
                        case ("Otvoreno"):
                            foreach (Events r in _database.Events.Where(it => it.StatusId == 5))
                            {
                                _events.Add(r);
                            }
                            break;

                        default:
                            break;
                    }

                }
                else if (searchString != null)
                {
                    foreach (Events r in _database.Events)
                    {
                        if (r.Name.Contains(searchString) || r.Adress.Contains(searchString))
                            _events.Add(r);

                    }

                }
                else
                {
                    _events = _database.Events.ToList();

                }
                ViewBag.Eventi = _events;
                ViewBag.Korisnici = _users;
                ViewBag.SearchString = searchString;
                return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
            }

        }

        [HttpPost]
        public JsonResult DeleteUser([FromBody]Users _user)
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {
                return Json(null);
            }
            else
            {
                Users user = _database.Users.FirstOrDefault(it => it.Email == _user.Email);
                try
                {
                    _database.Users.Remove(user);
                    _database.SaveChanges();
                    return Json(true);
                }
                catch
                {

                    return Json(false);
                }

            }
        }
        [HttpPost]
        public JsonResult DeleteEvent([FromBody]Events _event)
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {
                return Json(null);
            }
            else
            {
                Events _Event = _database.Events.FirstOrDefault(it => it.Name == _event.Name);
                try
                {
                    _database.Events.Remove(_Event);
                    _database.SaveChanges();
                    return Json(true);
                }
                catch
                {

                    return Json(false);
                }

            }
        }
        //[HttpPost]
        //public JsonResult UserProcess([FromBody]Users user)
        //{
        //    if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
        //    {
        //        return Json(null);
        //    }
        //    else
        //    {
        //        Users _user = _database.Users.FirstOrDefault(it => it.Email == user.Email);
        //        try
        //        {

        //            return Json(_user);
        //        }
        //        catch
        //        {

        //            return Json(false);
        //        }

        //    }
        //}

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


        public IActionResult UserView(int id)
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {
                    //int Id = Int32.Parse(id);
                    Users _user = _database.Users.FirstOrDefault(it => it.UserId == id);
                    List<Events> _events = _database.Events.Where(it => it.UserId == _user.UserId).ToList();

                    if (!_events.Any())
                    {
                        ViewBag.Korisnik = _user;
                        ViewBag.Eventi = null;
                    }
                    else
                    {
                        ViewBag.Eventi = _events;
                        ViewBag.Korisnik = _user;


                    }
                    return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
                }
                catch
                {

                    return RedirectToAction("Users", "Admin");
                }

            }
        }
    }
}