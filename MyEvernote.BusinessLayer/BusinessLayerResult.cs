using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    
  public  class BusinessLayerResult<T> where T : class // Class türünde nesne oluşturma (T)
    {
        // Hata mesajı için prop oluşturma 
        // Kısaltma için liste errorMessageObj türünden
        public List<ErrorMessageObj> Errors { get; set; } 
        
        public T Result { get; set; } // Generic(class) bir result prop 

        public BusinessLayerResult()
        {   
            // CTOR oluşunca hata nesnesi oluştur.
            Errors = new List<ErrorMessageObj>();

        }
        // Hata ekleme 
        public void AddError(ErrorMessageCode code,string message)
        {
            Errors.Add(new ErrorMessageObj() { Code = code ,Message = message});
            // Mesaj parametresini hata eklerken kendi istediğimiz gibi veriyoruz.
            // code paramteresi
            /*
            (ENUM)
            UsernameAlreadyExists = 101,
            EmailAlreadyExists = 102,
            UserIsNotActive = 151,
            UsernameOrPassWrong = 152,
            CheckYourEmail = 153,
            */
        }
    }
}
