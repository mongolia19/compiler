using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMPLIER
{
    class VarAndClass
    {
       public String VarName{ get; set; }
        public  int lengthInRam{get; set ;}
        Boolean isSimpleVar { get; set; }
        

        public VarAndClass(String Vname,Boolean ClassOrNot) 
        
        {
            VarName = Vname;
            lengthInRam = 2;
            isSimpleVar = !ClassOrNot;
          
        }


        public VarAndClass(String Vname, int ClassLength) 
        {
            VarName = Vname;
            lengthInRam = ClassLength;
            isSimpleVar = false;
        
        }


      
    }
}
