using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

internal class TestClassBase3
{
    public string BaseField1 { get; set; }
}

internal class TestClass3 : TestClassBase3
{
    public string FieldPublic1;
    public string FieldPublic2;
    private string FieldPrivate1;
    private string FieldPrivate2;
    internal string FieldInternal1;
    internal string FieldInternal2;

    public string PropPublic1 { get; set; }
    public string PropPublic2 { get; set; }
    private string PropPrivate1 { get; set; }
    private string PropPrivate2 { get; set; }
    internal string PropInternal1 { get; set; }
    internal string PropInternal2 { get; set; }

    public const string ConstantPublic1 = "1";
    public const string ConstantPublic2 = "2";
    private const string ConstantPrivate1 = "3";
    private const string ConstantPrivate2 = "4";
    internal const string ConstantInternal1 = "5";
    internal const string ConstantInternal2 = "6";

    public string MethodPublic1() { return "1";  }
    public string MethodPublic2() { return "2";  }
    private string MethodPrivate1() { return "3";  }
    private string MethodPrivate2() { return "4"; }
    internal string MethodInternal1() { return "5";  }
    internal string MethodInternal2() { return "6"; }

    public TestClass3()
    {
        base.BaseField1 = "BaseField1";
    }
}