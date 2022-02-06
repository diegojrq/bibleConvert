using System;
using System.IO;
using System.Xml;

namespace BibleConvert
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                // Create the file, or overwrite if the file exists.                
                using (FileStream fs = File.Create(@"C:\Users\diego\Documents\bible.json"))
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\diego\Documents\bible.xml");
            XmlNodeList books  = doc.DocumentElement.ChildNodes;

            string jsonPath = @"C:\Users\diego\Documents\bible.json";

            File.AppendAllText(jsonPath, "{" + Environment.NewLine);
            File.AppendAllText(jsonPath, "  \"book\": [{" + Environment.NewLine);

            foreach (XmlNode book in books)
            {
                XmlAttributeCollection attributes = book.Attributes;
                foreach (XmlNode attribute in attributes)
                {
                    File.AppendAllText(jsonPath, "    \"" + attribute.Name + "\": " + "\"" + attribute.Value + "\"," + Environment.NewLine);
                }

                bool vOnFire = false;
                bool cOnFire = false;

                XmlNodeList nodes = book.ChildNodes;
                foreach (XmlNode node in nodes)
                {

                    if(node.Name == "h")
                    {
                        File.AppendAllText(jsonPath, "    \"" + node.Name + "\": " + "\"" + node.InnerText + "\"," + Environment.NewLine);
                    }

                    if (node.Name == "c")
                    {
                        vOnFire = false;

                        if(!cOnFire)
                        {
                            File.AppendAllText(jsonPath, "    \"" + node.Name + "\": " + "[{" + Environment.NewLine);
                        } else
                        {
                            File.AppendAllText(jsonPath, "    {" + Environment.NewLine);
                        }
                        
                        cOnFire = true;

                        attributes = node.Attributes;
                        foreach (XmlNode attribute in attributes)
                        {
                            File.AppendAllText(jsonPath, "      \"" + attribute.Name + "\": " + "\"" + attribute.Value + "\"," + Environment.NewLine);
                        }
                    }

                    if (node.Name == "v")
                    {

                        if (!vOnFire)
                        {
                            File.AppendAllText(jsonPath, "        \"" + node.Name + "\": " + "[" + Environment.NewLine);
                        }      
                        
                        vOnFire = true;

                        attributes = node.Attributes;
                        foreach (XmlNode attribute in attributes)
                        {
                            File.AppendAllText(jsonPath, "          { \"" + attribute.Name + "\": " + "\"" + attribute.Value + "\", ");
                        }

                    }

                    if (node.Name == "#text")
                    {
                        vOnFire = true;

                        File.AppendAllText(jsonPath, "\"text\": \"" + node.InnerText.Trim() + "\" }");
                    }

                    if (node.Name == "ve")
                    {
                        if(node.NextSibling != null)
                        {
                            if (node.NextSibling.Name != "c")
                            {
                                File.AppendAllText(jsonPath, "," + Environment.NewLine);
                            } else
                            {
                                File.AppendAllText(jsonPath, Environment.NewLine);
                                File.AppendAllText(jsonPath, "        ]" + Environment.NewLine);
                                File.AppendAllText(jsonPath, "    }," + Environment.NewLine);
                            }
                        } else
                        {
                            File.AppendAllText(jsonPath, Environment.NewLine);
                            File.AppendAllText(jsonPath, "        ]" + Environment.NewLine);
                            File.AppendAllText(jsonPath, "    }]" + Environment.NewLine);
                            File.AppendAllText(jsonPath, "  }");

                            if(node.ParentNode.NextSibling != null)
                            {
                                File.AppendAllText(jsonPath, "," + Environment.NewLine);
                                File.AppendAllText(jsonPath, "  {" + Environment.NewLine);
                            } else
                            {
                                File.AppendAllText(jsonPath, "]" + Environment.NewLine);
                            }
                        }
                    }
                }
            }
            File.AppendAllText(jsonPath, "}");
        }
    }
}
