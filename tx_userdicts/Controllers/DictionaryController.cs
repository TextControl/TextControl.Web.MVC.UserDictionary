using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using tx_userdicts.Models;

namespace tx_userdicts.Controllers
{
    public class DictionaryController : ApiController
    {
        // returns all user dictionaries in the specific folder
        [System.Web.Http.HttpGet]
        public HttpResponseMessage UserDictionaryFilenames()
        {
            var sFilenames = System.IO.Directory
                .EnumerateFiles(System.Web.Hosting.HostingEnvironment.MapPath("~/Dictionaries/"), "*.txd", System.IO.SearchOption.TopDirectoryOnly)
                .Select(System.IO.Path.GetFileName); 

            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<string[]>(sFilenames.ToArray(), new JsonMediaTypeFormatter())
            };
        }

        // loads a specific dictionary by filename
        [System.Web.Http.HttpGet]
        public HttpResponseMessage LoadUserDictionary(string filename)
        {
            UserDictionary userDictionary = new UserDictionary() {
                language = "en-US",
                name = filename
            };

            // read all lines from the dictionary
            string[] sDictionaryLines = System.IO.File.ReadAllLines(System.Web.Hosting.HostingEnvironment.MapPath("~/Dictionaries/" + filename));

            // skip the first entry (encoding)
            userDictionary.words = sDictionaryLines.Skip(1).ToArray();

            // return the object
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<UserDictionary>(userDictionary, new JsonMediaTypeFormatter())
            };
        }

        // saves a specific dictionary
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveUserDictionary([FromBody] UserDictionary dictionary)
        {
            // get all words and save the dictionary
            List<string> dictionaryLines = new List<string>();
            dictionaryLines.Add("SET iso-8859-1");
            dictionaryLines.AddRange(dictionary.words);

            System.IO.File.WriteAllLines(System.Web.Hosting.HostingEnvironment.MapPath("~/Dictionaries/" + dictionary.name), dictionaryLines);

            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<string>("UserDictionary " + dictionary.name + " successfully saved.", new JsonMediaTypeFormatter())
            };
        }
    }
}
