using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace COMPLIER
{
    class Tokenizer
    {
        String VAR = " var";
        String CONST = "const";
        String OP = "op";
        String EXP = "exp";
       public  static String VM_Code;
        /// <summary>   
        /// 判断输入的字符串是否只包含数字和英文字母   
        /// </summary>   
        /// <param name="input"></param>   
        /// <returns></returns>   
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        public static bool IsNumberString(string str)
        {
            if (str == null || str == "") return false;
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
            return !objNotNumberPattern.IsMatch(str) &&
            !objTwoDotPattern.IsMatch(str) &&
            !objTwoMinusPattern.IsMatch(str) &&
            objNumberPattern.IsMatch(str);
        }
        public static bool IsExp(string str) 
        {
            return IsNumAndEnCh(str);
        }

        public static bool IsOp(string str) 
        {
            if (str.Equals("+")|str.Equals("-")|str.Equals("*")|str.Equals("&")|str.Equals("|"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public  static String remove_all_spaces(String s)
        {
             return Regex.Replace(s, "\\s{1,}","");  


        }
        public static ArrayList split_source_code( String src) 
        {
            String src_string = src;
            ArrayList token_list = new ArrayList();
            ArrayList string_stack=new ArrayList(); 
            char [] src_char_array =src_string.ToCharArray();
            for (int i = 0; i < src_char_array.GetLength(0); i++)
			{
			    if (src_char_array[i].Equals('(')|IsOp(Convert.ToString( src_char_array[i]))|src_char_array[i].Equals(')'))
	            {
                    if (string_stack.Count==0)
                    {
                        
                    }
                    else
                    {
                       token_list.Add(pop_stack(string_stack));
                       string_stack.Clear();
                    }
                    token_list.Add(Convert.ToString( src_char_array[i]));
                    
                    //token_list.Add(src_string.Substring(0, i));
                    //token_list.Add(src_string.Substring(i, 1));
                   
	            }
                else
                {
                    string_stack.Add(src_char_array[i]);
                }
			}
            for (int i = 0; i < token_list.Count; i++)
            {
                token_list[i] = Convert.ToString(token_list[i]) + " ";   
            }

            return token_list;

//            src.Substring

        }

        private static String pop_stack(ArrayList string_stack)
        {
            String return_string = "";
            for (int i = 0; i < string_stack.Count; i++)
            {

                return_string += Convert.ToString(string_stack[i]);

            }

            return return_string;
  //          throw new NotImplementedException();
        }
        
        public static String[] get_token_list(string src) 
        {
           
            
             String [] sentence=src.Split(' ');
             return sentence;
        
        }
        public static String Deal_one_statement(String statement)   ////can deal with strings without spaces
        {
            String pop_statement="";
            if (statement.Contains("="))
            {
               statement= statement.Insert(statement.IndexOf('=')+1, "(");
               statement = statement.Insert(statement.Length , ")");
                pop_statement = statement.Substring(0, statement.IndexOf("=") -0);
                statement = statement.Substring(statement.IndexOf("=") + 1);
                WriteCodeWithoutSpace(statement);
                VM_Code += "pop " + pop_statement + "\r\n";
                return "finish one statement";
            }
            else 
            {
                statement = statement.Insert(0, "(");
                statement = statement.Insert(statement.Length, ")");
                WriteCodeWithoutSpace(statement);
                return "finish one statement";
            }

        }
        public static String Deal_one_branch(String input_src) //////this function deal with strings without spaces
        {
            string[] statements=input_src.Split(';');
            for (int i = 0; i < statements.GetLength(0); i++)
            {
                Deal_one_statement(statements[i]);
            }
            return "finish one branch";
        }
        public static String Deal_If_branch_without_else (String input_src)
        {
            int cond_start, cond_end;
            int if_start, if_end;
            int else_start, else_end;
            int statement_before = input_src.IndexOf("if(") - 1;
            int statement_after = input_src.LastIndexOf("}") + 1;
            String src_statement_before = "";
            String src_statement_after = "";
            src_statement_before = input_src.Substring(0, statement_before);
            src_statement_after = input_src.Substring(statement_after, input_src.Length - statement_after);
            if (src_statement_before.Length > 0)////////deal the Sequential executions before if
            {
                Deal_one_branch(src_statement_before);
            }

            cond_start = input_src.IndexOf('(') + 1;
            cond_end = input_src.IndexOf('{') - 2;
            if_start = cond_end + 3;
            if_end = input_src.IndexOf('}') - 1;
            //else_end = input_src.LastIndexOf('}') - 1;
            string cond = input_src.Substring(cond_start, cond_end - cond_start + 1);
            string if_branch = input_src.Substring(if_start, if_end - if_start + 1);
           // string else_branch = input_src.Substring(if_end + 7, else_end - if_end - 7 + 1);
            WriteIfCond(cond);
            VM_Code += "jbz L1" + "\r\n";
            Deal_one_branch(if_branch);
            VM_Code += "label L1" + "\r\n";
            //Deal_one_branch(else_branch);
            //VM_Code += "label L2" + "\n";
            return "success";
        }
        public static String Deal_Stmts(String input_src) 
        {
            if (input_src == null | input_src.Equals(""))
            {
                return "";
            }
            else
            {
                for (int i = 0; i < input_src.Length; i++)
                {
                    if (input_src[i].Equals('{'))
                    {
                        String str1 = "";
                        String str2 = "";
                        String str3 = "";
                        int first_part_end = input_src.IndexOf('{');

                        int middle_part_end = get_top_level_end_token(input_src);
                        str1 = input_src.Substring(0, first_part_end + 1);
                        str2 = input_src.Substring(first_part_end + 1, middle_part_end - first_part_end + 1);
                        str3 = input_src.Substring(middle_part_end + 1, input_src.Length - middle_part_end);
                        Deal_Stmts(str1);
                       
                        Deal_Stmts(str2);
                        
                        Deal_Stmts(str3);
                        return "Sucess";
                    }
                    else
                    {
                        if (i+2<=input_src.Length-1)
                        {
                            if (input_src[i].Equals('i') & input_src[i + 1].Equals('f') & input_src[i + 2].Equals('('))
	                        {
                                String expression = "";
                                int expression_start = i + 3;
                                int expression_end = get_top_level_end_bracket_token(input_src);
                                expression = input_src.Substring(expression_start, expression_end - expression_start );
                                Deal_one_statement(expression);

                                int first_part_end = input_src.IndexOf('{');
                                int middle_part_end = get_top_level_end_token(input_src);

                                String str2 = input_src.Substring(first_part_end + 1, middle_part_end - first_part_end -1);
                                String str3 = "";
                                if (middle_part_end<input_src.Length-1)
                                {
                                  str3 = input_src.Substring(middle_part_end + 1, input_src.Length - middle_part_end);
                                }
                                
                                VM_Code += "jbz L1" + "\n";
                                Deal_Stmts(str2);
                                VM_Code += "label L1" + "\n";
                                Deal_Stmts(str3);
                                return "success";
                        	}
                            

                        }
                     
                            if (input_src[i].Equals(';'))
                            {
                                int first_part_end = i;
                                String str1 = input_src.Substring(0, first_part_end );
                                String str2 = "";
                                if (first_part_end+1<input_src.Length-1)
                                {
                                    str2 = input_src.Substring(first_part_end + 1, input_src.Length-1 - first_part_end);
                                }
                                
                                Deal_one_branch(str1);
                                Deal_Stmts(str2);
                                return "suceess";
                            }
                       
                        
                    }
                }
                return "Syntax error!";
            }
        }
        public static String Deal_If_branch(String input_src)
        {
            int cond_start,cond_end;
            int if_start, if_end;
            int else_start, else_end;
            int statement_before= input_src.IndexOf("if(")-1;
            int statement_after = input_src.LastIndexOf("}")+1;
            String src_statement_before="";
            String src_statement_after="";
            src_statement_before=input_src.Substring(0,statement_before);
            src_statement_after=input_src.Substring(statement_after,input_src.Length-statement_after);
            if (src_statement_before.Length > 0)////////deal the Sequential executions before if
            {
                Deal_one_branch(src_statement_before);
            }

            cond_start= input_src.IndexOf('(')+1;
            cond_end = input_src.IndexOf('{') - 2;
            if_start = cond_end + 3;
            if_end = input_src.IndexOf('}')-1;
            else_end = input_src.LastIndexOf('}')-1;
            string cond = input_src.Substring(cond_start, cond_end - cond_start+1);
            string if_branch = input_src.Substring(if_start, if_end - if_start+1);
            string else_branch = input_src.Substring(if_end + 7,else_end-if_end-7+1);
            WriteIfCond(cond);
            VM_Code += "jbz L1"+"\n";
            Deal_one_branch(if_branch);
            VM_Code += "jump L2"+"\n";
            VM_Code += "label L1"+"\n";
            Deal_one_branch(else_branch);
            VM_Code += "label L2"+"\n";
            return "success";
        }

       static int get_top_level_end_token(string end_token) 
        {
            int left_bracket_num = 0;
           

            for (int i = 0; i < end_token.Length; i++)
            {
                if (end_token[i].Equals('{'))
                {
                    left_bracket_num++;        
                }
                else
                {
                    if (end_token[i].Equals('}'))
                    {
                        left_bracket_num--;
                        if (left_bracket_num==0)
                        {
                            return i;
                        }
                    }
                }
            }
            return end_token.Length - 1;
        
        }
       static int get_top_level_end_bracket_token(string end_token)
       {
           int left_bracket_num = 0;


           for (int i = 0; i < end_token.Length; i++)
           {
               if (end_token[i].Equals('('))
               {
                   left_bracket_num++;
               }
               else
               {
                   if (end_token[i].Equals(')'))
                   {
                       left_bracket_num--;
                       if (left_bracket_num == 0)
                       {
                           return i;
                       }
                   }
               }
           }
           return end_token.Length - 1;

       }
        private static string WriteIfCond(string cond)
        {
          cond=MainForm.combine_string_list( Tokenizer.split_source_code(cond));
            return codeWrite(cond);
            throw new NotImplementedException();
        }
        public static string WriteCodeWithoutSpace(String input_src) 
        {
            input_src = MainForm.combine_string_list(Tokenizer.split_source_code(input_src));
            return codeWrite(input_src);
        }
        public static String codeWrite(String input_src)/////Attention! this function can ONLY deal WITH strings with spaces
    {
        //VM_Code = "";
        int left_bracket_list = 0;
        ArrayList op_list=new ArrayList();
         String [] token_list=get_token_list(input_src);
            for (int i = 0; i < token_list.GetLength(0); i++)
        {
            if (token_list[i].Equals("("))
            {
                left_bracket_list++;
            }

            if (IsExp( token_list[i])&(i==token_list.GetLength(0)-1))/////at the end must be a variable
            {
                VM_Code += "push " + Convert.ToString(token_list[i]) + "\r\n";
            }
            if (i < token_list.GetLength(0) - 1)
            {
                if (IsNumAndEnCh(token_list[i]) & (!token_list[i + 1].Equals("("))) /////not at the end and the next token is not a "("
                {
                    VM_Code += "push " + Convert.ToString(token_list[i]) + "\r\n";
                }
            }
            if (IsOp(token_list[i]))
            {
                op_list.Add(token_list[i]);
                //VM_Code += "push " + token_list[i] + "\n";
            }
            if (i<token_list.GetLength(0)-1)
            {
                if (IsNumAndEnCh(token_list[i])&(token_list[i+1].Equals("(")))
                {
                    op_list.Add("CALL " + token_list[i]);
                }
            }
            if (token_list[i].Equals(")"))
            {
                left_bracket_list--;
                if (left_bracket_list<0)
                {
                    return "Error";
                }
                String temp_op="";
                if (op_list.Count>0)
                {
                    temp_op = Convert.ToString(op_list[op_list.Count - 1]);
                    op_list.RemoveAt(op_list.Count - 1);
                }
                 
                 if (temp_op.Equals("+"))
                 {
                     VM_Code += "ADD" + "\r\n";
                 }
                 else
                 {
                     if (temp_op.Equals("-"))
                     {
                         VM_Code += "SUB" + "\r\n";
                     }
                     else
                     {
                         if (temp_op.Equals("*"))
                         {
                             VM_Code += "MULT" + "\r\n";
                         }
                         else
                         {
                             if (temp_op.Equals("&"))
                             {
                                 VM_Code += "AND" + "\r\n";
                             }
                             else
                             {
                                 if (temp_op.Equals("|"))
                                 {
                                     VM_Code += "OR" + "\r\n";
                                 }
                                 else 
                                 {

                                     VM_Code += temp_op + "\r\n";
                                 }
                             }
                         }
                     }
                 }

                //VM_Code += "push " + token_list[i] + "\n";
            }
            //if ((op_list.Count>0)&(left_bracket_list==0))
            //{
            //   String  temp_op=  Convert.ToString(op_list[op_list.Count - 1]);
            //     op_list.RemoveAt(op_list.Count - 1);
            //    if (temp_op.Equals("+"))
            //     {
            //         VM_Code += "ADD" + "\n";
            //     }
            //     if (temp_op.Equals("-"))
            //     {
            //         VM_Code += "SUB" + "\n";
            //     }
            //     if (temp_op.Equals("*"))
            //     {
            //         VM_Code += "MULT" + "\n";
            //     }
            //     if (temp_op.Equals("&"))
            //     {
            //         VM_Code += "AND" + "\n";
            //     }
            //     if (temp_op.Equals("|"))
            //     {
            //         VM_Code += "OR" + "\n";
            //     }
            //}
            
        }
            return "Finish";
    }
      


    }
}
