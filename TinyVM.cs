using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTinyVM
{
    class TinyVM
    {
        //definations
        //pc 0
        //sp 1
        //lr 2
        //
        //comands: push pop add sub and or call 
        //comands: only for vm: outprint readin 
        //
        //stack grow from low address to high address
        //
        String push = "push";
        String pop="pop";
        String add="add";
        String sub= "sub";
        String mult = "mult";
        String and="and";
        String or="or";
        String call = "call";
        String jbz = "jbz";//jump if the number on stack is bigger than zero
        String outprint = "outprint";
        String readin = "readin";
        String jump = "jump";
        String ret = "ret";
        public String state_recorder;
        int ram_vol =1024/2;
        String [] ram_array;
        int stack_pointer;
        int stack_start=10;
        int stack_length=180;

        int static_start=300;
        int static_length = 50;
        
        int programm_pointer=0;
        int link_Register;

        int code_seg_start=200;
        int code_seg_length=99;

        int var_seg_start=300;
        int var_seg_length=99;
        

        public TinyVM(int stacklength,int staticlength,int codeseglength,int varseglength,int pc,int sp) 
        {
            ram_array = new string[ram_vol];

            stack_length = stacklength;
            
            static_length = staticlength;
            
            code_seg_length = codeseglength;
            
            var_seg_length = varseglength;

            programm_pointer = pc;
            
            stack_pointer = sp;




        }
        public void load_asm_file(String [] asm_file)
        {
            for (int i = 0; i < asm_file.GetLength(0); i++)
            {
                ram_array[i] = asm_file[i];
            }
            
        }
        public String run_to_end() 
        {
            state_recorder = "";
            while (!ram_array[programm_pointer].Equals("shut"))
            {
                state_recorder+=run_one_step(ram_array)+"\n";
            }
            return state_recorder;
        }

        public string run_one_step(String [] vm_ram ) 
        {
            //ram structure
            //the even ram holds the command
            //the odd ram holds the data 

            if (vm_ram[programm_pointer].Equals(pop))
            {
                vm_ram[Convert.ToInt32( vm_ram[programm_pointer + 1])] = vm_ram[stack_pointer-2];//from stack to variables' ram
                programm_pointer=programm_pointer+2;
                stack_pointer=stack_pointer-2;
                return vm_ram[programm_pointer];
            }
            if (vm_ram[programm_pointer].Equals(push))
            {
                vm_ram[stack_pointer] =vm_ram[Convert.ToInt32( vm_ram[programm_pointer + 1])];//push variables into stack
                programm_pointer=programm_pointer+2;
                stack_pointer= stack_pointer+2;
                return vm_ram[programm_pointer];
            }
            if (vm_ram[programm_pointer].Equals(add))
            {
                vm_ram[stack_pointer-4] =Convert.ToString( Convert.ToInt32( vm_ram[stack_pointer - 2]) +Convert.ToInt32( vm_ram[stack_pointer-4]));//add stack vars
                programm_pointer = programm_pointer + 2;
                stack_pointer = stack_pointer -2 ;
                return vm_ram[programm_pointer];
            }

            if (vm_ram[programm_pointer].Equals(sub))
            {
                vm_ram[stack_pointer-4] =Convert.ToString(Convert.ToInt32( vm_ram[stack_pointer -4]) - Convert.ToInt32(vm_ram[stack_pointer - 2]));//sub stack vars
                programm_pointer = programm_pointer + 2;
                stack_pointer = stack_pointer -2 ;
                return vm_ram[programm_pointer];
            }
            if (vm_ram[programm_pointer].Equals(mult))
            {
                vm_ram[stack_pointer-4] = Convert.ToString(Convert.ToInt32(vm_ram[stack_pointer - 4])* Convert.ToInt32(vm_ram[stack_pointer - 2]));//mult stack vars
                programm_pointer = programm_pointer + 2;
                stack_pointer = stack_pointer -2;
                return vm_ram[programm_pointer];
            }
            if (vm_ram[programm_pointer].Equals(and))
            {
                vm_ram[stack_pointer-4] = Convert.ToString(Convert.ToInt32(vm_ram[stack_pointer - 4])& Convert.ToInt32(vm_ram[stack_pointer - 2]));//and stack vars
                programm_pointer = programm_pointer + 2;
                stack_pointer = stack_pointer -2;
                return vm_ram[programm_pointer];
            }
            if (vm_ram[programm_pointer].Equals(or))
            {
                vm_ram[stack_pointer-4] = Convert.ToString(Convert.ToInt32(vm_ram[stack_pointer - 4]) | Convert.ToInt32(vm_ram[stack_pointer - 2]));//and stack vars
                programm_pointer = programm_pointer + 2;
                stack_pointer = stack_pointer -2;
                return vm_ram[programm_pointer];
            }
            if (vm_ram[programm_pointer].Equals(call))
            {
                //vm_ram[stack_pointer] = Convert.ToString(Convert.ToInt32(vm_ram[stack_pointer - 1]) | Convert.ToInt32(vm_ram[stack_pointer - 2]));//and stack vars
                link_Register = programm_pointer + 2;//save the current pc to lr
                programm_pointer = Convert.ToInt32(vm_ram[programm_pointer+1])+1;//jump to the function
               
                return vm_ram[programm_pointer];
               // stack_pointer = stack_pointer + 2;
            }
            if (vm_ram[programm_pointer].Equals(jump))
            {
                //vm_ram[stack_pointer] = Convert.ToString(Convert.ToInt32(vm_ram[stack_pointer - 1]) | Convert.ToInt32(vm_ram[stack_pointer - 2]));//and stack vars
                programm_pointer = Convert.ToInt32(vm_ram[programm_pointer + 1])+1;
                return vm_ram[programm_pointer];
                // stack_pointer = stack_pointer + 2;
            }
            if (vm_ram[programm_pointer].Equals(jbz))
            {
                if (Convert.ToInt32(vm_ram[stack_pointer - 2]) > 0)
                {
                    programm_pointer = Convert.ToInt32(vm_ram[programm_pointer + 1]) + 1;
                    
                }//and stack vars
                else 
                {
                    programm_pointer = programm_pointer + 2;
                    
                }
                //programm_pointer = programm_pointer + 2;
                stack_pointer = stack_pointer - 2;
                return vm_ram[programm_pointer];
               
            }
            if (vm_ram[programm_pointer].Equals(ret))
            {
                //vm_ram[stack_pointer] = Convert.ToString(Convert.ToInt32(vm_ram[stack_pointer - 1]) | Convert.ToInt32(vm_ram[stack_pointer - 2]));//and stack vars
                programm_pointer = link_Register;
                return vm_ram[programm_pointer];
                // stack_pointer = stack_pointer + 2;
            }
            if (vm_ram[programm_pointer].Equals(outprint))
            {
              
                programm_pointer= programm_pointer + 2;
                return vm_ram[ Convert.ToInt32( vm_ram[programm_pointer -1])];
            }
            programm_pointer = programm_pointer + 2;
            
            return vm_ram[programm_pointer];
        
        }



    }
}
