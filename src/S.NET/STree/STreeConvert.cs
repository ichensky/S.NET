using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S.NET.STree
{
    public class STreeConvert
    {
        public const string ErrorStrNotValidFormat = "Not valid format.";
        public char[] TrimArr = new char[] { ' ', '\r', '\n', '\t' };

        public SNode Deserialize(string st)
        {
            if (st == null)
            {
                return null;
            }
            st = st.Trim(TrimArr);
            if (string.IsNullOrEmpty(st))
            {
                return null;
            }

            var node = new SNodeFull(false);
            Deserialize(ref st, node);
            return node;
        }

        private void Deserialize(ref string st, SNodeFull root)
        {
            st = st.Trim(TrimArr);
            if (string.IsNullOrEmpty(st))
            {
                return;
            }

            SNodeFull node = null;
            SNodeFull r = root;
            do
            {
                while (st[0] == ')')
                {
                    st = st.Remove(0, 1).TrimStart(TrimArr);
                    if (st.Length == 0)
                    {
                        return;
                    }
                    r = r.RootNode;
                    if (r == null)
                    {
                        throw new Exception(ErrorStrNotValidFormat);
                    }
                }
                node = DeserializeItem(ref st);
                st = st.Trim(TrimArr);

                r.AddNode(node);

                if (!node.IsLeaf)
                {
                    Deserialize(ref st, node);
                }

                st = RemoveComment(st);
            }
            while (st.Length > 0);

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

            if (st[0] == '(')
            {
                st = st.Remove(0, 1);
                return new SNodeFull(false);
            }

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
