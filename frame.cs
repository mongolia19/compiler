using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace COMPLIER
{
    class frame
    {
        int number_of_members;
        ArrayList member_table;
        int obj_address_start;
        int obj_num;
        ArrayList obj_table;
        public frame(int num_member,int obj_address) 
        {
            number_of_members = num_member;
            obj_address_start = obj_address;
            member_table = new ArrayList();
            obj_table = new ArrayList();
        }
        public int creat_obj(String obj_name)/////creat an object and return  address of its location adress in ram (its pointer)
        {
            obj_table.Add( obj_name);
            obj_num++;
            return (obj_num - obj_address_start + 1);
        }

    }
}
