using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    class CreateListBox
    {
        private string Name;
        private string Id;

        public CreateListBox(string id, string name)
        {

            this.Name = name;
            this.Id = id;
        }

        public string GetName
        {
            get
            {
                return Name;
            }
        }

        public string GetId
        {

            get
            {
                return Id;
            }
        }
    }
}
