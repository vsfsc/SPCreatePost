using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLegalAmountField
{
    class RMBCapitalization
    {
        private const string DXSZ = "零壹贰叁肆伍陆柒捌玖";
    private const string DXDW = "毫厘分角元拾佰仟万拾佰仟亿拾佰仟万兆拾佰仟万亿";
    /// <summary>
    /// 转换整数为大写金额
    /// </summary>
    /// <param name="capValue">整数值</param>
    /// <returns>返回大写金额</returns>
    private string ConvertIntToUppercaseAmount(string capValue)
    {
        string currCap = "";
        string capResult = "";            
        int prevChar = -1;
        int currChar = 0;
        int posIndex = 4;
        if (Convert.ToDouble(capValue) == 0) return "";
        for (int i = capValue.Length - 1; i >= 0; i--)
        {
            currChar = Convert.ToInt16(capValue.Substring(i, 1));
            if (posIndex > 22)
            {
                break;
            }
            else if (currChar != 0)
            {
                currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
            }
            else
            {                    
                switch (posIndex)
                {
                    case 4: currCap = "元"; break;
                    case 8: currCap = "万"; break;
                    case 12: currCap = "亿"; break;
                    case 17: currCap = "兆"; break;
                    default: break;
                }
                if (prevChar != 0)
                {
                    if (currCap != "")
                    {
                        if (currCap != "元") currCap += "零";
                    }
                    else
                    {
                        currCap = "零";
                    }
                }
            }
            capResult = currCap + capResult;
            prevChar = currChar;
            posIndex += 1;
            currCap = "";   
        }
        return capResult;
    }
    /// <summary>
    /// 转换小数为大写金额
    /// </summary>
    /// <param name="capValue">小数值</param>
    /// <param name="addZero">是否增加零位</param>
    /// <returns>返回大写金额</returns>
    private string ConvertDecToUppercaseAmount(string capValue, bool addZero)
    {
        string currCap = "";
        string capResult = "";
        int prevChar = addZero ? -1 : 0;
        int currChar = 0;
        int posIndex = 3;
        if (Convert.ToInt16(capValue) == 0) return "";
        for (int i = 0; i < capValue.Length; i++)
        {
            currChar = Convert.ToInt16(capValue.Substring(i, 1));
            if (currChar != 0)
            {
                currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
            }
            else
            {
                if (Convert.ToInt16(capValue.Substring(i)) == 0)
                {
                    break;
                }
                else if (prevChar != 0)
                {
                    currCap = "零";
                }
            }
            capResult += currCap;
            prevChar = currChar;
            posIndex -= 1;
            currCap = "";
        }
        return capResult;
    }
    /// <summary>
    /// 将人民币大写转换成小写
    /// </summary>
    /// <param name="capValue">大写金额值</param>
    /// <returns>返回人民币小写金额</returns>
    private string RMBLowercaseAmount(string capValue)
    {
        string capResult = "";
        string currCap = "";
        for (int i = 0; i < capValue.Length; i++)
        {
            currCap = capValue.Substring(i, 1);
            switch (currCap)
            {
                case "零": capResult += "〇"; break;
                case "壹": capResult += "一"; break;
                case "贰": capResult += "二"; break;
                case "叁": capResult += "三"; break;
                case "肆": capResult += "四"; break;
                case "伍": capResult += "五"; break;                        
                case "陆": capResult += "六"; break;                        
                case "柒": capResult += "七"; break;
                case "捌": capResult += "八"; break;                        
                case "玖": capResult += "九"; break;
                case "拾": capResult += "十"; break;
                case "佰": capResult += "百"; break;
                case "仟": capResult += "千"; break;
                case "万": capResult += "万"; break;
                default: capResult += currCap; break;
            }
        }
        return capResult;
    }
    /// <summary>
    /// 人民币大写金额
    /// </summary>
    /// <param name="value">人民币数字金额值</param>
    /// <param name="isLowercase">是否返回人民币小写金额</param>
    /// <returns>返回人民币大写/小写金额</returns>
    public string RMBAmount(double value, bool isLowercase=false )
    {
        string capResult = "";
        string capValue = String.Format("", value);            
        int dotPos = capValue.IndexOf(".");
        bool addInt = (dotPos == 0);
        bool addMinus = (capValue.Substring(0, 1) == "-");
        int beginPos = addMinus ? 1 : 0;
        string capInt = capValue.Substring(beginPos, dotPos);
        string capDec = capValue.Substring(dotPos + 1);
        if (dotPos > 0)
        {
            capResult = ConvertIntToUppercaseAmount(capInt) + ConvertDecToUppercaseAmount(capDec, Convert.ToDouble(capInt) != 0 ? true : false);
        }
        else
        {
            capResult = ConvertIntToUppercaseAmount(capDec);
        }
        if (addMinus) capResult = "负" + capResult;
        if (addInt) capResult += "整";
        return isLowercase ? RMBLowercaseAmount(capResult) : capResult;
    }
    
    }
}
