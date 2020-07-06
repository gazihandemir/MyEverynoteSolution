using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverynote.WebApp.ViewModels
{
    public class ErrorViewModel : NotifyViewModelBase<ErrorMessageObj>
    {
        public ErrorViewModel()
        {
            Title = "hata";
        }
    }
}