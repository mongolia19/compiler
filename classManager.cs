﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace COMPLIER
{
    class classManager
    {
         static String mid_code_output;
        ArrayList class_n_obj_list;
        int scan_prob;
        public classManager() 
        {
            class_n_obj_list = new ArrayList();

        }
        public String get_mid_code()
        {
            return mid_code_output;
        }
        private void write2output(String Name,int rows) /////the rows concludes the first string of name
        {
            mid_code_output += Name;
            for (int i = 0; i < rows-1; i++)
            {
                mid_code_output += "blank row "+"/n/r";
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


            //process the mumbers
            for (int i = 0; i < mumber_array.GetLength(0); i++)
            {
                if (!mumber_array[i].Equals(""))
                {
                    String temp_str = mumber_array[i];
                    if (temp_str.Contains("vmint "))
                    {
                        String obj_name = temp_str.Substring(6);
                        new_class.add_a_vmint_mumber(obj_name);
                        alloc("vmint");
                        write2output(obj_name, 2);
                    }
                    else if (temp_str.Contains("class "))
                    {
                        String obj_name_n_type = temp_str.Substring(6);
                        String[] name_array = obj_name_n_type.Split(' ');
                        String obj_name = name_array[1];
                        String obj_type = name_array[0];

                        new_class.add_a_mumber(obj_name, obj_type);
                        int mumber_offset = searchOffSet(obj_type);
                        alloc(obj_name);
                        write2output(obj_name, mumber_offset);

                    }
                }

            }
            new_class.SetOffSet(scan_prob);

            class_n_obj_list.Add(new_class);
            return src.Substring(class_end+1);
        }

        private int searchOffSet(string type)
        {
            for (int i = 0; i < class_n_obj_list.Count; i++)
            {
                ((classNode)class_n_obj_list[i]).
            }

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
                int tempoffset = 0;
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

    
    }
}