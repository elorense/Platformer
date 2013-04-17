using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Platformer
{
    public class XMLParse
    {

        /// <summary>
        /// Returns the value of an attribute of an XElement, or "" if no such
        /// attribute exists
        /// </summary>
        /// <param name="elem">Element to get attribute from</param>
        /// <param name="AttributeName">Name of the attribute to extract</param>
        /// <returns>Value of the attribute, or "" if it doesn't exist</returns>
        protected static string GetAttributeValue(XElement elem, String AttributeName)
        {
            foreach (XAttribute attr in elem.Attributes())
            {
                if (attr.Name.ToString() == AttributeName)
                {
                    return attr.Value;
                }
            }
            return "";
        }

        /// <summary>
        /// Adds a List&lt;Vector2> valued property to an object, based on
        /// an XML node
        /// </summary>
        /// <param name="elem">XML element to read name / value of property from</param>
        /// <param name="obj">Object to set proprerty on</param>
        protected static void AddVector2ListToClassInstance(XElement elem, Object obj)
        {
            List<Vector2> vectorList = new List<Vector2>();
            foreach (XElement pathPoint in elem.Elements())
            {
                
                Vector2 nextElem = new Vector2(float.Parse(pathPoint.Element("x").Value),
                                               float.Parse(pathPoint.Element("y").Value));
                vectorList.Add(nextElem);
            }
            PropertyInfo propertyInfo = obj.GetType().GetProperty(elem.Name.ToString());
            propertyInfo.SetValue(obj, vectorList, null);

        }

        /// <summary>
        /// Adds a Vector2 valued property to an object, based on
        /// an XML node
        /// </summary>
        /// <param name="elem">XML element to read name / value of property from</param>
        /// <param name="obj">Object to set proprerty on</param>
        protected static void AddVector2ToClassInstance(XElement elem, Object obj)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(elem.Name.ToString());
            Vector2 valueToSet = new Vector2(float.Parse(elem.Element("x").Value),
                    float.Parse(elem.Element("y").Value));
            propertyInfo.SetValue(obj, valueToSet, null);
        }


        /// <summary>
        /// Adds a float-valued property to an object, based on
        /// an XML node
        /// </summary>
        /// <param name="elem">XML element to read name / value of property from</param>
        /// <param name="obj">Object to set proprerty on</param>
        protected static void AddFloatToClassInstance(XElement elem, Object obj)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(elem.Name.ToString());
            propertyInfo.SetValue(obj, float.Parse(elem.Value), null);
        }

        /// <summary>
        /// Adds a string-valued property to an object, based on
        /// an XML node
        /// </summary>
        /// <param name="elem">XML element to read name / value of property from</param>
        /// <param name="obj">Object to set proprerty on</param>
        protected static void AddStringToClassInstance(XElement elem, Object obj)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(elem.Name.ToString());
            propertyInfo.SetValue(obj, elem.Value, null);
        }

        /// <summary>
        /// Adds a bool-valued property to an object, based on
        /// an XML node
        /// </summary>
        /// <param name="elem">XML element to read name / value of property from</param>
        /// <param name="obj">Object to set proprerty on</param>
        protected static void AddBoolToClassInstance(XElement elem, Object obj)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(elem.Name.ToString());
            string value = elem.Value.Trim().ToLower();
            bool boolVal = value == "true" || value == "t" || value == "1";
            propertyInfo.SetValue(obj, boolVal, null);
        }


        /// <summary>
        /// Adds a property value to an object, based on an XML node.  Use the
        /// 'type' attribute of the XML node to determine what type the element
        /// we are adding is -- float, Vector2, string, etc.
        /// </summary>
        /// <param name="elem">XML element to read name / value of property from</param>
        /// <param name="obj">Object to set proprerty on</param>
        public static void AddValueToClassInstance(XElement elem, Object obj)
        {
            string type = GetAttributeValue(elem, "type").ToLower();
            if (type == "float")
            {
                AddFloatToClassInstance(elem, obj);
            }
            else if (type == "vector2")
            {
                AddVector2ToClassInstance(elem, obj);
            }
            else if (type == "string")
            {
                AddStringToClassInstance(elem, obj);
            }
            else if (type == "bool")
            {
                AddBoolToClassInstance(elem, obj);
            }
            else if (type == "vector2list")
            {
                AddVector2ListToClassInstance(elem, obj);
            }

            else
            {
                throw new FormatException("Unknown Type attribute " + type + " in XML file");
            }

        }
    }
}