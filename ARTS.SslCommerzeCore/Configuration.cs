using System;
using System.IO;
using System.Xml;

namespace ARTS.SslCommerzeCore
{
    public class Configuration
    {
        private static bool IS_SANDBOX = true;
        private static string SANDBOX_STOREID = string.Empty;
        private static string SANDBOX_PASS = string.Empty;
        private static string LIVE_STOREID = string.Empty;
        private static string LIVE_PASS = string.Empty;

        private static string LIVE_URL = "https://securepay.sslcommerz.com";
        private static string SANDBOX_URL = "https://sandbox.sslcommerz.com";

        public static string STORE_ID = string.Empty;
        public static string STORE_PASS = string.Empty;
        public static string STORE_URL = string.Empty;

        public static void Configure()
        {
            XmlDocument doc = new XmlDocument();
            string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sslcommerz.config");

            if (File.Exists(xmlPath))
            {
                try
                {
                    doc.Load(xmlPath);

                    XmlNode rootNode = doc.SelectSingleNode("ArtsSslCommerz");

                    if (rootNode != null)
                    {
                        XmlNode isSuccesNode = rootNode.SelectSingleNode("IsSandBox");
                        if (isSuccesNode != null)
                        {
                            if (isSuccesNode.InnerText.ToLower().Equals("true"))
                                IS_SANDBOX = true;
                            else if (isSuccesNode.InnerText.ToLower().Equals("false"))
                                IS_SANDBOX = false;
                        }

                        XmlNode credential = rootNode.SelectSingleNode("Credential");
                        if (credential != null)
                        {
                            if (credential.HasChildNodes)
                            {
                                XmlNode sandboxNode = credential.SelectSingleNode("Sandbox");
                                if (sandboxNode != null)
                                {
                                    SANDBOX_STOREID = sandboxNode.Attributes["storeid"]?.Value;
                                    SANDBOX_PASS = sandboxNode.Attributes["pass"]?.Value;
                                }

                                XmlNode liveNode = credential.SelectSingleNode("Live");
                                if (liveNode != null)
                                {
                                    LIVE_STOREID = liveNode.Attributes["storeid"]?.Value;
                                    LIVE_PASS = liveNode.Attributes["pass"]?.Value;
                                }
                            }
                        }

                        STORE_ID = IS_SANDBOX ? SANDBOX_STOREID : LIVE_STOREID;
                        STORE_PASS = IS_SANDBOX ? SANDBOX_PASS : LIVE_PASS;

                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                throw new FileNotFoundException(Path.GetFileName(xmlPath) + " file not found.");
            }

            STORE_URL = IS_SANDBOX ? SANDBOX_URL : LIVE_URL;
        }
    }
}