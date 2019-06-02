using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BandAidMySql.Models;
using BandAidMySql.Models.PomocneKlase;

namespace BandAidMySql.Controllers
{
    public class AdminController : Controller
    {
        private bandaidContext _database = new bandaidContext();



        [HttpGet]
        public IActionResult Index()
        {

            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Events> _events = _database.Events.ToList();
                List<Users> _users = _database.Users.ToList();
                if (!_events.Any())
                {
                    ViewBag.SviEventi = 0.ToString();
                    ViewBag.DostupniEventi = 0.ToString();
                    ViewBag.ZavrseniEventi = 0.ToString();
                }
                else
                {
                    ViewBag.SviEventi = _events.Count()
                                               .ToString();
                    ViewBag.DostupniEventi = _events.Where(it => it.StatusId == 5)
                                                    .Count()
                                                    .ToString();
                    ViewBag.ZavrseniEventi = _events.Where(it => it.StatusId == 4)
                                                    .Count()
                                                    .ToString();
                    ViewBag.Admini = _users.Where(it => it.RoleId == 1)
                                           .Count()
                                           .ToString();
                    ViewBag.Izvodaci = _users.Where(it => it.RoleId == 2)
                                           .Count()
                                           .ToString();
                    ViewBag.Organizatori = _users.Where(it => it.RoleId == 3)
                                           .Count()
                                           .ToString();
                    ViewBag.Potvrdeni = _users.Where(it => it.IsEmailVerified==1)
                                           .Count()
                                           .ToString();
                    ViewBag.Neaktivni = _users.Where(it => it.IsEmailVerified==0)
                                           .Count()
                                           .ToString();
                }

                Users admin = HttpContext.Session.GetObjectFromJson<Users>("user");
                return View(admin);
            }


        }
        [HttpGet]
        public IActionResult Users()
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Users> _users = _database.Users.ToList();
                if (!_users.Any())
                {
                    ViewBag.NemaKorisnika = "Nema korisnika u bazi!";
                }
                else
                {
                    ViewBag.Korisnici = _users;

                }
                return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
            }

        }
        [HttpPost]
        public IActionResult Users(string uloga, string status, string searchString)
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {

                List<Users> _users = new List<Users>();
                if (uloga != null)
                {
                    switch (uloga)
                    {
                        case ("Admin"):
                            foreach (Users r in _database.Users.Where(it => it.RoleId == 1))
                            {
                                _users.Add(r);
                            }
                            break;
                        case ("Izvodac"):
                            foreach (Users r in _database.Users.Where(it => it.RoleId == 2))
                            {
                                _users.Add(r);
                            }
                            break;
                        case ("Organizator"):
                            foreach (Users r in _database.Users.Where(it => it.RoleId == 3))
                            {
                                _users.Add(r);
                            }
                            break;

                        default:
                            break;
                    }
                }
                else if (status != null)
                {
                    switch (status)
                    {
                        case ("True"):
                            foreach (Users r in _database.Users.Where(it => it.IsEmailVerified == 1))
                            {
                                _users.Add(r);
                            }
                            break;
                        case ("False"):
                            foreach (Users r in _database.Users.Where(it => it.IsEmailVerified == 0))
                            {
                                _users.Add(r);
                            }
                            break;

                        default:
                            break;
                    }

                }
                else if (searchString != null)
                {
                    foreach (Users r in _database.Users)
                    {
                        if (r.Name.Contains(searchString) || r.Email.Contains(searchString))
                            _users.Add(r);

                    }

                }
                else
                {
                    _users = _database.Users.ToList();

                }
                ViewBag.Korisnici = _users;
                ViewBag.SearchString = searchString;
                return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
            }

        }
        [HttpGet]
        public IActionResult Events()
        {
            if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Events> _events = _database.Events.ToList();
                List<Users> _users = _database.Users.ToList();
                if (_events.Count()==0)
                {
                    ViewBag.NemaEvenata = "Nema evenata u bazi!";
                    ViewBag.Korisnici = _users;
                }
                else
                {
                    ViewBag.Eventi = _events;
                    ViewBag.Korisnici = _users;


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