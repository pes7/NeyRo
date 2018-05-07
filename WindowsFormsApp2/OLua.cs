using LuaInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parabollian
{
    public class OLua
    {
        public Lua Lua { get; set; }

        public OLua()
        {
            Lua = new Lua();

            /*Register your command here*/
            Lua.RegisterFunction("sin", this, typeof(OLua).GetMethod("sin"));
            Lua.RegisterFunction("cos", this, typeof(OLua).GetMethod("cos"));
            Lua.RegisterFunction("tg", this, typeof(OLua).GetMethod("tg"));
            Lua.RegisterFunction("ctg", this, typeof(OLua).GetMethod("ctg"));

            Lua.RegisterFunction("abs", this, typeof(OLua).GetMethod("abs"));
            Lua.RegisterFunction("ceil", this, typeof(OLua).GetMethod("ceil"));
            Lua.RegisterFunction("exp", this, typeof(OLua).GetMethod("exp"));
            Lua.RegisterFunction("floor", this, typeof(OLua).GetMethod("floor"));
            Lua.RegisterFunction("sqrt", this, typeof(OLua).GetMethod("sqrt"));

            Lua.RegisterFunction("sinh", this, typeof(OLua).GetMethod("sinh"));
            Lua.RegisterFunction("cosh", this, typeof(OLua).GetMethod("cosh"));
            Lua.RegisterFunction("tanh", this, typeof(OLua).GetMethod("tanh"));
            Lua.RegisterFunction("truncate", this, typeof(OLua).GetMethod("truncate"));
        }

        public float sin(double x) { return (float)Math.Sin(x); }
        public float cos(double x) { return (float)Math.Cos(x); }
        public float tg(double x) { return (float)Math.Tan(x); }
        public float ctg(double x) { return (float)Math.Atan(x/1); }

        public float abs(double x) { return (float)Math.Abs(x); }
        public float ceil(double x) { return (float)Math.Ceiling(x); }
        public float exp(double x) { return (float)Math.Exp(x); }
        public float floor(double x) { return (float)Math.Floor(x); }
        public float sqrt(double x) { return (float)Math.Sqrt(x); }

        public float sinh(double x) { return (float)Math.Sinh(x); }
        public float cosh(double x) { return (float)Math.Cosh(x); }
        public float tanh(double x) { return (float)Math.Tanh(x); }
        public float truncate(double x) { return (float)Math.Truncate(x); }
    }
}
