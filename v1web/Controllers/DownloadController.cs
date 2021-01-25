using Microsoft.AspNet.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using v1web.Models;

namespace v1web.Controllers
{
    [Authorize]
    public class DownloadController : Controller
    {
        public static List<CancelationModel> _cancelationModels = new List<CancelationModel>();
        HttpClient client = new HttpClient();

        private CancelationModel UserCancelationModel
        {
            get
            {
                return _cancelationModels.FirstOrDefault(x => x.ActionType == ActionTypeEnum.GetPage
                && x.UserId == User.Identity.GetUserId());
            }
        }

        // GET: Download
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> DownloadPages(List<string> pages)
        {
            if (UserCancelationModel == null)
                _cancelationModels.Add(new CancelationModel()
                {
                    ActionType = ActionTypeEnum.GetPage,
                    UserId = User.Identity.GetUserId(),
                });

            var task = await GetPages(pages);

            ViewBag.PageData = String.Join("<br>", task);
            return View("Result");
        }

        public ActionResult CancelDownload()
        {
            UserCancelationModel?.TokenSource.Cancel();

            return new JsonResult();
        }

        private async Task<List<string>> GetPages(List<string> pages)
        {
            List<string> outputList = new List<string>();
            CancellationToken token = UserCancelationModel.TokenSource.Token;

            foreach (var page in pages)
            {
                try
                {
                    outputList.Add(await client.GetStringAsync(page));
                }
                catch
                {
                    Console.WriteLine("Something wrong");
                }
                Thread.Sleep(1000);

                if (token.IsCancellationRequested)
                {
                    _cancelationModels.Remove(UserCancelationModel);
                    outputList = new List<string>();
                    outputList.Add("REQUEST IS CANCELED");
                    return outputList;
                }
            }

            _cancelationModels.Remove(UserCancelationModel);
            return outputList;
        }
    }
}