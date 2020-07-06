using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverynote.WebApp.ViewModels
{
    public class NotifyViewModelBase<T>
    {
        // Her hata vb. sayfası için teker teker sayfa açmamak için generic bir class tanımlıyıp işlerimizi buna göre yapıyoruz.
        // Hata , bilgi , tamam ve uyarı sayfaları bu class'tan extends yaparak özelliklerini kullanıyor.
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set; }
        public int RedirectingTimeout{ get; set; }
        public NotifyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz...";
            Title = "Geçersiz işlem !";
            IsRedirecting = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeout = 1000;
            Items = new List<T>();

        }
    }
}