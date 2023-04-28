// See https://aka.ms/new-console-template for more information
using ClienteApiOAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

Console.WriteLine("Request Api OAuth");
Console.WriteLine("---------------------------");
Console.WriteLine("Introduzca sus credenciales");
Console.WriteLine("User Name");
string user = Console.ReadLine();
Console.WriteLine("Password");
string pass = Console.ReadLine();
string response = await GetToken(user, pass);
Console.WriteLine(response);
Console.WriteLine("Realizando peticion");
Console.WriteLine("--------------");
string data = await GetEmpleadosAsync(response);
Console.WriteLine(data);

Console.WriteLine("Fin de Programa");


static async Task<string> GetToken(string user, string pass)
{
    string urlApi = "https://apioauthempleados2023xzx.azurewebsites.net";
    LoginModel model = new LoginModel { UserName = user, Password = pass };
    using(HttpClient client = new HttpClient())
    {
        string request = "/api/auth/login";
        client.BaseAddress = new Uri(urlApi);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add
            (new MediaTypeWithQualityHeaderValue("application/json"));

        string jsonModel = JsonConvert.SerializeObject(model);
        StringContent content = new StringContent(jsonModel
            , Encoding.UTF8, "application/json");
        HttpResponseMessage response =
            await client.PostAsync(request, content);

        if(response.IsSuccessStatusCode)
        {
            string data = await 
                response.Content.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(data);
            string token = jsonObject.GetValue("response").ToString();
            return token;
        }else
        {
            return "Petición incorrecta: " + response.StatusCode;

        }

    }

}


static async Task<string> GetEmpleadosAsync(string token)
{
    string urlApi = "https://apioauthempleados2023xzx.azurewebsites.net";
    using(HttpClient client = new HttpClient()) 
    {
        string request = "/api/empleados";
        client.BaseAddress = new Uri(urlApi);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add
            (new MediaTypeWithQualityHeaderValue("application/json"));

        client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
        HttpResponseMessage response =
            await client.GetAsync(request);

        if(response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }
        else
        {
            return "Algo ha ido mal ..." + response.StatusCode;
        }

    }
}

