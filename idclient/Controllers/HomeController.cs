using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Net;

namespace idclient.Controllers
{
    
    public class HomeController : Controller
    {

        HttpClient client = new HttpClient();
        string activityUrl = "api/Activity";
        string presenceUrl = "api/Presence";

        // constructor for controller
        public HomeController()
        {
            client.BaseAddress = new Uri("http://localhost:60854/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Index()
        {
            try
            {
                if (CheckActivity() && !CheckPresence())
                {
                    ViewBag.Message = "alert-danger";
                }
                else
                {
                    ViewBag.Message = "alert-success";
                }

            } catch (HttpException e)
            {
                return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }

            return View();
        }


        // checks if there is activity in the room
        public bool CheckActivity()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(activityUrl).Result;           
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    if (data.Result == "true")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new HttpException("Cannot get response from Activity API");
                }
            }
            catch (Exception e)
            {
                throw new HttpException("CheckActivity:" + e.Message);
            }
        }

        // checks presence of authorized person in the room
        public bool CheckPresence()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(presenceUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    if (data.Result == "true")
                    {
                        return true;
                    }
                    else
                    {
                        throw new HttpException();
                    }
                }
                else
                {
                    throw new HttpException("Cannot get response from Presence API");
                }
            }
            catch (Exception e)
            {
                throw new HttpException("CheckPresence::" + e.Message);
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "SmartPower Intrusion Detection";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us";

            return View();
        }

    }
}