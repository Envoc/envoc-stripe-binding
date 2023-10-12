namespace StripeTerminal;

public static partial class Extensions
{
    //Should this log exception insted of info?
    public static void Info(this IStripeTerminalLogger logger, string message, NSError error)
    {
        Dictionary<string, object> parameters = null;

        //TODO: Do we only want to log *if* error is not null?
        if (error != null)
        {
            var enumString = ((StripeTerminal.SCPError)error.Code).ToString();

            parameters = new Dictionary<string, object>()
            {
                ["error_code"] = error.Code.ToString(),
                ["error_code_enum"] = enumString,
                ["error_description"] = error.LocalizedDescription,
            };
        }

        logger.Info(message, parameters);
    }

    public static void Exception(this IStripeTerminalLogger logger, string message, NSError error)
    {
        logger.Exception(message, new NSErrorException(error));
    }

    public static Dictionary<string, string> ToDictionary(this NSDictionary<NSString, NSString> nsDictionary) =>
        nsDictionary.ToDictionary<KeyValuePair<NSString, NSString>, string, string>
            (item => item.Key, item => item.Value);

    public static NSDictionary<NSString, NSString> ToNSDictionary(this Dictionary<string, string> dictionary) =>
        NSDictionary<NSString, NSString>.FromObjectsAndKeys(
            dictionary.Values.ToArray(),
            dictionary.Keys.ToArray());

    public static NSDictionary ToNSDictionary(this Dictionary<object, object> dictionary) =>
        NSDictionary.FromObjectsAndKeys(
            dictionary.Values.ToArray(),
            dictionary.Keys.ToArray());
}