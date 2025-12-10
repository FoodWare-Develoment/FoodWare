namespace FoodWare.Mobile;

public static class HttpsClientHandlerService
{
    // Corrección S2325: Método marcado como static
    public static HttpMessageHandler GetPlatformMessageHandler()
    {
#if ANDROID
        var handler = new Xamarin.Android.Net.AndroidMessageHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            }
        };
        return handler;
#else
        return new HttpClientHandler();
#endif
    }
}