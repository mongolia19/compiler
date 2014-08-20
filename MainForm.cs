/*
 * Created by SharpDevelop.
 * User: Me_Android
 * Date: 2013/8/8
 * Time: 11:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using COMPLIER;
using System.Collections;
using MyTinyVM;
using System.IO;
namespace COMPLIER
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
        ArrayList static_ram;//hold  static vars
        ArrayList local_ram;//hold local vars
        ArrayList this_ram;//hold  object
        ArrayList arg_ram;//hold the args pass to the functions
        ArrayList VariableList;
        TinyVM my_vm;
        classManager my_classManager;
        String temp_src;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			STR_PROC strproc=new STR_PROC();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}


        public String[] read_vm_asm_file(String path) 
        {
            FileStream fs = new FileStream(path, FileMode.Open);//"d:\\enstopword.txt"d:\\books\\37913.txt

            StreamReader m_streamReader = new StreamReader(fs);

            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            //string arry = "";
            string strLine;
            ArrayList tempArray = new ArrayList();
            do
            {
                //                string[] split = strLine.Split('=');
                //                string a = split[0];
                //                if (a.ToLower() == "ip")
                //                {
                //                    arry += strLine + "\n";
                //
                //                }
                strLine = m_streamReader.ReadLine();
                if (strLine!=null)
                {
                    tempArray.Add(strLine.Trim());
                }
               

            } while (strLine != null );

           String [] asm_file=new String[tempArray.Count];

           for (int i = 0; i < tempArray.Count; i++)
           {
               if (tempArray[i].ToString()=="")
               {
                   tempArray.RemoveAt(i);
               }
           }
            
           for (int i = 0; i < tempArray.Count; i++)
           {
               asm_file[i] =Convert.ToString( tempArray[i]);
           }

            m_streamReader.Close();
            m_streamReader.Dispose();
            fs.Close();
            fs.Dispose();

            return asm_file;
        
        }
        public static String combine_string_list(ArrayList str_list) 
        {
            String combined="";
            for (int i = 0; i < str_list.Count; i++)
            {
                combined += Convert.ToString(str_list[i]);
            }
            combined=combined.TrimEnd();
            return combined;
        }
		void Button1Click(object sender, EventArgs e)
		{
			//STR_PROC.varlist=new System.Collections.ArrayList();
            //if(STR_PROC.sentencelist==null) STR_PROC.sentencelist=new System.Collections.ArrayList();
			
            //textBox2.Text=COMPLIER.STR_PROC.get_var(textBox1.Text);
            //textBox1.Text=COMPLIER.STR_PROC.get_left(textBox1.Text);
            //ArrayList testarraylist =Tokenizer.split_source_code(Tokenizer.remove_all_spaces( textBox1.Text));
            //textBox1.Text =combine_string_list(testarraylist);
            //Tokenizer.codeWrite(textBox1.Text);
            textBox1.Text= Tokenizer.remove_all_spaces(textBox1.Text);
            //Tokenizer.Deal_If_branch(textBox1.Text);
            Tokenizer.Deal_Stmts(textBox1.Text);
            textBox2.Text = "";
            textBox2.Text=Tokenizer.VM_Code;
            
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
            my_vm = new TinyVM(50, 50, 50, 30, 0, 50);
            //string[] asm_test_file = read_vm_asm_file("d:\\TestIf.vmasm");
            //asm_test_file=vm_translator.translate(asm_test_file);
            //my_vm.load_asm_file(asm_test_file);
            my_classManager = new classManager();
		}

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                temp_src = my_classManager.ProcessFirstClass(textBox1.Text, 0); //Tokenizer.remove_all_spaces(textBox1.Text);
                textBox1.Text = temp_src;
                //Tokenizer.Deal_If_branch(textBox1.Text);
                textBox2.Text = my_classManager.get_mid_code();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String class_mid_code = textBox2.Text;

            my_classManager.SetAddressForClassNObjFromMidCode(class_mid_code);
            ;
            String addr = my_classManager.AccessObject(textBox3.Text);
//           String addr = classManager.AccessObjectFromMidCodeSegs(textBox3.Text, classManager.GetClassMidCodeInArray(class_mid_code));
           textBox3.Text = textBox3.Text + "\r\n" + addr;
        }

	}
}
