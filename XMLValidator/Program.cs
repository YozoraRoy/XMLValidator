using System;
using System.Collections.Generic;

/*
 
1. Given a string, write a function to return true if the string contains well-formed XML elements; otherwise return false.
An XML document is called well-formed if it satisfies the following two rules.
Two rules are:
•	A well-formed XML document must have a corresponding end tag for all its start tags.
•	Nesting of elements within each other in an XML document must be proper. For example, <tutorial><topic>XML</topic></tutorial> is a correct way of nesting but <tutorial><topic>XML</tutorial></topic> is not.

Above are not the complete rules for the definition of well-formed in W3C spec. In this question, we just consider above two rules only.

Sample 1:
Input = “<Design><Code>hello world</Code></Design>”
Output = true 
Sample 2:
Input = “<Design><Code>hello world</Code></Design><People>”
Output = false
Sample 3:
Input = “<People><Design><Code>hello world</People></Code></Design>”
Output = false

Write the code of the function and note that any class in System.XML and Regular Expression are prohibited in this question.

 
 */

namespace XMLValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            string input1 = "<Design><Code>hello world</Code></Design>"; // 預期回傳 true
            Console.WriteLine($"{nameof(input1)} - Valid Start");
            bool input1Result1 = VaildotXML(input1);
            Console.WriteLine($"{nameof(input1)} - Valid result = {input1Result1}");

            string input2 = "<Design><Code>hello world</Code></Design><People>"; // 預期回傳 false
            Console.WriteLine($"{nameof(input2)} - Valid Start");
            bool input1Result2 = VaildotXML(input2);
            Console.WriteLine($"{nameof(input2)} - Valid result = {input1Result2}");

            string input3 = "<People><Design><Code>hello world</People></Code></Design>"; // 預期回傳 false
            Console.WriteLine($"{nameof(input3)} - Valid Start");
            bool input1Result3 = VaildotXML(input3);
            Console.WriteLine($"{nameof(input3)} - Valid result = {input1Result3}");

        }

        public static bool VaildotXML(string xml)
        {
            bool result = false;
            int xmllen = xml.Length;
            string tagResult = getValidXMLTag(xml, 0, xmllen);
            if (string.IsNullOrEmpty(tagResult))
            {
                result = true;
            }

            return result;
        }

        private static string getValidXMLTag(string xml, int xmlStartPostin, int xmlEndPostin)
        {
            // 基礎驗證
            string baseVaildResult = baseXMLFormatVaild(xml);
            if (baseVaildResult != "OK")
            {
                return baseVaildResult;
            }

            // 驗證標籤數量
            string baseXMLTabVaildResult = baseXMLTabVaild(xml);
            if (baseXMLTabVaildResult != "OK")
            {
                return baseXMLTabVaildResult;
            }

            // 驗證標籤位置
            Dictionary<int, int[]> keyValueList = new Dictionary<int, int[]>();
            int[] terms = new int[2];

            for (int i = 0; i < xml.Length; i++)
            {
                if (xml[i] == '<')
                {
                    terms[0] = i;
                }
                else if (xml[i] == '>')
                {
                    terms[1] = i;
                }

                if (terms[0] >= 0 && terms[1] > 0)
                {
                    int[] temp = new int[2];
                    temp = terms;
                    keyValueList.Add(i, temp);
                    terms = new int[2];
                }
            }

            // 字串
            List<string> pairsWord = new List<string>();
            foreach (var item in keyValueList)
            {
                int substringStartIndex = item.Value[0]+1;
                int startIndex = item.Value[1] - item.Value[0];
                pairsWord.Add(xml.Substring(substringStartIndex, startIndex));
            }

            for (int i = 0; i < (pairsWord.Count / 2); i++)
            {
                string cp1 = $"/{pairsWord[i]}";
                string cp2 = pairsWord[pairsWord.Count - (i + 1)];
                if (cp1 != cp2)
                {
                    return "xml字串格式<tag></tag> 標籤位置不一致";
                }
            }

              
            return "";
        }

        /// <summary>
        /// 基礎驗證
        /// </summary>
        /// <param name="xml">string</param>
        /// <returns>String</returns>
        private static string baseXMLFormatVaild(string xml)
        {

            string result = "OK";

            if (string.IsNullOrEmpty(xml))
            {
                return "請輸入xml字串";
            }

            if (xml.Contains("<") == false)
            {
                return "xml字串格式不符合<";
            }

            if (xml.Contains(">") == false)
            {
                return "xml字串格式不符合>";
            }

            if (xml.StartsWith("<") == false)
            {
                return "開始格式不符合<";
            }

            if (xml.EndsWith(">") == false)
            {
                return "結束格式不符合>";
            }

            return result;
        }

        /// <summary>
        /// XMLTag標籤數量驗證
        /// </summary>
        /// <param name="xml">xmlString</param>
        /// <returns></returns>
        private static string baseXMLTabVaild(string xml)
        {
            string result = "OK";

            // 驗證標籤數量
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            for (int i = 0; i < xml.Length; i++)
            {
                if (xml[i] == '<')
                {
                    keyValuePairs.Add(i, xml[i].ToString());
                }
                else if (xml[i] == '>')
                {
                    keyValuePairs.Add(i, xml[i].ToString());
                }
            }

            if (keyValuePairs.Count % 2 > 0)
            {
                result = "xml字串格式<> 標籤數量不相等";
            }

            return result;
        }

    }
}
