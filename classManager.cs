using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace COMPLIER
{
    /// <summary>
    /// try new ways to record a class or a object :a string for a class or an object
    /// eg. name1_type1_name2_type2_..._...
    /// </summary>
    class classManager//////////////
    {
        const String VMINT = "vmint";
        const String CLASS = "class";

         static String mid_code_output;
        ArrayList class_n_obj_list;

        int scan_prob;
        //public ArrayList ScanForAllVarsAndClassesAndObjs(String RawCodeText) 
        //{
        //    class_n_obj_list = new ArrayList();
        //    String [] statements=RawCodeText.Split(';');
        //    for (int i = 0; i < statements.GetLength(0); i++)
        //    {
        //        if (statements[i].Contains(VMINT))
        //        {
        //            class_n_obj_list.Add(new VarAndClass(statements[i].Replace(VMINT + " ", ""), false));
        //        }
        //        else if(statements[i].Contains(CLASS))
        //        {
                    
                
        //        }
        //    }
        //}
        public classManager() 
        {
            class_n_obj_list = new ArrayList();

        }

        public static String[] GetClassMidCodeInArray(String MidCode)
        {
            String[] midCodeSeg = MidCode.Split('\r');
            for (int i = 0; i < midCodeSeg.GetLength(0); i++)
            {
                if (midCodeSeg[i].Length > 1)
                {
                    midCodeSeg[i] = midCodeSeg[i].Replace("\n", "");
                }

            }

            return midCodeSeg;
        
        }

        public void SetAddressForClassNObjFromMidCode(String MidCode) 
        {
            String[] midCodeSeg = MidCode.Split('\r');
            for (int i = 0; i < midCodeSeg.GetLength(0); i++)
            {
                if (midCodeSeg[i].Length>1)
                {
                    midCodeSeg[i] = midCodeSeg[i].Replace("\n","");   
                }
             
            }
            for (int i = 0; i < midCodeSeg.GetLength(0); i++)
            {
                if (!midCodeSeg[i].Contains("blank row "))
                {
                    for (int j = 0; j < class_n_obj_list.Count; j++)
                    {
                        if (midCodeSeg[i].Equals(((classNode)class_n_obj_list[j]).GetName()))
                        {
                            ((classNode)class_n_obj_list[j]).SetOffSet(i+1);
                        }
                    }
                }
            }
        
        
        }
        public String get_mid_code()/////mid code is the push pop stuffs
        {
            return mid_code_output;
        }
        private void write2output(String Name,int rows) /////the rows concludes the first string of name
        {
            mid_code_output += Name+"\r\n";
            for (int i = 0; i < rows-1; i++)
            {
                mid_code_output += "blank row "+"\r\n";
            }
            scan_prob += rows;
        
        }

        public String ProcessFirstClass(String src,int startAddr)
        {
            scan_prob = 0;
            int first_blank = src.IndexOf(' ');//get the class name the the first posiution of class name
            int bracket_start = src.IndexOf('{');//get the end position of class name
            int class_end=src.IndexOf('}');

            String class_name=src.Substring(first_blank+1,bracket_start- first_blank-1);

            String class_body=src.Substring(bracket_start+1,class_end-bracket_start-1);

            classNode new_class = new classNode(class_name, class_name);
           
            write2output(class_name, 2);
            
            String[] mumber_array = class_body.Split(';');

            int MumberOffset = 1;//////////////Maybe should be 1 ???
            //process the mumbers
            for (int i = 0; i < mumber_array.GetLength(0); i++)
            {
               
                if (!mumber_array[i].Equals(""))
                {
                    String temp_str = mumber_array[i];
                    if (temp_str.Contains("vmint "))
                    {
                        String obj_name = temp_str.Substring(6);
                        
                        MumberOffset += 2;
                        
                        new_class.add_a_vmint_mumber(obj_name,MumberOffset-1);
                        
                        alloc("vmint");
                        write2output(obj_name, 2);
                    }
                    else if (temp_str.Contains("class "))
                    {
                        String obj_name_n_type = temp_str.Substring(6);
                        String[] name_array = obj_name_n_type.Split(' ');
                        String obj_name = name_array[1];
                        String obj_type = name_array[0];

                        classNode a_obj = new classNode(obj_name, obj_type);
                        a_obj = (classNode)SearchDefinedClass(obj_type).CloneSelf();
                        a_obj.SetName(obj_name);
                        MumberOffset += 2;//pointer must increase two to new location 
                        a_obj.SetOffSet(MumberOffset-1);

                        int MumberHeader = 0;
                        MumberHeader = MumberOffset ;
                        int mumber_offset = searchOffSet(obj_type);

                        MumberOffset = MumberOffset + mumber_offset-2;//increase the pointer and point to next mumber

                        new_class.add_a_mumber(a_obj);
                       
                        alloc(obj_name);
                        write2output(obj_name, mumber_offset);

                    }
                }

            }
            new_class.SetOffSet(MumberOffset+1);

            class_n_obj_list.Add(new_class);
            return src.Substring(class_end+1);
        }
        public String ProcessObj(String src) //////cut the string to the first ";" than search the class and alloc for the object 
        {
            scan_prob = 0;
            int first_blank = src.IndexOf(' ');//get the class name the the first posiution of class name
           // int bracket_start = src.IndexOf(';');//get the end position of class name
            int obj_end = src.IndexOf(';');
            String declearation = src.Substring(0, obj_end);
            String[] declearation_array = declearation.Split(' ');

            String obj_type=declearation_array[1];

            String obj_name = declearation_array[2];// src.Substring(bracket_start + 1, class_end - bracket_start - 1);

            int obj_offset = searchOffSet(obj_type);
            classNode a_obj = SearchDefinedClass(obj_type);
            a_obj.SetName(obj_name);
            //new_class.add_a_mumber(a_obj);

            alloc(obj_name);
            write2output(obj_name, obj_offset);

            classNode new_class = new classNode(obj_name, obj_type);

            write2output(obj_name, 2);

            


            //process the mumbers
           
            new_class.SetOffSet(scan_prob);

            class_n_obj_list.Add(new_class);
            return src.Substring(obj_end + 1);
        }

        classNode get_off_set_mumber_from_mumbers(classNode cN,String SearchedName) //////////search mumber by name
        {
            if (cN!=null)
            {
                ArrayList mumberList = cN.GetMumberList();
                for (int i = 0; i <mumberList.Count; i++)
                {
                    if (((classNode)mumberList[i]).GetName().Equals(SearchedName))
                    {
                        return ((classNode)mumberList[i]);
                    }
                }
                return null;
               
            }
            else
            {
                return null;
            }
        
        }


        public static int SearchNameAddrFrom(int index, String ObjName,String [] MidCode)
        {
            int TotalCodeLength=MidCode.GetLength(0);

            if (index <= MidCode.GetLength(0) - 1)
            {
                for (int i = index ; i <TotalCodeLength; i++)
                {
                    if (MidCode[i].Equals(ObjName))
                    {
                        return i;
                    }
                }
                return -1;
            
            }
            else
            {
                return -1;
            }
            
        
        }
        public static String  AccessObjectFromMidCodeSegs(String rawString, String[] midCode)
        {
            String[] subStrings = rawString.Split('.');
            int subStringNum = subStrings.GetLength(0);

            int Addr=0;
            for (int i = 0; i <subStringNum; i++)
            {
                Addr+=SearchNameAddrFrom(0, subStrings[i], midCode);

            }
            return Addr.ToString();
        
        }
        public String AccessObject(String rawString) 
        {

                String [] subStrings=rawString.Split('.');
                int subStringNum = subStrings.GetLength(0);
                /////search first layer of an expression like xx.xx.xx in class_n_obj_list
                classNode MatchedObject=new classNode();    
                for (int i = 0; i < class_n_obj_list.Count; i++)
			    {
			          classNode temp_class = ((classNode)class_n_obj_list[i]);
                      if (temp_class.GetName().Equals(subStrings[0]))
                     {
                        MatchedObject=temp_class;
                     }     
		   	    }
                int total_off_set = MatchedObject.GetOffSet();
                classNode MatchedBeingSearched = MatchedObject;
                for (int i = 1; i < subStringNum; i++)
                { 
                    MatchedBeingSearched = get_off_set_mumber_from_mumbers(MatchedBeingSearched, subStrings[i]);
                    int each_off_set = MatchedBeingSearched.GetOffSet();
                    if (each_off_set!=-1)
                    {
                        total_off_set = total_off_set + each_off_set;

                    }

                }
                return total_off_set.ToString();
                
        }
        private classNode SearchDefinedClass(String class_type) 
        {
            for (int i = 0; i < class_n_obj_list.Count; i++)
            {
                classNode temp_class = ((classNode)class_n_obj_list[i]);
                if (temp_class.GetObjClassType().Equals(class_type))
                {

                    return temp_class;
                }
                ;
            }
            return null;
        }
        private int searchOffSet(string type)///////////search existing class if the mumber is already defined
        {
            for (int i = 0; i < class_n_obj_list.Count; i++)
            {
                classNode temp_class= ((classNode)class_n_obj_list[i]);
                if (temp_class.GetObjClassType().Equals(type)) 
                {
                    return temp_class.GetOffSet();
                }
                ;
            }
            return -1;

//            throw new NotImplementedException();
        }

        private void alloc(string p)
        {

           // throw new NotImplementedException();
        }


       

    }
    class classNode //class used to present a class in the source code
    {
        String name;
        int offset;
        String type;
        ArrayList mumber;
        public Object CloneSelf() 
        {
            return this.MemberwiseClone();
        }
        public ArrayList GetMumberList() 
        {
            return mumber;
        }
        public classNode(String Name,int OffSet,String Type) 
        {
            name = Name;
            offset = OffSet;
            type = Type;
            mumber = new ArrayList();

        }
        public classNode(String Name,String Type) 
        {
            name = Name;
            type = Type;
            mumber = new ArrayList();
        }
        public classNode() 
        {
            name = "";
            offset = 0;
            type = "";
            mumber = new ArrayList();
        
        }
        public int add_a_vmint_mumber(String Name) 
        {
            classNode a_mumber = new classNode(Name,"vmint");
            mumber.Add(a_mumber);
            return 1;
        }
        public int add_a_vmint_mumber(String Name, int OffSet) 
        {
            classNode a_mumber = new classNode(Name, "vmint");
            a_mumber.SetOffSet(OffSet);
            mumber.Add(a_mumber);
            return 1;
        
        }
        public int add_a_mumber(String Name,String Type) 
        {
            return -1;
        
        }
        public int GetMumberOffSet(String mum)/////get a certain mumber's offset
        {
            if (mum.Equals(name))
            {
                return offset;
            }
            else
            {
              //  int tempoffset = 0;
                for (int i = 0; i < mumber.Count; i++)
                {
                    if (((classNode)mumber[i]).name.Equals(mum))
                    {
                        return offset + ((classNode)mumber[i]).GetMumberOffSet(mum);
                    }

                }
                return -1;
            }
        }
        public int GetOffSet()
        {
            return offset;
        }
        public void SetOffSet(int os) 
        
        {

            offset = os;
        }
        public String GetObjClassType() 
        {
            return type;
        }
        public void SetName(String Name) 
        {
            name = Name;
        }

        public String GetName()
        {
            return name;
        }

        public void add_a_mumber(classNode Obj_Or_Class)
        {
            mumber.Add(Obj_Or_Class);
           
        }
    }
}
