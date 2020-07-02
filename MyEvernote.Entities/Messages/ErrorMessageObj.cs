using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.Messages
{
  public  class ErrorMessageObj
    {
        /*   ErrorMessageCode
         sernameAlreadyExists = 101,
         EmailAlreadyExists = 102,
         UserIsNotActive= 151,
         UsernameOrPassWrong = 152,
         CheckYourEmail = 153, */
        public ErrorMessageCode Code { get; set; }
        public string Message{ get; set; }
    }
}
