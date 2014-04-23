/*
 * Created by SharpDevelop.
 * User: Me_Android
 * Date: 2013/8/8
 * Time: 11:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace COMPLIER
{
	/// <summary>
	/// Description of STR_PROC.
	/// </summary>
	public class STR_PROC
	{
		public static ArrayList varlist,sentencelist,translist;
			
		public STR_PROC()
		{
			varlist=new ArrayList();
			sentencelist=new ArrayList();
			translist=new ArrayList();
		}
		
		public static string get_sentence(string str)
		{
			if(str.Contains(";"))
			{
				int start=str.IndexOf(";");
				string stc=str.Substring(0,start);
				
				sentencelist.Add(stc);
				
			return stc;
			}
			return null;
		}
		public static string get_left(string str)
		{
		if(str.Contains(";"))
			{
				int start=str.IndexOf(";");
				string stc=str.Substring(start+1);
				
				//sentencelist.Add(stc);
				
			return stc;
			}
			return str;
		}
		public static string get_var(string str)
		{
			
			if(str.Contains("var")&str.Contains(";"))
			{
				int start=str.IndexOf("var");
				int end=str.IndexOf(";");
				
				string var_name=str.Substring(start+4,(end-start-4));
				varlist.Add(var_name);
				return var_name;
			}
			return null;
		}
		
	}
}
