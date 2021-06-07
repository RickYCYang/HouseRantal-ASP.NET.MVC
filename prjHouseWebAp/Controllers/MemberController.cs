using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjHouseWebAp.Models;

namespace prjHouseWebAp.Controllers
{
    public class MemberController : Controller
    {
        HouseDBEntities db = new HouseDBEntities();
        // GET: Member
        [Authorize]
        public ActionResult Index()
        {
            string userid = User.Identity.Name;
            var house = db.地點名稱.Where(m=>m.帳號==userid).OrderByDescending(m => m.地點編號).ToList();
            return View(house);
        }
        [Authorize]
        public ActionResult Edit()
        {
            string userid = User.Identity.Name;
            var member = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
            return View(member);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(會員 member)
        {
            db.Entry(member).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.EditShow = true;
            return View(member);
        }


        [Authorize]
        public ActionResult HouseCreate()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult HouseCreate
            (string 類型, string 名稱, HttpPostedFileBase fImg, string 說明
            , string 地址)
        {
            string fileName = "question.png";
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    fileName = Guid.NewGuid().ToString() + ".jpg";
                    var path = string.Format("{0}/{1}", Server.MapPath("~/Images"), fileName);
                    fImg.SaveAs(path);
                }
            }

            string userid = User.Identity.Name;

            地點名稱 place = new 地點名稱();
            place.類型 = 類型;
            place.帳號 = userid;
            place.名稱 = 名稱;
            place.照片 = fileName;
            place.說明 = 說明;
            place.地址 = 地址;
            db.地點名稱.Add(place);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult HouseDelete(int id)
        {
            string userid = User.Identity.Name;
            var house = db.地點名稱.Where(m => m.地點編號 == id).FirstOrDefault();
            var houseid = house.地點編號;
            var message = db.評語.Where(m => m.地點編號 == houseid).ToList();
            var filename = house.帳號;
            if (filename != "question.png")
            {
                System.IO.File.Delete
                    (string.Format("{0}/{1}", Server.MapPath("~/Images"), filename));
            }

            db.評語.RemoveRange(message);

            db.地點名稱.Remove(house);
            db.SaveChanges();
            return RedirectToAction("Index");

        }


        [Authorize]
        public ActionResult HouseEdit(int id)
        {
            var house = db.地點名稱.Where(m => m.地點編號 == id).FirstOrDefault();
            return View(house);
        }


        [Authorize]
        [HttpPost]
        public ActionResult HouseEdit(int 地點編號, string 類型, string 名稱, HttpPostedFileBase fImg, string 說明
            , string 地址, string 照片)
        {

            string fileName = "";
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    fileName = Guid.NewGuid().ToString() + ".jpg";
                    var path = string.Format("{0}/{1}", Server.MapPath("~/Images"), fileName);
                    fImg.SaveAs(path);
                }
            }
            else
            {
                fileName = 照片;  //若無上傳圖，則使用hidden隱藏欄位的舊檔名
            }
            string userid = User.Identity.Name;
            var house = db.地點名稱.Where(m => m.地點編號 == 地點編號).FirstOrDefault();
            house.類型 = 類型;
            house.帳號 = userid;
            house.名稱 = 名稱;
            house.照片 = fileName;
            house.說明 = 說明;
            house.地址 = 地址;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult HouseMsg(int id)
        {
            var message = db.評語.Where(m=>m.地點編號==id).OrderByDescending(m => m.評語編號).ToList();
            return View(message);
        }

        [Authorize]
        public ActionResult HouseMsgDelete(int id)
        {
            var message = db.評語.Where(m => m.評語編號 == id).FirstOrDefault();
            var houseid = message.地點編號;
            db.評語.Remove(message);
            db.SaveChanges();
            return RedirectToAction("HouseMsg", new { id = houseid });
        }

    }
}