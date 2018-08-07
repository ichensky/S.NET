using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S.NET.STree
{
    public class STreeConvert
    {
        public const string ErrorStrNotValidFormat = "Not valid format."; //Unexpected character encountered while parsing value
        public char[] TrimArr = new char[] { ' ', '\r', '\n', '\t' };

        public SNode Deserialize(string st)
        {
            if (st == null)
            {
                throw new ArgumentNullException(st);
            }
            else if (st.Length<2)
            {
                throw new ArgumentException(st);
            }

            var node = new SNodeFull(false);
            Deserialize(ref st, node);
            return node;
        }

        private int Deserialize(ref string st, SNodeFull root)
        {
            st = st.Trim(TrimArr);
            st = RemoveComment(st);
            if (string.IsNullOrEmpty(st))
            {
                return 0;
            }

            SNodeFull node = null;
            SNodeFull r = root;
            do
            {
                if (st[0] == ')')
                {
                    st = st.Remove(0, 1);
                    if (r.RootNode == null)
                    {
                        throw new Exception(ErrorStrNotValidFormat);
                    }
                    return 1;
                }
                else if (st[0] == '(')
                {
                    st = st.Remove(0, 1);
                    node = new SNodeFull(false);
                    r.AddNode(node);
                    var x= Deserialize(ref st, node);
                    if (x!=1)
                    {
                        throw new Exception(ErrorStrNotValidFormat);
                    }
                }
                else
                {
                    node = DeserializeItem(ref st);
                    r.AddNode(node);
                }
                st = st.Trim(TrimArr);
                st = RemoveComment(st);
            }
            while (st.Length > 0);
            return 0;
        }
        private string RemoveComment(string str)
        {
            while (str.Length > 0 && str[0] == ';')
            {
                var index = str.IndexOf('\n');
                if (index > -1)
                {
                    str = str.Remove(0, index + 1).Trim(TrimArr);
                }
                else
                {
                    str = string.Empty;
                }
            }
            return str;
        }

        private SNodeFull DeserializeItem(ref string st)
        {
            var x = 0;
            var esc = 0;
            for (int i = 0; i < st.Length; i++)
            {
                if (st[i] == '"')
                {
                    if (esc == 0)
                    {
                        esc = 1;
                    }
                    else if (esc == 1 && (i > 0 && st[i - 1] == '\\'))
                    {
                        throw new Exception(ErrorStrNotValidFormat);
                    }
                    else
                    {
                        esc = 2;
                        break;
                    }
                }
                else if (esc == 0 && " ()\r\n\t".Contains(st[i]))
                {
                    break;
                }

                x++;
            }
            if (esc == 1)
            {
                throw new Exception(ErrorStrNotValidFormat);
            }

            // to do optimize this block of code
            var head = esc == 0 ? st.Substring(0, x) : st.Substring(1, x - 1);
            st = st.Remove(0, esc == 0 ? x : x + 1);
            if (esc == 0)
            {
                var index = head.IndexOf(';');
                if (index > -1)
                {
                    head = head.Remove(index);
                    st = RemoveComment(';' + st);
                }
            }

            return new SNodeFull(head, true);
        }
    }
}
