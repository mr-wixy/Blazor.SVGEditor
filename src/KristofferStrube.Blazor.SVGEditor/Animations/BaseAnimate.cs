﻿using AngleSharp.Dom;
using KristofferStrube.Blazor.SVGEditor.Extensions;
using System.Text;

namespace KristofferStrube.Blazor.SVGEditor;

public abstract class BaseAnimate : ISVGElement
{
    public BaseAnimate(IElement element, SVG svg)
    {
        Element = element;
        SVG = svg;
        if (Element.HasAttribute("values"))
        {
            Values = StringToValues(Element.GetAttribute("values"));
        }
        else
        {
            var valuesSB = new StringBuilder();
            if (Element.HasAttribute("from"))
            {
                valuesSB.Append(Element.GetAttribute("from"));
            }
            if (Element.HasAttribute("to"))
            {
                valuesSB.Append(Element.GetAttribute("to"));
            }
            Values = StringToValues(valuesSB.ToString());
        }
    }

    internal string _stateRepresentation;

    public IElement Element { get; init; }
    public SVG SVG { get; init; }
    public ISVGElement Parent { get; set; }
    public abstract Type Editor { get; }
    public abstract Type MenuItem { get; }
    public string StateRepresentation => Playing.ToString() + Dur + AttributeName + ValuesAsString + CurrentFrame.ToString();
    public bool Playing { get; set; }
    public List<string> Values { get; set; }
    public int FrameCount => Values.Count;
    public int? CurrentFrame { get; set; }
    public double Begin => Element.GetAttribute("begin") is string s ? s.Replace("s", "").ParseAsDouble() : 0;
    public double Dur => Element.GetAttribute("dur") is string s ? s.Replace("s", "").ParseAsDouble() : 0;
    public string AttributeName => Element.GetAttributeOrEmpty("attributename");
    public string ValuesAsString => Element.GetAttributeOrEmpty("values");
    public Action<ISVGElement> Changed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string StoredHtml { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void UpdateValues()
    {
        Element.SetAttribute("values", ValuesToString(Values));
    }

    public abstract bool IsEditing(string property);

    public static List<string> StringToValues(string attribute)
    {
        if (attribute == null)
        {
            return new List<string>();
        }

        return attribute.Split(";").Select(e => e.Trim()).ToList();
    }

    private static string ValuesToString(List<string> values)
    {
        return string.Join(";", values);
    }

    public void UpdateHtml()
    {
        throw new NotImplementedException();
    }

    public void Complete()
    {
        throw new NotImplementedException();
    }

    public void Rerender()
    {
        _stateRepresentation = null;
    }
}
