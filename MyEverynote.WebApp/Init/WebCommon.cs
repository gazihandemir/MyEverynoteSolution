using MyEvernote.Common;
using MyEvernote.Entities;
using MyEverynote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverynote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            /*   if(HttpContext.Current.Session["Login"] != null)
               {
                   EvernoteUser user = HttpContext.Current.Session["Login"] as EvernoteUser;
                   return user.UserName;
               }*/
            EvernoteUser user = CurrentSession.User;
            if (user != null)
            {
                return user.UserName;
            }
            else
            {
                return "system";
            }

        }
    }
}