using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace WebTemplateMVC5.App.Shared
{
    public static class SharedRepository
    {
        public static ExpandoObject RuntimeProps { get; set; }
        public static IDictionary<string, Object> RuntimeAsDict { get { return RuntimeProps; } }

        static SharedRepository(){
            RuntimeProps = new ExpandoObject();
            RuntimeAsDict.Add("TestProp", "Hello");
        }
    }
}