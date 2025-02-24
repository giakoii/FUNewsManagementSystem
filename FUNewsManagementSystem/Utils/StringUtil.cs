namespace Client.Utils;

/// <summary>
///  StringUtil
/// </summary>
public class StringUtil
{
    /// <summary>
    ///  Convert the first character of the string to uppercase
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToLowerCase(string value)
    {
        if (value == null) 
            return null;
        if (value.Length <= 0) return string.Empty;

        return char.ToLower(value[0]) + value.Substring(1);
    }
    
    
    #region ConvertToMoney
    /// <summary>
    /// Convert to VND
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToVND(decimal value)
    {
        return value.ToString("#,##0") + " VND";
    }
    
    /// <summary>
    /// Convert to VND
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToVND(int value)
    {
        return value.ToString("#,##0") + " VND";
    }
    
    /// <summary>
    /// Convert to VND
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToVND(long value)
    {
        return value.ToString("#,##0") + " VND";
    }
    
    /// <summary>
    /// Convert to VND
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToVND(double value)
    {
        return value.ToString("#,##0") + " VND";
    }

    /// <summary>
    /// Convert to percent
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToPercent(decimal value)
    {
        return value.ToString() + "%";
    }
    
    /// <summary>
    /// Convert to percent
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToPercent(decimal? value)
    {
        return value.HasValue ? value.Value.ToString() + "%" : "";
    }
    
    /// <summary>
    /// Convert to percent
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToPercent(int value)
    {
        return value.ToString() + "%";
    }
    
    /// <summary>
    /// Convert to percent
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToPercent(long value)
    {
        return value.ToString() + "%";
    }
    
    /// <summary>
    /// Convert to percent
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToPercent(double value)
    {
        return value.ToString() + "%";
    }
    
    /// <summary>
    /// Convert to percent
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToPercent(float value)
    {
        return value.ToString() + "%";
    }
    #endregion
}