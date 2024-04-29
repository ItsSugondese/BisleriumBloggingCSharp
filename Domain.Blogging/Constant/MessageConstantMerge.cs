using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.Constant
{
    public class MessageConstantMerge
    {
        public static string requetMessage(string requestType, string moduleName)
        {
            return moduleName + " has been " + requestType + " successfully.";
        }
        public static string notExist(string attribue, string moduleName)
        {
            return moduleName + " with that " + attribue + " doesn't exists.";
        }

        public static string alreadyExist(string attribue, string moduleName)
        {
            return moduleName + " with that " + attribue + " already exists.";
        }
    }
}
