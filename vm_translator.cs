using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace COMPLIER
{
    class vm_translator
    {
        static String define = "define";
        static String label = "label";


       static String push = "push";
       static String pop = "pop";
       static String add = "add";
       static String sub = "sub";
       static String mult = "mult";
       static String and = "and";
       static String or = "or";
       static String call = "call";
       static String outprint = "outprint";
       static String readin = "readin";
       static String jump = "jump";
       static String jbz = "jbz";
       static String ret = "ret";
       static  ArrayList variable_list;
       static ArrayList var_address_list;
       public static bool IsCommand(String sentence)
       {
           if (sentence.Equals(jbz)||sentence.Equals(ret)||sentence.Equals(push)||sentence.Equals(pop)||sentence.Equals(add)||sentence.Equals(sub) || sentence.Equals(mult) || sentence.Equals(and) || sentence.Equals(or) || sentence.Equals(call) || sentence.Equals(outprint) || sentence.Equals(readin) || sentence.Equals(jump) || sentence.Equals(label) || sentence.Equals("shut"))
           {
               return true;
           }
           else
	        {
               return false;
	        }
       }

       public static bool If_this_variable_exist(string variable) 
       {
           for (int i = 0; i < variable_list.Count; i++)
           {
               if (Convert.ToString(variable_list[i]).Equals(variable))
               {
                   return true;
               }
               else 
               {
               
               }
             
           }
           return false;
       }
        public static void get_all_variables(String[] vm_src_file) 
        {
            variable_list = new ArrayList();
            var_address_list = new ArrayList();
            for (int i = 0; i < vm_src_file.GetLength(0); i=i+2)
            {
                if (vm_src_file[i]!=null)
                {
                    if (vm_src_file[i].Equals(push))
                    {
                        if (!If_this_variable_exist(vm_src_file[i+1]))
                        {
                            variable_list.Add(vm_src_file[i+1]);
                            var_address_list.Add(i + 1);
                        }
                    }
                    else
                    {
                        if (Tokenizer.IsNumAndEnCh(vm_src_file[i]) & (!IsCommand(vm_src_file[i])))
                        {
                            variable_list.Add(vm_src_file[i]);
                            var_address_list.Add(i + 1);
                        }
                        else 
                        {
                            if (vm_src_file[i].Equals(label))
                            {
                                variable_list.Add(vm_src_file[i + 1]);
                                var_address_list.Add(i + 1);
                            }
                        }
                    }
                }
            }
        }

        public static String[] replace_variables_with_addresses(String[] vm_src_file) 
        {
            for (int j=0;j<variable_list.Count;j++)
            {
                for (int i = 0; i < vm_src_file.GetLength(0); i++)
                {
                    if (vm_src_file[i].Equals(Convert.ToString( variable_list[j])))
                    {
                        vm_src_file[i] =Convert.ToString( var_address_list[j]);
                    }     
                }
            }
            return vm_src_file;
        }
        public static String[] translate(String[] vm_src_file)
        {
            ArrayList temp_list = new ArrayList();
            for (int i = 0; i < vm_src_file.GetLength(0); i++)
            {
                if (!(vm_src_file[i]==null))
	    {
		     temp_list.Add(vm_src_file[i]);
	    }
                
            }
            vm_src_file=new String[temp_list.Count];
            for (int i = 0; i < temp_list.Count; i++)
            {
                vm_src_file[i] = Convert.ToString(temp_list[i]);
            }

            get_all_variables(vm_src_file);
            vm_src_file = replace_variables_with_addresses(vm_src_file);
            return vm_src_file;
        }
    }
}
