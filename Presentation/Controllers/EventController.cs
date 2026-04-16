using Common.CustomExceptions;
using Common.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.ActionFilters;
using System.Net;
using System.Xml;

namespace Presentation.Controllers
{
    public class EventController : Controller
    {
        private EventsRepository _eventsRepository;
        public EventController(EventsRepository eventsRepository) {
         _eventsRepository = eventsRepository;
        }
        public IActionResult Index()
        {
            var list = _eventsRepository.GetAllEvents().Where(e => e.Public == true).Select(e=> new Event
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                MaximumTickets = e.MaximumTickets,
                FilePath = e.FilePath,
            }).ToList();

            return View(list);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //generate a server side nonce token and validate it with 
                                   //the token that has to be generated on the client side as well
        [Authorize]
        public IActionResult Create(Event e, IFormFile file, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                e.FilePath = "";
                if (file != null)
                {
                    //validation for file

                    //#1
                    if (file.Length > 5 * 1024 * 1024) // Limit file size to 5MB
                    {
                        ModelState.AddModelError("File", "File size cannot exceed 5MB.");
                        return View(e);
                    }

                    //#2
                    string[] whitelistOfFileExtensions = new[] { ".jpg", ".jpeg", ".png" };

                    bool failedCheck = false;
                    int counter = 0;
                    do
                    {
                        if (Path.GetExtension(file.FileName).ToLower() == whitelistOfFileExtensions[counter])
                        {
                            failedCheck = false;
                            break;
                        }
                        else
                        {
                            failedCheck = true;
                        }

                        counter++;

                    } while (failedCheck == true && counter < whitelistOfFileExtensions.Length);

                    if (failedCheck == true)
                    {
                        ModelState.AddModelError("File", "Only .jpg, .jpeg, and .png files are allowed.");
                        return View(e);
                    }

                    //# 3 - checking the file signature
                    byte[] jpgFileSignature = new byte[] { 255, 216 }; //FF D8	
                    byte[] pngFileSignature = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }; //89 50 4E 47 0D 0A 1A 0A

                    bool jpgSignatureMatch = true;
                    bool pngSignatureMatch = true;

                    using (var fileStream = file.OpenReadStream())
                    {
                        byte[] fileSignature = new byte[jpgFileSignature.Length];
                        fileStream.Read(fileSignature, 0, fileSignature.Length);

                        if (!fileSignature.SequenceEqual(jpgFileSignature)
                        )
                        {
                            jpgSignatureMatch = false;
                        }

                        fileStream.Position = 0; //resetting position to 0

                        byte[] fileSignature2 = new byte[pngFileSignature.Length];
                        fileStream.Read(fileSignature2, 0, fileSignature2.Length);

                        if (!fileSignature2.SequenceEqual(pngFileSignature)
                        )
                        {
                            pngSignatureMatch = false;
                        }
                    }

                    if (jpgSignatureMatch == false && pngSignatureMatch == false)
                    {
                        ModelState.AddModelError("File", "The file signature does not match the expected format for .jpg or .png files.");
                        return View(e);
                    }

                    string uploadsFolder = "";
                    if (e.Public == true)
                    {
                        //this saves in wwwroot = public
                        uploadsFolder = Path.Combine(host.WebRootPath, "uploads");
                    }
                    else
                    {
                        //this saves outside the wwwroot/uploads = private
                        uploadsFolder = Path.Combine(host.ContentRootPath, "uploads");
                    }

                    Directory.CreateDirectory(uploadsFolder); // Ensure the uploads folder exists

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    e.FilePath = "/uploads/" + uniqueFileName; // Store the relative path to the file
                }

                e.Organiser = User.Identity.Name;


                //exclude the filepath and the organizer from the validated properties 
                //since we're taking care of the assignment of their values manually here in this action
                ModelState.Remove("FilePath");
                ModelState.Remove("Organiser");


                if (!ModelState.IsValid) //if you forget this, there's a chance that if the attacker
                                         //bypasses the page, then the validators you had on the page
                                         //become useless, hence we need server side validation.
                {
                    return View(e);
                }

                //validate
                //sanitize



                if (e.Name.Contains("<") || e.Name.Contains(">"))
                {
                    ModelState.AddModelError("Name", "Event name cannot contain HTML tags.");
                    return View(e);
                }

                e.Name = WebUtility.HtmlEncode(e.Name);

                _eventsRepository.CreateEvent(e);
                TempData["success"] = "Event created successfully!";
            }
            catch (CreateEventException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(e);
            }
            catch(Exception ex)
            {
                // log the exception

                // inform the user
                ModelState.AddModelError("", "An error occurred while creating the event. Please try again.");
                return View(e);
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "organizer")] //distinguish between an authenticated user and not
        //in case you need to verify whether user who is an organizer is actually the organizer
        //of the event about to be deleted => we have to use and apply an Authorization Filter
        [HasEventOrganizerPermission]
        public IActionResult Delete(int id)
        {
            _eventsRepository.DeleteEvent(id);
            TempData["success"] = "Event deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
