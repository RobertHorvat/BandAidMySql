using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BandAidMySql.Models;
using BandAidMySql.Models.PomocneKlase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace BandAidMySql.Controllers
{
	public class OrganizatorController : Controller
	{
		private bandaidContext _database = new bandaidContext();
		private readonly IHostingEnvironment he;

		public OrganizatorController(IHostingEnvironment e)
		{
			he = e;
		}


		[HttpGet]
		public IActionResult Index()
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 3)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				Users user = _database.Users.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);

				ViewBag.User = user;
				return View(new Events());
			}

		}

		[HttpGet]
		public IActionResult MyEvents()
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 3)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				List<Events> _events = _database.Events.Where(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId).ToList();


				if (!_events.Any())
				{
					ViewBag.Eventi = _events;
					ViewBag.BrojEvenata = 0.ToString();

				}
				else
				{
					ViewBag.Eventi = _events;
					ViewBag.BrojEvenata = _events.Count().ToString();

				}


				return View(HttpContext.Session.GetObjectFromJson<Users>("user"));
			}

		}

		[HttpGet]
		public IActionResult MyArtists()
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 3)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				List<KorisnikEventa> _users = new List<KorisnikEventa>();
				List<Events> eventiUsera = _database.Events.Where(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId).ToList();
				foreach (Events e in _database.Events)
				{
					foreach (Events ev in eventiUsera)
					{
						if (e.Name == ev.Name && e.UserId != ev.UserId)
						{
							Users user = _database.Users.FirstOrDefault(it => it.UserId == e.UserId);
							KorisnikEventa korisnik = new KorisnikEventa();
							korisnik.Adress = user.Street + ", " + user.City;
							korisnik.Email = user.Email;
							korisnik.Name = user.Name;
							korisnik.EventName = e.Name;
							korisnik.UserId = user.UserId;
							korisnik.Status = _database.Statuses.FirstOrDefault(it => it.StatusId == e.StatusId).Name;
							_users.Add(korisnik);
						}
					}
				}


				ViewBag.User = _database.Users.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);

				return View(_users);
			}

		}
		[HttpGet]
		public IActionResult Confirm(int id, string eventName)
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 3)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				List<KorisnikEventa> _users = new List<KorisnikEventa>();
				Events changedEvent = _database.Events.FirstOrDefault(it => it.UserId == id && it.Name == eventName);
				changedEvent.StatusId = _database.Statuses.FirstOrDefault(it => it.StatusId == 1).StatusId;
				_database.Update(changedEvent);

				Events baseEvent = _database.Events.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId && it.Name == eventName);
				baseEvent.StatusId = _database.Statuses.FirstOrDefault(it => it.StatusId == 4).StatusId;
				_database.Update(baseEvent);

				List<Events> eventiUsera = _database.Events.Where(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId).ToList();
				foreach (Events e in _database.Events)
				{
					foreach (Events ev in eventiUsera)
					{
						if (e.Name == ev.Name && e.UserId != ev.UserId && e.UserId != id)
						{
							e.StatusId = 3;
							_database.Update(e);


						}

						

					}
				}

				_database.SaveChanges();

				return RedirectToAction("MyArtists", "Organizator");
			}

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(Events e, IFormFile Img)
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 3)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				Users user = _database.Users.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);
				Events newEvent = e;
				if (ModelState.IsValid)
				{


					try
					{
						try
						{
							string fileName = he.WebRootPath + "\\EventPics\\" + Path.GetFileName(Img.FileName);
							using (var fs = new FileStream(fileName, FileMode.Create))
							{
								Img.CopyTo(fs);
							}
							newEvent.ImgUrl = "~/EventPics/" + Path.GetFileName(Img.FileName);
						}
						catch (Exception)
						{


						}


						newEvent.Adress = e.Adress;
						newEvent.Date = e.Date;
						newEvent.Description = e.Description;
						newEvent.Name = e.Name;
						newEvent.PhoneNumber = user.PhoneNumber;
						newEvent.UserId = user.UserId;
						newEvent.StatusId = 5;


						if (_database.Events.Count() == 0)
						{
							newEvent.EventId = 1;
						}
						else
						{
							newEvent.EventId = _database.Events.Last().EventId + 1;
						}

						_database.Events.Add(newEvent);
						//Obavezno
						_database.SaveChanges();


						return RedirectToAction("MyEvents");
					}
					catch (Exception)
					{

						return RedirectToAction("Index");
					}
				}
				else
				{
					

					ViewBag.User = user;
					return View(newEvent);
				}
			}
		}

		[HttpPost]
		public JsonResult DeleteEvent([FromBody]Events _event)
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 3)
			{
				return Json(null);
			}
			else
			{
				Events e = _database.Events.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);
				try
				{

					foreach (Events ev in _database.Events)
					{
						if (ev.Name == e.Name)
						{
							ev.StatusId = 6;
						}
					}
					_database.Events.Remove(e);
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
		public JsonResult SaveStatus([FromBody]Events _event)
		{
			if (HttpContext.Session.GetObjectFromJson<Users>("user") == null || HttpContext.Session.GetObjectFromJson<Users>("user").RoleId != 3)
			{
				return Json(null);
			}
			else
			{
				Events e = _database.Events.FirstOrDefault(it => it.UserId == HttpContext.Session.GetObjectFromJson<Users>("user").UserId);
				try
				{

					foreach (Events ev in _database.Events)
					{
						if (ev.Name == e.Name)
						{
							ev.StatusId = 6;
						}
					}
					_database.Events.Remove(e);
					_database.SaveChanges();
					return Json(true);
				}
				catch
				{

					return Json(false);
				}

			}
		}
	}
}
